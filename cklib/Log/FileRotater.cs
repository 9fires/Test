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
    /// ファイルログをローテーションする
    /// </summary>
    [Serializable]
    public class FileRotater
    {
        /// <summary>
        /// ローテーション処理
        /// </summary>
        /// <remarks>
        /// 以下のタイミングで呼び出される<br/>
        /// ログ起動時、ログ記録が無い待機状態で日替わり発生時、ログ日付が変更した時<br/>
        /// デフォルトの実装では、RoteteDayを超えたファイルを削除と圧縮ファイル化する
        /// </remarks>
        /// <param name="Config">設定情報</param>
        /// <returns>成功時true</returns>
        public virtual bool Rotate(FileLogConfig Config)
        {
            //  古いログの削除処理
            if (Config.RotateDays > 0)
            {   //  ローテートを行う。
                DateTime rmLimitDate = DateTime.Today.Subtract(TimeSpan.FromDays(Config.RotateDays));
                DirectoryInfo dir = new DirectoryInfo(Config.Path);
                Regex regx1 = new Regex("{0.*}");
                string regx2str = "^"+regx1.Replace(Config.FileName.Replace(@"\", @"\\").Replace(".", @"\."), ".*")+"$";
                Regex regx2 = new Regex(regx2str);
                FileInfo[] finfo = dir.GetFiles("*.*");
                for (int i = 0; i < finfo.Length; i++)
                {
                    //if ((!regx2.IsMatch(finfo[i].Name)) && (!regx3.IsMatch(finfo[i].Name)))
                    if (!regx2.IsMatch(finfo[i].Name,0))
                    {
                        continue;
                    }
                    if (finfo[i].LastWriteTime.Date.CompareTo(rmLimitDate.Date) <= 0)
                    {
                        try
                        {
                            finfo[i].Delete();
                        }
                        catch (Exception exp)
                        {
                            System.Diagnostics.Debug.WriteLine(exp.ToString());
                            System.Console.Error.WriteLine(exp.ToString());
                        }
                    }
                }
                if (Util.File.IsSupportCompressFile(Util.File.ParsePathToDrive(Config.Path)))
                {
                    if (Config.Compress)
                    {
                        for (int i = 0; i < finfo.Length; i++)
                        {
                            if (!regx2.IsMatch(finfo[i].Name))
                            {
                                continue;
                            }
                            if ((finfo[i].Attributes & FileAttributes.Compressed) == FileAttributes.Compressed)
                            {
                                continue;
                            }
                            Util.File.FileCompress(finfo[i].FullName);
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// ファイルサイズローテーション処理
        /// </summary>
        /// <remarks>
        /// 以下のタイミングで呼び出される<br/>
        /// ログ書き込み時に指定サイズを超える可能性がある場合
        /// デフォルトの実装では、RoteteDayを超えたファイルを削除と圧縮ファイル化する
        /// ファイルサイズローテートの追加によりRotateSizeを超えるログ発生時にも呼び出される。
        /// </remarks>
        /// <param name="Config">設定情報</param>
        /// <param name="LogTime">SizeRotateに必要なログ日時</param>
        /// <returns>成功時true</returns>
        public virtual bool SizeRotate(FileLogConfig Config, DateTime LogTime)
        {
            //  ファイルサイズローテートの実施
            int No = 0;
            string LogFileName = string.Format(Config.FileName, LogTime);
            string LogPath = Config.Path + LogFileName;
            // 拡張子を除去
            int pos = LogFileName.LastIndexOf('.');
            if (pos != -1)
                LogFileName = LogFileName.Substring(0, pos);

            if (Regex.IsMatch(Config.RotateFileName, "{3.*}"))
            {   //  通番指定有
                string RotateCheckFileName = string.Format(Regex.Replace(Config.RotateFileName, "{[1-3].*}", "*"), LogFileName);
                DirectoryInfo dir = new DirectoryInfo(Config.Path);
                FileInfo[] finfo = dir.GetFiles(RotateCheckFileName);
                //  通番が拡張子となっているか確認する
                foreach (FileInfo item in finfo)
                {
                    pos = item.Name.LastIndexOf('.');
                    if (pos == -1) continue;
                    int pos2 = item.Name.LastIndexOf('.', pos - 1);
                    if (pos2 == -1) continue;
                    int no;
                    string nostr = item.Name.Substring(pos2 + 1, (pos - pos2) - 1);
                    if (!int.TryParse(nostr, out no))
                        continue;
                    if (no >= No)
                        No = no + 1;
                }
                string RotateFileName = string.Empty;
                for (; ; )
                {
                    try
                    {
                        RotateFileName = Config.Path + string.Format(Config.RotateFileName, LogFileName, LogTime, LogTime.Millisecond, No);
                        System.IO.File.Move(LogPath, RotateFileName);
                        break;
                    }
#pragma warning disable 0168
                    catch (FileNotFoundException exp)
                    {   //  ローテーション元ファイルが無い
                        break;
                    }
#pragma warning restore 0168
                    catch (IOException exp)
                    {   //  既に同じファイル名が存在する
                        if (System.IO.File.Exists(RotateFileName))
                        {
                            No++;
                            continue;
                        }
                        throw exp;
                    }
                }
            }
            else
                if (Regex.IsMatch(Config.RotateFileName, "{1.*}"))
                {
                    string RotateFileName = string.Empty;
                    for (; ; )
                    {
                        try
                        {
                            RotateFileName = Config.Path + string.Format(Config.RotateFileName, LogFileName, LogTime, LogTime.Millisecond, 0);
                            System.IO.File.Move(LogPath, RotateFileName);
                            break;
                        }
#pragma warning disable 0168
                        catch (FileNotFoundException exp)
                        {   //  ローテーション元ファイルが無い
                            break;
                        }
#pragma warning restore 0168
                        catch (IOException exp)
                        {   //  既に同じファイル名が存在する
                            if (System.IO.File.Exists(RotateFileName))
                            {
                                if (Regex.IsMatch(Config.RotateFileName, "{2.*}"))
                                    LogTime = LogTime.AddMilliseconds(1);
                                else
                                    LogTime = LogTime.AddSeconds(1);
                                continue;
                            }
                            throw exp;
                        }
                    }
                }
                else
                {   //  ローテート出来ないので行わない
                }
            return true;
        }
    }
}
