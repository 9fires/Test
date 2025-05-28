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
    /// ���O�ݒ�G�������g��`�N���X
    /// </summary>
    public class LoggerConfigElement : ConfigurationElement
    {
        //public readonly string Name;
        /// <summary>
        /// �f�t�H���g�R���X�g���N�^
        /// </summary>
        public LoggerConfigElement()
        {}
        /// <summary>
        /// �G�������g
        /// </summary>
        /// <param name="elementName"></param>
        public LoggerConfigElement(string elementName)
        {
            Name = elementName;
        }
        /// <summary>
        /// �G�������g��
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
        /// ���O�̉�
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
        /// ���O���x��
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
        /// ���O�t�@�C���i�[�p�X
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
        /// ���O�t�@�C����
        /// </summary>
        /// <remarks>
        /// ���ڔԍ��@���ړ��e<br/>
        /// {0} ����<br/>
        /// �f�t�H���g�͈ȉ��̏���<br/>
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
        /// ���O�t�@�C�����b�N
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C�����������݃��b�N����<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
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
        /// ���O�t�@�C�����b�N���g���C�C���^�[�o��(msec
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C���I�[�v�����̔r�����b�N�ɂ�郊�g���C����<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
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
        /// ���O�t�@�C�����b�N���g���C��
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C���I�[�v�����̔r�����b�N�ɂ�郊�g���C���s��<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
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
        /// ���O�r������Mutex��
        /// </summary>
        /// <remarks>
        /// ���O�������݂�Mutex�ɂ��r�����䂷��B�󕶎��̏ꍇ�͖���
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
        /// ���O�r������Mutex�ҋ@����(msec)
        /// </summary>
        /// <remarks>
        /// ���O�r������Mutex�����b�N�o����܂ł̑ҋ@����<br/>
        /// �^�C���I�[�o�[�����ꍇ�́A���O�������݂͎��s����B
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
        /// ���O���[�e�V�����ۑ�����
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
        /// ���O���[�e�V�����T�C�Y�w��
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
        /// ���[�e�[�g���O�t�@�C����
        /// </summary>
        /// <remarks>
        /// ���ڔԍ��@���ړ��e<br/>
        /// {0} ���O�t�@�C����<br/>
        /// {1} ����<br/>
        /// {2} �_�b<br/>
        /// {3} �������O�̒ʔ�<br/>
        /// �ʔԂƓ������݂͕s��
        /// �ʔԂ́A�ҏW���ς���ƍő�l���o������ƂȂ�̂ŁA�ʒu�Œ�Ƃ��܂��B
        /// �f�t�H���g�͈ȉ��̏���<br/>
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
        /// ���O�����k�t�@�C���ɂ���
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
        /// ZIP���[�e�V����
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
        /// ZIP���[�e�V�������{����
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
        /// ZIP���[�e�V�����p�X���[�h
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
        /// ���O�����R�[�h�ݒ�
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
        /// �C�x���g�\�[�X
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
        /// �C�x���g���O��
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
        /// �C�x���g���O Debug���O���x���}�b�v
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
        /// �C�x���g���O Debug���O���x���}�b�v
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
        /// �C�x���g���O INFO���O���x���}�b�v
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
        /// �C�x���g���O NOTE���O���x���}�b�v
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
        /// �C�x���g���O WARN���O���x���}�b�v
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
        /// �C�x���g���O ERROR���O���x���}�b�v
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
        /// �C�x���g���O CRITICAL���O���x���}�b�v
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
        /// �C�x���g���O ALERT���O���x���}�b�v
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
        /// �C�x���g���O EMERGENCY���O���x���}�b�v
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
        /// �C�x���g���O FATAL���O���x���}�b�v
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
        /// ���b�Z�[�W���������
        /// </summary>
        /// <remarks>
        /// 2016/08/23 �C�x���g���O�����`�F�b�N�ǉ�
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
        /// syslog�T�[�o�[�̃z�X�g��
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
        /// syslog�T�[�o�[�̃|�[�g�ԍ�
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
        /// syslog��facility
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
        /// syslog���̎��z�X�g��
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
        /// �����w��
        /// </summary>
        /// <remarks>
        /// ���ڔԍ��@���ړ��e<br/>
        /// {0} ����<br/>
        /// {1} �����~���b<br/>
        /// {2} ���x��<br/>
        /// {3} ���O�R�[�h�i�f�o�b�O���O������у��O�R�[�h�𖢎g�p�̏ꍇ�͖��������<br/>
        /// {4} ���b�Z�[�W<br/>
        /// {5} ���ۂ̔��������\�[�X<br/>
        /// {6} ���ۂ̔��������N���X<br/>
        /// {7} ���ۂ̔��������N���X���\�b�h<br/>
        /// {8} �\�[�X�t�@�C���p�X<br/>
        /// {9} �\�[�X�t�@�C����<br/>
        /// {10} �\�[�X�s�ԍ�<br/>
        /// {11} �X���b�hID<br/>
        /// �f�t�H���g�͈ȉ��̏���<br/>
        /// {0:yyyy/MM/dd HH:mm:ss}.{1:000} {11} {2,-6} {3:0000} {4} {5} {6}.{7} {8}{9}:{10}<br/>
        /// ��1 ��O���͉��s��ɕt�������<br/>
        /// ��2 syslog�ɂ͍s�P�ʂő��M�����<br/>
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
        /// ���O�t�@�C���̓��e���Í�������
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
        /// �Í����L�[
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
        /// �G�������g�f�V���A���C�Y�@
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
        /// �G�������g�V���A���C�Y
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
        /// �ύX�̗L����Ԃ�
        /// </summary>
        /// <returns></returns>
        protected override bool IsModified()
        {
            bool ret = base.IsModified();

            return ret;
        }


    }
}
