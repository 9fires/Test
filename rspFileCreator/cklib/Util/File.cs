using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;
namespace cklib.Util
{
	/// <summary>
	/// File �t�@�C������֘A�̃��[�e�B���e�B�B
	/// </summary>
	public static class File
	{
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationA")]
		private	static extern bool	GetVolumeInformation
					(	string	lpRootPathName,
						StringBuilder	lpVolumeNameBuffer,
						int	nVolumeNameSize,
						ref int	lpVolumeSerialNumber,
						ref	int	lpMaximumComponentLength,
						ref	int	lpFileSystemFlags,
						StringBuilder	lpFileSystemNameBuffer,
						int	nFileSystemNameSize);
		[DllImport("kernel32.dll", EntryPoint="CreateFileA")]
		private static extern IntPtr CreateFile(string	path,
												uint AccessMode,
												uint ShareMode,
												uint SecurityAttributes,
												uint CreationDisposition,
												uint FlagsAndAttributes,
												uint TemplateFile);
        /// <summary>
        /// �n���h�������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="CloseHandle")]
		public static extern bool CloseHandle(IntPtr	handle);
        /// <summary>
        /// Win32�G���[�R�[�h�̎擾
        /// </summary>
        /// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="GetLastError")]
		public static extern uint GetLastError();
		[DllImport("kernel32.dll", EntryPoint="DeviceIoControl")]
		private	static extern bool DeviceIoControl(	IntPtr	hDevice,              // �f�o�C�X�A�t�@�C���A�f�B���N�g�������ꂩ�̃n���h��
													uint	dwIoControlCode,       // ���s���铮��̐���R�[�h
													IntPtr	lpInBuffer,           // ���̓f�[�^����������o�b�t�@�ւ̃|�C���^
													uint	nInBufferSize,         // ���̓o�b�t�@�̃o�C�g�P�ʂ̃T�C�Y
													IntPtr	lpOutBuffer,          // �o�̓f�[�^���󂯎��o�b�t�@�ւ̃|�C���^
													uint	nOutBufferSize,        // �o�̓o�b�t�@�̃o�C�g�P�ʂ̃T�C�Y
													IntPtr	lpBytesReturned,     // �o�C�g�����󂯎��ϐ��ւ̃|�C���^
													uint	lpOverlapped    // �񓯊������\���\���̂ւ̃|�C���^
													);
        [DllImport("kernel32.dll", EntryPoint = "GetTempFileName")]
        private extern static uint GetTempFileName( string PathName,
                                                    string PrefixString,
                                                    uint uUnique,
                                                    StringBuilder TempFileName);
        /// <summary>
        /// �ǂݎ��A�N�Z�X
        /// </summary>
		public	static	uint	AccessMode_GENERIC_READ		=	0x80000000;
        /// <summary>
        /// �������݃A�N�Z�X
        /// </summary>
		public	static	uint	AccessMode_GENERIC_WRITE    =	0x40000000;
        /// <summary>
        /// ���s�A�N�Z�X
        /// </summary>
		public	static	uint	AccessMode_GENERIC_EXECUTE	=	0x20000000;
        /// <summary>
        /// ���ׂẴA�N�Z�X
        /// </summary>
		public	static	uint	AccessMode_GENERIC_ALL      =	0x10000000;
        /// <summary>
        /// �ǂݎ�苤�L
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_READ	=	0x00000001;
        /// <summary>
        /// �������݋��L
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_WRITE	=	0x00000002;
        /// <summary>
        /// �폜���L
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_DELETE	=	0x00000004;  
        /// <summary>
        /// �V�����t�@�C�����쐬���܂��B�w�肵���t�@�C�������ɑ��݂��Ă���ꍇ�A���̊֐��͎��s���܂��B
        /// </summary>
		public	static	uint	CREATE_NEW          =	1;
        /// <summary>
        /// �V�����t�@�C�����쐬���܂��B�w�肵���t�@�C�������ɑ��݂��Ă���ꍇ�A���̃t�@�C�����㏑�����A�����̑������������܂��B
        /// </summary>
		public	static	uint	CREATE_ALWAYS       =	2;
        /// <summary>
        /// �t�@�C�����J���܂��B�w�肵���t�@�C�������݂��Ă��Ȃ��ꍇ�A���̊֐��͎��s���܂��B 
        /// </summary>
		public	static	uint	OPEN_EXISTING       =	3;
        /// <summary>
        /// �t�@�C�������݂��Ă���ꍇ�A���̃t�@�C�����J���܂��B�w�肵���t�@�C�������݂��Ă��Ȃ��ꍇ�A���̊֐��� dwCreationDisposition �p�����[�^�� CREATE_NEW ���w�肳��Ă����Ɖ��肵�ĐV�����t�@�C�����쐬���܂��B
        /// </summary>
		public	static	uint	OPEN_ALWAYS         =	4;
        /// <summary>
        /// �t�@�C�����J���A�t�@�C���̃T�C�Y�� 0 �o�C�g�ɂ��܂��B�Ăяo�����v���Z�X�́AdwDesiredAccess �p�����[�^�ŁA���Ȃ��Ƃ� GENERIC_WRITE �A�N�Z�X�����w�肵�Ȃ���΂Ȃ�܂���B�w�肵���t�@�C�������݂��Ă��Ȃ��ꍇ�A���̊֐��͎��s���܂��B
        /// </summary>
		public	static	uint	TRUNCATE_EXISTING   =	5;
        /// <summary>
        /// 	���̃t�@�C���͓ǂݎ���p�ł��B�A�v���P�[�V�����͂��̃t�@�C���̓ǂݎ����s���܂����A�������݂�폜�͂ł��܂���B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_READONLY     =	0x00000001;
        /// <summary>
        /// ���̃t�@�C���͉B���t�@�C���ł��B�ʏ�̃f�B���N�g�����X�e�B���O�ł͕\������܂���B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_HIDDEN       =	0x00000002;
        /// <summary>
        /// ���̃t�@�C���́A�I�y���[�e�B���O�V�X�e���̈ꕔ�A�܂��̓I�y���[�e�B���O�V�X�e����p�̃t�@�C���ł��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_SYSTEM       =	0x00000004;
        /// <summary>
        /// �w�肳�ꂽ�n���h���́A�f�B���N�g���Ɋ֘A���Ă��܂��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_DIRECTORY    =	0x00000010;
        /// <summary>
        /// ���̃t�@�C�����A�[�J�C�u����ׂ��ł��B�A�v���P�[�V�����͂��̑������A
        /// �t�@�C���̃o�b�N�A�b�v��폜�̂��߂̃}�[�N�Ƃ��Ďg���܂��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_ARCHIVE      =	0x00000020;
        /// <summary>
        /// �\��ς݁B�g��Ȃ��ł��������B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_DEVICE       =	0x00000040;
        /// <summary>
        /// ���̃t�@�C���ɓ��ɑ�����ݒ肵�܂���B�P�ƂŎw�肵���ꍇ�ɂ̂݁A���̑����͗L���ł��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_NORMAL       =	0x00000080;
        /// <summary>
        /// ���̃t�@�C���́A�ꎞ�t�@�C���Ƃ��Ďg���Ă��܂��B�t�@�C���V�X�e���́A
        /// �f�[�^���n�[�h�f�B�X�N�̂悤�ȑ�e�ʋL�����u�֏������ޑ���ɁA�����ȃA�N�Z�X���s����悤�A
        /// ���ׂẴf�[�^�����������Ɉێ����邱�Ƃ����݂܂��B�A�v���P�[�V�����́A
        /// �K�v���Ȃ��Ȃ����i�K�ňꎞ�t�@�C���������ɍ폜����ׂ��ł��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_TEMPORARY    =	0x00000100;
        /// <summary>
        /// 	���̃t�@�C���́A�X�p�[�X�t�@�C���i�a�ȃt�@�C���A���g�p�̗̈悪�����A�܂��͓����l�����������t�@�C���j�ł��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_SPARSE_FILE  =	0x00000200;
        /// <summary>
        /// ���̃t�@�C���ɂ́A�ĉ�̓|�C���g���֘A�t�����Ă��܂��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_REPARSE_POINT=	0x00000400;
        /// <summary>
        /// �w�肳�ꂽ�t�@�C���܂��̓f�B���N�g���͈��k����Ă��܂��B�t�@�C���̏ꍇ�A
        /// �t�@�C�����̑S�f�[�^�����k����Ă��邱�Ƃ��Ӗ����܂��B�f�B���N�g���̏ꍇ�A
        /// ���̃f�B���N�g�����ɐV�����쐬�����t�@�C���܂��̓T�u�f�B���N�g�����A
        /// ����ň��k��ԂɂȂ邱�Ƃ��Ӗ����܂��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_COMPRESSED   =	0x00000800;
        /// <summary>
        /// Windows 2000�F���̃t�@�C���̃f�[�^�́A�����ɂ͗��p�ł��܂���B
        /// ���̑����́A���̃t�@�C���̃f�[�^���I�t���C���L�����u�֕����I�Ɉړ����ꂽ���Ƃ������܂��B
        /// ���̑����́AWindows 2000 �̊K�w�L���Ǘ��\�t�g�E�F�A�ł���u�����[�g�L����v�����p������̂ł��B
        /// �A�v���P�[�V�����́A�C�ӂɂ��̑�����ύX����ׂ��ł͂���܂���B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_OFFLINE      =	0x00001000;
        /// <summary>
        /// 	Windows 2000�F���̃t�@�C�����A�u�C���f�b�N�X�T�[�r�X�v�̑Ώۂɂ��܂���B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_NOT_CONTENT_INDEXED=	0x00002000;
        /// <summary>
        /// ���̃t�@�C���܂��̓f�B���N�g�����Í�������悤�w�����܂��B
        /// �t�@�C���ɑ΂��Ďw�肵���ꍇ�A���̃t�@�C�����̂��ׂẴf�[�^���Í������邱�Ƃ��Ӗ����܂��B
        /// �f�B���N�g���ɑ΂��Ďw�������ꍇ�A���̒��ɐV�����쐬�����t�@�C���ƃT�u�f�B���N�g���ɑ΂��āA
        /// ����ňÍ������s�����Ƃ��Ӗ����܂��BFILE_ATTRIBUTE_SYSTEM �Ƌ��Ɏw�肷��ƁA
        /// FILE_ATTRIBUTE_ENCRYPTED �t���O�͖����ɂȂ�܂��B
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_ENCRYPTED    =	0x00004000;


