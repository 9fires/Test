using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Util.ByteArray
{
    /// <summary>
    /// 演算インターフェース
    /// </summary>
    /// <typeparam name="T">型指定</typeparam>
    public interface ICalculator<T>
    {
        /// <summary>
        /// 値のクリア
        /// </summary>
        void SetZero();
        /// <summary>
        /// 値設定
        /// </summary>
        /// <param name="value">設定する値</param>
        void SetValue(T value);
        /// <summary>
        /// 値の取得
        /// </summary>
        /// <returns>データ</returns>
        T GetValue();
        ///// <summary>
        ///// 値加算
        ///// </summary>
        ///// <param name="value">加算する値</param>
        //void Add(T value);
        /// <summary>
        /// 指定位置に値をセット
        /// </summary>
        /// <param name="b">設定する値</param>
        /// <param name="i">設定する位置</param>
        void SetByte(byte b, int i);
        /// <summary>
        /// ビットシフト
        /// </summary>
        /// <param name="offset">オフセット</param>
        /// <returns>シフト後のデータ</returns>
        byte ByteData(int offset);
        /// <summary>
        /// 型Tのbyte幅
        /// </summary>
        /// <returns>バイト幅数</returns>
        int Size();
    }
    /// <summary>
    /// int型用計算クラス
    /// </summary>
    internal struct IntCalculator : ICalculator<int>
    {
        int val;
        public void SetZero()
        { val = 0; }
        public void SetValue(int value)
        { val = value; }
        public int GetValue()
        { return val; }
        //public void Add(int value)
        //{ val += value; }
        public void SetByte(byte b, int p)
        { val += (b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 4; }
    }
    /// <summary>
    /// uint型用計算クラス
    /// </summary>
    internal struct UIntCalculator : ICalculator<uint>
    {
        uint val;
        public void SetZero()
        { val = 0; }
        public void SetValue(uint value)
        { val = value; }
        public uint GetValue()
        { return val; }
        //public void Add(uint value)
        //{ val += (uint)value; }
        public void SetByte(byte b, int p)
        { val += ((uint)b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 4; }
    }
    /// <summary>
    /// short型用計算クラス
    /// </summary>
    internal struct ShortCalculator : ICalculator<short>
    {
        short val;
        public void SetZero()
        { val = 0; }
        public void SetValue(short value)
        { val = value; }
        public short GetValue()
        { return val; }
        //public void Add(short value)
        //{ val += (short)value; }
        public void SetByte(byte b, int p)
        { val += (short)(b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 2; }
    }
    /// <summary>
    /// ushort型用計算クラス
    /// </summary>
    internal struct UShortCalculator : ICalculator<ushort>
    {
        ushort val;
        public void SetZero()
        { val = 0; }
        public void SetValue(ushort value)
        { val = value; }
        public ushort GetValue()
        { return val; }
        //public void Add(ushort value)
        //{ val += (ushort)value; }
        public void SetByte(byte b, int p)
        { val += (ushort)(b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 2; }
    }
    /// <summary>
    /// short型用計算クラス
    /// </summary>
    internal struct LongCalculator : ICalculator<long>
    {
        long val;
        public void SetZero()
        { val = 0; }
        public void SetValue(long value)
        { val = value; }
        public long GetValue()
        { return val; }
        //public void Add(long value)
        //{ val += (long)value; }
        public void SetByte(byte b, int p)
        { val += ((long)b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 8; }
    }
    /// <summary>
    /// ulong型用計算クラス
    /// </summary>
    internal struct ULongCalculator : ICalculator<ulong>
    {
        ulong val;
        public void SetZero()
        { val = 0; }
        public void SetValue(ulong value)
        { val = value; }
        public ulong GetValue()
        { return val; }
        //public void Add(ulong value)
        //{ val += (ulong)value; }
        public void SetByte(byte b, int p)
        { val += ((ulong)b << (p * 8)); }
        public byte ByteData(int offset)
        { return (byte)(val >> (offset * 8)); }
        public int Size()
        { return 8; }
    }
}
