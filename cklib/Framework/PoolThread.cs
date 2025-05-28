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
    public abstract class PoolThread : PoolThreadBase<EventCode, object>
	{
        #region コンストラクタ・デストラクタ
        /// <summary>
		///	コンストラクタ
		/// </summary>
		public PoolThread()
            : base(EventCode.Start, EventCode.Stop, EventCode.Data, true)
        {
		}
        #endregion コンストラクタ・デストラクタ
	}
}
