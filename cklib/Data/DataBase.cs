using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using cklib.Log;

namespace cklib.Data
{
    /// <summary>
    /// データアクセス制御管理クラス
    /// </summary>
    /// <typeparam name="TRYSCOPE"></typeparam>
    /// <typeparam name="DBCONNECT"></typeparam>
    /// <typeparam name="DATACONFIG"></typeparam>
    /// <typeparam name="DATACONFIGELEMENT"></typeparam>
    /// <typeparam name="Data_Transaction"></typeparam>
    /// <typeparam name="DBConnection"></typeparam>
    /// <typeparam name="DBCommand"></typeparam>
    /// <typeparam name="DBTransaction"></typeparam>
    /// <typeparam name="DBParamater"></typeparam>
    /// <typeparam name="DBDataReader"></typeparam>
    /// <typeparam name="DBDataAdapter"></typeparam>
    /// <typeparam name="DBException"></typeparam>
    public class DataBase<TRYSCOPE, DBCONNECT, DATACONFIG, DATACONFIGELEMENT, Data_Transaction, DBConnection, DBCommand, DBTransaction, DBParamater, DBDataReader, DBDataAdapter, DBException>
        where TRYSCOPE : DataTryScope<DBCONNECT, Data_Transaction, DBException>, new()
        where DBCONNECT : DataInstance<Data_Transaction, DBConnection, DBCommand, DBTransaction, DBParamater, DBDataReader, DBDataAdapter, DATACONFIG, DATACONFIGELEMENT>, new()
        where DATACONFIG : DataConfigSection<DATACONFIGELEMENT>
        where DATACONFIGELEMENT : DataConfigElement
        where Data_Transaction : DataTransaction<DBTransaction>
        where DBConnection : DbConnection
        where DBCommand : DbCommand
        where DBTransaction : DbTransaction
        where DBParamater : DbParameter
        where DBDataReader : DbDataReader
        where DBDataAdapter : DbDataAdapter
        where DBException : DbException
    {
        /// <summary>
        /// データベース設定セクション名
        /// </summary>
        public readonly string SectionName;
        /// <summary>
        /// データベース設定
        /// </summary>
        private DATACONFIG DBConfig;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">データベース名（設定のセクション名）</param>
        /// <param name="LogName">ログのキー名（設定のセクション名）</param>
        /// <param name="SqlLogName">SQLログのキー名（設定のセクション名）</param>
        public DataBase(string SectionName, string LogName, string SqlLogName)
        {
            this.SectionName = SectionName;
            this.LoadConfig();
            this.DBConfig.Common.LogSectionName = LogName;
            this.DBConfig.Common.SqlLogSectionName = SqlLogName;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="SectionName">データベース名（設定のセクション名）</param>
        public DataBase(string SectionName)
        {
            this.SectionName = SectionName;
            this.LoadConfig();
        }

        /// <summary>
        /// 設定のロード
        /// </summary>.e
        void LoadConfig()
        {
            this.DBConfig = (DATACONFIG)System.Configuration.ConfigurationManager.GetSection(this.SectionName);
        }

        /// <summary>
        /// TryScopeインスタンスの生成
        /// </summary>
        /// <returns></returns>
        public virtual TRYSCOPE NewTryScope()
        {
            var dc = this.CreateTryScopeInstance();
            dc.NewDBConnectInstance = (sc) => { return this.NewDBInstance(); };
            dc.ErrorLogCode = this.LookupLogCode(this.DBConfig.Common.ErrorLogCode);
            dc.DBConnectErrorLogCode = this.LookupLogCode(this.DBConfig.Common.DBConnectErrorLogCode);
            dc.DBErrorLogCode = this.LookupLogCode(this.DBConfig.Common.DBErrorLogCode);
            dc.DeadLockErrorLogCode = this.LookupLogCode(this.DBConfig.Common.DeadLockErrorLogCode);
            dc.DeadLockRetryOverErrorLogCode = this.LookupLogCode(this.DBConfig.Common.DeadLockRetryOverErrorLogCode);
            dc.TimeoutErrorLogCode = this.LookupLogCode(this.DBConfig.Common.TimeoutErrorLogCode);
            dc.TimeoutRetryOverErrorLogCode = this.LookupLogCode(this.DBConfig.Common.TimeoutRetryOverErrorLogCode);
            dc.LogManagerKey = this.DBConfig.Common.LogSectionName;
            dc.EnableDeadLockRetry = this.DBConfig.Common.EnableDeadLockRetry;
            dc.DeadLockRetryCountLimit = this.DBConfig.Common.DeadLockRetryCountLimit;
            dc.DeadLockRetryDelayTime = this.DBConfig.Common.DeadLockRetryDelayTime;
            dc.EnableTimeoutRetry = this.DBConfig.Common.EnableTimeoutRetry;
            dc.TimoutRetryCountLimit = this.DBConfig.Common.TimoutRetryCountLimit;
            dc.TimeoutRetryDelayTime = this.DBConfig.Common.TimeoutRetryDelayTime;
            return dc;
        }
        /// <summary>
        /// インスタンス生成処理
        /// </summary>
        /// <returns></returns>
        protected virtual TRYSCOPE CreateTryScopeInstance()
        {
            return new TRYSCOPE();
        }
        LogCodes LookupLogCode(string logcode)
        {
            try
            {
                var strs = logcode.Split(",".ToCharArray());
                int pos = strs[0].LastIndexOf('.');
                var cls = strs[0].Substring(0, pos);
                var mem = strs[0].Substring(pos + 1);
                var type = Type.GetType(string.Format("{0},{1}", cls, strs[1]));
                return type.InvokeMember(mem, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetField, null, null, null) as LogCodes;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// DB接続インスタンスを生成する
        /// </summary>
        /// <returns></returns>
        public virtual DBCONNECT NewDBInstance()
        {
            return CreateDBInstnce();
        }

        /// <summary>
        /// インスタンス生成処理
        /// </summary>
        /// <returns></returns>
        protected virtual DBCONNECT CreateDBInstnce()
        {
            var type = typeof(DBCONNECT);
            var db = Activator.CreateInstance(type, this.DBConfig) as DBCONNECT;
            return db;
        }
    }
}
