using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;

namespace cklib.Data.Odbc
{
    /// <summary>
    /// OdbcExceptionフォーマッタ
    /// </summary>
    [Serializable]
    public class OdbcExceptionFormater : cklib.Log.ExceptionFormater
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OdbcExceptionFormater()
            : base(typeof(OdbcException))
        {}
        /// <summary>
        /// 書式化
        /// </summary>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public override string Format(Exception exp)
        {
            OdbcException e = (OdbcException)exp;
            StringBuilder stb = new StringBuilder();
            foreach (OdbcError error in e.Errors)
            {
                stb.AppendFormat("Odbc Error SQLSate:{0} NativeError:{1} Message:{2} Source:{3}\r\n",
                                    error.SQLState, error.NativeError, error.Message, error.Source);
            }
            return stb.ToString();
        }
    }
}
