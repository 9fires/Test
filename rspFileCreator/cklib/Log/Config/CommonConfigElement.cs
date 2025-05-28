using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using cklib;
using cklib.Util;

namespace cklib.Log.Config
{
    /// <summary>
    /// 共通設定
    /// </summary>
    public  class CommonConfigElement : ConfigurationElement
    {
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        CommonConfigElement()
        {
            this.Name = "Common";
        }
        /// <summary>
        /// エレメント名
        /// </summary>
        [ConfigurationProperty("name",
            DefaultValue = "",
            IsRequired = false,
            IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// BackGroundモード
        /// </summary>
        [ConfigurationProperty("background",
            DefaultValue = "true",
            IsRequired = false)]
        public bool BackGround
        {
            get
            {
                return (bool)this["background"];
            }
            set
            {
                this["background"] = value;
            }
        }
        /// <summary>
        /// BackGroundモード時のキューイング数
        /// </summary>
        [ConfigurationProperty("queueingsize",
            DefaultValue = "-1",
            IsRequired = false)]
        public int QueueingSize
        {
            get
            {
                return (int)this["queueingsize"];
            }
            set
            {
                this["queueingsize"] = value;
            }
        }
        /// <summary>
        /// メッセージファイル定義
        /// </summary>
        [ConfigurationProperty("messagefile",
            DefaultValue = "",
            IsRequired = false)]
        public string messagefile
        {
            get
            {
                return (string)this["messagefile"];
            }
            set
            {
                this["messagefile"] = value;
            }
        }
        /// <summary>
        /// ログレベルメッセージ設定優先モード
        /// </summary>
        [ConfigurationProperty("LogLevelConfigurationPriority",
            DefaultValue = "false",
            IsRequired = false)]
        public bool LogLevelConfigurationPriority
        {
            get
            {
                return (bool)this["LogLevelConfigurationPriority"];
            }
            set
            {
                this["LogLevelConfigurationPriority"] = value;
            }
        }
        /// <summary>
        /// ログコードオフセット値
        /// </summary>
        [ConfigurationProperty("LogCodeOffset",
            DefaultValue = "0",
            IsRequired = false)]
        public int LogCodeOffset
        {
            get
            {
                return (int)this["LogCodeOffset"];
            }
            set
            {
                this["LogCodeOffset"] = value;
            }
        }
        /// <summary>
        /// ログフラッシュディレイ時間(ミリ秒)
        /// </summary>
        [ConfigurationProperty("FlushDelay",
            DefaultValue = "1000",
            IsRequired = false)]
        public int FlushDelay
        {
            get
            {
                return (int)this["FlushDelay"];
            }
            set
            {
                this["FlushDelay"] = value;
            }
        }
        /// <summary>
        /// ログメッセージ書式化異常時例外をスローしない)
        /// </summary>
        [ConfigurationProperty("NotThrowExceptionInFormatError",
            DefaultValue = "false",
            IsRequired = false)]
        public bool NotThrowExceptionInFormatError
        {
            get
            {
                return (bool)this["NotThrowExceptionInFormatError"];
            }
            set
            {
                this["NotThrowExceptionInFormatError"] = value;
            }
        }
    }
}
