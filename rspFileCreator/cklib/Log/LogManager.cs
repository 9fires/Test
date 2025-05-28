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
	/// ログ機能を管理する
	/// </summary>
	public static class LogManager
	{
        /// <summary>
        /// ログインスタンス
        /// </summary>
        private static LogManagerEx log;
        /// <summary>
        /// 排他制御用
        /// </summary>
        private static object CriticalSection = new object();
		/// <summary>
		/// 初期設定情報
		/// </summary>
		public	static	ConfigInfo	Config
		{
			get	{	return	log.Config;	}
		}
        /// <summary>
        /// デフォルトログコード
        /// </summary>
        /// <remarks>
        /// デバッグログ及びログコードなしのログに使用されるログコード<br/>
        /// イベントログを使用している場合は、コード割り当てが必須なので適切なコードを割り当てる必要がある<br/>
        /// </remarks>
        public static int DefaultLogCode
        {
            get { return log.DefaultLogCode; }
            set { log.DefaultLogCode = value; }
        }
        /// <summary>
        /// ログエンジンエラーイベントインスタンス
        /// </summary>
        public static LogEngineError LogEngineErrorEvent = null;
        /// <summary>
		/// 初期化アプリケーション設定をロードする
		/// </summary>
		/// <returns></returns>
        public static bool Initialize()
		{
            lock (CriticalSection)
            {
                log = new LogManagerEx();
                if (LogEngineErrorEvent!=null)
                {
                    log.LogEngineErrorEvent = LogEngineErrorEvent;
                }
                return log.Initialize();
            }
		}
        /// <summary>
        /// 初期化アプリケーション設定をロードする
        /// </summary>
        /// <param name="conf">設定情報インスタンス</param>
        /// <returns></returns>
        public static bool Initialize(ConfigInfo conf)
        {
            lock (CriticalSection)
            {
                log = new LogManagerEx();
                if (LogEngineErrorEvent != null)
                {
                    log.LogEngineErrorEvent = LogEngineErrorEvent;
                }
                return log.Initialize(conf);
            }
        }
        /// <summary>
        /// 再起動
        /// </summary>
        /// <returns>初期化結果</returns>
        public static bool ReStart()
        {
            lock (CriticalSection)
            {
                if (log != null)
                    return log.ReStart();
                else
                    return Initialize();
            }
        }
        /// <summary>
        /// 再起動
        /// </summary>
        /// <param name="conf">設定情報インスタンス</param>
        /// <returns></returns>
        public static bool ReStart(ConfigInfo conf)
        {
            lock (CriticalSection)
            {
                if (log != null)
                    return log.ReStart(conf);
                else
                    return Initialize();
            }
        }

		/// <summary>
		/// ログエンジンの停止
		/// </summary>
        public static void Terminate()
        {
            lock (CriticalSection)
            {
                if (log != null)
                {
                    log.Terminate();
                    log = null;
                }
            }
        }
	}
}
