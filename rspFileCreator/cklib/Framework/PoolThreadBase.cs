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
	/// AppThread �̊T�v�̐����ł��B
	/// </summary>
	public abstract class PoolThreadBase<TEventCode,TEventData> : AppThreadBase<TEventCode, TEventData>
        where TEventCode : struct
        where TEventData : class
	{
        /// <summary>
        /// �N���Ď��C�x���g
        /// </summary>
        private AutoResetEvent StartEvent = new AutoResetEvent(false);
        /// <summary>
        /// �N���Ď��C�x���g
        /// </summary>
        private AutoResetEvent StopEvent = new AutoResetEvent(false);
        #region �R���X�g���N�^�E�f�X�g���N�^
        /// <summary>
		///	�R���X�g���N�^
		/// </summary>
        public PoolThreadBase(TEventCode evcStart, TEventCode evcStop, TEventCode evcData, bool fUseFromQue)
            :base(evcStart, evcStop, evcData, fUseFromQue)
		{
            this.IsBackground = true;
		}
        #endregion �R���X�g���N�^�E�f�X�g���N�^
        #region �X���b�h�N�����~���䃁���o�[
        /// <summary>
		/// �X���b�h�̋N��
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
        /// ���[�J�[�N�����̃J�X�^�}�C�Y
        /// </summary>
        /// <param name="callBack"></param>
        /// <returns></returns>
        protected virtual bool StartPoolThread(WaitCallback callBack)
        {
            return ThreadPool.QueueUserWorkItem(callBack);
        }
		/// <summary>
		/// �X���b�h��~�҂�
		/// </summary>
		/// <param name="wTime">�҂�����(�~���b)</param>
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
		#region �X���b�h����
		/// <summary>
		/// �X���b�h���C�����[�`��
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
