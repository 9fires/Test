using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using cklib;
using cklib.Framework;
using cklib.Log.Config;
namespace cklib.Log
{
	/// <summary>
	/// ���O�@�\���Ǘ�����
	/// </summary>
	public static class LogManager
	{
        /// <summary>
        /// ���O�C���X�^���X
        /// </summary>
        private static LogManagerEx log;
        /// <summary>
        /// �r������p
        /// </summary>
        private static object CriticalSection = new object();
		/// <summary>
		/// �����ݒ���
		/// </summary>
		public	static	ConfigInfo	Config
		{
			get	{	return	log.Config;	}
		}
        /// <summary>
        /// �f�t�H���g���O�R�[�h
        /// </summary>
        /// <remarks>
        /// �f�o�b�O���O�y�у��O�R�[�h�Ȃ��̃��O�Ɏg�p����郍�O�R�[�h<br/>
        /// �C�x���g���O���g�p���Ă���ꍇ�́A�R�[�h���蓖�Ă��K�{�Ȃ̂œK�؂ȃR�[�h�����蓖�Ă�K�v������<br/>
        /// </remarks>
        public static int DefaultLogCode
        {
            get { return log.DefaultLogCode; }
            set { log.DefaultLogCode = value; }
        }
        /// <summary>
        /// ���O�G���W���G���[�C�x���g�C���X�^���X
        /// </summary>
        public static LogEngineError LogEngineErrorEvent = null;
        /// <summary>
		/// �������A�v���P�[�V�����ݒ�����[�h����
		/// </summary>
		/// <returns></returns>
        public static bool Initialize()
		{
            lock (CriticalSection)
            {
                log = new LogManagerEx();
                if (LogEngineErrorEvent!=null)
                {
                    log.LogEngineErrorEvent = LogEngineErrorEvent;
                }
                return log.Initialize();
            }
		}
        /// <summary>
        /// �������A�v���P�[�V�����ݒ�����[�h����
        /// </summary>
        /// <param name="conf">�ݒ���C���X�^���X</param>
        /// <returns></returns>
        public static bool Initialize(ConfigInfo conf)
        {
            lock (CriticalSection)
            {
                log = new LogManagerEx();
                if (LogEngineErrorEvent != null)
                {
                    log.LogEngineErrorEvent = LogEngineErrorEvent;
                }
                return log.Initialize(conf);
            }
        }
        /// <summary>
        /// �ċN��
        /// </summary>
        /// <returns>����������</returns>
        public static bool ReStart()
        {
            lock (CriticalSection)
            {
                if (log != null)
                    return log.ReStart();
                else
                    return Initialize();
            }
        }
        /// <summary>
        /// �ċN��
        /// </summary>
        /// <param name="conf">�ݒ���C���X�^���X</param>
        /// <returns></returns>
        public static bool ReStart(ConfigInfo conf)
        {
            lock (CriticalSection)
            {
                if (log != null)
                    return log.ReStart(conf);
                else
                    return Initialize();
            }
        }

		/// <summary>
		/// ���O�G���W���̒�~
		/// </summary>
        public static void Terminate()
        {
            lock (CriticalSection)
            {
                if (log != null)
                {
                    log.Terminate();
                    log = null;
                }
            }
        }
	}
}
