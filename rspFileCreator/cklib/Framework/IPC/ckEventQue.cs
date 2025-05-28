using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace  cklib.Framework.IPC
{
	/// <summary>
	/// EventObjectによるイベント監視可能なQueue
	/// </summary>
    /// <remarks>
    /// 更新:2008/02/26 Queue段数制御機能追加
    /// 更新:2008/07/08 Genericに変更
    /// </remarks>
	public class ckEventQue<T>:IDisposable
	{
        /// <summary>
        /// イベントオブジェクトインスタンス
        /// </summary>
		private	ManualResetEvent	ev	=	null;
        /// <summary>
        /// イベント保持Queue実態
        /// </summary>
		private Queue Que;
        /// <summary>
        /// Queueサイズ制限用セマフォ
        /// </summary>
        private Semaphore sem = null;
        /// <summary>
        /// Queueサイズ上限
        /// </summary>
        private int QueueLimitSize = -1;
        /// <summary>
        /// Queueサイズ上限
        /// </summary>
        /// <remarks>
        /// サイズの動的変更は不可<br/>
        /// 0以下の値で制限解除後再設定のみ可
        /// </remarks>
        public int QueueMaxSize
        {
            get
            {
                if (sem !=null)
                {
                    return QueueLimitSize;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                lock(this)
                {
                    if (sem == null)
                    {
                        if (value>=0)
                        {
                            QueueLimitSize = value;
                            sem = new Semaphore(QueueLimitSize, QueueLimitSize);
                        }
                    }
                    else
                    {
                        if (value < 0)
                        {   //  制限解除
                            sem.Close();
                            sem = null;
                        }
                        else
                        {
                            throw new Exception("Already Initialized Limit Queue Max Size");
                        }
                    }
                }
            }
        }
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ckEventQue()
		{
            Initialize(-1);
		}
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="QueueLimit">キューの段数 -1を指定した場合無制限</param>
        public ckEventQue(int QueueLimit)
        {
            Initialize(QueueLimit);
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="QueueLimit">キューの段数 -1を指定した場合無制限</param>
        private void Initialize(int QueueLimit)
        {
            QueueLimitSize = QueueLimit;
            ev = new ManualResetEvent(false);
            Que = new Queue();
            if (QueueLimit>=0)
            {
                sem = new Semaphore(QueueLimitSize, QueueLimitSize);
            }
        }
		/// <summary>
		/// デストラクタ
		/// </summary>
		~ckEventQue()
		{
			Dispose(false);
		}
		/// <summary>
		/// 待機用ハンドルを取得
		/// </summary>
		/// <returns></returns>
		public	WaitHandle	GetHandle()
		{
			lock(this)
			{
				return	ev;
			}
		}

		/// <summary>
		/// データを格納しイベントを発生させる
		/// </summary>
		/// <param name="obj">データ</param>
		public void Put(T obj)
		{
            this.Put(obj,Timeout.Infinite);
		}
        /// <summary>
        /// データを格納しイベントを発生させる
        /// </summary>
        /// <param name="obj">データ</param>
        /// <param name="WaitTime">待機時間</param>
        /// <returns>成功時true,Putタイムアウト時false</returns>
        public bool Put(T obj, int WaitTime)
        {
            Semaphore isem = this.sem;
            if (isem != null)
            {
                try
                {
                    if (!isem.WaitOne(WaitTime, false))
                    {
                        return false;
                    }
                }
                catch (ObjectDisposedException)
                {   // セマフォが破棄された
                }
            }
            lock (this)
            {
                if (ev != null)
                {
                    Que.Enqueue(obj);
                    ev.Set();
                }
            }
            return true;
        }
        /// <summary>
		/// QUEにデータが存在するか返す
		/// </summary>
		/// <returns></returns>
		public	bool	IsDataRegident()
		{
			lock(this)
			{
				if	(Que!=null)
				{
					if	(Que.Count==0)
						return	false;
					else
						return	true;
				}
				else
				{
					return	false;
				}
			}
		}
        /// <summary>
        /// 格納したデータを返す
        /// </summary>
        /// <param name="WaitTime">待機時間</param>
        /// <returns>データ</returns>
        public T Get(int WaitTime)
        {
            if (ev.WaitOne(WaitTime, false))
            {
                lock (this)
                {
                    return this.Get();
                }
            }
            return default(T);
        }
		/// <summary>
		/// 格納したデータを返す
		/// </summary>
		/// <returns>データ</returns>
		public T Get()
		{
			T	obj;
			lock(this)
			{
				if	(ev!=null)
				{
                    if (Que.Count==0)
                    {
                        return default(T);                       
                    }
					obj	=	(T)Que.Dequeue();
                    Semaphore isem = this.sem;
                    if (isem != null)
                    {
                        isem.Release();
                    }
                    if (Que.Count == 0)
					{
						Que.Clear();
						ev.Reset();
                    }
                    Que.TrimToSize();
					return obj;
				}
				else
				{
                    return default(T);
				}
			}
		}
		#region IDisposable メンバ
		/// <summary>
		/// Dispose完了フラグ
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Dispose処理の実装
		/// Queとイベントオブジェクトを破棄する
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Dispose処理の実装
		/// </summary>
		/// <param name="disposing">手動開放かディストラクタかの識別</param>
		private void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
				}
				if	(ev!=null)
				{
					ev.Close();
					ev	=	null;
				}
				if	(Que!=null)
				{
					Que.Clear();
					Que=null;
				}
                Semaphore isem = this.sem;
                if (isem != null)
                {
                    isem.Close();
                    isem = null;
                }
            }
			disposed = true;     
		}

		#endregion
	}
}
