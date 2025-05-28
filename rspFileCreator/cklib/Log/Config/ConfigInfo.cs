using System;
using System.Collections.Generic;
using System.Text;
using cklib.Log.ExceptionFormters;
using System.Configuration;
namespace cklib.Log.Config
{
    /// <summary>
    /// ���O�ݒ���N���X
    /// </summary>
    [Serializable]
    public class ConfigInfo
    {
        /// <summary>
        /// �f�t�H���g�̃R���t�B�O���[�V�����Z�N�V������
        /// </summary>
        public const string DefaultConfigSectionName    =   "SystemLog";
        /// <summary>
        /// ���O�ݒ�Z�N�V������
        /// </summary>
        public readonly string SectionName;
        #region �R���X�g���N�^
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="conf">�\���t�@�C���w��</param>
        /// <param name="sectionname">�Z�N�V������</param>
        public ConfigInfo(Configuration conf, string sectionname)
        {
            LoggerConfig lc;
            this.SectionName = sectionname;
            lc = (LoggerConfig)conf.GetSection(sectionname);
            IntializeLoggerConfig(lc);
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="sectionname">�Z�N�V������</param>
        public ConfigInfo(string sectionname)
        {
            LoggerConfig lc;
            this.SectionName = sectionname;
            lc = (LoggerConfig)System.Configuration.ConfigurationManager.GetSection(sectionname);
            IntializeLoggerConfig(lc);
        }
        /// <summary>
        /// �ݒ�t�@�C���ŏ���������
        /// </summary>
        /// <param name="lc"></param>
        private void IntializeLoggerConfig(LoggerConfig lc)
        {
            this.m_Common = new CommonConfig(lc.Common);
            this.m_Default = new BasicLogConfig(LoggerConfig.DefaultElementCollectionName,lc.Default);
            this.m_Console = new BasicLogConfig(LoggerConfig.ConsoleElementCollectionName,lc.Console);
            this.m_Extend = new BasicLogConfig(LoggerConfig.ExtendElementCollectionName, lc.Extend);
            this.m_File = new FileLogConfig(lc.File);
            this.m_EventLog = new EventLogConfig(lc.EventLog);
            this.m_Syslog = new SysLogConfig(lc.Syslog);
            if (lc.Messages.Count != 0)
                this.m_Message = new MessagesConfig(lc.Messages);
            else
                if (this.m_Common.MessageFile.Length != 0)
                    this.m_Message = new MessagesConfig(this.m_Common.MessageFile);
                else
                    this.m_Message = new MessagesConfig();
            SetupDefaultExceptionFormaterList();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public ConfigInfo()
        {
            this.SectionName = DefaultConfigSectionName;
            this.m_Common = new CommonConfig();
            this.m_Default = new BasicLogConfig(LoggerConfig.DefaultElementCollectionName);
            this.m_Console = new BasicLogConfig(LoggerConfig.ConsoleElementCollectionName);
            this.m_Extend = new BasicLogConfig(LoggerConfig.ExtendElementCollectionName);
            this.m_File = new FileLogConfig();
            this.m_EventLog = new EventLogConfig();
            this.m_Syslog = new SysLogConfig();
            this.m_Message = new MessagesConfig();
            SetupDefaultExceptionFormaterList();
        }
        #endregion
        #region �ݒ�C���X�^���X
        /// <summary>
        /// ���ʃ��O�ݒ�
        /// </summary>
        private CommonConfig m_Common;
        /// <summary>
        /// ���ʃ��O�ݒ�
        /// </summary>
        public CommonConfig Common
        {
            get { return this.m_Common; }
        }
        /// <summary>
        /// �R���\�[�����O�ݒ�
        /// </summary>
        private BasicLogConfig m_Console;
        /// <summary>
        /// �R���\�[�����O�ݒ�
        /// </summary>
        public BasicLogConfig Console
        {
            get { return this.m_Console; }
        }
        /// <summary>
        /// �g�����O�ݒ�
        /// </summary>
        private BasicLogConfig m_Extend;
        /// <summary>
        /// �g�����O�ݒ�
        /// </summary>
        public BasicLogConfig Extend
        {
            get { return this.m_Extend; }
        }
        /// <summary>
        /// �t�@�C�����O�ݒ�
        /// </summary>
        private FileLogConfig m_File;
        /// <summary>
        /// �t�@�C�����O�ݒ�
        /// </summary>
        public FileLogConfig File
        {
            get { return this.m_File; }
        }
        /// <summary>
        /// �C�x���g���O�ݒ�
        /// </summary>
        private EventLogConfig m_EventLog;
        /// <summary>
        /// �C�x���g���O�ݒ�
        /// </summary>
        public EventLogConfig EventLog
        {
            get { return this.m_EventLog; }
        }
        /// <summary>
        /// syslog�ݒ�
        /// </summary>
        private SysLogConfig m_Syslog;
        /// <summary>
        /// syslog�ݒ�
        /// </summary>
        public SysLogConfig Syslog
        {
            get { return this.m_Syslog; }
        }
        /// <summary>
        /// �f�t�H���g���O�ݒ�
        /// </summary>
        /// <remarks>
        /// �����w��̂ݗL��
        /// </remarks>
        private BasicLogConfig m_Default;
        /// <summary>
        /// �f�t�H���g���O�ݒ�
        /// </summary>
        /// <remarks>
        /// �����w��̂ݗL��
        /// </remarks>
        public BasicLogConfig Default
        {
            get { return this.m_Default; }
        }
        /// <summary>
        /// ���O���b�Z�[�W��`
        /// </summary>
        public MessagesConfig Message
        {
            get { return this.m_Message; }
        }
        /// <summary>
        /// ���O���b�Z�[�W��`�ύX
        /// </summary>
        public void SetMessageConfig(MessagesConfig Message)
        {
            this.m_Message = Message;
        }
        /// <summary>
        /// ���O���b�Z�[�W��`
        /// </summary>
        private MessagesConfig m_Message = null;
        #endregion
        #region ���[�e�B���e�B
        /// <summary>
        /// �L�^�Ώۂ̃��O���x���̃`�F�b�N
        /// </summary>
        /// <param name="level">���x��</param>
        /// <returns>�L�^�ΏۂȂ�true</returns>
        public bool IsValidLevel(LogLevel level)
        {
            if (m_Console.Enabled)
            {
                if (m_Console.Level <= level)
                    return true;
            }
            if (m_File.Enabled)
            {
                if (m_File.Level <= level)
                    return true;
            }
            if (m_EventLog.Enabled)
            {
                if (m_EventLog.Level <= level)
                    return true;
            }
            if (m_Extend.Enabled)
            {
                if (m_Extend.Level <= level)
                    return true;
            }
            if (m_Syslog.Enabled)
            {
                if (m_Syslog.Level <= level)
                    return true;
            }
            return false;
        }
        #endregion
        #region �g�����W���[��
        /// <summary>
        /// ���O�g�����W���[���N���X�C���X�^���X
        /// </summary>
        /// <remarks>
        /// ExtendLog�N���X�̔h���N���X���`���ݒ肷��
        /// </remarks>
        public ExtendLog ExtendLogInstance = null;
        /// <summary>
        /// ���������W���[��
        /// </summary>
        /// <remarks>
        /// ���O���t�H�[�}�b�g���鏑�������W���[���A�W���@�\�ȊO�̏��������K�v�ȏꍇ��Formatter�N���X���`���ݒ肷��
        /// </remarks>
        public Formatter FormatterInstance = new Formatter();
        /// <summary>
        /// �Í������W���[��
        /// </summary>
        /// <remarks>
        /// DES�ȊO�̈Í����������g�p����ꍇ�́AScrambler�N���X���`���ݒ肷��
        /// </remarks>
        public Scrambler ScramblerInstance = new Scrambler();
        /// <summary>
        /// ���O�t�@�C�����[�e�[�^���W���[��
        /// </summary>
        /// <remarks>
        /// ��{�̃��[�e�[�g�����ȊO���g�p����ꍇFileRotater�N���X���`���ݒ肷��
        /// </remarks>
        public FileRotater FileRotaterInstance = new FileRotater();
        /// <summary>
        /// ��O���������W���[���̈ꗗ
        /// </summary>
        internal Dictionary<Type, ExceptionFormater> ExceptionFormaterList = new Dictionary<Type, ExceptionFormater>();
        /// <summary>
        /// ��O���������W���[���̒ǉ�
        /// </summary>
        /// <param name="ef"></param>
        public void SetExceptionFormater(ExceptionFormater ef)
        {
            if (ExceptionFormaterList.ContainsKey(ef.ExceptionType))
            {   //  ���ɓo�^����Ă���̂Œu��������
                ExceptionFormaterList[ef.ExceptionType] = ef;
            }
            else
            {
                ExceptionFormaterList.Add(ef.ExceptionType, ef);
            }
        }
        /// <summary>
        /// �f�t�H���g��O���������W���[���̃Z�b�g�A�b�v
        /// </summary>
        private void SetupDefaultExceptionFormaterList()
        {
            //  �\�P�b�g��O
            this.SetExceptionFormater(new SocketExceptionFormater());
            //  SQL��O
            this.SetExceptionFormater(new SqlExceptionFormater());
            //  IO��O
            this.SetExceptionFormater(new IOExceptionFormater());
        }
        #endregion
    }
}
