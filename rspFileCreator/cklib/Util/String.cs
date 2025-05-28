using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace cklib.Util
{
	/// <summary>
	/// String
	/// </summary>
    /// <remarks>
    /// 文字列操作ユーティリティ
    /// </remarks>
	public static class String
    {
        #region 制御コード定義
        static	byte[]	KCODECNV_SO				=	{	0x0e	};
	    static	byte[]	KCODECNV_SI				=	{	0x0f	};
    //	static	byte	KCODECNV_ESC			=0x1b;
    //	static	byte	KCODECNV_LF				=0x0a;
	    static	byte[]	KCODECNV_CRLF			=	{	0x0d,0x0a	};
	    static	byte[]	TO_SINGLE				=	{	0x1b,0x28,0x48	};	//	single-byte-seq		= ESC "(" ( "B" / "J" )
	    static	byte[]	TO_DOUBLE				=	{	0x1b,0x24,0x40	};	//	double-byte-seq		= ESC "$" ( "@" / "B" )
	    static	byte[]	TO_SINGLE_SO			=	{	0x1b,0x28,0x48,0x0e	};
	    static	byte[]	TO_SI_DOUBLE			=	{	0x0f,0x1b,0x24,0x40	};	//	double-byte-seq		= ESC "$" ( "@" / "B" )
    //	static	byte	SJIS1_ST				=0x81;
    //	static	byte	SJIS1_ED				=0x9f;
    //	static	byte	SJIS2_ST				=0xe0;
        //	static	byte	SJIS2_ED				=0xfc;
        #endregion
        #region 文字列操作
        /// <summary>
        /// 文字列から行を切り出す
        /// </summary>
        /// <param name="src">切り出し元文字列</param>
        /// <param name="line">切り出された文字列</param>
        /// <param name="start">切り出し開始位置</param>
        /// <returns>改行が含まれない場合falseを返す</returns>
		public	static bool	LookupLine(ref string src,ref string line,ref int start)
		{
			int	offset = 0;
			bool	result;
			int	appendvalue=1;
            offset = src.IndexOf("\n", start);
			if	(offset==-1)
			{
				line	=	src.Substring(start);
				offset	=	start+line.Length;
				result	=	false;
			}
			else
			{
				if	(offset!=0)
				{
					if	(src.Substring(offset-1,1)=="\r")
					{	//	CRLF
						offset--;
						appendvalue	=	2;
					}
				}
				offset	-=	start;
				line	=	src.Substring(start,offset);
				offset	+=appendvalue;
				result	=	true;
			}
			start	+=	offset;
			return	result;
		}
        /// <summary>
        /// 半角→全角変換
        /// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換結果文字列</returns>
		public	static	string	AnkToWide(ref	string	src)
		{
			return	Microsoft.VisualBasic.Strings.StrConv(src,VbStrConv.Wide,1041);
		}
        /// <summary>
        /// 全角→半角変換
        /// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換結果文字列</returns>
        public static string WideToAnk(ref	string src)
		{
			return	Microsoft.VisualBasic.Strings.StrConv(src,VbStrConv.Narrow,1041);
		}
		/// <summary>
		/// 指定したカラム数の指定文字列を生成する
		/// </summary>
        /// <remarks>
        /// 桁数が指定文字列の桁数の倍数で無い場合は、近似する指定桁数以下の最大数となる。
        /// </remarks>
        /// <param name="pad">指定文字列</param>
        /// <param name="column">桁数</param>
        /// <returns>生成文字列</returns>
        public	static	string	GetPadingString(string	pad,int	column)
		{
			//	カラム数を数える為Shift_JISに変換する
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(pad);
			int	count	=	column/cb.Length;
			byte[]	padstr	=	new	byte[count*cb.Length];
			int	i;
			for	(i=0;i<count;i++)
			{
				Array.Copy(cb,0,padstr,i*cb.Length,cb.Length);
			}
			return	System.Text.Encoding.GetEncoding("Shift_Jis").GetString(padstr);
		}
        /// <summary>
        /// 指定したパディング文字を結合して指定カラム数の文字列を生成する
        /// </summary>
        /// <param name="src">結合元文字列</param>
        /// <param name="pad">パディング文字列</param>
        /// <param name="column">桁数</param>
        /// <returns>生成文字列</returns>
		public	static	string	AddPading(string src,string	pad,int	column)
		{
			//	カラム数を数える為Shift_JISに変換する
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(pad);
			byte[]	srcb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(src);
			if	((srcb.Length+cb.Length)>column)
			{	//	パディングできない
				return	src;
			}
			int	count	=	(column-srcb.Length)/cb.Length;
			byte[]	padstr	=	new	byte[column];
			int	i;
			Array.Copy(srcb,padstr,srcb.Length);	//	先頭の文字をコピー
			for	(i=srcb.Length;i<column;i+=cb.Length)
			{
				Array.Copy(cb,0,padstr,i,cb.Length);
			}
			return	System.Text.Encoding.GetEncoding("Shift_Jis").GetString(padstr);
		}
        /// <summary>
        /// 指定した文字列のカラム数を取得する
        /// </summary>
        /// <param name="str">チェックする文字列</param>
        /// <returns>カラム数</returns>
		public	static	int	GetColumn(string	str)
		{
			//	カラム数を数える為Shift_JISに変換する
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
			return	cb.Length;
		}
        /// <summary>
        /// 指定したカラムで文字列を分割する
        /// </summary>
        /// <remarks>
        /// 分割位置が全角文字を分割する位置となる場合分割位置を１カラム前半に変更し分割する。
        /// </remarks>
        /// <param name="str">分割元文字列</param>
        /// <param name="column">桁数</param>
        /// <param name="after">分割位置より後半文字列</param>
        /// <returns>分割位置の前半文字列</returns>
		public	static	string	SplitColumn(string	str,int	column,out	string	after)
		{
			//	カラム数を数える為Shift_JISに変換する
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
			int	i;
			bool	finKanji=false;
			if	(cb.Length>column)
			{	//	分割位置が文字列長より小さいこと
				//	指定カラム位置が漢字中間に入らないか検査する
				for	(i=0;i<cb.Length && i<column;i++)
				{
					if	(finKanji)
					{
						finKanji	=	false;
						continue;
					}
					if	(IsKanji(cb[i]))
					{	//	漢字
						finKanji	=	true;
					}
				}
				if	(finKanji&&(i==column))
				{	//	漢字の途中分割
					column--;
				}
				byte[]	dest	=	new	byte[cb.Length-column];
				Array.Copy(cb,column,dest,0,dest.Length);
				after	=	System.Text.Encoding.GetEncoding("Shift_Jis").GetString(dest);
				dest	=	new	byte[column];
				Array.Copy(cb,0,dest,0,column);
				return	System.Text.Encoding.GetEncoding("Shift_Jis").GetString(dest);
			}
			else
			{
				after	=	string.Empty;
				return	str;
			}
		}
        /// <summary>
        /// 指定したカラムで文字列を分割した左側を取得する
        /// </summary>
        /// <remarks>
        /// 分割位置が全角文字を分割する位置となる場合分割位置を１カラム前半に変更し分割する。
        /// </remarks>
        /// <param name="str">分割元文字列</param>
        /// <param name="column">桁数</param>
        /// <returns>分割位置左側文字列</returns>
        public static string SplitColumnLeft(string str, int column)
        {
            string after;
            return SplitColumn(str, column, out after);
        }
        /// <summary>
        /// 指定したカラムで文字列を分割した右側を取得する
        /// </summary>
        /// <remarks>
        /// 分割位置が全角文字を分割する位置となる場合分割位置を１カラム前半に変更し分割する。
        /// </remarks>
        /// <param name="str">分割元文字列</param>
        /// <param name="column">桁数</param>
        /// <returns>分割位置右側文字列</returns>
        public static string SplitColumnRight(string str, int column)
        {
            string after;
            SplitColumn(str, column, out after);
            return after;
        }
        /// <summary>
		/// ShiftJIS第一バイトか検査する
		/// </summary>
		/// <param name="src">検査するByteデータ</param>
		/// <returns>第一バイトならtrue</returns>
		public	static	bool	IsKanji(byte	src)
		{
			if	(((src>=0x81)&&(src<=0x9f))||
				((src>=0xe0)&&(src<=0xfc)))
			{	//	漢字
				return	true;
			}
			else
			{
				return	false;
			}
		}
        /// <summary>
        /// 指定したカラムから指定カラム数で文字列を切り出す
        /// </summary>
        /// <param name="str">切り出し元文字列</param>
        /// <param name="column">切り出しカラム位置</param>
        /// <param name="lengcol">切り出す長さ</param>
        /// <returns>切り出された文字列</returns>
        public static string SubStringColumn(string str, int column, int lengcol)
        {
            //	カラム数を数える為Shift_JISに変換する
            byte[] cb = System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
            int i;
            bool finKanji = false;
            if (cb.Length > column)
            {	//	位置が文字列長より小さいこと
                //	指定カラム位置が漢字中間に入らないか検査する
                for (i = 0; i < cb.Length && i < column; i++)
                {
                    if (finKanji)
                    {
                        finKanji = false;
                        continue;
                    }
                    if (((cb[i] >= 0x81) && (cb[i] <= 0x9f)) ||
                        ((cb[i] >= 0xe0) && (cb[i] <= 0xfc)))
                    {	//	漢字
                        finKanji = true;
                    }
                }
                if (finKanji && (i == column))
                {	//	漢字の途中分割
                    column++;
                    finKanji = false;
                }
                byte[] dest;
                if (lengcol != -1)
                {
                    for (i = column; i < cb.Length && (i - column) < lengcol; i++)
                    {
                        if (finKanji)
                        {
                            finKanji = false;
                            continue;
                        }
                        if (((cb[i] >= 0x81) && (cb[i] <= 0x9f)) ||
                            ((cb[i] >= 0xe0) && (cb[i] <= 0xfc)))
                        {	//	漢字
                            finKanji = true;
                        }
                    }
                    if (finKanji && ((i - column) == lengcol))
                    {	//	漢字の途中分割
                        lengcol++;
                    }
                    if (lengcol > cb.Length - column)
                    {
                        lengcol = cb.Length - column;
                    }
                    dest = new byte[lengcol];
                }
                else
                {
                    dest = new byte[cb.Length - column];
                }
                Array.Copy(cb, column, dest, 0, dest.Length);
                return System.Text.Encoding.GetEncoding("Shift_Jis").GetString(dest);
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 指定したカラムから指定文字位置で文字列を切り出す
        /// </summary>
        /// <remarks>
        /// インスタンスから部分文字列を取得します。<br/>
        /// 切り出し位置が文字列範囲外の場合は空文字列を返す<br/>
        /// lengthが文字列長に満たない場合は、取得出来る範囲で取得する<br/>
        /// </remarks>
        /// <param name="str">切り出し元文字列</param>
        /// <param name="col">部分文字列の開始位置のインデックス</param>
        /// <param name="length">部分文字列の文字数。 </param>
        /// <returns>切り出された文字列</returns>
        public static string SubString(string str, int col, int length)
        {
            if (str.Length <= col)
                return string.Empty;
            if (str.Length<(col+length))
            {
                length = str.Length - col;
            }
            return str.Substring(col, length);
        }
        /// <summary>
        /// 指定したカラムから指定文字位置で文字列を切り出す
        /// </summary>
        /// <remarks>
        /// インスタンスから部分文字列を取得します。<br/>
        /// 切り出し位置が文字列範囲外の場合は空文字列を返す<br/>
        /// lengthが文字列長に満たない場合は、取得出来る範囲で取得する<br/>
        /// </remarks>
        /// <param name="str">切り出し元文字列</param>
        /// <param name="col">部分文字列の開始位置のインデックス</param>
        /// <returns>切り出された文字列</returns>
        public static string SubString(string str, int col)
        {
            return SubString(str, col, str.Length);
        }
        /// <summary>
        /// 文字列の左端（先頭）から指定した文字数の文字列を切り出す
        /// </summary>
        /// <remarks>
        /// インスタンスから部分文字列を取得します。<br/>
        /// 切り出し位置が文字列範囲外の場合は空文字列を返す<br/>
        /// lengthが文字列長に満たない場合は、取得出来る範囲で取得する<br/>
        /// </remarks>
        /// <param name="str">切り出し元文字列</param>
        /// <param name="length">部分文字列の文字数。 </param>
        /// <returns>切り出された文字列</returns>
        public static string Left(string str, int length)
        {
            return SubString(str,0, length);
        }
        /// <summary>
        /// 文字列の右端（末尾）から指定した文字数の文字列を切り出す
        /// </summary>
        /// <remarks>
        /// インスタンスから部分文字列を取得します。<br/>
        /// 切り出し位置が文字列範囲外の場合は空文字列を返す<br/>
        /// lengthが文字列長に満たない場合は、取得出来る範囲で取得する<br/>
        /// </remarks>
        /// <param name="str">切り出し元文字列</param>
        /// <param name="length">部分文字列の文字数。 </param>
        /// <returns>切り出された文字列</returns>
        public static string Right(string str, int length)
        {
            if (str.Length<=length)
            {
                return str;
            }
            return str.Substring(str.Length - length);
        }
        #endregion
        #region 文字列数値変換
        /// <summary>
		/// 文字列を数字に変換
		/// </summary>
        /// <remarks>
        /// 変換出来ない場合例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
		/// <param name="src">変換元</param>
		/// <returns>変換結果</returns>
		public	static	int	ToInt(string	src)
		{
			return	int.Parse(src);
		}
		/// <summary>
		/// 文字列を数字に変換
		/// </summary>
        /// <remarks>
        /// 変換出来ない場合デフォルト値が設定される
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <param name="defval">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static int ToInt(string src, int defval)
		{
			try
			{
				return	int.Parse(src);
			}
			catch
			{
				return	defval;
			}
		}
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <returns>変換結果</returns>
        public static long ToLong(string src)
        {
            return long.Parse(src);
        }
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合デフォルト値が設定される
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <param name="defval">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static long ToLong(string src, long defval)
        {
            try
            {
                return long.Parse(src);
            }
            catch
            {
                return defval;
            }
        }
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <returns>変換結果</returns>
        public static decimal ToDecimal(string src)
        {
            return decimal.Parse(src);
        }
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合デフォルト値が設定される
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <param name="defval">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static decimal ToDecimal(string src, decimal defval)
        {
            try
            {
                return decimal.Parse(src);
            }
            catch
            {
                return defval;
            }
        }
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <returns>変換結果</returns>
        public static float ToFloat(string src)
        {
            return float.Parse(src);
        }
        /// <summary>
        /// 文字列を数字に変換
        /// </summary>
        /// <remarks>
        /// 変換出来ない場合デフォルト値が設定される
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <param name="defval">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static float ToFloat(string src, float defval)
        {
            try
            {
                return float.Parse(src);
            }
            catch
            {
                return defval;
            }
        }
        /// <summary>
        /// 数字を文字列に変換
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換後データ</returns>
		public	static	string	ToString(int	src)
		{
			return	src.ToString();
		}
        /// <summary>
        /// 数字を文字列に変換
        /// </summary>
        /// <param name="src">変換元文字列</param>
        /// <param name="format">書式指定</param>
        /// <returns>変換後文字列</returns>
		public	static	string	ToString(int	src,string format)
		{
			return	src.ToString(format);
		}
		/// <summary>
		/// 文字列をShiftJisのバイト配列に変換する
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>ShiftJIS文字列</returns>
        public static byte[] StringToBytes(string src)
		{
			return	System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(src);
		}
        /// <summary>
        /// ShiftJisのバイト配列をstringに変換する
        /// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>ShiftJIS文字列</returns>
        public static string ByteToString(byte[]    src)
        {
            return System.Text.Encoding.GetEncoding("Shift_Jis").GetString(src);
        }
        #endregion
        #region 文字列変換
        /// <summary>
        /// 文字列を指定したbool型に変換
        /// </summary>
        /// <remarks>
        /// デフォルト値が指定されない場合、
        /// 変換失敗で例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <param name="defval">デフォルト値</param>
        /// <returns>変換結果</returns>
        public static bool ToBool(string src, bool? defval = null)
        {
            bool ret;
            if (defval.HasValue)
            {
                if (!bool.TryParse(src, out ret))
                    ret = defval.Value;
            }
            else
                ret = bool.Parse(src);
            return ret;
        }
        /// <summary>
        /// 文字列を指定した列挙型に変換
        /// </summary>
        /// <remarks>
        /// デフォルト値が指定されない場合、
        /// 変換失敗で例外を発生する<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">変換元</param>
        /// <returns>変換結果</returns>
        public static T To<T>(string src, T? defval = null)
            where T : struct,IConvertible
        {
            T ret = default(T);
            if (defval.HasValue)
            {
#if __net35__ || __net20__
                try
                {
                    ret =  (T)Enum.Parse(typeof(T), src, false);
                }
                catch
                {
                    ret = defval.Value;
                }
#else
                if (!Enum.TryParse<T>(src, out ret))
                    ret = defval.Value;
#endif
            }
            else
            {
                return (T)Enum.Parse(typeof(T), src, false);
            }
            return ret;
        }

        #endregion
        #region ダンプ
        /// <summary>
		/// バイト配列をHexダンプし文字列に変換
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換後文字列</returns>
        public static string HexDumpStr(byte[] src)
		{
			return	HexDumpStr(src,src.Length);
		}
		/// <summary>
		/// バイト配列をHexダンプし文字列に変換
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <param name="length">変換元有効文字列長</param>
        /// <returns>変換後文字列</returns>
        public static string HexDumpStr(byte[] src, int length)
		{
			System.Text.StringBuilder	dest	=	new	System.Text.StringBuilder(length*2);
			int	i;
			for	(i=0;i<length;i++)
			{
				string	wk	=	"00"+src[i].ToString("X");
				dest.Append(wk.Substring(wk.Length-2,2));
			}
			return	dest.ToString();
		}
		/// <summary>
		/// バイト配列を横１６バイト毎のHexダンプリストに変換
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換後文字列</returns>
        public static string HexDumpList(byte[] src)
		{
			return	HexDumpList(src,src.Length);
		}
        /// <summary>
        /// バイト配列を横１６バイト毎のHexダンプリストに変換
        /// </summary>
        /// <param name="src">変換元文字列</param>
        /// <param name="length">変換元有効文字列長</param>
        /// <returns>変換後文字列</returns>
        public static string HexDumpList(byte[] src, int length)
		{
			System.Text.StringBuilder	dest	=	new	System.Text.StringBuilder(length*2);
			int	i;
			for	(i=0;i<length;i++)
			{
				if	(i!=0)
				{
					if	((i%16)==0)
						dest.Append("\r\n");
					else
						dest.Append(" ");
				}
				string	wk	=	"00"+src[i].ToString("X");
				dest.Append(wk.Substring(wk.Length-2,2));
			}
			dest.Append("\r\n");
			return	dest.ToString();
		}

		/// <summary>
		/// byte型データをHexダンプする
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換後文字列</returns>
        public static string HexDumpStr(byte src)
		{
			byte[]	b=new byte[1];
			b[0]	=	src;
			return	HexDumpStr(b);
		}
        /// <summary>
        /// HEXダンプ変換用文字列テーブル
        /// </summary>
        private const string HEXTbl = "0123456789ABCDEDFabcdef";
        /// <summary>
        /// HEXダンプをbyte配列へ変換する
        /// </summary>
        /// <param name="src">HEXダンプ</param>
        /// <returns>変換されたbyte配列</returns>
        public static byte[] ParseHexDump(string src)
        {
            List<byte> dest=new List<byte>(src.Length*2);
            byte b;
            int wk;
            b = 0;
            wk = 0;
            bool f=false;
            for (int i = 0; i < src.Length; i++)
            {
                wk = HEXTbl.IndexOf(src[i]);
                if (wk == -1)
                {
                    if (f)
                    {
                        f = false;
                        dest.Add(b);
                    }
                    continue;
                }
                if (wk > 15)
                    wk -= 10;
                if (f)
                {
                    b = (byte)((b * 0x10) + wk);
                    f = false;
                    dest.Add(b);
                }
                else
                {
                    b = (byte)wk;
                    f = true;
                }
            }
            return dest.ToArray();
        }
        #endregion
        #region SQL関連
        /// <summary>
		/// 文字列をSqlQueryに使用できるように変換
		/// </summary>
        /// <param name="src">変換元文字列</param>
        /// <returns>変換後文字列</returns>
        public static string SqlStrLapping(string src)
		{
			return	"'"	+	src.Replace("'","''") + "'";
		}
		/// <summary>
		/// 日付文字列をSqlQueryに使用できるように変換
		/// </summary>
		/// <param name="src">変換元文字列</param>
		/// <returns>変換後文字列</returns>
		public	static	string SqlDateLapping(string	src)
		{
			if	(src.Length==0)
			{
				return	"null";
			}
			else
			{
				return	"'"	+	src.Replace("'","''") + "'";
			}
        }
        #endregion
        #region 文字列チェック
        /// <summary>
		/// ニューメリックチェック
		/// </summary>
		/// <param name="str">チェック文字列</param>
		/// <returns>数字のみならtrueを返す</returns>
		public	static	bool	NumericCheck(string	str)
		{
			for	(int i=0;i<str.Length;i++)
			{
				if	(str[i]<'0')
					return	false;
				if	(str[i]>'9')
					return	false;
			}
			return	true;
		}
		/// <summary>
		/// ニューメリックチェック＆カット
		/// </summary>
		/// <param name="str">チェック文字列</param>
		/// <returns>数字以外を除外した文字列</returns>
		public	static	string	NumericCheckModify(string	str)
		{
			string	dest	=	string.Empty;
			for	(int i=0;i<str.Length;i++)
			{
				if	(str[i]<'0')
					continue;
				if	(str[i]>'9')
					continue;
				dest	+=	str[i];
			}
			return	dest;
        }
        #endregion
        #region 定数定義
        /// <summary>
        /// 月を示す文字列(RFC822)
        /// </summary>
        public static readonly string[] RFC822_MonthString =	{ "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        #endregion
        #region printf
        /// <summary>
        /// sprintf CRTライブラリ呼び出し
        /// </summary>
        /// <remarks>
        /// MSCRTライブラリsprintfを呼び出します。詳細はsprintfそ参照
        /// </remarks>
        /// <param name="buffer">編集後格納バッファ</param>
        /// <param name="format">printf互換編集文字列</param>
        /// <returns>結果文字列長</returns>
        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf(StringBuilder buffer, string format,__arglist);

        /// <summary>
        /// swprintf CRTライブラリ呼び出し
        /// </summary>
        /// <remarks>
        /// MSCRTライブラリswprintfを呼び出します。詳細はsprintfそ参照
        /// </remarks>
        /// <param name="buffer">編集後格納バッファ</param>
        /// <param name="format">printf互換編集文字列</param>
        /// <returns>結果文字列長</returns>
        [DllImport("msvcrt.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int swprintf(StringBuilder buffer, string format,__arglist);
        /*
        /// <summary>
        /// sprintf CRTライブラリ呼び出し
        /// </summary>
        /// <remarks>
        /// MSCRTライブラリsprintfを呼び出します。詳細はsprintfそ参照
        /// </remarks>
        /// <param name="buffer">編集後格納バッファ</param>
        /// <param name="fmt">printf互換編集文字列</param>
        /// <param name="arg1">編集組み込み文字列</param>
        /// <returns>結果文字列長</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1);

        /// <summary>
        /// swprintf CRTライブラリ呼び出し
        /// </summary>
        /// <remarks>
        /// MSCRTライブラリswprintfを呼び出します。詳細はsprintfそ参照
        /// </remarks>
        /// <param name="buffer">編集後格納バッファ</param>
        /// <param name="fmt">printf互換編集文字列</param>
        /// <param name="arg1">編集組み込み文字列1</param>
        /// <param name="arg2">編集組み込み文字列2</param>
        /// <returns>結果文字列長</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1, String arg2);

        /// swprintf CRTライブラリ呼び出し
        /// </summary>
        /// <remarks>
        /// MSCRTライブラリswprintfを呼び出します。詳細はsprintfそ参照
        /// </remarks>
        /// <param name="buffer">編集後格納バッファ</param>
        /// <param name="fmt">printf互換編集文字列</param>
        /// <param name="arg1">編集組み込み文字列1</param>
        /// <param name="arg2">編集組み込み文字列2</param>
        /// <param name="arg3">編集組み込み文字列3</param>
        /// <returns>結果文字列長</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1, String arg2, String arg3);
        */

        #endregion
    }
}
