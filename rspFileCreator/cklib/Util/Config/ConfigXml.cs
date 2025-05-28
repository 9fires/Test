using System;
using System.Xml;
namespace cklib.Util
{
	/// <summary>
	/// ConfigXml<br/>
	/// XML形式設定ファイル用アクセスユーティリティ
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class ConfigXml:IDisposable
	{
		/// <summary>
		/// XMLドキュメントインスタンス
		/// </summary>
		protected   XmlDocument        XmlDoc;
		/// <summary>
		/// ファイルパス
		/// </summary>
		protected	string	FilePath;
		/// <summary>
		/// 設定値更新フラグ
		/// </summary>
		protected	bool	fUpdate=false;
		/// <summary>
		/// 設定更新フラグ
		/// </summary>
		public	bool	IsUpdate
		{
			get	{	return	this.fUpdate;	}
		}
		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
		public ConfigXml()
		{
			XmlDoc	=	new	XmlDocument();
		}
		/// <summary>
		/// コンストラクタ	ファイル指定
		/// </summary>
		/// <param name="FilePath">XMLファイルパス</param>
		public	ConfigXml(string	FilePath)
		{
			XmlDoc	=	new	XmlDocument();
			this.Load(FilePath);
		}
		/// <summary>
		/// ファイルのロード
		/// </summary>
		/// <param name="FilePath">XMLファイルパス</param>
		public	void	Load(string	FilePath)
		{
			fUpdate	=	false;
			this.FilePath	=	FilePath;
			XmlDoc.Load(FilePath);
		}
		/// <summary>
		/// ファイルの保存
		/// </summary>
		public	void	Save()
		{
			Save(this.FilePath);
		}
		/// <summary>
		/// ファイルの保存
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
		/// ディストラクタ
		/// </summary>
		~ConfigXml()
		{
			Dispose(false);
		}
		#region IDisposable メンバ
		/// <summary>
		/// Dispose完了フラグ
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Dispose処理の実装
		/// Queとイベントオブジェクトを破棄する
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Dispose処理の実装
		/// </summary>
		/// <param name="disposing">手動開放かディストラクタかの識別</param>
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
		/// XPathによるオブジェクトの参照
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <returns>取得されたオブジェクト</returns>
		public XmlNode Select(string path)
		{
			return  XmlDoc.SelectSingleNode(path);
		}
		/// <summary>
		/// XPathによるオブジェクトの参照
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <returns>取得されたオブジェクト</returns>
		public XmlNodeList Selects(string path)
		{
			return  XmlDoc.SelectNodes(path);
		}
		/// <summary>
		/// XPathによるオブジェクトの更新
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="val">登録するデータ</param>
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
		/// XPathによるオブジェクトの更新
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="val">登録するデータ</param>
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
		/// XPathによるオブジェクトの追加
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="val">登録するデータ</param>
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
		/// XPathによる文字列の取得
		/// </summary>
		public string SelectString(string path)
		{
			return  this.Select(path).InnerText;
		}
		/// <summary>
		/// XPathによる文字列の取得(配列アクセス)
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="idx">添え字</param>
		/// <returns>取得された文字列</returns>
		public string SelectString(string path,int idx)
		{
			return  this.Selects(path)[idx].InnerText;
		}
		/// <summary>
		/// XPathによる文字列の取得
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="DefaultValue">データがない場合のディフォルト値</param>
		/// <returns>取得された文字列</returns>
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
		///  XPathによる文字列の取得(配列アクセス)
		/// </summary>
		/// <returns></returns>
		/// <param name="path">XPath指定</param>
		/// <param name="idx">添え字</param>
		/// <param name="DefaultValue">データがない場合のディフォルト値</param>
		/// <returns>取得された文字列</returns>
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
		/// XPathによる数値(int)の取得
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <returns>取得された数値</returns>
		public int SelectInt(string path)
		{
			return	int.Parse(this.SelectString(path));
		}
		/// <summary>
		/// XPathによる数値(int)の取得
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="DefaultValue">データがない場合のディフォルト値</param>
		/// <returns>取得された数値</returns>
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
		/// XPathによる数値の取得(配列アクセス)
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <param name="idx">添え字</param>
		/// <param name="DefaultValue">データがない場合のディフォルト値</param>
		/// <returns>取得された文字列</returns>
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
		/// XPathによるで指定したエレメントの数の取得
		/// </summary>
		/// <param name="path">XPath指定</param>
		/// <returns>エレメントの数</returns>
		public	int	Count(string	path)
		{
			return	this.Selects(path).Count;
		} 
	}
}
