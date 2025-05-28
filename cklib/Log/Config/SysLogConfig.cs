using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// 個別設定項目共通設定項目情報
    /// </summary>
    [Serializable]
    public class SysLogConfig : BasicLogConfig
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="e"></param>
        public SysLogConfig(LoggerConfigElement e)
            : base(LoggerConfig.SyslogElementCollectionName,e)
        {
            this.sysloghost = e.Server;
            this.port = e.Port;
            try
            {
                this.facility = int.Parse(e.Facility);
            }
            catch
            {
                #region Facility文字列変換
                switch (e.Facility.ToLower())
                {
                    case "auth":
                    case "security":    // 認証サービスのメッセージ（現在はauthprivが推奨されている）
                        this.facility = 4; break;
                    case "authpriv":    // 認証サービス（カテゴリはauthと同じ。authとは出力結果が異なる） 
                        this.facility = 10; break;
                    case "cron":        // cronのメッセージ 
                        this.facility = 3; break;
                    case "daemon":      // デーモンのメッセージ 
                        this.facility = 3; break;
                    case "kern":        // カーネルのメッセージ 
                        this.facility = 0; break;
                    case "lpr":         // プリンタサービスのメッセージ 
                        this.facility = 6; break;
                    case "mail":        // メールサービスのメッセージ 
                        this.facility = 2; break;
                    case "news":        //  ニュースサービスのメッセージ 
                        this.facility = 7; break;
                    case "syslog":      // syslogのメッセージ 
                        this.facility = 5; break;
                    case "user":        // ユーザープロセスのメッセージ 
                        this.facility = 1; break;
                    case "uucp":        // uucp転送を行うプログラムのメッセージ 
                        this.facility = 8; break;
                    case "local0":      // アプリケーションに依存する 
                        this.facility = 16; break;
                    case "local1":      // アプリケーションに依存する 
                        this.facility = 17; break;
                    case "local2":      // アプリケーションに依存する 
                        this.facility = 18; break;
                    case "local3":      // アプリケーションに依存する 
                        this.facility = 19; break;
                    case "local4":      // アプリケーションに依存する 
                        this.facility = 20; break;
                    case "local5":      // アプリケーションに依存する 
                        this.facility = 21; break;
                    case "local6":      // アプリケーションに依存する 
                        this.facility = 22; break;
                    case "local7":      // アプリケーションに依存する 
                        this.facility = 23; break;
                    default:
                        this.facility = 16; break;
                }
                #endregion
            }
            this.ownHost = e.hostname;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SysLogConfig()
            : base(LoggerConfig.SyslogElementCollectionName)
        { }

        /// <summary>
        /// syslogホスト
        /// </summary>
        public string Host
        {
            get
            {
                return this.sysloghost;
            }
            set
            {
                this.sysloghost = value;
            }
        }

        /// <summary>
        /// syslogホスト
        /// </summary>
        private string sysloghost = "localhost";
        /// <summary>
        /// syslogポート番号
        /// </summary>
        public int Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }
        /// <summary>
        /// syslogポート番号
        /// </summary>
        private int port = 514;
        /// <summary>
        /// syslog ファシリティ
        /// </summary>
        public int Facility
        {
            get
            {
                return this.facility;
            }
            set
            {
                this.facility = value;
            }
        }

        /// <summary>
        /// syslog ファシリティ
        /// </summary>
        private int facility = 16;
        /// <summary>
        /// 送信元ホスト名
        /// </summary>
        public string OwnHostname
        {
            get
            {
                return this.ownHost;
            }
            set
            {
                this.ownHost = value;
            }
        }
        /// <summary>
        /// 送信元ホスト名
        /// </summary>
        private string ownHost = "localhost";
    }
}
