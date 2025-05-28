using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.Sql
{
    /// <summary>
    /// DB処理スコープ制御
    /// </summary>
    public class SqlDataTryScope<DBCONNECT> : DataTryScope<DBCONNECT, SqlDataTransaction, SqlException>
        where DBCONNECT : SqlDataInstance, new()
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlDataTryScope()
            : base()
        {
        }
        /// <summary>
        /// デッドロック判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool IsDeadLock(SqlException exp)
        {
            if (exp.Number == 1205)
                return true;
            return false;
        }
        /// <summary>
        /// タイムアウト判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool IsTimeout(SqlException exp)
        {
            if (exp.Number == -2)
                return true;
            return false;
        }
    }
}
