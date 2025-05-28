using System;
using cklib;

namespace cklib.Log
{
	/// <summary>
	/// LogData の概要の説明です。
	/// </summary>
	public class LogData
	{
		/// <summary>
		/// ログ事象発生時間
		/// </summary>
		public	DateTime	Time=	DateTime.Now;
		/// <summary>
		/// ログレベル
		/// </summary>
		public	LogLevel	level=	LogLevel.DEBUG;
		/// <summary>
		/// ログコード
		/// </summary>
		public	int			Code	=	0;
		/// <summary>
		/// ログメッセージ
		/// </summary>
		public	string		Message	=	string.Empty;
		/// <summary>
		/// ソースファイル情報
		/// </summary>
		public	string		SourceName	=	string.Empty;
        /// <summary>
        /// クラス名
        /// </summary>
        public  string      ClassName   =   string.Empty;
        /// <summary>
        /// メソッド名
        /// </summary>
        public  string      Method      =   string.Empty;
		/// <summary>
		/// ソースファイル情報
		/// </summary>
		public	string		SourceFile	=	string.Empty;
		/// <summary>
		/// ソースファイル行
		/// </summary>
		public	int			SourceLine	=	0;
		/// <summary>
		/// 例外情報
		/// </summary>
		public	Exception	Exp		=	null;
        /// <summary>
        /// 通信トレースバイナリデータ
        /// </summary>
        public  object      TraceBuffer = null;
        /// <summary>
        /// 通信トレースバイナリデータ長
        /// </summary>
        public  int         TraceBufferLength = 0;
        /// <summary>
        /// ログ記録スレッドID
        /// </summary>
        public  int         ThreadID=0;
	}
}
