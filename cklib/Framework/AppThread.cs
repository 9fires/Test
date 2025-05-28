#define NEWTYPE
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using cklib;
using cklib.Framework.IPC;
using System.Runtime.InteropServices;

namespace cklib.Framework
{
    #region newlogic
    #if NEWTYPE
    /// <summary>
    /// 互換用クラス定義
    /// </summary>
    public abstract class AppThread : AppThreadBase<EventCode, object>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AppThread()
            : base(EventCode.Start, EventCode.Stop, EventCode.Data,true)
        {
        }
        /// <summary>
        /// スレッド開始イベント（オプション　起動処理同期用)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventStart(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventStart(eed);
        }
        /// <summary>
        /// スレッド開始イベント（オプション　起動処理同期用)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStart(EventDataType ed)
        {
            return true;
        }
        /// <summary>
        /// スレッド停止イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventStop(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventStop(eed);
        }
        /// <summary>
        /// スレッド停止イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStop(EventDataType ed)
        {
            return false;
        }
        /// <summary>
        /// データイベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventData(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventData(eed);
        }
        /// <summary>
        /// データイベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventData(EventDataType ed)
        {
            return true;
        }
        /// <summary>
        /// ユーザー拡張イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventUser(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventUser(eed);
        }
        /// <summary>
        /// ユーザー拡張イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventUser(EventDataType ed)
        {
            return true;
        }

    }
    #endif
    #endregion
    #region old
    #if !NEWTYPE
    /// <summary>
	/// イベントハンドラ定義
	/// </summary>
    public delegate void AppThreadEventHandler(AppThread sender,EventDataType  ed);
	/// <summary>
	/// ckAppThread の概要の説明です。
	/// </summary>
    /// <remarks>
    /// 更新:2008/02/26 Queue段数制御機能追加
    /// </remarks>
    public abstract class AppThread : IDisposable
	{
		#region	基底クラス公開メンバー
		/// <summary>
		/// このスレッドの名称
		/// </summary>
		public	String	Name;
		/// <summary>
		/// IPCイベントハンドラ
		/// </summary>
		/// <remarks>
		/// Form等QUEからデータを拾うと、ブロックされてしまう場合にQUEの変わりにデリゲートにより通知を行なうハンドラ。<br/>
		/// このハンドラが、設定されていないとFromQueにイベントが積まれてしまうので、スレッドを起動する前に設定すること。
		/// </remarks>
		public	event	AppThreadEventHandler	IPCEvent=null;
		#endregion
		#region	公開インナークラスと型
		#endregion
		#region protectedメンバー
		/// <summary>
		/// IPC上りイベントQUE
		/// </summary>
		protected	ckEventQue<EventDataType>	FromQue=null;
		/// <summary>
		/// IPC下りイベントQUE
		/// </summary>
		protected	ckEventQue<EventDataType>	ToQue=null;
		/// <summary>
		/// イベントテーブル項目定義
		/// </summary>
		protected	struct	EventTableItem
		{
            /// <summary>
            /// イベントID
            /// </summary>
			public	int			EventID;
            /// <summary>
            /// イベントハンドル
            /// </summary>
			public	WaitHandle	Handle;
		};
		/// <summary>
		/// イベントテーブル
		/// </summary>
		protected	ArrayList	EventTable;
		/// <summary>
		/// スレッドインスタンス
		/// </summary>
		protected	System.Threading.Thread thread=null;
		#endregion
        #region コンストラクタ・デストラクタ
        /// <summary>
		///	コンストラクタ
		/// </summary>
		public AppThread()
		{
            FromQue = new ckEventQue<EventDataType>();
            ToQue = new ckEventQue<EventDataType>();
			EventTable=	new	ArrayList();
		}
		/// <summary>
		/// ディストラクタ
		/// </summary>
		~AppThread()
		{
			Dispose(false);
        }
        #endregion コンストラクタ・デストラクタ
        #region スレッド起動･停止制御メンバー
        /// <summary>
		/// スレッドの起動
		/// </summary>
		/// <returns></returns>
		public virtual  bool	Start()
		{
			thread = new Thread(new ThreadStart(ThreadMain));
            thread.Name =   this.GetType().ToString();
            if (this.PreStart())
            {
                thread.Start();
                return true;
            }
            return false;
		}
        /// <summary>
        /// スレッド開始直前設定処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreStart()
        {
            return true;
        }
		/// <summary>
		/// スレッドの停止
		/// </summary>
		/// <returns></returns>
		public	virtual bool Stop()
		{
			return	Stop(-1);
		}
		/// <summary>
		///	スレッドの稼動生存状態を示す
		/// </summary>
		public	virtual bool	IsAlive
		{
			get
            {
                if (thread!=null)
                { 
                    return	thread.IsAlive;
                }
                return false;
            }
		}
		/// <summary>
		/// スレッドの停止
		/// </summary>
		/// <param name="wTime">待ち時間(ミリ秒)</param>
		/// <returns></returns>
		public	virtual bool Stop(int	wTime)
		{
			if	(thread!=null)
			{
				IPCPut(EventCode.Stop,null);
				if	(wTime!=0)
				{
					return	StopWait(wTime);
				}
			}
			return	true;
		}
		/// <summary>
		/// スレッド停止待ち
		/// </summary>
		/// <returns></returns>
		public	virtual bool	StopWait()
		{
			return	StopWait(-1);
		}
		/// <summary>
		/// スレッド停止待ち
		/// </summary>
		/// <param name="wTime">待ち時間(ミリ秒)</param>
		/// <returns></returns>
		public	virtual bool	StopWait(int	wTime)
		{
			if	(thread!=null)
			{
				try
				{
					if	(thread.IsAlive)
					{
						if	(wTime==-1)
						{
							thread.Join();
						}
						else
						{
							if	(!thread.Join(wTime))
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
		#region スレッドIPC用メンバー
        /// <summary>
        /// IPCQueueサイズ上限の設定取得
        /// </summary>
        public int IPCQueueMaxSize
        {
            get
            {
                return this.ToQue.QueueMaxSize;
            }
            set
            {
                this.ToQue.QueueMaxSize = value;
            }
        }
		/// <summary>
		/// スレッドにイベントを通知する
		/// </summary>
		/// <param name="ev">イベントコード</param>
		/// <returns>true/false</returns>
		public	virtual	bool	IPCPut(EventCode	ev)
		{
			return	IPCPut(ev,null);
		}
		/// <summary>
		/// スレッドにイベントを通知する
		/// </summary>
		/// <param name="ev">イベントコード</param>
		/// <param name="data">イベントデータ</param>
		/// <returns>true/false</returns>
		public	virtual	bool	IPCPut(EventCode	ev,object data)
		{
			EventDataType	evd;
			evd.EventCode	=	ev;
			evd.EventData	=	data;
			ToQue.Put(evd);
			return	true;
		}
		/// <summary>
		/// スレッドからこのスレッドの監視処理に対してイベントを通知する
		/// </summary>
		/// <param name="ev">イベントコード</param>
		/// <returns>true/false</returns>
		protected	virtual	bool	IPCResp(EventCode	ev)
		{
			return	IPCResp(ev,null);
		}
		/// <summary>
		/// スレッドからこのスレッドの監視処理に対してイベントを通知する
		/// </summary>
		/// <param name="ev">イベントコード</param>
		/// <param name="data">イベントデータ</param>
		/// <returns>true/false</returns>
		protected	virtual	bool	IPCResp(EventCode	ev,object data)
		{
			EventDataType	evd;
			evd.EventCode	=	ev;
			evd.EventData	=	data;
			if	(this.IPCEvent==null)
			{
				FromQue.Put(evd);
			}
			else
			{	//	イベントハンドラの呼び出し	非同期ディゲートで行なう
				this.IPCEvent.BeginInvoke(this,evd,new AsyncCallback(IPCEventCallback),IPCEvent);
			}
			return	true;
		}
		/// <summary>
		/// 非同期デリゲート結果を受け取る
		/// </summary>
		/// <param name="ar"></param>
		protected	virtual	void	IPCEventCallback(IAsyncResult ar) 
		{
			AppThreadEventHandler	handler	=	(AppThreadEventHandler)ar.AsyncState;
			handler.EndInvoke(ar);
		}

		/// <summary>
		/// スレッド発のイベントの取得
		/// </summary>
		/// <returns></returns>
		public	EventDataType	GetIPCEvent()
		{
			if	(FromQue.IsDataRegident())
				return	(EventDataType)FromQue.Get();
			else
			{
				return	new	EventDataType();
			}
		}
		/// <summary>
		/// 待機イベントハンドルを取得する
		/// </summary>
		/// <returns></returns>
		public	WaitHandle GetEventHandle()
		{
			return	FromQue.GetHandle();
		}
		/// <summary>
		/// 親スレッドからのイベント取り出し
		/// </summary>
		/// <returns></returns>
		protected	EventDataType	IPCParentEventGet()
		{
			return	(EventDataType)ToQue.Get();
		}
		/// <summary>
		/// 親スレッドからのイベントの有無の参照
		/// </summary>
		/// <returns></returns>
		protected	bool	IPCParentEventRedient()
		{
			return	ToQue.IsDataRegident();
		}

		#endregion
		#region イベント監視用メンバー
		/// <summary>
		/// 待機イベントの追加
		/// </summary>
		/// <param name="id">イベントID</param>
		/// <param name="ev">待機ハンドル</param>
		protected	void	AddEventList(int	id,WaitHandle	ev)
		{
			int	i;
			EventTableItem	ei;
			ei.EventID	=	id;
			ei.Handle	=	ev;
			for	(i=0;i<EventTable.Count;i++)
			{
				if	(((EventTableItem)EventTable[i]).EventID==id)
				{
					EventTable[i]	=	ev;
					return;
				}
				else
				if	(((EventTableItem)EventTable[i]).EventID>id)
				{
					EventTable.Insert(i,ei);
					return;
				}
			}
			EventTable.Add(ei);
		}
		/// <summary>
		/// 待機イベントの削除
		/// </summary>
		/// <param name="id">イベントID</param>
		protected	void	RemoveEventList(int	id)
		{
			int	i;
			for	(i=0;i<EventTable.Count;i++)
			{
				if	(((EventTableItem)EventTable[i]).EventID==id)
				{
					EventTable.RemoveAt(i);
					break;
				}
			}
		}
		/// <summary>
		/// 待機イベントのハンドル配列の取得
		/// </summary>
		/// <returns>イベント配列</returns>
		protected	WaitHandle[]	GetEventHandles()
		{
			WaitHandle[]	wh=new	WaitHandle[EventTable.Count];
			int	i;
			for	(i=0;i<EventTable.Count;i++)
			{
				wh[i]	=	((EventTableItem)EventTable[i]).Handle;
			}
			return	wh;
		}
        /// <summary>
        /// イベントの待機
        /// </summary>
        /// <param name="WaitTime">待ち時間(ミリ秒)</param>
        /// <returns>イベント状態となったハンドルのEventTable上のインデックス</returns>
        protected int EventWait(int WaitTime)
		{
			try
			{
				return	WaitHandle.WaitAny(GetEventHandles(),WaitTime,false);
			}
			catch	(Exception	e)
			{
				return	WaitError(e);
			}
		}
		#endregion
		#region 待機時間制御用のユティリティメンバー
        [DllImport("kernel32.dll", EntryPoint = "GetTickCount")]
        /// <summary>
        /// TickCountの取得
        /// </summary>
        /// <returns></returns>
        public extern static uint NativeGetTickCount();
        /// <summary>
		/// TickCountの取得
		/// </summary>
		/// <returns></returns>
		public	static	int	GetTickCount()
		{
			return	(int)NativeGetTickCount();
		}
		/// <summary>
		/// 待機時間の計算<br/>
		/// 計算結果が、MinimumTimeより短い時間の場合は、計算結果をそうでなければ、MinimumTimeを返す
		/// </summary>
		/// <param name="StartTime">監視開始時間ミリ秒</param>
		/// <param name="WaitTime">待機時間ミリ秒</param>
		/// <param name="MinimumTime">他の監視時間ミリ秒</param>
		/// <returns>待機時間ミリ秒</returns>
		public	static	int	CalcWaitTime(int	StartTime,int	WaitTime,int	MinimumTime)
		{
            if (WaitTime==Timeout.Infinite)
            {
                return Timeout.Infinite;
            }
			int	nowtm	=	GetTickCount();
			int	chktm	=	(nowtm-StartTime);
			int	wtm;
			if	(WaitTime>chktm)
			{
				wtm	=	WaitTime-chktm;
				if	(MinimumTime!=-1)
				{
					if	(MinimumTime<wtm)
					{
						return	MinimumTime;
					}
					else
					{
						return	wtm;
					}
				}
				else
				{
					return	wtm;
				}
			}
			return	0;
		}
		/// <summary>
		/// 待機時間の計算
		/// </summary>
		/// <param name="StartTime">監視開始時間ミリ秒</param>
		/// <param name="WaitTime">待機時間ミリ秒</param>
		/// <returns>待機時間ミリ秒</returns>
		public	static int	CalcWaitTime(int	StartTime,int	WaitTime)
		{
			return	CalcWaitTime(StartTime,WaitTime,WaitTime);
		}
		/// <summary>
		/// 待機時間を経過したかの判定を行なう
		/// </summary>
		/// <param name="StartTime">監視開始時間ミリ秒</param>
		/// <param name="WaitTime">待機時間ミリ秒</param>
		/// <returns>待機時間を過ぎていればtrue</returns>
		public	static	bool	IsWaitTimeOut(int	StartTime,int	WaitTime)
		{
			if	(CalcWaitTime(StartTime,WaitTime,WaitTime)==0)
				return	true;
			else
				return	false;
		}

		#endregion
		#region 内部イベントルーチン
		/// <summary>
		/// イベントメインルーチン
		/// </summary>
		/// <param name="idx">EventTable上のインデックス</param>
		/// <returns>falseスレッドの終了</returns>
		protected	bool	EventMain(int	idx)
		{
			try
			{
				return	Event((EventTableItem)EventTable[idx]);
			}
			catch	(Exception	e)
			{
				return	EventError(e);
			}
		}
		#endregion
		#region スレッド制御
		/// <summary>
		/// スレッドメインルーチン
		/// </summary>
		public	virtual void ThreadMain()
		{
            try
            {
                if (InitInstanse())
                {
                    this.ThreadIdleLoop();
                }
                ExitInstance();
            }
            catch (Exception exp)
            {
                ThreadError(exp);
            }
			IPCResp(EventCode.Stop,null);
			return;
		}
		/// <summary>
		/// スレッドアイドルループ
		/// </summary>
        public virtual void ThreadIdleLoop()
        {
            bool fLoop = true;
            for (; fLoop; )
            {
                if (BeforeIdle())
                {
                    continue;
                }
                fLoop = this.ThreadIdle(this.GetWaitTime());
            }
        }
        /// <summary>
        /// スレッドアイドル処理
        /// </summary>
        /// <param name="WaitTime"></param>
        public virtual bool ThreadIdle(int WaitTime)
        {
            int ev = EventWait(WaitTime);
            switch (ev)
            {
                case WaitHandle.WaitTimeout:	//	タイムアウト
                    return EventTimeout();
                case -1:		//	待機エラー
                    return false;
                default:	//	イベント発生
                    if (EventMain(ev))
                        return AfterIdle();
                    else
                        return false;
            }

        }
		#endregion
		#region オーバーライドメンバー
		#region 例外処理のオーバーライド
		/// <summary>
		/// 待機中の例外発生時の処理
		/// </summary>
		/// <returns></returns>
        protected abstract int WaitError(Exception exp);
		/// <summary>
		/// イベント処理中の例外発生時処理
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
        protected abstract bool EventError(Exception exp);
        /// <summary>
        /// catchされていない例外発生時処理
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected abstract void ThreadError(Exception exp);
        #endregion
		#region 起動初期化・後処理
		/// <summary>
		/// スレッドの初期化処理
		/// </summary>
		/// <returns></returns>
		protected	virtual bool	InitInstanse()
		{
			AddEventList(0,ToQue.GetHandle());
			return	true;
		}
		/// <summary>
		/// スレッドの終了処理
		/// </summary>
		/// <returns></returns>
		protected	virtual bool	ExitInstance()
		{
			return	true;
		}
		#endregion
		#region アイドル中処理
		/// <summary>
		/// イベント発生前処理
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	BeforeIdle()
		{
			return	false;
		}
		/// <summary>
		/// イベント発生後処理
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	AfterIdle()
		{
			return	true;
		}
		/// <summary>
		/// 待機タイムアウト時間をミリ秒で返す
		/// </summary>
		/// <returns></returns>
		protected	virtual	int	GetWaitTime()
		{
            return Timeout.Infinite;
		}
		/// <summary>
		/// イベント発生タイムアウト時の処理
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	EventTimeout()
		{
			return	true;
		}
		#endregion
		#region イベント処理
		/// <summary>
		/// イベント処理
		/// </summary>
		/// <param name="ei">イベント</param>
		/// <returns></returns>
		protected	virtual	bool	Event(EventTableItem	ei)
		{
			if	(ei.EventID==0)
			{
				return	EventParent();		
			}
			return	true;
		}
		/// <summary>
		/// 親スレッドからのイベント
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	EventParent()
		{
			EventDataType	ed	=	this.IPCParentEventGet();
			switch	(ed.EventCode)
			{
				case	EventCode.Start:
					return	EventStart(ed);
				case	EventCode.Stop:
					return	EventStop(ed);
				case	EventCode.Data:
					return	EventData(ed);
				default:
					return	EventUser(ed);
			}
//			return	true;
		}

		/// <summary>
		/// スレッド開始イベント（オプション　起動処理同期用)
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventStart(EventDataType ed)
		{
			return	true;
		}
		/// <summary>
		/// スレッド停止イベント
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventStop(EventDataType ed)
		{
			return	false;
		}
		/// <summary>
		/// データイベント
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventData(EventDataType ed)
		{
			return	true;
		}
		/// <summary>
		/// ユーザー拡張イベント
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventUser(EventDataType ed)
		{
			return	true;
		}
		#endregion
		#endregion
		#region IDisposable メンバ
		/// <summary>
		/// Dispose完了フラグ
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Disposeメソッド
		/// </summary>
		public virtual  void Dispose()
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
			if(!disposed)
			{
				//	スレッド停止処理
				Stop();
				if(disposing)
				{
					ReleseManagedResorce();
				}
				ReleseResorce();
				this.ToQue.Dispose();
				this.FromQue.Dispose();
			}
			disposed = true;         
		}
		/// <summary>
		/// リソース解放処理
		/// </summary>
		protected	virtual	void	ReleseResorce()
		{
		}
		/// <summary>
		/// マネージドリソース解放処理（明示的呼び出し時のみ実行される）
		/// </summary>
		protected	virtual	void	ReleseManagedResorce()
		{
		}

		#endregion
	}
    #endif
    #endregion
}
