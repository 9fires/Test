using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// 特権取得ユーティリティ
    /// </summary>
    public static class Privileges
    {
        #region 特権名定義
#pragma warning disable 1591
        public const string SE_CREATE_TOKEN_NAME             = "SeCreateTokenPrivilege";
        public const string SE_ASSIGNPRIMARYTOKEN_NAME       = "SeAssignPrimaryTokenPrivilege";
        public const string SE_LOCK_MEMORY_NAME              = "SeLockMemoryPrivilege";
        public const string SE_INCREASE_QUOTA_NAME           = "SeIncreaseQuotaPrivilege";
        public const string SE_UNSOLICITED_INPUT_NAME        = "SeUnsolicitedInputPrivilege";
        public const string SE_MACHINE_ACCOUNT_NAME          = "SeMachineAccountPrivilege";
        public const string SE_TCB_NAME                      = "SeTcbPrivilege";
        public const string SE_SECURITY_NAME                 = "SeSecurityPrivilege";
        public const string SE_TAKE_OWNERSHIP_NAME           = "SeTakeOwnershipPrivilege";
        public const string SE_LOAD_DRIVER_NAME              = "SeLoadDriverPrivilege";
        public const string SE_SYSTEM_PROFILE_NAME           = "SeSystemProfilePrivilege";
        public const string SE_SYSTEMTIME_NAME               = "SeSystemtimePrivilege";
        public const string SE_PROF_SINGLE_PROCESS_NAME      = "SeProfileSingleProcessPrivilege";
        public const string SE_INC_BASE_PRIORITY_NAME        = "SeIncreaseBasePriorityPrivilege";
        public const string SE_CREATE_PAGEFILE_NAME          = "SeCreatePagefilePrivilege";
        public const string SE_CREATE_PERMANENT_NAME         = "SeCreatePermanentPrivilege";
        public const string SE_BACKUP_NAME                   = "SeBackupPrivilege";
        public const string SE_RESTORE_NAME                  = "SeRestorePrivilege";
        public const string SE_SHUTDOWN_NAME                 = "SeShutdownPrivilege";
        public const string SE_DEBUG_NAME                    = "SeDebugPrivilege";
        public const string SE_AUDIT_NAME                    = "SeAuditPrivilege";
        public const string SE_SYSTEM_ENVIRONMENT_NAME       = "SeSystemEnvironmentPrivilege";
        public const string SE_CHANGE_NOTIFY_NAME            = "SeChangeNotifyPrivilege";
        public const string SE_REMOTE_SHUTDOWN_NAME          = "SeRemoteShutdownPrivilege";
        public const string SE_UNDOCK_NAME                   = "SeUndockPrivilege";
        public const string SE_SYNC_AGENT_NAME               = "SeSyncAgentPrivilege";
        public const string SE_ENABLE_DELEGATION_NAME        = "SeEnableDelegationPrivilege";
        public const string SE_MANAGE_VOLUME_NAME            = "SeManageVolumePrivilege";
        #endregion
#pragma warning restore 1591

        #region 日付更新権限取得処理
        #region Privileges related Structures

        /// <summary>
        /// typedef struct _LUID_AND_ATTRIBUTES { 
        ///    LUID   Luid;
        ///    DWORD  Attributes;
        /// } LUID_AND_ATTRIBUTES, *PLUID_AND_ATTRIBUTES;
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct LUID_AND_ATTRIBUTES
        {
            /// <summary>
            /// Specifies an LUID value.
            /// </summary>
            public long Luid;
            /// <summary>
            /// Specifies attributes of the LUID. 
            /// This value contains up to 32 one-bit flags. 
            /// Its meaning is dependent on the definition and use of the LUID.
            /// </summary>
            public int Attributes;
        }

        /// <summary>
        /// typedef struct _TOKEN_PRIVILEGES {
        ///     DWORD PrivilegeCount;
        ///     LUID_AND_ATTRIBUTES Privileges[];
        /// } TOKEN_PRIVILEGES, *PTOKEN_PRIVILEGES; 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct TOKEN_PRIVILEGES
        {
            /// <summary>
            /// This must be set to the number of entries in the Privileges array.
            /// </summary>
            public int PrivilegeCount;
            /// <summary>
            /// Specifies an array of LUID_AND_ATTRIBUTES structures.
            /// Each structure contains the LUID and attributes of a privilege.
            /// To get the name of the privilege associated with a LUID, 
            /// call the LookupPrivilegeName function, passing the address of the LUID as the value of the lpLuid parameter.
            /// </summary>
            public LUID_AND_ATTRIBUTES Privileges;
        }
        #endregion Privileges related Structures

        #region Privilege related APIs
        /// <summary>
        /// プロセスに関連付けられているアクセストークンを開きます。
        /// </summary>
        /// <param name="ProcessHandle">プロセスのハンドル</param>
        /// <param name="DesiredAccess">プロセスに対して希望するアクセス権</param>
        /// <param name="TokenHandle">開かれたアクセストークンのハンドルへのポインタ</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(
            IntPtr ProcessHandle,
            int DesiredAccess,
            ref IntPtr TokenHandle);

        /// <summary>
        /// 指定されたシステムで使われているローカル一意識別子（LUID）を取得し、
        /// 指定された特権名をローカルで表現します。
        /// </summary>
        /// <param name="lpSystemName">システムを指定する文字列のアドレス</param>
        /// <param name="lpName">特権を指定する文字列のアドレス</param>
        /// <param name="lpLuid">ローカル一意識別子のアドレス</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LookupPrivilegeValue(
            string lpSystemName,
            string lpName,
            ref long lpLuid);

        /// <summary>
        /// 指定したアクセストークン内の特権を有効または無効にします。
        /// TOKEN_ADJUST_PRIVILEGES アクセス権が必要です。
        /// </summary>
        /// <param name="TokenHandle">特権を保持するトークンのハンドル</param>
        /// <param name="DisableAllPrivileges">すべての特権を無効にするためのフラグ</param>
        /// <param name="NewState">新しい特権情報へのポインタ</param>
        /// <param name="BufferLength">PreviousState バッファのバイト単位のサイズ</param>
        /// <param name="PreviousState">変更を加えられた特権の元の状態を受け取る</param>
        /// <param name="ReturnLength">PreviousState バッファが必要とするサイズを受け取る</param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(
            IntPtr TokenHandle,
            bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            int BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength
            );
        #endregion Privilege related APIs

        /// <summary>
        /// 特権を取得する
        /// </summary>
        /// <param name="Privilege_NAME">取得する特権の名称</param>
        public static void SetPrivilege(string Privilege_NAME)
        {
            const int TOKEN_QUERY = 0x00000008;
            const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
            //const string Privilege_NAME = "SeSystemtimePrivilege";
            const int SE_PRIVILEGE_ENABLED = 0x00000002;
            uint errCode = 0;

            // プロセスのハンドルを取得する。
            IntPtr hproc = System.Diagnostics.Process.GetCurrentProcess().Handle;
            // Token を取得する。
            IntPtr hToken = IntPtr.Zero;
            if (!OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref hToken))
            {
                throw new cklib.Util.Win32ErrorException(cklib.Util.Errors.GetLastError(), "OpenProcessToken(SetSystemTimePrivilege)");
            }

            // LUID を取得する。
            long luid = 0;
            if (!LookupPrivilegeValue(null, Privilege_NAME, ref luid))
            {
                throw new cklib.Util.Win32ErrorException(cklib.Util.Errors.GetLastError(), "LookupPrivilegeValue(SetSystemTimePrivilege)");
            }

            // 特権を設定する。
            TOKEN_PRIVILEGES tp = new TOKEN_PRIVILEGES();
            tp.PrivilegeCount = 1;
            tp.Privileges = new LUID_AND_ATTRIBUTES();
            tp.Privileges.Luid = luid;
            tp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;

            // 特権をセットする。
            if (!AdjustTokenPrivileges(hToken, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero))
            {
                throw new cklib.Util.Win32ErrorException(cklib.Util.Errors.GetLastError(), "AdjustTokenPrivileges(SetSystemTimePrivilege)");
            }
            else
            {
                errCode = cklib.Util.Errors.GetLastError();
                if (errCode != 0)
                {
                    throw new cklib.Util.Win32ErrorException(cklib.Util.Errors.GetLastError(), "AdjustTokenPrivileges");
                }
            }
        }
        #endregion
    }
}