		/// <summary>
		/// �{�����[�����̎擾
		/// </summary>
		/// <param name="drive">�h���C�u�������������� ex)c:\</param>
		/// <param name="VolumeName">�{�����[�����̎󂯎��o�b�t�@</param>
		/// <param name="VolumeSerialNumber">�{�����[���V���A���ԍ�</param>
		/// <param name="MaximumComponentLength">�t�@�C�����̏��</param>
		/// <param name="FileSystemFlags">�t�@�C���V�X�e���t���O</param>
		/// <param name="FileSystemName">�t�@�C���V�X�e����</param>
		/// <returns></returns>
		public	static	bool	GetVolumeInformation(   string	drive,
													    out string VolumeName,
													    out int VolumeSerialNumber,
													    out int MaximumComponentLength,
													    out int	FileSystemFlags,
													    out	string	FileSystemName)
		{
            //  �����l���
            VolumeName = string.Empty;
            VolumeSerialNumber = 0;
            MaximumComponentLength = 0;
            FileSystemFlags = 0;
            FileSystemName = string.Empty;

			int	nVolumeNameSize=0,lpVolumeSerialNumber=0,lpFileSystemFlags=0,nFileSystemNameSize=0,lpMaximumComponentLength=0;
			System.Text.StringBuilder strVolumeName	= new System.Text.StringBuilder(4096);
			nVolumeNameSize	=	4096;
			System.Text.StringBuilder strFileSystemName	= new System.Text.StringBuilder(4096);
			nFileSystemNameSize=	4096;
			if	(GetVolumeInformation(	drive,
										strVolumeName,
										nVolumeNameSize,
										ref lpVolumeSerialNumber,
										ref lpMaximumComponentLength,
										ref	lpFileSystemFlags,
										strFileSystemName,
										nFileSystemNameSize))
			{
				VolumeName	=	strVolumeName.ToString();
				VolumeSerialNumber	=	lpVolumeSerialNumber;
				MaximumComponentLength=	lpMaximumComponentLength;
				FileSystemFlags	=	lpFileSystemFlags;
				FileSystemName	=	strFileSystemName.ToString();
				return	true;
			}
			return	false;
		}
        /// <summary>
        /// CreateFile-�t�@�C�����̃I�[�v��
        /// </summary>
        /// <param name="path">�t�@�C�����̃p�X</param>
        /// <param name="AccessMode"></param>
        /// <param name="ShareMode"></param>
        /// <param name="CreationDisposition"></param>
        /// <param name="FlagsAndAttributes"></param>
        /// <returns></returns>
		public	static	IntPtr CreateFile(	string	path,
											uint AccessMode,
											uint ShareMode,
											uint CreationDisposition,
											uint FlagsAndAttributes)
		{
			return	CreateFile(path,AccessMode,ShareMode,0,CreationDisposition,FlagsAndAttributes,0);
		}

