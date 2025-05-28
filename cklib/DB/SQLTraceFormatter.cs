using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using cklib;
using cklib.Framework;
using cklib.Framework.IPC;
using cklib.Log;
using cklib.Log.Config;
using System.Reflection;

namespace cklib.DB
{
    /// <summary>
    /// 通信トレースログ内容を書式化する
    /// </summary>
    [Serializable]
    public class SQLTraceFormatter:Formatter
    {
        /// <summary>
        /// 書式化処理(バイト配列出力)
        /// </summary>
        /// <param name="Data">ログデータ</param>
        /// <param name="conf">該当出力先のログ設定</param>
        /// <param name="engine">ログエンジンインスタンス</param>
        /// <returns>書式化後データ</returns>
        public override byte[] Format(LogData Data, BasicLogConfig conf, LogingEngine engine)
        {
            //if (DBInstance.SQLTraceLogCodeBase!=Data.Code)
            //{
            //    return base.Format(Data, conf, engine);
            //}
            return conf.Encoding.GetBytes(this.FormatString(Data, conf, engine));
        }
        /// <summary>
        /// 書式化処理(文字列出力)
        /// </summary>
        /// <param name="Data">ログデータ</param>
        /// <param name="conf">該当出力先のログ設定</param>
        /// <param name="engine">ログエンジンインスタンス</param>
        /// <returns>書式化後データ</returns>
        public override string FormatString(LogData Data, BasicLogConfig conf, LogingEngine engine)
        {
            //if (DBInstance.SQLTraceLogCodeBase != Data.Code)
            //{
            //    return base.FormatString(Data, conf, engine);
            //}
            string DefaultFormat = "{0:yyyy/MM/dd HH:mm:ss}.{1:000} {11} {2,-6} {3:0000} {4} {5} {6}.{7} {8}{9}:{10} {12}";
            StringBuilder stb = new StringBuilder();
            string format = conf.Format;
            if (format.Length == 0)
            {
                format = engine.Config.Default.Format;
                if (format.Length == 0)
                {
                    format = DefaultFormat;
                }
            }
            stb.AppendFormat(format,
                                Data.Time,                                          //  0
                                Data.Time.Millisecond,                              //  1
                                Data.level.ToString(),                              //  2
                                Data.Code,                                          //  3
                                Data.Message,                                       //  4
                                Data.SourceName,                                    //  5
                                Data.ClassName,                                     //  6
                                Data.Method,                                        //  7
                                System.IO.Path.GetDirectoryName(Data.SourceFile),   //  8
                                System.IO.Path.GetFileName(Data.SourceFile),        //  9
                                Data.SourceLine,                                    //  10
                                Data.ThreadID,                                      //  11
                                Data.TraceBufferLength);                            //  12
            if (Data.TraceBufferLength!=0)
            {
                //  structure型をそのままログキューに渡した場合書式化する時点でデータ元のDataTableが解放されてしまう可能性がある為
                //  文字列化を行ってキューを行う以下編集処理を削除    2011/11/06
                #region 削除　2011/11/06
                //SqlParameter[] prm = Data.TraceBuffer as SqlParameter[];
                //foreach (SqlParameter var in prm)
                //{
                //    if (var.Value == null)
                //        stb.AppendFormat("\r\n{0}:{1}:NULL", var.ParameterName, var.SqlDbType);
                //    else
                //        switch (var.SqlDbType)
                //        {
                //            case SqlDbType.Variant:
                //            case SqlDbType.Image:
                //            case SqlDbType.VarBinary:
                //            case SqlDbType.Binary:
                //                stb.AppendFormat("\r\n{0}:{1}\r\n{2}", var.ParameterName, var.SqlDbType, cklib.Util.String.HexDumpList((byte[])var.Value));
                //                break;
                //            case SqlDbType.Timestamp:
                //                stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, cklib.Util.String.HexDumpStr((byte[])var.Value));
                //                break;
                //            case SqlDbType.Structured:
                //                stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, var.TypeName);
                //                if ((var.Value.GetType().Equals(typeof(DataTable))) ||
                //                    (var.Value.GetType().IsSubclassOf(typeof(DataTable))))
                //                {   //  データテーブルを展開表示する
                //                    DataTable dt = var.Value as DataTable;
                //                    stb.Append("\r\n\t");
                //                    foreach (DataColumn item in dt.Columns)
                //                    {
                //                        stb.AppendFormat("{0},", item.ColumnName);
                //                    }
                //                    foreach (DataRow row in dt.Rows)
                //                    {
                //                        stb.Append("\r\n\t");
                //                        for (int i = 0; i < row.ItemArray.Length; i++)
                //                        {
                //                            if (row.ItemArray[i].GetType().Equals(typeof(byte[])))
                //                                stb.AppendFormat("{0},", cklib.Util.String.HexDumpStr((byte[])row.ItemArray[i]));
                //                            else
                //                                stb.AppendFormat("{0},", row.ItemArray[i]);
                //                        }
                //                    }
                //                }
                //                break;
                //            case SqlDbType.Date:
                //            case SqlDbType.DateTime:
                //            case SqlDbType.DateTime2:
                //            case SqlDbType.DateTimeOffset:
                //            case SqlDbType.SmallDateTime:
                //            case SqlDbType.Time:
                //            case SqlDbType.BigInt:
                //            case SqlDbType.Bit:
                //            case SqlDbType.Char:
                //            case SqlDbType.Decimal:
                //            case SqlDbType.Float:
                //            case SqlDbType.Int:
                //            case SqlDbType.Money:
                //            case SqlDbType.NChar:
                //            case SqlDbType.NVarChar:
                //            case SqlDbType.Real:
                //            case SqlDbType.SmallInt:
                //            case SqlDbType.TinyInt:
                //            case SqlDbType.Udt:
                //            case SqlDbType.UniqueIdentifier:
                //            case SqlDbType.SmallMoney:
                //            case SqlDbType.VarChar:
                //            case SqlDbType.NText:
                //            case SqlDbType.Text:
                //            case SqlDbType.Xml:
                //            default:
                //                stb.AppendFormat("\r\n{0}:{1}:{2}", var.ParameterName, var.SqlDbType, var.Value);
                //                break;
                //        }                    
                //}
                #endregion
                if ((Data.TraceBuffer != null) && (Data.TraceBuffer.GetType().Equals(typeof(string))))
                    stb.Append((string)Data.TraceBuffer);
            }
            AppendExceptionData(Data, ref stb, engine);
            if (this.IsNeedNL(conf))
                stb.AppendFormat("\r\n");
            return stb.ToString();
        }
    }
}
