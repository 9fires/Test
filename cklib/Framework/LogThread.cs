using System;
using System.IO;
using System.Text;
using cklib.Framework.IPC;
namespace cklib.Framework
{
	/// <summary>
	/// �������[�e�[�g�̊ȈՃ��O�X���b�h�N���X
	/// </summary>
	public class LogThread:AppThread
	{
		/// <summary>
		/// ���O�������݃p�X
		/// </summary>
		private	string	path	=	string.Empty;
		/// <summary>
		/// ���O�������݃p�X
		/// </summary>
		public	string	Path
		{
			get	{	return	path;	}
			set	
			{	
				path	=	value;	
				if	(path.Substring(Path.Length-1,1)!="\\")
				{
					path	+=	"\\";
				}
			}
		}
		/// <summary>
		/// ���O�t�@�C���������݃X�g���[��
		/// </summary>
		private	System.IO.FileStream	LogFile=null;
		/// <summary>
		/// ���O�t�@�C����
		/// </summary>
		private	string	LogFileName		=	string.Empty;
		/// <summary>
		/// ���O�t�@�C���������p�L�[��
		/// </summary>
		private	string	LogKeyName		=	string.Empty;
		/// <summary>
		/// ���O�t�@�C���������p�L�[��
		/// </summary>
		public	string	KeyName
		{
			get	{	return	LogKeyName;	}
			set	{	LogKeyName	=	value;	}
		}
        /// <summary>
        /// ���O�t�@�C���̕����G���R�[�h���
        /// </summary>
        public  Encoding Encoding = Encoding.GetEncoding("Shift_JIS");
		/// <summary>
		/// �t�@�C���������ݗL���t���O
		/// </summary>
		private	bool	fLogAfter=false;
		/// <summary>
		///	���O�f�[�^�\����
		/// </summary>
		private	struct	LogInfo
		{
			public	DateTime	LogDate;
			public	String		LogData;
		}
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public LogThread()
		{
			path		=	string.Empty;
			LogFileName	=	string.Empty;
			LogKeyName	=	string.Empty;
		}
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		/// <param name="path">���O�������݃f�B���N�g���p�X</param>
		/// <param name="key">���O�t�@�C���擪������</param>
		public LogThread(String	path,String	key)
		{
			this.Path	=	path;
			LogFileName	=	string.Empty;
			LogKeyName	=	key;
		}
		/// <summary>
		/// �X���b�h�̏I������
		/// </summary>
		/// <returns></returns>
		protected	override bool	ExitInstance()
		{
			CloseLog();
			return	true;
		}
        #region ��O�����̃I�[�o�[���C�h
        /// <summary>
        /// �ҋ@���̗�O�������̏���
        /// </summary>
        /// <returns></returns>
        protected override int WaitError(Exception e)
        {
            return -1;
        }
        /// <summary>
        /// �C�x���g�������̗�O����������
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool EventError(Exception e)
        {
            return false;
        }
        /// <summary>
        /// catch����Ă��Ȃ���O����������
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override void ThreadError(Exception e)
        {
            return;
        }
        #endregion
		/// <summary>
		/// �ҋ@�^�C���A�E�g���Ԃ��~���b�ŕԂ�
		/// </summary>
		/// <returns></returns>
		protected	override	int	GetWaitTime()
		{
			if	(fLogAfter)
			{
				return	500;
			}
			return	-1;
		}
		/// <summary>
		/// �C�x���g�����^�C���A�E�g���̏���
		/// </summary>
		/// <returns></returns>
		protected	override	bool	EventTimeout()
		{
			CloseLog();
			return	true;
		}
		/// <summary>
		/// ���O�������݃C�x���g
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	override	bool	EventData(EventDataType ed)
		{
			LogInfo	li	=	(LogInfo)ed.EventData;
			if	(OpenLog(li.LogDate))
			{
				try
				{
					byte[]	sjis	=	this.Encoding.GetBytes(
						li.LogDate.ToString("yyyy/MM/dd HH:mm:ss.")+li.LogDate.Millisecond.ToString("000")+" "+li.LogData
						);
					LogFile.Write(sjis,0,sjis.Length);
					fLogAfter	=	true;
				}
				catch	//(System.Exception exp)
				{
				}
			}
			return	true;
		}
		/// <summary>
		/// ���O�C�x���g
		/// </summary>
		/// <param name="ldate">���O����</param>
		/// <param name="msg">���O���b�Z�[�W</param>
		/// <returns></returns>
		public	virtual	bool	Loging(System.DateTime ldate,String	msg)
		{
			LogInfo	li	=	new	LogInfo();
			li.LogDate	=	ldate;
			li.LogData	=	msg;
			return	this.IPCPut(EventCode.Data,li);
		}
		/// <summary>
		/// ���O�C�x���g
		/// </summary>
		/// <param name="msg">���O���b�Z�[�W</param>
		/// <returns></returns>
		public	virtual	bool	Loging(String	msg)
		{
			return	this.Loging(DateTime.Now,msg);
		}
		/// <summary>
		/// ���O�t�@�C���I�[�v��
		/// </summary>
		/// <param name="ldate">�t�@�C�����t</param>
		/// <returns></returns>
		private	bool	OpenLog(DateTime	ldate)
		{
			String	fnam	=	GenLogFileName(ldate);
			if	(fnam!=LogFileName)
			{
				CloseLog();
				bool	ret=false;
				try
				{
					LogFile	=	System.IO.File.Open(fnam,
													System.IO.FileMode.Append,
													System.IO.FileAccess.Write,
													System.IO.FileShare.ReadWrite);	
					ret	=	true;
				}
				catch	//(System.Exception	exp)
				{
					LogFile	=null;
				}
				return	ret;
			}
			return	true;
		}
		/// <summary>
		/// �t�@�C�������
		/// </summary>
		private	void	CloseLog()
		{
			if	(LogFile!=null)
			{
				LogFile.Close();
				LogFile	=	null;
			}
		}
		/// <summary>
		/// ���O�t�@�C��������
		/// </summary>
		/// <param name="ldate"></param>
		/// <returns></returns>
		private	String	GenLogFileName(DateTime	ldate)
		{
			return	Path+LogKeyName+ldate.ToString("yyyyMMdd")+".log";
		}
	}
}
