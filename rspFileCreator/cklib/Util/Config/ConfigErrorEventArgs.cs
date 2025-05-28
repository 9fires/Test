using System;
using cklib;
namespace cklib.Util
{
	/// <summary>
	/// ConfigErrorEventArgs<br/>
	/// 設定情報取得エラー時発生イベント情報
	/// </summary>
	public class ConfigErrorEventArgs:EventArgs
	{
		/// <summary>
		/// 例外情報
		/// </summary>
		private	Exception	errExp=null;
		/// <summary>
		/// 例外情報
		/// </summary>
		public	Exception	Exception
		{
			get	{	return	this.errExp;	}
		}
		/// <summary>
		/// 取得データ名
		/// </summary>
		private	string	configName=string.Empty;
		/// <summary>
		/// 取得データ名
		/// </summary>
		public	string	ConfigName
		{
			get	{	return	configName;	}
		}
		/// <summary>
		/// デフォルトデータ値
		/// </summary>
		private	string	defaultValue=string.Empty;
		/// <summary>
		/// デフォルトデータ値
		/// </summary>
		public	string	DefaultValue
		{
			get	{	return	defaultValue;	}
		}
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public ConfigErrorEventArgs(string	name,string	defval,Exception	exp)
		{
			this.configName		=	name;
			this.defaultValue	=	defval;
			this.errExp			=	exp;
		}
	}
}
