using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cklib.Data.Odbc
{
    /// <summary>
    /// Odbc接続操作ラッパークラス
    /// </summary>
    [Serializable]
    public class OdbcDataInstance : DataInstance<OdbcDataTransaction, OdbcConnection, OdbcCommand, OdbcTransaction,OdbcParameter, OdbcDataReader,OdbcDataAdapter, OdbcDataConfigSection, DataConfigElement>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OdbcDataInstance()
            :base()
        {}
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataInstance(bool fCommandInitialize)
            :base(fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OdbcDataInstance(string DBConnectString)
            :base(DBConnectString)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OdbcDataInstance(string DBConnectString, int DBCommandTimer)
            :base(DBConnectString, DBCommandTimer)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataInstance(string DBConnectString, bool fCommandInitialize)
            :base(DBConnectString, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            :base(DBConnectString, DBCommandTimer, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public OdbcDataInstance(OdbcDataAdapter adapter)
            : base(adapter)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataInstance(OdbcDataAdapter adapter, bool fCommandInitialize)
            : base(adapter,fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OdbcDataInstance(OdbcDataAdapter adapter, string DBConnectString)
            : base(adapter,DBConnectString)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OdbcDataInstance(OdbcDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(adapter,DBConnectString, DBCommandTimer)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataInstance(OdbcDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
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
        public OdbcDataInstance(OdbcDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(adapter,DBConnectString, DBCommandTimer, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public OdbcDataInstance(OdbcDataConfigSection config)
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
        protected override OdbcConnection CreateDBConnection(string DBConnectString)
        {
            var SqlConnection = new OdbcConnection(DBConnectString);
            if (SQLTraceEnable)
                SqlConnection.InfoMessage += m_dbConnection_InfoMessage;
            return SqlConnection;
        }

        /// <summary>
        ///アダプタインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        protected override OdbcDataAdapter CreateDBAdapter()
        {
            return new OdbcDataAdapter();
        }
        /// <summary>
        /// SQLメッセージ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dbConnection_InfoMessage(object sender, OdbcInfoMessageEventArgs e)
        {
            if (!SQLTraceEnable) return;
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                StringBuilder stb = new StringBuilder();
                foreach (OdbcError error in e.Errors)
                {
                    stb.AppendFormat("Odbc Error SQLSate:{0} NativeError:{1} Message:{2} Source:{3}\r\n",
                                        error.SQLState, error.NativeError, error.Message, error.Source);
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
        protected override OdbcDataTransaction CreateDBTransaction(OdbcTransaction dbTransaction, IDataInstanceInnerLogging dbInstance)
        {
            return new OdbcDataTransaction(dbTransaction, dbInstance);
        }

        /// <summary>
        /// ログ編集用にSqlパラメータを文字列バッファに編集して格納する。
        /// </summary>
        /// <param name="stb"></param>
        /// <param name="var"></param>
        /// <param name="Value"></param>
        protected override void QueryParameterToLogStringItem(ref StringBuilder stb, OdbcParameter var, object Value)
        {
            if (Value == null || Value.Equals(DBNull.Value))    //  2012/03/01 bugfix
                stb.AppendFormat("\r\n{0}:{1}:NULL", var.ParameterName, var.OdbcType);
            else
            {
                if (this.ParameterMaskRegex.Length != 0)
                {
                    if (this.m_ParameterMaskRegex.IsMatch(var.ParameterName))
                    {   //  一致.出力をマスクする
                        stb.AppendFormat("\r\n{0}:{1}:*", var.ParameterName, var.OdbcType);
                        return;
                    }
                }
                switch (var.OdbcType)
                {
                    case OdbcType.Image:	//	可変長バイナリ データ。 最大長は、データ ソースによって異なります (SQL_LONGVARBINARY)。 Byte 型の Array に割り当てられます。
                        if (this.ParameterLogImageEnabled)
                            stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.OdbcType, cklib.Util.String.HexDumpList((byte[])Value));
                        else
                            stb.AppendFormat("\r\n{0}:{1}", var.ParameterName, var.OdbcType);
                        break;
                    case OdbcType.Binary:	//	バイナリ データのストリーム (SQL_BINARY)。 Byte 型の Array に割り当てられます。
                    case OdbcType.VarBinary:	//	可変長バイナリ。 最大値は、ユーザーが設定します (SQL_VARBINARY)。 Byte 型の Array に割り当てられます。
                        stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.OdbcType, cklib.Util.String.HexDumpList((byte[])Value));
                        break;
                    case OdbcType.Timestamp:	//	バイナリ データのストリーム (SQL_BINARY)。 Byte 型の Array に割り当てられます。
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.OdbcType, cklib.Util.String.HexDumpStr((byte[])Value));
                        break;

                    case OdbcType.NText:	//	Unicode 可変長文字データ。 最大長は、データ ソースによって異なります。 (SQL_WLONGVARCHAR)。 String に割り当てられます。
                    case OdbcType.Text:	//	可変長文字データ。 最大長は、データ ソースによって異なります (SQL_LONGVARCHAR)。 String に割り当てられます。
                        if (this.m_ParameterLogTextEnabled)
                            stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.OdbcType, Value);
                        else
                            stb.AppendFormat("\r\n{0}:{1}", var.ParameterName, var.OdbcType);
                        break;

                    case OdbcType.BigInt:	//	精度が 19 (符号付きの場合)、または 20 (符号なしの場合) で、小数部桁数が 0 の固定小数点数値 (符号付きの場合 ?2[63] <= n <= 2[63] ? 1、符号なしの場合 0 <= n <= 2[64] ? 1) (SQL_BIGINT)。 Int64 に割り当てられます。
                    case OdbcType.Bit:	//	1 ビットのバイナリ データ (SQL_BIT)。 Boolean に割り当てられます。
                    case OdbcType.Char:	//	固定長文字列 (SQL_CHAR)。 String に割り当てられます。
                    case OdbcType.Date:	//	yyyymmdd 形式の日付データ (SQL_TYPE_DATE)。 DateTime に割り当てられます。
                    case OdbcType.DateTime:	//	yyyymmddhhmmss 形式の日付データ (SQL_TYPE_TIMESTAMP)。 DateTime に割り当てられます。
                    case OdbcType.Decimal:	//	少なくとも精度が p で、小数部桁数が s の符号付き固定小数点数値 (1 <= p <= 15 で s <= p)。 最大精度は、ドライバーによって異なります (SQL_DECIMAL)。 これは、Decimal に割り当てられます。
                    case OdbcType.Double:	//	バイナリ精度が 53 の符号付き概数数値 (0、または絶対値が 10[?308] から 10[308] の間) (SQL_DOUBLE)。 Double に割り当てられます。
                    case OdbcType.Int:	//	精度が 10 で小数部桁数が 0 の固定数値 (符号付きの場合 ?2[31] <= n <= 2[31] ? 1、符号なしの場合 0 <= n <= 2[32] ? 1) (SQL_INTEGER)。 Int32 に割り当てられます。
                    case OdbcType.NChar:	//	固定文字列長の Unicode 文字列 (SQL_WCHAR)。 String に割り当てられます。
                    case OdbcType.Numeric:	//	精度が p で、小数部桁数が s の符号付き固定小数点数値 (1 <= p <= 15 で s <= p)。(SQL_NUMERIC)。 これは、Decimal に割り当てられます。
                    case OdbcType.NVarChar:	//	Unicode 文字の可変長ストリーム (SQL_WVARCHAR)。 String に割り当てられます。
                    case OdbcType.Real:	//	バイナリ精度が 24 の符号付き概数数値 (0、または絶対値が 10[-38] から 10[38] の間) (SQL_REAL)。 Single に割り当てられます。
                    case OdbcType.SmallDateTime:	//	yyyymmddhhmmss 形式の日付と時刻のデータ (SQL_TYPE_TIMESTAMP)。 DateTime に割り当てられます。
                    case OdbcType.SmallInt:	//	精度が 5 で小数部桁数が 0 の固定数値 (符号付きの場合 -32,768 <= n <= 32,767、符号なしの場合 0 <= n <= 65,535) (SQL_SMALLINT)。 Int16 に割り当てられます。
                    case OdbcType.Time:	//	hhmmss 形式の日付データ (SQL_TYPE_TIMES)。 DateTime に割り当てられます。
                    case OdbcType.TinyInt:	//	精度が 3 で小数部桁数が 0 の固定数値 (符号付きの場合 ?128 <= n <= 127、符号なしの場合 0 <= n <= 255) (SQL_TINYINT)。 Byte に割り当てられます。
                    case OdbcType.UniqueIdentifier:	//	固定長 GUID (SQL_GUID)。 Guid に割り当てられます。
                    case OdbcType.VarChar:	//	可変長ストリーム文字列 (SQL_CHAR)。 String に割り当てられます。                     
                    default:
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.OdbcType, Value);
                        break;
                }
            }
        }
        #endregion
    }
}
