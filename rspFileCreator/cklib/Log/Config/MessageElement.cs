using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// メッセージ定義コレクション
    /// </summary>
    public class MessageElement : ConfigurationElement 
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MessageElement()
        {}
        /// <summary>
        /// ログコード名指定
        /// </summary>
        [ConfigurationProperty("name", IsRequired = false, DefaultValue = "")]
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
        /// ログレベル指定
        /// </summary>
        [ConfigurationProperty("level", IsRequired = false, DefaultValue = LogLevel.Undefine)]
        public LogLevel Level
        {
            get
            {
                return (LogLevel)this["level"];
            }
            set
            {
                this["level"] = value;
            }
        }
        /// <summary>
        /// ログコード指定
        /// </summary>
        [ConfigurationProperty("code",IsRequired = true)]
        public int Code
        {
            get
            {
                return (int)this["code"];
            }
            set
            {
                this["code"] = value;
            }
        }
        /// <summary>
        /// メッセージフォーマット
        /// </summary>
        [ConfigurationProperty("format", IsRequired = true)]
        public string Format
        {
            get
            {
                return (string)this["format"];
            }
            set
            {
                this["format"] = value;
            }
        }

    }
}
