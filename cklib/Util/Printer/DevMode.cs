using System;
using System.IO;
using System.Runtime.InteropServices;
namespace cklib.Util.Printer
{
	/// <summary>
	/// DevMode のロードサーブ処理
	/// </summary>
	public class DevMode
	{
		/// <summary>
		/// DEVMODE構造体の保存(Wide型からAscii型に変換し保存する)
		/// </summary>
		/// <param name="hDevMode">DEVMODEへのグローバルハンドル</param>
        /// <param name="SavePathA">保存先ファイル名(アスキーコードモード）</param>
        /// <param name="SavePathW">保存先ファイル名(Unicodeモード)</param>
        /// <returns>成功時true</returns>
		public	static	bool	Save(System.IntPtr	hDevMode,string	SavePathA,string SavePathW)
		{
			System.IO.FileStream	fs	=	null;
			bool	fret	=	false;
			//	UNIコードモード用保存
			try
			{
				System.IntPtr	Devmode	=	System.Runtime.InteropServices.Marshal.ReadIntPtr(hDevMode);
				int	dmsize=System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,68);
				int	dmextra=System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,70);
				fs	=	System.IO.File.Open(SavePathW,
					System.IO.FileMode.Create,
					System.IO.FileAccess.Write,
					System.IO.FileShare.None);
				int	i;
				int	wdmsize	=	dmsize;
				int	wLength	=	wdmsize+dmextra;
				fs.WriteByte((byte)(wLength&0xff));
				fs.WriteByte((byte)((wLength>>8)&0xff));
				fs.WriteByte((byte)((wLength>>16)&0xff));
				fs.WriteByte((byte)((wLength>>24)&0xff));
				for	(i=0;i<wLength;i++)
				{
					fs.WriteByte(System.Runtime.InteropServices.Marshal.ReadByte(Devmode,i));
				}
				fret	=	true;
			}
			catch	//(System.Exception	exp)
			{
			}
			finally
			{
				if	(fs!=null)
				{
					fs.Close();
					fs	=	null;
				}
			}
			if	(!fret)
				return	false;
			//	asciiモード用の保存
			try
			{
				System.IntPtr	Devmode	=	System.Runtime.InteropServices.Marshal.ReadIntPtr(hDevMode);
				int	dmsize=System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,68);
				int	dmextra=System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,70);
				fs	=	System.IO.File.Open(SavePathA,
												System.IO.FileMode.Create,
												System.IO.FileAccess.Write,
												System.IO.FileShare.None);
				int	i,j;
				int	wdmsize	=	dmsize-64;
				int	wLength	=	wdmsize+dmextra;
				fs.WriteByte((byte)(wLength&0xff));
				fs.WriteByte((byte)((wLength>>8)&0xff));
				fs.WriteByte((byte)((wLength>>16)&0xff));
				fs.WriteByte((byte)((wLength>>24)&0xff));
				//	デバイス名を書き込む
				//	BCHAR  dmDeviceName[CCHDEVICENAME];→文字コード変換後書き込む 
				string	dname	=	System.Runtime.InteropServices.Marshal.PtrToStringUni(Devmode,32);
				byte[]	dnameb	=	System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(dname);
				for	(i=0;(i<32)&&(i<dnameb.Length);i++)
				{
					fs.WriteByte(dnameb[i]);
				}
				for	(;i<32;i++)
				{
					fs.WriteByte(0x00);
				}
				int	val;
				j=64;
				//	WORD   dmSpecVersion; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	WORD   dmDriverVersion; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	WORD   dmSize; 
				fs.WriteByte((byte)(wdmsize&0xff));
				fs.WriteByte((byte)((wdmsize>>8)&0xff));
				j+=2;
				//	WORD   dmDriverExtra; 
				fs.WriteByte((byte)(dmextra&0xff));
				fs.WriteByte((byte)((dmextra>>8)&0xff));
				j+=2;
				//	DWORD  dmFields; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt32(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				fs.WriteByte((byte)((val>>16)&0xff));
				fs.WriteByte((byte)((val>>24)&0xff));
				j+=4;
				//	union {
				//		struct {
				//		short dmOrientation;
				//		short dmPaperSize;
				//		short dmPaperLength;
				//		short dmPaperWidth;
				//		short dmScale; 
				//		short dmCopies; 
				//		short dmDefaultSource; 
				//		short dmPrintQuality; 
				//		};
				//		POINTL dmPosition;
				//		DWORD  dmDisplayOrientation;
				//		DWORD  dmDisplayFixedOutput;
				//	};
				//	Total 16Bytes
				for	(i=0;i<16;i++,j++)
				{
					fs.WriteByte(System.Runtime.InteropServices.Marshal.ReadByte(Devmode,j));					
				}
				//	short  dmColor; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	short  dmDuplex; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	short  dmYResolution; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	short  dmTTOption; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	short  dmCollate; 
				val	=	System.Runtime.InteropServices.Marshal.ReadInt16(Devmode,j);
				fs.WriteByte((byte)(val&0xff));
				fs.WriteByte((byte)((val>>8)&0xff));
				j+=2;
				//	BYTE  dmFormName[CCHFORMNAME];→文字コード変換後書き込む 
				byte[]	FormNameUNI=new	byte[64];
				for	(i=0;i<64;i++)
				{
					FormNameUNI[i]=	System.Runtime.InteropServices.Marshal.ReadByte(Devmode,j++);
				}
				string	FormName=	System.Text.Encoding.Unicode.GetString(FormNameUNI);
				byte[]	FormNameb	=	System.Text.Encoding.GetEncoding("Shift_JIS").GetBytes(FormName);
				for	(i=0;(i<32)&&(i<FormNameb.Length);i++)
				{
					fs.WriteByte(FormNameb[i]);
				}
				for	(;i<32;i++)
				{
					fs.WriteByte(0x00);
				}
				//	これ以後は、DEVICEEXTRAまでそのまま書き込む
				//	WORD  dmLogPixels; 
				//	DWORD  dmBitsPerPel; 
				//	DWORD  dmPelsWidth; 
				//	DWORD  dmPelsHeight; 
				//	union {
				//		DWORD  dmDisplayFlags; 
				//		DWORD  dmNup;
				//	}
				//	DWORD  dmDisplayFrequency; 
				//	#if(WINVER >= 0x0400) 
				//	DWORD  dmICMMethod;
				//	DWORD  dmICMIntent;
				//	DWORD  dmMediaType;
				//	DWORD  dmDitherType;
				//	DWORD  dmReserved1;
				//	DWORD  dmReserved2;
				//	#if (WINVER >= 0x0500) || (_WIN32_WINNT >= 0x0400)
				//	DWORD  dmPanningWidth;
				//	DWORD  dmPanningHeight;
				//	#endif
				//	#endif /* WINVER >= 0x0400 */
				for	(;j<dmsize+dmextra;j++)
				{
					fs.WriteByte(System.Runtime.InteropServices.Marshal.ReadByte(Devmode,j));
				}
				fret	=	true;
			}
			catch	//(System.Exception	exp)
			{
			}
			finally
			{
				if	(fs!=null)
				{
					fs.Close();
					fs	=	null;
				}
			}
			return	fret;
		}
        /// <summary>
        /// DevModeのロード
        /// </summary>
        /// <param name="LoadPath">DevModeファイルパス</param>
        /// <returns>DevModへのポインタ</returns>
		public	static	System.IntPtr	Load(string	LoadPath)
		{
			byte[]	src	=	LoadDevmode(LoadPath);
			System.IntPtr	hDevMode	=	System.Runtime.InteropServices.Marshal.AllocHGlobal(src.Length);
			int	i;
			for	(i=0;i<src.Length;i++)
			{
				System.Runtime.InteropServices.Marshal.WriteByte(hDevMode,i,src[i]);
			}
			return	hDevMode;
		}
		private	static	byte[]	LoadDevmode(string	LoadPath)
		{
			byte[]	ret=null;
			System.IO.FileStream	fs	=	null;
			try
			{
				fs	=	System.IO.File.Open(LoadPath,
					System.IO.FileMode.Open,
					System.IO.FileAccess.Read,
					System.IO.FileShare.ReadWrite);
				ret	=	new	byte[fs.Length-4];
				fs.Seek(4,System.IO.SeekOrigin.Begin);
				fs.Read(ret,0,(int)(fs.Length-4));
			}
			catch	//(System.Exception exp)
			{
			}
			finally
			{
				if	(fs!=null)
				{
					fs.Close();
				}
			}
			if	(ret==null)
			{
				throw	new	System.Exception("File Error");
			}
			return	ret;
		}
	}
}
