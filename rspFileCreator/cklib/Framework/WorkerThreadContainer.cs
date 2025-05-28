using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading;

namespace cklib.Framework
{
    /// <summary>
    /// WorkFiber管理クラス
    /// </summary>
    public class WorkerThreadContainer<WT, T, TResult>
        where WT : WorkerThread<T, TResult>
    {
        List<WorkerThread<T, TResult>> wflist = new List<WorkerThread<T, TResult>>();
        LinkedList<WorkerThread<T, TResult>> EndList = new LinkedList<WorkerThread<T, TResult>>();

        readonly private int WorkFiberLimit = 0;
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public WorkerThreadContainer()
        {
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="WorkFiberLimit">実行数制限</param>
        public WorkerThreadContainer(int WorkFiberLimit)
        {
            this.WorkFiberLimit = WorkFiberLimit;
        }
        /// <summary>
        /// ワーカースレッド生成
        /// </summary>
        /// <param name="func"></param>
        /// <param name="prm"></param>
        /// <param name="inhConcurrent"></param>
        /// <returns></returns>
        protected virtual WorkerThread<T, TResult> CreateWorkerThread(Func<T, TResult> func, T prm, bool inhConcurrent = false)
        {
            return new WorkerThread<T, TResult>(func, prm, inhConcurrent);
        }
        /// <summary>
        /// WorkerThreadを生成して追加
        /// </summary>
        public void Add(Func<T, TResult> func, T prm)
        {
            if (this.WorkFiberLimit == 1)
                this.EndList.AddLast(this.CreateWorkerThread(func, prm, true));
            else
                this.Add(this.CreateWorkerThread(func, prm));
        }
        /// <summary>
        /// WorkerThreadを追加
        /// </summary>
        public void Add(WorkerThread<T, TResult> wf)
        {
            if (this.WorkFiberLimit != 0 && this.WorkFiberLimit <= this.wflist.Count)
            {
                WaitWorkerThread();
            }
            this.wflist.Add(wf);
        }
        /// <summary>
        /// すべてのWorkerThreadの終了を待つ
        /// </summary>
        public void WaitAll()
        {
            for (; wflist.Count!=0; )
            {
                WaitWorkerThread();
            }
        }
        /// <summary>
        /// すべてのWorkerThreadの終了を待つ
        /// </summary>
        public void WaitAll(Action<WorkerThread<T, TResult>> action)
        {
            this.WaitAll();
            if (this.IsEnded)
            {
                var wf = this.GetEndedWorkerThread();
                for (; wf != null; wf = this.GetEndedWorkerThread())
                {
                    action(wf);
                }
            }
        }
        /// <summary>
        /// WorkerThreadの終了を待つ
        /// </summary>
        public void WaitWorkerThread()
        {
            if (wflist.Count == 0)
                return;
            WaitHandle[] wh = new WaitHandle[wflist.Count];
            for (int ii = 0; ii < wflist.Count; ii++)
            {
                wh[ii] = wflist[ii];
            }
            var idx = WaitHandle.WaitAny(wh);
            EndList.AddLast(wflist[idx]);
            wflist.RemoveAt(idx);
        }
        /// <summary>
        /// 終了済みWorkerThreadの有無を確認
        /// </summary>
        public bool IsEnded
        {
            get
            {
                return (this.EndList.Count != 0);
            }
        }
        /// <summary>
        /// 終了ワーカーの処理
        /// </summary>
        public void EndWorkerCheck(Action<WorkerThread<T, TResult>> action)
        {
            if (this.IsEnded)
            {
                var wf = this.GetEndedWorkerThread();
                for (; wf != null; wf = this.GetEndedWorkerThread())
                {
                    action(wf);
                }
            }
        }
        /// <summary>
        /// 終了済みWorkerThreadを取り出し
        /// </summary>
        /// <returns></returns>
        public WorkerThread<T, TResult> GetEndedWorkerThread()
        {
            if (this.EndList.Count != 0)
            {
                var ret = this.EndList.First;
                this.EndList.RemoveFirst();
                return ret.Value;
            }
            return null;
        }
    }
}
