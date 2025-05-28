using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// 時計時刻によるタイムアウト監視
    /// </summary>
    /// <typeparam name="TKEY">監視対象オブジェクトclass</typeparam>
    public class RTCTimeoutControlList<TKEY> : TimeoutControlList<TKEY, DateTime, TimeSpan>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RTCTimeoutControlList()
        { }
        /// <summary>
        /// 待ち時間の計算
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <param name="prewt">待ち時間</param>
        /// <returns>待ち時間</returns>
        protected override TimeSpan CalcWaitTime(DateTime target, DateTime nw, TimeSpan prewt)
        {
            if (nw >= target)
                return TimeSpan.Zero;
            return target.Subtract(nw);
        }

        /// <summary>
        /// タイムアウトしているか判定する
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時true</returns>
        protected override bool IsTimeout(DateTime target, DateTime nw)
        {
            if (nw >= target)
                return true;
            return false;
        }

        /// <summary>
        /// 現在時間の取得
        /// </summary>
        /// <returns>現在時間</returns>
        protected override DateTime GetNow()
        {
            return DateTime.Now;
        }
        /// <summary>
        /// 時刻を比較するDelegateインスタンスを取得するCompareメソッドと同一の仕様
        /// </summary>
        /// <returns></returns>
        protected override TimeComparerHandler<DateTime> Comarer
        {
            get {
                return (DateTime x, DateTime y) =>
                {
                    return x.CompareTo(y);
                };
            }
        }
        /// <summary>
        /// 待ち時間最大値の計算の為に使用する
        /// </summary>
        protected override TimeSpan UnlimitTime
        {
            get 
            {
                return TimeSpan.MaxValue;
            }
        }
        /// <summary>
        /// タイムアウト時間の計算
        /// </summary>
        /// <param name="wt">待ち時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時間</returns>
        protected override DateTime CalcTargetTime(TimeSpan wt, DateTime nw)
        {
            return nw.Add(wt);
        }

        /// <summary>
        /// インターバルタイマーの次回タイマーを取得する
        /// </summary>
        /// <param name="info">前回タイマー情報</param>
        /// <returns>次回タイマー情報</returns>
        protected override TimeoutControlList<TKEY, DateTime, TimeSpan>.TimeoutControl_Information GetNextIntervalTime(TimeoutControlList<TKEY, DateTime, TimeSpan>.TimeoutControl_Information info)
        {
            var tminfo = info;
            tminfo.StartTime = info.TargetTime;
            tminfo.TargetTime = tminfo.StartTime.Add(tminfo.TimeSpan);
            if (this.InhIntervalQueing)
            {
                while (DateTime.Now.CompareTo(tminfo.TargetTime)>0)
                {
                    tminfo.StartTime = tminfo.StartTime.Add(tminfo.TimeSpan);
                    tminfo.TargetTime = tminfo.StartTime.Add(tminfo.TimeSpan);
                }
            }
            return tminfo;
        }
    }
}
