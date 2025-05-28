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
    /// �V�X�e�����O�@�\���O�ݒ�
    /// </summary>
    public class LoggerConfig : ConfigurationSection
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public LoggerConfig()
        {
        }
        /// <summary>
        /// Common�G�������g��
        /// </summary>
        public const string CommonConfigElementName = "Common";
        /// <summary>
        /// Common�G�������g
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
        /// Message�G�������g��
        /// </summary>
        public const string MessageElementCollectionName = "Messages";
        /// <summary>
        /// Message�G�������g
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
        /// Console�G�������g��
        /// </summary>
        public const string ConsoleElementCollectionName = "Console";
        /// <summary>
        /// Console�G�������g
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
        /// File�G�������g��
        /// </summary>
        public const string FileElementCollectionName = "File";
        /// <summary>
        /// File�G�������g
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
        /// Syslog�G�������g��
        /// </summary>
        public const string SyslogElementCollectionName = "Syslog";
        /// <summary>
        /// Syslog�G�������g
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
        /// EventLog�G�������g��
        /// </summary>
        public const string EventLogElementCollectionName = "EventLog";
        /// <summary>
        /// EventLog�G�������g
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
        /// Extend�G�������g��
        /// </summary>
        public const string ExtendElementCollectionName = "Extend";
        /// <summary>
        /// Extend�G�������g
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
        /// Default�G�������g��
        /// </summary>
        public const string DefaultElementCollectionName = "Default";
        /// <summary>
        /// Default�G�������g
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
