using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// 共通設定
    /// </summary>
    [Serializable]
    public  class CommonConfig
    {
        /// <summary>
        /// 設定ファイル情報
        /// </summary>
        protected readonly bool fConfElm = false;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="e"></param>
        public CommonConfig(CommonConfigElement e)
        {
            this.fConfElm = true;
            this.background = e.BackGround;
            this.queueingSize = e.QueueingSize;
            this.messagefile = e.messagefile;
            this.logLevelConfigurationPriority = e.LogLevelConfigurationPriority;
            this.logCodeOffset = e.LogCodeOffset;
            this.flushDelay = e.FlushDelay;
            this.notThrowExceptionInFormatError = e.NotThrowExceptionInFormatError;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonConfig()
        {
        }
        /// <summary>
        /// BackGroundモード
        /// </summary>
        public bool BackGround
        {
            get
            {
                return this.background;
            }
            set
            {
                this.background = value;
            }
        }
        /// <summary>
        /// BackGroundモード
        /// </summary>
        private bool background = true;
        /// <summary>
        /// BackGroundモード時のキューイング数
        /// </summary>
        public int QueueingSize
        {
            get
            {
                return this.queueingSize;
            }
            set
            {
                this.queueingSize = value;
            }
        }
        /// <summary>
        /// BackGroundモード時のキューイング数
        /// </summary>
        private int queueingSize = -1;
        /// <summary>
        /// メッセージファイル定義
        /// </summary>
        public string MessageFile
        {
            get
            {
                return this.messagefile;
            }
            set
            {
                this.messagefile = value;
            }
        }
        private string messagefile = string.Empty;
        /// <summary>
        /// ログレベルメッセージ設定優先モード
        /// </summary>
        public bool LogLevelConfigurationPriority
        {
            get
            {
                return this.logLevelConfigurationPriority;
            }
            set
            {
                this.logLevelConfigurationPriority = value;
            }
        }
        private bool logLevelConfigurationPriority = false;
        /// <summary>
        /// ログコードオフセット値
        /// </summary>
        public int LogCodeOffset
        {
            get
            {
                return this.logCodeOffset;
            }
            set
            {
                this.logCodeOffset = value;
            }
        }
        /// <summary>
        /// ログコードオフセット値
        /// </summary>
        private int logCodeOffset = 0;

        /// <summary>
        /// ログフラッシュディレイ時間(ミリ秒)
        /// </summary>
        public int FlushDelay
        {
            get
            {
                return this.flushDelay;
            }
            set
            {
                this.flushDelay = value;
            }
        }
        private int flushDelay = 1000;
        /// <summary>
        /// ログメッセージ書式化異常時例外をスローしない)
        /// </summary>
        public bool NotThrowExceptionInFormatError
        {
            get
            {
                return notThrowExceptionInFormatError;
            }
            set
            {
                notThrowExceptionInFormatError = value;
            }
        }
        private bool notThrowExceptionInFormatError = false;
    }
}
