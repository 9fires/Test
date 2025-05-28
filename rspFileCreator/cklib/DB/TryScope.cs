using System;
using System.Collections.Generic;
#if __net35__ || __net4__ || __net45__
using System.Linq;
#endif
using System.Text;
using System.Data;
using System.Data.SqlClient;
using cklib.Log;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Reflection;

namespace cklib.DB
{
    /// <summary>
    /// DB処理スコープ制御
    /// </summary>
    public class TryScope<DBCONNECT>
        where DBCONNECT:DBInstance,new()
    {
        #region ログ関連定義
        #region static定義
        /// <summary>
        /// デフォルトのエラーログコード
        /// </summary>
        public static int DefaultErrorLogCode = 0;
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public static int DefaultDBConnectErrorLogCode = 0;
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public static int DefaultDBErrorLogCode = 0;
        /// <summary>
        /// デフォルトのデッドロックエラーログコード
        /// </summary>
        public static int DefaultDeadLockErrorLogCode = 0;
        /// <summary>
        /// デフォルトのデッドロックリトライオーバーログコード
        /// </summary>
        public static int DefaultDeadLockRetryOverErrorLogCode = 0;
        /// <summary>
        /// デフォルトのコマンドタイムアウトエラーログコード
        /// </summary>
        public static int DefaultTimeoutErrorLogCode = 0;
        /// <summary>
        /// デフォルトのタイムアウトリトライオーバーログコード
        /// </summary>
        public static int DefaultTimeoutRetryOverErrorLogCode = 0;
        /// <summary>
        /// エラーにより処理を終結するログ出力のログレベル
        /// </summary>
        public static LogLevel DefaultLogLevelErrorTerminate = LogLevel.ERROR;
        /// <summary>
        /// エラーにより処理を再思考するログ出力のログレベル
        /// </summary>
        public static LogLevel DefaultLogLevelErrorRetry = LogLevel.NOTE;
        /// <summary>
        /// ログ採取マネージャーキー
        /// </summary>
        public static string DefaultLogManagerKey = LogManagerEx.DefaultManagerKey;
        #endregion
        #region インスタンス毎情報
        /// <summary>
        /// デフォルトのエラーログコード
        /// </summary>
        public virtual int ErrorLogCode
        {
            get
            {
                return DefaultErrorLogCode;
            }
        }
        /// <summary>
        /// DBエラーログコード
        /// </summary>
        public virtual int DBConnectErrorLogCode
        {
            get
            {
                return DefaultDBConnectErrorLogCode;
            }
        }
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        public virtual int DBErrorLogCode
        {
            get
            {
                return DefaultDBErrorLogCode;
            }
        }
        /// <summary>
        /// デッドロックエラーログコード
        /// </summary>
        public virtual int DeadLockErrorLogCode
        {
            get
            {
                return DefaultDeadLockErrorLogCode;
            }
        }
        /// <summary>
        /// デッドロックリトライオーバーログコード
        /// </summary>
        public virtual int DeadLockRetryOverErrorLogCode
        {
            get
            {
                return DefaultDeadLockRetryOverErrorLogCode;
            }
        } 
        /// <summary>
        /// コマンドタイムアウトエラーログコード
        /// </summary>
        public virtual int TimeoutErrorLogCode
        {
            get
            {
                return DefaultTimeoutErrorLogCode;
            }
        }
        /// <summary>
        /// タイムアウトリトライオーバーログコード
        /// </summary>
        public virtual int TimeoutRetryOverErrorLogCode
        {
            get
            {
                return DefaultTimeoutRetryOverErrorLogCode;
            }
        }
        /// <summary>
        /// エラーにより処理を終結するログ出力のログレベル
        /// </summary>
        public virtual LogLevel LogLevelErrorTerminate
        {
            get
            {
                return DefaultLogLevelErrorTerminate;
            }
        }
        /// <summary>
        /// エラーにより処理を再思考するログ出力のログレベル
        /// </summary>
        public virtual LogLevel LogLevelErrorRetry
        {
            get
            {
                return DefaultLogLevelErrorRetry;
            }
        }
        /// <summary>
        /// ログ採取マネージャーキー
        /// </summary>
        public virtual string LogManagerKey
        {
            get
            {
                return DefaultLogManagerKey;
            }
        }
        /// <summary>
        /// ログインスタンス
        /// </summary>
        protected readonly Logger log = null;
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
        public TryScope()
        {
            this.log = new Log.Logger(this.GetType(), this.LogManagerKey);
        }
        #endregion

        #region ロギング制御メソッド
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="level">ログレベル</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        protected void InnerLog(int depth, LogLevel level, int Code, string msg, params object[] prms)
        {
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
        protected virtual string GetLogMessage(int LocCode)
        {
            return this.log.GetMessage(LocCode);
        }
        #endregion

        #region エラーハンドラ
        /// <summary>
        /// Sqlエラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        /// <returns>リトライ処理続行</returns>
        protected virtual bool SqlErrorHandler(SqlException exp)
        {
            if (exp.Number == 1205 && this.EnableDeadLockRetry)
            {
                if (this.m_DeadLockRetryCount < this.DeadLockRetryCountLimit)
                {
                    this.m_DeadLockRetryCount++;
                    this.InnerLog(1, this.LogLevelErrorRetry, this.DeadLockErrorLogCode, GetLogMessage(this.DeadLockErrorLogCode), exp);
                    System.Threading.Thread.Sleep(this.DeadLockRetryDelayTime);
                    return true;
                }
                this.InnerLog(1, this.LogLevelErrorTerminate, this.DeadLockRetryOverErrorLogCode, GetLogMessage(this.DeadLockRetryOverErrorLogCode), exp);
            }
            else
                if (exp.Number == -2 && this.EnableTimeoutRetry)
                {
                    if (this.m_TimoutRetryCount < this.TimoutRetryCountLimit)
                    {
                        this.m_TimoutRetryCount++;
                        this.InnerLog(1, this.LogLevelErrorRetry, this.TimeoutErrorLogCode, GetLogMessage(this.TimeoutErrorLogCode), exp);
                        System.Threading.Thread.Sleep(this.DeadLockRetryDelayTime);
                        return true;
                    }
                    this.InnerLog(1, this.LogLevelErrorTerminate, this.TimeoutRetryOverErrorLogCode, GetLogMessage(this.TimeoutRetryOverErrorLogCode), exp);
                }
                else
                {
                    this.InnerLog(1, this.LogLevelErrorTerminate, this.DBErrorLogCode, GetLogMessage(this.DBErrorLogCode), exp);
                }
            return false;
        }
        /// <summary>
        /// リトライ対象外Sqlエラー
        /// </summary>
        protected virtual void SqlNoRetryErrorHandler(SqlException exp)
        {
            this.InnerLog(1, this.LogLevelErrorTerminate, this.DBErrorLogCode, GetLogMessage(this.DBErrorLogCode), exp);
        }
        /// <summary>
        /// Sql接続エラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        protected virtual void SqlConnectErrorHandler(Exception exp)
        {
            this.InnerLog(1, this.LogLevelErrorTerminate, this.DBConnectErrorLogCode, GetLogMessage(this.DBConnectErrorLogCode), exp);
        }
        /// <summary>
        /// その他エラー
        /// </summary>
        /// <param name="exp">SQL例外情報</param>
        protected virtual void OtherErrorHandler(Exception exp)
        {
            this.InnerLog(1, this.LogLevelErrorTerminate, this.ErrorLogCode, GetLogMessage(this.ErrorLogCode), exp);
        }
 
        #endregion

        #region DB接続インスタンス生成処理
        /// <summary>
        /// DB接続インスタンスの初期化
        /// </summary>
        /// <returns></returns>
        protected virtual DBCONNECT DBConnectInstance()
        {
            return new DBCONNECT();
        }
        #endregion

#if __net35__ || __net4__ || __net45__
        #region DB処理スコープ(.NET3.5移行のみよう互換メソッド)
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <param name="iso">分離レベル</param>
        /// <returns>処理成否</returns>
        public bool Transaction(DBCONNECT db, Func<DBCONNECT, TryScope<DBCONNECT>, bool> tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
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
                sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

        #region DB処理スコープ終了処理
                        }
                        catch (SqlException exp)
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
                        finally
                        {
                        }
                    }
                }
                catch (SqlException exp)
                {
                    SqlNoRetryErrorHandler(exp);
                    return false;
                }
                catch (Exception exp)
                {
                    this.OtherErrorHandler(exp);
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
        public bool Transaction(Func<DBCONNECT, TryScope<DBCONNECT>, bool> tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
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
                sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

        #region DB処理スコープ終了処理
                                }
                                catch (SqlException exp)
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
                                finally
                                {
                                }
                            }
                        }
                        catch (SqlException exp)
                        {
                            SqlNoRetryErrorHandler(exp);
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
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <returns>処理成否</returns>
        public bool AutoTransaction(DBCONNECT db, Func<DBCONNECT, TryScope<DBCONNECT>, bool> tryScope)
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
                catch (SqlException exp)
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
        public bool AutoTransaction(Func<DBCONNECT, TryScope<DBCONNECT>, bool> tryScope)
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
                        catch (SqlException exp)
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
#else
        #region DB処理スコープデリゲート
        /// <summary>
        /// DB処理スコープデリゲート
        /// </summary>
        /// <param name="db"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public delegate bool Func(DBCONNECT db, TryScope<DBCONNECT> func);
        #endregion

        #region DB処理スコープ
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <param name="iso">分離レベル</param>
        /// <returns>処理成否</returns>
        public bool Transaction(DBCONNECT db, Func tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
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
                sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

            #region DB処理スコープ終了処理
                        }
                        catch (SqlException exp)
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
                        finally
                        {
                        }
                    }
                }
                catch (SqlException exp)
                {
                    SqlNoRetryErrorHandler(exp);
                    return false;
                }
                catch (Exception exp)
                {
                    this.OtherErrorHandler(exp);
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
        public bool Transaction(Func tryScope, IsolationLevel iso = IsolationLevel.ReadCommitted)
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
                sqlTransaction.Commit();
                return true;
            }
            else
            {
                return false;
            }

            #region DB処理スコープ終了処理
                                }
                                catch (SqlException exp)
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
                                finally
                                {
                                }
                            }
                        }
                        catch (SqlException exp)
                        {
                            SqlNoRetryErrorHandler(exp);
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
        /// <summary>
        /// DBアクセススコープ
        /// </summary>
        /// <param name="db">db接続インスタンス</param>
        /// <param name="tryScope">DBトランザクションスコープ</param>
        /// <returns>処理成否</returns>
        public bool AutoTransaction(DBCONNECT db, Func tryScope)
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
                catch (SqlException exp)
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
        public bool AutoTransaction(Func tryScope)
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
                        catch (SqlException exp)
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
#endif
    }
}
