using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using cklib;
using cklib.Framework;
using cklib.Framework.IPC;
using cklib.Log;
using cklib.Log.Config;
using System.Reflection;
namespace cklib.Log
{
    /// <summary>
    /// ���O�G���W���ُ�ʒm�f���Q�[�g
    /// </summary>
    /// <param name="level">���x�� Fatal���G���W���͒�~����ʏ�Error</param>
    /// <param name="msg">�ُ팟�o���b�Z�[�W</param>
    /// <param name="exp">Exception���</param>
    public delegate void LogEngineError(LogLevel level,string msg,Exception exp);
    /// <summary>
    /// ���O�G���W���ُ�ʒm�f���Q�[�g2
    /// </summary>
    /// <param name="lex">���O�}�l�[�W���C���X�^���X</param>
    /// <param name="level">���x�� Fatal���G���W���͒�~����ʏ�Error</param>
    /// <param name="msg">�ُ팟�o���b�Z�[�W</param>
    /// <param name="exp">Exception���</param>
    public delegate void LogEngineError2(LogManagerEx lex,LogLevel level, string msg, Exception exp);
    /// <summary>
	/// ���M���O�G���W��
	/// </summary>
	public class LogingEngine : AppThreadBase<EventCode,object>
	{
		/// <summary>
		/// �C�x���g���O�C���X�^���X
		/// </summary>
		private	EventLog ELog					=	null;
		/// <summary>
		/// �t�@�C���o�͐�C���X�^���X
		/// </summary>
		private	System.IO.FileStream	FLog	=	null;
        /// <summary>
        /// Syslog�o�͐�C���X�^���X
        /// </summary>
        private System.Net.Sockets.UdpClient SysLog = null;
		/// <summary>
		/// ���O�t�@�C�����t
		/// </summary>
		private	DateTime	CurrentLogDate		=	DateTime.Today;
		/// <summary>
		/// �o�̓��b�Z�[�W
		/// </summary>
		private	string	LogMessage				=	string.Empty;
		private	bool	preWrite				=	false;
        /// <summary>
        /// �����ݒ���
        /// </summary>
        private ConfigInfo config = null;
        /// <summary>
        /// �����ݒ���
        /// </summary>
        public ConfigInfo Config
        {
            get { return config; }
        }
        /// <summary>
        /// ���O�G���W���G���[�C�x���g�C���X�^���X
        /// </summary>
        public LogEngineError LogEngineErrorEvent = null;
        /// <summary>
        /// ���O�G���W���G���[�C�x���g�C���X�^���X
        /// </summary>
        public LogEngineError2 LogEngineErrorEvent2 = null;
        /// <summary>
        /// ���O�}�l�[�W���C���X�^���X
        /// </summary>
        private LogManagerEx lex = null;
        /// <summary>
        /// ���O�}�l�[�W���C���X�^���X
        /// </summary>
        public LogManagerEx LogManagerEx
        {
            get
            {
                return this.lex;
            }
        }
        /// <summary>
        /// �t�@�C�����b�N�pMutex
        /// </summary>
        private ckMutex FileLockMutex = null;
        /// <summary>
		/// �R���X�g���N�^
		/// </summary>
        public LogingEngine(ConfigInfo Config,LogManagerEx lex)
            :base(EventCode.Start,EventCode.Stop,EventCode.Data,false)
		{
            this.lex = lex;
            this.config = Config;
		}
        /// <summary>
        /// ���\�[�X�̊J��
        /// </summary>
        protected override void ReleseResorce()
        {
            try
            {
                if (!this.fThreadMode)
                {
                    LogFlush();                  
                }
                System.Diagnostics.Debug.WriteLine("LogEngine Garbageing");
                lex.EngineGarbaged();
            }
            catch //(Exception exp)
            {
                //this.ExceptionPrint(exp);
            }
        }
        #region LOG�X���b�h����
        /// <summary>
        /// �}���`�X���b�h���[�h����
        /// </summary>
        private bool fThreadMode = false;
        /// <summary>
        /// ���O�G���W���X���b�h�N��
        /// </summary>
        /// <returns></returns>
        public override bool Start()
        {
            this.fThreadMode = true;
            return base.Start();
        }
		/// <summary>
		/// �C���X�^���X�̏�����
		/// </summary>
		/// <returns></returns>
		protected	override	bool	InitInstanse()
		{
			if	(base.InitInstanse())
			{
				thread.IsBackground	=	true;
                if (this.config.File.Enabled)
                {
                    if (this.config.File.MutexLockEnable)
                    {   //  MutexLock�L��
                        this.FileLockMutex = new ckMutex(this.config.File.MutexLock, false);
                    }
                }
                this.LogExtendStart();
                this.LogRotation();
                return true;
			}
			return	false;
		}
		/// <summary>
		/// �X���b�h�̏I������
		/// </summary>
		/// <returns></returns>
		protected	override bool	ExitInstance()
		{
            LogFlush();
            this.LogExtendTerminate();
            return base.ExitInstance();
		}
		/// <summary>
		/// �C�x���g�����㏈��
		/// </summary>
		/// <returns></returns>
		protected	override	bool	AfterIdle()
		{
			return	true;
		}
		/// <summary>
		/// �ҋ@�^�C���A�E�g���Ԃ��~���b�ŕԂ�
		/// </summary>
		/// <returns></returns>
		protected	override	int	GetWaitTime()
		{
            int wTime = System.Threading.Timeout.Infinite;
            if (preWrite)
            {
                wTime = this.config.Common.FlushDelay;
            }

            //  ���ւ��܂ł̎���
            //TimeSpan sp = DateTime.Today.AddDays(1).Subtract(DateTime.Now);   //  2010/01/28  �폜�@Today��Now�̊Ԃɓ��ׂ������ꍇ�ɕ����ƂȂ�
            DateTime now = DateTime.Now;                                        //  2010/01/28  �ǉ�
            TimeSpan sp = now.Date.AddDays(1).Subtract(now);                    //  2010/01/28  �ǉ�
            int spms;
            if (sp.TotalMilliseconds > int.MaxValue)
                spms = int.MaxValue;
            else
                spms = Convert.ToInt32(sp.TotalMilliseconds);
            if (wTime == System.Threading.Timeout.Infinite)
                wTime = spms;
            else
                wTime = (spms > wTime ? wTime : spms);
            return wTime;
		}
		/// <summary>
		/// �C�x���g�����^�C���A�E�g���̏���
		/// </summary>
		/// <returns></returns>
		protected	override	bool	EventTimeout()
		{
			preWrite	=	false;
            LogFlush();
            if (this.CurrentLogDate != DateTime.Today.Date)
            {	//	���t���s��v
                //  ���O���[�e�[�V�������s��
                this.LogRotation();
            }
            return true;
		}
        #region ��O�����̃I�[�o�[���C�h
        /// <summary>
        /// �����G���[�G���[�o��
        /// </summary>
        /// <param name="level">�G���[���x��</param>
        /// <param name="msg">�G���[���b�Z�[�W</param>
        /// <param name="exp">��O</param>
        public void ExceptionPrint(LogLevel level, string msg, Exception exp)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}:{2}\r\n{3}", this.lex.ManagerKey, level.ToString(), msg, (exp != null ? exp.ToString() : string.Empty)));
            //System.Console.Error.WriteLine(msg);
            if (this.LogEngineErrorEvent2 != null)
            {
                this.LogEngineErrorEvent2.BeginInvoke(lex, level, string.Format("{0}:{1}", this.lex.ManagerKey, msg), exp, new AsyncCallback(LogEngineErrorEventCallback2), LogEngineErrorEvent2);  //  2014/02/19 BugFix
            }
            else
                if (this.LogEngineErrorEvent != null)
                {
                    this.LogEngineErrorEvent.BeginInvoke(level, string.Format("{0}:{1}", this.lex.ManagerKey, msg), exp, new AsyncCallback(LogEngineErrorEventCallback), LogEngineErrorEvent);
                }
        }
        /// <summary>
        /// �񓯊��f���Q�[�g���ʂ��󂯎��
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void LogEngineErrorEventCallback(IAsyncResult ar)
        {
            LogEngineError handler = ar.AsyncState as LogEngineError;
            handler.EndInvoke(ar);
        }
        /// <summary>
        /// �񓯊��f���Q�[�g���ʂ��󂯎��
        /// </summary>
        /// <remarks>
        /// �ǉ�  2014/02/19 BugFix
        /// </remarks>
        /// <param name="ar"></param>
        protected virtual void LogEngineErrorEventCallback2(IAsyncResult ar)
        {
            LogEngineError2 handler = ar.AsyncState as LogEngineError2;
            handler.EndInvoke(ar);
        }
        /// <summary>
        /// �ҋ@���̗�O�������̏���
        /// </summary>
        /// <returns></returns>
        protected override int WaitError(Exception exp)
        {
            this.ExceptionPrint(LogLevel.FATAL,"Waiting Error",exp);
            return -1;
        }
        /// <summary>
        /// �C�x���g�������̗�O����������
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool EventError(Exception exp)
        {
            this.ExceptionPrint(LogLevel.FATAL, "Event Error", exp);
            return false;
        }
        /// <summary>
        /// catch����Ă��Ȃ���O����������
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override void ThreadError(Exception exp)
        {
            this.ExceptionPrint(LogLevel.FATAL, "No Catched Exception", exp);
        }
        #endregion

		/// <summary>
		/// �f�[�^�C�x���g
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	override	bool	EventData(EventDataTypeBase<EventCode,object> ed)
		{
			LogData	data	=	(LogData)ed.EventData;
            return LogWrite(data);
        }

        #endregion
        #region ���O�������ݏ���
        /// <summary>
        /// ���O���t�H�A�O���E���h�������ݏ���
        /// </summary>
        /// <param name="data">���O�f�[�^</param>
        /// <returns></returns>
        public bool LogForeWrite(LogData data)
        {
            try
            {
                this.LogWrite(data);
                this.LogFlush();
                return true;
            }
            catch (Exception exp)
            {
                this.EventError(exp);
                return false;
            }
        }
        /// <summary>
        /// ���O��������
        /// </summary>
        /// <param name="data">���O�f�[�^</param>
        /// <returns></returns>
        public bool LogWrite(LogData data)
        {
			if	(!(this.Config.Console.Enabled||this.Config.File.Enabled||this.Config.EventLog.Enabled||this.Config.Extend.Enabled||this.Config.Syslog.Enabled))
			{	//	�o�͐悪����
				return	true;
			}
			LogMessage	=	string.Empty;
			LogConsole(data);
			LogEventLog(data);
			LogFile(data);
			LogExtend(data);
            LogSyslog(data);
            preWrite = true;
			return	true;
        }
        /// <summary>
        /// ���O�̏o�͂��m�肳����
        /// </summary>
        /// <returns></returns>
        public bool LogFlush()
        {
            CloseLogFile();
            CloseEventLog();
            CloseSyslog();
            return true;
        }
        #endregion
        #region �R���\�[��
        /// <summary>
		/// �R���\�[���o��
		/// </summary>
		/// <param name="data">���b�Z�[�W�\�[�X</param>
		/// <returns></returns>
		public	bool	LogConsole(LogData	data)
		{
			if	(!this.Config.Console.Enabled)
				return	false;
			if	(this.Config.Console.Level>data.level)
				return	false;
			Console.WriteLine(this.GenerateMessageToString(data,this.Config.Console));
			return	true;
        }
        #endregion
        #region ���O�t�@�C��
        /// <summary>
		/// ���O�t�@�C���o��
		/// </summary>
		/// <param name="data">���b�Z�[�W�\�[�X</param>
		/// <returns></returns>
		public	bool	LogFile(LogData	data)
		{
			if	(!this.Config.File.Enabled)
				return	false;
			if	(this.Config.File.Level>data.level)
				return	false;
            bool mtxlocked = false;
            try
            {
                if (this.config.File.MutexLockEnable)
                {
                    try
                    {
                        if (this.FileLockMutex.Lock(this.config.File.MutexLockWaitTime < 0 ? System.Threading.Timeout.Infinite : this.config.File.MutexLockWaitTime))
                            mtxlocked = true;
                        else
                            throw new Exception("Mutex Timeout");
                    }
                    catch (System.Threading.AbandonedMutexException)
                    {
                        mtxlocked = true;
                    }
                    catch (Exception exp)
                    {
                        this.ExceptionPrint(LogLevel.ERROR, "Log File Write Exclusive Mutex Error", exp);
                        return true;
                    }
                }
                if (OpenLogFile(data))
                {
                    bool fOpened = true;
                    try
                    {
                        byte[] log = GenerateMessage(data, this.Config.File);
                        if (Config.File.RotateSize > 0)
                        {   //  �t�@�C���T�C�Y���[�e�[�g�L
                            if ((this.FLog.Length + log.Length) > Config.File.RotateSize)
                            {   //  ���[�e�[�V�����v
                                try
                                {
                                    CloseLogFile();
                                    this.Config.FileRotaterInstance.SizeRotate(Config.File, data.Time);
                                }
                                catch (Exception exp)
                                {
                                    this.ExceptionPrint(LogLevel.ERROR, "File Log Rotate Error", exp);
                                }
                                fOpened = OpenLogFile(data);
                            }
                        }
                        if (fOpened)
                            FLog.Write(log, 0, log.Length);
                    }
                    catch (Exception exp)
                    {
                        this.ExceptionPrint(LogLevel.ERROR, "File Log Error", exp);
                        CloseLogFile();
                    }
                }
                if (this.Config.File.FileLock || this.Config.File.MutexLockEnable)
                {   //  �r�����[�h�̏ꍇ�ꌏ�Ât�@�C�������
                    this.CloseLogFile();
                }
            }
            finally
            {
                if (mtxlocked)
                {
                    try
                    {
                        this.FileLockMutex.UnLock();
                    }
                    catch (Exception exp)
                    {
                        this.ExceptionPrint(LogLevel.ERROR, "Log File Write Exclusive Mutex Error", exp);
                    }
                }
            }
			return	true;
		}
		/// <summary>
		/// ���O�t�@�C�����J��
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		protected	bool	OpenLogFile(LogData	data)
		{
            if (!this.CreateLogFileDirectory())
            {
	             return false;
            }
            if (this.CurrentLogDate != data.Time.Date)
            {	//	���t���s��v
                if (this.FLog != null)
			    {
					CloseLogFile();
				}
                this.LogRotation();
			}
			if	(this.FLog==null)
			{
				CurrentLogDate	=	data.Time.Date;
                string fileName = this.Config.File.Path + String.Format(this.Config.File.FileName, CurrentLogDate);
                if (this.Config.File.FileLock)
                {
                    for (int count = 0; ; count++)
                    {
                        try
                        {
                            this.FLog = new System.IO.FileStream(fileName,
                                                                        System.IO.FileMode.Append,
                                                                        System.IO.FileAccess.Write,
                                                                        System.IO.FileShare.Read);
                            break;
                        }
                        catch (IOException exp)
                        {
                            //  WinError.h���
                            //  #define FACILITY_WIN32                   7
                            //  #define __HRESULT_FROM_WIN32(x) ((HRESULT)(x) <= 0 ? ((HRESULT)(x)) : ((HRESULT) (((x) & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000)))
                            //  #define ERROR_SHARING_VIOLATION          32L    ��  0x80070020
                            //  #define ERROR_LOCK_VIOLATION             33L    ��  0x80070021
                            uint hr = cklib.Util.Errors.GetHResultFromIOException(exp);
                            if ((hr == 0x80070020) || (hr == 0x80070021))
                            {   //  �r���G���[
                                
                                if (config.File.FileLockTryLimit == -1 || count < config.File.FileLockTryLimit)
                                {
                                    System.Threading.Thread.Sleep(this.Config.File.FileLockWaitTime);
                                    continue;
                                }
                                this.ExceptionPrint(LogLevel.ERROR, "File Lock Try Over", exp);
                                this.FLog = null;
                                return false;
                            }
                            this.ExceptionPrint(LogLevel.ERROR, "File Log Open Error", exp);
                            this.FLog = null;
                            return false;
                        }
                        catch (Exception exp)
                        {
                            this.ExceptionPrint(LogLevel.ERROR, "File Log Open Error", exp);
                            this.FLog = null;
                            return false;
                        }
                    }
                }
                else
                {
                    try
                    {
                        this.FLog = new System.IO.FileStream(fileName,
                                                                    System.IO.FileMode.Append,
                                                                    System.IO.FileAccess.Write,
                                                                    System.IO.FileShare.ReadWrite);
                    }
                    catch (Exception exp)
                    {
                        this.ExceptionPrint(LogLevel.ERROR, "File Log Open Error", exp);
                        this.FLog = null;
                        return false;
                    }
                }
                if (Config.File.Compress)
                {
                    try
                    {
                        if (Util.File.IsSupportCompressFile(Util.File.ParsePathToDrive(Config.File.Path)))
                        {
                            System.IO.FileInfo finfo = new FileInfo(fileName);
                            if ((finfo.Attributes & FileAttributes.Compressed) != FileAttributes.Compressed)
                            {
                                this.CloseLogFile();
                                for (int count = 0; ; count++)
                                {
                                    if (Util.File.FileCompress(finfo.FullName))
                                    {
                                        return OpenLogFile(data);
                                    }
                                    else
                                    {
                                        var ec = Util.File.GetLastError();
                                        if ((ec == 0x00000020) || (ec == 0x00000021))
                                        {   //  �r������G���[
                                            if (config.File.FileLockTryLimit == -1 || count < config.File.FileLockTryLimit)
                                            {
                                                System.Threading.Thread.Sleep(this.Config.File.FileLockWaitTime);
                                                continue;
                                            }
                                            throw new cklib.Util.Win32ErrorException(ec,"File Lock Try Over");
                                        }
                                        throw new cklib.Util.Win32ErrorException(ec, "File Log Open");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        this.ExceptionPrint(LogLevel.ERROR, "File Log Open Error", exp);
                        this.CloseLogFile();
                        return false;
                    }
                }
			}
			return	true;
		}
		/// <summary>
		/// ���O�t�@�C���̃N���[�Y
		/// </summary>
		/// <returns></returns>
		protected	bool	CloseLogFile()
		{
			try
			{
				if	(this.FLog!=null)
				{
					this.FLog.Close();
					this.FLog	=	null;
				}
			}
			catch	//	(Exception	exp)
			{
				this.FLog	=	null;
				return	false;
			}
			return	true;
		}
        /// <summary>
        /// ���O�t�@�C���f�B���N�g��
        /// </summary>
        /// <returns></returns>
        protected bool CreateLogFileDirectory()
        {
            if (!Directory.Exists(this.Config.File.Path))
            {   //  ���݂��Ȃ��ꍇ�f�B���N�g�����쐬����
                try
                {
                    Directory.CreateDirectory(this.Config.File.Path);
                }
                catch   (Exception exp)
                {
                    this.ExceptionPrint(LogLevel.ERROR, "File Log Directory Create Error", exp);
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// ���O���[�e�[�V����
        /// </summary>
        /// <returns></returns>
        public bool LogRotation()
        {
            if (!this.CreateLogFileDirectory())
            {
	             return false;
            }
            CloseLogFile();
            return this.Config.FileRotaterInstance.Rotate(Config.File);
        }
        #endregion
        #region EventLog
        /// <summary>
        /// �C�x���g���O�o��
		/// </summary>
		/// <param name="Data">���b�Z�[�W�\�[�X</param>
		/// <returns></returns>
		public	bool	LogEventLog(LogData	Data)
		{
			if	(!this.Config.EventLog.Enabled)
				return	false;
			if	(this.Config.EventLog.Level>Data.level)
				return	false;
			if	(OpenEventLog())
			{
                try
                {
                    var msg = this.GenerateMessageToString(Data, this.Config.EventLog);
                    if (msg.Length > this.Config.EventLog.MessageMaxLength)
                        msg = msg.Substring(0, this.Config.EventLog.MessageMaxLength);  //  2016/08/23 �C�x���g���O�����`�F�b�N�ǉ�
                    ELog.WriteEntry(msg, this.Config.EventLog.LogLevelToEventLogType(Data.level), Data.Code);
                }
                catch (Exception exp)
                {
                    this.ExceptionPrint(LogLevel.ERROR, "EventLog Write Failed", exp);
                    CloseEventLog();
                }
#pragma warning disable 1058
                catch
                {
                    this.ExceptionPrint(LogLevel.ERROR, "EventLog Write Failed", null);
                    CloseEventLog();
                }
#pragma warning restore 1058
            }
			return	true;
		}
		/// <summary>
		/// �C�x���g���O���J��
		/// </summary>
		/// <returns></returns>
		private	bool	OpenEventLog()
		{
			if	(ELog==null)
			{
				try
				{
					ELog	=	new	EventLog();
					ELog.Source	=	this.Config.EventLog.EventSource;
				}
			    catch	(Exception	exp)
			    {
                    this.ExceptionPrint(LogLevel.ERROR, "EventLog Open Failed", exp);
                    CloseEventLog();
                    return false;
                }
#pragma warning disable 1058
                catch
				{
                    this.ExceptionPrint(LogLevel.ERROR, "EventLog Open Failed",null);
                    CloseEventLog();
					return	false;
				}
#pragma warning restore 1058
            }
			return	true;
		}
		/// <summary>
		/// �C�x���g���O�����
		/// </summary>
		/// <returns></returns>
		private	bool	CloseEventLog()
		{
			try
			{
				if	(ELog!=null)
				{
					ELog.Close();
					ELog.Dispose();
				}
			}
			catch
			{}
            ELog = null;
			return	true;
        }
        #endregion
        #region syslog
        /// <summary>
        /// syslog�o��
		/// </summary>
		/// <param name="data">���b�Z�[�W�\�[�X</param>
		/// <returns></returns>
		public	bool	LogSyslog(LogData	data)
		{
			if	(!this.Config.Syslog.Enabled)
				return	false;
			if	(this.Config.Syslog.Level>data.level)
				return	false;
            if (OpenSyslog())
			{
				try
				{
                    int Severity;
                    switch (data.level)
                    {
                        case LogLevel.DEBUG:    Severity = 7;   break;
                        case LogLevel.INFO:     Severity = 6;   break;
                        case LogLevel.NOTE:     Severity = 5; break;
                        case LogLevel.WARN:     Severity = 4; break;
                        case LogLevel.ERROR:    Severity = 3; break;
                        case LogLevel.CRIT: Severity = 2; break;
                        case LogLevel.ALERT:    Severity = 1; break;
                        case LogLevel.EMERG:Severity = 0; break;
                        case LogLevel.FATAL:    Severity = 0; break;
                        case LogLevel.Undefine:
                        default:    Severity = 0; break;
                    }
                    string[]    MonthStr	=	{	"","Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"	};

                    Severity += this.Config.Syslog.Facility * 8;
                    string header=string.Format("<{0}>{1,3}{2,3} {3} {4} ",Severity,MonthStr[data.Time.Month],data.Time.Day,data.Time.ToString("HH:mm:ss"),this.Config.Syslog.OwnHostname);
                    byte[] hb = Encoding.ASCII.GetBytes(header);
                    string[] msgs = this.GenerateMessageToString(data,this.Config.Syslog).Replace("\r","").Split("\n".ToCharArray());
                    foreach (string msg in msgs)
                    {
                        byte[] log = this.Config.Syslog.Encoding.GetBytes(msg);
                        int OnePacketMsg = 1024 - hb.Length;
                        for (int i = 0; i<log.Length;)
                        {
                            byte[] packet;
                            if ((log.Length - i) > OnePacketMsg)
                            {
                                packet = new byte[1024];
                                Array.Copy(hb, 0, packet, 0, hb.Length);
                                Array.Copy(log, i, packet, hb.Length, OnePacketMsg);
                                i += OnePacketMsg;
                            }
                            else
                            {
                                packet = new byte[hb.Length+(log.Length-i)];
                                Array.Copy(hb, 0, packet, 0, hb.Length);
                                Array.Copy(log, i, packet, hb.Length, log.Length-i);
                                i += log.Length - i;
                            }
                            this.SysLog.Send(packet, packet.Length);
                        }
                    }
				}
				catch	(Exception	exp)
				{
                    this.ExceptionPrint(LogLevel.ERROR, "Syslog Error", exp);
                    CloseSyslog();
				}
			}
			return	true;
		}
		/// <summary>
		/// syslog�J��
		/// </summary>
		/// <returns></returns>
		protected	bool	OpenSyslog()
		{
			if	(this.SysLog==null)
			{
				try
				{
                    this.SysLog = new System.Net.Sockets.UdpClient(this.Config.Syslog.Host,this.Config.Syslog.Port);
				}
				catch	(Exception	exp)
				{
                    this.ExceptionPrint(LogLevel.ERROR, "Syslog Open Error", exp);
                    this.SysLog = null;
					return	false;
				}
			}
			return	true;
		}
		/// <summary>
		/// syslog�N���[�Y
		/// </summary>
		/// <returns></returns>
		protected	bool	CloseSyslog()
		{
			try
			{
				if	(this.SysLog!=null)
				{
					this.SysLog.Close();
					this.SysLog =	null;
				}
			}
			catch	//	(Exception	exp)
			{
				this.SysLog=	null;
				return	false;
			}
			return	true;
        }
        #endregion
        #region �g�����O
        /// <summary>
        /// �g�����O�J�n
        /// </summary>
        /// <returns></returns>
        public void LogExtendStart()
        {
            if (!this.Config.Extend.Enabled)
                return;
            if (this.Config.ExtendLogInstance != null)
            {
                try
                {
                    this.Config.ExtendLogInstance.engine = this;
                    this.Config.ExtendLogInstance.Start(this.Config);
                }
                catch (Exception exp)
                {
                    this.ExceptionPrint(LogLevel.FATAL, "ExtendLog Start Failed", exp);
                }
#pragma warning disable 1058
                catch
                {
                    this.ExceptionPrint(LogLevel.FATAL, "ExtendLog Start Failed", null);
                }
#pragma warning restore 1058
            }
        }
        /// <summary>
        /// �g�����O�J�n
        /// </summary>
        /// <returns></returns>
        public void LogExtendTerminate()
        {
            if (!this.Config.Extend.Enabled)
                return;
            if (this.Config.ExtendLogInstance != null)
            {
                try
                {
                    this.Config.ExtendLogInstance.Terminate(this.Config);
                }
                catch
                {}
            }
        }
        /// <summary>
        /// �A�C�h�����t���b�V������
        /// </summary>
        public void LogExtendFlush()
        {
            if (!this.Config.Extend.Enabled)
                return;
            if (this.Config.ExtendLogInstance != null)
            {
                try
                {
                    this.Config.ExtendLogInstance.LogFlush(this.Config);
                }
                catch
                { }
            }
        }
        /// <summary>
		/// �g�����O�o��
		/// </summary>
		/// <param name="Data">���b�Z�[�W�\�[�X</param>
		/// <returns></returns>
		public	bool	LogExtend(LogData	Data)
		{
			if	(!this.Config.Extend.Enabled)
				return	false;
			if	(this.Config.Extend.Level>Data.level)
				return	false;
			if	(this.Config.ExtendLogInstance!=null)
			{
				try
				{
                    this.Config.ExtendLogInstance.Loging(this.GenerateMessageToString(Data, this.Config.Extend),Data,this.Config);
				}
                catch (Exception exp)
                {
                    this.ExceptionPrint(LogLevel.ERROR, "ExtendLog Failed", exp);
                }
#pragma warning disable 1058
                catch
                {
                    this.ExceptionPrint(LogLevel.ERROR, "ExtendLog Failed", null);
                }
#pragma warning restore 1058
            }
			return	true;
        }
        #endregion
        #region ���b�Z�[�W����
        /// <summary>
        /// ���b�Z�[�W�̐���(�G���R�[�h�ς�byte�z��ŏo��)
        /// </summary>
        /// <param name="Data">���O�f�[�^</param>
        /// <param name="conf">�ݒ�f�[�^</param>
        /// <returns>��������f�[�^</returns>
		public	byte[]	GenerateMessage(LogData	Data,BasicLogConfig conf)
		{
            byte[] msg = this.Config.FormatterInstance.Format(Data, conf, this);
            if (conf.Scramble)
            {   //  �Í���
                if (this.Config.ScramblerInstance != null)
                {
                    return this.Config.ScramblerInstance.Encrypt(msg, conf, this);
                }
            }
            return msg;
        }
        /// <summary>
        /// ���b�Z�[�W�̐���
        /// </summary>
        /// <param name="Data">���O�f�[�^</param>
        /// <param name="conf">�ݒ�f�[�^</param>
        /// <returns>��������f�[�^</returns>
        public string GenerateMessageToString(LogData Data, BasicLogConfig conf)
        {
            if (conf.Scramble)
            {   //  �Í���
                if (this.Config.ScramblerInstance != null)
                {
                    byte[] msg = this.Config.FormatterInstance.Format(Data, conf, this);
                    return this.Config.ScramblerInstance.EncryptToString(msg, conf, this);
                }
            }
            return this.Config.FormatterInstance.FormatString(Data, conf, this);
        }
        #endregion
    }
}
