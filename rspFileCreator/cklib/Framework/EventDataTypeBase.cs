using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Framework
{
    /// <summary>
    /// イベントデータ構造体
    /// </summary>
    public struct EventDataTypeBase<TEventCode, TEventData>
        where TEventCode : struct
        where TEventData : class
    {
        /// <summary>
        /// イベントコード
        /// </summary>
        public TEventCode EventCode;
        /// <summary>
        /// イベント付加情報
        /// </summary>
        public TEventData EventData;
    }
}
