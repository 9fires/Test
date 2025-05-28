using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using cklib.Log;
using System.Threading;
using System.Xml;

namespace cklib.DB
{
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス<BR/>
    /// SqlDataAdapterを利用するための拡張
    /// </summary>
    /// <remarks>
    /// 自動生成されたDataSetコードとの共存を行うため為の補助クラスです<br/>
    /// 自動生成されたAdapterラッパーコードを使用せず当該ライブラリを使用することで、
    /// 設定共有、トランザクション管理の自動化、SQLログの出力等<see cref="DBInstance"/>の機能が利用できます。<br/>
    /// 自動生成されたAdapterクラスのpartialクラスを生成して、独自のコンストラクタまたは、
    /// 初期化メソッドを追加し当クラスのsqlDataAdapterを初期化することで自動生成されたSqlステートメント、
    /// ORマッピングされたDataTable、DataRowsの派生クラスを利用できます。
    /// </remarks>
    public class DBAdapterInstance : DBInstance
    {
        #region インスタンス参照プロパティ
        /// <summary>
        /// アダプタインスタンス
        /// </summary>
        private SqlDataAdapter m_sqlDataAdapter = null;
        /// <summary>
        /// アダプタインスタンス
        /// </summary>
        public SqlDataAdapter sqlDataAdapter
        {
            get
            { return this.m_sqlDataAdapter; }
            set
            {
                this.m_sqlDataAdapter = value;
                SetupSqlCommand(this.m_sqlDataAdapter.SelectCommand);
                SetupSqlCommand(this.m_sqlDataAdapter.InsertCommand);
                SetupSqlCommand(this.m_sqlDataAdapter.UpdateCommand);
                SetupSqlCommand(this.m_sqlDataAdapter.DeleteCommand);
            }
        }
        /// <summary>
        /// Select用SqlCommandインスタンス
        /// </summary>
        public SqlCommand SelectSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.SelectCommand; }
            set
            {
                SetupSqlCommand(this.m_sqlDataAdapter.SelectCommand, value);
                this.m_sqlDataAdapter.SelectCommand = value;
            }
        }
        /// <summary>
        /// Insert用SqlCommandインスタンス
        /// </summary>
        public SqlCommand InsertSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.InsertCommand; }
            set
            {
                SetupSqlCommand(this.m_sqlDataAdapter.InsertCommand, value);
                this.m_sqlDataAdapter.InsertCommand = value;
            }
        }
        /// <summary>
        /// Update用SqlCommandインスタンス
        /// </summary>
        public SqlCommand UpdateSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.UpdateCommand; }
            set
            {
                SetupSqlCommand(this.m_sqlDataAdapter.UpdateCommand, value);
                this.m_sqlDataAdapter.UpdateCommand = value;
            }
        }
        /// <summary>
        /// Delete用SqlCommandインスタンス
        /// </summary>
        public SqlCommand DeleteSqlCommand
        {
            get
            { return this.m_sqlDataAdapter.DeleteCommand; }
            set
            {
                SetupSqlCommand(this.m_sqlDataAdapter.DeleteCommand, value);
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
        public DBAdapterInstance()
            : base()
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(bool fCommandInitialize)
            : base(fCommandInitialize)
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public DBAdapterInstance(string DBConnectString)
            : base(DBConnectString)
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public DBAdapterInstance(string DBConnectString, int DBCommandTimer)
            : base(DBConnectString, DBCommandTimer)
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(string DBConnectString, bool fCommandInitialize)
            : base(DBConnectString, fCommandInitialize)
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(DBConnectString, DBCommandTimer, fCommandInitialize)
        {
            this.m_sqlDataAdapter = new SqlDataAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public DBAdapterInstance(SqlDataAdapter adapter)
            : base()
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(SqlDataAdapter adapter, bool fCommandInitialize)
            : base(fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public DBAdapterInstance(SqlDataAdapter adapter, string DBConnectString)
            : base(DBConnectString)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public DBAdapterInstance(SqlDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(DBConnectString, DBCommandTimer)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(SqlDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
            : base(DBConnectString, fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public DBAdapterInstance(SqlDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(DBConnectString, DBCommandTimer, fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~DBAdapterInstance()
        {
            this.Dispose(false);
        }
        #endregion
        #region IDisposable メンバ
        /// <summary>
        /// リソース解放処理
        /// </summary>
        protected override void ReleseResorce()
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
            base.ReleseResorce();
        }
        /// <summary>
        /// マネージドリソース解放処理（明示的呼び出し時のみ実行される）
        /// </summary>
        protected override void ReleseManagedResorce()
        {
            base.ReleseManagedResorce();
        }

        #endregion
        #region トランザクション制御のオーバーロード
        /// <summary>
        /// トランザクションの開始
        /// </summary>
        /// <param name="iso">分離レベル</param>
        /// <returns>DBTransactionインスタンス</returns>
        public override DBTransaction BeginDBTransaction(IsolationLevel iso)
        {
            DBTransaction tran = base.BeginDBTransaction(iso);
            if (this.m_sqlDataAdapter.SelectCommand != null)
                this.m_sqlDataAdapter.SelectCommand.Transaction = tran.Transaction;
            if (this.m_sqlDataAdapter.InsertCommand != null)
                this.m_sqlDataAdapter.InsertCommand.Transaction = tran.Transaction;
            if (this.m_sqlDataAdapter.UpdateCommand != null)
                this.m_sqlDataAdapter.UpdateCommand.Transaction = tran.Transaction;
            if (this.m_sqlDataAdapter.DeleteCommand != null)
                this.m_sqlDataAdapter.DeleteCommand.Transaction = tran.Transaction;
            return tran;
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
                this.InnerLogExp(depth, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLog(2, SQLTraceLogCode.Statement, "Update Adapter From DataTable");
                this.LogAdapterUpdateStatement(2, dt.Rows);
                return this.sqlDataAdapter.Update(dt);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLog(2, SQLTraceLogCode.Statement, "Update Adapter From DatSet");
                foreach (DataTable dt in ds.Tables)
                {
                    this.LogAdapterUpdateStatement(2, dt.Rows);
                }
                return this.sqlDataAdapter.Update(ds, srcTable);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLog(2, SQLTraceLogCode.Statement, "Update Adapter From DatSet");
                foreach (DataTable dt in ds.Tables)
                {
                    this.LogAdapterUpdateStatement(2, dt.Rows);
                }
                return this.sqlDataAdapter.Update(ds);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
                this.InnerLog(2, SQLTraceLogCode.Statement, "Update Adapter From DatSet");
                this.LogAdapterUpdateStatement(2, drows);
                return this.sqlDataAdapter.Update(drows);
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, SQLTraceLogCode.Error, "Exception", exp);
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
            lock (traceLog)
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
            lock (traceLog)
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
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
            }
            if (!this.ParameterLogDataSetEnabled)
                return;
            SqlCommand cmd = null;
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
                    this.SqlParameterToLogStringItem(ref stb, cmd.Parameters[i], row[cmd.Parameters[i].SourceColumn, cmd.Parameters[i].SourceVersion]);
                }
            }
            lock (traceLog)
            {
                traceLog.LogStore<string>(SQLTraceLogLevel, SQLTraceLogCodeBase + (int)SQLTraceLogCode.Statement, string.Format("{0:000000}:{1}", this.InstanceID, cmd.CommandText), new System.Diagnostics.StackFrame(depth, true), null, stb.ToString(), stb.Length);
            }
        }
        #endregion
    }
}
