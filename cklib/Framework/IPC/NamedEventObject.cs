using System;
using System.Threading;

namespace cklib.Framework.IPC
{
    /// <summary>
    /// 名前つきのEventObjectクラス
    /// </summary>
    /// <remarks>
    /// 2012/02/06 64bit環境への対応を考慮しunsafeコード除去<br/>
    /// ライブラリ互換のためEventWaitHandleの派生クラスに変更
    /// </remarks>
	public class NamedEventObject : EventWaitHandle
    {
		///<summary>
        ///初期状態、イベント名を指定してNamedEventObjectのインスタンスを作成します。
		///</summary>
        ///<param name="fManual">trueで手動リセット,false自動リセットの指定</param>
		///<param name="initialState">初期状態をシグナル状態にする場合はtrue。</param>
        ///<param name="name">イベントの名前。</param>
		///<param name="createdNew">制御が返されるとき、イベントオブジェクトが新しく作成された場合にtrueが格納されます。すでに同名のイベントオブジェクトが存在していた場合はfalseが格納されます。</param>
        public NamedEventObject(bool fManual, bool initialState, string name, out bool createdNew)
            : base(initialState, (fManual ? EventResetMode.ManualReset : EventResetMode.AutoReset), name, out createdNew)
        {
        }
        ///<summary>
        ///初期状態、イベント名を指定してNamedEventObjectのインスタンスを作成します。
        ///</summary>
        ///<param name="fManual">trueで手動リセット,false自動リセットの指定</param>
        ///<param name="initialState">初期状態をシグナル状態にする場合はtrue。</param>
        ///<param name="name">イベントの名前。</param>
        public NamedEventObject(bool fManual, bool initialState, string name)
            : base(initialState, (fManual ? EventResetMode.ManualReset : EventResetMode.AutoReset), name)
        {
        }
        ///<summary>
        ///初期状態、イベント名を指定して自動リセットNamedEventObjectのインスタンスを作成します。
        ///</summary>
        ///<param name="initialState">初期状態をシグナル状態にする場合はtrue。</param>
        ///<param name="name">イベントの名前。</param>
        ///<param name="createdNew">制御が返されるとき、イベントオブジェクトが新しく作成された場合にtrueが格納されます。すでに同名のイベントオブジェクトが存在していた場合はfalseが格納されます。</param>
        public NamedEventObject(bool initialState, string name, out bool createdNew)
            : base(initialState, EventResetMode.AutoReset, name, out createdNew)
        {
        }
        ///<summary>
        ///初期状態、イベント名を指定して自動リセットNamedEventObjectのインスタンスを作成します。
        ///</summary>
        ///<param name="initialState">初期状態をシグナル状態にする場合はtrue。</param>
        ///<param name="name">イベントの名前。</param>
        public NamedEventObject(bool initialState, string name)
            : base(initialState, EventResetMode.AutoReset, name)
        {
        }
	}
}
