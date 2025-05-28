using cklib.Log;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Text;

namespace cklib.Data
{
    /// <summary>
    /// DB接続パラメータ定義
    /// </summary>
    [Serializable]
    public class DataConfigElement : ConfigurationElement
    {
        /// <summary>
        /// 接続文字列
        /// </summary>
        [ConfigurationProperty("ConnectString",
            IsRequired = true)]
        public string ConnectString
        {
            get
            {
                return (string)this["ConnectString"];
            }
            set
            {
                this["ConnectString"] = value;
            }
        }
        /// <summary>
        /// コマンドタイムアウト
        /// </summary>
        [ConfigurationProperty("CommandTimeout",
            DefaultValue = "-1",
            IsRequired = false)]
        public int CommandTimeout
        {
            get
            {
                return (int)this["CommandTimeout"];
            }
            set
            {
                this["CommandTimeout"] = value;
            }
        }

        /// <summary>
        /// デッドロックリトライ可否
        /// </summary>
        [ConfigurationProperty("EnableDeadLockRetry",
            DefaultValue = "true",
            IsRequired = false)]
        public bool EnableDeadLockRetry
        {
            get
            {
                return (bool)this["EnableDeadLockRetry"];
            }
            set
            {
                this["EnableDeadLockRetry"] = value;
            }
        }
 
        /// <summary>
        /// デッドロックリトライ回数
        /// </summary>
        [ConfigurationProperty("DeadLockRetryCountLimit",
            DefaultValue = "-1",
            IsRequired = false)]
        public int DeadLockRetryCountLimit
        {
            get
            {
                return (int)this["DeadLockRetryCountLimit"];
            }
            set
            {
                this["DeadLockRetryCountLimit"] = value;
            }
        }

        /// <summary>
        /// デッドロックリトライディレイ時間(ミリ秒）
        /// </summary>
        [ConfigurationProperty("DeadLockRetryDelayTime",
            DefaultValue = "-1",
            IsRequired = false)]
        public int DeadLockRetryDelayTime
        {
            get
            {
                return (int)this["DeadLockRetryDelayTime"];
            }
            set
            {
                this["DeadLockRetryDelayTime"] = value;
            }
        }
 
        /// <summary>
        /// コマンドタイムアウトリトライ可否
        /// </summary>
        [ConfigurationProperty("EnableTimeoutRetry",
            DefaultValue = "false",
            IsRequired = false)]
        public bool EnableTimeoutRetry
        {
            get
            {
                return (bool)this["EnableTimeoutRetry"];
            }
            set
            {
                this["EnableTimeoutRetry"] = value;
            }
        }

        /// <summary>
        /// コマンドタイムアウトリトライ回数
        /// </summary>
        [ConfigurationProperty("TimoutRetryCountLimit",
           DefaultValue = "-1",
           IsRequired = false)]
        public int TimoutRetryCountLimit
        {
            get
            {
                return (int)this["TimoutRetryCountLimit"];
            }
            set
            {
                this["TimoutRetryCountLimit"] = value;
            }
        }

        /// <summary>
        /// コマンドタイムアウトディレイ時間(ミリ秒）
        /// </summary>
        [ConfigurationProperty("TimeoutRetryDelayTime",
           DefaultValue = "-1",
           IsRequired = false)]
        public int TimeoutRetryDelayTime
        {
            get
            {
                return (int)this["TimeoutRetryDelayTime"];
            }
            set
            {
                this["TimeoutRetryDelayTime"] = value;
            }
        }
        /// <summary>
        /// ログマネージャキー名
        /// </summary>
        [ConfigurationProperty("LogSectionName",
            DefaultValue = "",
            IsRequired = false)]
        public string LogSectionName
        {
            get
            {
                return (string)this["LogSectionName"];
            }
            set
            {
                this["LogSectionName"] = value;
            }
        }
        /// <summary>
        /// SQLログ採取
        /// </summary>
        [ConfigurationProperty("SqlLogEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool SqlLogEnabled
        {
            get
            {
                return (bool)this["SqlLogEnabled"];
            }
            set
            {
                this["SqlLogEnabled"] = value;
            }
        }

        /// <summary>
        /// SQLログマネージャキー名
        /// </summary>
        [ConfigurationProperty("SqlLogSectionName",
            DefaultValue = "",
            IsRequired = false)]
        public string SqlLogSectionName
        {
            get
            {
                return (string)this["SqlLogSectionName"];
            }
            set
            {
                this["SqlLogSectionName"] = value;
            }
        }

        /// <summary>
        /// SQLログレベル
        /// </summary>
        [ConfigurationProperty("LogLevel",
            DefaultValue = "TRACE",
            IsRequired = false)]
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)this["LogLevel"];
            }
            set
            {
                this["LogLevel"] = value;
            }
        }
        
        /// <summary>
        /// パラメータログ採取
        /// </summary>
        [ConfigurationProperty("SQLTraceInhConnectStringLogging",
            DefaultValue = "false",
            IsRequired = false)]
        public bool SQLTraceInhConnectStringLogging
        {
            get
            {
                return (bool)this["SQLTraceInhConnectStringLogging"];
            }
            set
            {
                this["SQLTraceInhConnectStringLogging"] = value;
            }
        }

