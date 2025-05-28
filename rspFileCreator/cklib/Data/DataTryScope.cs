using System;
using System.Collections.Generic;
#if __net35__ || __net4__ || __net45__
using System.Linq;
#endif
using System.Text;
using System.Data;
using System.Data.Common;
using cklib.Log;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
#if __net20__
using cklib.Framework;
#endif

namespace cklib.Data
{
    /// <summary>
    /// DB処理スコープ制御
    /// </summary>
    public abstract class DataTryScope<DBCONNECT, Data_Transaction, DBException>
        where DBCONNECT:IDataInstanceTransaction<Data_Transaction>,IDisposable,new()
        where Data_Transaction : IDataTransaction
        where DBException : DbException
    {
        #region ログ関連定義
        #region static定義
        /// <summary>
        /// デフォルトのエラーログコード
        /// </summary>
        public static LogCodes DefaultErrorLogCode = LogCodes.SystemError;
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public static LogCodes DefaultDBConnectErrorLogCode = LogCodes.DBConnectError;
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public static LogCodes DefaultDBErrorLogCode = LogCodes.DBError;
        /// <summary>
        /// デフォルトのデッドロックエラーログコード
        /// </summary>
        public static LogCodes DefaultDeadLockErrorLogCode = LogCodes.DBDeadLock;
        /// <summary>
        /// デフォルトのデッドロックリトライオーバーログコード
        /// </summary>
        public static LogCodes DefaultDeadLockRetryOverErrorLogCode = LogCodes.DBDeadLockRetryOver;
        /// <summary>
        /// デフォルトのコマンドタイムアウトエラーログコード
        /// </summary>
        public static LogCodes DefaultTimeoutErrorLogCode = LogCodes.DBTimeout;
        /// <summary>
        /// デフォルトのタイムアウトリトライオーバーログコード
        /// </summary>
        public static LogCodes DefaultTimeoutRetryOverErrorLogCode = LogCodes.DBTimeoutRetryOver;
        /// <summary>
        /// ログ採取マネージャーキー
        /// </summary>
        public static string DefaultLogManagerKey = LogManagerEx.DefaultManagerKey;
        #endregion
        #region インスタンス毎情報
        /// <summary>
        /// デフォルトのエラーログコード
        /// </summary>
        public virtual LogCodes ErrorLogCode
        {
            get
            {
                if (this.m_ErrorLogCode == null)
                    return DefaultErrorLogCode;
                return this.m_ErrorLogCode;
            }
            set
            {
                this.m_ErrorLogCode = value;
            }
        }
        private LogCodes m_ErrorLogCode = null;
        /// <summary>
        /// DBエラーログコード
        /// </summary>
        public virtual LogCodes DBConnectErrorLogCode
        {
            get
            {
                if (this.m_DBConnectErrorLogCode == null)
                    return DefaultDBConnectErrorLogCode;
                return this.m_DBConnectErrorLogCode;
            }
            set
            {
                this.m_DBConnectErrorLogCode = value;
            }
        }
        private LogCodes m_DBConnectErrorLogCode = null;
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public virtual LogCodes DBErrorLogCode
        {
            get
            {
                if (this.m_DBErrorLogCode == null)
                    return DefaultDBErrorLogCode;
                return this.m_DBErrorLogCode;
            }
            set
            {
                this.m_DBErrorLogCode = value;
            }
        }
        private LogCodes m_DBErrorLogCode = null;
        /// <summary>
        /// デッドロックエラーログコード
        /// </summary>
        public virtual LogCodes DeadLockErrorLogCode
        {
            get
            {
                if (this.m_DeadLockErrorLogCode == null)
                    return DefaultDeadLockErrorLogCode;
                return this.m_DeadLockErrorLogCode;
            }
            set
            {
                this.m_DeadLockErrorLogCode = value;
            }
        }
        private LogCodes m_DeadLockErrorLogCode = null;
        /// <summary>
        /// デッドロックリトライオーバーログコード
        /// </summary>
        public virtual LogCodes DeadLockRetryOverErrorLogCode
        {
            get
            {
                if (this.m_DeadLockRetryOverErrorLogCode == null)
                    return DefaultDeadLockRetryOverErrorLogCode;
                return this.m_DeadLockRetryOverErrorLogCode;
            }
            set
            {
                this.m_DeadLockRetryOverErrorLogCode = value;
            }
        }
        private LogCodes m_DeadLockRetryOverErrorLogCode = null;
        /// <summary>
        /// コマンドタイムアウトエラーログコード
        /// </summary>
        public virtual LogCodes TimeoutErrorLogCode
        {
            get
            {
                if (this.m_TimeoutErrorLogCode == null)
                    return DefaultTimeoutErrorLogCode;
                return this.m_TimeoutErrorLogCode;
            }
            set
            {
                this.m_TimeoutErrorLogCode = value;
            }
        }
        private LogCodes m_TimeoutErrorLogCode = null;
        /// <summary>
        /// タイムアウトリトライオーバーログコード
        /// </summary>
        public virtual LogCodes TimeoutRetryOverErrorLogCode
        {
            get
            {
                if (this.m_TimeoutRetryOverErrorLogCode == null)
                    return DefaultTimeoutRetryOverErrorLogCode;
                return this.m_TimeoutRetryOverErrorLogCode;
            }
            set
            {
                this.m_TimeoutRetryOverErrorLogCode = value;
            }
        }
        private LogCodes m_TimeoutRetryOverErrorLogCode = null;
        /// <summary>
        /// ログ採取マネージャーキー
        /// </summary>
        public virtual string LogManagerKey
        {
            get
            {
                if (this.m_LogManagerKey.Length == 0)
                    return DefaultLogManagerKey;
                return this.m_LogManagerKey;
            }
            set
            {
                this.m_LogManagerKey = value;
            }
        }
        private string m_LogManagerKey= string.Empty;
        /// <summary>
        /// ログインスタンス
        /// </summary>
        protected readonly Logger log = null;
        /// <summary>
        /// ログに付与するパラメータ
        /// </summary>
        public void SetLogParams(params object[] prms)
        {
            this.LogParms = prms;
        }
        /// <summary>
        /// ログ埋め込みパラメータ
        /// </summary>
        protected object[] LogParms = null;
         
        #endregion
        #endregion

        #region リトライ関連定義
        #region static定義
        /// <summary>
        /// デッドロックリトライ可否
        /// </summary>
        public static bool DefaultEnableDeadLockRetry = true;
        /// <summary>
        /// デッドロックリトライ回数
        /// </summary>
        public static int DefaultDeadLockRetryCountLimit = 0;
        /// <summary>
        /// デッドロックリトライディレイ時間(ミリ秒）
        /// </summary>
        public static int DefaultDeadLockRetryDelayTime = 0;
        /// <summary>
        /// コマンドタイムアウトリトライ可否
        /// </summary>
        public static bool DefaultEnableTimeoutRetry = false;
        /// <summary>
        /// コマンドタイムアウトリトライ回数
        /// </summary>
        public static int DefaultTimoutRetryCountLimit = 0;
        /// <summary>
        /// コマンドタイムアウトディレイ時間(ミリ秒）
        /// </summary>
        public static int DefaultTimeoutRetryDelayTime = 0;
        #endregion
        #region インスタンス毎情報
        /// <summary>
        /// デッドロックリトライ可否
        /// </summary>
        public virtual bool EnableDeadLockRetry
        {
            get
            {
                if (this.m_EnableDeadLockRetrySetup)
                    return this.m_EnableDeadLockRetry;
                else
                    return DefaultEnableDeadLockRetry;
            }
            set
            {
                this.m_EnableDeadLockRetry = value;
                this.m_EnableDeadLockRetrySetup = true;
            }
        }
        /// <summary>
        /// デッドロックリトライ可否
        /// </summary>
        private bool m_EnableDeadLockRetry = true;
        /// <summary>
        /// デッドロックリトライ可否個別設定有効
        /// </summary>
        private bool m_EnableDeadLockRetrySetup = false;

        /// <summary>
        /// デッドロックリトライ回数
        /// </summary>
        public virtual int DeadLockRetryCountLimit
        {
            get
            {
                if (this.m_DeadLockRetryCountLimit < 0)
                    return DefaultDeadLockRetryCountLimit;
                else
                    return this.m_DeadLockRetryCountLimit;
            }
            set
            {
                this.m_DeadLockRetryCountLimit = value;
            }
        }
        /// <summary>
        /// デッドロックリトライ回数
        /// </summary>
        private int m_DeadLockRetryCountLimit = -1;
        /// <summary>
        /// デッドロックリトライ回数
        /// </summary>
        protected int m_DeadLockRetryCount = 0;
        /// <summary>
        /// デッドロックリトライディレイ時間(ミリ秒）
        /// </summary>
        public virtual int DeadLockRetryDelayTime
        {
            get
            {
                if (this.m_DeadLockRetryDelayTime < 0)
                    return DefaultDeadLockRetryDelayTime;
                else
                    return this.m_DeadLockRetryDelayTime;
            }
            set
            {
                this.m_DeadLockRetryDelayTime = value;
            }
        }
        /// <summary>
        /// デッドロックリトライディレイ時間(ミリ秒）
        /// </summary>
        private int m_DeadLockRetryDelayTime = -1;
        /// <summary>
        /// コマンドタイムアウトリトライ可否
        /// </summary>
        public virtual bool EnableTimeoutRetry
        {
            get
            {
                if (this.m_EnableTimeoutRetrySetup)
                    return this.m_EnableTimeoutRetry;
                else
                    return DefaultEnableTimeoutRetry;
            }
            set
            {
                this.m_EnableTimeoutRetry = value;
                this.m_EnableTimeoutRetrySetup = true;
            }
        }
        /// <summary>
        /// コマンドタイムアウトリトライ可否
        /// </summary>
        private bool m_EnableTimeoutRetry = true;
        /// <summary>
        /// コマンドタイムアウトリトライ可否個別設定有効
        /// </summary>
        private bool m_EnableTimeoutRetrySetup = false;
        /// <summary>
        /// コマンドタイムアウトリトライ回数
        /// </summary>
        public virtual int TimoutRetryCountLimit
        {
            get
            {
                if (this.m_TimoutRetryCountLimit < 0)
                    return DefaultTimoutRetryCountLimit;
                else
                    return this.m_TimoutRetryCountLimit;
            }
            set
            {
                this.m_TimoutRetryCountLimit = value;
            }
        }
        /// <summary>
        /// コマンドタイムアウトリトライ回数
        /// </summary>
        private int m_TimoutRetryCountLimit = -1;
        /// <summary>
        /// コマンドタイムアウトリトライ回数
        /// </summary>
        protected int m_TimoutRetryCount = 0;
        /// <summary>
        /// コマンドタイムアウトディレイ時間(ミリ秒）
        /// </summary>
        public virtual int TimeoutRetryDelayTime
        {
            get
            {
                if (this.m_TimeoutRetryDelayTime < 0)
                    return DefaultTimeoutRetryDelayTime;
                else
                    return this.m_TimeoutRetryDelayTime;
            }
            set
            {
                this.m_TimeoutRetryDelayTime = value;
            }
        }
        /// <summary>
        /// コマンドタイムアウトディレイ時間(ミリ秒）
        /// </summary>
        private int m_TimeoutRetryDelayTime = -1;
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataTryScope()
        {
            this.log = new Log.Logger(this.GetType(), this.LogManagerKey);
        }
        #endregion

        #region ロギング制御メソッド
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        protected void InnerLog(int depth, LogCodes Code, string msg, params object[] prms)
        {
            LogLevel level = log.GetLogLevel(Code);
            if (!log.IsValidLevel(level))
                return;
            Exception exp = null;
            if (prms.Length != 0)
            {
                if (prms[prms.Length - 1].GetType().IsSubclassOf(typeof(Exception)))
                {   //  例外の派生クラス
                    exp = (Exception)prms[prms.Length - 1];
                }
            }
            log.LogStore<object>(level, Code, string.Format(msg, prms), new System.Diagnostics.StackFrame(depth, true), exp, null, 0);
        }
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        protected void InnerLog(int depth, LogCodes Code, string msg, object[] prms, Exception exp)
        {
            LogLevel level = log.GetLogLevel(Code);
            if (!log.IsValidLevel(level))
                return;
            log.LogStore<object>(level, Code, prms != null ? string.Format(msg, prms) : msg, new System.Diagnostics.StackFrame(depth, true), exp, null, 0);
        }
        /// <summary>
        /// 外部モジュールからの呼び出しネスト数を取得する
        /// </summary>
        /// <returns></returns>
        protected int GetCallNestDepth()
        {
            StackTrace stacktrace = new StackTrace(2);
            int nest = 2;
            foreach (StackFrame stack in stacktrace.GetFrames())
            {
                MethodBase method = stack.GetMethod();
                if (!method.DeclaringType.Namespace.Equals(this.GetType().Namespace))
                    break;
                nest++;
            }
            return nest;
        }
        /// <summary>
        /// ログコードに対応したエラーメッセージを取得する
        /// </summary>
        /// <param name="LocCode"></param>
        /// <returns></returns>
        protected virtual string GetLogMessage(LogCodes LocCode)
        {
            return this.log.GetMessage(LocCode);
        }
        #endregion

        #region エラーハンドラ
        /// <summary>
        /// エラー情報
        /// </summary>
        public Exception LastErrorInformation
        {
            get
            {
                return this.m_LastErrorInformation;
            }
        }
        protected Exception m_LastErrorInformation = null;
        /// <summary>
        /// Sqlエラー処理ハンドラ型
        /// </summary>
        public delegate bool ErrorHandlerType(out bool result,Exception exp,DataTryScope<DBCONNECT, Data_Transaction, DBException> ts);
        /// <summary>
        /// Sql接続エラー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallBackSqlConnectErrorHandler = null;
        /// <summary>
        /// Sqlエラー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallBackSqlErrorHandler = null;
        /// <summary>
        /// Sqlエラー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallBackSqlNoRetryErrorHandler = null;
        /// <summary>
        /// デッドロックリトライ処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallDeadLockRetryHandler = null;
        /// <summary>
        /// デッドロックリトライオーバー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallDeadLockRetryOverHandler = null;
        /// <summary>
        /// タイマーリトライ処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallTimerRetryHandler = null;
        /// <summary>
        /// タイマーリトライオーバー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallTimerRetryOveHandler = null;
        /// <summary>
        /// その他エラー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallBackErrorHandler = null;
        /// <summary>
        /// トランザクション開始エラー処理ハンドラ
        /// </summary>
        public ErrorHandlerType CallBackTransactionStartErrorHandler = null;
        /// <summary>
        /// Commited処理ハンドラ
        /// </summary>
        public Action<DBCONNECT, DataTryScope<DBCONNECT, Data_Transaction, DBException>> CallBackCommitedHandler = null;
        /// <summary>
        /// Sqlエラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        /// <returns>リトライ処理続行</returns>
        protected virtual bool SqlErrorHandler(DBException exp)
        {
            var ret  = false;
            this.m_LastErrorInformation = exp;
            if (this.IsDeadLock(exp) && this.EnableDeadLockRetry)
            {
                if (this.m_DeadLockRetryCount < this.DeadLockRetryCountLimit)
                {
                    this.m_DeadLockRetryCount++;
                    if (CallDeadLockRetryHandler != null)
                    {
                        if (!CallDeadLockRetryHandler(out ret, exp, this))
                            return ret;
                    }
                    this.InnerLog(1, this.DeadLockErrorLogCode, GetLogMessage(this.DeadLockErrorLogCode), this.LogParms, exp);
                    System.Threading.Thread.Sleep(this.DeadLockRetryDelayTime);
                    return true;
                }
                if (CallDeadLockRetryOverHandler != null)
                {
                    if (!CallDeadLockRetryOverHandler(out ret, exp, this))
                        return ret;
                }
                this.InnerLog(1, this.DeadLockRetryOverErrorLogCode, GetLogMessage(this.DeadLockRetryOverErrorLogCode), this.LogParms, exp);
            }
            else
                if (this.IsTimeout(exp) && this.EnableTimeoutRetry)
                {
                    if (this.m_TimoutRetryCount < this.TimoutRetryCountLimit)
                    {
                        this.m_TimoutRetryCount++;
                        if (CallTimerRetryHandler != null)
                        {
                            if (!CallTimerRetryHandler(out ret, exp, this))
                                return ret;
                        }
                        this.InnerLog(1, this.TimeoutErrorLogCode, GetLogMessage(this.TimeoutErrorLogCode), this.LogParms, exp);
                        System.Threading.Thread.Sleep(this.DeadLockRetryDelayTime);
                        return true;
                    }
                    if (CallTimerRetryOveHandler != null)
                    {
                        if (!CallTimerRetryOveHandler(out ret, exp, this))
                            return ret;
                    }
                    this.InnerLog(1, this.TimeoutRetryOverErrorLogCode, GetLogMessage(this.TimeoutRetryOverErrorLogCode), this.LogParms, exp);
                }
                else
                {
                    if (CallBackSqlErrorHandler != null)
                    {
                        if (!CallBackSqlErrorHandler(out ret, exp, this))
                            return ret;
                    }
                    this.InnerLog(1, this.DBErrorLogCode, GetLogMessage(this.DBErrorLogCode), this.LogParms, exp);
                }
            return false;
        }
        /// <summary>
        /// DB例外からデッドロックを判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected abstract bool IsDeadLock(DBException exp);
        /// <summary>
        /// DB例外からタイムアウト例外を判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected abstract bool IsTimeout(DBException exp);

        /// <summary>
        /// リトライ対象外Sqlエラー
        /// </summary>
        protected virtual void SqlNoRetryErrorHandler(DBException exp)
        {
            this.m_LastErrorInformation = exp;
            if (CallBackSqlNoRetryErrorHandler != null)
            {
                bool ret;
                if (!CallBackSqlNoRetryErrorHandler(out ret, exp, this))
                    return;
            }
            this.InnerLog(1, this.DBErrorLogCode, GetLogMessage(this.DBErrorLogCode), this.LogParms, exp);
        }
        /// <summary>
        /// Sql接続エラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        protected virtual void SqlConnectErrorHandler(Exception exp)
        {
            this.m_LastErrorInformation = exp;
            if (CallBackSqlConnectErrorHandler != null)
            {
                bool ret;
                if (!CallBackSqlConnectErrorHandler(out ret, exp, this))
                    return;
            }
            this.InnerLog(1, this.DBConnectErrorLogCode, GetLogMessage(this.DBConnectErrorLogCode), this.LogParms, exp);
        }
        /// <summary>
        /// その他エラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        protected virtual bool OtherErrorHandler(Exception exp)
        {
            this.m_LastErrorInformation = exp;
            if (CallBackErrorHandler != null)
            {
                bool ret;
                if (!CallBackErrorHandler(out ret, exp, this))
                    return ret;
            }
            this.InnerLog(1, this.ErrorLogCode, GetLogMessage(this.ErrorLogCode), exp);
            return false;
        }
        /// <summary>
        /// トランザクション開始エラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        protected virtual void TransactionStartErrorHandler(Exception exp)
        {
            this.m_LastErrorInformation = exp;
            if (CallBackTransactionStartErrorHandler != null)
            {
                bool ret;
                if (!CallBackTransactionStartErrorHandler(out ret, exp, this))
                    return;
            }
            this.InnerLog(1, this.ErrorLogCode, GetLogMessage(this.ErrorLogCode), exp);
        }
 
        #endregion

        #region DB接続インスタンス生成処理
        /// <summary>
        /// DB接続インスタンスの初期化
        /// </summary>
        /// <returns></returns>
        protected virtual DBCONNECT DBConnectInstance()
        {
            if (this.NewDBConnectInstance != null)
                return this.NewDBConnectInstance(this);
            return new DBCONNECT();
        }
        /// <summary>
        /// DBインスタンス生成デリゲート
        /// </summary>
        internal Func<DataTryScope<DBCONNECT, Data_Transaction, DBException>, DBCONNECT> NewDBConnectInstance = null;
        #endregion

        #region DB処理スコープ
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <param name="iso">分離レベル</param>
        /// <returns>処理成否</returns>
        public bool Transaction(DBCONNECT db, Func<DBCONNECT, DataTryScope<DBCONNECT, Data_Transaction, DBException>, bool> tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
        {
            #region DB処理スコープ開始処理
            for (this.m_DeadLockRetryCount=0,this.m_TimoutRetryCount=0; ; )
            {
                try
                {
                    using (var sqlTransaction = db.BeginDBTransaction(iso))
                    {
                        try
                        {
            #endregion

            if (tryScope(db, this))
            {
                if (!sqlTransaction.IsCommited)
                    sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

            #region DB処理スコープ終了処理
                        }
                        catch (DBException exp)
                        {
                            if (this.SqlErrorHandler(exp))
                            {
                                continue;
                            }
                            return false;
                        }
                        catch (Exception exp)
                        {
                            if (this.OtherErrorHandler(exp))
                                continue;
                            return false;
                        }
                        finally
                        {
                            if (CallBackCommitedHandler != null)
                                CallBackCommitedHandler(db, this);
                        }
                    }
                }
                catch (DBException exp)
                {
                    SqlNoRetryErrorHandler(exp);
                    return false;
                }
                catch (Exception exp)
                {
                    this.TransactionStartErrorHandler(exp);
                    return false;
                }
            }
            #endregion
        }
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <remarks>
        /// リトライをDB再接続から行う<br/>
        /// DB接続はデフォルトとなる。<br/>
        /// 接続方法をカスタマイズする場合は<see cref="DBConnectInstance">DBConnectInstance</see>をオーバーライドする
        /// </remarks>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <param name="iso">分離レベル</param>
        /// <returns>処理成否</returns>
        public bool Transaction(Func<DBCONNECT, DataTryScope<DBCONNECT, Data_Transaction, DBException>, bool> tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
        {
        #region DB処理スコープ開始処理
            for (this.m_DeadLockRetryCount = 0, this.m_TimoutRetryCount = 0; ; )
            {
                try
                {
                    using (DBCONNECT db = this.DBConnectInstance())
                    {
                        try
                        {
                            using (var sqlTransaction = db.BeginDBTransaction(iso))
                            {
                                try
                                {
        #endregion

            if (tryScope(db, this))
            {
                if (!sqlTransaction.IsCommited)
                    sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

        #region DB処理スコープ終了処理
                                }
                                catch (DBException exp)
                                {
                                    if (this.SqlErrorHandler(exp))
                                    {
                                        continue;
                                    }
                                    return false;
                                }
                                catch (Exception exp)
                                {
                                    if (this.OtherErrorHandler(exp))
                                        continue;
                                    return false;
                                }
                                finally
                                {
                                    if (CallBackCommitedHandler != null)
                                        CallBackCommitedHandler(db, this);
                                }
                            }
                        }
                        catch (DBException exp)
                        {
                            SqlNoRetryErrorHandler(exp);
                            return false;
                        }
                        catch (Exception exp)
                        {
                            this.TransactionStartErrorHandler(exp);
                            return false;
                        }
                    }
                }
                catch (Exception exp)
                {
                    this.SqlConnectErrorHandler(exp);
                    return false;
                }
            }
        #endregion
        }
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <returns>処理成否</returns>
        public bool AutoTransaction(DBCONNECT db, Func<DBCONNECT, DataTryScope<DBCONNECT, Data_Transaction, DBException>, bool> tryScope)
        {
        #region DB処理スコープ開始処理
            for (this.m_DeadLockRetryCount = 0, this.m_TimoutRetryCount = 0; ; )
            {
                try
                {
        #endregion

            if (tryScope(db, this))
            {
                return true;
            }
            else
            {
                return false;
            }

        #region DB処理スコープ終了処理
                }
                catch (DBException exp)
                {
                    if (this.SqlErrorHandler(exp))
                    {
                        continue;
                    }
                    return false;
                }
                catch (Exception exp)
                {
                    if (this.OtherErrorHandler(exp))
                        continue;
                    return false;
                }
            }
        #endregion
        }
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <remarks>
        /// リトライをDB再接続から行う<br/>
        /// DB接続はデフォルトとなる。<br/>
        /// 接続方法をカスタマイズする場合は<see cref="DBConnectInstance">DBConnectInstance</see>をオーバーライドする
        /// </remarks>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <returns>処理成否</returns>
        public bool AutoTransaction(Func<DBCONNECT, DataTryScope<DBCONNECT, Data_Transaction, DBException>, bool> tryScope)
        {
        #region DB処理スコープ開始処理
            for (this.m_DeadLockRetryCount = 0, this.m_TimoutRetryCount = 0; ; )
            {
                try
                {
                    using (DBCONNECT db = this.DBConnectInstance())
                    {
                        try
                        {
        #endregion

            if (tryScope(db, this))
            {
                return true;
            }
            else
            {
                return false;
            }

        #region DB処理スコープ終了処理
                        }
                        catch (DBException exp)
                        {
                            if (this.SqlErrorHandler(exp))
                            {
                                continue;
                            }
                            return false;
                        }
                        catch (Exception exp)
                        {
                            this.OtherErrorHandler(exp);
                            return false;
                        }
                    }
                }
                catch (Exception exp)
                {
                    this.SqlConnectErrorHandler(exp);
                    return false;
                }
            }
        #endregion
        }
        #endregion
    }
}
