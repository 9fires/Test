using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.Odbc
{
    /// <summary>
    /// DB処理スコープ制御
    /// </summary>
    public class OdbcDataTryScope<DBCONNECT> : DataTryScope<DBCONNECT, OdbcDataTransaction, OdbcException>
        where DBCONNECT : OdbcDataInstance, new()
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OdbcDataTryScope()
            : base()
        {
        }
        /// <summary>
        /// デッドロック判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool IsDeadLock(OdbcException exp)
        {
            foreach (OdbcError error in exp.Errors)
            {
                if (error.SQLState == "40001")
                    return true;    //  直列化失敗（デッドロック)
            }
            return false;
        }
        /// <summary>
        /// タイムアウト判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool IsTimeout(OdbcException exp)
        {
            return false;
        }
    }
}
