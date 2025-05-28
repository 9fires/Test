using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;
namespace cklib.Util
{
	/// <summary>
	/// File ファイル操作関連のユーティリティ。
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
        /// ハンドルを閉じる
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="CloseHandle")]
		public static extern bool CloseHandle(IntPtr	handle);
        /// <summary>
        /// Win32エラーコードの取得
        /// </summary>
        /// <returns></returns>
		[DllImport("kernel32.dll", EntryPoint="GetLastError")]
		public static extern uint GetLastError();
		[DllImport("kernel32.dll", EntryPoint="DeviceIoControl")]
		private	static extern bool DeviceIoControl(	IntPtr	hDevice,              // デバイス、ファイル、ディレクトリいずれかのハンドル
													uint	dwIoControlCode,       // 実行する動作の制御コード
													IntPtr	lpInBuffer,           // 入力データを供給するバッファへのポインタ
													uint	nInBufferSize,         // 入力バッファのバイト単位のサイズ
													IntPtr	lpOutBuffer,          // 出力データを受け取るバッファへのポインタ
													uint	nOutBufferSize,        // 出力バッファのバイト単位のサイズ
													IntPtr	lpBytesReturned,     // バイト数を受け取る変数へのポインタ
													uint	lpOverlapped    // 非同期動作を表す構造体へのポインタ
													);
        [DllImport("kernel32.dll", EntryPoint = "GetTempFileName")]
        private extern static uint GetTempFileName( string PathName,
                                                    string PrefixString,
                                                    uint uUnique,
                                                    StringBuilder TempFileName);
        /// <summary>
        /// 読み取りアクセス
        /// </summary>
		public	static	uint	AccessMode_GENERIC_READ		=	0x80000000;
        /// <summary>
        /// 書き込みアクセス
        /// </summary>
		public	static	uint	AccessMode_GENERIC_WRITE    =	0x40000000;
        /// <summary>
        /// 実行アクセス
        /// </summary>
		public	static	uint	AccessMode_GENERIC_EXECUTE	=	0x20000000;
        /// <summary>
        /// すべてのアクセス
        /// </summary>
		public	static	uint	AccessMode_GENERIC_ALL      =	0x10000000;
        /// <summary>
        /// 読み取り共有
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_READ	=	0x00000001;
        /// <summary>
        /// 書き込み共有
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_WRITE	=	0x00000002;
        /// <summary>
        /// 削除共有
        /// </summary>
		public	static	uint	ShareMode_FILE_SHARE_DELETE	=	0x00000004;  
        /// <summary>
        /// 新しいファイルを作成します。指定したファイルが既に存在している場合、この関数は失敗します。
        /// </summary>
		public	static	uint	CREATE_NEW          =	1;
        /// <summary>
        /// 新しいファイルを作成します。指定したファイルが既に存在している場合、そのファイルを上書きし、既存の属性を消去します。
        /// </summary>
		public	static	uint	CREATE_ALWAYS       =	2;
        /// <summary>
        /// ファイルを開きます。指定したファイルが存在していない場合、この関数は失敗します。 
        /// </summary>
		public	static	uint	OPEN_EXISTING       =	3;
        /// <summary>
        /// ファイルが存在している場合、そのファイルを開きます。指定したファイルが存在していない場合、この関数は dwCreationDisposition パラメータで CREATE_NEW が指定されていたと仮定して新しいファイルを作成します。
        /// </summary>
		public	static	uint	OPEN_ALWAYS         =	4;
        /// <summary>
        /// ファイルを開き、ファイルのサイズを 0 バイトにします。呼び出し側プロセスは、dwDesiredAccess パラメータで、少なくとも GENERIC_WRITE アクセス権を指定しなければなりません。指定したファイルが存在していない場合、この関数は失敗します。
        /// </summary>
		public	static	uint	TRUNCATE_EXISTING   =	5;
        /// <summary>
        /// 	このファイルは読み取り専用です。アプリケーションはこのファイルの読み取りを行えますが、書き込みや削除はできません。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_READONLY     =	0x00000001;
        /// <summary>
        /// このファイルは隠しファイルです。通常のディレクトリリスティングでは表示されません。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_HIDDEN       =	0x00000002;
        /// <summary>
        /// このファイルは、オペレーティングシステムの一部、またはオペレーティングシステム専用のファイルです。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_SYSTEM       =	0x00000004;
        /// <summary>
        /// 指定されたハンドルは、ディレクトリに関連しています。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_DIRECTORY    =	0x00000010;
        /// <summary>
        /// このファイルをアーカイブするべきです。アプリケーションはこの属性を、
        /// ファイルのバックアップや削除のためのマークとして使います。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_ARCHIVE      =	0x00000020;
        /// <summary>
        /// 予約済み。使わないでください。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_DEVICE       =	0x00000040;
        /// <summary>
        /// このファイルに特に属性を設定しません。単独で指定した場合にのみ、この属性は有効です。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_NORMAL       =	0x00000080;
        /// <summary>
        /// このファイルは、一時ファイルとして使われています。ファイルシステムは、
        /// データをハードディスクのような大容量記憶装置へ書き込む代わりに、高速なアクセスが行えるよう、
        /// すべてのデータをメモリ内に維持することを試みます。アプリケーションは、
        /// 必要がなくなった段階で一時ファイルをすぐに削除するべきです。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_TEMPORARY    =	0x00000100;
        /// <summary>
        /// 	このファイルは、スパースファイル（疎なファイル、未使用の領域が多い、または同じ値が長く続くファイル）です。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_SPARSE_FILE  =	0x00000200;
        /// <summary>
        /// このファイルには、再解析ポイントが関連付けられています。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_REPARSE_POINT=	0x00000400;
        /// <summary>
        /// 指定されたファイルまたはディレクトリは圧縮されています。ファイルの場合、
        /// ファイル内の全データが圧縮されていることを意味します。ディレクトリの場合、
        /// そのディレクトリ内に新しく作成されるファイルまたはサブディレクトリが、
        /// 既定で圧縮状態になることを意味します。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_COMPRESSED   =	0x00000800;
        /// <summary>
        /// Windows 2000：このファイルのデータは、すぐには利用できません。
        /// この属性は、このファイルのデータがオフライン記憶装置へ物理的に移動されたことを示します。
        /// この属性は、Windows 2000 の階層記憶管理ソフトウェアである「リモート記憶域」が利用するものです。
        /// アプリケーションは、任意にこの属性を変更するべきではありません。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_OFFLINE      =	0x00001000;
        /// <summary>
        /// 	Windows 2000：このファイルを、「インデックスサービス」の対象にしません。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_NOT_CONTENT_INDEXED=	0x00002000;
        /// <summary>
        /// このファイルまたはディレクトリを暗号化するよう指示します。
        /// ファイルに対して指定した場合、そのファイル内のすべてのデータを暗号化することを意味します。
        /// ディレクトリに対して指示した場合、その中に新しく作成されるファイルとサブディレクトリに対して、
        /// 既定で暗号化を行うことを意味します。FILE_ATTRIBUTE_SYSTEM と共に指定すると、
        /// FILE_ATTRIBUTE_ENCRYPTED フラグは無効になります。
        /// </summary>
		public	static	uint	FILE_ATTRIBUTE_ENCRYPTED    =	0x00004000;


		/// <summary>
		/// ボリューム情報の取得
		/// </summary>
		/// <param name="drive">ドライブ名を示す文字列 ex)c:\</param>
		/// <param name="VolumeName">ボリューム名の受け取りバッファ</param>
		/// <param name="VolumeSerialNumber">ボリュームシリアル番号</param>
		/// <param name="MaximumComponentLength">ファイル名の上限</param>
		/// <param name="FileSystemFlags">ファイルシステムフラグ</param>
		/// <param name="FileSystemName">ファイルシステム名</param>
		/// <returns></returns>
		public	static	bool	GetVolumeInformation(   string	drive,
													    out string VolumeName,
													    out int VolumeSerialNumber,
													    out int MaximumComponentLength,
													    out int	FileSystemFlags,
													    out	string	FileSystemName)
		{
            //  初期値代入
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
        /// CreateFile-ファイル等のオープン
        /// </summary>
        /// <param name="path">ファイル等のパス</param>
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
		/// 圧縮ファイルをサポートしているかチェックする
		/// </summary>
		/// <param name="drive">ドライブレター</param>
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
        /// ファイルの圧縮属性指定
        /// </summary>
        /// <param name="file">ファイルパス</param>
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
        /// ファイルパスの正規化
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
        /// テンポラリファイル生成
        /// </summary>
        /// <remarks>
        /// 詳細はWin32SDK GetTempFileNameの記述を参照
        /// </remarks>
        /// <param name="Path">ディレクトリ名</param>
        /// <param name="Prefix">ディレクトリ名</param>
        /// <param name="Unique">整数</param>
        /// <returns>生成されたファイルのファイル名</returns>
        public static string GetTempFileName(string Path, string Prefix, uint Unique)
        {
            StringBuilder strb = new StringBuilder(1024);
            GetTempFileName(Path, Prefix, Unique, strb);
            return strb.ToString();
        }
    }
}
