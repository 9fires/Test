using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util.ByteArray
{
    /// <summary>
    /// Networkオーダー(上位先順）変換クラス
    /// </summary>
    public class NetOrder : cklib.Util.ByteArray.ConvertBase
    {
        #region 変換共通処理
        /// <summary>
        /// T型→byte配列変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public override void FromNumeric<T, C>(T src, ref byte[] dest, int Offset)
        {
            C srcType = new C();
            srcType.SetValue(src);
            if (dest.Length<(Offset+srcType.Size()))
            {
                throw new IndexOutOfRangeException(string.Format("need distination size={0} size={1} offset={2}",srcType.Size(),dest.Length,Offset));
            }
            for (int i = 0; i < srcType.Size(); i++)
            {
                dest[Offset + ((srcType.Size()-1)-i)] = srcType.ByteData(i);
            }
        }
        /// <summary>
        /// byte配列→T型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <param name="dest">変換結果格納配列</param>
        /// <param name="Offset">格納先添え字オフセット</param>
        public override void ToNumeric<T, C>(byte[] src, int Offset, out T dest)
        {
            C srcType = new C();
            srcType.SetZero();
            if (src.Length < (Offset + srcType.Size()))
            {
                throw new IndexOutOfRangeException(string.Format("need distination size={0} size={1} offset={2}", srcType.Size(), src.Length, Offset));
            }
            for (int i = 0; i < srcType.Size(); i++)
            {
                srcType.SetByte(src[Offset + ((srcType.Size() - 1) - i)], i);

                //srcType.Add(src[Offset + ((srcType.Size()-1) - i)] << (i * 8));
            }
            dest = srcType.GetValue();
        }
        #endregion
    }
}
