using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log
{
    /// <summary>
    /// ログマネージャを初期化起動するインスタンスを生成する
    /// </summary>
    public  class LogManagerStarter : IDisposable
    {
        /// <summary>
        /// ログマネージャインスタンス
        /// </summary>
        public readonly LogManagerEx mng;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogManagerStarter()
        {
            mng = new LogManagerEx();
            mng.Initialize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="conf">初期化パラメータ</param>
        public LogManagerStarter(Config.ConfigInfo conf)
        {
            mng = new LogManagerEx();
            mng.Initialize(conf);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="section">設定セクション名</param>
        public LogManagerStarter(string section)
        {
            mng = new LogManagerEx(section);
            mng.Initialize();
        }

        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~LogManagerStarter()
        {
            Dispose(false);
        }
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
                ReleseResorce();
                this.mng.Terminate();
                this.mng.Dispose();
            }
            disposed = true;
        }
        /// <summary>
        /// リソース解放処理
        /// </summary>
        protected virtual void ReleseResorce()
        {
        }
        #endregion

    }
}
