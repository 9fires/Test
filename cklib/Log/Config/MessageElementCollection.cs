using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// ログメッセージエレメントクラス
    /// </summary>
    public class MessageElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MessageElementCollection()
        {
            this.AddElementName = "Message";
        }
        /// <summary>
        /// MessageElementCollection 内の MessageElement をインデックスから取得
        /// </summary>
        /// <param name="index">先頭からのインデックス</param>
        /// <returns>MessageElement</returns>
        public MessageElement Get(int index)
        {
            return (MessageElement)base.BaseGet(index);
        }
 
        /// <summary>
        /// MessageElementCollection 内の MessageElement をキーから取得
        /// </summary>
        /// <param name="name">キー</param>
        /// <returns>MessageElement</returns>
        public MessageElement　Get(string name)
        {
            return BaseGet(name) as MessageElement;
        }
        /// <summary>
        /// MessageElementCollection 内に指定したキーの MessageElement があるか
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
        /// <returns>作成されたConfigurationElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MessageElement();
        }

        /// <summary>
        /// 指定した構成要素の要素キーを取得
        /// </summary>
        /// <param name="element">ConfigurationElement</param>
        /// <returns>キー</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            MessageElement childElement = element as MessageElement;
            return childElement.Name.Length == 0 ? childElement.Code.ToString() : childElement.Name;
        }

    }
}
