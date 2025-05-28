using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace cklib.Util
{
    /// <summary>
    /// エラー情報関連
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// IOExceptionからHResult値を取得する
        /// </summary>
        /// <remarks>
        ///  WinError.hより<br/>
        ///  #define FACILITY_WIN32                   7<br/>
        ///  #define __HRESULT_FROM_WIN32(x) ((HRESULT)(x) &gt;= 0 ? ((HRESULT)(x)) : ((HRESULT) (((x) &amp; 0x0000FFFF) | (FACILITY_WIN32 &gt;&gt; 16) | 0x80000000)))<br/>
        ///  #define ERROR_SHARING_VIOLATION          32L    →  0x80070020<br/>
        ///  #define ERROR_LOCK_VIOLATION             33L    →  0x80070021<br/>
        /// </remarks>
        /// <param name="exp">IOExceptionのインスタンス</param>
        /// <returns>HResult</returns>
        public static uint GetHResultFromIOException(System.IO.IOException exp)
        {
            int hr = (int)typeof(Exception).GetProperty("HResult", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(exp, null);   //  2015/03/24 .NET4.5以降参照がpublic となったため、BindingFlags.Publicを追加
            return (uint)hr;
        }
        /// <summary>
        /// IOExceptionからWin32エラー値を取得する
        /// </summary>
        /// <remarks>
        ///  WinError.hより<br/>
        ///  #define FACILITY_WIN32                   7<br/>
        ///  #define __HRESULT_FROM_WIN32(x) ((HRESULT)(x) &gt; 0 ? ((HRESULT)(x)) : ((HRESULT) (((x) &amp; 0x0000FFFF) | (FACILITY_WIN32 &gt;&gt; 16) | 0x80000000)))<br/>
        ///  #define ERROR_SHARING_VIOLATION          32L    →  0x80070020<br/>
        ///  #define ERROR_LOCK_VIOLATION             33L    →  0x80070021<br/>
        /// </remarks>
        /// <param name="exp">IOExceptionのインスタンス</param>
        /// <returns>HResult</returns>
        public static uint GetWin32ErrorCodeFromIOException(System.IO.IOException exp)
        {
            return HResultToWin32ErrorCode(GetHResultFromIOException(exp));
        }
        /// <summary>
        /// HResultがWin32エラーか判別する
        /// </summary>
        /// <param name="hResult">hResult値</param>
        /// <returns>true Win32エラー</returns>
        public static bool IsWin32ErrorHResult(uint hResult)
        {
            if ((hResult & 0x8007000) == 0x8007000)
                return true;
            return false;
        }
        /// <summary>
        /// HResultをWin32エラーコードに変換する
        /// </summary>
        /// <param name="hResult">hResult値</param>
        /// <returns>Win32エラーコード</returns>
        public static uint HResultToWin32ErrorCode(uint hResult)
        {
            if (!IsWin32ErrorHResult(hResult))
                return 0;
            return hResult & 0xffff;
        }
        /// <summary>
        /// Win32エラーコードの取得
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetLastError")]
        public static extern uint GetLastError();
        /// <summary>
        /// エラーメッセージの取得
        /// </summary>
        /// <param name="dwFlags"></param>
        /// <param name="lpSource"></param>
        /// <param name="dwMessageId"></param>
        /// <param name="dwLanguageId"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="nSize"></param>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern uint FormatMessage( uint dwFlags,
                                                    IntPtr lpSource,
                                                    uint dwMessageId,
                                                    uint dwLanguageId,
                                                    StringBuilder lpBuffer,
                                                    int nSize,
                                                    IntPtr Arguments);

        private const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        /// <summary>
        /// Win32エラーメッセージの取得
        /// </summary>
        /// <param name="ecode">Win32エラーコード</param>
        /// <returns>エラーメッセージ</returns>
        public static string GetSystemErrorMessage(uint ecode)
        {

            StringBuilder message = new StringBuilder(4096);

            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM,
                          IntPtr.Zero,
                          ecode,
                          0,
                          message,
                          message.Capacity,
                          IntPtr.Zero);
            return message.ToString();
        }
    }
}
