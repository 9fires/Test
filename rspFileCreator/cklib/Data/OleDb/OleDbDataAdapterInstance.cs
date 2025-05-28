using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.OleDb
{
    /// <summary>
    /// SqlServersDB接続操作ラッパークラス<BR/>
    /// SqlDataAdapterを利用するための拡張
    /// </summary>
    /// <remarks>
    /// 自動生成されたDataSetコードとの共存を行うため為の補助クラスです<br/>
    /// 自動生成されたAdapterラッパーコードを使用せず当該ライブラリを使用することで、
    /// 設定共有、トランザクション管理の自動化、SQLログの出力等<see cref="OleDbDataInstance"/>の機能が利用できます。<br/>
    /// 自動生成されたAdapterクラスのpartialクラスを生成して、独自のコンストラクタまたは、
    /// 初期化メソッドを追加し当クラスのsqlDataAdapterを初期化することで自動生成されたSqlステートメント、
    /// ORマッピングされたDataTable、DataRowsの派生クラスを利用できます。
    /// </remarks>
    public class OleDbDataAdapterInstance:OleDbDataInstance
    {
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OleDbDataAdapterInstance()
            : base()
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataAdapterInstance(bool fCommandInitialize)
            : base(fCommandInitialize)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OleDbDataAdapterInstance(string DBConnectString)
            : base(DBConnectString)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OleDbDataAdapterInstance(string DBConnectString, int DBCommandTimer)
            : base(DBConnectString, DBCommandTimer)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataAdapterInstance(string DBConnectString, bool fCommandInitialize)
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
        public OleDbDataAdapterInstance(string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(DBConnectString, DBCommandTimer, fCommandInitialize)
        {
            this.InitializeAdapter();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter)
            : base(adapter)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter, bool fCommandInitialize)
            : base(adapter,fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter, string DBConnectString)
            : base(adapter,DBConnectString)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="DBCommandTimer">SqlCommand実行タイマー</param>
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter, string DBConnectString, int DBCommandTimer)
            : base(adapter,DBConnectString, DBCommandTimer)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="adapter">SqlDataAdapterインスタンス</param>
        /// <param name="DBConnectString">DB接続文字列</param>
        /// <param name="fCommandInitialize">SqlCommandの初期化可否</param>
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter, string DBConnectString, bool fCommandInitialize)
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
        public OleDbDataAdapterInstance(OleDbDataAdapter adapter, string DBConnectString, int DBCommandTimer, bool fCommandInitialize)
            : base(adapter,DBConnectString, DBCommandTimer, fCommandInitialize)
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報</param>
        public OleDbDataAdapterInstance(OleDbDataConfigSection config)
            : base(config)   
        {
        }
        #endregion
    }
}
