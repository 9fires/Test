using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using cklib;
using cklib.Framework.IPC;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace cklib.Framework
{
    /// <summary>
    /// イベントハンドラ定義
    /// </summary>
    public delegate void AppThreadBaseEventHandler<TAppThreadType, TEventDataType>(TAppThreadType sender, TEventDataType ed);
    /// <summary>
    /// イベントハンドラ定義(Function型)
    /// </summary>
    public delegate TResult AppThreadBaseEventFuncHandler<TAppThreadType, TEventDataType, TResult>(TAppThreadType sender, TEventDataType ed);

    /// <summary>
    /// ckAppThread の概要の説明です。
    /// </summary>
    /// <remarks>
    /// 更新:2008/02/26 Queue段数制御機能追加
    /// 更新:2014/03/25 時間監視ユーティリティ機能追加
    /// </remarks>
    public abstract class AppThreadBase<TEventCode, TEventData> : IIPCControl<TEventCode, TEventData>, IDisposable
        where TEventCode : struct
        where TEventData : class
    {
#if DEBUG
        /// <summary>
        /// デバッグ用ログインスタンス
        /// </summary>
        static private cklib.Log.Logger dbglog = new Log.Logger();
#endif
        #region	基底クラス公開メンバー
        /// <summary>
        /// このスレッドの名称
        /// </summary>
        public string Name
        {
            get
            {
                if (this.thread != null)
                {
                    return this.thread.Name;
                }
                else
                {
                    return this.m_Name;
                }
            }
            set
            {
                if (this.thread != null)
                {
                    this.thread.Name = value;
                }
                else
                {
                    this.m_Name = value;
                }
            }
        }
        private string m_Name = "";
        /// <summary>
        /// バックグラウンド実行
        /// </summary>
        public bool IsBackground
        {
            get
            {
                if (this.thread != null)
                    return this.thread.IsBackground;
                return this.m_Background;
            }
            set
            {
                if (this.thread != null)
                   this.thread.IsBackground = value;
                this.m_Background = value;
            }
        }
        /// <summary>
        /// バックグラウンド実行
        /// </summary>
        private bool m_Background = false;
        /// <summary>
        /// IPCイベントハンドラ
        /// </summary>
        /// <remarks>
        /// Form等QUEからデータを拾うと、ブロックされてしまう場合にQUEの変わりにデリゲートにより通知を行なうハンドラ。<br/>
        /// このハンドラが、設定されていないとFromQueにイベントが積まれてしまうので、スレッドを起動する前に設定すること。
        /// </remarks>
        public event AppThreadBaseEventHandler<AppThreadBase<TEventCode, TEventData>, EventDataTypeBase<TEventCode,TEventData>> IPCEvent = null;
        #endregion
        #region	公開インナークラスと型
        #endregion
        #region protectedメンバー
        /// <summary>
        /// IPCインスタンス
        /// </summary>
        protected readonly IPCControl<TEventCode, TEventData> IPCControl = null;
        /// <summary>
        /// イベントテーブル項目定義
        /// </summary>
        protected struct EventTableItem
        {
            /// <summary>
            /// イベントID
            /// </summary>
            public int EventID;
            /// <summary>
            /// イベントハンドル
            /// </summary>
            public WaitHandle Handle;
        };
        /// <summary>
        /// イベントテーブル
        /// </summary>
        protected SortedList<int,EventTableItem> EventTable;
        /// <summary>
        /// スレッドインスタンス
        /// </summary>
        protected System.Threading.Thread thread = null;
        #endregion
        #region privateメンバー
        /// <summary>
        /// 開始イベントコード
        /// </summary>
        protected readonly TEventCode evcStart;
        /// <summary>
        /// 停止イベントコード
        /// </summary>
        protected readonly TEventCode evcStop;
        /// <summary>
        /// データイベントコード
        /// </summary>
        protected readonly TEventCode evcData;
        #endregion
        #region コンストラクタ・デストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="evcStart">開始イベントコード</param>
        /// <param name="evcStop">停止イベントコード</param>
        /// <param name="evcData">データイベントコード</param>
        /// <param name="fUseFromQue">応答Queueの利用有無</param>
        public AppThreadBase(TEventCode evcStart, TEventCode evcStop, TEventCode evcData, bool fUseFromQue)
        {
            this.evcStart = evcStart;
            this.evcStop = evcStop;
            this.evcData = evcData;
            this.IPCControl = this.CreateIPCControl(fUseFromQue);
            this.IPCControl.IPCEventHandler += (ed) =>
                {
                    return this.EventParent(ed);
                };
            this.IPCControl.IPCAfterCheckEventHandler += (ed) =>
                {
                    return this.AfterCheckEventParent(ed);
                };
            EventTable = new SortedList<int, EventTableItem>();
        }

        /// <summary>
        /// IPCインスタンスを生成する
        /// </summary>
        /// <param name="fUseFromQue">応答Queueの利用有無</param>
        /// <returns></returns>
        protected virtual IPCControl<TEventCode, TEventData> CreateIPCControl(bool fUseFromQue)
        {
            return new IPCControl<TEventCode, TEventData>(fUseFromQue);
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~AppThreadBase()
        {
            Dispose(false);
        }
        #endregion コンストラクタ・デストラクタ
        #region スレッド起動･停止制御メンバー
        /// <summary>
        /// スレッドの起動
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            thread = new Thread(new ThreadStart(ThreadMain));
            thread.IsBackground = this.IsBackground;
            SetupThreadName();
            if (this.PreStart())
            {
                thread.Start();
                return true;
            }
            return false;
        }
        /// <summary>
        /// スレッド名を設定
        /// </summary>
        protected virtual void SetupThreadName()
        {
            if (this.m_Name.Length == 0)
                thread.Name = this.GetType().ToString();
            else
                thread.Name = this.m_Name;
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
        public virtual bool Stop()
        {
            return Stop(-1);
        }
        /// <summary>
        ///	スレッドの稼動生存状態を示す
        /// </summary>
        public virtual bool IsAlive
        {
            get
            {
                if (thread != null)
                {
                    return thread.IsAlive;
                }
                return false;
            }
        }
        /// <summary>
        /// スレッドの停止
        /// </summary>
        /// <param name="wTime">待ち時間(ミリ秒)</param>
        /// <returns></returns>
        public virtual bool Stop(int wTime)
        {
            if (thread != null)
            {
                IPCPut(this.evcStop, default(TEventData));
                if (wTime != 0)
                {
                    return StopWait(wTime);
                }
            }
            return true;
        }
        /// <summary>
        /// スレッド停止待ち
        /// </summary>
        /// <returns></returns>
        public virtual bool StopWait()
        {
            return StopWait(-1);
        }
        /// <summary>
        /// スレッド停止待ち
        /// </summary>
        /// <param name="wTime">待ち時間(ミリ秒)</param>
        /// <returns></returns>
        public virtual bool StopWait(int wTime)
        {
            if (thread != null)
            {
                try
                {
                    if (thread.IsAlive)
                    {
                        if (wTime == -1)
                        {
                            thread.Join();
                        }
                        else
                        {
                            if (!thread.Join(wTime))
                                return false;
                        }
                    }
                }
                catch	//(Exception	exp)
                {
                }
                thread = null;
            }
            StopAfterRecovery();
            return true;
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
                return this.IPCControl.IPCQueueMaxSize;
            }
            set
            {
                this.IPCControl.IPCQueueMaxSize = value;
            }
        }
        /// <summary>
        /// スレッドにイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(TEventCode ev)
        {
            return IPCPut(ev, default(TEventData));
        }
        /// <summary>
        /// スレッドにイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <param name="data">イベントデータ</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(TEventCode ev, TEventData data)
        {
            return this.IPCControl.IPCPut(ev, data);
        }
        /// <summary>
        /// スレッドからこのスレッドの監視処理に対してイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <returns>true/false</returns>
        protected virtual bool IPCResp(TEventCode ev)
        {
            return this.IPCResp(ev, null);
        }

        /// <summary>
        /// スレッドにイベントを通知する
        /// </summary>
        /// <param name="evd">イベント情報</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(EventDataTypeBase<TEventCode, TEventData> evd)
        {
            return this.IPCControl.IPCPut(evd);
        }

        /// <summary>
        /// スレッドからこのスレッドの監視処理に対してイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <param name="data">イベントデータ</param>
        /// <returns>true/false</returns>
        protected virtual bool IPCResp(TEventCode ev, TEventData data)
        {
            if (this.IPCEvent!=null)
            {
                EventDataTypeBase<TEventCode, TEventData> evd;
                evd.EventCode = ev;
                evd.EventData = data;
                this.IPCEvent.BeginInvoke(this, evd, new AsyncCallback(IPCEventCallback), IPCEvent);
                return true;
            }
            else
                return this.IPCControl.IPCResp(ev, data);
        }
        /// <summary>
        /// 非同期デリゲート結果を受け取る
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void IPCEventCallback(IAsyncResult ar)
        {
            AppThreadBaseEventHandler<AppThreadBase<TEventCode, TEventData>, EventDataTypeBase<TEventCode,TEventData>> handler
                            = ar.AsyncState as AppThreadBaseEventHandler<AppThreadBase<TEventCode, TEventData>, EventDataTypeBase<TEventCode,TEventData>>;
            handler.EndInvoke(ar);
        }

        /// <summary>
        /// スレッド発のイベントの取得
        /// </summary>
        /// <returns></returns>
        public EventDataTypeBase<TEventCode,TEventData> GetIPCEvent()
        {
            return this.IPCControl.GetIPCEvent();
        }
        /// <summary>
        /// 待機イベントハンドルを取得する
        /// </summary>
        /// <returns></returns>
        public WaitHandle GetEventHandle()
        {
            return this.IPCControl.FromQue.GetHandle();
        }

        #endregion
        #region イベント監視用メンバー
        /// <summary>
        /// 待機イベントの追加
        /// </summary>
        /// <param name="id">イベントID</param>
        /// <param name="ev">待機ハンドル</param>
        protected void AddEventList(int id, WaitHandle ev)
        {
            EventTableItem ei;
            ei.EventID = id;
            ei.Handle = ev;
            if (this.EventTable.ContainsKey(ei.EventID))
                this.EventTable[ei.EventID] = ei;
            else
                this.EventTable.Add(ei.EventID, ei);
        }
        /// <summary>
        /// 待機イベントの削除
        /// </summary>
        /// <param name="id">イベントID</param>
        protected void RemoveEventList(int id)
        {
            if (this.EventTable.ContainsKey(id))
                this.EventTable.Remove(id);
        }
        /// <summary>
        /// 待機イベントのハンドル配列の取得
        /// </summary>
        /// <returns>イベント配列</returns>
        protected WaitHandle[] GetEventHandles()
        {
            WaitHandle[] wh = new WaitHandle[EventTable.Count];
            int i=0;
            foreach (EventTableItem ei in EventTable.Values)
            {
                wh[i++] = ei.Handle;
            }
            return wh;
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
                return WaitHandle.WaitAny(GetEventHandles(), WaitTime, false);
            }
            catch (Exception e)
            {
                return WaitError(e);
            }
        }
        #endregion
        #region 待機時間制御用のユティリティメンバー

        #region Timerクラスへ移行互換の為の中継メソッド
        /// <summary>
        /// TickCountの取得
        /// </summary>
        /// <returns></returns>
        public static int GetTickCount()
        {
            return Timer.GetTickCount();
        }
        /// <summary>
        /// 待機時間の計算<br/>
        /// 計算結果が、MinimumTimeより短い時間の場合は、計算結果をそうでなければ、MinimumTimeを返す
        /// </summary>
        /// <param name="StartTime">監視開始時間ミリ秒</param>
        /// <param name="WaitTime">待機時間ミリ秒</param>
        /// <param name="MinimumTime">他の監視時間ミリ秒</param>
        /// <returns>待機時間ミリ秒</returns>
        public static int CalcWaitTime(int StartTime, int WaitTime, int MinimumTime)
        {
            return Timer.CalcWaitTime(StartTime, WaitTime, MinimumTime);
        }
        /// <summary>
        /// 待機時間の計算
        /// </summary>
        /// <param name="StartTime">監視開始時間ミリ秒</param>
        /// <param name="WaitTime">待機時間ミリ秒</param>
        /// <returns>待機時間ミリ秒</returns>
        public static int CalcWaitTime(int StartTime, int WaitTime)
        {
            return Timer.CalcWaitTime(StartTime, WaitTime);
        }
        /// <summary>
        /// 待機時間を経過したかの判定を行なう
        /// </summary>
        /// <param name="StartTime">監視開始時間ミリ秒</param>
        /// <param name="WaitTime">待機時間ミリ秒</param>
        /// <returns>待機時間を過ぎていればtrue</returns>
        public static bool IsWaitTimeOut(int StartTime, int WaitTime)
        {
            return Timer.IsWaitTimeOut(StartTime, WaitTime);
        }
        #endregion

        #region 時間監視ユーティリティ
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Interval">インターバルタイマー</param>
        public void AddTimerEvent(TEventCode code, int EventTime, bool Interval = false)
        {
            this.IPCControl.AddTimerEvent(code, EventTime, Interval);
        }
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="Interval">インターバルタイマー</param>
        public void AddTimerEvent(TEventCode code, int EventTime, object Context, bool Interval)
        {
            this.IPCControl.AddTimerEvent(code, EventTime, Context, Interval);
        }
        /// <summary>
        /// タイマーイベント削除
        /// </summary>
        /// <param name="code">EventCode</param>
        public void RemoveTimerEvent(TEventCode code)
        {
            this.IPCControl.RemoveTimerEvent(code);
        }

        #endregion
        #endregion
        #region 内部イベントルーチン
        /// <summary>
        /// イベントメインルーチン
        /// </summary>
        /// <param name="idx">EventTable上のインデックス</param>
        /// <returns>falseスレッドの終了</returns>
        protected bool EventMain(int idx)
        {
            try
            {
                return Event((EventTableItem)EventTable[idx]);
            }
            catch (Exception e)
            {
                return EventError(e);
            }
        }
        /// <summary>
        /// スレッド終了後イベントメインルーチン
        /// </summary>
        /// <param name="idx">EventTable上のインデックス</param>
        /// <returns>falseスレッドの終了</returns>
        protected bool AfterCheckEventMain(int idx)
        {
            try
            {
                return AfterCheckEvent((EventTableItem)EventTable[idx]);
            }
            catch (Exception e)
            {
                return EventError(e);
            }
        }
        #endregion
        #region スレッド制御
        /// <summary>
        /// スレッドメインルーチン
        /// </summary>
        public virtual void ThreadMain()
        {
            try
            {
                AppThreadBaseListStore.Add<TEventCode, TEventData>(this);
                try
                {
                    if (InitInstanse())
                    {
                        if (PreIdle())
                        {
                            this.ThreadIdleLoop();
                        }
                    }
                    ExitInstance();
                }
                catch (Exception exp)
                {
                    ThreadError(exp);
                }
                IPCResp(this.evcStop, default(TEventData));
            }
            finally
            {
                AppThreadBaseListStore.Remove();
            }
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
        /// <summary>
        /// スレッド終了後の後処理
        /// </summary>
        public virtual void StopAfterRecovery()
        {
            this.AfterCheckEvent();
        }
        /// <summary>
        /// スレッド終了後の残留イベント処理
        /// </summary>
        public virtual void AfterCheckEvent()
        {
            //  メッセージキューの後処理
            while (this.IPCControl.ToQue.IsDataRegident())
            {
                try
                {
                    this.IPCControl.AfterCheckEventIPC();
                }
                catch
                { }
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
        /// <param name="exp"></param>
        /// <returns></returns>
        protected abstract bool EventError(Exception exp);
        /// <summary>
        /// catchされていない例外発生時処理
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected abstract void ThreadError(Exception exp);
        #endregion
        #region 起動初期化・後処理
        /// <summary>
        /// スレッドの初期化処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitInstanse()
        {
            AddEventList(0, this.IPCControl.ToQue.GetHandle());
            return true;
        }
        /// <summary>
        /// スレッドの初期化後アイドル前処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreIdle()
        {
            return true;
        }
        /// <summary>
        /// スレッドの終了処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool ExitInstance()
        {
            return true;
        }
        #endregion
        #region アイドル中処理
        /// <summary>
        /// イベント発生前処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool BeforeIdle()
        {
            return false;
        }
        /// <summary>
        /// イベント発生後処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool AfterIdle()
        {
            return this.TimeoutCheck();
        }
        /// <summary>
        /// 待機タイムアウト時間をミリ秒で返す
        /// </summary>
        /// <returns></returns>
        protected virtual int GetWaitTime()
        {
            return this.GetWaitTime(Timer.GetTickCount(), Timeout.Infinite);
        }

        /// <summary>
        /// 待機タイムアウト時間をミリ秒で返す
        /// </summary>
        /// <returns></returns>
        protected virtual int GetWaitTime(int now,int userTimer)
        {
            return this.IPCControl.GetWaitTime(now, userTimer);
        }

        /// <summary>
        /// イベント発生タイムアウト時の処理
        /// </summary>
        /// <returns></returns>
        protected virtual bool EventTimeout()
        {
            return this.TimeoutCheck();
        }
        /// <summary>
        /// タイムアウトチェック
        /// </summary>
        /// <returns></returns>
        protected virtual bool TimeoutCheck()
        {
            return this.IPCControl.TimeoutCheck();
        }
        #endregion
        #region イベント処理
        /// <summary>
        /// イベント処理
        /// </summary>
        /// <param name="ei">イベント</param>
        /// <returns></returns>
        protected virtual bool Event(EventTableItem ei)
        {
            if (ei.EventID == 0)
            {
                return this.IPCControl.EventIPC();
            }
            return true;
        }
        /// <summary>
        /// 親スレッドからのイベント
        /// </summary>
        /// <returns></returns>
        protected virtual bool EventParent(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            if (ed.EventCode.Equals(this.evcStart))
            {
                return EventStart(ed);                
            }
            else if (ed.EventCode.Equals(this.evcStop))
            {
                return EventStop(ed);
            }
            else if (ed.EventCode.Equals(this.evcData))
            {
                return EventData(ed);
            }
            else
            {
                return EventUser(ed);
            }
        }

        /// <summary>
        /// スレッド開始イベント（オプション　起動処理同期用)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStart(EventDataTypeBase<TEventCode,TEventData> ed)
        {
            return true;
        }
        /// <summary>
        /// スレッド停止イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStop(EventDataTypeBase<TEventCode,TEventData> ed)
        {
            return false;
        }
        /// <summary>
        /// データイベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventData(EventDataTypeBase<TEventCode,TEventData> ed)
        {
            return true;
        }
        /// <summary>
        /// ユーザー拡張イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventUser(EventDataTypeBase<TEventCode,TEventData> ed)
        {
            return true;
        }
        #endregion
        #region スレッド終了の残留イベント処理
        /// <summary>
        /// スレッド終了の残留イベント処理
        /// </summary>
        /// <param name="ei">イベント</param>
        /// <returns></returns>
        protected virtual bool AfterCheckEvent(EventTableItem ei)
        {
            if (ei.EventID == 0)
            {
                return this.IPCControl.AfterCheckEventIPC();
            }
            return true;
        }
        /// <summary>
        /// スレッド終了の残留親スレッドからのイベント
        /// </summary>
        /// <returns></returns>
        protected virtual bool AfterCheckEventParent(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            if (ed.EventCode.Equals(this.evcStart))
            {
                return AfterCheckEventStart(ed);
            }
            else if (ed.EventCode.Equals(this.evcStop))
            {
                return AfterCheckEventStop(ed);
            }
            else if (ed.EventCode.Equals(this.evcData))
            {
                return AfterCheckEventData(ed);
            }
            else
            {
                return AfterCheckEventUser(ed);
            }
        }

        /// <summary>
        /// スレッド終了の残留スレッド開始イベント（オプション　起動処理同期用)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool AfterCheckEventStart(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            return true;
        }
        /// <summary>
        /// スレッド終了の残留スレッド停止イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool AfterCheckEventStop(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            return false;
        }
        /// <summary>
        /// スレッド終了の残留データイベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool AfterCheckEventData(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            return true;
        }
        /// <summary>
        /// スレッド終了の残留ユーザー拡張イベント
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool AfterCheckEventUser(EventDataTypeBase<TEventCode, TEventData> ed)
        {
            return true;
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
                //	スレッド停止処理
                Stop();
                if (disposing)
                {
                    ReleseManagedResorce();
                }
                ReleseResorce();
                this.IPCControl.Dispose();
            }
            disposed = true;
        }
        /// <summary>
        /// リソース解放処理
        /// </summary>
        protected virtual void ReleseResorce()
        {
        }
        /// <summary>
        /// マネージドリソース解放処理（明示的呼び出し時のみ実行される）
        /// </summary>
        protected virtual void ReleseManagedResorce()
        {
        }

        #endregion
    }
}
