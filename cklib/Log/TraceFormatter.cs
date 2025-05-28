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

namespace cklib.Log
{
    /// <summary>
    /// 通信トレースログ内容を書式化する
    /// </summary>
    [Serializable]
    public class TraceFormatter:Formatter
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
            return conf.Encoding.GetBytes(this.FormatString(Data,conf,engine));
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
            string DefaultFormat = "{0:yyyy/MM/dd HH:mm:ss}.{1:000} {11} {2,-6} {3:0000} {4} {5} {6}.{7} {8}{9}:{10} {12}:{13}";
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
                                Data.TraceBufferLength,                             //  12
                                (Data.TraceBuffer == null ? string.Empty : cklib.Util.String.HexDumpStr((byte[])Data.TraceBuffer, Data.TraceBufferLength)));    //  13
            AppendExceptionData(Data, ref stb, engine);
            if (this.IsNeedNL(conf))
                stb.AppendFormat("\r\n");
            return stb.ToString();
        }
    }
}
