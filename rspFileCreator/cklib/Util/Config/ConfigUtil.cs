using System;
using System.Collections;
using System.Configuration;
using cklib;
using cklib.Util;

namespace cklib.Util
{
	/// <summary>
	/// 設定情報の取得
	/// </summary>
	public static class ConfigUtil
	{
		/// <summary>
		/// 設定値取得エラーイベントハンドラ定義
		/// </summary>
		public delegate void ConfigErrorEventHandler(object sender, ConfigErrorEventArgs e);
		/// <summary>
		/// 設定値取得エラーイベントハンドラ定義
		/// </summary>
		public	static	event	ConfigErrorEventHandler	ConfigErrorEvent;
		/// <summary>
		/// 設定情報の取得
		/// </summary>
		/// <param name="name">キー名</param>
		/// <param name="defaultvalue">未設定時のデフォルト値</param>
		/// <returns>取得されたオブジェクト</returns>
		public	static	object GetConfigObject(string name,object defaultvalue)
		{
			object	val;
			try
			{
                val = ConfigurationManager.AppSettings[name];
				if	(val==null)
					throw	new	Exception("設定なし");
			}
			catch	(Exception	exp)
			{
				if	(ConfigUtil.ConfigErrorEvent!=null)
				{
					if	(defaultvalue!=null)
						ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,defaultvalue.ToString(),exp));
					else
						ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,string.Empty,exp));
				}
				val	=	defaultvalue;
			}
			return	val;
		}
		/// <summary>
		/// 設定文字列の取得
		/// </summary>
		/// <param name="name">キー名</param>
		/// <param name="defaultvalue">未設定時のデフォルト値</param>
		/// <returns>取得された文字列</returns>
		public	static	string	GetConfigString(string name,string defaultvalue)
		{
			return	(string)GetConfigObject(name,defaultvalue);
		}
		/// <summary>
		/// 設定数値の取得
		/// </summary>
		/// <param name="name">キー名</param>
		/// <param name="defaultvalue">未設定時のデフォルト値</param>
		/// <returns>取得された数値</returns>
		public	static	int	GetConfigInt(string name,int defaultvalue)
		{
			try
			{
				return	int.Parse((string)GetConfigObject(name,defaultvalue));
			}
			catch	(Exception	exp)
			{
				if	(ConfigUtil.ConfigErrorEvent!=null)
				{
					ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,defaultvalue.ToString(),exp));
				}
				return	defaultvalue;
			}
        }

        #region DotNet1.1互換用クラス
        #pragma warning disable  618
        /// <summary>
        /// 外部設定情報クラス
        /// ※DotNet1.1互換用クラス
        /// </summary>
        public class ExtendConfig
        {
            /// <summary>
            /// セクション名
            /// </summary>
            protected string section;
            /// <summary>
            /// セクション情報取得データ
            /// </summary>
            protected IDictionary dic;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="sectionname">セクション名</param>
            public ExtendConfig(string sectionname)
            {
                section = sectionname;
                dic = (IDictionary)ConfigurationSettings.GetConfig(sectionname);
            }
            /// <summary>
            /// 設定情報の取得
            /// </summary>
            /// <param name="name">キー名</param>
            /// <param name="defaultvalue">未設定時のデフォルト値</param>
            /// <returns>取得されたオブジェクト</returns>
            public object GetConfigObject(string name, object defaultvalue)
            {
                object val;
                try
                {
                    val = dic[name];
                    if (val == null)
                        throw new Exception("設定なし");
                }
                catch (Exception exp)
                {
                    if (ConfigUtil.ConfigErrorEvent != null)
                    {
                        if (defaultvalue != null)
                            ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, defaultvalue.ToString(), exp));
                        else
                            ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, string.Empty, exp));
                    }
                    val = defaultvalue;
                }
                return val;
            }
            /// <summary>
            /// 設定文字列の取得
            /// </summary>
            /// <param name="name">キー名</param>
            /// <param name="defaultvalue">未設定時のデフォルト値</param>
            /// <returns>取得された文字列</returns>
            public string GetConfigString(string name, string defaultvalue)
            {
                return (string)GetConfigObject(name, defaultvalue);
            }
            /// <summary>
            /// 設定数値の取得
            /// </summary>
            /// <param name="name">キー名</param>
            /// <param name="defaultvalue">未設定時のデフォルト値</param>
            /// <returns>取得された数値</returns>
            public int GetConfigInt(string name, int defaultvalue)
            {
                try
                {
                    return int.Parse((string)GetConfigObject(name, defaultvalue));
                }
                catch (Exception exp)
                {
                    if (ConfigUtil.ConfigErrorEvent != null)
                    {
                        ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, defaultvalue.ToString(), exp));
                    }
                    return defaultvalue;
                }
            }
        }
        /// <summary>
        /// Default付一覧形式の拡張設定情報参照クラス
        /// </summary>
        public class ExtendListTypeConfig : ExtendConfig
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="sectionname">セクション名</param>
            public ExtendListTypeConfig(string sectionname)
                : base(sectionname)
            { }

            /// <summary>
            /// バージョンからURLを取得する
            /// 指定されたバージョンが存在しない場合は、"Default"設定をロードする
            /// </summary>
            public string this[string key]
            {
                get
                {
                    string ret = (string)dic[key];
                    if (ret == null)
                    {
                        ret = (string)dic["Default"];
                    }
                    return ret;
                }
            }
        }

        /// <summary>
        /// 列挙定義参照クラス
        /// </summary>
        public class EnumrateConfig : ExtendConfig, IEnumerable
        {
            /// <summary>
            /// 定義データの配列
            /// </summary>
            private ArrayList msgs = new ArrayList();
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="sectionname">セクション名</param>
            public EnumrateConfig(string sectionname)
                : base(sectionname)
            {
                ArrayList keys = new ArrayList();
                int i;
                int key;
                foreach (System.Collections.DictionaryEntry msg in dic)
                {
                    key = String.ToInt(msg.Key.ToString(), 0);
                    for (i = 0; i < keys.Count; i++)
                    {
                        if (key < (int)keys[i])
                            break;
                    }
                    keys.Insert(i, key);
                    msgs.Insert(i, msg.Value);
                }
            }
            /// <summary>
            /// 設定の配列アクセス
            /// </summary>
            public string this[int idx]
            {
                get
                {
                    return msgs[idx].ToString();
                }
            }
            /// <summary>
            /// 定義数の取得
            /// </summary>
            /// <returns></returns>
            public int Count
            {
                get { return msgs.Count; }
            }
            /// <summary>
            /// IEnumerable実装メンバ
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator()
            {
                return new TokenEnumerator(this);
            }
            /// <summary>
            /// IEnumerator実装クラス
            /// </summary>
            private class TokenEnumerator : IEnumerator
            {
                private int position = -1;
                private EnumrateConfig t;

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="t"></param>
                public TokenEnumerator(EnumrateConfig t)
                {
                    this.t = t;
                }
                /// <summary>
                /// ポジション移動
                /// </summary>
                /// <returns></returns>
                public bool MoveNext()
                {
                    if (position < t.Count - 1)
                    {
                        position++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                /// <summary>
                /// ポジションリセット
                /// </summary>
                public void Reset()
                {
                    position = -1;
                }
                /// <summary>
                /// 現在のポジションのオブジェクトの取得
                /// </summary>
                public object Current
                {
                    get
                    {
                        return t[position];
                    }
                }
            }
        }
        #endregion DotNet1.1互換用クラス
    }
}
