using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util.ByteArray
{
    /// <summary>
    /// byte配列変換ユーティリティ(ビックエンディアン用)
    /// </summary>
    public static class Net
    {
        /// <summary>
        /// 変換クラスインスタンス
        /// </summary>
        static private NetOrder cnv = new NetOrder();
        /// <summary>
        /// 変換クラスインスタンス
        /// </summary>
        static public ConvertBase Converter
        {
            get { return cnv; }
        }
        #region long
        /// <summary>
        /// long→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromLong(long src, ref byte[] dest, int Offset)
        {
            cnv.FromLong(src, ref dest, Offset);
        }
        /// <summary>
        /// long→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromLong(long src, ref byte[] dest)
        {
            cnv.FromLong(src, ref dest);
        }
        /// <summary>
        /// byte配列→long変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToLong(byte[] src, int Offset, out long dest)
        {
            cnv.ToLong(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→long変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToLong(byte[] src, out long dest)
        {
            cnv.ToLong(src, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public long ToLong(byte[] src, int Offset)
        {
            return cnv.ToLong(src, Offset);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public long ToLong(byte[] src)
        {
            return cnv.ToLong(src);
        }
        #endregion
        #region ulong
        /// <summary>
        /// ulong→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromULong(ulong src, ref byte[] dest, int Offset)
        {
            cnv.FromULong(src, ref dest, Offset);
        }
        /// <summary>
        /// ulong→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromULong(ulong src, ref byte[] dest)
        {
            cnv.FromULong(src, ref dest);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToULong(byte[] src, int Offset, out ulong dest)
        {
            cnv.ToULong(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToULong(byte[] src, out ulong dest)
        {
            cnv.ToULong(src, out dest);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public ulong ToULong(byte[] src, int Offset)
        {
            return cnv.ToULong(src, Offset);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public ulong ToULong(byte[] src)
        {
            return cnv.ToULong(src);
        }
        #endregion
        #region int
        /// <summary>
        /// int→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromInt(int src, ref byte[] dest, int Offset)
        {
            cnv.FromInt(src, ref dest, Offset);
        }
        /// <summary>
        /// int→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromInt(int src, ref byte[] dest)
        {
            cnv.FromInt(src, ref dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToInt(byte[] src, int Offset, out int dest)
        {
            cnv.ToInt(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToInt(byte[] src, out int dest)
        {
            cnv.ToInt(src, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public int ToInt(byte[] src, int Offset)
        {
            return cnv.ToInt(src, Offset);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public int ToInt(byte[] src)
        {
            return cnv.ToInt(src);
        }
        #endregion
        #region uint
        /// <summary>
        /// uint→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromUInt(uint src, ref byte[] dest, int Offset)
        {
            cnv.FromUInt(src, ref dest, Offset);
        }
        /// <summary>
        /// uint→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromUInt(uint src, ref byte[] dest)
        {
            cnv.FromUInt(src, ref dest);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToUInt(byte[] src, int Offset, out uint dest)
        {
            cnv.ToUInt(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToUInt(byte[] src, out uint dest)
        {
            cnv.ToUInt(src, out dest);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public uint ToUInt(byte[] src, int Offset)
        {
            return cnv.ToUInt(src, Offset);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public uint ToUInt(byte[] src)
        {
            return cnv.ToUInt(src);
        }
        #endregion
        #region short
        /// <summary>
        /// short→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromShort(short src, ref byte[] dest, int Offset)
        {
            cnv.FromShort(src, ref dest, Offset);
        }
        /// <summary>
        /// short→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromShort(short src, ref byte[] dest)
        {
            cnv.FromShort(src, ref dest);
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToShort(byte[] src, int Offset, out short dest)
        {
            cnv.ToShort(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToShort(byte[] src, out short dest)
        {
            cnv.ToShort(src, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public short ToShort(byte[] src, int Offset)
        {
            return cnv.ToShort(src, Offset);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public short ToShort(byte[] src)
        {
            return cnv.ToShort(src);
        }
        #endregion
        #region ushort
        /// <summary>
        /// ushort→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        static public void FromUShort(ushort src, ref byte[] dest, int Offset)
        {
            cnv.FromUShort(src, ref dest, Offset);
        }
        /// <summary>
        /// ushort→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        static public void FromUShort(ushort src, ref byte[] dest)
        {
            cnv.FromUShort(src, ref dest);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToUShort(byte[] src, int Offset, out ushort dest)
        {
            cnv.ToUShort(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        static public void ToUShort(byte[] src, out ushort dest)
        {
            cnv.ToUShort(src, out dest);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        static public ushort ToUShort(byte[] src, int Offset)
        {
            return cnv.ToUShort(src, Offset);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        static public ushort ToUShort(byte[] src)
        {
            return cnv.ToUShort(src);
        }
        #endregion
    }
}
