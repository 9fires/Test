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
	/// ログ書き込みインスタンス
	/// </summary>
    [Serializable]
	public class Logger
    {
        #region フィールド
        /// <summary>
        /// ログマネージャインスタンス
        /// </summary>
        public readonly string MngKey = null;
        /// <summary>
		/// ソース名
		/// </summary>
		internal string	SourceName=string.Empty;
        /// <summary>
        /// コールスタックを取り出しネスト
        /// </summary>
        protected int CallStackDepth=2;
        #endregion
        #region コンストラクタ
        /// <summary>
		/// コンストラクタ
		/// </summary>
        public Logger()
		{
            this.MngKey = LogManagerEx.DefaultManagerKey;
            SetSourceName();
        }
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public Logger(System.Type type)
		{
            this.MngKey = LogManagerEx.DefaultManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger(System.Type type,string ManagerKey)
        {
            this.MngKey = ManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger(System.Type type, LogManagerEx mng)
        {
            this.MngKey = mng.ManagerKey;
            SourceName = type.ToString();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger(string ManagerKey)
        {
            this.MngKey = ManagerKey;
            SetSourceName();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Logger(LogManagerEx mng)
        {
            this.MngKey = mng.ManagerKey;
            SetSourceName();
        }
        /// <summary>
        /// スタックフレームから呼び出し元のソース名を抽出する
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
        #region レベル指定無し
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Write(string message)
        {
            InnerLog(LogLevel.Undefine, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Write(string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Write(int LogCode, string message)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, null);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Write(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, exp);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WriteFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WriteFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, message, prms);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void WriteMessage(int LogCode)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void WriteMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void WriteFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Write(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, null);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Write(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, message, exp);
        }

        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WriteFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, message, prms);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void WriteMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void WriteMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.Undefine, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Writeメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void WriteFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.Undefine, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region TRACE
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Trace(string message)
        {
            InnerLog(LogLevel.TRACE, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Trace(string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Trace(int LogCode, string message)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, null);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Trace(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, exp);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void TraceFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void TraceFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, message, prms);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void TraceMessage(int LogCode)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void TraceMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void TraceFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Trace(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, null);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Trace(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, message, exp);
        }

        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void TraceFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, message, prms);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void TraceMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void TraceMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.TRACE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// TRACEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void TraceFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.TRACE, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region DEBUG
        /// <summary>
		/// DEBUGメッセージ
		/// </summary>
		/// <param name="message">メッセージ</param>
		public	void	Debug(string	message)
		{
            InnerLog(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, null);
		}
		/// <summary>
		/// DEBUGメッセージ
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="exp">例外情報</param>
		public	void	Debug(string	message,Exception	exp)
		{
            InnerLog(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, exp);
		}
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Debug(int LogCode, string message)
        {
            InnerLog(LogLevel.DEBUG,LogCode, message, null);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Debug(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, exp);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void DebugFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void DebugFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, message, prms);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void DebugMessage(int LogCode)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void DebugMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void DebugFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Debug(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, null);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Debug(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, message, exp);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void DebugFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, message, prms);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void DebugMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void DebugMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.DEBUG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// DEBUGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void DebugFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.DEBUG, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region INFO
        /// <summary>
		/// 情報メッセージ
		/// </summary>
		/// <param name="message">メッセージ</param>
		public	void	Info(string	message)
		{
            InnerLog(LogLevel.INFO, this.GetDefaultLogCode(message), message, null);
		}
		/// <summary>
		/// 情報メッセージ
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="exp">例外情報</param>
		public	void	Info(string	message,Exception	exp)
		{
            InnerLog(LogLevel.INFO, this.GetDefaultLogCode(message), message, exp);
		}
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Info(int LogCode, string message)
        {
            InnerLog(LogLevel.INFO, LogCode, message, null);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Info(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, message, exp);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void InfoFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void InfoFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, message, prms);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void InfoMessage(int LogCode)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void InfoMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void InfoFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Info(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.INFO, LogCode, message, null);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Info(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, message, exp);
        }
        /// <summary>
        /// 情報メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void InfoFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, message, prms);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void InfoMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void InfoMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.INFO, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// INFOメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void InfoFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.INFO, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region NOTICE-syslog用拡張
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Note(string message)
        {
            InnerLog(LogLevel.NOTE, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Note(string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Note(int LogCode, string message)
        {
            InnerLog(LogLevel.NOTE,LogCode, message, null);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Note(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, exp);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void NoteFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void NoteFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, message, prms);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void NoteMessage(int LogCode)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void NoteMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void NoteFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Note(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, null);
        }
        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Note(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, message, exp);
        }

        /// <summary>
        /// 通知メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void NoteFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, message, prms);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void NoteMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void NoteMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.NOTE, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// NOTEメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void NoteFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.NOTE, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region WARN
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Warn(string message)
        {
            InnerLog(LogLevel.WARN, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Warn(string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Warn(int LogCode, string message)
        {
            InnerLog(LogLevel.WARN,LogCode, message, null);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Warn(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, message, exp);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WarnFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WarnFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, message, prms);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void WarnMessage(int LogCode)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void WarnMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void WarnFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Warn(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.WARN, LogCode, message, null);
        }
        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Warn(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, message, exp);
        }

        /// <summary>
        /// 警告メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void WarnFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, message, prms);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void WarnMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void WarnMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.WARN, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// WARNメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void WarnFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.WARN, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region ERROR
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Error(string message)
        {
            InnerLog(LogLevel.ERROR, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Error(string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Error(int LogCode, string message)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, null);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Error(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, exp);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void ErrorFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void ErrorFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, message, prms);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void ErrorMessage(int LogCode)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void ErrorMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void ErrorFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Error(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, null);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Error(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, message, exp);
        }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void ErrorFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, message, prms);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void ErrorMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void ErrorMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.ERROR, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ERRORメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void ErrorFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ERROR, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region CRITICAL-syslog用拡張
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Critical(string message)
        {
            InnerLog(LogLevel.CRIT, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Critical(string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Critical(int LogCode, string message)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, null);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Critical(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, exp);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void CriticalFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void CriticalFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, message, prms);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void CriticalMessage(int LogCode)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void CriticalMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void CriticalFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Critical(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, null);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Critical(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, message, exp);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void CriticalFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, message, prms);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void CriticalMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void CriticalMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.CRIT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// Criticalメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void CriticalFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.CRIT, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region ALERT-syslog用拡張
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Alert(string message)
        {
            InnerLog(LogLevel.ALERT, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Alert(string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Alert(int LogCode, string message)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, null);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Alert(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, exp);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void AlertFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void AlertFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, message, prms);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void AlertMessage(int LogCode)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void AlertMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void AlertFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Alert(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, null);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Alert(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, message, exp);
        }
        /// <summary>
        /// 警戒メッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void AlertFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, message, prms);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void AlertMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void AlertMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.ALERT, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// ALERTメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void AlertFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.ALERT, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region EMERGENCY-syslog用拡張
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Emergency(string message)
        {
            InnerLog(LogLevel.EMERG, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Emergency(string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Emergency(int LogCode, string message)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Emergency(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void EmergencyFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void EmergencyFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, message, prms);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void EmergencyMessage(int LogCode)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void EmergencyMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void EmergencyFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Emergency(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Emergency(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void EmergencyFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, message, prms);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void EmergencyMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void EmergencyMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.EMERG, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// EMERGメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void EmergencyFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.EMERG, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region FATAL
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Fatal(string message)
        {
            InnerLog(LogLevel.FATAL, this.GetDefaultLogCode(message), message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Fatal(string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, this.GetDefaultLogCode(message), message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Fatal(int LogCode, string message)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Fatal(int LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void FatalFormat(string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, this.GetDefaultLogCode(message), message, prms);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void FatalFormat(int LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, message, prms);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void FatalMessage(int LogCode)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void FatalMessage(int LogCode, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void FatalFormatMessage(int LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, GetMessage(LogCode), prms);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        public void Fatal(cklib.Log.LogCodes LogCode, string message)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, null);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        public void Fatal(cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, message, exp);
        }
        /// <summary>
        /// 致命的エラーメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        public void FatalFormat(cklib.Log.LogCodes LogCode, string message, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, message, prms);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        public void FatalMessage(cklib.Log.LogCodes LogCode)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), null);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="exp">例外情報</param>
        public void FatalMessage(cklib.Log.LogCodes LogCode, Exception exp)
        {
            InnerLog(LogLevel.FATAL, LogCode, GetMessage(LogCode), exp);
        }
        /// <summary>
        /// FATALメッセージ
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <param name="prms">編集データ</param>
        public void FatalFormatMessage(cklib.Log.LogCodes LogCode, params object[] prms)
        {
            InnerLogFormat(LogLevel.FATAL, LogCode, GetMessage(LogCode), prms);
        }
        #endregion
        #region 内部基本Logingメソッド
        /// <summary>
        /// 内部基本Logingメソッド(書式編集版)
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="formatmsg">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        protected virtual void InnerLogFormat(LogLevel level, int LogCode, string formatmsg, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            Exception exp = null;
            if (prms.Length != 0)
            {
                if (prms[prms.Length - 1].GetType().IsSubclassOf(typeof(Exception)) ||
                    prms[prms.Length - 1].GetType().Equals(typeof(Exception)))          //  Exceptionクラス自体が含まれていないので判定修正　2014.05.31
                {   //  例外の派生クラス
                    exp = (Exception)prms[prms.Length - 1];
                }
            }
            this.LogStore<object>(level, LogCode, Formating(formatmsg, prms), CallStack, exp, null, 0);
        }
        /// <summary>
        /// 内部基本Logingメソッド
		/// </summary>
		/// <param name="level">ログレベル</param>
		/// <param name="LogCode">ログコード</param>
		/// <param name="message">メッセージ</param>
		/// <param name="exp">例外情報</param>
		protected	virtual void	InnerLog(LogLevel level,int LogCode,string message,Exception	exp)
		{
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
		}
        /// <summary>
        /// 内部基本Logingメソッド(書式編集版)
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="formatmsg">書式メッセージ</param>
        /// <param name="prms">編集データ</param>
        protected virtual void InnerLogFormat(LogLevel level, cklib.Log.LogCodes LogCode, string formatmsg, params object[] prms)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            Exception exp = null;
            if (prms.Length != 0)
            {
                if (prms[prms.Length - 1].GetType().IsSubclassOf(typeof(Exception)) ||
                    prms[prms.Length - 1].GetType().Equals(typeof(Exception)))          //  Exceptionクラス自体が含まれていないので判定修正　2014.05.31
                {   //  例外の派生クラス
                    exp = (Exception)prms[prms.Length - 1];
                }
            }
            this.LogStore<object>(level, LogCode, Formating(formatmsg, prms), CallStack, exp, null, 0);
        }
        /// <summary>
        /// メッセージの書式化
        /// </summary>
        /// <param name="formatmsg">書式化文字列</param>
        /// <param name="prms">パラメータ</param>
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
        /// 内部基本Logingメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
        protected virtual void InnerLog(LogLevel level, cklib.Log.LogCodes LogCode, string message, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            StackFrame CallStack = new StackFrame(CallStackDepth, true);
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        #endregion
        #region 基本Logingメソッド
        /// <summary>
        /// 基本ログメソッド
        /// </summary>
        /// <remarks>
        /// 書き込み情報を加工するなど共通のメソッド経由で呼び出す際に呼び出し元のCallStackを指定する
        /// </remarks>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="CallStack">スタックフレーム</param>
        /// <param name="exp">例外情報</param>
        public void Log(LogLevel level, LogCodes LogCode, string message, StackFrame CallStack, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        /// <summary>
        /// 基本ログメソッド
        /// </summary>
        /// <remarks>
        /// 書き込み情報を加工するなど共通のメソッド経由で呼び出す際に呼び出し元のCallStackを指定する
        /// </remarks>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="CallStack">スタックフレーム</param>
        /// <param name="exp">例外情報</param>
        public void Log(LogLevel level, int LogCode, string message,StackFrame CallStack, Exception exp)
        {
            level = this.GetLogLevel(LogCode, level);
            if (!IsValidLevel(level)) return;
            this.LogStore<object>(level, LogCode, message, CallStack, exp, null, 0);
        }
        /// <summary>
        /// 基本ログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
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
        /// 基本ログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="exp">例外情報</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
        /// <param name="prms">編集データ</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
        /// <param name="prms">編集データ</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
        /// <param name="prms">編集データ</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
        /// <param name="prms">編集データ</param>
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
        /// トレースログメソッド
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// ログを書き込む
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="CallStack">スタックフレーム</param>
        /// <param name="exp">例外情報</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
        public void LogStore<T>(LogLevel level, cklib.Log.LogCodes LogCode, string message, StackFrame CallStack, Exception exp, T buffer, int bufferleng)
            where T : class
        {
            this.LogStore<T>(level, this.GetLogCode(LogCode), message, CallStack, exp, buffer, bufferleng);
        }
        /// <summary>
        /// ログを書き込む
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <param name="LogCode">ログコード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="CallStack">スタックフレーム</param>
        /// <param name="exp">例外情報</param>
        /// <param name="buffer">トレースバッファ</param>
        /// <param name="bufferleng">トレースデータ長</param>
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
        /// ログマネージャにログ情報を引き渡す
        /// </summary>
        /// <param name="ld">ログ情報</param>
        public virtual void LogManagerLogStore(LogData ld)
        {
            LogManagerEx Mng = LogManagerEx.LookupLogManagerEx(this.MngKey);
            if (Mng != null)
            {
                Mng.LogStore(ld);
            }
        }
        /// <summary>
        /// 記録対象のログレベルのチェック
        /// </summary>
        /// <param name="level">レベル</param>
        /// <returns>記録対象ならtrue</returns>
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
        /// デフォルトログコードを取得する
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

        #region メッセージ・ログコード取得
        /// <summary>
        /// ログメッセージを取得する
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <returns>メッセージ</returns>
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
        /// ログメッセージを取得する
        /// </summary>
        /// <param name="LogCode">ログコード</param>
        /// <returns>メッセージ</returns>
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
        /// ログコードを取得する
        /// </summary>
        /// <param name="LogCode">ログコードクラスインスタンス</param>
        /// <returns>ログコード</returns>
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
        /// ログレベルを取得する
        /// </summary>
        /// <param name="LogCode">ログコードクラスインスタンス</param>
        /// <param name="Level">ログレベル</param>
        /// <returns>ログコード</returns>
        public LogLevel GetLogLevel(cklib.Log.LogCodes LogCode, LogLevel Level = LogLevel.Undefine)
        {
            return this.GetLogLevel(Level, (Mng) => { return Mng.Config.Message.GetLogLevel(LogCode); });
        }
        /// <summary>
        /// ログレベルを取得する
        /// </summary>
        /// <param name="LogCode">ログコードクラスインスタンス</param>
        /// <param name="Level">ログレベル</param>
        /// <returns>ログコード</returns>
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
