using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using cklib;
using cklib.Util;

namespace cklib.Log.Config
{
    /// <summary>
    /// ログ設定エレメント定義クラス
    /// </summary>
    public class LoggerConfigElement : ConfigurationElement
    {
        //public readonly string Name;
        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public LoggerConfigElement()
        {}
        /// <summary>
        /// エレメント
        /// </summary>
        /// <param name="elementName"></param>
        public LoggerConfigElement(string elementName)
        {
            Name = elementName;
        }
        /// <summary>
        /// エレメント名
        /// </summary>
        [ConfigurationProperty("name", 
            DefaultValue = "",
            IsRequired = false, 
            IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        /// <summary>
        /// ログの可否
        /// </summary>
        [ConfigurationProperty("enabled",
            DefaultValue = "false",
            IsRequired = false)]
        public bool Enabled
        {
            get
            {
                return (bool)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
        /// <summary>
        /// ログレベル
        /// </summary>
        [ConfigurationProperty("level",
            DefaultValue = "debug",
            IsRequired = false)]
        [RegexStringValidator(@"[A-Za-z]*")]
        public string Level
        {
            get
            {
                return  (string)this["level"];
            }
            set
            {
                this["level"] = value.GetType().FullName;
            }
        }
        /// <summary>
        /// ログファイル格納パス
        /// </summary>
        [ConfigurationProperty("path",
            DefaultValue = ".\\",
            IsRequired = false)]
        [StringValidator(InvalidCharacters = "~!@#$^&*()[]{}/;'\"|",
            MinLength = 1, MaxLength = 260)]
        public string Path
        {
            get
            {
                return (string)this["path"];
            }
            set
            {
                this["path"] = value;
            }
        }
        /// <summary>
        /// ログファイル名
        /// </summary>
        /// <remarks>
        /// 項目番号　項目内容<br/>
        /// {0} 日時<br/>
        /// デフォルトは以下の書式<br/>
        /// {0:yyyyMMdd}.log<br/>
        /// </remarks>
        [ConfigurationProperty("FileName",
            DefaultValue = "{0:yyyyMMdd}.log",
            IsRequired = false)]
        [StringValidator(MinLength = 1, MaxLength = 260)]
        public string FileName
        {
            get
            {
                return (string)this["FileName"];
            }
            set
            {
                this["FileName"] = value;
            }
        }
        /// <summary>
        /// ログファイルロック
        /// </summary>
        /// <remarks>
        /// ログファイルを書き込みロックする<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        [ConfigurationProperty("FileLock",
            DefaultValue = "false",
            IsRequired = false)]
        public bool FileLock
        {
            get
            {
                return (bool)this["FileLock"];
            }
            set
            {
                this["FileLock"] = value;
            }
        }
        /// <summary>
        /// ログファイルロックリトライインターバル(msec
        /// </summary>
        /// <remarks>
        /// ログファイルオープン時の排他ロックによるリトライ時間<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        [ConfigurationProperty("FileLockWaitTime",
            DefaultValue = "200",
            IsRequired = false)]
        public int FileLockWaitTime
        {
            get
            {
                return (int)this["FileLockWaitTime"];
            }
            set
            {
                this["FileLockWaitTime"] = value;
            }
        }
        /// <summary>
        /// ログファイルロックリトライ回数
        /// </summary>
        /// <remarks>
        /// ログファイルオープン時の排他ロックによるリトライ試行回数<br/>
        /// マルチプロセスで同一ログファイルに書き込む際有効にする
        /// </remarks>
        [ConfigurationProperty("FileLockTryLimit",
            DefaultValue = "-1",
            IsRequired = false)]
        public int FileLockTryLimit
        {
            get
            {
                return (int)this["FileLockTryLimit"];
            }
            set
            {
                this["FileLockTryLimit"] = value;
            }
        }
        /// <summary>
        /// ログ排他制御Mutex名
        /// </summary>
        /// <remarks>
        /// ログ書き込みをMutexにより排他制御する。空文字の場合は無効
        /// </remarks>
        [ConfigurationProperty("MutexLock",
           DefaultValue = "",
           IsRequired = false)]
        public string MutexLock
        {
            get
            {
                return this["MutexLock"].ToString();
            }
            set
            {
                this["MutexLock"] = value;
            }
        }
        /// <summary>
        /// ログ排他制御Mutex待機時間(msec)
        /// </summary>
        /// <remarks>
        /// ログ排他制御Mutexをロック出来るまでの待機時間<br/>
        /// タイムオーバーした場合は、ログ書き込みは失敗する。
        /// </remarks>
        [ConfigurationProperty("MutexLockWaitTime",
            DefaultValue = "-1",
            IsRequired = false)]
        public int MutexLockWaitTime
        {
            get
            {
                return (int)this["MutexLockWaitTime"];
            }
            set
            {
                this["MutexLockWaitTime"] = value;
            }
        }
        /// <summary>
        /// ログローテション保存日数
        /// </summary>
        [ConfigurationProperty("RotateDays",
            DefaultValue = "-1",
            IsRequired = false)]
        public int RotateDays
        {
            get
            {
                return (int)this["RotateDays"];
            }
            set
            {
                this["RotateDays"] = value;
            }
        }

