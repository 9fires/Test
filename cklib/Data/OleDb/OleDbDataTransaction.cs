using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.OleDb
{
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    [Serializable]
    public class OleDbDataTransaction : DataTransaction<OleDbTransaction>
    {
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Transaction">生成済みのトランザクションインスタンス</param>
        /// <param name="dbInstance">DB接続インスタンス</param>
        public OleDbDataTransaction(OleDbTransaction Transaction, IDataInstanceInnerLogging dbInstance)
            :base(Transaction,dbInstance)
        {
        }
        #endregion
    }
}
