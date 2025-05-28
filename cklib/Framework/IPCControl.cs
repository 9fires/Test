using cklib.Framework.IPC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace cklib.Framework
{
    /// <summary>
    /// IPCControl IPC処理制御クラス
    /// </summary>
    /// <typeparam name="TEventCode">イベントコードenum定義</typeparam>
    /// <typeparam name="TEventData">イベントデータ定義</typeparam>
    public class IPCControl<TEventCode, TEventData> : IIPCControl<TEventCode, TEventData>, IDisposable
        where TEventCode : struct
        where TEventData : class
    {
        #region protectedメンバー
        /// <summary>
        /// IPC上りイベントQUE
        /// </summary>
        public readonly ckEventQue<EventDataTypeBase<TEventCode,TEventData>> FromQue = null;
        /// <summary>
        /// IPC下りイベントQUE
        /// </summary>
        public readonly ckEventQue<EventDataTypeBase<TEventCode, TEventData>> ToQue = null;
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
        /// <param name="fUseFromQue">応答Queueの利用有無</param>
        public IPCControl(bool fUseFromQue)
        {
            if (fUseFromQue)
            {
                FromQue = new ckEventQue<EventDataTypeBase<TEventCode, TEventData>>();                
            }
            ToQue = new ckEventQue<EventDataTypeBase<TEventCode,TEventData>>();
        }
        /// <summary>
        /// ディストラクタ
        /// </summary>
        ~IPCControl()
        {
            Dispose(false);
        }
        #endregion コンストラクタ・デストラクタ
        #region IPC用メンバー
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
        /// イベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(TEventCode ev)
        {
            return IPCPut(ev, default(TEventData));
        }
        /// <summary>
        /// イベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <param name="data">イベントデータ</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(TEventCode ev, TEventData data)
        {
            EventDataTypeBase<TEventCode,TEventData> evd;
            evd.EventCode = ev;
            evd.EventData = data;
            return this.IPCPut(evd);
        }
        /// <summary>
        /// イベントを通知する
        /// </summary>
        /// <param name="evd">イベント情報</param>
        /// <returns>true/false</returns>
        public virtual bool IPCPut(EventDataTypeBase<TEventCode, TEventData> evd)
        {
            ToQue.Put(evd);
            return true;
        }
        /// <summary>
        /// IPCイベントハンドラ
        /// </summary>
        /// <remarks>
        /// Form等QUEからデータを拾うと、ブロックされてしまう場合にQUEの変わりにデリゲートにより通知を行なうハンドラ。<br/>
        /// このハンドラが、設定されていないとFromQueにイベントが積まれてしまうので、スレッドを起動する前に設定すること。
        /// </remarks>
        public event AppThreadBaseEventHandler<IIPCControl<TEventCode, TEventData>, EventDataTypeBase<TEventCode, TEventData>> IPCEvent = null;
        /// <summary>
        /// このオブジェクトの監視処理に対してイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <returns>true/false</returns>
        public virtual bool IPCResp(TEventCode ev)
        {
            return IPCResp(ev, default(TEventData));
        }
        /// <summary>
        /// このオブジェクトの監視処理に対してイベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <param name="data">イベントデータ</param>
        /// <returns>true/false</returns>
        public virtual bool IPCResp(TEventCode ev, TEventData data)
        {
            EventDataTypeBase<TEventCode,TEventData> evd;
            evd.EventCode = ev;
            evd.EventData = data;
            if (this.IPCEvent == null)
            {
                if (this.FromQue!=null)
                    this.FromQue.Put(evd);
            }
            else
            {	//	イベントハンドラの呼び出し	非同期ディゲートで行なう
                this.IPCEvent.BeginInvoke(this, evd, new AsyncCallback(IPCEventCallback), IPCEvent);
            }
            return true;
        }
        /// <summary>
        /// 非同期デリゲート結果を受け取る
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void IPCEventCallback(IAsyncResult ar)
        {
            try
            {
                AppThreadBaseEventHandler<IIPCControl<TEventCode, TEventData>, EventDataTypeBase<TEventCode, TEventData>> handler
                                = ar.AsyncState as AppThreadBaseEventHandler<IIPCControl<TEventCode, TEventData>, EventDataTypeBase<TEventCode, TEventData>>;
                handler.EndInvoke(ar);
            }
            catch
            { }
        }

        /// <summary>
        /// このオブジェクトのイベントの取得
        /// </summary>
        /// <returns></returns>
        public EventDataTypeBase<TEventCode,TEventData> GetIPCEvent()
        {
            if (FromQue.IsDataRegident())
                return FromQue.Get();
            else
            {
                return new EventDataTypeBase<TEventCode,TEventData>();
            }
        }
        /// <summary>
        /// 待機イベントハンドルを取得する
        /// </summary>
        /// <returns></returns>
        public WaitHandle GetEventHandle()
        {
            return FromQue.GetHandle();
        }
        /// <summary>
        /// イベント取り出し
        /// </summary>
        /// <returns></returns>
        protected EventDataTypeBase<TEventCode,TEventData> IPCParentEventGet()
        {
            return ToQue.Get();
        }
        /// <summary>
        /// イベントの有無の参照
        /// </summary>
        /// <returns></returns>
        protected bool IPCParentEventRedient()
        {
            return ToQue.IsDataRegident();
        }

        #endregion
        #region 待機時間制御用のユティリティメンバー

        #region 時間監視ユーティリティ
        /// <summary>
        /// タイマーイベント一覧
        /// </summary>
        private cklib.Util.TickCountTimeoutControlList<TEventCode> TimerEventList = new Util.TickCountTimeoutControlList<TEventCode>();
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Interval">インターバルタイマー</param>
        public virtual void AddTimerEvent(TEventCode code, int EventTime, bool Interval = false)
        {
            TimerEventList.Append(code, EventTime, Interval);
        }
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="Interval">インターバルタイマー</param>
        public virtual void AddTimerEvent(TEventCode code, int EventTime, object Context, bool Interval)
        {
            TimerEventList.Append(code, EventTime, Interval, Context);
        }
        /// <summary>
        /// タイマーイベント削除
        /// </summary>
        /// <param name="code">EventCode</param>
        public virtual void RemoveTimerEvent(TEventCode code)
        {
            TimerEventList.Remove(code);
        }
        /// <summary>
        ///  タイムアウト処理は常に全実行
        /// </summary>
        public bool TimeoutProcessAlwaysAllExcute
        {
            get
            {
                return TimerEventList.TimeoutProcessAlwaysAllExcute;
            }
            set
            {
                TimerEventList.TimeoutProcessAlwaysAllExcute = value;
            }
        }

        #endregion
        #endregion
        #region アイドル中処理
        /// <summary>
        /// 待機タイムアウト時間をミリ秒で返す
        /// </summary>
        /// <returns></returns>
        public virtual int GetWaitTime()
        {
            return this.GetWaitTime(Timer.GetTickCount(), Timeout.Infinite);
        }

        /// <summary>
        /// 待機タイムアウト時間をミリ秒で返す
        /// </summary>
        /// <returns></returns>
        public virtual int GetWaitTime(int now, int userTimer)
        {
            return this.TimerEventList.GetWaitTime(now, userTimer);
        }

        /// <summary>
        /// タイムアウトチェック
        /// </summary>
        /// <returns></returns>
        public virtual bool TimeoutCheck()
        {
            return this.TimerEventList.TimeoutProcess(Timer.GetTickCount(), (code) =>
            {
                EventDataTypeBase<TEventCode, TEventData> ed;
                ed.EventCode = code;
                ed.EventData = null;
                if (this.IPCEventHandler != null)
                    return this.IPCEventHandler(ed);
                return true;
            });
        }
        #endregion
        #region イベント処理
        /// <summary>
        /// イベントハンドラ定義
        /// </summary>
        /// <returns></returns>
        public delegate bool PreIPCEventHandlerCallBack();
        /// <summary>
        /// イベントハンドラ定義
        /// </summary>
        /// <typeparam name="TECode">EventCode型</typeparam>
        /// <typeparam name="TData">EventData型</typeparam>
        /// <param name="ed">イベント情報</param>
        /// <returns></returns>
        public delegate bool IPCEventHandlerCallBack<TECode, TData>(EventDataTypeBase<TECode, TData> ed)
            where TECode : struct
            where TData : class;
        /// <summary>
        /// プレIPCイベントハンドラ（キュー読み取り前処理)
        /// </summary>
        public event PreIPCEventHandlerCallBack PreIPCEventHandler = null;
        /// <summary>
        /// IPCイベント後処理ハンドラ
        /// </summary>
        public event PreIPCEventHandlerCallBack AfterIPCEventHandler = null;
        /// <summary>
        /// IPCイベントハンドラ
        /// </summary>
        public event IPCEventHandlerCallBack<TEventCode, TEventData> IPCEventHandler = null;
        /// <summary>
        /// IPC残留イベント処理ハンドラ
        /// </summary>
        public event IPCEventHandlerCallBack<TEventCode, TEventData> IPCAfterCheckEventHandler = null;

        /// <summary>
        /// IPCイベント
        /// </summary>
        /// <returns></returns>
        public virtual bool EventIPC()
        {
            if (this.PreIPCEventHandler != null)
                if (!this.PreIPCEventHandler())
                    return false;
            EventDataTypeBase<TEventCode, TEventData> ed = this.IPCParentEventGet();
            if (this.IPCEventHandler != null)
            {
                if (this.IPCEventHandler(ed))
                {
                    if (this.AfterIPCEventHandler != null)
                        return this.AfterIPCEventHandler();
                    return true;
                }
                return false;
            }
            return true;
        }

        #endregion
        #region 残留イベント処理
        /// <summary>
        /// 残留イベント処理
        /// </summary>
        /// <returns></returns>
        public virtual bool AfterCheckEventIPC()
        {
            EventDataTypeBase<TEventCode, TEventData> ed = this.IPCParentEventGet();
            if (this.IPCAfterCheckEventHandler != null)
                return this.IPCAfterCheckEventHandler(ed);
            return true;
        }

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
                if (disposing)
                {
                    ReleseManagedResorce();
                }
                ReleseResorce();
                this.ToQue.Dispose();
                if (this.FromQue != null)
                {
                    this.FromQue.Dispose();
                }
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