		/// <summary>
		/// ���k�t�@�C�����T�|�[�g���Ă��邩�`�F�b�N����
		/// </summary>
		/// <param name="drive">�h���C�u���^�[</param>
		/// <returns></returns>
		public	static	bool	IsSupportCompressFile(string	drive)
		{
			string VolumeName;
			int VolumeSerialNumber,MaximumComponentLength,FileSystemFlags;
			string	FileSystemName;
			if	(GetVolumeInformation(	drive,
										out	VolumeName,
										out VolumeSerialNumber,
										out	MaximumComponentLength,
										out FileSystemFlags,
										out	FileSystemName)	)
			{
				if	((FileSystemFlags&0x00000010)==0x00000010)
					return	true;
			}
			return	false;
		}
        /// <summary>
        /// �t�@�C���̈��k�����w��
        /// </summary>
        /// <param name="file">�t�@�C���p�X</param>
        /// <returns></returns>
        public	static	bool	FileCompress(string	file)
		{
			IntPtr	hndle	=	CreateFile(file,
											AccessMode_GENERIC_ALL,
											ShareMode_FILE_SHARE_READ|
											ShareMode_FILE_SHARE_WRITE,
											OPEN_EXISTING,
											FILE_ATTRIBUTE_NORMAL);
			if	(GetLastError()!=0)
			{
				return	false;
			}
				bool	result	=false;
			unsafe
			{
				ushort commode	=	0x0001;
				uint	retb	=	0;
				IntPtr	lpcommode	=	new	IntPtr(&commode);
				IntPtr	lpretb	=	new	IntPtr(&retb);
				if	(DeviceIoControl(hndle,
					0x00000009<<16|3<<14|16<<2,
					lpcommode,2,IntPtr.Zero,0,lpretb,0))
				{
					result	=	true;
				}
			}
			CloseHandle(hndle);
			return	result;
		}
        /// <summary>
        /// �t�@�C���p�X�̐��K��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public	static	string	ParsePathToDrive(string	path)
		{
			int	pos	=path.IndexOf(":");
			if	(pos!=-1)
			{
				return	path.Substring(0,pos)+":\\";
			}
			else
			{
				return	path+":\\";
			}
		}
        /// <summary>
        /// �e���|�����t�@�C������
        /// </summary>
        /// <remarks>
        /// �ڍׂ�Win32SDK GetTempFileName�̋L�q���Q��
        /// </remarks>
        /// <param name="Path">�f�B���N�g����</param>
        /// <param name="Prefix">�f�B���N�g����</param>
        /// <param name="Unique">����</param>
        /// <returns>�������ꂽ�t�@�C���̃t�@�C����</returns>
        public static string GetTempFileName(string Path, string Prefix, uint Unique)
        {
            StringBuilder strb = new StringBuilder(1024);
            GetTempFileName(Path, Prefix, Unique, strb);
            return strb.ToString();
        }
    }
}
