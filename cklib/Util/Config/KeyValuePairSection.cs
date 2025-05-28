using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Configuration;

namespace cklib.Util.Config
{
    /// <summary>
    /// KeyValuePair設定Section
    /// </summary>
    public class KeyValuePairSection : ConfigurationSection
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyValuePairSection()
        {}
        /// <summary>
        /// KeyValuePair設定Sectionエレメント名
        /// </summary>
        public const string ElementCollectionName = "KeyValuePair";
        /// <summary>
        /// KeyValuePair設定Section情報エレメント
        /// </summary>
        [ConfigurationProperty(ElementCollectionName)]
        public KeyValuePairElementCollection KeyValuePairElements
        {
            get
            {
                KeyValuePairElementCollection elm =
                (KeyValuePairElementCollection)base[ElementCollectionName];
                return elm;
            }
        }
    }
}
