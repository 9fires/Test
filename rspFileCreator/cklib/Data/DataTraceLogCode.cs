using cklib.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Data
{
    /// <summary>
    /// SQLトレース用ログコード
    /// </summary>
    public class DataTraceLogCode:LogCodes
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogCode"></param>
        private DataTraceLogCode(int LogCode)
            : base(LogCode)
        { }

        /// <summary>
        /// 接続
        /// </summary>
        public static readonly DataTraceLogCode Connect = new DataTraceLogCode(0);
        /// <summary>
        /// トランザクション開始
        /// </summary>
        public static readonly DataTraceLogCode BeginTransaction = new DataTraceLogCode(1);
        /// <summary>
        /// SQLステートメント
        /// </summary>
        public static readonly DataTraceLogCode Statement = new DataTraceLogCode(2);
        /// <summary>
        /// トランザクションコミット
        /// </summary>
        public static readonly DataTraceLogCode Commit = new DataTraceLogCode(6);
        /// <summary>
        /// トランザクションロールバック
        /// </summary>
        public static readonly DataTraceLogCode RollBack = new DataTraceLogCode(7);
        /// <summary>
        /// 接続解放
        /// </summary>
        public static readonly DataTraceLogCode Close = new DataTraceLogCode(8);
        /// <summary>
        /// エラー
        /// </summary>
        public static readonly DataTraceLogCode Error = new DataTraceLogCode(9);
        /// <summary>
        /// メッセージ
        /// </summary>
        public static readonly DataTraceLogCode Message = new DataTraceLogCode(10);
    }
}
