using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// 個別設定項目共通設定項目情報
    /// </summary>
    [Serializable]
    public class BasicLogConfig
    {
        /// <summary>
        /// 設定ファイル情報
        /// </summary>
        //  public readonly LoggerConfigElement elm =   null;
        protected readonly bool fConfElm=false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Name">エレメント名を指定</param>
        /// <param name="e"></param>
        public BasicLogConfig(string Name,LoggerConfigElement e)
        {
            this.fConfElm = true;
            this.Name = Name;
            this.enabled = e.Enabled;
            this.level = LogLevelString(e.Level);
            this.format = e.Format;
            this.scramble = e.scramble;
            this.scrambleKey = e.scrambleKey;
            this.encoding = System.Text.Encoding.GetEncoding(e.Encoding);
            this.MessageMaxLength = e.MessageMaxLength; // 2016/08/23 イベントログ長さチェック追加
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Name">エレメント名を指定</param>
        public BasicLogConfig(string Name)
        {
            this.Name = Name;
        }
        /// <summary>
        /// エレメント名
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// 有効・無効設定
        /// </summary>
        public bool Enabled
        {
           get
            {
                lock (this)
                {
                    return (bool)enabled;
                }
            }
            set
            {
                lock (this)
                {
                    this.enabled= value;
                }
            }
        }
        /// <summary>
        /// 有効・無効設定
        /// </summary>
        private bool enabled    = false;
        /// <summary>
        /// ログレベル
        /// </summary>
        public LogLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level  = value;
            }
        }
        /// <summary>
        /// ログレベル
        /// </summary>
        private LogLevel level = LogLevel.DEBUG;
        /// <summary>
        /// 文字列からログレベルに変換する
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        static public  LogLevel LogLevelString(string level)
        {
            switch (level.ToUpper())
            {
                case "TRACE": return LogLevel.TRACE;
                case "DEBUG": return LogLevel.DEBUG;
                case "INFO": return LogLevel.INFO;
                case "NOTE": return LogLevel.NOTE;
                case "WARN": return LogLevel.WARN;
                case "ERROR": return LogLevel.ERROR;
                case "CRITICAL": return LogLevel.CRIT;
                case "ALERT": return LogLevel.ALERT;
                case "EMERGENCY": return LogLevel.EMERG;
                case "FATAL": return LogLevel.FATAL;
                default:
                    return LogLevel.Undefine;
            }
        }
        /// <summary>
        /// 書式
        /// </summary>
        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format =   value;
            }
        }
        /// <summary>
        /// 書式
        /// </summary>
        private string format = string.Empty;
        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }
        /// <summary>
        /// 文字エンコーディング
        /// </summary>
        private System.Text.Encoding encoding = System.Text.Encoding.Default;
        /// <summary>
        /// スクランブルの有無
        /// </summary>
        public bool Scramble
        {
            get
            {
                return this.scramble;
            }
            set
            {
                this.scramble = value;
            }
        }
        /// <summary>
        /// スクランブルの有無
        /// </summary>
        private bool scramble = false;
        /// <summary>
        /// スクランブルのキー
        /// </summary>
        public string ScrambleKey
        {
            get { return this.scrambleKey; }
            set { this.scrambleKey = value;}
        }
        /// <summary>
        /// スクランブルのキー
        /// </summary>
        private string scrambleKey = string.Empty;
        /// <summary>
        /// メッセージ上限文字数
        /// </summary>
        /// <remarks>
        /// 2016/08/23 イベントログ長さチェック追加
        /// </remarks>
        public virtual int MessageMaxLength
        {
            get
            {
                return this.messageMaxLength;
            }
            set
            {
                this.messageMaxLength = value;
            }
        }
        int messageMaxLength = -1;
    }
}
