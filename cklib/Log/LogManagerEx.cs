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
    public class LogManagerEx : IDisposable
	{
        /// <summary>
        /// 初期化参照数
        /// </summary>
        private bool fInitialize = false;
        /// <summary>
        /// 初期化状態フラグ
        /// </summary>
        public bool IsInitialized
        {
            get { return    fInitialize;}
        }
        /// <summary>
		/// 初期設定情報
		/// </summary>
		private	ConfigInfo	config=null;
		/// <summary>
		/// 初期設定情報
		/// </summary>
		public	ConfigInfo	Config
		{
			get	{	return	config;	}
		}
        /// <summary>
        /// ログエンジンエラーイベントインスタンス
        /// </summary>
        public LogEngineError LogEngineErrorEvent = null;
        /// <summary>
        /// ログエンジンエラーイベントインスタンス
        /// </summary>
        public LogEngineError2 LogEngineErrorEvent2 = null;
        /// <summary>
        /// デフォルトログコード
        /// </summary>
        /// <remarks>
        /// デバッグログ及びログコードなしのログに使用されるログコード<br/>
        /// イベントログを使用している場合は、コード割り当てが必須なので適切なコードを割り当てる必要がある<br/>
        /// </remarks>
        public int DefaultLogCode = 0;
		/// <summary>
		/// ロギング処理エンジンをログマネージャ毎に保持する
		/// </summary>
        private static System.Collections.Generic.Dictionary<string, LogingEngine> Engines = new System.Collections.Generic.Dictionary<string, LogingEngine>();
        /// <summary>
        /// ロギング処理エンジンをログマネージャ毎に保持する
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, LogManagerEx> Managers = new System.Collections.Generic.Dictionary<string, LogManagerEx>();
        /// <summary>
        /// 指定されキーのマネジャークラスインスタンスを取得する
        /// </summary>
        /// <param name="key">キー文字列</param>
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
        /// デフォルトのマネージャーキー
        /// </summary>
        public static readonly string DefaultManagerKey = ConfigInfo.DefaultConfigSectionName;
        /// <summary>
        /// インスタンスを識別するキー情報
        /// </summary>
        public readonly string ManagerKey;
        #region コンストラクタ・デストラクタ
        /// <summary>
        ///	コンストラクタ
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
		///	コンストラクタ
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
		/// ディストラクタ
		/// </summary>
        ~LogManagerEx()
		{
			Dispose(false);
        }
        #endregion コンストラクタ・デストラクタ
        #region IDisposable メンバ
        /// <summary>
        /// Dispose完了フラグ
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Disposeメソッド
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose処理の実装
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
        /// リソース解放処理
        /// </summary>
        protected virtual void ReleseResorce()
        {
        }
        /// <summary>
        /// マネージドリソース解放処理（明示的呼び出し時のみ実行される）
        /// </summary>
        protected virtual void ReleseManagedResorce()
        {
        }

        #endregion
        #region 初期化メソッド
        /// <summary>
        /// 初期化アプリケーション設定をロードする
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
        /// 初期化アプリケーション設定をロードする
        /// </summary>
        /// <param name="conf">設定情報インスタンス</param>
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
        /// 初期化サブ処理
        /// </summary>
        /// <returns></returns>
        private bool InitializeSub()
        {
            //	イベントログの初期化
            if (config.EventLog.Enabled)
            {
                if (!System.Diagnostics.EventLog.SourceExists(config.EventLog.EventSource))
                {	//	イベントログソースの生成
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
                    {   //  バックグラウンドモード時にスレッドを起動する
                        LogManagerEx.Engines[ManagerKey].IPCQueueMaxSize = config.Common.QueueingSize;
                        LogManagerEx.Engines[ManagerKey].Start();
                    }
                    else
                    {   //  起動時のログローテート抜けするので対処
                        try
                        {
                            LogManagerEx.Engines[ManagerKey].LogExtendStart();
                            LogManagerEx.Engines[ManagerKey].LogRotation();
                        }
                        catch (Exception exp)
                        {
                            //  ログエンジン例外ハンドラが設定されている可能性があるので呼び出す。
                            LogManagerEx.Engines[ManagerKey].ExceptionPrint(LogLevel.ERROR, "Rotate Failed", exp);
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 再起動
        /// </summary>
        /// <returns>初期化結果</returns>
        public bool ReStart()
        {
            lock (this)
            {
                this.Terminate();
                return Initialize();
            }
        }
        /// <summary>
        /// 再起動
        /// </summary>
        /// <param name="conf">設定情報インスタンス</param>
        /// <returns></returns>
        public bool ReStart(ConfigInfo conf)
        {
            lock (this)
            {
                this.Terminate();
                return Initialize(conf);
            }
        }
        #endregion 初期化メソッド
        /// <summary>
        /// ログエンジンのガベージ
        /// </summary>
        /// <remarks>
        /// Web等アプリケーションの制御と無関係にログエンジンがガベージされてしまった場合にログエンジンより呼び出しつじつまを合わせる
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
		/// ログエンジンの停止
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
                        //	ログエンジンの停止
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
		/// ログの書き込み
		/// </summary>
		/// <param name="data">ログデータ</param>
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
                        {   //  バックグラウンドモード
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
