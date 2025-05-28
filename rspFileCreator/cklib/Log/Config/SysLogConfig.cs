using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log.Config
{
    /// <summary>
    /// �ʐݒ荀�ڋ��ʐݒ荀�ڏ��
    /// </summary>
    [Serializable]
    public class SysLogConfig : BasicLogConfig
    {
        /// <summary>
        /// �R���X�g���N�^
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
                #region Facility������ϊ�
                switch (e.Facility.ToLower())
                {
                    case "auth":
                    case "security":    // �F�؃T�[�r�X�̃��b�Z�[�W�i���݂�authpriv����������Ă���j
                        this.facility = 4; break;
                    case "authpriv":    // �F�؃T�[�r�X�i�J�e�S����auth�Ɠ����Bauth�Ƃ͏o�͌��ʂ��قȂ�j 
                        this.facility = 10; break;
                    case "cron":        // cron�̃��b�Z�[�W 
                        this.facility = 3; break;
                    case "daemon":      // �f�[�����̃��b�Z�[�W 
                        this.facility = 3; break;
                    case "kern":        // �J�[�l���̃��b�Z�[�W 
                        this.facility = 0; break;
                    case "lpr":         // �v�����^�T�[�r�X�̃��b�Z�[�W 
                        this.facility = 6; break;
                    case "mail":        // ���[���T�[�r�X�̃��b�Z�[�W 
                        this.facility = 2; break;
                    case "news":        //  �j���[�X�T�[�r�X�̃��b�Z�[�W 
                        this.facility = 7; break;
                    case "syslog":      // syslog�̃��b�Z�[�W 
                        this.facility = 5; break;
                    case "user":        // ���[�U�[�v���Z�X�̃��b�Z�[�W 
                        this.facility = 1; break;
                    case "uucp":        // uucp�]�����s���v���O�����̃��b�Z�[�W 
                        this.facility = 8; break;
                    case "local0":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 16; break;
                    case "local1":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 17; break;
                    case "local2":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 18; break;
                    case "local3":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 19; break;
                    case "local4":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 20; break;
                    case "local5":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 21; break;
                    case "local6":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 22; break;
                    case "local7":      // �A�v���P�[�V�����Ɉˑ����� 
                        this.facility = 23; break;
                    default:
                        this.facility = 16; break;
                }
                #endregion
            }
            this.ownHost = e.hostname;
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public SysLogConfig()
            : base(LoggerConfig.SyslogElementCollectionName)
        { }

        /// <summary>
        /// syslog�z�X�g
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
        /// syslog�z�X�g
        /// </summary>
        private string sysloghost = "localhost";
        /// <summary>
        /// syslog�|�[�g�ԍ�
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
        /// syslog�|�[�g�ԍ�
        /// </summary>
        private int port = 514;
        /// <summary>
        /// syslog �t�@�V���e�B
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
        /// syslog �t�@�V���e�B
        /// </summary>
        private int facility = 16;
        /// <summary>
        /// ���M���z�X�g��
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
        /// ���M���z�X�g��
        /// </summary>
        private string ownHost = "localhost";
    }
}
