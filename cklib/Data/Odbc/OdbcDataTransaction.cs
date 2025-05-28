using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.Odbc
{
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    [Serializable]
    public class OdbcDataTransaction : DataTransaction<OdbcTransaction>
    {
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Transaction">生成済みのトランザクションインスタンス</param>
        /// <param name="dbInstance">DB接続インスタンス</param>
        public OdbcDataTransaction(OdbcTransaction Transaction, IDataInstanceInnerLogging dbInstance)
            :base(Transaction,dbInstance)
        {
        }
        #endregion
    }
}
