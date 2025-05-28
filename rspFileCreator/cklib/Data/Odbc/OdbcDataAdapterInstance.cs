using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.Odbc
{
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス<BR/>
    /// SqlDataAdapterを利用するための拡張
    /// </summary>
    /// <remarks>
    /// 自動生成されたDataSetコードとの共存を行うため為の補助クラスです<br/>
    /// 自動生成されたAdapterラッパーコードを使用せず当該ライブラリを使用することで、
    /// 設定共有、トランザクション管理の自動化、SQLログの出力等<see cref="OdbcDataInstance"/>の機能が利用できます。<br/>
    /// 自動生成されたAdapterクラスのpartialクラスを生成して、独自のコンストラクタまたは、
    /// 初期化メソッドを追加し当クラスのsqlDataAdapterを初期化することで自動生成されたSqlステートメント、
    /// ORマッピングされたDataTable、DataRowsの派生クラスを利用できます。
    /// </remarks>
    public class OdbcDataAdapterInstance:OdbcDataInstance
    {
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OdbcDataAdapterInstance()
            : base()
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataAdapterInstance(bool fCommandInitialize)
            : base(fCommandInitialize)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OdbcDataAdapterInstance(string DBConnectString)
            : base(DBConnectString)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OdbcDataAdapterInstance(string DBConnectString, int DBCommandTimer)
            : base(DBConnectString, DBCommandTimer)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataAdapterInstance(string DBConnectString, bool fCommandInitialize)
            : base(DBConnectString, fCommandInitialize)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataAdapterInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(DBConnectString, DBCommandTimer, fCommandInitialize)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter)
            : base(adapter)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter, bool fCommandInitialize)
            : base(adapter,fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter, string DBConnectString)
            : base(adapter,DBConnectString)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(adapter,DBConnectString, DBCommandTimer)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
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
        public OdbcDataAdapterInstance(OdbcDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(adapter,DBConnectString, DBCommandTimer, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public OdbcDataAdapterInstance(OdbcDataConfigSection config)
            : base(config)   
        {
        }
        #endregion
    }
}
