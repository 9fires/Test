using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace cklib.Log.ExceptionFormters
{
    /// <summary>
    /// SocketExceptionフォーマッタ
    /// </summary>
    [Serializable]
    public class SocketExceptionFormater : ExceptionFormater
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SocketExceptionFormater()
        :base(typeof(SocketException))
        {}
        /// <summary>
        /// 書式化
        /// </summary>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public override string Format(Exception exp)
        {
            SocketException se = (SocketException)exp;
            return String.Format("Socket Error Information Code:{0} Message:{2}\r\n", se.ErrorCode, se.NativeErrorCode, se.SocketErrorCode);
        }
    }
}
