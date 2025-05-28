using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace cklib.Data.OleDb
{
    /// <summary>
    /// OleDbExceptionフォーマッタ
    /// </summary>
    [Serializable]
    public class OleDbExceptionFormater : cklib.Log.ExceptionFormater
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OleDbExceptionFormater()
            : base(typeof(OleDbException))
        {}
        /// <summary>
        /// 書式化
        /// </summary>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public override string Format(Exception exp)
        {
            OleDbException e = (OleDbException)exp;
            StringBuilder stb = new StringBuilder();
            foreach (OleDbError error in e.Errors)
            {
                stb.AppendFormat("OleDb Error SQLSate:{0} NativeError:{1} Message:{2} Source:{3}\r\n",
                                    error.SQLState, error.NativeError, error.Message, error.Source);
            }
            return stb.ToString();
        }
    }
}
