using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// 時計時刻によるタイムアウト監視
    /// </summary>
    /// <typeparam name="TKEY">監視対象オブジェクトclass</typeparam>
    public class TickCountTimeoutControlList<TKEY> : TimeoutControlList<TKEY, int, int>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TickCountTimeoutControlList()
        { }

        /// <summary>
        /// 待ち時間最大値の計算の為に使用する
        /// </summary>
        protected override int UnlimitTime
        {
            get 
            {
                return System.Threading.Timeout.Infinite;
            }
        }

        /// <summary>
        /// 待ち時間の計算
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <param name="prewt">待ち時間</param>
        /// <returns>待ち時間</returns>
        protected override int CalcWaitTime(int target, int nw, int prewt)
        {
            int wt = cklib.Framework.Timer.CalcUntilWaitTime(target, nw);
            if (prewt == System.Threading.Timeout.Infinite)
                return wt;
            return prewt > wt ? wt : prewt;
        }

        /// <summary>
        /// タイムアウト時間の計算
        /// </summary>
        /// <param name="wt">待ち時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時間</returns>
        protected override int CalcTargetTime(int wt, int nw)
        {
            return nw + wt;
        }

        /// <summary>
        /// タイムアウトしているか判定する
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時true</returns>
        protected override bool IsTimeout(int target, int nw)
        {
            return this.CalcWaitTime(target, nw, System.Threading.Timeout.Infinite) <= 0;
        }

        /// <summary>
        /// 現在時間の取得
        /// </summary>
        /// <returns>現在時間</returns>
        protected override int GetNow()
        {
            return cklib.Framework.Timer.GetTickCount();
        }
        /// <summary>
        /// 時刻を比較するDelegateインスタンスを取得するCompareメソッドと同一の仕様
        /// </summary>
        /// <returns></returns>
        protected override TimeComparerHandler<int> Comarer
        {
            get {
                return (int x, int y) =>
                {
                    if (x == y) return 0;

                    if (x > y)
                    {
                        int s = x - y;
                        if (s < 0)
                            return -1;
                        else
                            return 1;
                    }
                    else
                    {
                        int s = y - x;
                        if (s < 0)
                            return 1;
                        else
                            return -1;
                    }
                };
            }
        }

        /// <summary>
        /// インターバルタイマーの次回タイマーを取得する
        /// </summary>
        /// <param name="info">前回タイマー情報</param>
        /// <returns>次回タイマー情報</returns>
        protected override TimeoutControlList<TKEY, int, int>.TimeoutControl_Information GetNextIntervalTime(TimeoutControlList<TKEY, int, int>.TimeoutControl_Information info)
        {
            var tminfo = info;
            tminfo.StartTime = info.TargetTime;
            if (this.InhIntervalQueing)
            {
                while (cklib.Framework.Timer.IsWaitTimeOut(tminfo.StartTime, tminfo.TimeSpan))
                {
                    tminfo.StartTime += tminfo.TimeSpan;
                }
            }
            tminfo.TargetTime = this.CalcTargetTime(tminfo.TimeSpan, tminfo.StartTime);
            return tminfo;
        }
    }
}
