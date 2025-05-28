using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace cklib.Data
{
    /// <summary>
    /// データ
    /// </summary>
    public class DataConfigSection<TDataConfigElement> : ConfigurationSection
        where TDataConfigElement:DataConfigElement
    {
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public DataConfigSection()
        { }

        /// <summary>
        /// DataConfigエレメント名
        /// </summary>
        public const string DataConfigElementName = "Setting";
        /// <summary>
        /// DataConfigエレメント
        /// </summary>
        [ConfigurationProperty(DataConfigElementName)]
        public TDataConfigElement Common
        {
            get
            {
                TDataConfigElement elm =
                (TDataConfigElement)base[DataConfigElementName];
                return elm;
            }
        }
    }
}
