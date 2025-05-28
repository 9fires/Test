using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.DB
{
    /// <summary>
    /// SQLトレース用ログコード
    /// </summary>
    public enum SQLTraceLogCode:int
    {
        /// <summary>
        /// 接続
        /// </summary>
        Connect = 0,
        /// <summary>
        /// トランザクション開始
        /// </summary>
        BeginTransaction = 1,
        /// <summary>
        /// SQLステートメント
        /// </summary>
        Statement=2,
        /// <summary>
        /// トランザクションコミット
        /// </summary>
        Commit=6,
        /// <summary>
        /// トランザクションロールバック
        /// </summary>
        RollBack = 7,
        /// <summary>
        /// 接続解放
        /// </summary>
        Close=8,
        /// <summary>
        /// エラー
        /// </summary>
        Error=9,
        /// <summary>
        /// メッセージ
        /// </summary>
        Message = 10,
    }
}
