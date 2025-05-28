using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cklib.Data.OleDb
{
    public class OleDbDataBase<DBCONNECT> : DataBase<OleDbDataTryScope<DBCONNECT>, DBCONNECT, OleDbDataConfigSection, DataConfigElement, OleDbDataTransaction, OleDbConnection, OleDbCommand, OleDbTransaction, OleDbParameter, OleDbDataReader, OleDbDataAdapter, OleDbException>
        where DBCONNECT : OleDbDataInstance, new()
 
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">セクション名</param>
        public OleDbDataBase(string SectionName)
            : base(SectionName)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">セクション名</param>
        /// <param name="SystemLogSectionName">ログのキー名（設定のセクション名）</param>
        /// <param name="SqlLogSecitonName">SQLログのキー名（設定のセクション名）</param>
        public OleDbDataBase(string SectionName, string SystemLogSectionName, string SqlLogSecitonName)
            : base(SectionName, SystemLogSectionName, SqlLogSecitonName)
        { }


    }
}
