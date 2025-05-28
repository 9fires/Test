using System;
using cklib;
namespace cklib.Util
{
	/// <summary>
	/// ConfigErrorEventArgs<br/>
	/// �ݒ���擾�G���[�������C�x���g���
	/// </summary>
	public class ConfigErrorEventArgs:EventArgs
	{
		/// <summary>
		/// ��O���
		/// </summary>
		private	Exception	errExp=null;
		/// <summary>
		/// ��O���
		/// </summary>
		public	Exception	Exception
		{
			get	{	return	this.errExp;	}
		}
		/// <summary>
		/// �擾�f�[�^��
		/// </summary>
		private	string	configName=string.Empty;
		/// <summary>
		/// �擾�f�[�^��
		/// </summary>
		public	string	ConfigName
		{
			get	{	return	configName;	}
		}
		/// <summary>
		/// �f�t�H���g�f�[�^�l
		/// </summary>
		private	string	defaultValue=string.Empty;
		/// <summary>
		/// �f�t�H���g�f�[�^�l
		/// </summary>
		public	string	DefaultValue
		{
			get	{	return	defaultValue;	}
		}
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public ConfigErrorEventArgs(string	name,string	defval,Exception	exp)
		{
			this.configName		=	name;
			this.defaultValue	=	defval;
			this.errExp			=	exp;
		}
	}
}
