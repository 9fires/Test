using System;
using System.Collections.Generic;
using System.Text;

namespace cklib.Log
{
    /// <summary>
    /// ���O�}�l�[�W�����������N������C���X�^���X�𐶐�����
    /// </summary>
    public  class LogManagerStarter : IDisposable
    {
        /// <summary>
        /// ���O�}�l�[�W���C���X�^���X
        /// </summary>
        public readonly LogManagerEx mng;
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public LogManagerStarter()
        {
            mng = new LogManagerEx();
            mng.Initialize();
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="conf">�������p�����[�^</param>
        public LogManagerStarter(Config.ConfigInfo conf)
        {
            mng = new LogManagerEx();
            mng.Initialize(conf);
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="section">�ݒ�Z�N�V������</param>
        public LogManagerStarter(string section)
        {
            mng = new LogManagerEx(section);
            mng.Initialize();
        }

        /// <summary>
        /// �f�B�X�g���N�^
        /// </summary>
        ~LogManagerStarter()
        {
            Dispose(false);
        }
        #region IDisposable �����o
        /// <summary>
        /// Dispose�����t���O
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// Dispose���\�b�h
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose�����̎���
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                ReleseResorce();
                this.mng.Terminate();
                this.mng.Dispose();
            }
            disposed = true;
        }
        /// <summary>
        /// ���\�[�X�������
        /// </summary>
        protected virtual void ReleseResorce()
        {
        }
        #endregion

    }
}
