using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace cklib.Log
{
    /// <summary>
    /// ログコード定義
    /// </summary>
    public class LogCodes
    {
        #region 管理情報
        /// <summary>
        /// 内部ログコード基準値
        /// </summary>
        private static int InnerLogCodeBase = 0;
        /// <summary>
        /// ログコード管理テーブル
        /// </summary>
        private static Dictionary<int, LogCodes> LogCodeTable = new Dictionary<int, LogCodes>();
        #endregion

        #region インスタンス毎の情報
        /// <summary>
        /// 内部ログコード
        /// </summary>
        public readonly int InnerLogCode = 0;
        /// <summary>
        /// 内部ログコードを使用する
        /// </summary>
        public readonly bool UseInstanceLogCode = false;
 
        /// <summary>
        /// ログコード名を取得
        /// </summary>
        public string Name
        {
            get
            {
                if (this.m_name == null)
                {
                    this.m_name = this.LookupName();
                }
                return this.m_name;
            }
        }
        /// <summary>
        /// ログコード（設定ファイルにより定義等されていないデフォルト値)
        /// </summary>
        private readonly int m_LogCode = 0;
        /// <summary>
        /// ログコード（設定ファイルにより定義等されていないデフォルト値)
        /// </summary>
        public virtual int LogCode
        {
            get
            {
                return this.m_LogCode;
            }
        }
        private string m_name = null;
        /// <summary>
        /// ログレベル（設定ファイルにより定義等されていないデフォルト値)
        /// </summary>
        public readonly LogLevel LogLevel = LogLevel.Undefine;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogCode">内部ログコード</param>
        protected LogCodes(int LogCode)
        {
            lock (LogCodeTable)
            {
                this.InnerLogCode = Interlocked.Increment(ref LogCodes.InnerLogCodeBase);
                LogCodeTable.Add(this.InnerLogCode, this);
                this.m_LogCode = LogCode;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogCode">内部ログコード</param>
        /// <param name="UseInnerLogCode">ログ出力時に内部ログコードを使用する</param>
        protected LogCodes(int LogCode, bool UseInnerLogCode)
        {
            lock (LogCodeTable)
            {
                this.InnerLogCode = Interlocked.Increment(ref LogCodes.InnerLogCodeBase);
                LogCodeTable.Add(this.InnerLogCode, this);
                this.m_LogCode = LogCode;
                this.UseInstanceLogCode = UseInnerLogCode;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogLevel">内部ログレベル</param>
        /// <param name="LogCode">内部ログコード</param>
        protected LogCodes(LogLevel LogLevel,int LogCode)
        {
            lock (LogCodeTable)
            {
                this.InnerLogCode = Interlocked.Increment(ref LogCodes.InnerLogCodeBase);
                LogCodeTable.Add(this.InnerLogCode, this);
                this.m_LogCode = LogCode;
                this.LogLevel = LogLevel;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogLevel">内部ログレベル</param>
        /// <param name="LogCode">内部ログコード</param>
        /// <param name="UseInnerLogCode">ログ出力時に内部ログコードを使用する</param>
        protected LogCodes(LogLevel LogLevel, int LogCode, bool UseInnerLogCode)
        {
            lock (LogCodeTable)
            {
                this.InnerLogCode = Interlocked.Increment(ref LogCodes.InnerLogCodeBase);
                LogCodeTable.Add(this.InnerLogCode, this);
                this.m_LogCode = LogCode;
                this.UseInstanceLogCode = UseInnerLogCode;
                this.LogLevel = LogLevel;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected LogCodes()
        {
            lock (LogCodeTable)
            {
                this.InnerLogCode = Interlocked.Increment(ref LogCodes.InnerLogCodeBase);
                LogCodeTable.Add(this.InnerLogCode, this);
                this.m_LogCode = 0;
            }
        }
        #endregion

        #region 内部メソッド
        /// <summary>
        /// 名前の抽出
        /// </summary>
        /// <returns></returns>
        private string LookupName()
        {
            var type = this.GetType();
            var chkt = typeof(LogCodes);
            FieldInfo[] members = type.GetFields(
                BindingFlags.Public | BindingFlags.Static |
                BindingFlags.DeclaredOnly);
            foreach (var m in members)
            {
                if (!m.FieldType.Equals(chkt))
                    if (!m.FieldType.IsSubclassOf(chkt))
                        continue;
                var inst = m.GetValue(null) as LogCodes;
                if (inst.InnerLogCode == this.InnerLogCode)
                {
                    return m.FieldType.FullName + '.' + m.Name;
                }
            }
            return "";
        }
        #endregion

        #region 公開オーバーロードメソッド
        /// <summary>
        /// 内部ログコードを返す
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.InnerLogCode;
        }
        /// <summary>
        /// LogCodeの一致確認
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }
        /// <summary>
        /// ログコード名を返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
        /// <summary>
        /// 内部ログコードからログコードインスタンスを取得する
        /// </summary>
        /// <param name="logcodes"></param>
        /// <returns></returns>
        public static LogCodes GetLogCodesFromInnerLogCode(int InnnerLogCode)
        {
            lock(LogCodeTable)
            {
                if (LogCodeTable.ContainsKey(InnnerLogCode))
                    return LogCodeTable[InnnerLogCode];
            }
            return null;
        }
        #endregion

        #region 基本共通ログコード定義
        /// <summary>
        /// システムエラー
        /// </summary>
        public readonly static LogCodes SystemError = new LogCodes(LogLevel.ERROR, 1000);
        /// <summary>
        /// プログラムエラー
        /// </summary>
        public readonly static LogCodes ProgramError = new LogCodes(LogLevel.ERROR, 1001);
        /// <summary>
        /// 設定エラー
        /// </summary>
        public readonly static LogCodes ConfigError = new LogCodes(LogLevel.ERROR, 1002);
        /// <summary>
        /// パラメータエラー
        /// </summary>
        public readonly static LogCodes ParamError = new LogCodes(LogLevel.ERROR, 1003);
        /// <summary>
        /// ログ初期化エラー
        /// </summary>
        public readonly static LogCodes LogInitializeError = new LogCodes(LogLevel.ERROR, 1004);
        /// <summary>
        /// ログ設定エラー
        /// </summary>
        public readonly static LogCodes LogConfigError = new LogCodes(LogLevel.ERROR, 1005);
        /// <summary>
        /// ログエラー
        /// </summary>
        public readonly static LogCodes LogError = new LogCodes(LogLevel.ERROR, 1006);
        /// <summary>
        /// 起動開始
        /// </summary>
        public readonly static LogCodes BootUpStart = new LogCodes(LogLevel.INFO, 1010);
        /// <summary>
        /// 起動終了
        /// </summary>
        public readonly static LogCodes BootUpTerminate = new LogCodes(LogLevel.INFO, 1011);
        /// <summary>
        /// 起動中止
        /// </summary>
        public readonly static LogCodes BootUpAbort = new LogCodes(1012);
        /// <summary>
        /// 起動エラー
        /// </summary>
        public readonly static LogCodes BootUpError = new LogCodes(LogLevel.ERROR, 1013);
        /// <summary>
        /// 停止開始
        /// </summary>
        public readonly static LogCodes ShutdownStart = new LogCodes(LogLevel.INFO, 1020);
        /// <summary>
        /// 停止終了
        /// </summary>
        public readonly static LogCodes ShutdownTerminate = new LogCodes(LogLevel.INFO, 1021);
        /// <summary>
        /// 停止エラー
        /// </summary>
        public readonly static LogCodes ShutdownError = new LogCodes(LogLevel.ERROR, 1022);
        /// <summary>
        /// 強制停止終了
        /// </summary>
        public readonly static LogCodes ForceTerminate = new LogCodes(LogLevel.WARN, 1023);
        /// <summary>
        /// Unhandle例外による停止終了
        /// </summary>
        public readonly static LogCodes UnhandleExceptionTerminate = new LogCodes(LogLevel.ERROR, 1024);
        /// <summary>
        /// コンソールクローズによる停止終了
        /// </summary>
        public readonly static LogCodes ConsoleCloseTerminate = new LogCodes(LogLevel.WARN, 1025);
        /// <summary>
        /// スレッドプールワーカー起動エラー
        /// </summary>
        public readonly static LogCodes ThreadPoolWorkerStartError = new LogCodes(LogLevel.ERROR, 1030);
        /// <summary>
        /// スレッドプールワーカー実行タイムアウト検出
        /// </summary>
        public readonly static LogCodes ThreadPoolWorkerAliveTimeoutDetect = new LogCodes(LogLevel.ERROR, 1031);
        /// <summary>
        /// スレッドプールワーカー実行タイムアウトエラー
        /// </summary>
        public readonly static LogCodes ThreadPoolWorkerAliveTimeout = new LogCodes(LogLevel.ERROR, 1032);
        /// <summary>
        /// スレッドプールワーカー未処理例外
        /// </summary>
        public readonly static LogCodes ThreadPoolWorkerUnhandleException = new LogCodes(LogLevel.ERROR, 1033);
        /// <summary>
        /// DB接続エラー
        /// </summary>
        public readonly static LogCodes DBConnectError = new LogCodes(LogLevel.ERROR, 1100);
        /// <summary>
        /// DBエラー
        /// </summary>
        public readonly static LogCodes DBError = new LogCodes(LogLevel.ERROR, 1101);
        /// <summary>
        /// DBデッドロックエラー
        /// </summary>
        public readonly static LogCodes DBDeadLock = new LogCodes(LogLevel.WARN, 1102);
        /// <summary>
        /// DBデッドロックリトライオーバーエラー
        /// </summary>
        public readonly static LogCodes DBDeadLockRetryOver = new LogCodes(LogLevel.ERROR, 1103);
        /// <summary>
        /// DBタイムアウトエラー
        /// </summary>
        public readonly static LogCodes DBTimeout = new LogCodes(LogLevel.WARN, 1104);
        /// <summary>
        /// DBタイムアウトリトライオーバーエラー
        /// </summary>
        public readonly static LogCodes DBTimeoutRetryOver = new LogCodes(LogLevel.ERROR, 1105);
        /// <summary>
        /// MQオープンエラー
        /// </summary>
        public readonly static LogCodes MQOpenError = new LogCodes(LogLevel.ERROR, 1106);
        /// <summary>
        /// MQエラー
        /// </summary>
        public readonly static LogCodes MQError = new LogCodes(LogLevel.ERROR, 1107);

        #endregion
    }
}
