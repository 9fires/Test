using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Text;

namespace cklib.Util.Config
{
    /// <summary>
    /// KeyValuePair定義コレクション
    /// </summary>
    public class KeyValuePairElement : ConfigurationElement 
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyValuePairElement()
        {}
        /// <summary>
        /// ログコード指定
        /// </summary>
        [ConfigurationProperty("Key",IsRequired = true)]
        public string Key
        {
            get
            {
                return (string)this["Key"];
            }
            set
            {
                this["Key"] = value;
            }
        }
        /// <summary>
        /// メッセージフォーマット
        /// </summary>
        [ConfigurationProperty("Value", IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["Value"];
            }
            set
            {
                this["Value"] = value;
            }
        }

    }
}
