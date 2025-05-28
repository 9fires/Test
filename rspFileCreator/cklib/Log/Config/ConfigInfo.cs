using System;
using System.Collections.Generic;
using System.Text;
using cklib.Log.ExceptionFormters;
using System.Configuration;
namespace cklib.Log.Config
{
    /// <summary>
    /// ログ設定情報クラス
    /// </summary>
    [Serializable]
    public class ConfigInfo
    {
        /// <summary>
        /// デフォルトのコンフィグレーションセクション名
        /// </summary>
        public const string DefaultConfigSectionName    =   "SystemLog";
        /// <summary>
        /// ログ設定セクション名
        /// </summary>
        public readonly string SectionName;
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="conf">構成ファイル指定</param>
        /// <param name="sectionname">セクション名</param>
        public ConfigInfo(Configuration conf, string sectionname)
        {
            LoggerConfig lc;
            this.SectionName = sectionname;
            lc = (LoggerConfig)conf.GetSection(sectionname);
            IntializeLoggerConfig(lc);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sectionname">セクション名</param>
        public ConfigInfo(string sectionname)
        {
            LoggerConfig lc;
            this.SectionName = sectionname;
            lc = (LoggerConfig)System.Configuration.ConfigurationManager.GetSection(sectionname);
            IntializeLoggerConfig(lc);
        }
        /// <summary>
        /// 設定ファイルで初期化する
        /// </summary>
        /// <param name="lc"></param>
        private void IntializeLoggerConfig(LoggerConfig lc)
        {
            this.m_Common = new CommonConfig(lc.Common);
            this.m_Default = new BasicLogConfig(LoggerConfig.DefaultElementCollectionName,lc.Default);
            this.m_Console = new BasicLogConfig(LoggerConfig.ConsoleElementCollectionName,lc.Console);
            this.m_Extend = new BasicLogConfig(LoggerConfig.ExtendElementCollectionName, lc.Extend);
            this.m_File = new FileLogConfig(lc.File);
            this.m_EventLog = new EventLogConfig(lc.EventLog);
            this.m_Syslog = new SysLogConfig(lc.Syslog);
            if (lc.Messages.Count != 0)
                this.m_Message = new MessagesConfig(lc.Messages);
            else
                if (this.m_Common.MessageFile.Length != 0)
                    this.m_Message = new MessagesConfig(this.m_Common.MessageFile);
                else
                    this.m_Message = new MessagesConfig();
            SetupDefaultExceptionFormaterList();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigInfo()
        {
            this.SectionName = DefaultConfigSectionName;
            this.m_Common = new CommonConfig();
            this.m_Default = new BasicLogConfig(LoggerConfig.DefaultElementCollectionName);
            this.m_Console = new BasicLogConfig(LoggerConfig.ConsoleElementCollectionName);
            this.m_Extend = new BasicLogConfig(LoggerConfig.ExtendElementCollectionName);
            this.m_File = new FileLogConfig();
            this.m_EventLog = new EventLogConfig();
            this.m_Syslog = new SysLogConfig();
            this.m_Message = new MessagesConfig();
            SetupDefaultExceptionFormaterList();
        }
        #endregion
        #region 設定インスタンス
        /// <summary>
        /// 共通ログ設定
        /// </summary>
        private CommonConfig m_Common;
        /// <summary>
        /// 共通ログ設定
        /// </summary>
        public CommonConfig Common
        {
            get { return this.m_Common; }
        }
        /// <summary>
        /// コンソールログ設定
        /// </summary>
        private BasicLogConfig m_Console;
        /// <summary>
        /// コンソールログ設定
        /// </summary>
        public BasicLogConfig Console
        {
            get { return this.m_Console; }
        }
        /// <summary>
        /// 拡張ログ設定
        /// </summary>
        private BasicLogConfig m_Extend;
        /// <summary>
        /// 拡張ログ設定
        /// </summary>
        public BasicLogConfig Extend
        {
            get { return this.m_Extend; }
        }
        /// <summary>
        /// ファイルログ設定
        /// </summary>
        private FileLogConfig m_File;
        /// <summary>
        /// ファイルログ設定
        /// </summary>
        public FileLogConfig File
        {
            get { return this.m_File; }
        }
        /// <summary>
        /// イベントログ設定
        /// </summary>
        private EventLogConfig m_EventLog;
        /// <summary>
        /// イベントログ設定
        /// </summary>
        public EventLogConfig EventLog
        {
            get { return this.m_EventLog; }
        }
        /// <summary>
        /// syslog設定
        /// </summary>
        private SysLogConfig m_Syslog;
        /// <summary>
        /// syslog設定
        /// </summary>
        public SysLogConfig Syslog
        {
            get { return this.m_Syslog; }
        }
        /// <summary>
        /// デフォルトログ設定
        /// </summary>
        /// <remarks>
        /// 書式指定のみ有効
        /// </remarks>
        private BasicLogConfig m_Default;
        /// <summary>
        /// デフォルトログ設定
        /// </summary>
        /// <remarks>
        /// 書式指定のみ有効
        /// </remarks>
        public BasicLogConfig Default
        {
            get { return this.m_Default; }
        }
        /// <summary>
        /// ログメッセージ定義
        /// </summary>
        public MessagesConfig Message
        {
            get { return this.m_Message; }
        }
        /// <summary>
        /// ログメッセージ定義変更
        /// </summary>
        public void SetMessageConfig(MessagesConfig Message)
        {
            this.m_Message = Message;
        }
        /// <summary>
        /// ログメッセージ定義
        /// </summary>
        private MessagesConfig m_Message = null;
        #endregion
        #region ユーティリティ
        /// <summary>
        /// 記録対象のログレベルのチェック
        /// </summary>
        /// <param name="level">レベル</param>
        /// <returns>記録対象ならtrue</returns>
        public bool IsValidLevel(LogLevel level)
        {
            if (m_Console.Enabled)
            {
                if (m_Console.Level <= level)
                    return true;
            }
            if (m_File.Enabled)
            {
                if (m_File.Level <= level)
                    return true;
            }
            if (m_EventLog.Enabled)
            {
                if (m_EventLog.Level <= level)
                    return true;
            }
            if (m_Extend.Enabled)
            {
                if (m_Extend.Level <= level)
                    return true;
            }
            if (m_Syslog.Enabled)
            {
                if (m_Syslog.Level <= level)
                    return true;
            }
            return false;
        }
        #endregion
        #region 拡張モジュール
        /// <summary>
        /// ログ拡張モジュールクラスインスタンス
        /// </summary>
        /// <remarks>
        /// ExtendLogクラスの派生クラスを定義し設定する
        /// </remarks>
        public ExtendLog ExtendLogInstance = null;
        /// <summary>
        /// 書式化モジュール
        /// </summary>
        /// <remarks>
        /// ログをフォーマットする書式化モジュール、標準機能以外の書式化が必要な場合はFormatterクラスを定義し設定する
        /// </remarks>
        public Formatter FormatterInstance = new Formatter();
        /// <summary>
        /// 暗号化モジュール
        /// </summary>
        /// <remarks>
        /// DES以外の暗号化方式を使用する場合は、Scramblerクラスを定義し設定する
        /// </remarks>
        public Scrambler ScramblerInstance = new Scrambler();
        /// <summary>
        /// ログファイルローテータモジュール
        /// </summary>
        /// <remarks>
        /// 基本のローテート方式以外を使用する場合FileRotaterクラスを定義し設定する
        /// </remarks>
        public FileRotater FileRotaterInstance = new FileRotater();
        /// <summary>
        /// 例外書式化モジュールの一覧
        /// </summary>
        internal Dictionary<Type, ExceptionFormater> ExceptionFormaterList = new Dictionary<Type, ExceptionFormater>();
        /// <summary>
        /// 例外書式化モジュールの追加
        /// </summary>
        /// <param name="ef"></param>
        public void SetExceptionFormater(ExceptionFormater ef)
        {
            if (ExceptionFormaterList.ContainsKey(ef.ExceptionType))
            {   //  既に登録されているので置き換える
                ExceptionFormaterList[ef.ExceptionType] = ef;
            }
            else
            {
                ExceptionFormaterList.Add(ef.ExceptionType, ef);
            }
        }
        /// <summary>
        /// デフォルト例外書式化モジュールのセットアップ
        /// </summary>
        private void SetupDefaultExceptionFormaterList()
        {
            //  ソケット例外
            this.SetExceptionFormater(new SocketExceptionFormater());
            //  SQL例外
            this.SetExceptionFormater(new SqlExceptionFormater());
            //  IO例外
            this.SetExceptionFormater(new IOExceptionFormater());
        }
        #endregion
    }
}
