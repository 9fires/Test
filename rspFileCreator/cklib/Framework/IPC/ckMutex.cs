using System;
using System.Threading;

namespace cklib.Framework.IPC
{
	/// <summary>
	/// Mutex���b�p�[�N���X
	/// </summary>
	public	class ckMutex
	{
		System.Threading.Mutex	mtx=null;
		/// <summary>
		/// ��̫�ĺݽ�׸�
		/// </summary>
		public ckMutex()
		{
			mtx	=	new	Mutex();
		}
		/// <summary>
		/// �������L���擾�w��t
		/// </summary>
		/// <param name="lk"></param>
		public ckMutex(bool	lk)
		{
			mtx	=	new	Mutex(lk);
		}
		/// <summary>
		/// ���O�t
		/// </summary>
		/// <param name="name"></param>
		/// <param name="lk"></param>
		public ckMutex(string name,bool lk)
		{
			mtx	=	new	Mutex(lk,name);
		}
		/// <summary>
		/// �f�B�X�g���N�^
		/// </summary>
		~ckMutex()
		{
			if	(mtx!=null)
			{
				mtx.Close();
			}
		}
		/// <summary>
		/// �ҋ@�n���h���̎擾
		/// </summary>
		/// <returns></returns>
		public	WaitHandle	GetHandle()
		{
			return	mtx;
		}
		/// <summary>
		/// ���b�N���s��
		/// </summary>
		/// <returns></returns>
		public	bool	Lock()
		{
			return	mtx.WaitOne();
		}
		/// <summary>
		/// ���b�N���s��
		/// </summary>
		/// <param name="wt">�ҋ@����(�~���b)</param>
		/// <returns></returns>
		public	bool	Lock(int	wt)
		{
			return	mtx.WaitOne(wt,false);
		}
        /// <summary>
        /// �~���[�e�b�N�X���
        /// </summary>
		public	void	UnLock()
		{
			mtx.ReleaseMutex();
		}
	}
}