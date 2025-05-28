using System;
using cklib;

namespace cklib.Log
{
	/// <summary>
	/// LogLevel の概要の説明です。
	/// </summary>
	public enum	LogLevel:int
	{
        /// <summary>
        /// TRACEレベルデバッグ
        /// </summary>
        TRACE = 0,
        /// <summary>
        /// デバッグ
        /// </summary>
		DEBUG=1,
        /// <summary>
        /// 情報
        /// </summary>
		INFO=2,
        /// <summary>
        /// 通知
        /// </summary>
        NOTE=3,
        /// <summary>
        /// 警告
        /// </summary>
        WARN=4,
        /// <summary>
        /// エラー
        /// </summary>
        ERROR=5,
        /// <summary>
        /// 危機的
        /// </summary>
        CRIT=6,
        /// <summary>
        /// 警戒
        /// </summary>
        ALERT=7,
        /// <summary>
        /// 緊急
        /// </summary>
    　  EMERG=8,
        /// <summary>
        /// 致命的エラー
        /// </summary>
        FATAL= 9,
        /// <summary>
        /// 未定義
        /// </summary>
        Undefine = -1,
	}
}
