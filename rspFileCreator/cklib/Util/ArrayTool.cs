using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util
{
    /// <summary>
    /// Arrayユーティリティ
    /// </summary>
    public static class ArrayTool
    {
        /// <summary>
        /// 配列の一致検査を行う
        /// </summary>
        /// <remarks>
        /// 配列の有効長がlengthに満たない場合は、比較可能な範囲で比較を行う。<br/>
        /// 但し、配列1,2で比較可能長が異なる場合は、配列が不一致なので比較を中止しfalseを返す
        /// </remarks>
        /// <param name="p1">配列1</param>
        /// <param name="p1_start">配列1のチェック開始先頭位置</param>
        /// <param name="p2">配列2</param>
        /// <param name="p2_start">配列2のチェック開始先頭位置</param>
        /// <param name="length">比較を行う長さ</param>
        /// <returns>true:一致</returns>
        public static bool Equals<T>(T[] p1, int p1_start, T[] p2, int p2_start, int length)
        {
            int p1i = p1_start;
            int p2i = p2_start;
            int p1leng = ((p1_start + length) > p1.Length ? p1.Length : length);
            int p2leng = ((p2_start + length) > p2.Length ? p2.Length : length);
            if (p1leng!=p2leng)
            {   //  比較対象データ長が不一致
                return false;
            }
            for (int i = 0; i < p1leng; i++)
            {
                if (!p1[p1i+i].Equals(p2[p2i+i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 配列の一致検査を行う
        /// </summary>
        /// <remarks>
        /// 配列の有効長がlengthに満たない場合は、比較可能な範囲で比較を行う。<br/>
        /// 但し、配列1,2で比較可能長が異なる場合は、配列が不一致なので比較を中止しfalseを返す
        /// </remarks>
        /// <param name="p1">配列1</param>
        /// <param name="p2">配列2</param>
        /// <param name="length">比較を行う長さ</param>
        /// <returns>true:一致</returns>
        public static bool Equals<T>(T[] p1, T[] p2, int length)
        {
            return Equals<T>(p1, 0, p2, 0, length);
        }
    }
}
