using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace cklib.Util.Config
{
    /// <summary>
    /// プロファイル操作
    /// </summary>
    public class Profile
    {
        #region API
        /// <summary>
        /// 指定したプライベートプロファイルから設定をリードする
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="lpDefault">デフォルト値</param>
        /// <param name="lpReturnedString">情報が格納されるバッファ</param>
        /// <param name="nSize">情報バッファのサイズ</param>
        /// <param name="lpFileName"> .ini ファイルの名前</param>
        /// <returns>バッファに格納された文字数</returns>
        [DllImport("KERNEL32.DLL",EntryPoint = "GetPrivateProfileStringA")]
        protected static extern uint GetPrivateProfileString(   string lpAppName,
                                                                string lpKeyName,
                                                                string lpDefault,
                                                                byte[] lpReturnedString,
                                                                uint nSize,
                                                                string lpFileName);
        /// <summary>
        /// 指定したプライベートプロファイルへ書き込む
        /// </summary>
        /// <param name="lpAppName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="lpString"></param>
        /// <param name="lpFileName"> .ini ファイルの名前</param>
        /// <returns>成功時true</returns>
        [DllImport("KERNEL32.DLL", EntryPoint="WritePrivateProfileStringA")]
        protected static extern bool WritePrivateProfileString( string lpAppName,
                                                                string lpKeyName,
                                                                string lpString,
                                                                string lpFileName);
        #endregion
        /// <summary>
        /// プロファイル名
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">プロファイル名</param>
        public Profile(string path)
        {
            this.Name = path;
        }
        /// <summary>
        /// 文字列の取得
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="key">キー</param>
        /// <param name="defaultvalue">デフォルト値</param>
        /// <returns>取得文字列</returns>
        public string GetString(string section, string key, string defaultvalue)
        {
            int bsize=4096;
            byte[] buff= new byte[bsize];
            uint r;
            for (; ; )
            {
                r = GetPrivateProfileString(section, key, defaultvalue, buff, (uint)buff.Length, this.Name);
                if (r==(uint)buff.Length-1)
                {   //  バッファサイズ不足
                    buff = new byte[buff.Length + bsize];
                    continue;
                }
                break;
            }
            string str = System.Text.Encoding.GetEncoding("Shift_JIS").GetString(buff, 0, (int)r);
            return str;
        }
        /// <summary>
        /// 設定の書き込み
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <param name="key">キー</param>
        /// <param name="value">設定値</param>
        public void SetString(string section, string key, string value)
        {
            if (!WritePrivateProfileString(section,key,value,this.Name))
            {
                throw new Win32ErrorException(Errors.GetLastError());
            }
        }
        /// <summary>
        /// インデクサ
        /// </summary>
        /// <param name="section">セクション名</param>
        /// <returns>セクションインスタンス</returns>
        public virtual Section this[string section]
        {
            get
            {
                return new Section(this,section);
            }
        }

        /// <summary>
        /// セクションへのアクセス
        /// </summary>
        public class Section
        {
            /// <summary>
            /// プロファイルインスタンス
            /// </summary>
            public readonly Profile profile;
            /// <summary>
            /// セクション名
            /// </summary>
            public readonly string Name;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="profile">プロファイル</param>
            /// <param name="SectionName"></param>
            public Section(Profile profile,string SectionName)
            {
                this.profile = profile;
                this.Name = SectionName;
            }
            /// <summary>
            /// 文字列の取得
            /// </summary>
            /// <param name="key">キー</param>
            /// <param name="defaultvalue">デフォルト値</param>
            /// <returns>取得文字列</returns>
            public virtual string GetString(string key, string defaultvalue)
            {
                return this.profile.GetString(this.Name, key, defaultvalue);
            }
            /// <summary>
            /// 設定の書き込み
            /// </summary>
            /// <param name="key">キー</param>
            /// <param name="value">設定値</param>
            public virtual void SetString(string key, string value)
            {
                this.profile.SetString(this.Name, key, value);
            }
            /// <summary>
            /// 数値の取得
            /// </summary>
            /// <param name="key">キー</param>
            /// <param name="defaultvalue">デフォルト値</param>
            /// <returns>取得数値</returns>
            public virtual int GetInt(string key, int defaultvalue)
            {
                return cklib.Util.String.ToInt(this.profile.GetString(this.Name, key, defaultvalue.ToString()),defaultvalue);
            }
            /// <summary>
            /// 数値設定の書き込み
            /// </summary>
            /// <param name="key">キー</param>
            /// <param name="value">設定値</param>
            public virtual void SetInt(string key, int value)
            {
                this.profile.SetString(this.Name, key, value.ToString());
            }
            /// <summary>
            /// インデクサ
            /// </summary>
            /// <param name="key">キー</param>
            /// <returns>設定値</returns>
            public virtual string this[string key]
            {
                get
                {
                    return this.GetString(key, string.Empty);
                }
                set
                {
                    this.SetString(key, value);
                }
            }
        }
    }
}
