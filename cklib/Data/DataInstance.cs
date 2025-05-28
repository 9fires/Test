using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using cklib.Log;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace  cklib.Data
{
    /// <summary>
    /// DataInstanceロギングインターフェース
    /// </summary>
    public interface IDataInstanceInnerLogging
    {
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="exp">例外情報</param>
        /// <param name="prms">その他パラメータ</param>
        void InnerLogExp(int depth, DataTraceLogCode Code, string msg, Exception exp, params object[] prms);
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        void InnerLog(int depth, DataTraceLogCode Code, string msg, params object[] prms);

    }
    /// <summary>
    /// トランザクション関連のインターフェース
    /// </summary>
    public interface IDataInstanceTransaction<Data_Transaction>
        where Data_Transaction:IDataTransaction
    {
        /// <summary>
        /// DBトランザクションインスタンス
        /// </summary>
        Data_Transaction DataTransaction
        {
            get;
        }
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">ロックレベルの指定</param>
        /// <returns>DBTransactionインスタンス</returns>
        Data_Transaction BeginDBTransaction(IsolationLevel iso);
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <returns>DBTransactionインスタンス</returns>
        Data_Transaction BeginDBTransaction();
    }
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス
    /// </summary>
    [Serializable]
    public abstract class DataInstance<Data_Transaction, DBConnection, DBCommand, DBTransaction, DBParamater, DBDataReader, DBDataAdapter, DATACONFIG, DATACONFIGELEMENT> : IDisposable, IDataInstanceInnerLogging, IDataInstanceTransaction<Data_Transaction>
        where Data_Transaction:DataTransaction<DBTransaction>
        where DBConnection:DbConnection
        where DBCommand:DbCommand
        where DBTransaction : DbTransaction
        where DBParamater : DbParameter
        where DBDataReader:DbDataReader
        where DBDataAdapter:DbDataAdapter
        where DATACONFIG : DataConfigSection<DATACONFIGELEMENT>
        where DATACONFIGELEMENT : DataConfigElement
    {
        #region インスタンス参照プロパティ
        /// <summary>
        /// 接続インスタンス
        /// </summary>
        protected DBConnection m_dbConnection = null;
        /// <summary>
        /// 接続インスタンス
        /// </summary>
        public DBConnection dbConnection
        {
            get
            { return this.m_dbConnection; }
        }
        /// <summary>
        /// DataCommandインスタンス
        /// </summary>
        protected DBCommand  m_dbCommand   = null;
        /// <summary>
        /// DataCommandインスタンス
        /// </summary>
        public DBCommand sqlCommand
        {
            get
            { return this.m_dbCommand; }
            set
            {
                SetupDataCommand(this.m_dbCommand, value);
                this.m_dbCommand = value;
            }
        }
        /// <summary>
        /// SQLコマンドのセットアップ
        /// </summary>
        /// <param name="prmDataCommand">格納先のDataCommandインスタンス</param>
        /// <param name="value">新しいSQLCommandインスタンス</param>
        protected void SetupDataCommand(DBCommand prmDataCommand, DBCommand value)
        {
            this.ReleaseDataCommand(prmDataCommand);
            this.SetupDataCommand(value);
        }
        /// <summary>
        /// SQLコマンドのセットアップ
        /// </summary>
        /// <param name="prmDataCommand">格納先のDataCommandインスタンス</param>
        protected void SetupDataCommand(DBCommand prmDataCommand)
        {
            if (prmDataCommand == null)
            {
                return;
            }
            prmDataCommand.Connection = this.dbConnection;
            if (this.DataTransaction != null)
            {
                prmDataCommand.Transaction = this.DataTransaction.Transaction;
            }
            if (this.DBCommandTimer != -1)
            {
                prmDataCommand.CommandTimeout = this.DBCommandTimer;
            }
        }
        /// <summary>
        /// SQLコマンドの開放
        /// </summary>
        /// <param name="prmDataCommand">格納先のDataCommandインスタンス</param>
        protected void ReleaseDataCommand(DBCommand prmDataCommand)
        {
            if (prmDataCommand != null)
            {
                try
                {
                    prmDataCommand.Dispose();
                }
                catch
                { }
            }
        }
        /// <summary>
        /// DBトランザクションインスタンス
        /// </summary>
        private Data_Transaction m_DBTransaction = null;
        /// <summary>
        /// DBトランザクションインスタンス
        /// </summary>
        public Data_Transaction DataTransaction
        {
            get
            { return this.m_DBTransaction; }
        }
        /// <summary>
        /// DataCommand実行タイマー
        /// </summary>
        private int m_DBCommandTimer = -1;
        /// <summary>
        /// DataCommand実行タイマー
        /// </summary>
        public int DBCommandTimer
        {
            get
            {
                if (this.m_DBCommandTimer == -1)
                    return m_ShareDBCommandTimer;
                return m_DBCommandTimer;
            }
            set
            {
                this.m_DBCommandTimer = value;
            }
        }
        /// <summary>
        /// インスタンスID
        /// </summary>
        public readonly long InstanceID = Interlocked.Increment(ref InstanceIDBase);
        /// <summary>
        /// 共通DBコマンドタイマー
        /// </summary>
        static private int m_ShareDBCommandTimer = -1;
        /// <summary>
        /// 共通DBコマンドタイマー
        /// </summary>
        static public int ShareDBCommandTimer
        {
            get
            {
                return m_ShareDBCommandTimer;
            }
            set
            {
                m_ShareDBCommandTimer = value;
            }
        }
        /// <summary>
        /// 共通接続文字列
        /// </summary>
        static private string m_ShareDBConnectString = string.Empty;
        /// <summary>
        /// 共通接続文字列
        /// </summary>
        static public string ShareDBConnectString
        {
            get
            {
                return m_ShareDBConnectString;
            }
            set
            {
                lock (m_ShareDBConnectString)
                {
                    m_ShareDBConnectString = value;
                }
            }
        }
        /// <summary>
        /// インスタンス生成ベース
        /// </summary>
        static private long InstanceIDBase = 0;
        #region トーレスログ設定
        /// <summary>
        /// SQLトレースログ許可
        /// </summary>
        static public bool SQLTraceEnableDefault
        {
            get
            {
                return m_SQLTraceEnableDefault;
            }
            set
            {
                m_SQLTraceEnableDefault = value;
            }
        }
        static bool m_SQLTraceEnableDefault;
        /// <summary>
        /// SQLトレースログ許可
        /// </summary>
        public bool SQLTraceEnable
        {
            get
            {
                if (!this.m_SQLTraceEnable.HasValue)
                    return SQLTraceEnableDefault;
                return this.m_SQLTraceEnable.Value;
            }
            set
            {
                m_SQLTraceEnable = value;
            }
        }
        bool? m_SQLTraceEnable;
        /// <summary>
        /// SQLトレースログ採取マネージャーキー
        /// </summary>
        static public string DefaultSQLTraceManagerKey=LogManagerEx.DefaultManagerKey;
        /// <summary>
        /// SQLトレースログ採取マネージャーキー
        /// </summary>
        public string SQLTraceManagerKey
        {
            get
            {
                if (this.m_SQLTraceManagerKey.Length == 0)
                    return DefaultSQLTraceManagerKey;
                return this.m_SQLTraceManagerKey;
            }
            set
            {
                this.m_SQLTraceManagerKey = value;
                this.traceLog = new Logger(this.SQLTraceManagerKey);
            }
        }
        string m_SQLTraceManagerKey = string.Empty;
        /// <summary>
        /// SQLトレース採取用ログレベル
        /// </summary>
        static public LogLevel DefaultSQLTraceLogLevel = LogLevel.TRACE;
        /// <summary>
        /// SQLトレース採取用ログレベル
        /// </summary>
        public LogLevel SQLTraceLogLevel
        {
            get
            {
                if (this.m_SQLTraceLogLevel == LogLevel.Undefine)
                    return DefaultSQLTraceLogLevel;
                return this.m_SQLTraceLogLevel;
            }
            set
            {
                this.m_SQLTraceLogLevel = value;
            }
        }
        LogLevel m_SQLTraceLogLevel = LogLevel.Undefine;
        /// <summary>
        /// SQLトレース採取用接続文字列を記録しない
        /// </summary>
        static public bool DefaultSQLTraceInhConnectStringLogging = false;
        /// <summary>
        /// SQLトレース採取用接続文字列を記録しない
        /// </summary>
        public bool SQLTraceInhConnectStringLogging
        {
            get
            {
                if (!this.m_SQLTraceInhConnectStringLogging.HasValue)
                    return DefaultSQLTraceInhConnectStringLogging;
                return this.m_SQLTraceInhConnectStringLogging.Value;
            }
            set
            {
                this.m_SQLTraceInhConnectStringLogging = value;
            }
        }
        bool? m_SQLTraceInhConnectStringLogging = false;
        /// <summary>
        /// SQLトレースインスタンス
        /// </summary>
        protected Logger traceLog;
        /// <summary>
        /// トレース編集排他制御用オブジェクト
        /// </summary>
        protected static readonly  object traceEditLock = new object();
        #region パラメータログに関する設定
        #region インスタンス毎のパラメータログに関する設定
        /// <summary>
        /// パラメータログを採取する
        /// </summary>
        protected bool m_ParameterLogEnabled = true;
        /// <summary>
        /// パラメータログにデータセットを採取する
        /// </summary>
        protected bool m_ParameterLogDataSetEnabled = true;
        /// <summary>
        /// パラメータログにImageのダンプを採取する
        /// </summary>
        protected bool m_ParameterLogImageEnabled = true;
        /// <summary>
        /// パラメータログにTextのダンプを採取する
        /// </summary>
        protected bool m_ParameterLogTextEnabled = true;
        /// <summary>
        /// パラメータログにStructureのダンプを採取する
        /// </summary>
        protected bool m_ParameterLogStructureEnabled = true;
        /// <summary>
        /// パラメータログ出力時に内容をマスクするパラメータ名判別用正規表現
        /// </summary>
        protected string m_ParameterMaskRegexDeine = string.Empty;
        /// <summary>
        /// パラメータログ出力時に内容をマスクするパラメータ名判別用正規表現インスタンス
        /// </summary>
        protected Regex m_ParameterMaskRegex;
        /// <summary>
        /// パラメータログを採取する
        /// </summary>
        public bool ParameterLogEnabled
        {
            get
            {
                return this.m_ParameterLogEnabled;
            }
            set
            {
                this.m_ParameterLogEnabled = value;
            }
        }
        /// <summary>
        /// パラメータログにデータセットを採取する
        /// </summary>
        public bool ParameterLogDataSetEnabled
        {
            get
            {
                return this.m_ParameterLogDataSetEnabled;
            }
            set
            {
                this.m_ParameterLogDataSetEnabled = value;
            }
        }
        /// <summary>
        /// パラメータログにImageのダンプを採取する
        /// </summary>
        public bool ParameterLogImageEnabled
        {
            get
            {
                return this.m_ParameterLogImageEnabled;
            }
            set
            {
                this.m_ParameterLogImageEnabled = value;
            }
        }
        /// <summary>
        /// パラメータログにTextのダンプを採取する
        /// </summary>
        public bool ParameterLogTextEnabled
        {
            get
            {
                return this.m_ParameterLogTextEnabled;
            }
            set
            {
                this.m_ParameterLogTextEnabled = value;
            }
        }
        /// <summary>
        /// パラメータログにStructureのダンプを採取する
        /// </summary>
        public bool ParameterLogStructureEnabled
        {
            get
            {
                return this.m_ParameterLogStructureEnabled;
            }
            set
            {
                this.m_ParameterLogStructureEnabled = value;
            }
        }
        /// <summary>
        /// パラメータログ出力時に内容をマスクするパラメータ名判別用正規表現
        /// </summary>
        public string ParameterMaskRegex
        {
            get
            {
                return this.m_ParameterMaskRegexDeine;
            }
            set
            {
                this.m_ParameterMaskRegexDeine = value;
                this.m_ParameterMaskRegex = new Regex(value);
            }
        }

        #endregion

        #endregion
        #endregion
        #endregion
        #region アダプタ用インスタンス参照プロパティ
        /// <summary>
        /// アダプタインスタンス
        /// </summary>
        protected DBDataAdapter m_sqlDataAdapter = null;
        /// <summary>
        ///アダプタインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        protected abstract DBDataAdapter CreateDBAdapter();
        /// <summary>
        /// アダプタインスタンスを初期化する
        /// </summary>
        protected void InitializeAdapter()
        {
            this.m_sqlDataAdapter = this.CreateDBAdapter();
        }
        /// <summary>
        /// アダプタインスタンス
        /// </summary>
        public DBDataAdapter sqlDataAdapter
        {
            get
            { return this.m_sqlDataAdapter; }
            set
            {
                this.m_sqlDataAdapter = value;
                SetupDataCommand(this.m_sqlDataAdapter.SelectCommand as DBCommand);
                SetupDataCommand(this.m_sqlDataAdapter.InsertCommand as DBCommand);
                SetupDataCommand(this.m_sqlDataAdapter.UpdateCommand as DBCommand);
                SetupDataCommand(this.m_sqlDataAdapter.DeleteCommand as DBCommand);
            }
        }
        /// <summary>
        /// Select用SqlCommandインスタンス
        /// </summary>
        public DBCommand SelectSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.SelectCommand as DBCommand; }
            set
            {
                SetupDataCommand(this.m_sqlDataAdapter.SelectCommand as DBCommand, value);
                this.m_sqlDataAdapter.SelectCommand = value;
            }
        }
        /// <summary>
        /// Insert用SqlCommandインスタンス
        /// </summary>
        public DBCommand InsertSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.InsertCommand as DBCommand; }
            set
            {
                SetupDataCommand(this.m_sqlDataAdapter.InsertCommand as DBCommand, value);
                this.m_sqlDataAdapter.InsertCommand = value;
            }
        }
        /// <summary>
        /// Update用SqlCommandインスタンス
        /// </summary>
        public DBCommand UpdateSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.UpdateCommand as DBCommand; }
            set
            {
                SetupDataCommand(this.m_sqlDataAdapter.UpdateCommand as DBCommand, value);
                this.m_sqlDataAdapter.UpdateCommand = value;
            }
        }
        /// <summary>
        /// Delete用SqlCommandインスタンス
        /// </summary>
        public DBCommand DeleteSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.DeleteCommand as DBCommand; }
            set
            {
                SetupDataCommand(this.m_sqlDataAdapter.DeleteCommand as DBCommand, value);
                this.m_sqlDataAdapter.DeleteCommand = value;
            }
        }
        /// <summary>
        /// データセット更新時の詳細ログを採取する
        /// </summary>
        public bool UpdateRowLogEnable = false;
        #endregion
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataInstance()
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            GetDataCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">DataCommandの初期化可否</param>
        public DataInstance(bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public DataInstance(string DBConnectString)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            GetDataCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">DataCommand実行タイマー</param>
        public DataInstance(string DBConnectString, int DBCommandTimer)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            GetDataCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">DataCommandの初期化可否</param>
        public DataInstance(string DBConnectString, bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();                
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">DataCommand実行タイマー</param>
        /// <param name="fCommandInitialize">DataCommandの初期化可否</param>
        public DataInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();
            }
        }

         /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public DataInstance(DBDataAdapter adapter)
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            GetDataCommand();
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DataInstance(DBDataAdapter adapter, bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();
            }           
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public DataInstance(DBDataAdapter adapter, string DBConnectString)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            GetDataCommand();
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public DataInstance(DBDataAdapter adapter, string DBConnectString, int DBCommandTimer)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            GetDataCommand();
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DataInstance(DBDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();
            }
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DataInstance(DBDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetDataCommand();
            }
            this.sqlDataAdapter = adapter;
        }

        /// <summary>
        /// 初期設定
        /// </summary>
        private void Initialized()
        {
            this.traceLog = new Logger(this.SQLTraceManagerKey);
            this.m_ParameterLogEnabled = DB.Settings.Default.ParameterLogEnabled;
            this.m_ParameterLogImageEnabled = DB.Settings.Default.ParameterLogImageEnabled;
            this.m_ParameterLogDataSetEnabled = DB.Settings.Default.ParameterLogDataSetEnabled;
            this.m_ParameterLogTextEnabled = DB.Settings.Default.ParameterLogTextEnabled;
            this.m_ParameterLogStructureEnabled = DB.Settings.Default.ParameterLogStructureEnabled;
            this.ParameterMaskRegex = DB.Settings.Default.ParameterMaskRegex;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        internal DataInstance(DATACONFIG config)
        {
            Initialized();
            this.SQLTraceEnable = config.Common.SqlLogEnabled;
            this.SQLTraceManagerKey = config.Common.SqlLogSectionName;
            this.SQLTraceLogLevel = config.Common.LogLevel;
            this.SQLTraceInhConnectStringLogging = config.Common.SQLTraceInhConnectStringLogging;
            this.ParameterLogEnabled = config.Common.ParameterLogEnabled;
            this.ParameterLogDataSetEnabled = config.Common.ParameterLogDataSetEnabled;
            this.ParameterLogImageEnabled = config.Common.ParameterLogImageEnabled;
            this.ParameterLogTextEnabled = config.Common.ParameterLogTextEnabled;
            this.ParameterLogStructureEnabled = config.Common.ParameterLogStructureEnabled;
            this.ParameterMaskRegex = config.Common.ParameterMaskRegex;

            this.DBCommandTimer = config.Common.CommandTimeout;
            GetSqlConnection(config.Common.ConnectString);
            GetDataCommand();
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~DataInstance()
        {
            this.Dispose(false);
        }
        #endregion
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
                this.ReleaseSqlResource();
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
        #region TRACELOGメソッド
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="exp">例外情報</param>
        /// <param name="prms">その他パラメータ</param>
        public void InnerLogExp(int depth, DataTraceLogCode Code, string msg, Exception exp, params object[] prms)
        {
            if (!SQLTraceEnable) return;
            lock (traceEditLock)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                traceLog.LogStore<object>(SQLTraceLogLevel, Code, string.Format("{0:000000}:{1}", this.InstanceID, string.Format(msg, prms)), new System.Diagnostics.StackFrame(depth, true), exp, null, 0);
            }
        }
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        public void InnerLog(int depth, DataTraceLogCode Code, string msg, params object[] prms)
        {
            if (!SQLTraceEnable)    return;
            this.InnerLogExp(depth + 1,Code, msg, null, prms);
        }
        /// <summary>
        /// 外部モジュールからの呼び出しネスト数を取得する
        /// </summary>
        /// <returns></returns>
        protected int GetCallNestDepth()
        {
            if (!SQLTraceEnable) return 0;
            StackTrace stacktrace = new StackTrace(2);
            int nest=2;
            foreach (StackFrame stack in stacktrace.GetFrames())
            {
                MethodBase method = stack.GetMethod();
                if  (!method.DeclaringType.Namespace.Equals(this.GetType().Namespace))
                    break;
                nest++;
            }
            return nest;
        }
        /// <summary>
        /// 要求ステートメントとパラメータをロギングする
        /// </summary>
        public void LogStatement()
        {
            this.LogStatement(2);
        }
        /// <summary>
        /// 要求ステートメントとパラメータをロギングする
        /// </summary>
        public void LogStatement(int depth)
        {
            this.LogStatement(depth + 1,this.sqlCommand);
        }
        /// <summary>
        /// 要求ステートメントとパラメータをロギングする
        /// </summary>
        /// <remarks>
        /// structure型をそのままログキューに渡した場合書式化する時点で
        /// データ元のDataTableが解放されてしまう可能性がある為文字列化を
        /// 行ってキューを行う仕様に変更する
        /// これに伴いパラメータの一部をImageやDataTableのログを除外するオプションを追加1
        /// </remarks>
        public void LogStatement(int depth,DBCommand prmDataCommand)
        {
            if (!SQLTraceEnable) return;
            lock (traceEditLock)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
            }
            StringBuilder stb = new StringBuilder();
            if (this.ParameterLogEnabled)
            {
                foreach (DBParamater var in prmDataCommand.Parameters)
                {
                    this.QueryParameterToLogStringItem(ref stb, var, var.Value);
                }
            }
            lock (traceEditLock)
            {
                traceLog.LogStore<string>(SQLTraceLogLevel, DataTraceLogCode.Statement, string.Format("{0:000000}:{1}", this.InstanceID, prmDataCommand.CommandText), new System.Diagnostics.StackFrame(depth, true), null, stb.ToString(), stb.Length);
            }
        }

        /// <summary>
        /// ログ編集用にSqlパラメータを文字列バッファに編集して格納する。
        /// </summary>
        /// <param name="stb"></param>
        /// <param name="var"></param>
        /// <param name="Value"></param>
        protected virtual void QueryParameterToLogStringItem(ref StringBuilder stb, DBParamater var, object Value)
        {
            if (Value == null || Value.Equals(DBNull.Value))    //  2012/03/01 bugfix
                stb.AppendFormat("\r\n{0}:{1}:NULL", var.ParameterName, var.DbType);
            else
            {
                if (this.ParameterMaskRegex.Length != 0)
                {
                    if (this.m_ParameterMaskRegex.IsMatch(var.ParameterName))
                    {   //  一致.出力をマスクする
                        stb.AppendFormat("\r\n{0}:{1}:*", var.ParameterName, var.DbType);
                        return;
                    }
                }
                switch (var.DbType)
                {
                    case System.Data.DbType.Binary:                 //	1 から 8,000 バイトの範囲内のバイナリ データの可変長ストリーム。
                        stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.DbType, cklib.Util.String.HexDumpList((byte[])Value));
                        break;
                    case System.Data.DbType.AnsiString:             //  	1 から 8,000 文字の範囲内の非 Unicode 文字の可変長ストリーム。
                    case System.Data.DbType.AnsiStringFixedLength:  //	非 Unicode 文字の固定長ストリーム。
                    case System.Data.DbType.Boolean:	            //	true または false のブール値を表す単純型。
                    case System.Data.DbType.Byte:	                //	値が 0 から 255 までの範囲内の 8 ビット符号なし整数。
                    case System.Data.DbType.Currency:	            //	精度が通貨単位の 1/10,000 の、-2 63 (-922,337,203,685,477.5808) から 2 63 -1 (+922,337,203,685,477.5807) までの範囲内の通貨値。
                    case System.Data.DbType.Date:	                //	日付の値を表す型。
                    case System.Data.DbType.DateTime:	            //	日時の値を表す型。
                    case System.Data.DbType.DateTime2:	            //	日付と時刻のデータ。 日付の値の範囲は、AD 1 年 1 月 1 日から AD 9999 年 12 月 31 日です。 時刻の値の範囲は、00:00:00 から 23:59:59.9999999 で、精度は 100 ナノ秒です。
                    case System.Data.DbType.DateTimeOffset:	        //	タイム ゾーンに対応した日付と時刻。 日付の値の範囲は、AD 1 年 1 月 1 日から AD 9999 年 12 月 31 日です。 時刻の値の範囲は、00:00:00 から 23:59:59.9999999 で、精度は 100 ナノ秒です。 タイム ゾーンの値の範囲は、-14:00 から +14:00 です。
                    case System.Data.DbType.Decimal:	            //	1.0 × 10 -28 から概数 7.9 × 10 28 までの範囲で、有効桁数が 28 または 29 の値を表す単純型。
                    case System.Data.DbType.Double:	                //	概数 5.0 × 10 -324 から 1.7 × 10 308 までの範囲で、有効桁数が 15 または 16 の値を表す浮動小数点型。
                    case System.Data.DbType.Guid:	                //	グローバル一意識別子 (GUID)。
                    case System.Data.DbType.Int16:	                //	-32768 から 32767 までの値を保持する符号付き 16 ビット整数を表す整数型。
                    case System.Data.DbType.Int32:	                //	-2147483648 から 2147483647 までの値を保持する符号付き 32 ビット整数を表す整数型。
                    case System.Data.DbType.Int64:	                //	-9223372036854775808 から 9223372036854775807 までの値を保持する符号付き 64 ビット整数を表す整数型。
                    case System.Data.DbType.Object:         	    //	別の DbType 値で明示的に表されていない参照型または値型を表す汎用型。
                    case System.Data.DbType.SByte:	                //	-128 から 127 までの値を保持する符号付き 8 ビット整数を表す整数型。
                    case System.Data.DbType.Single:	                //	概数 1.5 × 10 -45 から 3.4 × 10 38 までの範囲で、有効桁数が 7 の値を表す浮動小数点型。
                    case System.Data.DbType.String:	                //	Unicode 文字列を表す型。
                    case System.Data.DbType.StringFixedLength:	    //	Unicode 文字の固定長文字列。
                    case System.Data.DbType.Time:	                //	SQL Server の DateTime 値を表す型。 SQL Server の time 値を使用する場合は、Time を使用してください。
                    case System.Data.DbType.UInt16:	                //	0 から 65535 までの値を保持する符号なし 16 ビット整数を表す整数型。
                    case System.Data.DbType.UInt32:	                //	0 から 4294967295 までの値を保持する符号なし 32 ビット整数を表す整数型。
                    case System.Data.DbType.UInt64:	                //	0 から 18446744073709551615 までの値を保持する符号なし 64 ビット整数を表す整数型。
                    case System.Data.DbType.VarNumeric:	            //	可変長数値。
                    case System.Data.DbType.Xml:	                //  解析された XML ドキュメントまたは XML フラグメントの表現。
                    default:
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.DbType, Value);
                        break;
                }
            }
        }
        #endregion
        #region 基本メソッド
        /// <summary>
        /// 接続済みSqlConnectionインスタンスを取得する
        /// </summary>
        /// <returns>接続済みSqlConnectionインスタンス</returns>
        protected void GetSqlConnection(string DBConnectString)
        {
            try
            {
                if (SQLTraceInhConnectStringLogging)
                    this.InnerLog(GetCallNestDepth(), DataTraceLogCode.Connect, "CONNECT");
                else
                    this.InnerLog(GetCallNestDepth(), DataTraceLogCode.Connect, "CONNECT [{0}]", DBConnectString);
                this.m_dbConnection = this.CreateDBConnection(DBConnectString);
                //if (SQLTraceEnable)
                //    this.m_dbConnection.InfoMessage += new SqlInfoMessageEventHandler(m_sqlConnection_InfoMessage);
                this.m_dbConnection.Open();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// DB接続インスタンスを生成する
        /// </summary>
        /// <param name="DBConnectString"></param>
        /// <returns></returns>
        protected abstract DBConnection CreateDBConnection(string DBConnectString); 
        /// <summary>
        /// DataCommandインスタンスの取得
        /// </summary>
        /// <returns>DataCommandインスタンス</returns>
        public void GetDataCommand()
        {
            try
            {
                this.m_dbCommand = this.dbConnection.CreateCommand() as DBCommand;
                if (this.DBCommandTimer != -1)
                {
                    this.m_dbCommand.CommandTimeout = this.DBCommandTimer;
                }
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">ロックレベルの指定</param>
        /// <returns>SqlTransactionインスタンス</returns>
        public DBTransaction BeginTransaction(IsolationLevel iso)
        {
            try
            {
                DBTransaction sqlTransaction = this.dbConnection.BeginTransaction(iso) as DBTransaction;
                sqlCommand.Transaction = sqlTransaction;
                return sqlTransaction;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションの開始(規定レベル)
        /// </summary>
        /// <returns>SqlTransactionインスタンス</returns>
        public DBTransaction BeginTransaction()
        {
            return  this.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">ロックレベルの指定</param>
        /// <returns>DBTransactionインスタンス</returns>
        public virtual Data_Transaction BeginDBTransaction(IsolationLevel iso)
        {
            try
            {
                this.InnerLog(GetCallNestDepth(), DataTraceLogCode.BeginTransaction, "BEGIN TRANSACTION [{0}]", iso);
                this.m_DBTransaction = this.CreateDBTransaction(this.BeginTransaction(iso), this);
                if (this.m_sqlDataAdapter != null)
                {
                    if (this.m_sqlDataAdapter.SelectCommand != null)
                        this.m_sqlDataAdapter.SelectCommand.Transaction = this.m_DBTransaction.Transaction;
                    if (this.m_sqlDataAdapter.InsertCommand != null)
                        this.m_sqlDataAdapter.InsertCommand.Transaction = this.m_DBTransaction.Transaction;
                    if (this.m_sqlDataAdapter.UpdateCommand != null)
                        this.m_sqlDataAdapter.UpdateCommand.Transaction = this.m_DBTransaction.Transaction;
                    if (this.m_sqlDataAdapter.DeleteCommand != null)
                        this.m_sqlDataAdapter.DeleteCommand.Transaction = this.m_DBTransaction.Transaction;
                }
                return this.m_DBTransaction;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションクラスインスタンス生成
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <param name="dbInstanc"></param>
        /// <returns></returns>
        protected abstract Data_Transaction CreateDBTransaction(DBTransaction dbTransaction, IDataInstanceInnerLogging dbInstanc);
        /// <summary>
        /// トランザクションの開始(規定レベル)
        /// </summary>
        /// <returns>SqlTransactionインスタンス</returns>
        public Data_Transaction BeginDBTransaction()
        {
            return this.BeginDBTransaction(IsolationLevel.ReadCommitted);
        }
        /// <summary>
        /// ExecuteNonQueryのラッパーメソッド
        /// </summary>
        /// <returns></returns>
        public int ExecuteNonQuery()
        {
            try
            {
                this.LogStatement(2);
                return this.m_dbCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteScalarのラッパーメソッド
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            try
            {
                this.LogStatement(2);
                return this.m_dbCommand.ExecuteScalar();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteReaderのラッパーメソッド
        /// </summary>
        /// <returns></returns>
        public DBDataReader ExecuteReader()
        {
            try
            {
                this.LogStatement(2);
                return this.m_dbCommand.ExecuteReader() as DBDataReader;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteReaderのラッパーメソッド
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public DBDataReader ExecuteReader(CommandBehavior behavior)
        {
            try
            {
                this.LogStatement(2);
                return this.m_dbCommand.ExecuteReader(behavior) as DBDataReader;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }

        /// <summary>
        /// SQL接続リソースの開放
        /// </summary>
        public void ReleaseSqlResource()
        {
            if (this.m_sqlDataAdapter != null)
            {
                try
                {
                    this.m_sqlDataAdapter.Dispose();
                }
                catch
                { }
                finally
                {
                    this.m_sqlDataAdapter = null;
                }
            }
            if (this.m_DBTransaction != null)
            {
                try
                {
                    this.m_DBTransaction.Dispose();
                }
                catch
                { }
                finally
                {
                    this.m_DBTransaction = null;
                }
            }
            if (this.m_dbCommand!=null)
            {
                try
                {
                    this.m_dbCommand.Dispose();
                }
                catch
                { }
                finally
                {
                    this.m_dbCommand = null;
                }
            }
            if (this.m_dbConnection!= null)
            {
                this.InnerLog(4, DataTraceLogCode.Close, "CLOSE");
                try
                {
                    this.m_dbConnection.Dispose();
                }
                catch
                {}
                finally
                {
                    this.m_dbConnection = null;
                }
            }
        }
        #endregion
        #region データ読み込み処理
        /// <summary>
        /// DataTableにデータをロードする
        /// </summary>
        /// <param name="depth">呼び出し段数</param>
        /// <param name="dt">データテーブル</param>
        /// <returns>正常に追加または更新された行数</returns>
        protected int Fill(int depth, DataTable dt)
        {
            try
            {
                this.LogStatement(depth, SelectSqlCommand);
                return this.sqlDataAdapter.Fill(dt);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(depth, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// DataTableにデータをロードする
        /// </summary>
        /// <returns>ロードされたデータセット</returns>
        public int Fill(DataTable dt)
        {
            return this.Fill(3, dt);
        }

        /// <summary>
        /// 指定したDataTableにデータをロードする
        /// </summary>
        /// <typeparam name="T">オーバーロードされたデータセットを指定</typeparam>
        /// <returns>ロードされたDataTable</returns>
        public T GetData<T>()
            where T : class, new()
        {
            T dt = new T();
            this.Fill(3, dt as DataTable);
            return dt;
        }
        /// <summary>
        /// データセットにデータをロードする
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Fill(DataSet ds)
        {
            try
            {
                this.LogStatement(2, SelectSqlCommand);
                return this.sqlDataAdapter.Fill(ds);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// データセットにデータをロードする
        /// </summary>
        /// <param name="ds">データセット</param>
        /// <param name="srcTable">テーブル マップに使用するソース テーブルの名前。</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Fill(DataSet ds, string srcTable)
        {
            try
            {
                this.LogStatement(2, SelectSqlCommand);
                return this.sqlDataAdapter.Fill(ds, srcTable);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        #endregion
        #region データ更新処理
        /// <summary>
        /// DataTableからテーブル更新
        /// </summary>
        /// <param name="dt">データテーブル</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Update(DataTable dt)
        {
            try
            {
                this.InnerLog(2, DataTraceLogCode.Statement, "Update Adapter From DataTable");
                this.LogAdapterUpdateStatement(2, dt.Rows);
                return this.sqlDataAdapter.Update(dt);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// DataSetからテーブル更新
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="srcTable">テーブル マップに使用するソース テーブルの名前。</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Update(DataSet ds, string srcTable)
        {
            try
            {
                this.InnerLog(2, DataTraceLogCode.Statement, "Update Adapter From DatSet");
                foreach (DataTable dt in ds.Tables)
                {
                    this.LogAdapterUpdateStatement(2, dt.Rows);
                }
                return this.sqlDataAdapter.Update(ds, srcTable);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }

        /// <summary>
        /// DataSetからテーブル更新
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Update(DataSet ds)
        {
            try
            {
                this.InnerLog(2, DataTraceLogCode.Statement, "Update Adapter From DatSet");
                foreach (DataTable dt in ds.Tables)
                {
                    this.LogAdapterUpdateStatement(2, dt.Rows);
                }
                return this.sqlDataAdapter.Update(ds);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// DataRowからテーブル更新
        /// </summary>
        /// <param name="drows">DataRowの配列</param>
        /// <returns>正常に追加または更新された行数</returns>
        public int Update(DataRow[] drows)
        {
            try
            {
                this.InnerLog(2, DataTraceLogCode.Statement, "Update Adapter From DatSet");
                this.LogAdapterUpdateStatement(2, drows);
                return this.sqlDataAdapter.Update(drows);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// Adapterの更新ステートメントのログを採取する
        /// </summary>
        /// <param name="depth">呼び出しのネスト段数</param>
        /// <param name="rows">行配列</param>
        protected void LogAdapterUpdateStatement(int depth, DataRow[] rows)
        {
            if (!SQLTraceEnable) return;
            lock (traceEditLock)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                foreach (DataRow row in rows)
                {
                    LogAdapterUpdateStatement(depth + 1, row);
                }
            }
        }
        /// <summary>
        /// Adapterの更新ステートメントのログを採取する
        /// </summary>
        /// <param name="depth">呼び出しのネスト段数</param>
        /// <param name="rows">行コレクション</param>
        protected void LogAdapterUpdateStatement(int depth, DataRowCollection rows)
        {
            if (!SQLTraceEnable) return;
            lock (traceEditLock)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                foreach (DataRow row in rows)
                {
                    LogAdapterUpdateStatement(depth + 1, row);
                }
            }
        }
        /// <summary>
        /// Adapterの更新ステートメントのログを採取する
        /// </summary>
        /// <param name="depth">呼び出しのネスト段数</param>
        /// <param name="row">行</param>
        protected void LogAdapterUpdateStatement(int depth, DataRow row)
        {
            if (!SQLTraceEnable) return;
            lock (traceEditLock)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
            }
            if (!this.ParameterLogDataSetEnabled)
                return;
            DBCommand cmd = null;
            switch (row.RowState)
            {
                case DataRowState.Added: cmd = this.InsertSqlCommand; break;
                case DataRowState.Modified: cmd = this.UpdateSqlCommand; break;
                case DataRowState.Deleted: cmd = this.DeleteSqlCommand; break;
                default: break;
            }
            if (cmd == null)
            {
                return;
            }
            StringBuilder stb = new StringBuilder();
            if (this.ParameterLogEnabled)
            {
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    this.QueryParameterToLogStringItem(ref stb, cmd.Parameters[i] as DBParamater, row[cmd.Parameters[i].SourceColumn, cmd.Parameters[i].SourceVersion]);
                }
            }
            lock (traceEditLock)
            {
                traceLog.LogStore<string>(SQLTraceLogLevel, DataTraceLogCode.Statement, string.Format("{0:000000}:{1}", this.InstanceID, cmd.CommandText), new System.Diagnostics.StackFrame(depth, true), null, stb.ToString(), stb.Length);
            }
        }
        #endregion
        #region ﾕｰﾃｨﾘﾃｨ
        /// <summary>
        /// パラメータ化クエリ未使用パラメータの削除
        /// </summary>
        public void PurgeNoUseParameters()
        {
            for (int i = 0; i < this.sqlCommand.Parameters.Count; )
            {
                if (this.sqlCommand.Parameters[i].Value == null)
                {
                    this.sqlCommand.Parameters.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }
        #endregion
    }
}
