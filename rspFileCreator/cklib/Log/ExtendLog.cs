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
    /// ログ拡張モジュール規定クラス
    /// </summary>
    [Serializable]
    public abstract class ExtendLog
    {
        /// <summary>
        /// ロギングエンジン
        /// </summary>
        public LogingEngine engine;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ExtendLog()
        { }

        /// <summary>
        /// ログ開始処理
        /// </summary>
        /// <param name="config">ログ設定情報</param>
        public abstract void Start(ConfigInfo config);
        /// <summary>
        /// ログ終了処理
        /// </summary>
        /// <param name="config">ログ設定情報</param>
        public abstract void Terminate(ConfigInfo config);
        /// <summary>
        /// ログアイドル時フラッシュ処理
        /// </summary>
        /// <param name="config">ログ設定情報</param>
        public abstract void LogFlush(ConfigInfo config);
        /// <summary>
        /// ロギング処理メソッド
        /// </summary>
        /// <param name="formatedMessasge">編集済みメッセージ</param>
        /// <param name="Data">ロギング情報</param>
        /// <param name="Config">ログ設定情報</param>
        public abstract void Loging(string formatedMessasge, LogData Data, ConfigInfo Config);
    }
}