        /// <summary>
        /// ログローテションサイズ指定
        /// </summary>
        [ConfigurationProperty("RotateSize",
            DefaultValue = "-1",
            IsRequired = false)]
        [RegexStringValidator(@"^-?([0-9]+)|([0-9]+\.[0-9]+)[TGMKtgmk]?$")]
        public string RotateSize
        {
            get
            {
                return (string)this["RotateSize"];
            }
            set
            {
                this["RotateSize"] = value;
            }
        }
        /// <summary>
        /// ローテートログファイル名
        /// </summary>
        /// <remarks>
        /// 項目番号　項目内容<br/>
        /// {0} ログファイル名<br/>
        /// {1} 日時<br/>
        /// {2} ㍉秒<br/>
        /// {3} 同日ログの通番<br/>
        /// 通番と日時混在は不可
        /// 通番は、編集を可変すると最大値抽出が困難となるので、位置固定とします。
        /// デフォルトは以下の書式<br/>
        /// {0}.{3}.log<br/>
        /// </remarks>
        [ConfigurationProperty("RotateFileName",
            DefaultValue = "{0}.{3}.log",
            IsRequired = false)]
        [StringValidator(MinLength = 1, MaxLength = 260)]
        public string RotateFileName
        {
            get
            {
                return (string)this["RotateFileName"];
            }
            set
            {
                this["RotateFileName"] = value;
            }
        }

        /// <summary>
        /// ログを圧縮ファイルにする
        /// </summary>
        [ConfigurationProperty("Compress",
            DefaultValue = "false",
            IsRequired = false)]
        public bool Compress
        {
            get
            {
                return (bool)this["Compress"];
            }
            set
            {
                this["Compress"] = value;
            }
        }

        /// <summary>
        /// ZIPローテション
        /// </summary>
        [ConfigurationProperty("ZipRotate",
            DefaultValue = "false",
            IsRequired = false)]
        public bool ZipRotate
        {
            get
            {
                return (bool)this["ZipRotate"];
            }
            set
            {
                this["ZipRotate"] = value;
            }
        }

        /// <summary>
        /// ZIPローテション実施日数
        /// </summary>
        [ConfigurationProperty("ZipRotateDays",
            DefaultValue = "-1",
            IsRequired = false)]
        public string ZipRotateDays
        {
            get
            {
                return (string)this["ZipRotateDays"];
            }
            set
            {
                this["ZipRotateDays"] = value;
            }
        }

        /// <summary>
        /// ZIPローテションパスワード
        /// </summary>
        [ConfigurationProperty("ZipRotatePassword",
            DefaultValue = "",
            IsRequired = false)]
        public string ZipRotatePassword
        {
            get
            {
                return (string)this["ZipRotatePassword"];
            }
            set
            {
                this["ZipRotatePassword"] = value;
            }
        }

        /// <summary>
        /// ログ文字コード設定
        /// </summary>
        [ConfigurationProperty("encoding",
            DefaultValue = "UTF-8",
            IsRequired = false)]
        public string Encoding
        {
            get
            {
                return (string)this["encoding"];
            }
            set
            {
                this["encoding"] = value;
            }
        }

