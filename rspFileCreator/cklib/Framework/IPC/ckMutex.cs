using System;
using System.Threading;

namespace cklib.Framework.IPC
{
	/// <summary>
	/// Mutexラッパークラス
	/// </summary>
	public	class ckMutex
	{
		System.Threading.Mutex	mtx=null;
		/// <summary>
		/// ﾃﾞﾌｫﾙﾄｺﾝｽﾄﾗｸﾀ
		/// </summary>
		public ckMutex()
		{
			mtx	=	new	Mutex();
		}
		/// <summary>
		/// 初期所有権取得指定付
		/// </summary>
		/// <param name="lk"></param>
		public ckMutex(bool	lk)
		{
			mtx	=	new	Mutex(lk);
		}
		/// <summary>
		/// 名前付
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lk"></param>
		public ckMutex(string name,bool lk)
		{
			mtx	=	new	Mutex(lk,name);
		}
		/// <summary>
		/// ディストラクタ
		/// </summary>
		~ckMutex()
		{
			if	(mtx!=null)
			{
				mtx.Close();
			}
		}
		/// <summary>
		/// 待機ハンドルの取得
		/// </summary>
		/// <returns></returns>
		public	WaitHandle	GetHandle()
		{
			return	mtx;
		}
		/// <summary>
		/// ロックを行う
		/// </summary>
		/// <returns></returns>
		public	bool	Lock()
		{
			return	mtx.WaitOne();
		}
		/// <summary>
		/// ロックを行う
		/// </summary>
		/// <param name="wt">待機時間(ミリ秒)</param>
		/// <returns></returns>
		public	bool	Lock(int	wt)
		{
			return	mtx.WaitOne(wt,false);
		}
        /// <summary>
        /// ミューテックス解放
        /// </summary>
		public	void	UnLock()
		{
			mtx.ReleaseMutex();
		}
	}
}