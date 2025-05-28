using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Text;

namespace cklib.Util.Config
{
    /// <summary>
    /// KeyValuePair設定コレクション
    /// </summary>
    public class KeyValuePairElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public KeyValuePairElementCollection()
        {
            this.AddElementName = "Item";
        }
        /// <summary>
        /// 項目抽出
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public KeyValuePairElement Get(int index)
        {
            return (KeyValuePairElement)base.BaseGet(index);
        }
        /// <summary>
        /// ConfigurationElementCollection 内の ConfigurationElement をキーから取得
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public KeyValuePairElement Get(string name)
        {
            return BaseGet(name) as KeyValuePairElement;
        }
        /// <summary>
        /// 配列アクセス
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                if (index>=this.Count)
                {   //  上限超え
                    //  デフォルト設定を使用する
                    return this.Get("Default").Value;
                }
                return this.Get(index).Value;
            }
        }
        /// <summary>
        /// 配列アクセス
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new string this[string key]
        {
            get
            {
                if (!this.Contains(key))
                {
                    //  デフォルト設定を使用する
                    return this.Get("Default").Value;                                        
                }
                return this.Get(key).Value;
            }
        }
        /// <summary>
        /// ConfigurationElementCollection 内に指定したキーの ConfigurationElement があるか
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            return BaseGet(name) != null;
        }
        /// <summary>
        /// 新しい ConfigurationElement を作成
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValuePairElement();
        }
        /// <summary>
        /// 指定した構成要素の要素キーを取得
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            KeyValuePairElement childElement = element as KeyValuePairElement;
            return childElement.Key;
        }

    }
}
