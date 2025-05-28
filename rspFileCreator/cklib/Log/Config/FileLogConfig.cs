using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace cklib.Log.Config
{
    /// <summary>
    /// �ʐݒ荀�ڋ��ʐݒ荀�ڏ��
    /// </summary>
    [Serializable]
    public class FileLogConfig : BasicLogConfig
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="e"></param>
        public FileLogConfig(LoggerConfigElement e)
            : base(LoggerConfig.FileElementCollectionName, e)
        {
            this.Path   = e.Path;
            this.FileName = e.FileName;
            this.fileLock = e.FileLock;
            this.fileLockWaitTime = e.FileLockWaitTime;
            this.fileLockTryLimit = e.FileLockTryLimit;
            this.mutexLock = e.MutexLock;
            this.mutexLockWaitTime = e.MutexLockWaitTime;
            this.compress = e.Compress;
            this.rotateDays = e.RotateDays;
            this.SetRotateSize(e.RotateSize);
            this.rotateFileName = e.RotateFileName;
            this.zipRotate = e.ZipRotate;
            this.zipRotateDays = cklib.Util.String.ToInt(e.ZipRotateDays, -1);
            this.zipRotatePassword = e.ZipRotatePassword;
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public FileLogConfig()
            : base(LoggerConfig.FileElementCollectionName)
        {}
        /// <summary>
        /// �o�̓p�X
        /// </summary>
        public string Path
        {
            get
            {
                return  this.logPath;
            }
            set
            {
                string path = Environment.ExpandEnvironmentVariables(value);
                if (path.Length == 0)
                {	//	�ݒ薳��
                    Assembly asm= Assembly.GetExecutingAssembly();
                    path = System.IO.Path.GetDirectoryName(asm.Location);
                }
                if (path[0] == '.')
                {	//	���΃f�B���N�g��
                    Assembly asm = Assembly.GetExecutingAssembly();
                    path = System.IO.Path.GetDirectoryName(asm.Location) + "\\" + path;
                }
                if (!path.EndsWith("\\"))
                {
                    path += "\\";   
                }
                this.logPath = path;
            }
        }
        /// <summary>
        /// �o�̓p�X
        /// </summary>
        private string logPath = string.Empty;
        /// <summary>
        /// �o�̓t�@�C����
        /// </summary>
        public string FileName
        {
            get
            {
                return this.logFileName;
            }
            set
            {
                this.logFileName = value;
            }
        }
        /// <summary>
        /// �o�̓t�@�C����
        /// </summary>
        private string logFileName = string.Empty;
        /// <summary>
        /// ���O�t�@�C�����b�N
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C�����������݃��b�N����<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
        /// </remarks>
        public bool FileLock
        {
            get
            {
                return this.fileLock;
            }
            set
            {
                this.fileLock = value;
            }
        }
        /// <summary>
        /// ���O�t�@�C�����b�N
        /// </summary>
        private bool fileLock = false;
        /// <summary>
        /// ���O�t�@�C�����b�N���g���C�C���^�[�o��(msec
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C���I�[�v�����̔r�����b�N�ɂ�郊�g���C����<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
        /// </remarks>
        public int FileLockWaitTime
        {
            get
            {
                return this.fileLockWaitTime;
            }
            set
            {
                this.fileLockWaitTime = value;
            }
        }
        /// <summary>
        /// ���O�t�@�C�����b�N���g���C�C���^�[�o��(msec
        /// </summary>
        private int fileLockWaitTime = 1000;
        /// <summary>
        /// ���O�t�@�C�����b�N���g���C��
        /// </summary>
        /// <remarks>
        /// ���O�t�@�C���I�[�v�����̔r�����b�N�ɂ�郊�g���C���s��<br/>
        /// �}���`�v���Z�X�œ��ꃍ�O�t�@�C���ɏ������ލۗL���ɂ���
        /// </remarks>
        public int FileLockTryLimit
        {
            get
            {
                return this.fileLockTryLimit;
            }
            set
            {
                this.fileLockTryLimit = value;
            }
        }
        private int fileLockTryLimit = -1;
        /// <summary>
        /// ���O�r������Mutex�L��
        /// </summary>
        public bool MutexLockEnable
        {
            get
            {
                if (this.MutexLock.Length != 0)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// ���O�r������Mutex��
        /// </summary>
        /// <remarks>
        /// ���O�������݂�Mutex�ɂ��r�����䂷��B�󕶎��̏ꍇ�͖���
        /// </remarks>
        public string MutexLock
        {
            get
            {
                return this.mutexLock;
            }
            set
            {
                this.mutexLock = value;
            }
        }
        private string mutexLock = string.Empty;
        /// <summary>
        /// ���O�r������Mutex�ҋ@����(msec)
        /// </summary>
        /// <remarks>
        /// ���O�r������Mutex�����b�N�o����܂ł̑ҋ@����<br/>
        /// �^�C���I�[�o�[�����ꍇ�́A���O�������݂͎��s����B
        /// </remarks>
        public int MutexLockWaitTime
        {
            get
            {
                return this.mutexLockWaitTime;
            }
            set
            {
                this.mutexLockWaitTime = value;
            }
        }
        private int mutexLockWaitTime = -1;
        /// <summary>
        /// ���O�����k�t�@�C���ɂ���
        /// </summary>
        public bool Compress
        {
            get
            {
                return this.compress;
            }
            set
            {
                this.compress = value;
            }
        }
        /// <summary>
        /// ���O�����k�t�@�C���ɂ���
        /// </summary>
        private bool compress = false;

        /// <summary>
        /// ���O���[�e�V�����ۑ�����
        /// </summary>
        public int RotateDays
        {
            get
            {
                return this.rotateDays;
            }
            set
            {
                this.rotateDays = value;
            }
        }
        /// <summary>
        /// ���O���[�e�V�����ۑ�����
        /// </summary>
        private int rotateDays = -1;
        /// <summary>
        /// ���O���[�e�V�����T�C�Y�w��
        /// </summary>
        /// <param name="str">���[�e�[�V�����T�C�Y�𕶎���Ŏw�肷��<bf/>K/M/G�̒P�ʎw���</param>
        public void SetRotateSize(string str)
        {
            if (!str.StartsWith("-"))
            {
                long x = 1;
                if (str.Length > 1)
                {
                    string xx = str.Substring(str.Length - 1, 1);
                    switch (xx.ToUpper())
                    {
                        case "K": x = 1024; break;
                        case "M": x = 1024 * 1024; break;
                        case "G": x = 1024 * 1024 * 1024; break;
                    }
                    str = str.Substring(0, str.Length - 1);
                }
                decimal value = Decimal.Parse(str) * x;
                this.rotateSize = Decimal.ToInt64(value);
            }
            this.rotateSizeStr = str;
        }
        /// <summary>
        /// ���O���[�e�V�����T�C�Y�w��
        /// </summary>
        private string rotateSizeStr = "-1";

        /// <summary>
        /// ���O���[�e�V�����T�C�Y�w��
        /// </summary>
        public long RotateSize
        {
            get
            {
                return this.rotateSize;
            }
            set
            {
                this.rotateSize = value;
            }
        }
        /// <summary>
        /// ���O���[�e�V�����T�C�Y�w��
        /// </summary>
        private long rotateSize = -1;
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
        public string RotateFileName
        {
            get
            {
                return this.rotateFileName;
            }
            set
            {
                this.rotateFileName = value;
            }
        }
        /// <summary>
        /// ���[�e�[�g���O�t�@�C����
        /// </summary>
        private string rotateFileName = string.Empty;
        /// <summary>
        /// ZIP���[�e�V����
        /// </summary>
        public bool ZipRotate
        {
            get
            {
                return this.zipRotate;
            }
            set
            {
                this.zipRotate = value;
            }
        }
        /// <summary>
        /// ZIP���[�e�V����
        /// </summary>
        private bool zipRotate=false;
        /// <summary>
        /// ZIP���[�e�V�������{����
        /// </summary>
        public int ZipRotateDays
        {
            get
            {
                return this.zipRotateDays;
            }
            set
            {
                this.zipRotateDays = value;
            }
        }
        /// <summary>
        /// ZIP���[�e�V�������{����
        /// </summary>
        private int zipRotateDays = -1;
        /// <summary>
        /// ZIP���[�e�V�����p�X���[�h
        /// </summary>
        public string ZipRotatePassword
        {
            get
            {
                return this.zipRotatePassword;
            }
            set
            {
                this.zipRotatePassword = value;
            }
        }
        /// <summary>
        /// ZIP���[�e�V�����p�X���[�h
        /// </summary>
        private string zipRotatePassword = string.Empty;

    }
}
