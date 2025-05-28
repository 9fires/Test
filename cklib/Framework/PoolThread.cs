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
    public abstract class PoolThread : PoolThreadBase<EventCode, object>
	{
        #region �R���X�g���N�^�E�f�X�g���N�^
        /// <summary>
		///	�R���X�g���N�^
		/// </summary>
		public PoolThread()
            : base(EventCode.Start, EventCode.Stop, EventCode.Data, true)
        {
		}
        #endregion �R���X�g���N�^�E�f�X�g���N�^
	}
}
