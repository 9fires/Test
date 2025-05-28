using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using cklib;
using cklib.Framework.IPC;
using System.Runtime.InteropServices;

namespace cklib.Framework
{
	/// <summary>
	/// AppThread の概要の説明です。
	/// </summary>
	public abstract class PoolThreadBase<TEventCode,TEventData> : AppThreadBase<TEventCode, TEventData>
        where TEventCode : struct
        where TEventData : class
	{
        /// <summary>
        /// 起動監視イベント
        /// </summary>
        private AutoResetEvent StartEvent = new AutoResetEvent(false);
        /// <summary>
        /// 起動監視イベント
        /// </summary>
        private AutoResetEvent StopEvent = new AutoResetEvent(false);
        #region コンストラクタ・デストラクタ
        /// <summary>
		///	コンストラクタ
		/// </summary>
        public PoolThreadBase(TEventCode evcStart, TEventCode evcStop, TEventCode evcData, bool fUseFromQue)
            :base(evcStart, evcStop, evcData, fUseFromQue)
		{
            this.IsBackground = true;
		}
        #endregion コンストラクタ・デストラクタ
        #region スレッド起動･停止制御メンバー
        /// <summary>
		/// スレッドの起動
		/// </summary>
		/// <returns></returns>
		public override bool  Start()
		{
            if (this.PreStart())
            {
                if (this.StartPoolThread(new WaitCallback(PoolThreadMain)))
                {
                    this.StartEvent.WaitOne();
                    return true;
                }
            }
            return false;
		}
        /// <summary>
        /// ワーカー起動方のカスタマイズ
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        protected virtual bool StartPoolThread(WaitCallback callBack)
        {
            return ThreadPool.QueueUserWorkItem(callBack);
        }
		/// <summary>
		/// スレッド停止待ち
		/// </summary>
		/// <param name="wTime">待ち時間(ミリ秒)</param>
		/// <returns></returns>
        public override bool StopWait(int wTime)
		{
           
			if	(thread!=null)
			{
				try
				{
					if	(thread.IsAlive)
					{
						if	(wTime==-1)
						{
							this.StopEvent.WaitOne();
						}
						else
                        {
                            if  (this.StopEvent.WaitOne(wTime,false))
								return	false;
						}
					}
				}
				catch	//(Exception	exp)
				{
				}
				thread	=	null;
			}
			return	true;
		}
		#endregion
		#region スレッド制御
		/// <summary>
		/// スレッドメインルーチン
		/// </summary>
        public void PoolThreadMain(Object stateInfo)
        {
            try
            {
                this.thread = System.Threading.Thread.CurrentThread;
                this.thread.IsBackground = this.IsBackground;
                this.SetupThreadName();
                StartEvent.Set();
                this.ThreadMain();
                StopEvent.Set();
            }
            catch (Exception exp)
            {
                this.ThreadError(exp);
            }
            return;
        }
		#endregion
	}
}
