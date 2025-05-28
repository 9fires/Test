using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// システムユーティリティ
    /// </summary>
    static class SystemInformation
    {
        /// <summary>
        /// 64Bitプロセスか判別する
        /// </summary>
        public static bool Is64BitProcess
        {
            get
            {
#if __net20__ || __net35__
                if (IntPtr.Size == 4)
                {
                    return false;
                }
                else if (IntPtr.Size == 8)
                {
                    return true;
                }
                else
                    return false;   //  unknown
#else   //  .NET4.0以降
                return System.Environment.Is64BitProcess;

#endif
            }
        }
    }
}
