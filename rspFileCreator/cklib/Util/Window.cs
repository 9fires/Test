using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace cklib.Util
{
    /// <summary>
    /// Window操作関連ユーティリテイ
    /// </summary>
    public static class Window
    {
        /// <summary>
        /// SendMessage
        /// </summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <param name="Msg">メッセージID</param>
        /// <param name="wParam">WPARAM</param>
        /// <param name="lParam">LPARAM</param>
        /// <returns>処理結果</returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern uint SendMessage(IntPtr hWnd, uint Msg, ulong wParam, uint lParam);
        /// <summary>
        /// PostMessage
        /// </summary>
        /// <param name="hWnd">ウインドウハンドル</param>
        /// <param name="Msg">メッセージID</param>
        /// <param name="wParam">WPARAM</param>
        /// <param name="lParam">LPARAM</param>
        /// <returns>処理結果</returns>
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, ulong wParam, uint lParam);
        /// <summary>
        /// PostThreadMessage
        /// </summary>
        /// <param name="ThreadID">スレッドＩＤ</param>
        /// <param name="Msg">メッセージID</param>
        /// <param name="wParam">WPARAM</param>
        /// <param name="lParam">LPARAM</param>
        /// <returns>処理結果</returns>
        [DllImport("user32.dll", EntryPoint = "PostThreadMessage")]
        public static extern bool PostThreadMessage(int ThreadID, uint Msg, ulong wParam, uint lParam);
        /// <summary>
        /// GetMessage
        /// </summary>
        /// <param name="lpMsg"></param>
        /// <param name="hWnd"></param>
        /// <param name="wMsgFilterMin"></param>
        /// <param name="wMsgFilterMax"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetMessage")]
        private static extern bool GetMessage(ref IntPtr lpMsg,IntPtr hWnd,uint wMsgFilterMin,uint wMsgFilterMax);
        /**********
        /// <summary>
        /// GetMessageラッパークラス
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="hWnd"></param>
        /// <param name="wMsgFilterMin"></param>
        /// <param name="wMsgFilterMax"></param>
        /// <returns></returns>
        public static bool GetMessage(out Message msg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax)
        {
            byte[] buff = new byte[256];
            unsafe
            {
                bool r;
                fixed (byte* p = buff)
                {
                    r = GetMessage(ref (IntPtr)p, hWnd, wMsgFilterMin, wMsgFilterMax);
                }
                if (r)
                {
                    msg = ByteArrayToMessage(buff);
                }
                return r;
            }
        }
        /// <summary>
        /// メッセージ変換
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private static Message ByteArrayToMessage(byte[] buff)
        {
            Message msg = new Message();
            return msg;
        }
         * ********/
        /// <summary>
        /// FindWindowEx
        /// </summary>
        /// <param name="hWndParent"></param>
        /// <param name="hWndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
        /// <summary>
        /// BringWindowToTop
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "BringWindowToTop")]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        /// <summary>
        /// SetForegroundWindow
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// SetActiveWindow
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);
        /// <summary>
        /// SetFocus
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        /// <summary>
        /// GetTopWindow
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetTopWindow")]
        public static extern IntPtr GetTopWindow(IntPtr hWnd);
        /// <summary>
        /// GetForegroundWindow
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
        public extern static IntPtr GetForegroundWindow();

        /// <summary>
        /// SetWindowPos
        /// </summary>
        /// <param name="hWnd">ウィンドウのハンドル</param>
        /// <param name="hWndInsertAfter">配置順序のハンドル</param>
        /// <param name="x">横方向の位置</param>
        /// <param name="y">縦方向の位置</param>
        /// <param name="cx">幅</param>
        /// <param name="cy">高さ</param>
        /// <param name="uFlags">ウィンドウ位置のオプション</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        /// <summary>
        /// SetWindowPos uFlags値
        /// </summary>
        public static class SetWindowPos_uFlags
        {
            /// <summary>
            /// サイズ変更をしない
            /// </summary>
            public const int SWP_NOSIZE = 0x0001;
            /// <summary>
            /// 表示位置を変更しない
            /// </summary>
            public const int SWP_NOMOVE = 0x0002;
            /// <summary>
            /// Zオーダーを変更しない
            /// </summary>
            public const int SWP_NOZORDER = 0x0004;
            /// <summary>
            /// 再描画を行わない
            /// </summary>
            public const int SWP_NOREDRAW = 0x0008;
            /// <summary>
            /// アクティブ化しない
            /// </summary>
            public const int SWP_NOACTIVATE = 0x0010;
            /// <summary>
            /// 新しいフレームスタイルの設定を適用
            /// </summary>
            public const int SWP_FRAMECHANGED = 0x0020; /* The frame changed: send WM_NCCALCSIZE */
            /// <summary>
            /// ウィンドウ表示
            /// </summary>
            public const int SWP_SHOWWINDOW = 0x0040;
            /// <summary>
            /// ウィンドウ非表示
            /// </summary>
            public const int SWP_HIDEWINDOW = 0x0080;
            /// <summary>
            /// クライアント領域の無効化
            /// </summary>
            public const int SWP_NOCOPYBITS = 0x0100;
            /// <summary>
            /// OwnerウィンドウのZオーダーを変更しない
            /// </summary>
            public const int SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
            /// <summary>
            /// WM_WINDOWPOSCHANGINGの送信を抑止
            /// </summary>
            public const int SWP_NOSENDCHANGING = 0x0400; /* Don't send WM_WINDOWPOSCHANGING */
            /// <summary>
            /// 新しいフレームスタイルの設定を適用
            /// </summary>
            public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
            /// <summary>
            /// OwnerウィンドウのZオーダーを変更しない
            /// </summary>
            public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;
            /// <summary>
            /// ウィンドウを囲む枠（ ウィンドウクラスの記述部分で定義されている）を描画します。
            /// </summary>
            public const int SWP_DEFERERASE = 0x2000;
            /// <summary>
            /// この関数を呼び出したスレッドとウィンドウを所有するスレッドが異なる入力キューに関連付けられている場合、
            /// ウィンドウを所有するスレッドへ要求が送られます。こうすると、要求を受け取ったスレッドが要求を処理している間も、
            /// 関数を呼び出したスレッドの実行が止まってしまうことはありません。
            /// </summary>
            public const int SWP_ASYNCWINDOWPOS = 0x4000;
        }

        /// <summary>
        /// SetWindowPos hWndInsertAfterValue指定値
        /// </summary>
        public static class SetWindowPos_hWndInsertAfterValue
        {
            /// <summary>
            /// ウィンドウを Z オーダーの先頭に置きます。
            /// </summary>
            public static readonly IntPtr HWND_TOP = new IntPtr(0);
            /// <summary>
            /// ウィンドウを Z オーダーの最後に置きます。
            /// </summary>
            public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
            /// <summary>
            /// ウィンドウを最前面ウィンドウ以外のすべてのウィンドウの前
            /// </summary>
            public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
            /// <summary>
            /// ウィンドウを最前面ウィンドウ以外のすべてのウィンドウの前
            /// </summary>
            public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        }
        /// <summary>
        /// ウィンドウのEnable/Disable切替
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bEnable"></param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        /// <summary>
        /// 現在ウィンドウEnableか取得する
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32")]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        /// <summary>
        /// IsWindow HWNDの有効性判定
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);
        /// <summary>
        /// LockSetForegroundWindowフォアグラウンド移動の抑止
        /// </summary>
        /// <param name="uLockCode"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LockSetForegroundWindow(LockSetForegroundWindow_uLockCode uLockCode);
        /// <summary>
        /// LockSetForegroundWindow uLockCode値
        /// </summary>
        public enum LockSetForegroundWindow_uLockCode
        {
            /// <summary>
            /// フォアグラウンド抑止
            /// </summary>
            LSFW_LOCK = 1,
            /// <summary>
            /// フォアグラウンド抑止解除
            /// </summary>
            LSFW_UNLOCK = 2,
        }

        /// <summary>;
        /// ウィンドウパラメータの変更
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern UInt32 GetWindowLong(IntPtr hWnd, int index);
        /// <summary>
        /// ウィンドウパラメータの設定
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="index"></param>
        /// <param name="unValue"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern UInt32 SetWindowLong(IntPtr hWnd, int index, uint unValue);
        /// <summary>
        /// ウィンドウパラメータの変更指定
        /// </summary>
        public static class GetWindowLong_Index
        {
            /// <summary>
            /// WindProcへのポインタ
            /// </summary>
            public const int GWL_WINDPROC = -4;
            /// <summary>
            /// HINSTANCE
            /// </summary>
            public const int GWL_HINSTANCE = -6;
            /// <summary>
            /// 親ウィンドウのHWND
            /// </summary>
            public const int GWL_HWNDPARENT = -8;
            /// <summary>
            /// ウィンドウスタイル
            /// </summary>
            public const int GWL_STYLE = -16;
            /// <summary>
            /// 拡張ウィンドウスタイル
            /// </summary>
            public const int GWL_EXSTYLE = -20;
            /// <summary>
            /// ユーザーデータ
            /// </summary>
            public const int GWL_USERDATA = -21;
            /// <summary>
            /// ウィンドウID
            /// </summary>
            public const int GWL_ID = -12;
        }
        /// <summary>
        /// IsWindowVisible
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        private extern static int GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        [DllImport("user32.dll", EntryPoint = "AttachThreadInput")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);
        [DllImport("Kernel32.dll", EntryPoint = "GetCurrentThreadId")]
        private extern static int GetCurrentThreadId();
        /// <summary>
        /// 指定したFormのウインドウをアクティブにする
        /// </summary>
        /// <remarks>
        /// 指定したFormのウインドウをアクティブにする
        /// </remarks>
        /// <param name="form">アクティブにするForm</param>
        public static void SetActivate(Form form)
        {
            int fore_thread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            int this_thread = GetCurrentThreadId();
            AttachThreadInput(this_thread, fore_thread, true);

            // this をアクティブ
            form.Activate();

            // Thread のデタッチ
            AttachThreadInput(this_thread, fore_thread, false);
        }
        /// <summary>
        /// 指定したウインドウハンドルのウインドウをアクティブにする
        /// </summary>
        /// <remarks>
        /// 指定したウインドウハンドルのウインドウをアクティブにする
        /// </remarks>
        /// <param name="hwnd">ウインドウハンドル</param>
        public static void SetActivate(IntPtr hwnd)
        {
            if (IsWindowVisible(hwnd))
            {
                SetForegroundWindow(hwnd);  // アクティブにする
            }
        }
    }
}
