using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Framework
{
    /// <summary>
    /// IPCインターフェース定義
    /// </summary>
    /// <typeparam name="TEventCode"></typeparam>
    /// <typeparam name="TEventData"></typeparam>
    public interface IIPCControl<TEventCode, TEventData>
        where TEventCode : struct
        where TEventData : class
    {

        #region IPC用メンバー
        /// <summary>
        /// IPCQueueサイズ上限の設定取得
        /// </summary>
        int IPCQueueMaxSize
        {
            get;
            set;
        }
        /// <summary>
        /// イベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <returns>true/false</returns>
        bool IPCPut(TEventCode ev);

        /// <summary>
        /// イベントを通知する
        /// </summary>
        /// <param name="ev">イベントコード</param>
        /// <param name="data">イベントデータ</param>
        /// <returns>true/false</returns>
        bool IPCPut(TEventCode ev, TEventData data);

        /// <summary>
        /// イベントを通知する
        /// </summary>
        /// <param name="evd">イベント情報</param>
        /// <returns>true/false</returns>
        bool IPCPut(EventDataTypeBase<TEventCode, TEventData> evd);

        /// <summary>
        /// このオブジェクトのイベントの取得
        /// </summary>
        /// <returns></returns>
        EventDataTypeBase<TEventCode, TEventData> GetIPCEvent();
        /// <summary>
        /// 待機イベントハンドルを取得する
        /// </summary>
        /// <returns></returns>
        System.Threading.WaitHandle GetEventHandle();
        #endregion

        #region 待機時間制御用のユティリティメンバー
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Interval">インターバルタイマー</param>
        void AddTimerEvent(TEventCode code, int EventTime, bool Interval = false);
        /// <summary>
        /// タイマーイベント追加・更新
        /// </summary>
        /// <param name="code">EventCode</param>
        /// <param name="EventTime">タイムアウトまでの時間(ミリ秒)</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="Interval">インターバルタイマー</param>
        void AddTimerEvent(TEventCode code, int EventTime, object Context, bool Interval);
        /// <summary>
        /// タイマーイベント削除
        /// </summary>
        /// <param name="code">EventCode</param>
        void RemoveTimerEvent(TEventCode code);
        #endregion
    }
}
