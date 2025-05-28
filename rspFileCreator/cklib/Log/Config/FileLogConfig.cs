using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace cklib.Log.Config
{
    /// <summary>
    /// 個別設定項目共通設定項目情報
    /// </summary>
    [Serializable]
    public class FileLogConfig : BasicLogConfig
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="e"></param>
        public FileLogConfig(LoggerConfigElement e)
            : base(LoggerConfig.FileElementCollectionName, e)
        {
            this.Path   = e.Path;
            this.FileName = e.FileName;
            this.fileLock = e.FileLock;
            this.fileLockWaitTime = e.FileLockWaitTime;
            this.fileLockTryLimit = e.FileLockTryLimit;
            this.mutexLock = e.MutexLock;
            this.mutexLockWaitTime = e.MutexLockWaitTime;
            this.compress = e.Compress;
            this.rotateDays = e.RotateDays;
            this.SetRotateSize(e.RotateSize);
            this.rotateFileName = e.RotateFileName;
            this.zipRotate = e.ZipRotate;
            this.zipRotateDays = cklib.Util.String.ToInt(e.ZipRotateDays, -1);
            this.zipRotatePassword = e.ZipRotatePassword;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FileLogConfig()
            : base(LoggerConfig.FileElementCollectionName)
        {}
        /// <summary>
        /// 出力パス
        /// </summary>
        public string Path
        {
            get
            {
                return  this.logPath;
            }
            set
            {
                string path = Environment.ExpandEnvironmentVariables(value);
                if (path.Length == 0)
                {	//	設定無し
                    Assembly asm= Assembly.GetExecutingAssembly();
                    path = System.IO.Path.GetDirectoryName(asm.Location);
                }
                if (path[0] == '.')
                {	//	相対ディレクトリ
                    Assembly asm = Assembly.GetExecutingAssembly();
                    path = System.IO.Path.GetDirectoryName(asm.Location) + "\\" + path;
                }
                if (!path.EndsWith("\\"))
                {
                    path += "\\";   
                }
                this.logPath = path;
            }
        }
        /// <summary>
        /// 出力パス
        /// </summary>
        private string logPath = string.Empty;
        /// <summary>
        /// 出力ファイル名
        /// </summary>
        public string FileName
        {
            get
            {
                return this.logFileName;
            }
            set
            {
                this.logFileName = value;
            }
        }
        /// <summary>
        /// 出力ファイル名
        /// </summary>
        private string logFileName = string.Empty;
        /// <summary>
        /// ログファイルロック
        /// </summary>
        /// <remarks>
        /// ログファイルを書き込みロックする<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        public bool FileLock
        {
            get
            {
                return this.fileLock;
            }
            set
            {
                this.fileLock = value;
            }
        }
        /// <summary>
        /// ログファイルロック
        /// </summary>
        private bool fileLock = false;
        /// <summary>
        /// ログファイルロックリトライインターバル(msec
        /// </summary>
        /// <remarks>
        /// ログファイルオープン時の排他ロックによるリトライ時間<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        public int FileLockWaitTime
        {
            get
            {
                return this.fileLockWaitTime;
            }
            set
            {
                this.fileLockWaitTime = value;
            }
        }
        /// <summary>
        /// ログファイルロックリトライインターバル(msec
        /// </summary>
        private int fileLockWaitTime = 1000;
        /// <summary>
        /// ログファイルロックリトライ回数
        /// </summary>
        /// <remarks>
        /// ログファイルオープン時の排他ロックによるリトライ試行回数<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        public int FileLockTryLimit
        {
            get
            {
                return this.fileLockTryLimit;
            }
            set
            {
                this.fileLockTryLimit = value;
            }
        }
        private int fileLockTryLimit = -1;
        /// <summary>
        /// ログ排他制御Mutex有効
        /// </summary>
        public bool MutexLockEnable
        {
            get
            {
                if (this.MutexLock.Length != 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// ログ排他制御Mutex名
        /// </summary>
        /// <remarks>
        /// ログ書き込みをMutexにより排他制御する。空文字の場合は無効
        /// </remarks>
        public string MutexLock
        {
            get
            {
                return this.mutexLock;
            }
            set
            {
                this.mutexLock = value;
            }
        }
        private string mutexLock = string.Empty;
        /// <summary>
        /// ログ排他制御Mutex待機時間(msec)
        /// </summary>
        /// <remarks>
        /// ログ排他制御Mutexをロック出来るまでの待機時間<br/>
        /// タイムオーバーした場合は、ログ書き込みは失敗する。
        /// </remarks>
        public int MutexLockWaitTime
        {
            get
            {
                return this.mutexLockWaitTime;
            }
            set
            {
                this.mutexLockWaitTime = value;
            }
        }
        private int mutexLockWaitTime = -1;
        /// <summary>
        /// ログを圧縮ファイルにする
        /// </summary>
        public bool Compress
        {
            get
            {
                return this.compress;
            }
            set
            {
                this.compress = value;
            }
        }
        /// <summary>
        /// ログを圧縮ファイルにする
        /// </summary>
        private bool compress = false;

        /// <summary>
        /// ログローテション保存日数
        /// </summary>
        public int RotateDays
        {
            get
            {
                return this.rotateDays;
            }
            set
            {
                this.rotateDays = value;
            }
        }
        /// <summary>
        /// ログローテション保存日数
        /// </summary>
        private int rotateDays = -1;
        /// <summary>
        /// ログローテションサイズ指定
        /// </summary>
        /// <param name="str">ローテーションサイズを文字列で指定する<bf/>K/M/Gの単位指定可</param>
        public void SetRotateSize(string str)
        {
            if (!str.StartsWith("-"))
            {
                long x = 1;
                if (str.Length > 1)
                {
                    string xx = str.Substring(str.Length - 1, 1);
                    switch (xx.ToUpper())
                    {
                        case "K": x = 1024; break;
                        case "M": x = 1024 * 1024; break;
                        case "G": x = 1024 * 1024 * 1024; break;
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                decimal value = Decimal.Parse(str) * x;
                this.rotateSize = Decimal.ToInt64(value);
            }
            this.rotateSizeStr = str;
        }
        /// <summary>
        /// ログローテションサイズ指定
        /// </summary>
        private string rotateSizeStr = "-1";

        /// <summary>
        /// ログローテションサイズ指定
        /// </summary>
        public long RotateSize
        {
            get
            {
                return this.rotateSize;
            }
            set
            {
                this.rotateSize = value;
            }
        }
        /// <summary>
        /// ログローテションサイズ指定
        /// </summary>
        private long rotateSize = -1;
        /// <summary>
        /// ローテートログファイル名
        /// </summary>
        /// <remarks>
        /// 項目番号　項目内容<br/>
        /// {0} ログファイル名<br/>
        /// {1} 日時<br/>
        /// {2} ㍉秒<br/>
        /// {3} 同日ログの通番<br/>
        /// 通番と日時混在は不可
        /// 通番は、編集を可変すると最大値抽出が困難となるので、位置固定とします。
        /// デフォルトは以下の書式<br/>
        /// {0}.{3}.log<br/>
        /// </remarks>
        public string RotateFileName
        {
            get
            {
                return this.rotateFileName;
            }
            set
            {
                this.rotateFileName = value;
            }
        }
        /// <summary>
        /// ローテートログファイル名
        /// </summary>
        private string rotateFileName = string.Empty;
        /// <summary>
        /// ZIPローテション
        /// </summary>
        public bool ZipRotate
        {
            get
            {
                return this.zipRotate;
            }
            set
            {
                this.zipRotate = value;
            }
        }
        /// <summary>
        /// ZIPローテション
        /// </summary>
        private bool zipRotate=false;
        /// <summary>
        /// ZIPローテション実施日数
        /// </summary>
        public int ZipRotateDays
        {
            get
            {
                return this.zipRotateDays;
            }
            set
            {
                this.zipRotateDays = value;
            }
        }
        /// <summary>
        /// ZIPローテション実施日数
        /// </summary>
        private int zipRotateDays = -1;
        /// <summary>
        /// ZIPローテションパスワード
        /// </summary>
        public string ZipRotatePassword
        {
            get
            {
                return this.zipRotatePassword;
            }
            set
            {
                this.zipRotatePassword = value;
            }
        }
        /// <summary>
        /// ZIPローテションパスワード
        /// </summary>
        private string zipRotatePassword = string.Empty;

    }
}
