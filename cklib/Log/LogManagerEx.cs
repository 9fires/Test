using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using cklib;
using cklib.Framework;
using cklib.Log.Config;
namespace cklib.Log
{
	/// <summary>
	/// ���O�@�\���Ǘ�����
	/// </summary>
    public class LogManagerEx : IDisposable
	{
        /// <summary>
        /// �������Q�Ɛ�
        /// </summary>
        private bool fInitialize = false;
        /// <summary>
        /// ��������ԃt���O
        /// </summary>
        public bool IsInitialized
        {
            get { return    fInitialize;}
        }
        /// <summary>
		/// �����ݒ���
		/// </summary>
		private	ConfigInfo	config=null;
		/// <summary>
		/// �����ݒ���
		/// </summary>
		public	ConfigInfo	Config
		{
			get	{	return	config;	}
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
        /// �f�t�H���g���O�R�[�h
        /// </summary>
        /// <remarks>
        /// �f�o�b�O���O�y�у��O�R�[�h�Ȃ��̃��O�Ɏg�p����郍�O�R�[�h<br/>
        /// �C�x���g���O���g�p���Ă���ꍇ�́A�R�[�h���蓖�Ă��K�{�Ȃ̂œK�؂ȃR�[�h�����蓖�Ă�K�v������<br/>
        /// </remarks>
        public int DefaultLogCode = 0;
		/// <summary>
		/// ���M���O�����G���W�������O�}�l�[�W�����ɕێ�����
		/// </summary>
        private static System.Collections.Generic.Dictionary<string, LogingEngine> Engines = new System.Collections.Generic.Dictionary<string, LogingEngine>();
        /// <summary>
        /// ���M���O�����G���W�������O�}�l�[�W�����ɕێ�����
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, LogManagerEx> Managers = new System.Collections.Generic.Dictionary<string, LogManagerEx>();
        /// <summary>
        /// �w�肳��L�[�̃}�l�W���[�N���X�C���X�^���X���擾����
        /// </summary>
        /// <param name="key">�L�[������</param>
        /// <returns></returns>
        public static   LogManagerEx LookupLogManagerEx(string key)
        {
            lock (Managers)
            {
                if (Managers.ContainsKey(key))
                {
                    return Managers[key];
                }
            }
            return null;
        }
        /// <summary>
        /// �f�t�H���g�̃}�l�[�W���[�L�[
        /// </summary>
        public static readonly string DefaultManagerKey = ConfigInfo.DefaultConfigSectionName;
        /// <summary>
        /// �C���X�^���X�����ʂ���L�[���
        /// </summary>
        public readonly string ManagerKey;
        #region �R���X�g���N�^�E�f�X�g���N�^
        /// <summary>
        ///	�R���X�g���N�^
        /// </summary>
        public LogManagerEx()
        {
            lock (Managers)
            {
                this.ManagerKey = DefaultManagerKey;
                System.Diagnostics.Debug.WriteLine("LogManagerEx Construct Key=" + this.ManagerKey);
                Managers.Add(this.ManagerKey, this);
            }
        }
        /// <summary>
		///	�R���X�g���N�^
		/// </summary>
		public LogManagerEx(string ManagerKey)
		{
            lock (Managers)
            {
                this.ManagerKey = ManagerKey;
                System.Diagnostics.Debug.WriteLine("LogManagerEx Construct Key=" + this.ManagerKey);
                Managers.Add(this.ManagerKey, this);
            }
		}
        /// <summary>
		/// �f�B�X�g���N�^
		/// </summary>
        ~LogManagerEx()
		{
			Dispose(false);
        }
        #endregion �R���X�g���N�^�E�f�X�g���N�^
        #region IDisposable �����o
        /// <summary>
        /// Dispose�����t���O
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Dispose���\�b�h
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose�����̎���
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                this.Terminate();
                lock (Managers)
                {
                    if (this.ManagerKey!=null)
                    {
                        if (Managers.ContainsKey(this.ManagerKey))
                        {
                            Managers.Remove(this.ManagerKey);
                        } 
                    }
                }
                if (disposing)
                {
                    ReleseManagedResorce();
                }
                ReleseResorce();
            }
            disposed = true;
        }
        /// <summary>
        /// ���\�[�X�������
        /// </summary>
        protected virtual void ReleseResorce()
        {
        }
        /// <summary>
        /// �}�l�[�W�h���\�[�X��������i�����I�Ăяo�����̂ݎ��s�����j
        /// </summary>
        protected virtual void ReleseManagedResorce()
        {
        }

