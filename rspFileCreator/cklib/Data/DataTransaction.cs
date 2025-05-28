using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace cklib.Data
{
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    public interface IDataTransaction : IDisposable
    {
        /// <summary>
        /// コミット済み未済いフラグ
        /// </summary>
        bool IsCommited
        {
            get;
            set;
        }
        /// <summary>
        /// トランザクションをコミットする
        /// </summary>
        void Commit();
    }
    /// <summary>
    /// SqlServerDBTransactionラッパークラス
    /// </summary>
    /// <remarks>
    /// Commitされていない場合インスタンス解放時にロールバックされる<br/>
    /// </remarks>
    [Serializable]
    public class DataTransaction<DBTransaction> : IDisposable, IDataTransaction
        where DBTransaction : DbTransaction
    {
        /// <summary>
        /// コミット済み未済みフラグ
        /// </summary>
        private bool m_Commited=false;
        /// <summary>
        /// DBインスタンス
        /// </summary>
        private readonly IDataInstanceInnerLogging dbInstance;
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
        public readonly DBTransaction Transaction;
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Transaction">生成済みのトランザクションインスタンス</param>
        /// <param name="dbInstance">DB接続インスタンス</param>
        public DataTransaction(DBTransaction Transaction, IDataInstanceInnerLogging dbInstance)
        {
            this.dbInstance = dbInstance;
            this.Transaction = Transaction;
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~DataTransaction()
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
                        this.dbInstance.InnerLog(3,DataTraceLogCode.RollBack, "ROLLBACK");
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
                this.dbInstance.InnerLog(2, DataTraceLogCode.Commit, "COMMIT");
                this.Transaction.Commit();
                this.m_Commited = true;
            }
            catch (Exception exp)
            {
                this.dbInstance.InnerLogExp(2, DataTraceLogCode.Error, "Exception", exp);
                throw exp;
            }
        }
        #endregion

    }
}
