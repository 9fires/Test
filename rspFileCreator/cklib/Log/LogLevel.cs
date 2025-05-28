using System;
using cklib;

namespace cklib.Log
{
	/// <summary>
	/// LogLevel �̊T�v�̐����ł��B
	/// </summary>
	public enum	LogLevel:int
	{
        /// <summary>
        /// TRACE���x���f�o�b�O
        /// </summary>
        TRACE = 0,
        /// <summary>
        /// �f�o�b�O
        /// </summary>
		DEBUG=1,
        /// <summary>
        /// ���
        /// </summary>
		INFO=2,
        /// <summary>
        /// �ʒm
        /// </summary>
        NOTE=3,
        /// <summary>
        /// �x��
        /// </summary>
        WARN=4,
        /// <summary>
        /// �G���[
        /// </summary>
        ERROR=5,
        /// <summary>
        /// ��@�I
        /// </summary>
        CRIT=6,
        /// <summary>
        /// �x��
        /// </summary>
        ALERT=7,
        /// <summary>
        /// �ً}
        /// </summary>
    �@  EMERG=8,
        /// <summary>
        /// �v���I�G���[
        /// </summary>
        FATAL= 9,
        /// <summary>
        /// ����`
        /// </summary>
        Undefine = -1,
	}
}
