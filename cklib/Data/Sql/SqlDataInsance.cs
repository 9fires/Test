using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cklib.Data.Sql
{
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス
    /// </summary>
    [Serializable]
    public class SqlDataInstance : DataInstance<SqlDataTransaction, SqlConnection, SqlCommand, SqlTransaction, SqlParameter, SqlDataReader, SqlDataAdapter, SqlDataConfigSection, DataConfigElement>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlDataInstance()
            :base()
        {}
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(bool fCommandInitialize)
            :base(fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public SqlDataInstance(string DBConnectString)
            :base(DBConnectString)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public SqlDataInstance(string DBConnectString, int DBCommandTimer)
            :base(DBConnectString, DBCommandTimer)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(string DBConnectString, bool fCommandInitialize)
            :base(DBConnectString, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            :base(DBConnectString, DBCommandTimer, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public SqlDataInstance(SqlDataAdapter adapter)
            : base(adapter)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(SqlDataAdapter adapter, bool fCommandInitialize)
            : base(adapter,fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public SqlDataInstance(SqlDataAdapter adapter, string DBConnectString)
            : base(adapter,DBConnectString)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public SqlDataInstance(SqlDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(adapter,DBConnectString, DBCommandTimer)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(SqlDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
            : base(adapter,DBConnectString, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public SqlDataInstance(SqlDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(adapter,DBConnectString, DBCommandTimer, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public SqlDataInstance(SqlDataConfigSection config)
            : base(config)   
        {
        }
        #endregion

        #region データベースプロバイダ毎の実装
        /// <summary>
        /// 接続インスタンスを生成
        /// </summary>
        /// <param name="DBConnectString">接続文字列</param>
        /// <returns>生成されたインスタンス</returns>
        protected override SqlConnection CreateDBConnection(string DBConnectString)
        {
            var SqlConnection =  new SqlConnection(DBConnectString);
            if (SQLTraceEnable)
                SqlConnection.InfoMessage += m_sqlConnection_InfoMessage;
            return SqlConnection;
        }
        /// <summary>
        ///アダプタインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        protected override SqlDataAdapter CreateDBAdapter()
        {
            return new SqlDataAdapter();
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
                InnerLog(1,DataTraceLogCode.Message, stb.ToString());
            }
        }

        /// <summary>
        /// トランザクションクラスインスタンス生成
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <param name="dbInstance"></param>
        /// <returns></returns>
        protected override SqlDataTransaction CreateDBTransaction(SqlTransaction dbTransaction, IDataInstanceInnerLogging dbInstance)
        {
            return new SqlDataTransaction(dbTransaction, dbInstance);
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
                return this.m_dbCommand.ExecuteXmlReader();
            }
            catch (Exception exp)
            {
                this.InnerLogExp(1, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }

        /// <summary>
        /// ログ編集用にSqlパラメータを文字列バッファに編集して格納する。
        /// </summary>
        /// <param name="stb"></param>
        /// <param name="var"></param>
        /// <param name="Value"></param>
        protected override void QueryParameterToLogStringItem(ref StringBuilder stb, SqlParameter var, object Value)
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
    }
}
