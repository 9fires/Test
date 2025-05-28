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
    /// システムログ機能ログ設定
    /// </summary>
    public class LoggerConfig : ConfigurationSection
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoggerConfig()
        {
        }
        /// <summary>
        /// Commonエレメント名
        /// </summary>
        public const string CommonConfigElementName = "Common";
        /// <summary>
        /// Commonエレメント
        /// </summary>
        [ConfigurationProperty(CommonConfigElementName)]
        public CommonConfigElement Common
        {
            get
            {
                CommonConfigElement elm =
                (CommonConfigElement)base[CommonConfigElementName];
                return elm;
            }
        }
        /// <summary>
        /// Messageエレメント名
        /// </summary>
        public const string MessageElementCollectionName = "Messages";
        /// <summary>
        /// Messageエレメント
        /// </summary>
        [ConfigurationProperty(MessageElementCollectionName)]
        public MessageElementCollection Messages
        {
            get
            {
                MessageElementCollection elm =
                (MessageElementCollection)base[MessageElementCollectionName];
                return elm;
            }
        }

        /// <summary>
        /// Consoleエレメント名
        /// </summary>
        public const string ConsoleElementCollectionName = "Console";
        /// <summary>
        /// Consoleエレメント
        /// </summary>
        [ConfigurationProperty(ConsoleElementCollectionName)]
        public LoggerConfigElement Console
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[ConsoleElementCollectionName];
                return elm;
            }
        }
        /// <summary>
        /// Fileエレメント名
        /// </summary>
        public const string FileElementCollectionName = "File";
        /// <summary>
        /// Fileエレメント
        /// </summary>
        [ConfigurationProperty(FileElementCollectionName)]
        public LoggerConfigElement File
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[FileElementCollectionName];
                return elm;
            }
        }
        /// <summary>
        /// Syslogエレメント名
        /// </summary>
        public const string SyslogElementCollectionName = "Syslog";
        /// <summary>
        /// Syslogエレメント
        /// </summary>
        [ConfigurationProperty(SyslogElementCollectionName)]
        public LoggerConfigElement Syslog
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[SyslogElementCollectionName];
                return elm;
            }
        }
        /// <summary>
        /// EventLogエレメント名
        /// </summary>
        public const string EventLogElementCollectionName = "EventLog";
        /// <summary>
        /// EventLogエレメント
        /// </summary>
        [ConfigurationProperty(EventLogElementCollectionName)]
        public LoggerConfigElement EventLog
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[EventLogElementCollectionName];
                return elm;
            }
        }
        /// <summary>
        /// Extendエレメント名
        /// </summary>
        public const string ExtendElementCollectionName = "Extend";
        /// <summary>
        /// Extendエレメント
        /// </summary>
        [ConfigurationProperty(ExtendElementCollectionName)]
        public LoggerConfigElement Extend
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[ExtendElementCollectionName];
                return elm;
            }
        }
        /// <summary>
        /// Defaultエレメント名
        /// </summary>
        public const string DefaultElementCollectionName = "Default";
        /// <summary>
        /// Defaultエレメント
        /// </summary>
        [ConfigurationProperty(DefaultElementCollectionName)]
        public LoggerConfigElement Default
        {
            get
            {
                LoggerConfigElement elm =
                (LoggerConfigElement)base[DefaultElementCollectionName];
                return elm;
            }
        } 
    }
}
