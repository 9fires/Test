using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util.ByteArray
{
    /// <summary>
    /// Intelオーダー（下位先順）変換クラス
    /// </summary>
    public class IntelOrder : cklib.Util.ByteArray.ConvertBase
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
            for (int i = 0; i < srcType.Size(); i++)
            {
                if (dest.Length > i)
                {
                    if (dest.Length>(Offset+i))
                    {
                        dest[Offset + i] = srcType.ByteData(i);
                    }
                }
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
            for (int i = 0; i < srcType.Size(); i++)
            {
                if (src.Length > (Offset + i))
                {
                    srcType.SetByte(src[Offset + i],i);
                    //srcType.Add(src[Offset + i] << (i * 8));
                }
            }
            dest = srcType.GetValue();
        }
        #endregion
    }
}
