using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace cklib.Framework
{

    /// <summary>
    /// スレッド一覧格納クラス
    /// </summary>
    public static class AppThreadBaseListStore
    {
        static readonly private Dictionary<int, object> ThreadList = new Dictionary<int, object>();
        /// <summary>
        /// スレッド一覧に追加
        /// </summary>
        /// <typeparam name="TEventCode"></typeparam>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="thread"></param>
        public static void Add<TEventCode, TEventData>(AppThreadBase<TEventCode, TEventData> thread)
            where TEventCode : struct
            where TEventData : class
        {
            Add<AppThreadBase<TEventCode, TEventData>>(thread);
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thread"></param>
        public static void Add<T>(T thread)
            where T : class
        {
            lock (ThreadList)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                if (ThreadList.ContainsKey(id))
                    ThreadList[id] = thread;
                else
                    ThreadList.Add(id, thread);
            }
        }
        /// <summary>
        /// 削除
        /// </summary>
        public static void Remove()
        {
            lock (ThreadList)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                if (ThreadList.ContainsKey(id))
                    ThreadList.Remove(id);
            }
        }
        /// <summary>
        /// 参照
        /// </summary>
        /// <typeparam name="TEventCode"></typeparam>
        /// <typeparam name="TEventData"></typeparam>
        /// <returns></returns>
        public static AppThreadBase<TEventCode, TEventData> GetThead<TEventCode, TEventData>()
            where TEventCode : struct
            where TEventData : class
        {
            return GetThead<AppThreadBase<TEventCode, TEventData>>();
        }
        /// <summary>
        /// 参照
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetThead<T>()
            where T : class
        {
            lock (ThreadList)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                if (ThreadList.ContainsKey(id))
                    return ThreadList[id] as T;
                return null;
            }
        }
    }
}
