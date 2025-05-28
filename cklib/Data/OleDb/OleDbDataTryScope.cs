using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data.OleDb
{
    /// <summary>
    /// DB処理スコープ制御
    /// </summary>
    public class OleDbDataTryScope<DBCONNECT> : DataTryScope<DBCONNECT, OleDbDataTransaction, OleDbException>
        where DBCONNECT:OleDbDataInstance,new()
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OleDbDataTryScope()
            : base()
        {
        }
        /// <summary>
        /// デッドロック判定
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override bool IsDeadLock(OleDbException exp)
        {
            foreach (OleDbError error in exp.Errors)
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
        protected override bool IsTimeout(OleDbException exp)
        {
            return false;
        }
    }
}
