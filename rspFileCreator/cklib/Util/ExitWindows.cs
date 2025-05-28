using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// ExitWindowsユーティリティ
    /// </summary>
    public static class ExitWindows
    {
        /// <summary>
        /// シャットダウンを行う
        /// </summary>
        public enum ExitWindows_uFlags : uint
        {
            /// <summary>
            /// ログオフ
            /// </summary>
            EWX_LOGOFF = 0x00,
            /// <summary>
            /// シャットダウン
            /// </summary>
            EWX_SHUTDOWN = 0x01,
            /// <summary>
            /// 再起動
            /// </summary>
            EWX_REBOOT = 0x02,
            /// <summary>
            /// 電源オフ
            /// </summary>
            EWX_POWEROFF = 0x08,
            /// <summary>
            /// シャットダウン後再起動
            /// </summary>
            EWX_RESTARTAPPS = 0x40,
            /// <summary>
            /// 強制終了（WM_QUERYENDSESSION を送信しない)
            /// </summary>
            EWX_FORCE = 0x04,
            /// <summary>
            /// 強制終了　WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない
            /// </summary>
            EWX_FORCEIFHUNG = 0x10,
        }
        /// <summary>
        /// ExitWindowsEx
        /// </summary>
        /// <param name="uFlags">停止指定コード</param>
        /// <param name="dwReason">Reserve</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern bool ExitWindowsEx(ExitWindows_uFlags uFlags, int dwReason);
        /// <summary>
        /// ログオフ
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        public static void ExitWindowsEx(ExitWindows_uFlags uFlags,bool Force = false, bool ForceIfHung = false)
        {
            if (!ExitWindowsEx(uFlags | (Force ? ExitWindows_uFlags.EWX_FORCE : 0) | (ForceIfHung ? ExitWindows_uFlags.EWX_FORCEIFHUNG : 0), 0))
            {
                throw new cklib.Util.Win32ErrorException(cklib.Util.Errors.GetLastError(), "ExitWindowsEx:" + uFlags.ToString());
            }
        }
        /// <summary>
        /// ログオフ
        /// </summary>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        public static void Logoff(bool Force = false, bool ForceIfHung = false)
        {
            ExitWindowsEx(ExitWindows_uFlags.EWX_LOGOFF, Force, ForceIfHung);
        }
        /// <summary>
        /// シャットダウン
        /// </summary>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        /// <returns></returns>
        public static void Shutdown(bool Force=false,bool ForceIfHung=false)
        {
            cklib.Util.Privileges.SetPrivilege(cklib.Util.Privileges.SE_SHUTDOWN_NAME);
            ExitWindowsEx(ExitWindows_uFlags.EWX_SHUTDOWN, Force, ForceIfHung);
        }
        /// <summary>
        /// 再起動
        /// </summary>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        /// <returns></returns>
        public static void Reboot(bool Force = false, bool ForceIfHung = false)
        {
            cklib.Util.Privileges.SetPrivilege(cklib.Util.Privileges.SE_SHUTDOWN_NAME);
            ExitWindowsEx(ExitWindows_uFlags.EWX_REBOOT, Force, ForceIfHung);
        }
        /// <summary>
        /// 電源断
        /// </summary>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        /// <returns></returns>
        public static void PowerOff(bool Force = false, bool ForceIfHung = false)
        {
            cklib.Util.Privileges.SetPrivilege(cklib.Util.Privileges.SE_SHUTDOWN_NAME);
            ExitWindowsEx(ExitWindows_uFlags.EWX_POWEROFF, Force, ForceIfHung);
        }
        /// <summary>
        /// シャットダウン後再起動
        /// </summary>
        /// <param name="Force">WM_QUERYENDSESSION を送信しない</param>
        /// <param name="ForceIfHung">WM_QUERYENDSESSION、WM_ENDSESSIONの応答を待たない</param>
        /// <returns></returns>
        public static void RestartApps(bool Force = false, bool ForceIfHung = false)
        {
            cklib.Util.Privileges.SetPrivilege(cklib.Util.Privileges.SE_SHUTDOWN_NAME);
            ExitWindowsEx(ExitWindows_uFlags.EWX_RESTARTAPPS, Force, ForceIfHung);
        }
    }
}
