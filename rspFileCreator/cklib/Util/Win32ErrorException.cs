using System;
using System.Collections.Generic;
using System.Text;
using cklib.Util;

namespace cklib.Util
{
    /// <summary>
    /// Win32エラーラップするException
    /// </summary>
    public class Win32ErrorException : Exception
    {
        /// <summary>
        /// APIリザルトコード
        /// </summary>
        public readonly uint ErrorCode;
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public Win32ErrorException()
            : base()
        {
            this.ErrorCode = 0;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ErrorCode">Win32エラーコード</param>
        public Win32ErrorException(uint ErrorCode)
            : base()
        {
            this.ErrorCode = ErrorCode;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ErrorCode">Win32エラーコード</param>
        /// <param name="Msg">メッセージ</param>
        public Win32ErrorException(uint ErrorCode, string Msg)
            : base(Msg)
        {
            this.ErrorCode = ErrorCode;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ErrorCode">Win32エラーコード</param>
        /// <param name="Msg">メッセージ</param>
        /// <param name="exp">子エクセプション</param>
        public Win32ErrorException(uint ErrorCode, string Msg, Exception exp)
            : base(Msg, exp)
        {
            this.ErrorCode = ErrorCode;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Msg">メッセージ</param>
        public Win32ErrorException(string Msg)
            : base(Msg)
        {
            this.ErrorCode = 0;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Msg">メッセージ</param>
        /// <param name="exp">子エクセプション</param>
        public Win32ErrorException(string Msg, Exception exp)
            : base(Msg, exp)
        {
            this.ErrorCode = 0;
        }
        /// <summary>
        /// メッセージ取得のオーバーロード
        /// </summary>
        public override string Message
        {
            get
            {
                if (this.ErrorCode == 0)
                {
                    return base.Message;
                }
                var msg = Errors.GetSystemErrorMessage(this.ErrorCode);
                msg = msg.TrimEnd("\r\n ".ToCharArray());
                if (base.Message.Length == 0)
                {
                    return string.Format("{1} ({2})", base.Message, msg, this.ErrorCode);
                }
                return string.Format("{0}\r\n{1} ({2})", base.Message, msg, this.ErrorCode);
            }
        }
    }
}