        #endregion
        #region ���������\�b�h
        /// <summary>
        /// �������A�v���P�[�V�����ݒ�����[�h����
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            lock (this)
            {
                if (IsInitialized)
                {
                    return false;
                }
                this.config = new ConfigInfo(this.ManagerKey);
                return this.InitializeSub();
            }
        }
        /// <summary>
        /// �������A�v���P�[�V�����ݒ�����[�h����
        /// </summary>
        /// <param name="conf">�ݒ���C���X�^���X</param>
        /// <returns></returns>
        public bool Initialize(ConfigInfo conf)
        {
            lock (this)
            {
                if (IsInitialized)
                {
                    return false;
                }
                config = conf;
                return this.InitializeSub();
            }
        }
        /// <summary>
        /// �������T�u����
        /// </summary>
        /// <returns></returns>
        private bool InitializeSub()
        {
            //	�C�x���g���O�̏�����
            if (config.EventLog.Enabled)
            {
                if (!System.Diagnostics.EventLog.SourceExists(config.EventLog.EventSource))
                {	//	�C�x���g���O�\�[�X�̐���
                    System.Diagnostics.EventLog.CreateEventSource(config.EventLog.EventSource, config.EventLog.EventLogName);
                }
            }
            lock (this)
            {
                lock (LogManagerEx.Engines)
                {
                    LogManagerEx.Engines.Add(ManagerKey, new LogingEngine(this.config, this));
                    System.Diagnostics.Debug.WriteLine("LogManagerEx Initialize Key=" + this.ManagerKey);
                    if (this.LogEngineErrorEvent2 != null)
                    {
                        LogManagerEx.Engines[ManagerKey].LogEngineErrorEvent2 = this.LogEngineErrorEvent2;
                    }
                    else
                    if (this.LogEngineErrorEvent != null)
                    {
                        LogManagerEx.Engines[ManagerKey].LogEngineErrorEvent = this.LogEngineErrorEvent;
                    }
                    fInitialize = true;
                    if (config.Common.BackGround)
                    {   //  �o�b�N�O���E���h���[�h���ɃX���b�h���N������
                        LogManagerEx.Engines[ManagerKey].IPCQueueMaxSize = config.Common.QueueingSize;
                        LogManagerEx.Engines[ManagerKey].Start();
                    }
                    else
                    {   //  �N�����̃��O���[�e�[�g��������̂őΏ�
                        try
                        {
                            LogManagerEx.Engines[ManagerKey].LogExtendStart();
                            LogManagerEx.Engines[ManagerKey].LogRotation();
                        }
                        catch (Exception exp)
                        {
                            //  ���O�G���W����O�n���h�����ݒ肳��Ă���\��������̂ŌĂяo���B
                            LogManagerEx.Engines[ManagerKey].ExceptionPrint(LogLevel.ERROR, "Rotate Failed", exp);
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// �ċN��
        /// </summary>
        /// <returns>����������</returns>
        public bool ReStart()
        {
            lock (this)
            {
                this.Terminate();
                return Initialize();
            }
        }
        /// <summary>
        /// �ċN��
        /// </summary>
        /// <param name="conf">�ݒ���C���X�^���X</param>
        /// <returns></returns>
        public bool ReStart(ConfigInfo conf)
        {
            lock (this)
            {
                this.Terminate();
                return Initialize(conf);
            }
        }
        #endregion ���������\�b�h
        /// <summary>
        /// ���O�G���W���̃K�x�[�W
        /// </summary>
        /// <remarks>
        /// Web���A�v���P�[�V�����̐���Ɩ��֌W�Ƀ��O�G���W�����K�x�[�W����Ă��܂����ꍇ�Ƀ��O�G���W�����Ăяo�����܂����킹��
        /// </remarks>
        internal void EngineGarbaged()
        {
            lock (this)
            {
                if (this.IsInitialized)
                {
                    this.fInitialize = false;
                    lock (LogManagerEx.Engines)
                    {
                        if (LogManagerEx.Engines.ContainsKey(ManagerKey))
                        {
                            LogManagerEx.Engines.Remove(ManagerKey);
                            System.Diagnostics.Debug.WriteLine("LogManagerEx Garbaged Key=" + this.ManagerKey);
                        }
                    }
                }
            }
        }

        /// <summary>
		/// ���O�G���W���̒�~
		/// </summary>
        public void Terminate()
        {
            lock (this)
            {
                if (this.fInitialize)
                {
                    this.fInitialize = false;
                    LogingEngine Engine = null;
                    lock (LogManagerEx.Engines)
                    {
                        if (LogManagerEx.Engines.ContainsKey(ManagerKey))
                        {
                            Engine = LogManagerEx.Engines[ManagerKey];
                            System.Diagnostics.Debug.WriteLine("LogManagerEx Terminate Key=" + this.ManagerKey);
                            LogManagerEx.Engines.Remove(ManagerKey);
                        }
                    }
                    if (Engine != null)
                    {
                        //	���O�G���W���̒�~
                        if (Config.Common.BackGround)
                        {
                            Engine.Stop();
                            Engine.Dispose();
                        }
                        else
                        {
                            Engine.LogFlush();
                            Engine.Dispose();
                        }
                    }
                }
            }
        }
		/// <summary>
		/// ���O�̏�������
		/// </summary>
		/// <param name="data">���O�f�[�^</param>
		/// <returns></returns>
		public	bool	LogStore(LogData	data)
		{
            lock (this)
            {
                if (IsInitialized)
                {
                    LogingEngine Engine = null;
                    data.Code += config.Common.LogCodeOffset;
                    lock (LogManagerEx.Engines)
                    {
                        if (LogManagerEx.Engines.ContainsKey(ManagerKey))
                        {
                            Engine = LogManagerEx.Engines[ManagerKey];
                        }
                    }
                    if (Engine != null)
                    {
                        if (Config.Common.BackGround)
                        {   //  �o�b�N�O���E���h���[�h
                            if (Engine.IsAlive)
                                return Engine.IPCPut(EventCode.Data, data);
                            else
                                Terminate();
                        }
                        else
                        {
                            Engine.LogForeWrite(data);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("LogManagerEx Drop Log Key=" + this.ManagerKey + " Msg=" + data.Message);
                    }
                }
            }
            return true;
		}
	}
}
