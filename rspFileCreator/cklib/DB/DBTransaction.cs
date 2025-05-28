using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace cklib.DB
{
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    [Serializable]
    public class DBTransaction:IDisposable
    {
        /// <summary>
        /// コミット済み未済みフラグ
        /// </summary>
        private bool m_Commited=false;
        /// <summary>
        /// DBインスタンス
        /// </summary>
        private readonly DBInstance dbInstance;
        /// <summary>
        /// コミット済み未済いフラグ
        /// </summary>
        public bool IsCommited
        {
            get
            {
                return this.m_Commited;
            }
            set
            {
                this.m_Commited = value;
            }
        }
        /// <summary>
        /// SQLトランザクションインスタンス
        /// </summary>
        public readonly SqlTransaction Transaction;
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Transaction">生成済みのトランザクションインスタンス</param>
        /// <param name="dbInstance">DB接続インスタンス</param>
        public DBTransaction(SqlTransaction Transaction,DBInstance dbInstance)
        {
            this.dbInstance = dbInstance;
            this.Transaction = Transaction;
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~DBTransaction()
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
                if (!this.m_Commited)
                {
                    try
                    {
                        this.dbInstance.InnerLog(3,SQLTraceLogCode.RollBack, "ROLLBACK");
                        this.Transaction.Rollback();
                    }
                    catch
                    { }
                }
                try
                {
                    this.Transaction.Dispose();
                }
                catch
                { }
            }
            disposed = true;
        }

        #endregion
        #region 基本メソッド
        /// <summary>
        /// トランザクションをコミットする
        /// </summary>
        public void Commit()
        {
            try
            {
                this.dbInstance.InnerLog(2,SQLTraceLogCode.Commit, "COMMIT");
                this.Transaction.Commit();
                this.m_Commited = true;
            }
            catch (Exception exp)
            {
                this.dbInstance.InnerLogExp(2,SQLTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        #endregion

    }
}
