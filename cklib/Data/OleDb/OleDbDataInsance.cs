using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cklib.Data.OleDb
{
    /// <summary>
    /// OleDb接続操作ラッパークラス
    /// </summary>
    [Serializable]
    public class OleDbDataInstance : DataInstance<OleDbDataTransaction, OleDbConnection, OleDbCommand, OleDbTransaction, OleDbParameter, OleDbDataReader, OleDbDataAdapter, OleDbDataConfigSection, DataConfigElement>
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OleDbDataInstance()
            : base()
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(bool fCommandInitialize)
            : base(fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OleDbDataInstance(string DBConnectString)
            : base(DBConnectString)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OleDbDataInstance(string DBConnectString, int DBCommandTimer)
            : base(DBConnectString, DBCommandTimer)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(string DBConnectString, bool fCommandInitialize)
            : base(DBConnectString, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(DBConnectString, DBCommandTimer, fCommandInitialize)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public OleDbDataInstance(OleDbDataAdapter adapter)
            : base(adapter)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(OleDbDataAdapter adapter, bool fCommandInitialize)
            : base(adapter, fCommandInitialize)
        {
            this.sqlDataAdapter = adapter;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OleDbDataInstance(OleDbDataAdapter adapter, string DBConnectString)
            : base(adapter, DBConnectString)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OleDbDataInstance(OleDbDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(adapter, DBConnectString, DBCommandTimer)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(OleDbDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
            : base(adapter, DBConnectString, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataInstance(OleDbDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(adapter, DBConnectString, DBCommandTimer, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public OleDbDataInstance(OleDbDataConfigSection config)
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
        protected override OleDbConnection CreateDBConnection(string DBConnectString)
        {
            var SqlConnection = new OleDbConnection(DBConnectString);
            if (SQLTraceEnable)
                SqlConnection.InfoMessage += m_dbConnection_InfoMessage;
            return SqlConnection;
        }

        /// <summary>
        ///アダプタインスタンスを生成する
        /// </summary>
        /// <returns></returns>
        protected override OleDbDataAdapter CreateDBAdapter()
        {
            return new OleDbDataAdapter();
        }
        /// <summary>
        /// SQLメッセージ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_dbConnection_InfoMessage(object sender, OleDbInfoMessageEventArgs e)
        {
            if (!SQLTraceEnable) return;
            lock (traceLog)
            {
                if (!traceLog.IsValidLevel(SQLTraceLogLevel))
                    return;
                StringBuilder stb = new StringBuilder();
                foreach (OleDbError error in e.Errors)
                {
                    stb.AppendFormat("OleDb Error SQLSate:{0} NativeError:{1} Message:{2} Source:{3}\r\n",
                                        error.SQLState, error.NativeError, error.Message, error.Source);
                }
                InnerLog(1, DataTraceLogCode.Message, stb.ToString());
            }
        }

        /// <summary>
        /// トランザクションクラスインスタンス生成
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <param name="dbInstance"></param>
        /// <returns></returns>
        protected override OleDbDataTransaction CreateDBTransaction(OleDbTransaction dbTransaction, IDataInstanceInnerLogging dbInstance)
        {
            return new OleDbDataTransaction(dbTransaction, dbInstance);
        }

        /// <summary>
        /// ログ編集用にSqlパラメータを文字列バッファに編集して格納する。
        /// </summary>
        /// <param name="stb"></param>
        /// <param name="var"></param>
        /// <param name="Value"></param>
        protected override void QueryParameterToLogStringItem(ref StringBuilder stb, OleDbParameter var, object Value)
        {
            if (Value == null || Value.Equals(DBNull.Value))    //  2012/03/01 bugfix
                stb.AppendFormat("\r\n{0}:{1}:NULL", var.ParameterName, var.OleDbType);
            else
            {
                if (this.ParameterMaskRegex.Length != 0)
                {
                    if (this.m_ParameterMaskRegex.IsMatch(var.ParameterName))
                    {   //  一致.出力をマスクする
                        stb.AppendFormat("\r\n{0}:{1}:*", var.ParameterName, var.OleDbType);
                        return;
                    }
                }
                switch (var.OleDbType)
                {
                    case OleDbType.Binary:	//	バイナリ データのストリーム (DBTYPE_BYTES)。 Byte 型の Array に割り当てられます。
                    case OleDbType.VarBinary:	//	バイナリ データの可変長ストリーム (OleDbParameter だけ)。 Byte 型の Array に割り当てられます。
                    case OleDbType.Variant:	//	数値、文字列、バイナリ、日付データのいずれか、および特殊な値である Empty と Null を格納できる、特殊なデータ型 (DBTYPE_VARIANT)。 他の型が指定されていない場合は、この型と見なされます。 Object に割り当てられます。
                        stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.OleDbType, cklib.Util.String.HexDumpList((byte[])Value));
                        break;
                    case OleDbType.BigInt:	//	64 ビット符号付き整数 (DBTYPE_I8)。 Int64 に割り当てられます。
                    case OleDbType.Boolean:	//	ブール値 (DBTYPE_BOOL)。 Boolean に割り当てられます。
                    case OleDbType.BSTR:	//	null で終わる Unicode 文字列 (DBTYPE_BSTR)。 String に割り当てられます。
                    case OleDbType.Char:	//	文字列 (DBTYPE_STR)。 String に割り当てられます。
                    case OleDbType.Currency:	//	精度が通貨単位の 1/10,000 の、-2 63 (-922,337,203,685,477.5808) から 2 63 -1 (+922,337,203,685,477.5807) までの範囲内の通貨値 (DBTYPE_CY)。 これは、Decimal に割り当てられます。
                    case OleDbType.Date:	//	倍精度浮動小数点数として格納される日付データ (DBTYPE_DATE)。 正数部は 1899 年 12 月 30 日以降の日数、小数部は 1 日の端数を示します。 DateTime に割り当てられます。
                    case OleDbType.DBDate:	//	yyyymmdd 書式の日付データ (DBTYPE_DBDATE)。 DateTime に割り当てられます。
                    case OleDbType.DBTime:	//	hhmmss 書式の時刻データ (DBTYPE_DBTIME)。 TimeSpan に割り当てられます。
                    case OleDbType.DBTimeStamp:	//	yyyymmddhhmmss 書式の日時データ (DBTYPE_DBTIMESTAMP)。 DateTime に割り当てられます。
                    case OleDbType.Decimal:	//	-10 38 -1 から 10 38 -1 までの範囲内の固定精度小数部桁数 (DBTYPE_DECIMAL)。 これは、Decimal に割り当てられます。
                    case OleDbType.Double:	//	-1.79E +308 ～ 1.79E +308 の範囲の浮動小数点数 (DBTYPE_R8)。 Double に割り当てられます。
                    case OleDbType.Empty:	//	値なし (DBTYPE_EMPTY)。
                    case OleDbType.Error:	//	32 ビット エラー コード (DBTYPE_ERROR)。 Exception に割り当てられます。
                    case OleDbType.Filetime:	//	1601 年 1 月 1 日以降の 100 ナノ秒数を表す、64 ビット符号なし整数 (DBTYPE_FILETIME)。 DateTime に割り当てられます。
                    case OleDbType.Guid:	//	グローバル一意識別子 (GUID) (DBTYPE_GUID)。 Guid に割り当てられます。
                    case OleDbType.IDispatch:	//	IDispatch インターフェイスを指すポインター (DBTYPE_IDISPATCH)。 Object に割り当てられます。
                    //	このデータ型は、ADO.NET で現在サポートされていません。 使用すると、予期しない結果が生じることがあります。
                    case OleDbType.Integer:	    //	32 ビット符号付き整数 (DBTYPE_I4)。 Int32 に割り当てられます。
                    case OleDbType.IUnknown:	//	IUnknown インターフェイスを指すポインター (DBTYPE_IDISPATCH)。 Object に割り当てられます。
                    //	このデータ型は、ADO.NET で現在サポートされていません。 使用すると、予期しない結果が生じることがあります。
                    case OleDbType.LongVarBinary:	//	long 型バイナリ値 (OleDbParameter だけ)。 Byte 型の Array に割り当てられます。
                    case OleDbType.LongVarChar:	//	long 型文字列値 (OleDbParameter だけ)。 String に割り当てられます。
                    case OleDbType.LongVarWChar:	//	long 型の、null で終わる Unicode 文字列値 (OleDbParameter だけ)。 String に割り当てられます。
                    case OleDbType.Numeric:	//	有効桁数と小数部桁数が固定の固定小数点数値 (DBTYPE_NUMERIC)。 これは、Decimal に割り当てられます。
                    case OleDbType.PropVariant:	//	オートメーション PROPVARIANT (DBTYPE_PROP_VARIANT)。 Object に割り当てられます。
                    case OleDbType.Single:	//	-3.40E +38 ～ 3.40E +38 の範囲の浮動小数点数 (DBTYPE_R4)。 Single に割り当てられます。
                    case OleDbType.SmallInt:	//	16 ビット符号付き整数 (DBTYPE_I2)。 Int16 に割り当てられます。
                    case OleDbType.TinyInt:	//	8 ビット符号付き整数 (DBTYPE_I1)。 SByte に割り当てられます。
                    case OleDbType.UnsignedBigInt:	//	64 ビット符号なし整数 (DBTYPE_UI8)。 UInt64 に割り当てられます。
                    case OleDbType.UnsignedInt:	//	32 ビット符号なし整数 (DBTYPE_UI4)。 UInt32 に割り当てられます。
                    case OleDbType.UnsignedSmallInt:	//	16 ビット符号なし整数 (DBTYPE_UI2)。 UInt16 に割り当てられます。
                    case OleDbType.UnsignedTinyInt:	//	8 ビット符号なし整数 (DBTYPE_UI1)。 Byte に割り当てられます。
                    case OleDbType.VarChar:	//	非 Unicode 文字の可変長ストリーム (OleDbParameter だけ)。 String に割り当てられます。
                    case OleDbType.VarNumeric:	//	可変長数値 (OleDbParameter だけ)。 これは、Decimal に割り当てられます。
                    case OleDbType.VarWChar:	//	可変長の、null で終わる Unicode 文字ストリーム (OleDbParameter だけ)。 String に割り当てられます。
                    case OleDbType.WChar:	//	null で終わる Unicode 文字ストリーム (DBTYPE_WSTR)。 String に割り当てられます。 
                        break;
                    default:
                        stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.OleDbType, Value);
                        break;
                }
            }
        }
        #endregion
    }
}
