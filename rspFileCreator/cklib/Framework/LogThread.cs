using System;
using System.IO;
using System.Text;
using cklib.Framework.IPC;
namespace cklib.Framework
{
	/// <summary>
	/// 日毎ローテートの簡易ログスレッドクラス
	/// </summary>
	public class LogThread:AppThread
	{
		/// <summary>
		/// ログ書き込みパス
		/// </summary>
		private	string	path	=	string.Empty;
		/// <summary>
		/// ログ書き込みパス
		/// </summary>
		public	string	Path
		{
			get	{	return	path;	}
			set	
			{	
				path	=	value;	
				if	(path.Substring(Path.Length-1,1)!="\\")
				{
					path	+=	"\\";
				}
			}
		}
		/// <summary>
		/// ログファイル書き込みストリーム
		/// </summary>
		private	System.IO.FileStream	LogFile=null;
		/// <summary>
		/// ログファイル名
		/// </summary>
		private	string	LogFileName		=	string.Empty;
		/// <summary>
		/// ログファイル名生成用キー名
		/// </summary>
		private	string	LogKeyName		=	string.Empty;
		/// <summary>
		/// ログファイル名生成用キー名
		/// </summary>
		public	string	KeyName
		{
			get	{	return	LogKeyName;	}
			set	{	LogKeyName	=	value;	}
		}
        /// <summary>
        /// ログファイルの文字エンコード種類
        /// </summary>
        public  Encoding Encoding = Encoding.GetEncoding("Shift_JIS");
		/// <summary>
		/// ファイル書き込み有無フラグ
		/// </summary>
		private	bool	fLogAfter=false;
		/// <summary>
		///	ログデータ構造体
		/// </summary>
		private	struct	LogInfo
		{
			public	DateTime	LogDate;
			public	String		LogData;
		}
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public LogThread()
		{
			path		=	string.Empty;
			LogFileName	=	string.Empty;
			LogKeyName	=	string.Empty;
		}
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="path">ログ書き込みディレクトリパス</param>
		/// <param name="key">ログファイル先頭文字列</param>
		public LogThread(String	path,String	key)
		{
			this.Path	=	path;
			LogFileName	=	string.Empty;
			LogKeyName	=	key;
		}
		/// <summary>
		/// スレッドの終了処理
		/// </summary>
		/// <returns></returns>
		protected	override bool	ExitInstance()
		{
			CloseLog();
			return	true;
		}
        #region 例外処理のオーバーライド
        /// <summary>
        /// 待機中の例外発生時の処理
        /// </summary>
        /// <returns></returns>
        protected override int WaitError(Exception e)
        {
            return -1;
        }
        /// <summary>
        /// イベント処理中の例外発生時処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool EventError(Exception e)
        {
            return false;
        }
        /// <summary>
        /// catchされていない例外発生時処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void ThreadError(Exception e)
        {
            return;
        }
        #endregion
		/// <summary>
		/// 待機タイムアウト時間をミリ秒で返す
		/// </summary>
		/// <returns></returns>
		protected	override	int	GetWaitTime()
		{
			if	(fLogAfter)
			{
				return	500;
			}
			return	-1;
		}
		/// <summary>
		/// イベント発生タイムアウト時の処理
		/// </summary>
		/// <returns></returns>
		protected	override	bool	EventTimeout()
		{
			CloseLog();
			return	true;
		}
		/// <summary>
		/// ログ書き込みイベント
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	override	bool	EventData(EventDataType ed)
		{
			LogInfo	li	=	(LogInfo)ed.EventData;
			if	(OpenLog(li.LogDate))
			{
				try
				{
					byte[]	sjis	=	this.Encoding.GetBytes(
						li.LogDate.ToString("yyyy/MM/dd HH:mm:ss.")+li.LogDate.Millisecond.ToString("000")+" "+li.LogData
						);
					LogFile.Write(sjis,0,sjis.Length);
					fLogAfter	=	true;
				}
				catch	//(System.Exception exp)
				{
				}
			}
			return	true;
		}
		/// <summary>
		/// ログイベント
		/// </summary>
		/// <param name="ldate">ログ時刻</param>
		/// <param name="msg">ログメッセージ</param>
		/// <returns></returns>
		public	virtual	bool	Loging(System.DateTime ldate,String	msg)
		{
			LogInfo	li	=	new	LogInfo();
			li.LogDate	=	ldate;
			li.LogData	=	msg;
			return	this.IPCPut(EventCode.Data,li);
		}
		/// <summary>
		/// ログイベント
		/// </summary>
		/// <param name="msg">ログメッセージ</param>
		/// <returns></returns>
		public	virtual	bool	Loging(String	msg)
		{
			return	this.Loging(DateTime.Now,msg);
		}
		/// <summary>
		/// ログファイルオープン
		/// </summary>
		/// <param name="ldate">ファイル日付</param>
		/// <returns></returns>
		private	bool	OpenLog(DateTime	ldate)
		{
			String	fnam	=	GenLogFileName(ldate);
			if	(fnam!=LogFileName)
			{
				CloseLog();
				bool	ret=false;
				try
				{
					LogFile	=	System.IO.File.Open(fnam,
													System.IO.FileMode.Append,
													System.IO.FileAccess.Write,
													System.IO.FileShare.ReadWrite);	
					ret	=	true;
				}
				catch	//(System.Exception	exp)
				{
					LogFile	=null;
				}
				return	ret;
			}
			return	true;
		}
		/// <summary>
		/// ファイルを閉じる
		/// </summary>
		private	void	CloseLog()
		{
			if	(LogFile!=null)
			{
				LogFile.Close();
				LogFile	=	null;
			}
		}
		/// <summary>
		/// ログファイル名生成
		/// </summary>
		/// <param name="ldate"></param>
		/// <returns></returns>
		private	String	GenLogFileName(DateTime	ldate)
		{
			return	Path+LogKeyName+ldate.ToString("yyyyMMdd")+".log";
		}
	}
}
