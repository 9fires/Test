using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using cklib;
using cklib.Framework;
using cklib.Framework.IPC;
using cklib.Log;
using cklib.Log.Config;
using System.Reflection;

namespace cklib.Log
{
    /// <summary>
    /// ログ内容の暗号化モジュール
    /// </summary>
    [Serializable]
    public class Scrambler
    {
        /// <summary>
        /// 暗号処理（
        /// </summary>
        /// <remarks>
        /// デフォルトではDES暗号が適応される(ScrambleKeyへ要設定)<br/>
        /// 他のスクランブルを使用する場合はオーバーロードする
        /// </remarks>
        /// <param name="msg">暗号元メッセージ</param>
        /// <param name="Config">設定情報</param>
        /// <param name="engine">ログエンジンインスタンス</param>
        /// <returns>暗号化メッセージ(文字列エンコードする）</returns>
        public virtual string EncryptToString(byte[] msg, BasicLogConfig Config, LogingEngine engine)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] key = new byte[des.KeySize / 8];
                byte[] keysrc = System.Text.Encoding.ASCII.GetBytes(Config.ScrambleKey);
                Array.Copy(keysrc, key, (keysrc.Length < des.KeySize ? keysrc.Length : des.KeySize));
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                using (MemoryStream deststrm = new MemoryStream())
                {
                    using (CryptoStream encstream = new CryptoStream(deststrm, des.CreateEncryptor(key, null), CryptoStreamMode.Write))
                    {
                        encstream.Write(msg, 0, msg.Length);
                        encstream.Close();
                        if (this.IsNeedNL(Config))
                        {
                            return Convert.ToBase64String(deststrm.ToArray())+"\r\n";                            
                        }
                        return Convert.ToBase64String(deststrm.ToArray());
                    }
                }
            }
        }
        /// <summary>
        /// 暗号処理
        /// </summary>
        /// <remarks>
        /// デフォルトではDES暗号が適応される(ScrambleKeyへ要設定)<br/>
        /// 他のスクランブルを使用する場合はオーバーロードする
        /// </remarks>
        /// <param name="msg">暗号元メッセージ</param>
        /// <param name="Config">設定情報</param>
        /// <param name="engine">ログエンジンインスタンス</param>
        /// <returns>暗号化メッセージ(文字列エンコードする）</returns>
        public virtual byte[] Encrypt(byte[] msg, BasicLogConfig Config, LogingEngine engine)
        {
            return Config.Encoding.GetBytes(EncryptToString(msg,Config,engine));
        }
        /// <summary>
        /// 複合化処理
        /// </summary>
        /// <param name="bsrc"><see cref="Encrypt"/>で生成された暗号データのBase64エンコードを解除したもの</param>
        /// <param name="ScrambleKey">暗号化キー文字列</param>
        /// <returns>複合されたデータ</returns>
        public virtual byte[] Decrypt(byte[] bsrc, string ScrambleKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] key = new byte[des.KeySize / 8];
                byte[] keysrc = System.Text.Encoding.ASCII.GetBytes(ScrambleKey);
                Array.Copy(keysrc, key, (keysrc.Length < des.KeySize ? keysrc.Length : des.KeySize));
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                using (MemoryStream deststrm = new MemoryStream())
                {
                    using (CryptoStream encstream = new CryptoStream(deststrm, des.CreateDecryptor(key, null), CryptoStreamMode.Write))
                    {
                        encstream.Write(bsrc, 0, bsrc.Length);
                        encstream.Close();
                        return deststrm.ToArray();
                    }
                }
            }
        }
        /// <summary>
        /// 複合化処理
        /// </summary>
        /// <param name="scramblestr"><see cref="Encrypt"/>で生成された暗号データ</param>
        /// <param name="ScrambleKey">暗号化キー文字列</param>
        /// <returns>複合されたデータ</returns>
        public virtual byte[] Decrypt(string scramblestr, string ScrambleKey)
        {
            byte[] bsrc = Convert.FromBase64String(scramblestr);
            return this.Decrypt(bsrc,ScrambleKey);
        }
        /// <summary>
        /// 行末改行の要否
        /// </summary>
        /// <param name="conf">該当出力先のログ設定</param>
        /// <returns>true必要</returns>
        public virtual bool IsNeedNL(BasicLogConfig conf)
        {
            if (conf.Name == LoggerConfig.FileElementCollectionName)
            {
                return true;
            }
            return false;
        }
    }
}
