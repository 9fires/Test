using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// サービスインストーラ
    /// </summary>
    public class ServiceInstaller
    {
	    /// <summary>
	    /// サービス種類
	    /// </summary>
	    public enum ServiceType
	    {
            /// <summary>
            /// ドライバサービスを指定します。
            /// </summary>
		    KERNEL_DRIVER		=	0x00000001,
            /// <summary>
            /// ファイルシステムのドライバサービスを指定します。
            /// </summary>
		    FILE_SYSTEM_DRIVER	=	0x00000002,
            /// <summary>
            /// Reserved.
            /// </summary>
		    ADAPTER				=	0x00000004,
            /// <summary>
            /// Reserved.
            /// </summary>
		    RECOGNIZER_DRIVER	=	0x00000008,
            /// <summary>
            ///
            /// </summary>
		    DRIVER				=	(KERNEL_DRIVER | FILE_SYSTEM_DRIVER | RECOGNIZER_DRIVER),
            /// <summary>
            /// サービスアプリケーションがそのアプリケーション専用のプロセス内で動作することを指定します。
            /// </summary>
		    Win32_OWN_PROCESS	=	0x00000010,
            /// <summary>
            /// サービスアプリケーションが 1 つのプロセスを他のサービスと共有することを指定します。
            /// </summary>
		    Win32_SHARE_PROCESS	=	0x00000020,
            /// <summary>
            /// 
            /// </summary>
		    Win32				=	(Win32_OWN_PROCESS | Win32_SHARE_PROCESS),
            /// <summary>
            /// 
            /// </summary>
		    INTERACTIVE_PROCESS	=	0x00000100,
            /// <summary>
            /// すべてのタイプ
            /// </summary>
		    TYPE_ALL			=	(Win32  | ADAPTER | DRIVER  | INTERACTIVE_PROCESS),
	    }

	    /// <summary>
	    /// 開始種類
	    /// </summary>
	    public enum StartType
	    {
            /// <summary>
            /// システムローダーが開始するデバイスドライバを指定します。この値は、ドライバサービスにのみ有効です。
            /// </summary>
		    BOOT_START	    =	0x00000000,
            /// <summary>
            /// IoInitSystem 関数が開始するデバイスドライバを指定します。この値は、ドライバサービスにのみ有効です。
            /// </summary>
		    SYSTEM_START	=	0x00000001,
            /// <summary>
            /// システムの起動時にサービス制御マネージャが自動的に開始するサービスを指定します。
            /// </summary>
		    AUTO_START	    =	0x00000002,
            /// <summary>
            /// プロセスが StartService 関数を呼び出したときにサービス制御マネージャが開始するサービスを指定します。
            /// </summary>
		    DEMAND_START	=	0x00000003,
            /// <summary>
            /// 開始できなくするサービスを指定します。
            /// </summary>
		    DISABLED	    =	0x00000004,
	    };
	    /// <summary>
	    /// エラー制御種類
	    /// </summary>
	    public enum ErrorControlType
	    {
            /// <summary>
            /// 開始プログラムはエラーをログに記録しますが、開始操作を続行します。
            /// </summary>
		    ERROR_IGNORE	=	0x00000000,
            /// <summary>
            /// 開始プログラムはエラーをログに記録し、メッセージボックスをポップアップ表示しますが、開始操作を続行します。
            /// </summary>
		    ERROR_NORMAL	=	0x00000001,
            /// <summary>
            /// 開始プログラムはエラーをログに記録します。直前に利用したときは正常であったことがわかっている構成を使って開始操作を行っている場合は、その開始操作を続行します。それ以外の場合、直前に利用したときは正常であった構成を使ってシステムを再起動します。
            /// </summary>
		    ERROR_SEVERE	=	0x00000002,
            /// <summary>
            /// 可能であれば、開始プログラムはエラーをログに記録します。直前に利用したときは正常であったことがわかっている構成を使って開始操作を行っている場合は、その開始操作は失敗します。それ以外の場合、直前に利用したときは正常であった構成を使ってシステムを再起動します。
            /// </summary>
		    ERROR_CRITICAL	=	0x00000003,
	    };
        const int SC_MANAGER_CREATE_SERVICE = 0x0002;
        const int SERVICE_CONFIG_DESCRIPTION = 1;
        const int STANDARD_RIGHTS_REQUIRED     = 0x000F0000;
        const int SERVICE_QUERY_CONFIG         = 0x0001;
        const int SERVICE_CHANGE_CONFIG        = 0x0002;
        const int SERVICE_QUERY_STATUS         = 0x0004;
        const int SERVICE_ENUMERATE_DEPENDENTS = 0x0008;
        const int SERVICE_START                = 0x0010;
        const int SERVICE_STOP                 = 0x0020;
        const int SERVICE_PAUSE_CONTINUE       = 0x0040;
        const int SERVICE_INTERROGATE          = 0x0080;
        const int SERVICE_USER_DEFINED_CONTROL = 0x0100;
        const int  SERVICE_ALL_ACCESS          = STANDARD_RIGHTS_REQUIRED     |
                                                 SERVICE_QUERY_CONFIG         |
                                                 SERVICE_CHANGE_CONFIG        |
                                                 SERVICE_QUERY_STATUS         |
                                                 SERVICE_ENUMERATE_DEPENDENTS |
                                                 SERVICE_START                |
                                                 SERVICE_STOP                 |
                                                 SERVICE_PAUSE_CONTINUE       |
                                                 SERVICE_INTERROGATE          |
                                                 SERVICE_USER_DEFINED_CONTROL;

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManager", SetLastError = true)]
        private static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, int dwDesiredAccess);
        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandle(IntPtr h);
        //[DllImport("advapi32.dll", EntryPoint = "OpenSCManager")]
        //private static extern IntPtr OpenSCManager2(IntPtr lpMachineName, IntPtr lpDatabaseName, int dwDesiredAccess);
        [DllImport("advapi32.dll", EntryPoint = "CreateService", SetLastError = true)]
        private static extern IntPtr CreateService(
            IntPtr hSCManager, 
            string lpServiceName,
            string lpDisplayName, 
            int dwDesiredAccess,
            int dwServiceType,
            int dwStartType, 
            int dwErrorControl, 
            string lpBinaryPathName, 
            string lpLoadOrderGroup, 
            IntPtr lpdwTagId,
            string lpDependencies, 
            string lp, string lpPassword);
        [DllImport("advapi32.dll", EntryPoint = "OpenService", SetLastError = true)]
        private static extern IntPtr OpenService(
            IntPtr hSCManager,
            string lpServiceName,
            int dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ChangeServiceConfig2(
            IntPtr hService,
            int dwInfoLevel,
            IntPtr lpInfo);
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);
        /// <summary>
	    /// サービスの登録
	    /// </summary>
	    /// <param name="serviceName">サービス名</param>
	    /// <param name="servicedispName">サービス表示名</param>
	    /// <param name="Description">サービスの説明</param>
	    /// <param name="serviceExe">サービス実行プログラムパス</param>
	    /// <param name="type">サービス種類</param>
	    /// <param name="start">起動種類</param>
	    /// <param name="errCtl">エラー制御種類</param>
	    /// <param name="dependes">依存サービス</param>
	    /// <param name="acount">サービスアカウント</param>
	    static public bool Install(string serviceName,
						    string servicedispName,
						    string Description,
						    string serviceExe,
						    ServiceType type,
						    StartType start,
						    ErrorControlType errCtl,
						    string dependes,
						    string	acount)
        {
            var sch = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
            if (sch == IntPtr.Zero)
                return false;
            try
            {
                var schService = CreateService(
                    sch,				// SCManager database
                    serviceName,		// name of service
                    servicedispName,	// name to display (new parameter after october beta)
                    SC_MANAGER_CREATE_SERVICE,		// desired access
                    (int)type,	        // service type
                    (int)start,		    // start type
                    (int)errCtl,		// error control type
                    serviceExe,		    // service's binary
                    null,				// no load ordering group
                    IntPtr.Zero,		// no tag identifier
                    (dependes == null || dependes.Length == 0 ? "\0\0" : dependes),			 // dependencies
                    (acount == null || acount.Length == 0 ? null : acount),	// LocalSystem account
                    null);						// no password
                if (schService == IntPtr.Zero)
                    return false;
                try
                {
                    byte[] bDescription = System.Text.Encoding.Unicode.GetBytes(Description + "\0");
                    IntPtr lpDescription = Marshal.AllocHGlobal(bDescription.Length);
                    if (lpDescription == IntPtr.Zero)
                        return false;
                    try
                    {
                        Marshal.Copy(bDescription, 0, lpDescription, bDescription.Length);
                        SERVICE_DESCRIPTION sd = new SERVICE_DESCRIPTION();
                        sd.lpDescription = lpDescription;
                        IntPtr lpInfo = Marshal.AllocHGlobal(Marshal.SizeOf(sd));
                        if (lpInfo == IntPtr.Zero)
                            return false;
                        try
                        {
                            Marshal.StructureToPtr(sd, lpInfo, false);
                            if (ChangeServiceConfig2(schService, SERVICE_CONFIG_DESCRIPTION, lpInfo))
                                return true;
                        }
                        finally
                        {
                            Marshal.FreeHGlobal(lpInfo);
                        }
                    }                    
                    finally
                    {
                        Marshal.FreeHGlobal(lpDescription);
                    }
                }
                finally
                {
                    CloseServiceHandle(schService);
                }
            }
            finally
            {
                CloseServiceHandle(sch);
            }
            return false;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct SERVICE_DESCRIPTION
        {
            public IntPtr lpDescription;
        }
	    /// <summary>
	    /// サービスの削除
	    /// </summary>
	    /// <param name="serviceName">サービス名</param>
	    static public bool	Remove(string serviceName)
        {
            var sch = OpenSCManager(null, null, SC_MANAGER_CREATE_SERVICE);
            if (sch == IntPtr.Zero)
                return false;
            try
            {
                var schService = OpenService(
                    sch,				// SCManager database
                    serviceName,		// name of service
                    SERVICE_ALL_ACCESS);
                if (schService == IntPtr.Zero)
                    return false;
                try
                {
                    if (DeleteService(schService))
                        return true;
                }
                finally
                {
                    CloseServiceHandle(sch);
                }
            }
            finally
            {
                CloseServiceHandle(sch);
            }
            return false;
        }

    }
}
