using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace cklib.Data.Odbc
{
    public class OdbcDataBase<DBCONNECT> : DataBase<OdbcDataTryScope<DBCONNECT>, DBCONNECT, OdbcDataConfigSection, DataConfigElement, OdbcDataTransaction, OdbcConnection, OdbcCommand, OdbcTransaction, OdbcParameter, OdbcDataReader, OdbcDataAdapter, OdbcException>
        where DBCONNECT : OdbcDataInstance, new()
 
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">セクション名</param>
        public OdbcDataBase(string SectionName)
            : base(SectionName)
        { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">セクション名</param>
        /// <param name="SystemLogSectionName">ログのキー名（設定のセクション名）</param>
        /// <param name="SqlLogSecitonName">SQLログのキー名（設定のセクション名）</param>
        public OdbcDataBase(string SectionName, string SystemLogSectionName, string SqlLogSecitonName)
            : base(SectionName, SystemLogSectionName, SqlLogSecitonName)
        { }


    }
}
