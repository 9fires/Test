using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace cklib.Log.ExceptionFormters
{
    /// <summary>
    /// SocketExceptionフォーマッタ
    /// </summary>
    [Serializable]
    public class IOExceptionFormater : ExceptionFormater
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IOExceptionFormater()
        :base(typeof(IOException))
        {}
        /// <summary>
        /// 書式化
        /// </summary>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public override string Format(Exception exp)
        {
            IOException se = (IOException)exp;
            return String.Format("HResult=0x{0:x8}\r\n", cklib.Util.Errors.GetHResultFromIOException(se));
        }
    }
}
