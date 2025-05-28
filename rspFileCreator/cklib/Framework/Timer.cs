using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace cklib.Framework
{
    /// <summary>
    /// 時間計測用タイマーユーティリティ
    /// </summary>
    public static class Timer
    {
        /// <summary>
        /// 待機時間最大
        /// </summary>
        static  public readonly int MaxWaitTime =   0x7ffffff;
        /// <summary>
        /// 待機時間最少
        /// </summary>
        static  public readonly int MinWaitTime =   0;
        /// <summary>
        /// TickCountの取得
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetTickCount")]
        public extern static uint NativeGetTickCount();
        /// <summary>
        /// TickCountの取得
        /// </summary>
        /// <returns></returns>
        public static int GetTickCount()
        {
            return (int)NativeGetTickCount();
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
            if (WaitTime == Timeout.Infinite)
            {
                return Timeout.Infinite;
            }
            int nowtm = GetTickCount();
            int chktm = (nowtm - StartTime);
            int wtm;
            if (WaitTime > chktm)
            {
                wtm = WaitTime - chktm;
                if (MinimumTime != -1)
                {
                    if (MinimumTime < wtm)
                    {
                        return MinimumTime;
                    }
                    else
                    {
                        return wtm;
                    }
                }
                else
                {
                    return wtm;
                }
            }
            return 0;
        }
        /// <summary>
        /// 指定時間までの待機時間の計算
        /// </summary>
        /// <param name="TargetTime">監視開始時間ミリ秒</param>
        /// <param name="now">現在時間ミリ秒</param>
        /// <returns>待機時間ミリ秒</returns>
        public static int CalcUntilWaitTime(int TargetTime, int now)
        {
            if (Timer.Compare(TargetTime, now) <= 0)
                return 0;
            return TargetTime - now;
        }
        /// <summary>
        /// 指定時間までの待機時間の計算
        /// </summary>
        /// <param name="TargetTime">監視開始時間ミリ秒</param>
        /// <returns>待機時間ミリ秒</returns>
        public static int CalcUntilWaitTime(int TargetTime)
        {
            return Timer.CalcUntilWaitTime(TargetTime, Timer.GetTickCount());
        }

        /// <summary>
        /// 待機時間の計算
        /// </summary>
        /// <param name="StartTime">監視開始時間ミリ秒</param>
        /// <param name="WaitTime">待機時間ミリ秒</param>
        /// <returns>待機時間ミリ秒</returns>
        public static int CalcWaitTime(int StartTime, int WaitTime)
        {
            return CalcWaitTime(StartTime, WaitTime, WaitTime);
        }
        /// <summary>
        /// 待機時間を経過したかの判定を行なう
        /// </summary>
        /// <param name="StartTime">監視開始時間ミリ秒</param>
        /// <param name="WaitTime">待機時間ミリ秒</param>
        /// <returns>待機時間を過ぎていればtrue</returns>
        public static bool IsWaitTimeOut(int StartTime, int WaitTime)
        {
            if (CalcWaitTime(StartTime, WaitTime, WaitTime) == 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 時刻の比較
        /// </summary>
        /// <param name="s1">比較対象1</param>
        /// <param name="s2">比較対象2</param>
        /// <returns>0一致</returns>
        public static int Compare(int s1, int s2)
        {
            return (s1 - s2);
        }
    }
}
