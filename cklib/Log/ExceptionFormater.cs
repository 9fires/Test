using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log
{
    /// <summary>
    /// 例外を書式化する
    /// </summary>
    [Serializable]
    public abstract class ExceptionFormater
    {
        /// <summary>
        /// フォーマッタが処理するクラスType
        /// </summary>
        /// <remarks>
        /// 派生クラスのコンストラクタで初期化する
        /// </remarks>
        public readonly Type ExceptionType;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExceptionFormater(Type ExceptionType)
        {
            this.ExceptionType = ExceptionType;
        }
        /// <summary>
        /// 書式化元の例外
        /// </summary>
        /// <remarks>
        /// ToPrintメソッドにより出力されない、Exceotion固有の情報を書式化する<br/>
        /// ToPrintで出力される情報の前に出力される。
        /// </remarks>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public abstract string Format(Exception exp);
    }
}
