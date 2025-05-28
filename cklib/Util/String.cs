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
    /// �����񑀍샆�[�e�B���e�B
    /// </remarks>
	public static class String
    {
        #region ����R�[�h��`
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
        #region �����񑀍�
        /// <summary>
        /// �����񂩂�s��؂�o��
        /// </summary>
        /// <param name="src">�؂�o����������</param>
        /// <param name="line">�؂�o���ꂽ������</param>
        /// <param name="start">�؂�o���J�n�ʒu</param>
        /// <returns>���s���܂܂�Ȃ��ꍇfalse��Ԃ�</returns>
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
        /// ���p���S�p�ϊ�
        /// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ����ʕ�����</returns>
		public	static	string	AnkToWide(ref	string	src)
		{
			return	Microsoft.VisualBasic.Strings.StrConv(src,VbStrConv.Wide,1041);
		}
        /// <summary>
        /// �S�p�����p�ϊ�
        /// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ����ʕ�����</returns>
        public static string WideToAnk(ref	string src)
		{
			return	Microsoft.VisualBasic.Strings.StrConv(src,VbStrConv.Narrow,1041);
		}
		/// <summary>
		/// �w�肵���J�������̎w�蕶����𐶐�����
		/// </summary>
        /// <remarks>
        /// �������w�蕶����̌����̔{���Ŗ����ꍇ�́A�ߎ�����w�茅���ȉ��̍ő吔�ƂȂ�B
        /// </remarks>
        /// <param name="pad">�w�蕶����</param>
        /// <param name="column">����</param>
        /// <returns>����������</returns>
        public	static	string	GetPadingString(string	pad,int	column)
		{
			//	�J�������𐔂����Shift_JIS�ɕϊ�����
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
        /// �w�肵���p�f�B���O�������������Ďw��J�������̕�����𐶐�����
        /// </summary>
        /// <param name="src">������������</param>
        /// <param name="pad">�p�f�B���O������</param>
        /// <param name="column">����</param>
        /// <returns>����������</returns>
		public	static	string	AddPading(string src,string	pad,int	column)
		{
			//	�J�������𐔂����Shift_JIS�ɕϊ�����
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(pad);
			byte[]	srcb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(src);
			if	((srcb.Length+cb.Length)>column)
			{	//	�p�f�B���O�ł��Ȃ�
				return	src;
			}
			int	count	=	(column-srcb.Length)/cb.Length;
			byte[]	padstr	=	new	byte[column];
			int	i;
			Array.Copy(srcb,padstr,srcb.Length);	//	�擪�̕������R�s�[
			for	(i=srcb.Length;i<column;i+=cb.Length)
			{
				Array.Copy(cb,0,padstr,i,cb.Length);
			}
			return	System.Text.Encoding.GetEncoding("Shift_Jis").GetString(padstr);
		}
        /// <summary>
        /// �w�肵��������̃J���������擾����
        /// </summary>
        /// <param name="str">�`�F�b�N���镶����</param>
        /// <returns>�J������</returns>
		public	static	int	GetColumn(string	str)
		{
			//	�J�������𐔂����Shift_JIS�ɕϊ�����
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
			return	cb.Length;
		}
        /// <summary>
        /// �w�肵���J�����ŕ�����𕪊�����
        /// </summary>
        /// <remarks>
        /// �����ʒu���S�p�����𕪊�����ʒu�ƂȂ�ꍇ�����ʒu���P�J�����O���ɕύX����������B
        /// </remarks>
        /// <param name="str">������������</param>
        /// <param name="column">����</param>
        /// <param name="after">�����ʒu���㔼������</param>
        /// <returns>�����ʒu�̑O��������</returns>
		public	static	string	SplitColumn(string	str,int	column,out	string	after)
		{
			//	�J�������𐔂����Shift_JIS�ɕϊ�����
			byte[]	cb=System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
			int	i;
			bool	finKanji=false;
			if	(cb.Length>column)
			{	//	�����ʒu�������񒷂�菬��������
				//	�w��J�����ʒu���������Ԃɓ���Ȃ�����������
				for	(i=0;i<cb.Length && i<column;i++)
				{
					if	(finKanji)
					{
						finKanji	=	false;
						continue;
					}
					if	(IsKanji(cb[i]))
					{	//	����
						finKanji	=	true;
					}
				}
				if	(finKanji&&(i==column))
				{	//	�����̓r������
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
        /// �w�肵���J�����ŕ�����𕪊������������擾����
        /// </summary>
        /// <remarks>
        /// �����ʒu���S�p�����𕪊�����ʒu�ƂȂ�ꍇ�����ʒu���P�J�����O���ɕύX����������B
        /// </remarks>
        /// <param name="str">������������</param>
        /// <param name="column">����</param>
        /// <returns>�����ʒu����������</returns>
        public static string SplitColumnLeft(string str, int column)
        {
            string after;
            return SplitColumn(str, column, out after);
        }
        /// <summary>
        /// �w�肵���J�����ŕ�����𕪊������E�����擾����
        /// </summary>
        /// <remarks>
        /// �����ʒu���S�p�����𕪊�����ʒu�ƂȂ�ꍇ�����ʒu���P�J�����O���ɕύX����������B
        /// </remarks>
        /// <param name="str">������������</param>
        /// <param name="column">����</param>
        /// <returns>�����ʒu�E��������</returns>
        public static string SplitColumnRight(string str, int column)
        {
            string after;
            SplitColumn(str, column, out after);
            return after;
        }
        /// <summary>
		/// ShiftJIS���o�C�g����������
		/// </summary>
		/// <param name="src">��������Byte�f�[�^</param>
		/// <returns>���o�C�g�Ȃ�true</returns>
		public	static	bool	IsKanji(byte	src)
		{
			if	(((src>=0x81)&&(src<=0x9f))||
				((src>=0xe0)&&(src<=0xfc)))
			{	//	����
				return	true;
			}
			else
			{
				return	false;
			}
		}
        /// <summary>
        /// �w�肵���J��������w��J�������ŕ������؂�o��
        /// </summary>
        /// <param name="str">�؂�o����������</param>
        /// <param name="column">�؂�o���J�����ʒu</param>
        /// <param name="lengcol">�؂�o������</param>
        /// <returns>�؂�o���ꂽ������</returns>
        public static string SubStringColumn(string str, int column, int lengcol)
        {
            //	�J�������𐔂����Shift_JIS�ɕϊ�����
            byte[] cb = System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(str);
            int i;
            bool finKanji = false;
            if (cb.Length > column)
            {	//	�ʒu�������񒷂�菬��������
                //	�w��J�����ʒu���������Ԃɓ���Ȃ�����������
                for (i = 0; i < cb.Length && i < column; i++)
                {
                    if (finKanji)
                    {
                        finKanji = false;
                        continue;
                    }
                    if (((cb[i] >= 0x81) && (cb[i] <= 0x9f)) ||
                        ((cb[i] >= 0xe0) && (cb[i] <= 0xfc)))
                    {	//	����
                        finKanji = true;
                    }
                }
                if (finKanji && (i == column))
                {	//	�����̓r������
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
                        {	//	����
                            finKanji = true;
                        }
                    }
                    if (finKanji && ((i - column) == lengcol))
                    {	//	�����̓r������
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
        /// �w�肵���J��������w�蕶���ʒu�ŕ������؂�o��
        /// </summary>
        /// <remarks>
        /// �C���X�^���X���畔����������擾���܂��B<br/>
        /// �؂�o���ʒu��������͈͊O�̏ꍇ�͋󕶎����Ԃ�<br/>
        /// length�������񒷂ɖ����Ȃ��ꍇ�́A�擾�o����͈͂Ŏ擾����<br/>
        /// </remarks>
        /// <param name="str">�؂�o����������</param>
        /// <param name="col">����������̊J�n�ʒu�̃C���f�b�N�X</param>
        /// <param name="length">����������̕������B </param>
        /// <returns>�؂�o���ꂽ������</returns>
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
        /// �w�肵���J��������w�蕶���ʒu�ŕ������؂�o��
        /// </summary>
        /// <remarks>
        /// �C���X�^���X���畔����������擾���܂��B<br/>
        /// �؂�o���ʒu��������͈͊O�̏ꍇ�͋󕶎����Ԃ�<br/>
        /// length�������񒷂ɖ����Ȃ��ꍇ�́A�擾�o����͈͂Ŏ擾����<br/>
        /// </remarks>
        /// <param name="str">�؂�o����������</param>
        /// <param name="col">����������̊J�n�ʒu�̃C���f�b�N�X</param>
        /// <returns>�؂�o���ꂽ������</returns>
        public static string SubString(string str, int col)
        {
            return SubString(str, col, str.Length);
        }
        /// <summary>
        /// ������̍��[�i�擪�j����w�肵���������̕������؂�o��
        /// </summary>
        /// <remarks>
        /// �C���X�^���X���畔����������擾���܂��B<br/>
        /// �؂�o���ʒu��������͈͊O�̏ꍇ�͋󕶎����Ԃ�<br/>
        /// length�������񒷂ɖ����Ȃ��ꍇ�́A�擾�o����͈͂Ŏ擾����<br/>
        /// </remarks>
        /// <param name="str">�؂�o����������</param>
        /// <param name="length">����������̕������B </param>
        /// <returns>�؂�o���ꂽ������</returns>
        public static string Left(string str, int length)
        {
            return SubString(str,0, length);
        }
        /// <summary>
        /// ������̉E�[�i�����j����w�肵���������̕������؂�o��
        /// </summary>
        /// <remarks>
        /// �C���X�^���X���畔����������擾���܂��B<br/>
        /// �؂�o���ʒu��������͈͊O�̏ꍇ�͋󕶎����Ԃ�<br/>
        /// length�������񒷂ɖ����Ȃ��ꍇ�́A�擾�o����͈͂Ŏ擾����<br/>
        /// </remarks>
        /// <param name="str">�؂�o����������</param>
        /// <param name="length">����������̕������B </param>
        /// <returns>�؂�o���ꂽ������</returns>
        public static string Right(string str, int length)
        {
            if (str.Length<=length)
            {
                return str;
            }
            return str.Substring(str.Length - length);
        }
        #endregion
        #region �����񐔒l�ϊ�
        /// <summary>
		/// ������𐔎��ɕϊ�
		/// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ��O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
		/// <param name="src">�ϊ���</param>
		/// <returns>�ϊ�����</returns>
		public	static	int	ToInt(string	src)
		{
			return	int.Parse(src);
		}
		/// <summary>
		/// ������𐔎��ɕϊ�
		/// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ�f�t�H���g�l���ݒ肳���
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <param name="defval">�f�t�H���g�l</param>
        /// <returns>�ϊ�����</returns>
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
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ��O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <returns>�ϊ�����</returns>
        public static long ToLong(string src)
        {
            return long.Parse(src);
        }
        /// <summary>
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ�f�t�H���g�l���ݒ肳���
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <param name="defval">�f�t�H���g�l</param>
        /// <returns>�ϊ�����</returns>
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
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ��O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <returns>�ϊ�����</returns>
        public static decimal ToDecimal(string src)
        {
            return decimal.Parse(src);
        }
        /// <summary>
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ�f�t�H���g�l���ݒ肳���
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <param name="defval">�f�t�H���g�l</param>
        /// <returns>�ϊ�����</returns>
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
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ��O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <returns>�ϊ�����</returns>
        public static float ToFloat(string src)
        {
            return float.Parse(src);
        }
        /// <summary>
        /// ������𐔎��ɕϊ�
        /// </summary>
        /// <remarks>
        /// �ϊ��o���Ȃ��ꍇ�f�t�H���g�l���ݒ肳���
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <param name="defval">�f�t�H���g�l</param>
        /// <returns>�ϊ�����</returns>
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
        /// �����𕶎���ɕϊ�
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ���f�[�^</returns>
		public	static	string	ToString(int	src)
		{
			return	src.ToString();
		}
        /// <summary>
        /// �����𕶎���ɕϊ�
        /// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <param name="format">�����w��</param>
        /// <returns>�ϊ��㕶����</returns>
		public	static	string	ToString(int	src,string format)
		{
			return	src.ToString(format);
		}
		/// <summary>
		/// �������ShiftJis�̃o�C�g�z��ɕϊ�����
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>ShiftJIS������</returns>
        public static byte[] StringToBytes(string src)
		{
			return	System.Text.Encoding.GetEncoding("Shift_Jis").GetBytes(src);
		}
        /// <summary>
        /// ShiftJis�̃o�C�g�z���string�ɕϊ�����
        /// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>ShiftJIS������</returns>
        public static string ByteToString(byte[]    src)
        {
            return System.Text.Encoding.GetEncoding("Shift_Jis").GetString(src);
        }
        #endregion
        #region ������ϊ�
        /// <summary>
        /// ��������w�肵��bool�^�ɕϊ�
        /// </summary>
        /// <remarks>
        /// �f�t�H���g�l���w�肳��Ȃ��ꍇ�A
        /// �ϊ����s�ŗ�O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <param name="defval">�f�t�H���g�l</param>
        /// <returns>�ϊ�����</returns>
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
        /// ��������w�肵���񋓌^�ɕϊ�
        /// </summary>
        /// <remarks>
        /// �f�t�H���g�l���w�肳��Ȃ��ꍇ�A
        /// �ϊ����s�ŗ�O�𔭐�����<see cref="System.Int32.Parse(System.String)"/>
        /// </remarks>
        /// <param name="src">�ϊ���</param>
        /// <returns>�ϊ�����</returns>
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
        #region �_���v
        /// <summary>
		/// �o�C�g�z���Hex�_���v��������ɕϊ�
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ��㕶����</returns>
        public static string HexDumpStr(byte[] src)
		{
			return	HexDumpStr(src,src.Length);
		}
		/// <summary>
		/// �o�C�g�z���Hex�_���v��������ɕϊ�
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <param name="length">�ϊ����L��������</param>
        /// <returns>�ϊ��㕶����</returns>
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
		/// �o�C�g�z������P�U�o�C�g����Hex�_���v���X�g�ɕϊ�
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ��㕶����</returns>
        public static string HexDumpList(byte[] src)
		{
			return	HexDumpList(src,src.Length);
		}
        /// <summary>
        /// �o�C�g�z������P�U�o�C�g����Hex�_���v���X�g�ɕϊ�
        /// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <param name="length">�ϊ����L��������</param>
        /// <returns>�ϊ��㕶����</returns>
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
		/// byte�^�f�[�^��Hex�_���v����
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ��㕶����</returns>
        public static string HexDumpStr(byte src)
		{
			byte[]	b=new byte[1];
			b[0]	=	src;
			return	HexDumpStr(b);
		}
        /// <summary>
        /// HEX�_���v�ϊ��p������e�[�u��
        /// </summary>
        private const string HEXTbl = "0123456789ABCDEDFabcdef";
        /// <summary>
        /// HEX�_���v��byte�z��֕ϊ�����
        /// </summary>
        /// <param name="src">HEX�_���v</param>
        /// <returns>�ϊ����ꂽbyte�z��</returns>
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
        #region SQL�֘A
        /// <summary>
		/// �������SqlQuery�Ɏg�p�ł���悤�ɕϊ�
		/// </summary>
        /// <param name="src">�ϊ���������</param>
        /// <returns>�ϊ��㕶����</returns>
        public static string SqlStrLapping(string src)
		{
			return	"'"	+	src.Replace("'","''") + "'";
		}
		/// <summary>
		/// ���t�������SqlQuery�Ɏg�p�ł���悤�ɕϊ�
		/// </summary>
		/// <param name="src">�ϊ���������</param>
		/// <returns>�ϊ��㕶����</returns>
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
        #region ������`�F�b�N
        /// <summary>
		/// �j���[�����b�N�`�F�b�N
		/// </summary>
		/// <param name="str">�`�F�b�N������</param>
		/// <returns>�����݂̂Ȃ�true��Ԃ�</returns>
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
		/// �j���[�����b�N�`�F�b�N���J�b�g
		/// </summary>
		/// <param name="str">�`�F�b�N������</param>
		/// <returns>�����ȊO�����O����������</returns>
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
        #region �萔��`
        /// <summary>
        /// ��������������(RFC822)
        /// </summary>
        public static readonly string[] RFC822_MonthString =	{ "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        #endregion
        #region printf
        /// <summary>
        /// sprintf CRT���C�u�����Ăяo��
        /// </summary>
        /// <remarks>
        /// MSCRT���C�u����sprintf���Ăяo���܂��B�ڍׂ�sprintf���Q��
        /// </remarks>
        /// <param name="buffer">�ҏW��i�[�o�b�t�@</param>
        /// <param name="format">printf�݊��ҏW������</param>
        /// <returns>���ʕ�����</returns>
        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf(StringBuilder buffer, string format,__arglist);

        /// <summary>
        /// swprintf CRT���C�u�����Ăяo��
        /// </summary>
        /// <remarks>
        /// MSCRT���C�u����swprintf���Ăяo���܂��B�ڍׂ�sprintf���Q��
        /// </remarks>
        /// <param name="buffer">�ҏW��i�[�o�b�t�@</param>
        /// <param name="format">printf�݊��ҏW������</param>
        /// <returns>���ʕ�����</returns>
        [DllImport("msvcrt.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int swprintf(StringBuilder buffer, string format,__arglist);
        /*
        /// <summary>
        /// sprintf CRT���C�u�����Ăяo��
        /// </summary>
        /// <remarks>
        /// MSCRT���C�u����sprintf���Ăяo���܂��B�ڍׂ�sprintf���Q��
        /// </remarks>
        /// <param name="buffer">�ҏW��i�[�o�b�t�@</param>
        /// <param name="fmt">printf�݊��ҏW������</param>
        /// <param name="arg1">�ҏW�g�ݍ��ݕ�����</param>
        /// <returns>���ʕ�����</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1);

        /// <summary>
        /// swprintf CRT���C�u�����Ăяo��
        /// </summary>
        /// <remarks>
        /// MSCRT���C�u����swprintf���Ăяo���܂��B�ڍׂ�sprintf���Q��
        /// </remarks>
        /// <param name="buffer">�ҏW��i�[�o�b�t�@</param>
        /// <param name="fmt">printf�݊��ҏW������</param>
        /// <param name="arg1">�ҏW�g�ݍ��ݕ�����1</param>
        /// <param name="arg2">�ҏW�g�ݍ��ݕ�����2</param>
        /// <returns>���ʕ�����</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1, String arg2);

        /// swprintf CRT���C�u�����Ăяo��
        /// </summary>
        /// <remarks>
        /// MSCRT���C�u����swprintf���Ăяo���܂��B�ڍׂ�sprintf���Q��
        /// </remarks>
        /// <param name="buffer">�ҏW��i�[�o�b�t�@</param>
        /// <param name="fmt">printf�݊��ҏW������</param>
        /// <param name="arg1">�ҏW�g�ݍ��ݕ�����1</param>
        /// <param name="arg2">�ҏW�g�ݍ��ݕ�����2</param>
        /// <param name="arg3">�ҏW�g�ݍ��ݕ�����3</param>
        /// <returns>���ʕ�����</returns>
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, String fmt, String arg1, String arg2, String arg3);
        */

        #endregion
    }
}
