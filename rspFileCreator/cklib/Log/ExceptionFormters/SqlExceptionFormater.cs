using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace cklib.Log.ExceptionFormters
{
    /// <summary>
    /// SqlExceptionフォーマッタ
    /// </summary>
    [Serializable]
    public class SqlExceptionFormater:ExceptionFormater
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlExceptionFormater()
        :base(typeof(SqlException))
        {}
        /// <summary>
        /// 書式化
        /// </summary>
        /// <param name="exp">例外インスタンス</param>
        /// <returns>書式化データ</returns>
        public override string Format(Exception exp)
        {
            SqlException se = (SqlException)exp;
            StringBuilder stb = new StringBuilder();
            foreach (SqlError error in se.Errors)
            {
                stb.AppendFormat("Sql Error Code:{0} Class:{1} State:{2} Message:{3} Procedure:{4} LineNumber:{5} Server:{6}\r\n",
                                    error.Number, error.Class, error.State, error.Message, error.Procedure, error.LineNumber, error.Server);
            }
            return stb.ToString();
        }
    }
}