        /// <summary>
        /// イベントソース
        /// </summary>
        [ConfigurationProperty("eventsource",
            DefaultValue = "",
            IsRequired = false)]
        public string EventSource
        {
            get
            {
                return (string)this["eventsource"];
            }
            set
            {
                this["eventsource"] = value;
            }
        }
        /// <summary>
        /// イベントログ名
        /// </summary>
        [ConfigurationProperty("EventLogName",
            DefaultValue = "",
            IsRequired = false)]
        public string EventLogName
        {
            get
            {
                return (string)this["EventLogName"];
            }
            set
            {
                this["EventLogName"] = value;
            }
        }
        /// <summary>
        /// イベントログ Debugログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogTRACE",
            DefaultValue = "Information",
            IsRequired = false)]
        public string EventLogTRACE
        {
            get
            {
                return (string)this["EventLogTRACE"];
            }
            set
            {
                this["EventLogTRACE"] = value;
            }
        }
        /// <summary>
        /// イベントログ Debugログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogDEBUG",
            DefaultValue = "Information",
            IsRequired = false)]
        public string EventLogDEBUG
        {
            get
            {
                return (string)this["EventLogDEBUG"];
            }
            set
            {
                this["EventLogDEBUG"] = value;
            }
        }
        /// <summary>
        /// イベントログ INFOログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogINFO",
            DefaultValue = "Information",
            IsRequired = false)]
        public string EventLogINFO
        {
            get
            {
                return (string)this["EventLogINFO"];
            }
            set
            {
                this["EventLogINFO"] = value;
            }
        }
        /// <summary>
        /// イベントログ NOTEログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogNOTE",
            DefaultValue = "Information",
            IsRequired = false)]
        public string EventLogNOTE
        {
            get
            {
                return (string)this["EventLogNOTE"];
            }
            set
            {
                this["EventLogNOTE"] = value;
            }
        }
        /// <summary>
        /// イベントログ WARNログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogWARN",
            DefaultValue = "Warning",
            IsRequired = false)]
        public string EventLogWARN
        {
            get
            {
                return (string)this["EventLogWARN"];
            }
            set
            {
                this["EventLogWARN"] = value;
            }
        }
        /// <summary>
        /// イベントログ ERRORログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogERROR",
            DefaultValue = "Error",
            IsRequired = false)]
        public string EventLogERROR
        {
            get
            {
                return (string)this["EventLogERROR"];
            }
            set
            {
                this["EventLogERROR"] = value;
            }
        }
        /// <summary>
        /// イベントログ CRITICALログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogCRITICAL",
            DefaultValue = "Error",
            IsRequired = false)]
        public string EventLogCRITICAL
        {
            get
            {
                return (string)this["EventLogCRITICAL"];
            }
            set
            {
                this["EventLogCRITICAL"] = value;
            }
        }
        /// <summary>
        /// イベントログ ALERTログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogALERT",
            DefaultValue = "Error",
            IsRequired = false)]
        public string EventLogALERT
        {
            get
            {
                return (string)this["EventLogALERT"];
            }
            set
            {
                this["EventLogALERT"] = value;
            }
        }
        /// <summary>
        /// イベントログ EMERGENCYログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogEMERGENCY",
            DefaultValue = "Error",
            IsRequired = false)]
        public string EventLogEMERGENCY
        {
            get
            {
                return (string)this["EventLogEMERGENCY"];
            }
            set
            {
                this["EventLogEMERGENCY"] = value;
            }
        }
        /// <summary>
        /// イベントログ FATALログレベルマップ
        /// </summary>
        [ConfigurationProperty("EventLogFATAL",
            DefaultValue = "Error",
            IsRequired = false)]
        public string EventLogFATAL
        {
            get
            {
                return (string)this["EventLogFATAL"];
            }
            set
            {
                this["EventLogFATAL"] = value;
            }
        }

        /// <summary>
        /// メッセージ上限文字数
        /// </summary>
        /// <remarks>
        /// 2016/08/23 イベントログ長さチェック追加
        /// </remarks>
        [ConfigurationProperty("MessageMaxLength",
            DefaultValue = "-1",
            IsRequired = false)]
        public int MessageMaxLength
        {
            get
            {
                return (int)this["MessageMaxLength"];
            }
            set
            {
                this["MessageMaxLength"] = value;
            }
        }

        /// <summary>
        /// syslogサーバーのホスト名
        /// </summary>
        [ConfigurationProperty("server",
            DefaultValue = "localhost",
            IsRequired = false)]
        public string Server
        {
            get
            {
                return (string)this["server"];
            }
            set
            {
                this["server"] = value;
            }
        }
        /// <summary>
        /// syslogサーバーのポート番号
        /// </summary>
        [ConfigurationProperty("port",
            DefaultValue = "514",
            IsRequired = false)]
        [IntegerValidator(MinValue= 1, MaxValue= 65535)]
        public int Port
        {
            get
            {
                return (int)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }
        /// <summary>
        /// syslogのfacility
        /// </summary>
        [ConfigurationProperty("facility",
            DefaultValue = "local0",
            IsRequired = false)]
        public string Facility
        {
            get
            {
                return (string)this["facility"];
            }
            set
            {
                this["facility"] = value;
            }
        }
        /// <summary>
        /// syslog時の自ホスト名
        /// </summary>
        [ConfigurationProperty("hostname",
            DefaultValue = "localhost",
            IsRequired = false)]
        public string hostname
        {
            get
            {
                return (string)this["hostname"];
            }
            set
            {
                this["hostname"] = value;
            }
        }
        /// <summary>
        /// 書式指定
        /// </summary>
        /// <remarks>
        /// 項目番号　項目内容<br/>
        /// {0} 日時<br/>
        /// {1} 日時ミリ秒<br/>
        /// {2} レベル<br/>
        /// {3} ログコード（デバッグログ時およびログコードを未使用の場合は無視される<br/>
        /// {4} メッセージ<br/>
        /// {5} 事象の発生したソース<br/>
        /// {6} 事象の発生したクラス<br/>
        /// {7} 事象の発生したクラスメソッド<br/>
        /// {8} ソースファイルパス<br/>
        /// {9} ソースファイル名<br/>
        /// {10} ソース行番号<br/>
        /// {11} スレッドID<br/>
        /// デフォルトは以下の書式<br/>
        /// {0:yyyy/MM/dd HH:mm:ss}.{1:000} {11} {2,-6} {3:0000} {4} {5} {6}.{7} {8}{9}:{10}<br/>
        /// ※1 例外情報は改行後に付加される<br/>
        /// ※2 syslogには行単位で送信される<br/>
        /// </remarks>
        [ConfigurationProperty("format",
            DefaultValue = "",
            IsRequired = false)]
        public string Format
        {
            get
            {
                return (string)this["format"];
            }
            set
            {
                this["format"] = value;
            }
        }
        /// <summary>
        /// ログファイルの内容を暗号化する
        /// </summary>
        [ConfigurationProperty("scramble",
            DefaultValue = "false",
            IsRequired = false)]
        public bool scramble
        {
            get
            {
                return (bool)this["scramble"];
            }
            set
            {
                this["scramble"] = value;
            }
        }

        /// <summary>
        /// 暗号化キー
        /// </summary>
        [ConfigurationProperty("scrambleKey",
            DefaultValue = "",
            IsRequired = false)]
        public string scrambleKey
        {
            get
            {
                return (string)this["scrambleKey"];
            }
            set
            {
                this["scrambleKey"] = value;
            }
        }
        /// <summary>
        /// エレメントデシリアライズ　
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="serializeCollectionKey"></param>
        protected override void DeserializeElement(
           System.Xml.XmlReader reader, 
            bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, 
                serializeCollectionKey);
        }

        /// <summary>
        /// エレメントシリアライズ
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="serializeCollectionKey"></param>
        /// <returns></returns>
        protected override bool SerializeElement(
            System.Xml.XmlWriter writer, 
            bool serializeCollectionKey)
        {
            bool ret = base.SerializeElement(writer, 
                serializeCollectionKey);
            return ret;

        }

        /// <summary>
        /// 変更の有無を返す
        /// </summary>
        /// <returns></returns>
        protected override bool IsModified()
        {
            bool ret = base.IsModified();

            return ret;
        }


    }
}
