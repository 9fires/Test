using System;
using System.Xml;
namespace cklib.Util
{
	/// <summary>
	/// ConfigXml<br/>
	/// XML�`���ݒ�t�@�C���p�A�N�Z�X���[�e�B���e�B
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class ConfigXml:IDisposable
	{
		/// <summary>
		/// XML�h�L�������g�C���X�^���X
		/// </summary>
		protected   XmlDocument        XmlDoc;
		/// <summary>
		/// �t�@�C���p�X
		/// </summary>
		protected	string	FilePath;
		/// <summary>
		/// �ݒ�l�X�V�t���O
		/// </summary>
		protected	bool	fUpdate=false;
		/// <summary>
		/// �ݒ�X�V�t���O
		/// </summary>
		public	bool	IsUpdate
		{
			get	{	return	this.fUpdate;	}
		}
		/// <summary>
		/// �f�t�H���g�R���X�g���N�^
		/// </summary>
		public ConfigXml()
		{
			XmlDoc	=	new	XmlDocument();
		}
		/// <summary>
		/// �R���X�g���N�^	�t�@�C���w��
		/// </summary>
		/// <param name="FilePath">XML�t�@�C���p�X</param>
		public	ConfigXml(string	FilePath)
		{
			XmlDoc	=	new	XmlDocument();
			this.Load(FilePath);
		}
		/// <summary>
		/// �t�@�C���̃��[�h
		/// </summary>
		/// <param name="FilePath">XML�t�@�C���p�X</param>
		public	void	Load(string	FilePath)
		{
			fUpdate	=	false;
			this.FilePath	=	FilePath;
			XmlDoc.Load(FilePath);
		}
		/// <summary>
		/// �t�@�C���̕ۑ�
		/// </summary>
		public	void	Save()
		{
			Save(this.FilePath);
		}
		/// <summary>
		/// �t�@�C���̕ۑ�
		/// </summary>
		public	void	Save(string FilePath)
		{
			if	(fUpdate)
			{
				XmlDoc.Save(this.FilePath);
				fUpdate	=	false;
			}
		}
		/// <summary>
		/// �f�B�X�g���N�^
		/// </summary>
		~ConfigXml()
		{
			Dispose(false);
		}
		#region IDisposable �����o
		/// <summary>
		/// Dispose�����t���O
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Dispose�����̎���
		/// Que�ƃC�x���g�I�u�W�F�N�g��j������
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Dispose�����̎���
		/// </summary>
		/// <param name="disposing">�蓮�J�����f�B�X�g���N�^���̎���</param>
		private void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
				}
			}
			disposed = true;     
		}

		#endregion

		/// <summary>
		/// XPath�ɂ��I�u�W�F�N�g�̎Q��
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <returns>�擾���ꂽ�I�u�W�F�N�g</returns>
		public XmlNode Select(string path)
		{
			return  XmlDoc.SelectSingleNode(path);
		}
		/// <summary>
		/// XPath�ɂ��I�u�W�F�N�g�̎Q��
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <returns>�擾���ꂽ�I�u�W�F�N�g</returns>
		public XmlNodeList Selects(string path)
		{
			return  XmlDoc.SelectNodes(path);
		}
		/// <summary>
		/// XPath�ɂ��I�u�W�F�N�g�̍X�V
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="val">�o�^����f�[�^</param>
		public void	Select(string path,object	val)
		{
			XmlNode	node	=	XmlDoc.SelectSingleNode(path);
			if	(node!=null)
			{
				node.InnerText	=	val.ToString();
				fUpdate	=	true;
			}
			else
			{
				SelectAdd(path,val);
			}
		}
		/// <summary>
		/// XPath�ɂ��I�u�W�F�N�g�̍X�V
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="val">�o�^����f�[�^</param>
		public bool	SelectUpdate(string path,object	val)
		{
			XmlNode	node	=	XmlDoc.SelectSingleNode(path);
			if	(node!=null)
			{
				node.InnerText	=	val.ToString();
				fUpdate	=	true;
				return	true;
			}
			else
			{
				return	false;
			}
		}
		/// <summary>
		/// XPath�ɂ��I�u�W�F�N�g�̒ǉ�
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="val">�o�^����f�[�^</param>
		public void SelectAdd(string path,object	val)
		{
			XmlNode	node	=	XmlDoc.SelectSingleNode(path);
			string[]	paths	=	path.Split("/".ToCharArray());
			string	cpath,prepath;
			int	i;
			cpath	=	"/";
			prepath	=	"/";
			for	(i=0;i<paths.Length-1;i++)
			{
				if	(paths[i].Length==0)
					continue;
				cpath	+=	paths[i];
				node	=	XmlDoc.SelectSingleNode(cpath);
				if	(node==null)
				{
					break;
				}
				else
				{
					prepath	=	cpath;
					cpath+="/";
				}
			}
			if	(prepath.Length!=0)
				node	=	XmlDoc.SelectSingleNode(prepath);
			else
				node	=	XmlDoc.SelectSingleNode("/");
			for	(;i<paths.Length;i++)
			{
				XmlElement	elm	=	XmlDoc.CreateElement(paths[i]);
				node.AppendChild(elm);
				node	=	elm;
			}
			node.InnerText	=	val.ToString();
			fUpdate	=	true;
		}
		/// <summary>
		/// XPath�ɂ�镶����̎擾
		/// </summary>
		public string SelectString(string path)
		{
			return  this.Select(path).InnerText;
		}
		/// <summary>
		/// XPath�ɂ�镶����̎擾(�z��A�N�Z�X)
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="idx">�Y����</param>
		/// <returns>�擾���ꂽ������</returns>
		public string SelectString(string path,int idx)
		{
			return  this.Selects(path)[idx].InnerText;
		}
		/// <summary>
		/// XPath�ɂ�镶����̎擾
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="DefaultValue">�f�[�^���Ȃ��ꍇ�̃f�B�t�H���g�l</param>
		/// <returns>�擾���ꂽ������</returns>
		public string SelectString(string path, string DefaultValue)
		{
			try
			{
				return  this.SelectString(path);
			}
			catch
			{
				return  DefaultValue;
			}
		}
		/// <summary>
		///  XPath�ɂ�镶����̎擾(�z��A�N�Z�X)
		/// </summary>
		/// <returns></returns>
		/// <param name="path">XPath�w��</param>
		/// <param name="idx">�Y����</param>
		/// <param name="DefaultValue">�f�[�^���Ȃ��ꍇ�̃f�B�t�H���g�l</param>
		/// <returns>�擾���ꂽ������</returns>
		public string SelectString(string path,int idx,string DefaultValue)
		{
			try
			{
				return  this.SelectString(path,idx);
			}
			catch
			{
				return  DefaultValue;
			}
		}
		/// <summary>
		/// XPath�ɂ�鐔�l(int)�̎擾
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <returns>�擾���ꂽ���l</returns>
		public int SelectInt(string path)
		{
			return	int.Parse(this.SelectString(path));
		}
		/// <summary>
		/// XPath�ɂ�鐔�l(int)�̎擾
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="DefaultValue">�f�[�^���Ȃ��ꍇ�̃f�B�t�H���g�l</param>
		/// <returns>�擾���ꂽ���l</returns>
		public int SelectInt(string path,int DefaultValue)
		{
			try
			{
				return	int.Parse(this.SelectString(path));
			}
			catch
			{
				return	DefaultValue;
			}
		}
		/// <summary>
		/// XPath�ɂ�鐔�l�̎擾(�z��A�N�Z�X)
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <param name="idx">�Y����</param>
		/// <param name="DefaultValue">�f�[�^���Ȃ��ꍇ�̃f�B�t�H���g�l</param>
		/// <returns>�擾���ꂽ������</returns>
		public int SelectInt(string path,int idx,int DefaultValue)
		{
			try
			{
				return	int.Parse(this.Selects(path)[idx].InnerText);
			}
			catch
			{
				return	DefaultValue;
			}
		}
		/// <summary>
		/// XPath�ɂ��Ŏw�肵���G�������g�̐��̎擾
		/// </summary>
		/// <param name="path">XPath�w��</param>
		/// <returns>�G�������g�̐�</returns>
		public	int	Count(string	path)
		{
			return	this.Selects(path).Count;
		} 
	}
}
