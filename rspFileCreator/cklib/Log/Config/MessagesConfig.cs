using System;
using System.Collections.Generic;
using System.Text;
using cklib.Log;
using System.Xml;
using System.Reflection;
namespace cklib.Log.Config
{
    /// <summary>
    /// ログメッセージ読み出しユーティリティクラス
    /// </summary>
    public class MessagesConfig
    {
        /// <summary>
        /// ログメッセージ情報
        /// </summary>
        protected class MessageInfo
        {
            /// <summary>
            /// ログコード名(namespace.Class.フィール名)
            /// </summary>
            public string Name = "";
            /// <summary>
            /// ログコード
            /// </summary>
            public int Code = 0;
            /// <summary>
            /// ログレベル
            /// </summary>
            public LogLevel Level = LogLevel.Undefine;
            /// <summary>
            /// 書式定義
            /// </summary>
            public string Format = "";
        }
        /// <summary>
        /// メッセージ一覧
        /// </summary>
        protected System.Collections.Generic.Dictionary<int, MessageInfo> MessageList = new Dictionary<int, MessageInfo>();
        /// <summary>
        /// ログコード名マップ
        /// </summary>
        protected System.Collections.Generic.Dictionary<string, MessageInfo> NameMessageMapList = new Dictionary<string, MessageInfo>();
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public MessagesConfig()
        {}
        /// <summary>
        /// ログ設定からメッセージ取得
        /// </summary>
        /// <param name="conf"></param>
        public MessagesConfig(MessageElementCollection conf)
        {
            if (conf!=null)
            {
                foreach (MessageElement elm in conf)
                {
                    MessageInfo info = new MessageInfo();
                    info.Code = elm.Code;
                    info.Level = elm.Level;
                    info.Name = elm.Name;
                    info.Format = elm.Format;
                    if (!MessageList.ContainsKey(elm.Code))
                        MessageList.Add(elm.Code, info);
                    if (elm.Name.Length != 0)
                    {
                        if (!NameMessageMapList.ContainsKey(elm.Name))
                        {
                            this.NameMessageMapList.Add(elm.Name, info);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// メッセージファイル指定
        /// </summary>
        /// <param name="MessageFile">メッセージファイルパス</param>
        public MessagesConfig(string MessageFile)
        {
            this.SetMessageFile(MessageFile);
        }
        /// <summary>
        /// メッセージファイル指定
        /// </summary>
        /// <param name="MessageFile">メッセージファイルパス</param>
        public void SetMessageFile(string MessageFile)
        {
            XmlDocument xml = new XmlDocument();
            if (MessageFile.StartsWith("."))
            {   //  相対パス
                Assembly asm = Assembly.GetExecutingAssembly();
                MessageFile = System.IO.Path.GetDirectoryName(asm.Location) + "\\" + MessageFile;
            }
            xml.Load(MessageFile);
            foreach (XmlNode node in xml.DocumentElement)
            {
                if (!node.Name.Equals("Message"))
                    continue;
                MessageInfo info = new MessageInfo();
                info.Level = LogLevel.Undefine;
                var level_attr = node.Attributes["level"];
                if (level_attr != null)
                {
                    info.Level = (LogLevel)Enum.Parse(typeof(LogLevel), level_attr.InnerText, true);
                }
                info.Code = cklib.Util.String.ToInt(node.Attributes["code"].InnerText);
                info.Format = node.Attributes["format"].InnerText;
                if (!MessageList.ContainsKey(info.Code))
                    MessageList.Add(info.Code, info);
                var name_attr = node.Attributes["name"];
                if (name_attr != null)
                {
                    info.Name = name_attr.InnerText;
                    if (!this.NameMessageMapList.ContainsKey(info.Name))
                    {
                        this.NameMessageMapList.Add(info.Name, info);
                    }
                }
            }
        }
        /// <summary>
        /// ログコード名からログコードを取得する
        /// </summary>
        /// <param name="LogCode"></param>
        /// <returns></returns>
        public virtual int GetLogCode(cklib.Log.LogCodes LogCode)
        {
            if (NameMessageMapList.ContainsKey(LogCode.Name))
            {
                return NameMessageMapList[LogCode.Name].Code;
            }
            return LogCode.LogCode;
        }
        /// <summary>
        /// ログコード名からログレベルを取得する
        /// </summary>
        /// <param name="LogCode"></param>
        /// <returns></returns>
        public virtual LogLevel GetLogLevel(cklib.Log.LogCodes LogCode)
        {
            if (NameMessageMapList.ContainsKey(LogCode.Name))
            {
                return NameMessageMapList[LogCode.Name].Level;
            }
            return LogCode.LogLevel;
        }
        /// <summary>
        /// ログコードからログレベルを取得する
        /// </summary>
        /// <param name="LogCode">ログコード値</param>
        /// <returns></returns>
        public virtual LogLevel GetLogLevel(int LogCode)
        {
            if (MessageList.ContainsKey(LogCode))
            {
                return MessageList[LogCode].Level;
            }
            return LogLevel.Undefine;
        }
        /// <summary>
        /// ログコード参照
        /// </summary>
        /// <param name="LogCode">ログコード値</param>
        /// <returns></returns>
        public virtual string this[int LogCode]
        {
            get
            {
                if (MessageList.ContainsKey(LogCode))
                {
                    return MessageList[LogCode].Format;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// ログコード名からメッセージを参照
        /// </summary>
        /// <param name="LogCode"></param>
        /// <returns></returns>
        public virtual string this[cklib.Log.LogCodes LogCode]
        {
            get
            {
                if (this.NameMessageMapList.ContainsKey(LogCode.Name))
                {
                    return this.NameMessageMapList[LogCode.Name].Format;
                }
                else
                    return this[LogCode.LogCode];
            }
        }
    }
}
