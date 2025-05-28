using System;
using System.Threading;

namespace cklib.Framework.IPC
{
    /// <summary>
    /// ���O����EventObject�N���X
    /// </summary>
    /// <remarks>
    /// 2012/02/06 64bit���ւ̑Ή����l����unsafe�R�[�h����<br/>
    /// ���C�u�����݊��̂���EventWaitHandle�̔h���N���X�ɕύX
    /// </remarks>
	public class NamedEventObject : EventWaitHandle
    {
		///<summary>
        ///������ԁA�C�x���g�����w�肵��NamedEventObject�̃C���X�^���X���쐬���܂��B
		///</summary>
        ///<param name="fManual">true�Ŏ蓮���Z�b�g,false�������Z�b�g�̎w��</param>
		///<param name="initialState">������Ԃ��V�O�i����Ԃɂ���ꍇ��true�B</param>
        ///<param name="name">�C�x���g�̖��O�B</param>
		///<param name="createdNew">���䂪�Ԃ����Ƃ��A�C�x���g�I�u�W�F�N�g���V�����쐬���ꂽ�ꍇ��true���i�[����܂��B���łɓ����̃C�x���g�I�u�W�F�N�g�����݂��Ă����ꍇ��false���i�[����܂��B</param>
        public NamedEventObject(bool fManual, bool initialState, string name, out bool createdNew)
            : base(initialState, (fManual ? EventResetMode.ManualReset : EventResetMode.AutoReset), name, out createdNew)
        {
        }
        ///<summary>
        ///������ԁA�C�x���g�����w�肵��NamedEventObject�̃C���X�^���X���쐬���܂��B
        ///</summary>
        ///<param name="fManual">true�Ŏ蓮���Z�b�g,false�������Z�b�g�̎w��</param>
        ///<param name="initialState">������Ԃ��V�O�i����Ԃɂ���ꍇ��true�B</param>
        ///<param name="name">�C�x���g�̖��O�B</param>
        public NamedEventObject(bool fManual, bool initialState, string name)
            : base(initialState, (fManual ? EventResetMode.ManualReset : EventResetMode.AutoReset), name)
        {
        }
        ///<summary>
        ///������ԁA�C�x���g�����w�肵�Ď������Z�b�gNamedEventObject�̃C���X�^���X���쐬���܂��B
        ///</summary>
        ///<param name="initialState">������Ԃ��V�O�i����Ԃɂ���ꍇ��true�B</param>
        ///<param name="name">�C�x���g�̖��O�B</param>
        ///<param name="createdNew">���䂪�Ԃ����Ƃ��A�C�x���g�I�u�W�F�N�g���V�����쐬���ꂽ�ꍇ��true���i�[����܂��B���łɓ����̃C�x���g�I�u�W�F�N�g�����݂��Ă����ꍇ��false���i�[����܂��B</param>
        public NamedEventObject(bool initialState, string name, out bool createdNew)
            : base(initialState, EventResetMode.AutoReset, name, out createdNew)
        {
        }
        ///<summary>
        ///������ԁA�C�x���g�����w�肵�Ď������Z�b�gNamedEventObject�̃C���X�^���X���쐬���܂��B
        ///</summary>
        ///<param name="initialState">������Ԃ��V�O�i����Ԃɂ���ꍇ��true�B</param>
        ///<param name="name">�C�x���g�̖��O�B</param>
        public NamedEventObject(bool initialState, string name)
            : base(initialState, EventResetMode.AutoReset, name)
        {
        }
	}
}
