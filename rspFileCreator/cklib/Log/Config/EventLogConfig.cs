using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace cklib.Log.Config
{
    /// <summary>
    /// �ʐݒ荀�ڋ��ʐݒ荀�ڏ��
    /// </summary>
    [Serializable]
    public class EventLogConfig : BasicLogConfig
    {
        /// <summary>
        /// �R���X�g���N�^
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
        /// �R���X�g���N�^
        /// </summary>
        public EventLogConfig()
            : base(LoggerConfig.EventLogElementCollectionName)
        {
        }
        /// <summary>
        /// �C�x���g�\�[�X�̎擾
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
        /// �C�x���g�\�[�X
        /// </summary>
        private string eventSource = string.Empty;
        /// <summary>
        /// �C�x���g���O���i�A�v���P�[�V�����E�V�X�e�����j
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
        /// �C�x���g���O���i�A�v���P�[�V�����E�V�X�e�����j
        /// </summary>
        private string eventLogName = string.Empty;
        /// <summary>
        /// ���x�����C�x���g���O��ނɕϊ�
        /// </summary>
        /// <param name="level">���O���x��</param>
        /// <returns>�C�x���g���O���</returns>
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
        /// �C�x���g���O TRACE���O���x���}�b�v
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
        /// ���O���x��DEBUG�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogTRACE= EventLogEntryType.Information;
        /// <summary>
        /// �C�x���g���O Debug���O���x���}�b�v
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
        /// ���O���x��DEBUG�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogDEBUG = EventLogEntryType.Information;
        /// <summary>
        /// �C�x���g���O INFO���O���x���}�b�v
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
        /// ���O���x��INFO�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogINFO = EventLogEntryType.Information;
        /// <summary>
        /// �C�x���g���O NOTE���O���x���}�b�v
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
        /// ���O���x��NOTE�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogNOTE = EventLogEntryType.Information;
        /// <summary>
        /// �C�x���g���O WARN���O���x���}�b�v
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
        /// ���O���x��WARN�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogWARN = EventLogEntryType.Warning;
        /// <summary>
        /// �C�x���g���O ERROR���O���x���}�b�v
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
        /// ���O���x��ERROR�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogERROR = EventLogEntryType.Error;
        /// <summary>
        /// �C�x���g���O CRITICAL���O���x���}�b�v
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
        /// ���O���x��CRIT�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogCRIT = EventLogEntryType.Error;
        /// <summary>
        /// �C�x���g���O ALERT���O���x���}�b�v
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
        /// ���O���x��ALERT�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogALERT = EventLogEntryType.Error;
        /// <summary>
        /// �C�x���g���O EMERGENCY���O���x���}�b�v
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
        /// ���O���x��EMERG�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogEMERG = EventLogEntryType.Error;
        /// <summary>
        /// �C�x���g���O FATAL���O���x���}�b�v
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
        /// ���O���x��FATAL�̃C�x���g���O���
        /// </summary>
        private EventLogEntryType eventLogFATAL = EventLogEntryType.Error;

        /// <summary>
        /// �����񂩂�C�x���g���O�̃C�x���g��ނɕϊ�����
        /// </summary>
        /// <param name="type">�C�x���g��ޕ�����</param>
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
        /// ���b�Z�[�W���������
        /// </summary>
        /// <remarks>
        /// 2016/08/23 �C�x���g���O�����`�F�b�N�ǉ�
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
