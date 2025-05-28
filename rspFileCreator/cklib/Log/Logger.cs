using System;
using System.Diagnostics;
using System.Reflection;
using cklib;
using System.Text;
#if __net20__
using cklib.Framework;
#endif
namespace cklib.Log
{
	/// <summary>
	/// ���O�������݃C���X�^���X
	/// </summary>
    [Serializable]
	public class Logger
    {
        #region �t�B�[���h
        /// <summary>
        /// ���O�}�l�[�W���C���X�^���X
        /// </summary>
        public readonly string MngKey = null;
        /// <summary>
		/// �\�[�X��
		/// </summary>
		internal string	SourceName=string.Empty;
        /// <summary>
        /// �R�[���X�^�b�N�����o���l�X�g
        /// </summary>
        protected int CallStackDepth=2;
        #endregion
        #region �R���X�g���N�^
        /// <summary>
		/// �R���X�g���N�^
		/// </summary>
        public Logger()
		{
            this.MngKey = LogManagerEx.DefaultManagerKey;
            SetSourceName();
        }
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public Logger(System.Type type)
		{
            this.MngKey = LogManagerEx.DefaultManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public Logger(System.Type type,string ManagerKey)
        {
            this.MngKey = ManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public Logger(System.Type type, LogManagerEx mng)
        {
            this.MngKey = mng.ManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public Logger(string ManagerKey)
        {
            this.MngKey = ManagerKey;
            SetSourceName();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public Logger(LogManagerEx mng)
        {
            this.MngKey = mng.ManagerKey;
            SetSourceName();
        }
        /// <summary>
        /// �X�^�b�N�t���[������Ăяo�����̃\�[�X���𒊏o����
        /// </summary>
        private void SetSourceName()
        {
            try
            {
                StackFrame CallStack = new StackFrame(2, false);
                SourceName = CallStack.GetMethod().DeclaringType.ToString();
            }
            catch
            { }
        }
        #endregion
        #region ���x���w�薳��
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Write(string message)
        {
            InnerLog(LogLevel.Undefine, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Write(string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Write(int LogCode, string message)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, null);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Write(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, exp);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WriteFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WriteFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, message, prms);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void WriteMessage(int LogCode)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void WriteMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WriteFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Write(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, null);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Write(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, exp);
        }

        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WriteFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, message, prms);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void WriteMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void WriteMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Write���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WriteFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region TRACE
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Trace(string message)
        {
            InnerLog(LogLevel.TRACE, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Trace(string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Trace(int LogCode, string message)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, null);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Trace(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, exp);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, message, prms);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void TraceMessage(int LogCode)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void TraceMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Trace(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, null);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Trace(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, exp);
        }

        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, message, prms);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void TraceMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void TraceMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// TRACE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region DEBUG
        /// <summary>
		/// DEBUG���b�Z�[�W
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		public	void	Debug(string	message)
		{
            InnerLog(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, null);
		}
		/// <summary>
		/// DEBUG���b�Z�[�W
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		/// <param name="exp">��O���</param>
		public	void	Debug(string	message,Exception	exp)
		{
            InnerLog(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, exp);
		}
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Debug(int LogCode, string message)
        {
            InnerLog(LogLevel.DEBUG,LogCode, message, null);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Debug(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, exp);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void DebugFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void DebugFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, message, prms);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void DebugMessage(int LogCode)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void DebugMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void DebugFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Debug(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, null);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Debug(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, exp);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void DebugFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, message, prms);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void DebugMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void DebugMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// DEBUG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void DebugFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region INFO
        /// <summary>
		/// ��񃁃b�Z�[�W
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		public	void	Info(string	message)
		{
            InnerLog(LogLevel.INFO, this.GetDefaultLogCode(message), message, null);
		}
		/// <summary>
		/// ��񃁃b�Z�[�W
		/// </summary>
		/// <param name="message">���b�Z�[�W</param>
		/// <param name="exp">��O���</param>
		public	void	Info(string	message,Exception	exp)
		{
            InnerLog(LogLevel.INFO, this.GetDefaultLogCode(message), message, exp);
		}
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Info(int LogCode, string message)
        {
            InnerLog(LogLevel.INFO, LogCode, message, null);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Info(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, message, exp);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void InfoFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void InfoFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, message, prms);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void InfoMessage(int LogCode)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void InfoMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void InfoFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Info(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.INFO, LogCode, message, null);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Info(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, message, exp);
        }
        /// <summary>
        /// ��񃁃b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void InfoFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, message, prms);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void InfoMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void InfoMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// INFO���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void InfoFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region NOTICE-syslog�p�g��
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Note(string message)
        {
            InnerLog(LogLevel.NOTE, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Note(string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Note(int LogCode, string message)
        {
            InnerLog(LogLevel.NOTE,LogCode, message, null);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Note(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, exp);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void NoteFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void NoteFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, message, prms);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void NoteMessage(int LogCode)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void NoteMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void NoteFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Note(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, null);
        }
        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Note(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, exp);
        }

        /// <summary>
        /// �ʒm���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void NoteFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, message, prms);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void NoteMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void NoteMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// NOTE���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void NoteFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region WARN
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Warn(string message)
        {
            InnerLog(LogLevel.WARN, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Warn(string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Warn(int LogCode, string message)
        {
            InnerLog(LogLevel.WARN,LogCode, message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Warn(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, message, exp);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WarnFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WarnFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, message, prms);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void WarnMessage(int LogCode)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void WarnMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WarnFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Warn(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.WARN, LogCode, message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Warn(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, message, exp);
        }

        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WarnFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, message, prms);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void WarnMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void WarnMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// WARN���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void WarnFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region ERROR
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Error(string message)
        {
            InnerLog(LogLevel.ERROR, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Error(string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Error(int LogCode, string message)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, null);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Error(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, exp);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void ErrorFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void ErrorFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, message, prms);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void ErrorMessage(int LogCode)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void ErrorMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void ErrorFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Error(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, null);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Error(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, exp);
        }
        /// <summary>
        /// �G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void ErrorFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, message, prms);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void ErrorMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void ErrorMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ERROR���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void ErrorFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region CRITICAL-syslog�p�g��
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Critical(string message)
        {
            InnerLog(LogLevel.CRIT, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Critical(string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Critical(int LogCode, string message)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, null);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Critical(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, exp);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void CriticalFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void CriticalFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, message, prms);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void CriticalMessage(int LogCode)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void CriticalMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void CriticalFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Critical(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, null);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Critical(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, exp);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void CriticalFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, message, prms);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void CriticalMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void CriticalMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Critical���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void CriticalFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region ALERT-syslog�p�g��
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Alert(string message)
        {
            InnerLog(LogLevel.ALERT, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Alert(string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Alert(int LogCode, string message)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Alert(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, exp);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void AlertFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void AlertFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, message, prms);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void AlertMessage(int LogCode)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void AlertMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void AlertFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Alert(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, null);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Alert(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, exp);
        }
        /// <summary>
        /// �x�����b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void AlertFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, message, prms);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void AlertMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void AlertMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ALERT���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void AlertFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region EMERGENCY-syslog�p�g��
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Emergency(string message)
        {
            InnerLog(LogLevel.EMERG, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Emergency(string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Emergency(int LogCode, string message)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Emergency(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void EmergencyFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void EmergencyFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, message, prms);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void EmergencyMessage(int LogCode)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void EmergencyMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void EmergencyFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Emergency(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Emergency(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void EmergencyFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, message, prms);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void EmergencyMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void EmergencyMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// EMERG���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void EmergencyFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region FATAL
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        public void Fatal(string message)
        {
            InnerLog(LogLevel.FATAL, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Fatal(string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Fatal(int LogCode, string message)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Fatal(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void FatalFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void FatalFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, message, prms);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void FatalMessage(int LogCode)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void FatalMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void FatalFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        public void Fatal(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, null);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Fatal(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, exp);
        }
        /// <summary>
        /// �v���I�G���[���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void FatalFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, message, prms);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        public void FatalMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="exp">��O���</param>
        public void FatalMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// FATAL���b�Z�[�W
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void FatalFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region ������{Loging���\�b�h
        /// <summary>
        /// ������{Loging���\�b�h(�����ҏW��)
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="formatmsg">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        protected virtual void InnerLogFormat(LogLevel level, int LogCode, string formatmsg, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            Exception exp = null;
            if (prms.Length != 0)
            {
                if (prms[prms.Length - 1].GetType().IsSubclassOf(typeof(Exception)) ||
                    prms[prms.Length - 1].GetType().Equals(typeof(Exception)))          //  Exception�N���X���̂��܂܂�Ă��Ȃ��̂Ŕ���C���@2014.05.31
                {   //  ��O�̔h���N���X
                    exp = (Exception)prms[prms.Length - 1];
                }
            }
            this.LogStore<object>(level, LogCode, Formating(formatmsg, prms), CallStack, exp, null, 0);
        }
        /// <summary>
        /// ������{Loging���\�b�h
		/// </summary>
		/// <param name="level">���O���x��</param>
		/// <param name="LogCode">���O�R�[�h</param>
		/// <param name="message">���b�Z�[�W</param>
		/// <param name="exp">��O���</param>
		protected	virtual void	InnerLog(LogLevel level,int LogCode,string message,Exception	exp)
		{
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
		}
        /// <summary>
        /// ������{Loging���\�b�h(�����ҏW��)
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="formatmsg">�������b�Z�[�W</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        protected virtual void InnerLogFormat(LogLevel level, cklib.Log.LogCodes LogCode, string formatmsg, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            Exception exp = null;
            if (prms.Length != 0)
            {
                if (prms[prms.Length - 1].GetType().IsSubclassOf(typeof(Exception)) ||
                    prms[prms.Length - 1].GetType().Equals(typeof(Exception)))          //  Exception�N���X���̂��܂܂�Ă��Ȃ��̂Ŕ���C���@2014.05.31
                {   //  ��O�̔h���N���X
                    exp = (Exception)prms[prms.Length - 1];
                }
            }
            this.LogStore<object>(level, LogCode, Formating(formatmsg, prms), CallStack, exp, null, 0);
        }
        /// <summary>
        /// ���b�Z�[�W�̏�����
        /// </summary>
        /// <param name="formatmsg">������������</param>
        /// <param name="prms">�p�����[�^</param>
        /// <returns></returns>
        protected string Formating(string formatmsg, params object[] prms)
        {
            try
            {
                return string.Format(formatmsg, prms);
            }
            catch (Exception exp)
            {
                StringBuilder stb = new StringBuilder();
                stb.AppendLine("Log Formating Failed");
                stb.AppendLine(formatmsg);
                stb.AppendLine("Parameters");
                foreach (var prm in prms)
                {
                    if (prm != null)
                    {
                        stb.AppendFormat("type:{0}\tValue:{1}\n", prm.GetType().FullName, prm);
                    }
                    else
                    {
                        stb.AppendFormat("type:\tValue:null\n");
                    }
                }
                LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
                if (Mng != null)
                {
                    if (Mng.Config.Common.NotThrowExceptionInFormatError)
                    {
                        stb.Append(exp.ToString());
                        return stb.ToString();
                    }
                }
                throw new Exception(stb.ToString(), exp);
            }
        }
        /// <summary>
        /// ������{Loging���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        protected virtual void InnerLog(LogLevel level, cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        #endregion
        #region ��{Loging���\�b�h
        /// <summary>
        /// ��{���O���\�b�h
        /// </summary>
        /// <remarks>
        /// �������ݏ������H����Ȃǋ��ʂ̃��\�b�h�o�R�ŌĂяo���ۂɌĂяo������CallStack���w�肷��
        /// </remarks>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="CallStack">�X�^�b�N�t���[��</param>
        /// <param name="exp">��O���</param>
        public void Log(LogLevel level, LogCodes LogCode, string message, StackFrame CallStack, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        /// <summary>
        /// ��{���O���\�b�h
        /// </summary>
        /// <remarks>
        /// �������ݏ������H����Ȃǋ��ʂ̃��\�b�h�o�R�ŌĂяo���ۂɌĂяo������CallStack���w�肷��
        /// </remarks>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="CallStack">�X�^�b�N�t���[��</param>
        /// <param name="exp">��O���</param>
        public void Log(LogLevel level, int LogCode, string message,StackFrame CallStack, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        /// <summary>
        /// ��{���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Log(LogLevel level, cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        /// <summary>
        /// ��{���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="exp">��O���</param>
        public void Log(LogLevel level, int LogCode, string message, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<object>(level, LogCode, message, CallStack, exp,null,0);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog(LogLevel level, cklib.Log.LogCodes LogCode, string message, byte[] buffer, int bufferleng)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, message, CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceLog(LogLevel level, cklib.Log.LogCodes LogCode, byte[] buffer, int bufferleng, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, Formating(GetMessage(LogCode), prms), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog(LogLevel level, cklib.Log.LogCodes LogCode, byte[] buffer, int bufferleng)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, GetMessage(LogCode), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog(LogLevel level, int LogCode, string message, byte[] buffer, int bufferleng)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, message, CallStack, null,buffer,bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceLog(LogLevel level, int LogCode, byte[] buffer, int bufferleng, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, Formating(GetMessage(LogCode), prms), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog(LogLevel level, int LogCode, byte[] buffer, int bufferleng)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<byte[]>(level, LogCode, GetMessage(LogCode), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog<T>(LogLevel level, cklib.Log.LogCodes LogCode, string message, T buffer, int bufferleng)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, message, CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceLog<T>(LogLevel level, cklib.Log.LogCodes LogCode, T buffer, int bufferleng, params object[] prms)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, Formating(GetMessage(LogCode), prms), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog<T>(LogLevel level, cklib.Log.LogCodes LogCode, T buffer, int bufferleng)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, GetMessage(LogCode), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog<T>(LogLevel level, int LogCode, string message, T buffer, int bufferleng)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, message, CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        /// <param name="prms">�ҏW�f�[�^</param>
        public void TraceLog<T>(LogLevel level, int LogCode, T buffer, int bufferleng, params object[] prms)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, Formating(GetMessage(LogCode), prms), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// �g���[�X���O���\�b�h
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void TraceLog<T>(LogLevel level, int LogCode, T buffer, int bufferleng)
            where T : class
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = null;
            try
            {
                CallStack = new StackFrame(1, true);
            }
            catch
            { }
            this.LogStore<T>(level, LogCode, GetMessage(LogCode), CallStack, null, buffer, bufferleng);
        }
        /// <summary>
        /// ���O����������
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="CallStack">�X�^�b�N�t���[��</param>
        /// <param name="exp">��O���</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void LogStore<T>(LogLevel level, cklib.Log.LogCodes LogCode, string message, StackFrame CallStack, Exception exp, T buffer, int bufferleng)
            where T : class
        {
            this.LogStore<T>(level, this.GetLogCode(LogCode), message, CallStack, exp, buffer, bufferleng);
        }
        /// <summary>
        /// ���O����������
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <param name="message">���b�Z�[�W</param>
        /// <param name="CallStack">�X�^�b�N�t���[��</param>
        /// <param name="exp">��O���</param>
        /// <param name="buffer">�g���[�X�o�b�t�@</param>
        /// <param name="bufferleng">�g���[�X�f�[�^��</param>
        public void LogStore<T>(LogLevel level, int LogCode, string message, StackFrame CallStack, Exception exp,T buffer,int bufferleng)
            where T:class
        {
            level = this.GetLogLevel(LogCode, level);
            LogData ld = new LogData();
            ld.level = level;
            ld.Code = LogCode;
            ld.Message = message;
            ld.SourceName = this.SourceName;
            try
            {
                if (CallStack != null)
                {
                    MethodBase method = CallStack.GetMethod();
                    if (method != null)
                    {
                        ld.Method = method.Name;
                        if (method.DeclaringType != null)
                        {
                            ld.ClassName = method.DeclaringType.FullName;
                        }
                    }
                    ld.SourceFile = CallStack.GetFileName();
                    ld.SourceLine = CallStack.GetFileLineNumber();
                }
            }
            catch	//(Exception e)
            { }
            ld.Exp = exp;
            ld.TraceBuffer = buffer;
            ld.TraceBufferLength = bufferleng;
            ld.ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
            this.LogManagerLogStore(ld);
        }
        /// <summary>
        /// ���O�}�l�[�W���Ƀ��O���������n��
        /// </summary>
        /// <param name="ld">���O���</param>
        public virtual void LogManagerLogStore(LogData ld)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                Mng.LogStore(ld);
            }
        }
        /// <summary>
        /// �L�^�Ώۂ̃��O���x���̃`�F�b�N
        /// </summary>
        /// <param name="level">���x��</param>
        /// <returns>�L�^�ΏۂȂ�true</returns>
        public  virtual bool IsValidLevel(LogLevel level)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                return Mng.Config.IsValidLevel(level);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// �f�t�H���g���O�R�[�h���擾����
        /// </summary>
        /// <returns></returns>
        protected virtual int GetDefaultLogCode(string message)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                return Mng.DefaultLogCode;
            }
            else
            {
                return cklib.Util.String.ToInt(LogManagerEx.DefaultManagerKey,0);
            }
        }
        #endregion

        #region ���b�Z�[�W�E���O�R�[�h�擾
        /// <summary>
        /// ���O���b�Z�[�W���擾����
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <returns>���b�Z�[�W</returns>
        public string GetMessage(int LogCode)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                return Mng.Config.Message[LogCode+Mng.Config.Common.LogCodeOffset];
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// ���O���b�Z�[�W���擾����
        /// </summary>
        /// <param name="LogCode">���O�R�[�h</param>
        /// <returns>���b�Z�[�W</returns>
        public string GetMessage(cklib.Log.LogCodes LogCode)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                return Mng.Config.Message[LogCode];
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// ���O�R�[�h���擾����
        /// </summary>
        /// <param name="LogCode">���O�R�[�h�N���X�C���X�^���X</param>
        /// <returns>���O�R�[�h</returns>
        public int GetLogCode(cklib.Log.LogCodes LogCode)
        {
            if (LogCode.UseInstanceLogCode)
                return LogCode.LogCode;

            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                return Mng.Config.Message.GetLogCode(LogCode);
            }
            else
            {
                return Mng.DefaultLogCode;
            }
        }
        /// <summary>
        /// ���O���x�����擾����
        /// </summary>
        /// <param name="LogCode">���O�R�[�h�N���X�C���X�^���X</param>
        /// <param name="Level">���O���x��</param>
        /// <returns>���O�R�[�h</returns>
        public LogLevel GetLogLevel(cklib.Log.LogCodes LogCode, LogLevel Level = LogLevel.Undefine)
        {
            return this.GetLogLevel(Level, (Mng) => { return Mng.Config.Message.GetLogLevel(LogCode); });
        }
        /// <summary>
        /// ���O���x�����擾����
        /// </summary>
        /// <param name="LogCode">���O�R�[�h�N���X�C���X�^���X</param>
        /// <param name="Level">���O���x��</param>
        /// <returns>���O�R�[�h</returns>
        public LogLevel GetLogLevel(int LogCode, LogLevel Level = LogLevel.Undefine)
        {
            return this.GetLogLevel(Level, (Mng) => { return Mng.Config.Message.GetLogLevel(LogCode); });
        }
        private LogLevel GetLogLevel(LogLevel Level, Func<LogManagerEx, LogLevel> GetLocCode)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Level != LogLevel.Undefine && (Mng == null || !Mng.Config.Common.LogLevelConfigurationPriority))
                return Level;
            if (Mng != null)
            {
                var level = GetLocCode(Mng);
                if (level == LogLevel.Undefine)
                    return Level;
                else
                    return level;
            }
            else
            {
                return LogLevel.Undefine;
            }
        }
        #endregion
    }
}
