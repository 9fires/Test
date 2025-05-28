using System;
using System.Collections.Generic;//
using System.Text;
#if __net20__
using cklib.Framework;
using System.Collections;
#endif

namespace cklib.Util
{
    /// <summary>
    /// イベントハンドラ定義
    /// </summary>
    public delegate int TimeComparerHandler<TIME>(TIME x ,TIME y);
    /// <summary>
    /// タイマー再設定モード
    /// </summary>
    public enum TimeoutControl_TimerResetMode
    {
        /// <summary>
        /// 常に置き換え
        /// </summary>
        Update,
        /// <summary>
        /// 現在の設定時間より早い設定時間なら置き換え
        /// </summary>
        UpdateBefore,
        /// <summary>
        /// 現在の設定時間より遅い設定時間なら置き換え
        /// </summary>
        UpdateAfter,
    }
    /// <summary>
    /// タイムアウト監視用クラス
    /// </summary>
    /// <typeparam name="TKEY">監視対象オブジェクトclass</typeparam>
    /// <typeparam name="TIME">タイムアウト時刻型</typeparam>
    /// <typeparam name="TIMESPAN">待機時間の等の経過時間型</typeparam>
    public abstract class TimeoutControlList<TKEY, TIME, TIMESPAN>
    {
        /// <summary>
        /// タイマー情報
        /// </summary>
        public struct TimeoutControl_Information
        {
            /// <summary>
            /// タイマーキー
            /// </summary>
            public TKEY Key;
            /// <summary>
            /// 監視開始時刻
            /// </summary>
            public TIME StartTime;
            /// <summary>
            /// タイムアウト時刻
            /// </summary>
            public TIME TargetTime;
            /// <summary>
            /// タイムアウト時間
            /// </summary>
            public TIMESPAN TimeSpan;
            /// <summary>
            /// インターバルモード
            /// </summary>
            public bool Interval;
            /// <summary>
            /// コンテキスト
            /// </summary>
            public object Context;
        }
        /// <summary>
        /// 監視対象一覧
        /// </summary>
        private Dictionary<TKEY, TimeoutControl_Information> TargetList;
        /// <summary>
        /// タイムアウトチェックテーブル
        /// </summary>
        private SortedList<TIME, List<TimeoutControl_Information>> CheckList;
        /// <summary>
        /// コンストラクタ
        /// </summary>
	    public  TimeoutControlList()
        {
            this.TargetList = new Dictionary<TKEY, TimeoutControl_Information>();
            this.CheckList = new SortedList<TIME, List<TimeoutControl_Information>>(new TimeComparer(this.Comarer));
	    }
        private class TimeComparer : IComparer<TIME>
        {
            private TimeComparerHandler<TIME> Compare;
            public TimeComparer(TimeComparerHandler<TIME> func)
            {
                this.Compare = func;
            }
            int IComparer<TIME>.Compare(TIME x, TIME y)
            {
                return this.Compare(x, y);
            }
        }
        /// <summary>
        ///タイムアウト監視対象数の取得
        /// </summary>
        /// <returns>登録されているタイムアウト監視対象の総数</returns>
	    public int Count
	    {
            get
            {
                return this.TargetList.Count;
            }
	    }
        /// <summary>
        ///タイムアウト監視対象数の取得
        /// </summary>
        /// <returns>登録されているタイムアウト監視対象の総数</returns>
        public int GetTargetCount()
        {
            return this.TargetList.Count;
        }
        /// <summary>
        /// タイムアウト監視時間登録数の取得
        /// </summary>
        /// <returns>登録されているタイムアウト時間の総数</returns>
	    public int GetTargetTimeCount()
	    {
		    return this.CheckList.Count;
	    }
        /// <summary>
        /// 先頭のアイテムを取得
        /// </summary>
        /// <returns>先頭のアイテム</returns>
        private KeyValuePair<TIME, List<TimeoutControl_Information>> GetFirst()
        {
            foreach(var item in this.CheckList)
            {
                return item;
            }
            throw new Exception("None Item");
        }
 
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        /// <param name="wt">現在の最小待ち時間</param>
        /// <returns>算出された待ち時間</returns>
        public TIMESPAN GetWaitTime(TIME nw,TIMESPAN wt)
        {
            lock (this)
            {
                if (this.CheckList.Count != 0)
                {
                    KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                    wt = this.CalcWaitTime(item.Key, nw, wt);
                }
            }
    	    return wt;
        }
        /// <summary>
        /// 待ち時間最大値の計算の為に使用する
        /// </summary>
        protected abstract TIMESPAN UnlimitTime
        {
            get;
        }
        /// <summary>
        /// 待ち時間の計算
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <param name="prewt">待ち時間</param>
        /// <returns>待ち時間</returns>
        protected abstract TIMESPAN CalcWaitTime(TIME target,TIME nw,TIMESPAN prewt);
        /// <summary>
        /// タイムアウト時間の計算
        /// </summary>
        /// <param name="wt">待ち時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時間</returns>
        protected abstract TIME CalcTargetTime(TIMESPAN wt, TIME nw);
        /// <summary>
        /// タイムアウトしているか判定する
        /// </summary>
        /// <param name="target">タイムアウト時間</param>
        /// <param name="nw">現在時間</param>
        /// <returns>タイムアウト時true</returns>
        protected abstract bool IsTimeout(TIME target,TIME nw);
        /// <summary>
        /// インターバルタイマーの次回タイマーを取得する
        /// </summary>
        /// <param name="info">前回タイマー情報</param>
        /// <returns>次回タイマー情報</returns>
        protected abstract TimeoutControl_Information GetNextIntervalTime(TimeoutControl_Information info);
        /// <summary>
        /// 現在時間の取得
        /// </summary>
        /// <returns>現在時間</returns>
        protected abstract TIME GetNow();
        /// <summary>
        /// 時刻を比較するDelegateインスタンスを取得するCompareメソッドと同一の仕様
        /// </summary>
        /// <returns></returns>
        protected abstract TimeComparerHandler<TIME> Comarer
        {
            get;
        }
        /// <summary>
        /// インターバルタイマー未実行分をキューイングし遅延してもすべて実行を行わず、
        /// 最後の一過のみ実行する
        /// </summary>
        public bool InhIntervalQueing
        {
            get
            {
                return this.m_inhIntervalQueing;
            }
            set
            {
                this.m_inhIntervalQueing = value;
            }
        }
        private bool m_inhIntervalQueing = true;
        /// <summary>
        ///  タイムアウト処理は常に全実行
        /// </summary>
        public bool TimeoutProcessAlwaysAllExcute
        {
            get
            {
                return this.m_TimeoutProcessAlwaysAllExcute;
            }
            set
            {
                this.m_TimeoutProcessAlwaysAllExcute = value;
            }
        }
        bool m_TimeoutProcessAlwaysAllExcute = true;
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        /// <param name="action">タイムアウト処理</param>
 #if __net20__
        public void TimeoutProcess(TIME nw,System.Action<TKEY> action)
#else
        public void TimeoutProcess(TIME nw,Action<TKEY> action)
#endif
        {
            this.TimeoutProcess(nw, (info) =>
                {
                    action(info);
                    return true;
                });
        }
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        /// <param name="func">タイムアウト処理</param>
        public bool TimeoutProcess(TIME nw, Func<TKEY, bool> func)
        {
            return this.TimeoutProcess(nw, (key, info) => { return func(key); });
        }
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        /// <param name="func">タイムアウト処理</param>
        public bool TimeoutProcess(TIME nw, Func<TKEY,TimeoutControl_Information,bool> func)
        {
            bool result = true;
            lock (this)
            {
                while (this.CheckList.Count != 0)
                {
                    KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                    //System.Diagnostics.Debug.Write(string.Format("{0}:{1}:{2}\r\n", item.Key, nw, this.CalcWaitTime(item.Key, nw, this.UnlimitTime)));
                    if (!this.IsTimeout(item.Key, nw))
                        break;
                    //	タイムアウト
                    List<TimeoutControl_Information> list = this.CheckList[item.Key];
                    List<TimeoutControl_Information> actlist = new List<TimeoutControl_Information>(list.Count);
                    foreach (var info in list)
                    {
                        actlist.Add(info);
                    }

                    foreach (var info in actlist)
                    {
                        try
                        {
                            if (!info.Interval)
                                this.Remove(info.Key);
                            if (!func(info.Key, info))
                                result = false;
                            if (!this.TimeoutProcessAlwaysAllExcute && !result)
                                break;
                        }
                        finally
                        {   //  実行済みのイベントはエラーでも処理済みとする
                            if (info.Interval && this.ContainsKey(info.Key))
                            {   //  インターバルタイマー且つタイマーが有効
                                TimeoutControl_Information tminfo = this.GetNextIntervalTime(info);
                                //System.Diagnostics.Debug.Write(string.Format("{0}:{1}:{2}:{3}:{4}\r\n", tminfo.StartTime, tminfo.TargetTime, tminfo.TimeSpan, nw, this.CalcWaitTime(tminfo.TargetTime, nw, this.UnlimitTime)));
                                this.Append(tminfo);
                            }
                            //else
                            //    this.Remove(info.Key);
                        }
                    }
                }
            }
            return result ;
        }
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        public bool HasTimeout(TIME nw)
        {
            lock (this)
            {
                while (this.CheckList.Count != 0)
                {
                    KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                    if (!this.IsTimeout(item.Key, nw))
                        break;
                    if (this.CheckList[item.Key].Count == 0)
                        continue;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// タイムアウトチェックリストからタイムアウト対象を検索する
        /// </summary>
        /// <param name="nw">現在時間</param>
        public TKEY GetTimeoutFirst(TIME nw)
        {
            lock (this)
            {
                while (this.CheckList.Count != 0)
                {
                    KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                    if (!this.IsTimeout(item.Key, nw))
                        break;
                    if (this.CheckList[item.Key].Count == 0)
                        continue;
                    var info = this.CheckList[item.Key][0];
                    if (!info.Interval)
                        this.Remove(info.Key);
                    if (info.Interval && this.ContainsKey(info.Key))
                    {   //  インターバルタイマー且つタイマーが有効
                        TimeoutControl_Information tminfo = this.GetNextIntervalTime(info);
                        this.Append(tminfo);
                    }
                    return info.Key;
                }
            }
            return default(TKEY);
        }
        /// <summary>
        /// Keyの検索
        /// </summary>
        /// <param name="key">対象情報キー</param>
        public bool ContainsKey(TKEY key)
        {
            lock (this)
            {
                return this.TargetList.ContainsKey(key);
            }
        }

        /// <summary>
        /// Keyの検索
        /// </summary>
        /// <param name="key">対象情報キー</param>
        public TIME GetValue(TKEY key)
        {
            lock (this)
            {
                if (this.TargetList.ContainsKey(key))
                    return this.TargetList[key].TargetTime;
                else
                    return default(TIME);
            }
        }
   
        /// <summary>
        /// ターゲット追加
        /// </summary>
        /// <param name="key">対象情報キー</param>
        /// <param name="tm">タイムアウト時間</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="mode">タイマー置き換え時の指定方法法</param>
        public void AppendTimeout(TKEY key, TIME tm, object Context = null, TimeoutControl_TimerResetMode mode = TimeoutControl_TimerResetMode.Update)
        {
            TimeoutControl_Information info = new TimeoutControl_Information();
            info.Key = key;
            info.StartTime = this.GetNow();
            info.TargetTime = tm;
            info.TimeSpan = this.CalcWaitTime(tm, info.StartTime, this.UnlimitTime);
            info.Interval = false;
            info.Context = Context;

            this.Append(info, mode);
        }

        /// <summary>
        /// ターゲット追加
        /// </summary>
        /// <param name="key">対象情報キー</param>
        /// <param name="timespan">タイマー時間</param>
        /// <param name="interval">インターバルタイマー</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="mode">タイマー置き換え時の指定方法法</param>
        public void Append(TKEY key, TIMESPAN timespan, bool interval = false, object Context = null, TimeoutControl_TimerResetMode mode = TimeoutControl_TimerResetMode.Update)
        {
            TimeoutControl_Information info = new TimeoutControl_Information();
            info.Key = key;
            info.StartTime = this.GetNow();
            info.TimeSpan = timespan;
            info.TargetTime = this.CalcTargetTime(info.TimeSpan, info.StartTime);
            info.Interval = interval;
            info.Context = Context;
            this.Append(info, mode);
        }
        /// <summary>
        /// ターゲット追加
        /// </summary>
        /// <param name="key">対象情報キー</param>
        /// <param name="timespan">タイマー時間</param>
        /// <param name="Context">タイマーコンテキスト</param>
        /// <param name="mode">タイマー置き換え時の指定方法法</param>
        public void Append(TKEY key, TIMESPAN timespan, object Context, TimeoutControl_TimerResetMode mode)
        {
            TimeoutControl_Information info = new TimeoutControl_Information();
            info.Key = key;
            info.StartTime = this.GetNow();
            info.TimeSpan = timespan;
            info.TargetTime = this.CalcTargetTime(info.TimeSpan, info.StartTime);
            info.Interval = false;
            info.Context = Context;
            this.Append(info, mode);
        }

        /// <summary>
        /// ターゲット追加
        /// </summary>
        /// <param name="info">対象情報</param>
        /// <param name="mode">タイマー置き換え時の指定方法法</param>
        private void Append(TimeoutControl_Information info, TimeoutControl_TimerResetMode mode = TimeoutControl_TimerResetMode.Update)
        {
            lock (this)
            {
                if (this.ContainsKey(info.Key))
                {
                    if (mode != TimeoutControl_TimerResetMode.Update)
                    {
                        TimeoutControl_Information tinfo = this.TargetList[info.Key];
                        var r = this.Comarer(tinfo.TargetTime, info.TargetTime);
                        if (r == 0)
                            return;
                        if (r < 0 && mode == TimeoutControl_TimerResetMode.UpdateAfter)
                            return;
                        if (r > 0 && mode == TimeoutControl_TimerResetMode.UpdateBefore)
                            return;
                    }
                    this.Remove(info.Key);
                }
                if (!this.CheckList.ContainsKey(info.TargetTime))
                    this.CheckList.Add(info.TargetTime, new List<TimeoutControl_Information>());
                this.CheckList[info.TargetTime].Add(info);
                this.TargetList.Add(info.Key, info);
            }
        }

        /// <summary>
        /// ターゲット削除
        /// </summary>
        /// <param name="key">対象情報キー</param>
        public void Remove(TKEY key)
        {
            lock (this)
            {
                if (this.ContainsKey(key))
                {
                    TimeoutControl_Information info = this.TargetList[key];
                    this.TargetList.Remove(key);
                    List<TimeoutControl_Information> list = this.CheckList[info.TargetTime];
                    list.Remove(info);
                    list.TrimExcess();
                    if (list.Count == 0)
                    {
                        this.CheckList.Remove(info.TargetTime);
                        this.CheckList.TrimExcess();
                    }
                }
            }
        }

        /// <summary>
        /// 先頭を取得する
        /// </summary>
        /// <returns></returns>
        public TKEY PeekFirst()
        {
            lock (this)
            {
                KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                return item.Value[0].Key;
            }
        }
        /// <summary>
        /// 先頭を削除して取得する
        /// </summary>
        /// <returns></returns>
        public TKEY RemoveFirst()
        {
            lock (this)
            {
                KeyValuePair<TIME, List<TimeoutControl_Information>> item = this.GetFirst();
                TKEY ret = item.Value[0].Key;
                this.Remove(ret);
                return ret;
            }
        }
 
        /// <summary>
        /// リストをクリアする
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                this.CheckList.Clear();
                this.TargetList.Clear();
            }
        }

        /// <summary>
        /// タイマー一覧
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<TIME, List<TimeoutControl_Information>>> GetTimerList()
        {
            foreach(var item in this.CheckList)
            {
                yield return item;
            }
        }
    }
}

