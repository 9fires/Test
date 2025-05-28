using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace cklib.Util
{
    /// <summary>
    /// コンソール制御
    /// </summary>
    public class Console
    {
        #region API定義
        [DllImport("kernel32.dll", EntryPoint = "AllocConsole")]
        private static extern bool AllocConsole();
        [DllImport("kernel32.dll", EntryPoint = "AttachConsole")]
        private static extern bool AttachConsole(int ProcessID);
        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        private static extern bool FreeConsole();
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCtrlHandler")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler,uint fadd);
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle")]
        private static extern IntPtr GetStdHandle(int nStdHandle);
        /// <summary>
        /// 呼び出し側プロセスのコンソールを共有するコンソールプロセスグループに、指定した信号を送信します。
        /// </summary>
        /// <param name="CtlEvent">送信する信号の種類を指定します。次のいずれかの定数を指定します。<br/>
        /// CTRL_C_EVENT	CTRL+C 信号を送信します。<br/>
        /// CTRL_BREAK_EVENT	CTRL+BREAK 信号を送信します。</param>
        /// <param name="ProcessID">信号の送信先プロセスグループの識別子を指定します。
        /// CreateProcess 関数を呼び出すときに CREATE_NEW_PROCESS_GROUP フラグをセットすると、
        /// プロセスグループが作成されます。新しいプロセスのプロセス識別子が新しいプロセスグループの
        /// プロセスグループ識別子にもなります。プロセスグループには、ルートプロセスの子孫となるすべてのプロセスが含まれます。
        /// 信号を受け取るのは、グループ内の、呼び出し側プロセスと同じコンソールを共有するプロセスだけです。
        /// つまり、グループ内のあるプロセスが新しいコンソールを作成すると、そのプロセスは信号を受け取りません。
        /// そのプロセスの子孫も信号を受け取りません。
        /// dwProcessGroupId パラメータに 0 を指定すると、呼び出し側プロセスのコンソールを共有するすべてのプロセスに
        /// 信号が送信されます。 </param>
        /// <returns>関数が成功すると、0 以外の値が返ります。
        /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、GetLastError 関数を使います。</returns>
        [DllImport("kernel32.dll", EntryPoint = "GenerateConsoleCtrlEvent")]
        public static extern bool GenerateConsoleCtrlEvent(int CtlEvent, int ProcessID);
        /// <summary>
        /// 呼び出し側プロセスのコンソールが使う入力コードページを返します。
        /// コンソールはその入力コードページを使って、キーボード入力を対応する文字値に変換します。
        /// </summary>
        /// <returns>コードページの識別コードが返ります。></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleCP")]
        public static extern int GetConsoleCP();
        /// <summary>
        /// 呼び出し側プロセスのコンソールが使う入力コードページを設定します。
        /// コンソールはその入力コードページを使って、キーボード入力を対応する文字値に変換します。
        /// </summary>
        /// <param name="cp">設定するコードページの識別子を指定します。
        /// ローカルコンピュータで利用できるコードページの識別子がレジストリの次のキーに格納されています。
        /// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Nls\CodePage </param>
        /// <returns>関数が成功すると、0 以外の値が返ります。
        /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、GetLastError 関数を使います。</returns>
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleCP")]
        public static extern bool SetConsoleCP(int cp);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleDisplayMode")]
        private static extern bool GetConsoleDisplayMode(ref int lpModeFlags);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleDisplayMode")]
        private static extern bool SetConsoleDisplayMode(IntPtr hConsoleOutput,int dwFlags,IntPtr lpNewScreenBufferDimensions);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleCursorInfo")]
        private static extern bool GetConsoleCursorInfo(IntPtr hConsoleOutput,IntPtr lpConsoleCursorInfo);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCursorInfo")]
        private static extern bool SetConsoleCursorInfo(IntPtr hConsoleOutput, IntPtr lpConsoleCursorInfo);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleMode")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle,ref int lpMode);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleMode")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int lpMode);
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentConsoleFont")]
        private static extern bool GetCurrentConsoleFont(IntPtr hConsoleOutput,int bMaximumWindow,IntPtr lpConsoleCurrentFont);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleFontSize")]
        private static extern uint GetConsoleFontSize(IntPtr hConsoleOutput,int nFont);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleTitle")]
        private static extern int GetConsoleTitle(StringBuilder lpConsoleTitle,int nSize);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleTitle")]
        private static extern bool SetConsoleTitle(StringBuilder lpConsoleTitle);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleScreenBufferSize")]
        private static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput,IntPtr dwSize);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleScreenBufferInfo")]
        private static extern bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, IntPtr lpConsoleScreenBufferInfo);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleWindowInfo")]
        private static extern bool SetConsoleWindowInfo(IntPtr hConsoleOutput,int bAbsolute,IntPtr lpConsoleWindow);
        [DllImport("kernel32.dll", EntryPoint = "SetConsoleWindowInfo")]
        private static extern uint GetLargestConsoleWindowSize(IntPtr hConsoleOutput);
        /// <summary>
        /// コンソールコールバックハンドラ定義
        /// </summary>
        /// <param name="dwDevice"></param>
        private delegate void HandlerRoutine(uint    dwDevice);
        /// <summary>
        /// コールバックイベントハンドラ
        /// </summary>
        private static HandlerRoutine handler;
        /// <summary>
        /// エラーコード取得
        /// </summary>
        /// <returns>Win32エラーコード</returns>
        [DllImport("kernel32.dll", EntryPoint = "GetLastError")]
        public static extern uint GetLastError();
        #endregion
        #region 構造体定義
        /// <summary>
        /// 構造体定義基本クラス
        /// </summary>
        public abstract class structBase
        {
            /// <summary>
            /// 指定バッファに書き込む
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public abstract void GetBytes(ref byte[] b, int offset);
            /// <summary>
            /// 指定バッファに書き込む
            /// </summary>
            /// <param name="b">バイト配列</param>
            public virtual void GetBytes(ref byte[] b)
            {
                this.GetBytes(ref b, 0);
            }
            /// <summary>
            /// 指定バッファからセットする
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public abstract void SetBytes(byte[] b, int offset);
            /// <summary>
            /// 指定バッファからセットする
            /// </summary>
            /// <param name="b">バイト配列</param>
            public virtual void SetBytes(byte[] b)
            {
                this.SetBytes(b, 0);
            }
            /// <summary>
            /// 構造体の長さ
            /// </summary>
            public abstract int Length
            {
                get;
            }
        }
        /// <summary>
        /// coordinates of a character cell
        /// </summary>
        public class COORD : structBase
        {
            /// <summary>
            /// 水平位置または幅
            /// </summary>
            public short x = 0;
            /// <summary>
            /// 垂直位置または高さ
            /// </summary>
            public short y = 0;
            /// <summary>
            /// 指定バッファに書き込む
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void GetBytes(ref byte[] b, int offset)
            {
                cklib.Util.ByteArray.Intel.FromShort(this.x, ref b, offset);
                cklib.Util.ByteArray.Intel.FromShort(this.y, ref b, offset + 2);
            }
            /// <summary>
            /// 指定バッファからセットする
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void SetBytes(byte[] b, int offset)
            {
                cklib.Util.ByteArray.Intel.ToShort(b, offset, out this.x);
                cklib.Util.ByteArray.Intel.ToShort(b, offset + 2, out this.y);
            }
            /// <summary>
            /// 構造体の長さ
            /// </summary>
            public override int Length
            {
                get { return 4; }
            }
        }
        /// <summary>
        /// coordinates of the upper left and lower right corners of a rectangle.
        /// </summary>
        public class SMALL_RECT : structBase
        {
            /// <summary>
            /// 左端
            /// </summary>
            public short Left = 0;
            /// <summary>
            /// 上端
            /// </summary>
            public short Top = 0;
            /// <summary>
            /// 右端
            /// </summary>
            public short Right = 0;
            /// <summary>
            /// 下端
            /// </summary>
            public short Bottom = 0;
            /// <summary>
            /// 指定バッファに書き込む
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void GetBytes(ref byte[] b, int offset)
            {
                cklib.Util.ByteArray.Intel.FromShort(this.Left, ref b, offset);
                cklib.Util.ByteArray.Intel.FromShort(this.Top, ref b, offset + 2);
                cklib.Util.ByteArray.Intel.FromShort(this.Right, ref b, offset + 4);
                cklib.Util.ByteArray.Intel.FromShort(this.Bottom, ref b, offset + 6);
            }
            /// <summary>
            /// 指定バッファからセットする
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void SetBytes(byte[] b, int offset)
            {
                cklib.Util.ByteArray.Intel.ToShort(b, offset, out this.Left);
                cklib.Util.ByteArray.Intel.ToShort(b, offset + 2, out this.Top);
                cklib.Util.ByteArray.Intel.ToShort(b, offset + 4, out this.Right);
                cklib.Util.ByteArray.Intel.ToShort(b, offset + 6, out this.Bottom);
            }
            /// <summary>
            /// 構造体の長さ
            /// </summary>
            public override int Length
            {
                get { return 8; }
            }
        }
        /// <summary>
        /// contains information about a console screen buffer.
        /// </summary>
        public class CONSOLE_SCREEN_BUFFER_INFO : structBase
        {
            /// <summary>
            /// contains the size of the console screen buffer
            /// </summary>
            public COORD dwSize = new COORD();
            /// <summary>
            /// column and row coordinates of the cursor in the console screen buffer
            /// </summary>
            public COORD dwCursorPosition = new COORD();
            /// <summary>
            /// Attributes of the characters written to a screen buffer 
            /// </summary>
            public ushort wAttributes = 0;
            /// <summary>
            /// console screen buffer coordinates of the upper-left and lower-right corners of the display window. 
            /// </summary>
            public SMALL_RECT srWindow = new SMALL_RECT();
            /// <summary>
            /// contains the maximum size 
            /// </summary>
            public COORD dwMaximumWindowSize = new COORD();
            /// <summary>
            /// 指定バッファに書き込む
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void GetBytes(ref byte[] b, int offset)
            {
                this.dwSize.GetBytes(ref b, offset);
                offset += this.dwSize.Length;
                this.dwCursorPosition.GetBytes(ref b, offset);
                offset += this.dwCursorPosition.Length;
                cklib.Util.ByteArray.Intel.FromUShort(this.wAttributes, ref b, offset);
                offset += 2;
                this.srWindow.GetBytes(ref b, offset);
                offset += this.srWindow.Length;
                this.dwMaximumWindowSize.GetBytes(ref b, offset);
            }
            /// <summary>
            /// 指定バッファからセットする
            /// </summary>
            /// <param name="b">バイト配列</param>
            /// <param name="offset">配列へのオフセット</param>
            public override void SetBytes(byte[] b, int offset)
            {
                this.dwSize.SetBytes(b, offset);
                offset += this.dwSize.Length;
                this.dwCursorPosition.SetBytes(b, offset);
                offset += this.dwCursorPosition.Length;
                cklib.Util.ByteArray.Intel.ToUShort(b, offset, out this.wAttributes);
                offset += 2;
                this.srWindow.SetBytes(b, offset);
                offset += this.srWindow.Length;
                this.dwMaximumWindowSize.SetBytes(b, offset);
            }
            /// <summary>
            /// 構造体の長さ
            /// </summary>
            public override int Length
            {
                get
                {
                    return this.dwSize.Length +
                            this.dwCursorPosition.Length +
                            2 +
                            this.srWindow.Length +
                            this.dwMaximumWindowSize.Length;
                }
            }

        }
        #endregion
        #region コンソールオープンクローズ
        /// <summary>
        /// コンソールを開く
        /// </summary>
        /// <returns></returns>
        public static bool Alloc()
        {
            handler = new HandlerRoutine(ConsoleHandlerRoutine);
            if (!AllocConsole())
            {
                return false;
            }
            if (!SetConsoleCtrlHandler(handler,1))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// コンソールにアタッチする
        /// </summary>
        /// <param name="ProcessID">アタッチするコンソールを所有するプロセス</param>
        /// <returns></returns>
        public static bool Attach(int ProcessID)
        {
            handler = new HandlerRoutine(ConsoleHandlerRoutine);
            if (!AttachConsole(ProcessID))
            {
                return false;
            }
            if (!SetConsoleCtrlHandler(handler, 1))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 親プロセスコンソールにアタッチする場合
        /// </summary>
        public const int ATTACH_PARENT_PROCESS = -1;
        /// <summary>
        /// コンソールを閉じる
        /// </summary>
        /// <returns></returns>
        public static bool Close()
        {
            return FreeConsole();
        }
        #endregion

        #region イベント制御
        /// <summary>
        /// コンソールコールバックハンドラ
        /// </summary>
        /// <param name="dwDevice"></param>
        public static void ConsoleHandlerRoutine(uint dwDevice)
        {
            if (ConsoleEventHandler!=null)
            {
                ConsoleEventHandler(dwDevice);
            }
        }
        /// <summary>
        /// コンソールイベントデリゲート定義
        /// </summary>
        /// <param name="reason">イベント理由<br/>0:CTRL+C<BR/>1:CTRL+BREAK<BR/>2:CLOSE<BR/>5:LogOff<BR/>6:ShutDown<BR/></param>
        public delegate void ConsoleHandlerEventRoutine(uint reason);
        /// <summary>
        /// コンソールイベントデリゲート
        /// </summary>
        public static ConsoleHandlerEventRoutine ConsoleEventHandler = null;
        #endregion
        #region 設定
        /// <summary>
        /// コンソールモードの取得
        /// </summary>
        /// <returns></returns>
        public static int GetDisplayMode()
        {
            int lpModeFlags=0;
            if (GetConsoleDisplayMode(ref lpModeFlags))
            {
                return lpModeFlags;
            }
            return -1;
        }
        /// <summary>
        /// コンソールモード設定
        /// </summary>
        /// <param name="dwFlags">1 フルスクリーンモード<br/>2 ウインドウモード</param>
        /// <param name="x">スクリーンバッファ桁数</param>
        /// <param name="y">スクリーンバッファ行数</param>
        /// <returns></returns>
        public static bool SetDisplayMode(int dwFlags,out int x,out int y)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                byte[] buff=new byte[4];
                fixed (byte* p = buff)
                {
                    r = SetConsoleDisplayMode(hConsoleOutput,dwFlags,(IntPtr)p);
                }
                x = Util.ByteArray.Intel.ToShort(buff, 0);
                y = Util.ByteArray.Intel.ToShort(buff, 2);
            }
            return r;
        }
        /// <summary>
        /// コンソールモード設定
        /// </summary>
        /// <param name="dwFlags">1 フルスクリーンモード<br/>2 ウインドウモード</param>
        /// <returns></returns>
        public static bool SetDisplayMode(int dwFlags)
        {
            int x, y;
            return SetDisplayMode(dwFlags,out x,out y);
        }
        /// <summary>
        /// カーソル情報の取得
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <param name="visible">表示状態</param>
        /// <returns></returns>
        public static bool GetCursorInfo(out int size, out bool visible)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                byte[] buff = new byte[8];
                fixed (byte* p = buff)
                {
                    r = GetConsoleCursorInfo(hConsoleOutput, (IntPtr)p);
                }
                size = Util.ByteArray.Intel.ToInt(buff, 0);
                if (Util.ByteArray.Intel.ToInt(buff, 4) == 0)
                {
                    visible = false;
                }
                else
                {
                    visible = true;
                }
            }
            return r;
        }
        /// <summary>
        /// カーソル情報の設定
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <param name="visible">表示状態</param>
        /// <returns></returns>
        public static bool SetCursorInfo(int size,bool visible)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                byte[] buff = new byte[8];
                Util.ByteArray.Intel.FromInt(size,ref buff, 0);
                if (visible)
                    Util.ByteArray.Intel.FromInt(1,ref buff, 4);
                else
                    Util.ByteArray.Intel.FromInt(0,ref buff, 4);
                fixed (byte* p = buff)
                {
                    r = SetConsoleCursorInfo(hConsoleOutput, (IntPtr)p);
                }
            }
            return r;

        }
        #region コンソールモード定義
        /// <summary>
        /// Characters read by the ReadFile or ReadConsole function are written to
        /// the active screen buffer as they are read.
        /// This mode can be used only if the ENABLE_LINE_INPUT mode is also enabled. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_ECHO_INPUT =   0x0004;
        /// <summary>
        /// When enabled, text entered in a console window will be inserted at the
        /// current cursor location and all text following that location will not 
        /// be overwritten. When disabled, all following text will be overwritten.
        /// An OR operation must be performed with this flag and the ENABLE_EXTENDED_FLAGS flag
        /// to enable this functionality. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_INSERT_MODE=0x0020;
        /// <summary>
        /// The ReadFile or ReadConsole function returns only when a carriage return 
        /// character is read. If this mode is disabled, the functions return when 
        /// one or more characters are available
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_LINE_INPUT=0x0002;
        /// <summary>
        /// If the mouse pointer is within the borders of the console window and the 
        /// window has the keyboard focus, mouse events generated by mouse movement 
        /// and button presses are placed in the input buffer.
        /// These events are discarded by ReadFile or ReadConsole, even when this mode is enabled. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_MOUSE_INPUT=0x0010;
        /// <summary>
        /// CTRL+C is processed by the system and is not placed in the input buffer.
        /// If the input buffer is being read by ReadFile or ReadConsole, other control
        /// keys are processed by the system and are not returned in the ReadFile or 
        /// ReadConsole buffer. If the ENABLE_LINE_INPUT mode is also enabled, backspace,
        /// carriage return, and linefeed characters are handled by the system. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_PROCESSED_INPUT=0x0001;
        /// <summary>
        /// This flag enables the user to use the mouse to select and edit text. 
        /// To enable this option, use the OR to combine this flag with ENABLE_EXTENDED_FLAGS.  
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_QUICK_EDIT_MODE=0x0040;
        /// <summary>
        /// User interactions that change the size of the console screen buffer are 
        /// reported in the console's input buffer. Information about these events can 
        /// be read from the input buffer by applications using the ReadConsoleInput function, 
        /// but not by those using ReadFile or ReadConsole. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_WINDOW_INPUT=0x0008;
        /// <summary>
        /// Characters written by the WriteFile or WriteConsole function or echoed by the ReadFile 
        /// or ReadConsole function are examined for ASCII control sequences and the correct action 
        /// is performed. Backspace, tab, bell, carriage return, and linefeed characters are processed. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_PROCESSED_OUTPUT=0x0001;
        /// <summary>
        /// When writing with WriteFile or WriteConsole or echoing with ReadFile or ReadConsole,
        /// the cursor moves to the beginning of the next row when it reaches the end of the current row.
        /// This causes the rows displayed in the console window to scroll up automatically when the cursor
        /// advances beyond the last row in the window. It also causes the contents of the console screen 
        /// buffer to scroll up (discarding the top row of the console screen buffer) when the cursor advances
        /// beyond the last row in the console screen buffer. If this mode is disabled, the last character 
        /// in the row is overwritten with any subsequent characters.
        /// Windows Me/98/95:  This value is not supported. 
        /// </summary>
        public const int CONSOLE_MODE_ENABLE_WRAP_AT_EOL_OUTPUT=0x0002;
        #endregion

        /// <summary>
        /// 入力モード設定
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool SetInputMode(int mode)
        {
            IntPtr hConsole= GetSTDINHandle();
            return SetConsoleMode(hConsole,mode);
        }
        /// <summary>
        /// 入力モード設定
        /// </summary>
        /// <returns></returns>
        public static int GetInputMode()
        {
            IntPtr hConsole = GetSTDINHandle();
            int mode=0;
            if (GetConsoleMode(hConsole, ref mode))
            {
                return mode;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 出力モード設定
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool SetOutputMode(int mode)
        {
            IntPtr hConsole = GetSTDOutHandle();
            return SetConsoleMode(hConsole, mode);
        }
        /// <summary>
        /// 出力モード設定
        /// </summary>
        /// <returns></returns>
        public static int GetOutputMode()
        {
            IntPtr hConsole = GetSTDOutHandle();
            int mode = 0;
            if (GetConsoleMode(hConsole, ref mode))
            {
                return mode;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 現在のコンソールのフォントとフォントサイズを取得する
        /// </summary>
        /// <param name="bMaximumWindow">If this parameter is TRUE, font information is retrieved for the maximum window size.
        /// If this parameter is FALSE, font information is retrieved for the current window size.
        /// このパラメータがtrueの場合、フォント情報は、最大ウィンドウサイズのために取得されます。
        /// このパラメータがFALSEの場合、フォント情報は、現在のウィンドウサイズのために取得されます。</param>
        /// <param name="nFont">フォントサイズ</param>
        /// <param name="w">フォント幅</param>
        /// <param name="h">フォント高</param>
        /// <returns></returns>
        public static bool GetCurrentConsoleFont(bool bMaximumWindow, out int nFont, out int w, out int h)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                byte[] buff = new byte[8];
                fixed (byte* p = buff)
                {
                    r = GetCurrentConsoleFont(hConsoleOutput,(bMaximumWindow?1:0), (IntPtr)p);
                }
                nFont = Util.ByteArray.Intel.ToInt(buff, 0);
                w = Util.ByteArray.Intel.ToShort(buff, 4);
                h = Util.ByteArray.Intel.ToShort(buff, 6);
            }
            return r;
            
        }
        //[DllImport("kernel32.dll", EntryPoint = "GetConsoleFontSize")]
        //private static extern uint GetConsoleFontSize(HANDLE hConsoleOutput, DWORD nFont);
        /// <summary>
        /// コンソールタイトルの取得
        /// </summary>
        /// <returns></returns>
        public static string GetTitle()
        {
            int nSize   =   4096;
            StringBuilder ConsoleTitle=new StringBuilder(nSize);
            int sz = GetConsoleTitle(ConsoleTitle, nSize);
            if (sz>nSize)
            {
                ConsoleTitle=new StringBuilder(sz+1);
                sz = GetConsoleTitle(ConsoleTitle, nSize);
            }
            if (sz==0)
            {
                return string.Empty;
            }
            return ConsoleTitle.ToString().Substring(0, sz-1);
        }
        /// <summary>
        /// コンソールタイトルを設定する
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static bool SetTitle(string title)
        {
            StringBuilder ConsoleTitle = new StringBuilder(4096);
            ConsoleTitle.Append(title);
            return SetConsoleTitle(ConsoleTitle);
        }
        /// <summary>
        /// スクリーンバッファの設定
        /// </summary>
        /// <param name="w">幅</param>
        /// <param name="h">高さ</param>
        /// <returns></returns>
        public static bool SetScreenBufferSize(int w,int h)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            COORD coord = new COORD();
            coord.x = (short)w;
            coord.y = (short)h;
            unsafe
            {
                byte[] buff = new byte[coord.Length];
                coord.GetBytes(ref buff);
                fixed (byte* p = buff)
                {
                    r = SetConsoleScreenBufferSize(hConsoleOutput, (IntPtr)p);
                }
            }
            return r;
        }
        /// <summary>
        /// スクリーンバッファ情報の取得
        /// </summary>
        /// <param name="info">情報構造体</param>
        /// <returns></returns>
        public  static bool GetScreenBufferInfo(out CONSOLE_SCREEN_BUFFER_INFO info)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                info = new CONSOLE_SCREEN_BUFFER_INFO();
                byte[] buff = new byte[info.Length];
                fixed (byte* p = buff)
                {
                    r = GetConsoleScreenBufferInfo(hConsoleOutput, (IntPtr)p);
                }
                info.SetBytes(buff);
            }
            return r;
        }
        /// <summary>
        /// ウインドウ情報設定
        /// </summary>
        /// <param name="left">左端</param>
        /// <param name="top">上端</param>
        /// <param name="right">右端</param>
        /// <param name="bottom">下端</param>
        /// <returns></returns>
        public static bool SetWindowInfo(int left, int top, int right, int bottom)
        {
            return SetWindowInfo(true, left, top, right, bottom);
        }
        /// <summary>
        /// ウインドウ情報設定
        /// </summary>
        /// <param name="width">幅</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static bool SetWindowInfo(int width, int height)
        {
            return SetWindowInfo(false, 0, 0, width, height);
        }
        /// <summary>
        /// ウインドウ情報設定
        /// </summary>
        /// <param name="bAbsolute">左端指定の有無</param>
        /// <param name="left">左端</param>
        /// <param name="top">上端</param>
        /// <param name="right">右端</param>
        /// <param name="bottom">下端</param>
        /// <returns></returns>
        public static bool SetWindowInfo(bool bAbsolute,int left,int top,int right,int bottom)
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            bool r;
            unsafe
            {
                SMALL_RECT sr = new SMALL_RECT();
                sr.Left = (short)left;
                sr.Top = (short)top;
                sr.Right = (short)right;
                sr.Bottom = (short)bottom;
                byte[] buff = new byte[sr.Length];
                sr.GetBytes(ref buff);
                fixed (byte* p = buff)
                {
                    r = SetConsoleWindowInfo(hConsoleOutput,(bAbsolute?1:0), (IntPtr)p);
                }
            }
            return r;
        }
        /// <summary>
        /// 最大ウインドウサイズの取得
        /// </summary>
        /// <returns></returns>
        public static COORD GetLargestConsoleWindowSize()
        {
            IntPtr hConsoleOutput = GetSTDOutHandle();
            uint r = GetLargestConsoleWindowSize(hConsoleOutput);
            COORD coord=new COORD();
            byte[] rr = new byte[coord.Length];
            Util.ByteArray.Intel.FromUInt(r, ref rr);
            coord.SetBytes(rr);
            return coord;
        }
        #endregion
        #region 標準入出力ハンドル
        /// <summary>
        /// Handle to the standard input device. Initially, this is a handle to the console input buffer, CONIN$. 
        /// </summary>
        private const int STD_INPUT_HANDLE=-10;
        /// <summary>
        /// Handle to the standard output device. Initially, this is a handle to the active console screen buffer, CONOUT$. 
        /// </summary>
        private const int STD_OUTPUT_HANDLE=-11;
        /// <summary>
        /// Handle to the standard error device. Initially, this is a handle to the active console screen buffer, CONOUT$. 
        /// </summary>
        private const int STD_ERROR_HANDLE=-12;
        /// <summary>
        /// 標準入力ハンドラを取得
        /// </summary>
        /// <remarks>
        /// CONIN$デバイス、コンソール入力バッファ
        /// </remarks>
        /// <returns>標準入力</returns>
        public static IntPtr GetSTDINHandle()
        {
            return GetStdHandle(STD_INPUT_HANDLE);
        }
        /// <summary>
        /// 標準出力ハンドラを取得
        /// </summary>
        /// <remarks>
        /// CONOUT$デバイス、コンソールスクリーンバッファ
        /// </remarks>
        /// <returns>標準入力</returns>
        public static IntPtr GetSTDOutHandle()
        {
            return GetStdHandle(STD_OUTPUT_HANDLE);
        }
        /// <summary>
        /// 標準エラーハンドラを取得
        /// </summary>
        /// <remarks>
        /// CONOUT$デバイス、コンソールスクリーンバッファ
        /// </remarks>
        /// <returns>標準入力</returns>
        public static IntPtr GetSTDErrHandle()
        {
            return GetStdHandle(STD_INPUT_HANDLE);
        }
        #endregion
    }
}
