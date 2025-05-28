using System;
using cklib;

namespace cklib.Log
{
	/// <summary>
	/// LogData �̊T�v�̐����ł��B
	/// </summary>
	public class LogData
	{
		/// <summary>
		/// ���O���۔�������
		/// </summary>
		public	DateTime	Time=	DateTime.Now;
		/// <summary>
		/// ���O���x��
		/// </summary>
		public	LogLevel	level=	LogLevel.DEBUG;
		/// <summary>
		/// ���O�R�[�h
		/// </summary>
		public	int			Code	=	0;
		/// <summary>
		/// ���O���b�Z�[�W
		/// </summary>
		public	string		Message	=	string.Empty;
		/// <summary>
		/// �\�[�X�t�@�C�����
		/// </summary>
		public	string		SourceName	=	string.Empty;
        /// <summary>
        /// �N���X��
        /// </summary>
        public  string      ClassName   =   string.Empty;
        /// <summary>
        /// ���\�b�h��
        /// </summary>
        public  string      Method      =   string.Empty;
		/// <summary>
		/// �\�[�X�t�@�C�����
		/// </summary>
		public	string		SourceFile	=	string.Empty;
		/// <summary>
		/// �\�[�X�t�@�C���s
		/// </summary>
		public	int			SourceLine	=	0;
		/// <summary>
		/// ��O���
		/// </summary>
		public	Exception	Exp		=	null;
        /// <summary>
        /// �ʐM�g���[�X�o�C�i���f�[�^
        /// </summary>
        public  object      TraceBuffer = null;
        /// <summary>
        /// �ʐM�g���[�X�o�C�i���f�[�^��
        /// </summary>
        public  int         TraceBufferLength = 0;
        /// <summary>
        /// ���O�L�^�X���b�hID
        /// </summary>
        public  int         ThreadID=0;
	}
}
