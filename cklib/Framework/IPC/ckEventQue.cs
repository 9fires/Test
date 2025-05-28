using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace  cklib.Framework.IPC
{
	/// <summary>
	/// EventObject�ɂ��C�x���g�Ď��\��Queue
	/// </summary>
    /// <remarks>
    /// �X�V:2008/02/26 Queue�i������@�\�ǉ�
    /// �X�V:2008/07/08 Generic�ɕύX
    /// </remarks>
	public class ckEventQue<T>:IDisposable
	{
        /// <summary>
        /// �C�x���g�I�u�W�F�N�g�C���X�^���X
        /// </summary>
		private	ManualResetEvent	ev	=	null;
        /// <summary>
        /// �C�x���g�ێ�Queue����
        /// </summary>
		private Queue Que;
        /// <summary>
        /// Queue�T�C�Y�����p�Z�}�t�H
        /// </summary>
        private Semaphore sem = null;
        /// <summary>
        /// Queue�T�C�Y���
        /// </summary>
        private int QueueLimitSize = -1;
        /// <summary>
        /// Queue�T�C�Y���
        /// </summary>
        /// <remarks>
        /// �T�C�Y�̓��I�ύX�͕s��<br/>
        /// 0�ȉ��̒l�Ő���������Đݒ�̂݉�
        /// </remarks>
        public int QueueMaxSize
        {
            get
            {
                if (sem !=null)
                {
                    return QueueLimitSize;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                lock(this)
                {
                    if (sem == null)
                    {
                        if (value>=0)
                        {
                            QueueLimitSize = value;
                            sem = new Semaphore(QueueLimitSize, QueueLimitSize);
                        }
                    }
                    else
                    {
                        if (value < 0)
                        {   //  ��������
                            sem.Close();
                            sem = null;
                        }
                        else
                        {
                            throw new Exception("Already Initialized Limit Queue Max Size");
                        }
                    }
                }
            }
        }
		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public ckEventQue()
		{
            Initialize(-1);
		}
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="QueueLimit">�L���[�̒i�� -1���w�肵���ꍇ������</param>
        public ckEventQue(int QueueLimit)
        {
            Initialize(QueueLimit);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="QueueLimit">�L���[�̒i�� -1���w�肵���ꍇ������</param>
        private void Initialize(int QueueLimit)
        {
            QueueLimitSize = QueueLimit;
            ev = new ManualResetEvent(false);
            Que = new Queue();
            if (QueueLimit>=0)
            {
                sem = new Semaphore(QueueLimitSize, QueueLimitSize);
            }
        }
		/// <summary>
		/// �f�X�g���N�^
		/// </summary>
		~ckEventQue()
		{
			Dispose(false);
		}
		/// <summary>
		/// �ҋ@�p�n���h�����擾
		/// </summary>
		/// <returns></returns>
		public	WaitHandle	GetHandle()
		{
			lock(this)
			{
				return	ev;
			}
		}

		/// <summary>
		/// �f�[�^���i�[���C�x���g�𔭐�������
		/// </summary>
		/// <param name="obj">�f�[�^</param>
		public void Put(T obj)
		{
            this.Put(obj,Timeout.Infinite);
		}
        /// <summary>
        /// �f�[�^���i�[���C�x���g�𔭐�������
        /// </summary>
        /// <param name="obj">�f�[�^</param>
        /// <param name="WaitTime">�ҋ@����</param>
        /// <returns>������true,Put�^�C���A�E�g��false</returns>
        public bool Put(T obj, int WaitTime)
        {
            Semaphore isem = this.sem;
            if (isem != null)
            {
                try
                {
                    if (!isem.WaitOne(WaitTime, false))
                    {
                        return false;
                    }
                }
                catch (ObjectDisposedException)
                {   // �Z�}�t�H���j�����ꂽ
                }
            }
            lock (this)
            {
                if (ev != null)
                {
                    Que.Enqueue(obj);
                    ev.Set();
                }
            }
            return true;
        }
        /// <summary>
		/// QUE�Ƀf�[�^�����݂��邩�Ԃ�
		/// </summary>
		/// <returns></returns>
		public	bool	IsDataRegident()
		{
			lock(this)
			{
				if	(Que!=null)
				{
					if	(Que.Count==0)
						return	false;
					else
						return	true;
				}
				else
				{
					return	false;
				}
			}
		}
        /// <summary>
        /// �i�[�����f�[�^��Ԃ�
        /// </summary>
        /// <param name="WaitTime">�ҋ@����</param>
        /// <returns>�f�[�^</returns>
        public T Get(int WaitTime)
        {
            if (ev.WaitOne(WaitTime, false))
            {
                lock (this)
                {
                    return this.Get();
                }
            }
            return default(T);
        }
		/// <summary>
		/// �i�[�����f�[�^��Ԃ�
		/// </summary>
		/// <returns>�f�[�^</returns>
		public T Get()
		{
			T	obj;
			lock(this)
			{
				if	(ev!=null)
				{
                    if (Que.Count==0)
                    {
                        return default(T);                       
                    }
					obj	=	(T)Que.Dequeue();
                    Semaphore isem = this.sem;
                    if (isem != null)
                    {
                        isem.Release();
                    }
                    if (Que.Count == 0)
					{
						Que.Clear();
						ev.Reset();
                    }
                    Que.TrimToSize();
					return obj;
				}
				else
				{
                    return default(T);
				}
			}
		}
		#region IDisposable �����o
		/// <summary>
		/// Dispose�����t���O
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Dispose�����̎���
		/// Que�ƃC�x���g�I�u�W�F�N�g��j������
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		/// <summary>
		/// Dispose�����̎���
		/// </summary>
		/// <param name="disposing">�蓮�J�����f�B�X�g���N�^���̎���</param>
		private void Dispose(bool disposing)
		{
			if(!disposed)
			{
				if(disposing)
				{
				}
				if	(ev!=null)
				{
					ev.Close();
					ev	=	null;
				}
				if	(Que!=null)
				{
					Que.Clear();
					Que=null;
				}
                Semaphore isem = this.sem;
                if (isem != null)
                {
                    isem.Close();
                    isem = null;
                }
            }
			disposed = true;     
		}

		#endregion
	}
}
