using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util.ByteArray
{
    /// <summary>
    /// バイナリデータ並び順変換基本クラス
    /// </summary>
    public abstract class  ConvertBase
    {
        #region 変換共通処理
        /// <summary>
        /// T型→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        abstract public void FromNumeric<T, C>(T src, ref byte[] dest, int Offset)
            where C : ICalculator<T>, new();
        /// <summary>
        /// byte配列→T型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        abstract public void ToNumeric<T, C>(byte[] src, int Offset, out T dest)
            where C : ICalculator<T>, new();
        #endregion
        #region long
        /// <summary>
        /// long→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromLong(long src, ref byte[] dest, int Offset)
        {
            FromNumeric<long, LongCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// long→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromLong(long src, ref byte[] dest)
        {
            FromLong(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→long変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToLong(byte[] src, int Offset, out long dest)
        {
            ToNumeric<long, LongCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→long変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToLong(byte[] src, out long dest)
        {
            ToLong(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public long ToLong(byte[] src, int Offset)
        {
            long dest;
            ToLong(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public long ToLong(byte[] src)
        {
            return ToLong(src, 0);
        }
        #endregion
        #region ulong
        /// <summary>
        /// ulong→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromULong(ulong src, ref byte[] dest, int Offset)
        {
            FromNumeric<ulong, ULongCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// ulong→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromULong(ulong src, ref byte[] dest)
        {
            FromULong(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToULong(byte[] src, int Offset, out ulong dest)
        {
            ToNumeric<ulong, ULongCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→ulong変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToULong(byte[] src, out ulong dest)
        {
            ToULong(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public ulong ToULong(byte[] src, int Offset)
        {
            ulong dest;
            ToULong(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public ulong ToULong(byte[] src)
        {
            return ToULong(src, 0);
        }
        #endregion
        #region int
        /// <summary>
        /// int→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromInt(int src, ref byte[] dest, int Offset)
        {
            FromNumeric<int, IntCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// int→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromInt(int src, ref byte[] dest)
        {
            FromInt(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToInt(byte[] src, int Offset, out int dest)
        {
            ToNumeric<int, IntCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToInt(byte[] src, out int dest)
        {
            ToInt(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public int ToInt(byte[] src, int Offset)
        {
            int dest;
            ToInt(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→int変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public int ToInt(byte[] src)
        {
            return ToInt(src, 0);
        }
        #endregion
        #region uint
        /// <summary>
        /// uint→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromUInt(uint src, ref byte[] dest, int Offset)
        {
            FromNumeric<uint, UIntCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// uint→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromUInt(uint src, ref byte[] dest)
        {
            FromUInt(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToUInt(byte[] src, int Offset, out uint dest)
        {
            ToNumeric<uint, UIntCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToUInt(byte[] src, out uint dest)
        {
            ToUInt(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public uint ToUInt(byte[] src, int Offset)
        {
            uint dest;
            ToUInt(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→uint変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public uint ToUInt(byte[] src)
        {
            return ToUInt(src, 0);
        }
        #endregion
        #region short
        /// <summary>
        /// short→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromShort(short src, ref byte[] dest, int Offset)
        {
            FromNumeric<short, ShortCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// short→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromShort(short src, ref byte[] dest)
        {
            FromShort(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToShort(byte[] src, int Offset, out short dest)
        {
            ToNumeric<short, ShortCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToShort(byte[] src, out short dest)
        {
            ToShort(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public short ToShort(byte[] src, int Offset)
        {
            short dest;
            ToShort(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→short変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public short ToShort(byte[] src)
        {
            return ToShort(src, 0);
        }
        #endregion
        #region ushort
        /// <summary>
        /// ushort→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public void FromUShort(ushort src, ref byte[] dest, int Offset)
        {
            FromNumeric<ushort, UShortCalculator>(src, ref dest, Offset);
        }
        /// <summary>
        /// ushort→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        public void FromUShort(ushort src, ref byte[] dest)
        {
            FromUShort(src, ref dest, 0);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToUShort(byte[] src, int Offset, out ushort dest)
        {
            ToNumeric<ushort, UShortCalculator>(src, Offset, out dest);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="dest">変換結果格納先</param>
        public void ToUShort(byte[] src, out ushort dest)
        {
            ToUShort(src, 0, out dest);
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <param name="Offset">変換元添え字オフセット</param>
        /// <returns>変換結果</returns>
        public ushort ToUShort(byte[] src, int Offset)
        {
            ushort dest;
            ToUShort(src, Offset, out dest);
            return dest;
        }
        /// <summary>
        /// byte配列→ushort変換
        /// </summary>
        /// <param name="src">変換元配列</param>
        /// <returns>変換結果</returns>
        public ushort ToUShort(byte[] src)
        {
            return ToUShort(src, 0);
        }
        #endregion
    }
}
