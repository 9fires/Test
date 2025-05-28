using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// �ʐݒ荀�ڋ��ʐݒ荀�ڏ��
    /// </summary>
    [Serializable]
    public class BasicLogConfig
    {
        /// <summary>
        /// �ݒ�t�@�C�����
        /// </summary>
        //  public readonly LoggerConfigElement elm =   null;
        protected readonly bool fConfElm=false;
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="Name">�G�������g�����w��</param>
        /// <param name="e"></param>
        public BasicLogConfig(string Name,LoggerConfigElement e)
        {
            this.fConfElm = true;
            this.Name = Name;
            this.enabled = e.Enabled;
            this.level = LogLevelString(e.Level);
            this.format = e.Format;
            this.scramble = e.scramble;
            this.scrambleKey = e.scrambleKey;
            this.encoding = System.Text.Encoding.GetEncoding(e.Encoding);
            this.MessageMaxLength = e.MessageMaxLength; // 2016/08/23 �C�x���g���O�����`�F�b�N�ǉ�
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="Name">�G�������g�����w��</param>
        public BasicLogConfig(string Name)
        {
            this.Name = Name;
        }
        /// <summary>
        /// �G�������g��
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// �L���E�����ݒ�
        /// </summary>
        public bool Enabled
        {
           get
            {
                lock (this)
                {
                    return (bool)enabled;
                }
            }
            set
            {
                lock (this)
                {
                    this.enabled= value;
                }
            }
        }
        /// <summary>
        /// �L���E�����ݒ�
        /// </summary>
        private bool enabled    = false;
        /// <summary>
        /// ���O���x��
        /// </summary>
        public LogLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level  = value;
            }
        }
        /// <summary>
        /// ���O���x��
        /// </summary>
        private LogLevel level = LogLevel.DEBUG;
        /// <summary>
        /// �����񂩂烍�O���x���ɕϊ�����
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        static public  LogLevel LogLevelString(string level)
        {
            switch (level.ToUpper())
            {
                case "TRACE": return LogLevel.TRACE;
                case "DEBUG": return LogLevel.DEBUG;
                case "INFO": return LogLevel.INFO;
                case "NOTE": return LogLevel.NOTE;
                case "WARN": return LogLevel.WARN;
                case "ERROR": return LogLevel.ERROR;
                case "CRITICAL": return LogLevel.CRIT;
                case "ALERT": return LogLevel.ALERT;
                case "EMERGENCY": return LogLevel.EMERG;
                case "FATAL": return LogLevel.FATAL;
                default:
                    return LogLevel.Undefine;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                this.format =   value;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        private string format = string.Empty;
        /// <summary>
        /// �����G���R�[�f�B���O
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }
        /// <summary>
        /// �����G���R�[�f�B���O
        /// </summary>
        private System.Text.Encoding encoding = System.Text.Encoding.Default;
        /// <summary>
        /// �X�N�����u���̗L��
        /// </summary>
        public bool Scramble
        {
            get
            {
                return this.scramble;
            }
            set
            {
                this.scramble = value;
            }
        }
        /// <summary>
        /// �X�N�����u���̗L��
        /// </summary>
        private bool scramble = false;
        /// <summary>
        /// �X�N�����u���̃L�[
        /// </summary>
        public string ScrambleKey
        {
            get { return this.scrambleKey; }
            set { this.scrambleKey = value;}
        }
        /// <summary>
        /// �X�N�����u���̃L�[
        /// </summary>
        private string scrambleKey = string.Empty;
        /// <summary>
        /// ���b�Z�[�W���������
        /// </summary>
        /// <remarks>
        /// 2016/08/23 �C�x���g���O�����`�F�b�N�ǉ�
        /// </remarks>
        public virtual int MessageMaxLength
        {
            get
            {
                return this.messageMaxLength;
            }
            set
            {
                this.messageMaxLength = value;
            }
        }
        int messageMaxLength = -1;
    }
}
