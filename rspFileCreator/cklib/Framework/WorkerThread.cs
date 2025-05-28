using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;
using cklib.Log;

namespace cklib.Framework
{
    /// <summary>
    /// 簡易ワーカースレッド処理
    /// </summary>
    /// <typeparam name="T">ワーカーパラメータ型定義</typeparam>
    /// <typeparam name="TResult">ワーカー処理結果返却値型</typeparam>
    public class WorkerThread<T, TResult> : WaitHandle
    {
        private static Logger log = new Logger();
        readonly Func<T, TResult> func;
        readonly T prm;
        /// <summary>
        /// 処理結果取得
        /// </summary>
        public TResult Result
        {
            get
            {
                return this.m_Result;
            }
        }
        private TResult m_Result;
        /// <summary>
        /// エラー終了したかを取得する
        /// </summary>
        public bool IsError
        {
            get
            {
                if (this.Exception != null)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 例外情報取得
        /// </summary>
        public Exception Exception
        {
            get
            {
                return this.inExp;
            }
        }
        private Exception inExp = null;
        /// <summary>
        /// 終了イベント
        /// </summary>
        public readonly AutoResetEvent Event=new AutoResetEvent(false);
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="func"> ワーカー処理デリゲート</param>
        /// <param name="prm">ワーカーパラメータ</param>
        public WorkerThread(Func<T,TResult> func,T prm)
        {
            base.SafeWaitHandle = Event.SafeWaitHandle;
            this.func = func;
            this.prm = prm;
            this.StartThread();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="func"> ワーカー処理デリゲート</param>
        /// <param name="prm">ワーカーパラメータ</param>
        /// <param name="inhConcurrent">スレッド起動抑止</param>
        public WorkerThread(Func<T, TResult> func, T prm, bool inhConcurrent)
        {
            base.SafeWaitHandle = Event.SafeWaitHandle;
            this.func = func;
            this.prm = prm;
            if (inhConcurrent)
            {
                WorkerThreadEntry(this);
            }
            else
                this.StartThread();

        }
        /// <summary>
        /// スレッドの起動
        /// </summary>
        protected virtual void StartThread()
        {
            if (!ThreadPool.QueueUserWorkItem(WorkerThreadEntry, this))
            {
                throw new Exception("ワーカー起動エラー");
            }
        }
        /// <summary>
        /// ワーカースレッド
        /// </summary>
        /// <param name="prm">処理パラメータ</param>
        protected static void WorkerThreadEntry(object prm)
        {
            WorkerThread<T, TResult> wf = prm as WorkerThread<T, TResult>; 
            try
            {
               wf.m_Result = wf.func(wf.prm);
            }
            catch (Exception exp)
            {
                wf.inExp = exp;
            }
            finally
            {
                wf.Event.Set();
            }
        }

        
    }
}
