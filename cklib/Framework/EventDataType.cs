using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Framework
{
    /// <summary>
    /// イベントデータ構造体
    /// </summary>
    public struct EventDataType
    {
        /// <summary>
        /// イベントコード
        /// </summary>
        public EventCode EventCode;
        /// <summary>
        /// イベント付加情報
        /// </summary>
        public object EventData;
    }
}
