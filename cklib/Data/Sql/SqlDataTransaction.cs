using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.Sql
{
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    [Serializable]
    public class SqlDataTransaction : DataTransaction<SqlTransaction>
    {
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Transaction">生成済みのトランザクションインスタンス</param>
        /// <param name="dbInstance">DB接続インスタンス</param>
        public SqlDataTransaction(SqlTransaction Transaction, IDataInstanceInnerLogging dbInstance)
            :base(Transaction,dbInstance)
        {
        }
        #endregion
    }
}
