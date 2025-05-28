using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using cklib.Log;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace  cklib.DB
{
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス
    /// </summary>
    [Serializable]
    public class DBInstance : IDisposable
    {
        #region インスタンス参照プロパティ
        /// <summary>
        /// 接続インスタンス
        /// </summary>
        private SqlConnection m_sqlConnection = null;
        /// <summary>
        /// 接続インスタンス
        /// </summary>
        public SqlConnection sqlConnection
        {
            get
            { return this.m_sqlConnection; }
        }
        /// <summary>
        /// SqlCommandインスタンス
        /// </summary>
        private SqlCommand  m_sqlCommand   = null;
        /// <summary>
        /// SqlCommandインスタンス
        /// </summary>
        public SqlCommand sqlCommand
        {
            get
            { return this.m_sqlCommand; }
            set
            {
                SetupSqlCommand(this.m_sqlCommand, value);
                this.m_sqlCommand = value;
            }
        }
        /// <summary>
        /// SQLコマンドのセットアップ
        /// </summary>
        /// <param name="prmSqlCommand">格納先のSqlCommandインスタンス</param>
        /// <param name="value">新しいSQLCommandインスタンス</param>
        protected void SetupSqlCommand(SqlCommand prmSqlCommand, SqlCommand value)
        {
            this.ReleaseSqlCommand(prmSqlCommand);
            this.SetupSqlCommand(value);
        }
        /// <summary>
        /// SQLコマンドのセットアップ
        /// </summary>
        /// <param name="prmSqlCommand">格納先のSqlCommandインスタンス</param>
        protected void SetupSqlCommand(SqlCommand prmSqlCommand)
        {
            if (prmSqlCommand == null)
            {
                return;
            }
            prmSqlCommand.Connection = this.sqlConnection;
            if (this.DBTransaction != null)
            {
                prmSqlCommand.Transaction = this.DBTransaction.Transaction;
            }
            if (this.DBCommandTimer != -1)
            {
                prmSqlCommand.CommandTimeout = this.DBCommandTimer;
            }
        }
        /// <summary>
        /// SQLコマンドの開放
        /// </summary>
        /// <param name="prmSqlCommand">格納先のSqlCommandインスタンス</param>
        protected void ReleaseSqlCommand(SqlCommand prmSqlCommand)
        {
            if (prmSqlCommand != null)
            {
                try
                {
                    prmSqlCommand.Dispose();
                }
                catch
                { }
            }
        }
        /// <summary>
        /// DBトランザクションインスタンス
        /// </summary>
        private DBTransaction m_DBTransaction = null;
        /// <summary>
        /// DBトランザクションインスタンス
        /// </summary>
        public DBTransaction DBTransaction
        {
            get
            { return this.m_DBTransaction; }
        }
        /// <summary>
        /// SqlCommand実行タイマー
        /// </summary>
        private int m_DBCommandTimer = -1;
        /// <summary>
        /// SqlCommand実行タイマー
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
        static public bool m_SQLTraceEnable;
        /// <summary>
        /// SQLトレースログ許可
        /// </summary>
        static public bool SQLTraceEnable
        {
            get
            {
                return m_SQLTraceEnable;
            }
            set
            {
                if (!m_SQLTraceEnable && value)
                {
                    traceLog = new Logger(SQLTraceManagerKey);
                    m_SQLTraceEnable = true;
                }
                else
                {
                    lock (traceLog)
                    {
                        m_SQLTraceEnable = false;
                    }
                }
            }
        }
        /// <summary>
        /// SQLトレースログ採取マネージャーキー
        /// </summary>
        static public string SQLTraceManagerKey=LogManagerEx.DefaultManagerKey;
        /// <summary>
        /// SQLトレース採取用ログレベル
        /// </summary>
        static public LogLevel SQLTraceLogLevel=LogLevel.TRACE;
        /// <summary>
        /// SQLトレース採取用ログコード
        /// </summary>
        public const int SQLTraceDefaultLogCode=65000;
        /// <summary>
        /// SQLトレース採取用ログコード
        /// </summary>
        static public int SQLTraceLogCodeBase = SQLTraceDefaultLogCode;
        /// <summary>
        /// SQLトレース採取用接続文字列を記録しない
        /// </summary>
        static public bool SQLTraceInhConnectStringLogging = false;
        /// <summary>
        /// SQLトレースインスタンス
        /// </summary>
        static protected Logger traceLog;
        #region パラメータログに関する設定
        #region インスタンス毎のパラメータログに関する設定
        /// <summary>
        /// パラメータログを採取する
        /// </summary>
        private bool m_ParameterLogEnabled = true;
        /// <summary>
        /// パラメータログにデータセットを採取する
        /// </summary>
        private bool m_ParameterLogDataSetEnabled = true;
        /// <summary>
        /// パラメータログにImageのダンプを採取する
        /// </summary>
        private bool m_ParameterLogImageEnabled = true;
        /// <summary>
        /// パラメータログにTextのダンプを採取する
        /// </summary>
        private bool m_ParameterLogTextEnabled = true;
        /// <summary>
        /// パラメータログにStructureのダンプを採取する
        /// </summary>
        private bool m_ParameterLogStructureEnabled = true;
        /// <summary>
        /// パラメータログ出力時に内容をマスクするパラメータ名判別用正規表現
        /// </summary>
        private string m_ParameterMaskRegexDeine = string.Empty;
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
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DBInstance()
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            GetSqlCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBInstance(bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(ShareDBConnectString);
            if (fCommandInitialize)
            {
                GetSqlCommand();
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public DBInstance(string DBConnectString)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            GetSqlCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public DBInstance(string DBConnectString, int DBCommandTimer)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            GetSqlCommand();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBInstance(string DBConnectString, bool fCommandInitialize)
        {
            Initialized();
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetSqlCommand();                
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
        {
            Initialized();
            this.DBCommandTimer = DBCommandTimer;
            GetSqlConnection(DBConnectString);
            if (fCommandInitialize)
            {
                GetSqlCommand();
            }
        }
        /// <summary>
        /// 初期設定
        /// </summary>
        private void Initialized()
        {
            this.m_ParameterLogEnabled = DB.Settings.Default.ParameterLogEnabled;
            this.m_ParameterLogImageEnabled = DB.Settings.Default.ParameterLogImageEnabled;
            this.m_ParameterLogDataSetEnabled = DB.Settings.Default.ParameterLogDataSetEnabled;
            this.m_ParameterLogTextEnabled = DB.Settings.Default.ParameterLogTextEnabled;
            this.m_ParameterLogStructureEnabled = DB.Settings.Default.ParameterLogStructureEnabled;
            this.ParameterMaskRegex = DB.Settings.Default.ParameterMaskRegex;
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~DBInstance()
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
        internal void InnerLogExp(int depth, SQLTraceLogCode Code, string msg, Exception exp, params object[] prms)
        {
            if (!SQLTraceEnable) return;
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                traceLog.LogStore<object>(SQLTraceLogLevel, SQLTraceLogCodeBase+(int)Code, string.Format("{0:000000}:{1}", this.InstanceID, string.Format(msg, prms)), new System.Diagnostics.StackFrame(depth, true), exp, null, 0);
            }
        }
        /// <summary>
        /// 内部イベントのログ
        /// </summary>
        /// <param name="depth">スタックトレースの深さ</param>
        /// <param name="Code">ログコード</param>
        /// <param name="msg">メッセージ</param>
        /// <param name="prms">その他パラメータ</param>
        internal void InnerLog(int depth, SQLTraceLogCode Code, string msg, params object[] prms)
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
        /// SQLメッセージ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_sqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (!SQLTraceEnable) return;
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                StringBuilder stb = new StringBuilder();
                foreach (SqlError error in e.Errors)
                {
                    stb.AppendFormat("Sql Error Code:{0} Class:{1} State:{2} Message:{3} Procedure:{4} LineNumber:{5} Server:{6}\r\n",
                                        error.Number, error.Class, error.State, error.Message, error.Procedure, error.LineNumber, error.Server);
                }
                InnerLog(1,SQLTraceLogCode.Message, stb.ToString());
            }
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
        public void LogStatement(int depth,SqlCommand prmSqlCommand)
        {
            if (!SQLTraceEnable) return;
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
            }
            StringBuilder stb = new StringBuilder();
            if (this.ParameterLogEnabled)
            {
                foreach (SqlParameter var in prmSqlCommand.Parameters)
                {
                    this.SqlParameterToLogStringItem(ref stb, var, var.Value);
                }
            }
            lock (traceLog)
            {
                traceLog.LogStore<string>(SQLTraceLogLevel, SQLTraceLogCodeBase + (int)SQLTraceLogCode.Statement, string.Format("{0:000000}:{1}", this.InstanceID, prmSqlCommand.CommandText), new System.Diagnostics.StackFrame(depth, true), null, stb.ToString(), stb.Length);
            }
        }

        /// <summary>
        /// ログ編集用にSqlパラメータを文字列バッファに編集して格納する。
        /// </summary>
        /// <param name="stb"></param>
        /// <param name="var"></param>
        /// <param name="Value"></param>
        protected void SqlParameterToLogStringItem(ref StringBuilder stb, SqlParameter var,object Value)
        {
            if (Value == null || Value.Equals(DBNull.Value))    //  2012/03/01 bugfix
                stb.AppendFormat("\r\n{0}:{1}:NULL", var.ParameterName, var.SqlDbType);
            else
            {
                if (this.ParameterMaskRegex.Length != 0)
                {
                    if (this.m_ParameterMaskRegex.IsMatch(var.ParameterName))
                    {   //  一致.出力をマスクする
                        stb.AppendFormat("\r\n{0}:{1}:*", var.ParameterName, var.SqlDbType);
                        return;
                    }
                }
                switch (var.SqlDbType)
                {
                    case SqlDbType.Image:
                        if (this.ParameterLogImageEnabled)
                            stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.SqlDbType, cklib.Util.String.HexDumpList((byte[])Value));
                        else
                            stb.AppendFormat("\r\n{0}:{1}", var.ParameterName, var.SqlDbType);
                        break;
                    case SqlDbType.Variant:
                    case SqlDbType.VarBinary:
                    case SqlDbType.Binary:
                        stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.SqlDbType, cklib.Util.String.HexDumpList((byte[])Value));
                        break;
                    case SqlDbType.Timestamp:
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, cklib.Util.String.HexDumpStr((byte[])Value));
                        break;
                    case SqlDbType.Structured:
                        if (this.ParameterLogStructureEnabled)
                        {
                            stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, var.TypeName);
                            if ((Value.GetType().Equals(typeof(DataTable))) ||
                                (Value.GetType().IsSubclassOf(typeof(DataTable))))
                            {   //  データテーブルを展開表示する
                                DataTable dt = Value as DataTable;
                                stb.Append("\r\n\t");
                                foreach (DataColumn item in dt.Columns)
                                {
                                    stb.AppendFormat("{0},", item.ColumnName);
                                }
                                foreach (DataRow row in dt.Rows)
                                {
                                    stb.Append("\r\n\t");
                                    for (int i = 0; i < row.ItemArray.Length; i++)
                                    {
                                        if (row.ItemArray[i].GetType().Equals(typeof(byte[])))
                                            stb.AppendFormat("{0},", cklib.Util.String.HexDumpStr((byte[])row.ItemArray[i]));
                                        else
                                            stb.AppendFormat("{0},", row.ItemArray[i]);
                                    }
                                }
                            }
                        }
                        else
                            stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, var.TypeName);
                        break;
                    case SqlDbType.NText:
                    case SqlDbType.Text:
                        if (this.m_ParameterLogTextEnabled)
                            stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, Value);
                        else
                            stb.AppendFormat("\r\n{0}:{1}", var.ParameterName, var.SqlDbType);
                        break;
                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                    case SqlDbType.DateTime2:
                    case SqlDbType.DateTimeOffset:
                    case SqlDbType.SmallDateTime:
                    case SqlDbType.Time:
                    case SqlDbType.BigInt:
                    case SqlDbType.Bit:
                    case SqlDbType.Char:
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                    case SqlDbType.Int:
                    case SqlDbType.Money:
                    case SqlDbType.NChar:
                    case SqlDbType.NVarChar:
                    case SqlDbType.Real:
                    case SqlDbType.SmallInt:
                    case SqlDbType.TinyInt:
                    case SqlDbType.Udt:
                    case SqlDbType.UniqueIdentifier:
                    case SqlDbType.SmallMoney:
                    case SqlDbType.VarChar:
                    case SqlDbType.Xml:
                    default:
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, Value);
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
                    this.InnerLog(GetCallNestDepth(), SQLTraceLogCode.Connect, "CONNECT");
                else
                    this.InnerLog(GetCallNestDepth(), SQLTraceLogCode.Connect, "CONNECT [{0}]", DBConnectString);
                this.m_sqlConnection = new System.Data.SqlClient.SqlConnection(DBConnectString);
                if (SQLTraceEnable)
                    this.m_sqlConnection.InfoMessage += new SqlInfoMessageEventHandler(m_sqlConnection_InfoMessage);
                this.m_sqlConnection.Open();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error,"Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// SqlCommandインスタンスの取得
        /// </summary>
        /// <returns>SqlCommandインスタンス</returns>
        public void GetSqlCommand()
        {
            try
            {
                this.m_sqlCommand = this.sqlConnection.CreateCommand();
                if (this.DBCommandTimer != -1)
                {
                    this.m_sqlCommand.CommandTimeout = this.DBCommandTimer;
                }
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1,SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">ロックレベルの指定</param>
        /// <returns>SqlTransactionインスタンス</returns>
        public SqlTransaction BeginTransaction(IsolationLevel iso)
        {
            try
            {
                SqlTransaction sqlTransaction = this.sqlConnection.BeginTransaction(iso);
                sqlCommand.Transaction = sqlTransaction;
                return sqlTransaction;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションの開始(規定レベル)
        /// </summary>
        /// <returns>SqlTransactionインスタンス</returns>
        public SqlTransaction BeginTransaction()
        {
            return  this.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">ロックレベルの指定</param>
        /// <returns>DBTransactionインスタンス</returns>
        public virtual DBTransaction BeginDBTransaction(IsolationLevel iso)
        {
            try
            {
                this.InnerLog(GetCallNestDepth(), SQLTraceLogCode.BeginTransaction, "BEGIN TRANSACTION [{0}]", iso);
                this.m_DBTransaction = new DBTransaction(this.BeginTransaction(iso), this);
                return this.m_DBTransaction;
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// トランザクションの開始(規定レベル)
        /// </summary>
        /// <returns>SqlTransactionインスタンス</returns>
        public DBTransaction BeginDBTransaction()
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
                return this.m_sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                return this.m_sqlCommand.ExecuteScalar();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteReaderのラッパーメソッド
        /// </summary>
        /// <returns></returns>
        public SqlDataReader ExecuteReader()
        {
            try
            {
                this.LogStatement(2);
                return this.m_sqlCommand.ExecuteReader();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteReaderのラッパーメソッド
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(CommandBehavior behavior)
        {
            try
            {
                this.LogStatement(2);
                return this.m_sqlCommand.ExecuteReader(behavior);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        /// <summary>
        /// ExecuteXmlReaderのラッパー
        /// </summary>
        /// <returns>XmlReader</returns>
        public XmlReader ExecuteXmlReader()
        {
            try
            {
                this.LogStatement(2);
                return this.m_sqlCommand.ExecuteXmlReader();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }

        /// <summary>
        /// SQL接続リソースの開放
        /// </summary>
        public void ReleaseSqlResource()
        {
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
            if (this.m_sqlCommand!=null)
            {
                try
                {
                    this.m_sqlCommand.Dispose();
                }
                catch
                { }
                finally
                {
                    this.m_sqlCommand = null;
                }
            }
            if (this.m_sqlConnection!= null)
            {
                this.InnerLog(4,SQLTraceLogCode.Close, "CLOSE");
                try
                {
                    this.m_sqlConnection.Dispose();
                }
                catch
                {}
                finally
                {
                    this.m_sqlConnection = null;
                }
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
