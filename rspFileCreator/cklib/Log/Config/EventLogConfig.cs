using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace cklib.Log.Config
{
    /// <summary>
    /// 個別設定項目共通設定項目情報
    /// </summary>
    [Serializable]
    public class EventLogConfig : BasicLogConfig
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="e"></param>
        public EventLogConfig(LoggerConfigElement e)
            : base(LoggerConfig.EventLogElementCollectionName, e)
        {
            this.eventSource = e.EventSource;
            this.eventLogName = e.EventLogName;
            this.eventLogTRACE = EventLogTypeString(e.EventLogTRACE);
            this.eventLogDEBUG = EventLogTypeString(e.EventLogDEBUG);
            this.eventLogINFO = EventLogTypeString(e.EventLogINFO);
            this.eventLogNOTE = EventLogTypeString(e.EventLogNOTE);
            this.eventLogWARN = EventLogTypeString(e.EventLogWARN);
            this.eventLogERROR = EventLogTypeString(e.EventLogERROR);
            this.eventLogCRIT = EventLogTypeString(e.EventLogCRITICAL);
            this.eventLogALERT = EventLogTypeString(e.EventLogALERT);
            this.eventLogEMERG = EventLogTypeString(e.EventLogEMERGENCY);
            this.eventLogFATAL = EventLogTypeString(e.EventLogFATAL);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventLogConfig()
            : base(LoggerConfig.EventLogElementCollectionName)
        {
        }
        /// <summary>
        /// イベントソースの取得
        /// </summary>
        public string EventSource
        {
            get
            {
                return this.eventSource;
            }
            set
            {
                this.eventSource = value;
            }
        }
        /// <summary>
        /// イベントソース
        /// </summary>
        private string eventSource = string.Empty;
        /// <summary>
        /// イベントログ名（アプリケーション・システム等）
        /// </summary>
        public string EventLogName
        {
            get
            {
                return this.eventLogName;
            }
            set
            {
                this.eventLogName = value;
            }
        }
        /// <summary>
        /// イベントログ名（アプリケーション・システム等）
        /// </summary>
        private string eventLogName = string.Empty;
        /// <summary>
        /// レベルをイベントログ種類に変換
        /// </summary>
        /// <param name="level">ログレベル</param>
        /// <returns>イベントログ種類</returns>
        public EventLogEntryType LogLevelToEventLogType(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.TRACE: return this.eventLogTRACE;
                case LogLevel.DEBUG: return this.eventLogDEBUG;
                case LogLevel.INFO: return this.eventLogINFO;
                case LogLevel.NOTE: return this.eventLogNOTE;
                case LogLevel.WARN: return this.eventLogWARN;
                case LogLevel.ERROR: return this.eventLogERROR;
                case LogLevel.CRIT: return this.eventLogCRIT;
                case LogLevel.ALERT: return this.eventLogALERT;
                case LogLevel.EMERG: return this.eventLogEMERG;
                case LogLevel.FATAL: return this.eventLogFATAL;
                default:
                    return EventLogEntryType.Error;
            }
        }
        /// <summary>
        /// イベントログ TRACEログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogTRACE
        {
            get
            {
                return this.eventLogTRACE;
            }
            set
            {
                this.eventLogTRACE = value;
            }
        }
        /// <summary>
        /// ログレベルDEBUGのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogTRACE= EventLogEntryType.Information;
        /// <summary>
        /// イベントログ Debugログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogDEBUG
        {
            get
            {
                return this.eventLogDEBUG;
            }
            set
            {
                this.eventLogDEBUG = value;
            }
        }
        /// <summary>
        /// ログレベルDEBUGのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogDEBUG = EventLogEntryType.Information;
        /// <summary>
        /// イベントログ INFOログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogINFO
        {
            get
            {
                return this.eventLogINFO;
            }
            set
            {
                this.eventLogINFO = value;
            }
        }
        /// <summary>
        /// ログレベルINFOのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogINFO = EventLogEntryType.Information;
        /// <summary>
        /// イベントログ NOTEログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogNOTE
        {
            get
            {
                return this.eventLogNOTE;
            }
            set
            {
                this.eventLogNOTE = value;
            }
        }
        /// <summary>
        /// ログレベルNOTEのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogNOTE = EventLogEntryType.Information;
        /// <summary>
        /// イベントログ WARNログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogWARN
        {
            get
            {
                return this.eventLogWARN;
            }
            set
            {
                this.eventLogWARN = value;
            }
        }
        /// <summary>
        /// ログレベルWARNのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogWARN = EventLogEntryType.Warning;
        /// <summary>
        /// イベントログ ERRORログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogERROR
        {
            get
            {
                return this.eventLogERROR;
            }
            set
            {
                this.eventLogERROR = value;
            }
        }
        /// <summary>
        /// ログレベルERRORのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogERROR = EventLogEntryType.Error;
        /// <summary>
        /// イベントログ CRITICALログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogCRITICAL
        {
            get
            {
                return this.eventLogCRIT;
            }
            set
            {
                this.eventLogCRIT = value;
            }
        }
        /// <summary>
        /// ログレベルCRITのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogCRIT = EventLogEntryType.Error;
        /// <summary>
        /// イベントログ ALERTログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogALERT
        {
            get
            {
                return this.eventLogALERT;
            }
            set
            {
                this.eventLogALERT = value;
            }
        }
        /// <summary>
        /// ログレベルALERTのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogALERT = EventLogEntryType.Error;
        /// <summary>
        /// イベントログ EMERGENCYログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogEMERGENCY
        {
            get
            {
                return this.eventLogEMERG;
            }
            set
            {
                this.eventLogEMERG = value;
            }
        }
        /// <summary>
        /// ログレベルEMERGのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogEMERG = EventLogEntryType.Error;
        /// <summary>
        /// イベントログ FATALログレベルマップ
        /// </summary>
        public EventLogEntryType EventLogFATAL
        {
            get
            {
                return this.eventLogFATAL;
            }
            set
            {
                this.eventLogFATAL = value;
            }
        }
        /// <summary>
        /// ログレベルFATALのイベントログ種類
        /// </summary>
        private EventLogEntryType eventLogFATAL = EventLogEntryType.Error;

        /// <summary>
        /// 文字列からイベントログのイベント種類に変換する
        /// </summary>
        /// <param name="type">イベント種類文字列</param>
        /// <returns></returns>
        static public EventLogEntryType EventLogTypeString(string type)
        {
            switch (type.ToUpper())
            {
                case "INFORMATION": return EventLogEntryType.Information;
                case "WARNING": return EventLogEntryType.Warning;
                case "ERROR": return EventLogEntryType.Error;
                case "SUCCESSAUDIT": return EventLogEntryType.SuccessAudit;
                case "FAILUREAUDIT": return EventLogEntryType.FailureAudit;
                default:
                    return EventLogEntryType.Error;
            }
        }
        /// <summary>
        /// メッセージ上限文字数
        /// </summary>
        /// <remarks>
        /// 2016/08/23 イベントログ長さチェック追加
        /// </remarks>
        public override int MessageMaxLength
        {
            get
            {
                return base.MessageMaxLength;
            }
            set
            {
                if (value < 0)
                    base.MessageMaxLength = 15919;
                else
                    base.MessageMaxLength = value;
            }
        }

    }
}