        /// <summary>
        /// パラメータログ採取
        /// </summary>
        [ConfigurationProperty("ParameterLogEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool ParameterLogEnabled
        {
            get
            {
                return (bool)this["ParameterLogEnabled"];
            }
            set
            {
                this["ParameterLogEnabled"] = value;
            }
        }

        /// <summary>
        /// データセットパラメータログ採取
        /// </summary>
        [ConfigurationProperty("ParameterLogDataSetEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool ParameterLogDataSetEnabled
        {
            get
            {
                return (bool)this["ParameterLogDataSetEnabled"];
            }
            set
            {
                this["ParameterLogDataSetEnabled"] = value;
            }
        }

        /// <summary>
        /// Imageパラメータログ採取
        /// </summary>
        [ConfigurationProperty("ParameterLogImageEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool ParameterLogImageEnabled
        {
            get
            {
                return (bool)this["ParameterLogImageEnabled"];
            }
            set
            {
                this["ParameterLogImageEnabled"] = value;
            }
        }

        /// <summary>
        /// Textパラメータログ採取
        /// </summary>
        [ConfigurationProperty("ParameterLogTextEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool ParameterLogTextEnabled
        {
            get
            {
                return (bool)this["ParameterLogTextEnabled"];
            }
            set
            {
                this["ParameterLogTextEnabled"] = value;
            }
        }
        /// <summary>
        /// structiureパラメータログ採取
        /// </summary>
        [ConfigurationProperty("ParameterLogStructureEnabled",
            DefaultValue = "true",
            IsRequired = false)]
        public bool ParameterLogStructureEnabled
        {
            get
            {
                return (bool)this["ParameterLogStructureEnabled"];
            }
            set
            {
                this["ParameterLogStructureEnabled"] = value;
            }
        }
        /// <summary>
        /// パラメータログ出力時に内容をマスクするパラメータ名判別用正規表現
        /// </summary>
        [ConfigurationProperty("ParameterMaskRegex",
            DefaultValue = "",
            IsRequired = false)]
        public string ParameterMaskRegex
        {
            get
            {
                return (string)this["ParameterMaskRegex"];
            }
            set
            {
                this["ParameterMaskRegex"] = value;
            }
        }
        /// <summary>
        /// デフォルトのエラーログコード
        /// </summary>
        [ConfigurationProperty("ErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.SystemError,cklib",
            IsRequired = false)]
        public string ErrorLogCode
        {
            get
            {
                return (string)this["ErrorLogCode"];
            }
            set
            {
                this["ErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        [ConfigurationProperty("DBConnectErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.DBConnectError,cklib",
            IsRequired = false)]
        public string DBConnectErrorLogCode
        {
            get
            {
                return (string)this["DBConnectErrorLogCode"];
            }
            set
            {
                this["DBConnectErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのDBエラーログコード
        /// </summary>
        [ConfigurationProperty("DBErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.DBError,cklib",
            IsRequired = false)]
        public string DBErrorLogCode
        {
            get
            {
                return (string)this["DBErrorLogCode"];
            }
            set
            {
                this["DBErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのデッドロックエラーログコード
        /// </summary>
        [ConfigurationProperty("DeadLockErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.DBDeadLock,cklib",
            IsRequired = false)]
        public string DeadLockErrorLogCode
        {
            get
            {
                return (string)this["DeadLockErrorLogCode"];
            }
            set
            {
                this["DeadLockErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのデッドロックリトライオーバーログコード
        /// </summary>
        [ConfigurationProperty("DeadLockRetryOverErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.DBDeadLockRetryOver,cklib",
            IsRequired = false)]
        public string DeadLockRetryOverErrorLogCode
        {
            get
            {
                return (string)this["DeadLockRetryOverErrorLogCode"];
            }
            set
            {
                this["DeadLockRetryOverErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのコマンドタイムアウトエラーログコード
        /// </summary>
        [ConfigurationProperty("TimeoutErrorLogCode",
           DefaultValue = "cklib.Log.LogCodes.DBTimeout,cklib",
           IsRequired = false)]
        public string TimeoutErrorLogCode
        {
            get
            {
                return (string)this["TimeoutErrorLogCode"];
            }
            set
            {
                this["TimeoutErrorLogCode"] = value;
            }
        }
        /// <summary>
        /// デフォルトのタイムアウトリトライオーバーログコード
        /// </summary>
        [ConfigurationProperty("TimeoutRetryOverErrorLogCode",
            DefaultValue = "cklib.Log.LogCodes.DBTimeoutRetryOver,cklib",
            IsRequired = false)]
        public string TimeoutRetryOverErrorLogCode
        {
            get
            {
                return (string)this["TimeoutRetryOverErrorLogCode"];
            }
            set
            {
                this["TimeoutRetryOverErrorLogCode"] = value;
            }
        }

    }
}
