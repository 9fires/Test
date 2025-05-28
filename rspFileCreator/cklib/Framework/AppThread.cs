#define NEWTYPE
using System;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using cklib;
using cklib.Framework.IPC;
using System.Runtime.InteropServices;

namespace cklib.Framework
{
    #region newlogic
    #if NEWTYPE
    /// <summary>
    /// �݊��p�N���X��`
    /// </summary>
    public abstract class AppThread : AppThreadBase<EventCode, object>
    {
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public AppThread()
            : base(EventCode.Start, EventCode.Stop, EventCode.Data,true)
        {
        }
        /// <summary>
        /// �X���b�h�J�n�C�x���g�i�I�v�V�����@�N�����������p)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventStart(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventStart(eed);
        }
        /// <summary>
        /// �X���b�h�J�n�C�x���g�i�I�v�V�����@�N�����������p)
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStart(EventDataType ed)
        {
            return true;
        }
        /// <summary>
        /// �X���b�h��~�C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventStop(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventStop(eed);
        }
        /// <summary>
        /// �X���b�h��~�C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventStop(EventDataType ed)
        {
            return false;
        }
        /// <summary>
        /// �f�[�^�C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventData(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventData(eed);
        }
        /// <summary>
        /// �f�[�^�C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventData(EventDataType ed)
        {
            return true;
        }
        /// <summary>
        /// ���[�U�[�g���C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected override bool EventUser(EventDataTypeBase<EventCode, object> ed)
        {
            EventDataType eed;
            eed.EventCode = ed.EventCode;
            eed.EventData = ed.EventData;
            return this.EventUser(eed);
        }
        /// <summary>
        /// ���[�U�[�g���C�x���g
        /// </summary>
        /// <param name="ed"></param>
        /// <returns></returns>
        protected virtual bool EventUser(EventDataType ed)
        {
            return true;
        }

    }
    #endif
    #endregion
    #region old
    #if !NEWTYPE
    /// <summary>
	/// �C�x���g�n���h����`
	/// </summary>
    public delegate void AppThreadEventHandler(AppThread sender,EventDataType  ed);
	/// <summary>
	/// ckAppThread �̊T�v�̐����ł��B
	/// </summary>
    /// <remarks>
    /// �X�V:2008/02/26 Queue�i������@�\�ǉ�
    /// </remarks>
    public abstract class AppThread : IDisposable
	{
		#region	���N���X���J�����o�[
		/// <summary>
		/// ���̃X���b�h�̖���
		/// </summary>
		public	String	Name;
		/// <summary>
		/// IPC�C�x���g�n���h��
		/// </summary>
		/// <remarks>
		/// Form��QUE����f�[�^���E���ƁA�u���b�N����Ă��܂��ꍇ��QUE�̕ς��Ƀf���Q�[�g�ɂ��ʒm���s�Ȃ��n���h���B<br/>
		/// ���̃n���h�����A�ݒ肳��Ă��Ȃ���FromQue�ɃC�x���g���ς܂�Ă��܂��̂ŁA�X���b�h���N������O�ɐݒ肷�邱�ƁB
		/// </remarks>
		public	event	AppThreadEventHandler	IPCEvent=null;
		#endregion
		#region	���J�C���i�[�N���X�ƌ^
		#endregion
		#region protected�����o�[
		/// <summary>
		/// IPC���C�x���gQUE
		/// </summary>
		protected	ckEventQue<EventDataType>	FromQue=null;
		/// <summary>
		/// IPC����C�x���gQUE
		/// </summary>
		protected	ckEventQue<EventDataType>	ToQue=null;
		/// <summary>
		/// �C�x���g�e�[�u�����ڒ�`
		/// </summary>
		protected	struct	EventTableItem
		{
            /// <summary>
            /// �C�x���gID
            /// </summary>
			public	int			EventID;
            /// <summary>
            /// �C�x���g�n���h��
            /// </summary>
			public	WaitHandle	Handle;
		};
		/// <summary>
		/// �C�x���g�e�[�u��
		/// </summary>
		protected	ArrayList	EventTable;
		/// <summary>
		/// �X���b�h�C���X�^���X
		/// </summary>
		protected	System.Threading.Thread thread=null;
		#endregion
        #region �R���X�g���N�^�E�f�X�g���N�^
        /// <summary>
		///	�R���X�g���N�^
		/// </summary>
		public AppThread()
		{
            FromQue = new ckEventQue<EventDataType>();
            ToQue = new ckEventQue<EventDataType>();
			EventTable=	new	ArrayList();
		}
		/// <summary>
		/// �f�B�X�g���N�^
		/// </summary>
		~AppThread()
		{
			Dispose(false);
        }
        #endregion �R���X�g���N�^�E�f�X�g���N�^
        #region �X���b�h�N�����~���䃁���o�[
        /// <summary>
		/// �X���b�h�̋N��
		/// </summary>
		/// <returns></returns>
		public virtual  bool	Start()
		{
			thread = new Thread(new ThreadStart(ThreadMain));
            thread.Name =   this.GetType().ToString();
            if (this.PreStart())
            {
                thread.Start();
                return true;
            }
            return false;
		}
        /// <summary>
        /// �X���b�h�J�n���O�ݒ菈��
        /// </summary>
        /// <returns></returns>
        protected virtual bool PreStart()
        {
            return true;
        }
		/// <summary>
		/// �X���b�h�̒�~
		/// </summary>
		/// <returns></returns>
		public	virtual bool Stop()
		{
			return	Stop(-1);
		}
		/// <summary>
		///	�X���b�h�̉ғ�������Ԃ�����
		/// </summary>
		public	virtual bool	IsAlive
		{
			get
            {
                if (thread!=null)
                { 
                    return	thread.IsAlive;
                }
                return false;
            }
		}
		/// <summary>
		/// �X���b�h�̒�~
		/// </summary>
		/// <param name="wTime">�҂�����(�~���b)</param>
		/// <returns></returns>
		public	virtual bool Stop(int	wTime)
		{
			if	(thread!=null)
			{
				IPCPut(EventCode.Stop,null);
				if	(wTime!=0)
				{
					return	StopWait(wTime);
				}
			}
			return	true;
		}
		/// <summary>
		/// �X���b�h��~�҂�
		/// </summary>
		/// <returns></returns>
		public	virtual bool	StopWait()
		{
			return	StopWait(-1);
		}
		/// <summary>
		/// �X���b�h��~�҂�
		/// </summary>
		/// <param name="wTime">�҂�����(�~���b)</param>
		/// <returns></returns>
		public	virtual bool	StopWait(int	wTime)
		{
			if	(thread!=null)
			{
				try
				{
					if	(thread.IsAlive)
					{
						if	(wTime==-1)
						{
							thread.Join();
						}
						else
						{
							if	(!thread.Join(wTime))
								return	false;
						}
					}
				}
				catch	//(Exception	exp)
				{
				}
				thread	=	null;
			}
			return	true;
		}
		#endregion
		#region �X���b�hIPC�p�����o�[
        /// <summary>
        /// IPCQueue�T�C�Y����̐ݒ�擾
        /// </summary>
        public int IPCQueueMaxSize
        {
            get
            {
                return this.ToQue.QueueMaxSize;
            }
            set
            {
                this.ToQue.QueueMaxSize = value;
            }
        }
		/// <summary>
		/// �X���b�h�ɃC�x���g��ʒm����
		/// </summary>
		/// <param name="ev">�C�x���g�R�[�h</param>
		/// <returns>true/false</returns>
		public	virtual	bool	IPCPut(EventCode	ev)
		{
			return	IPCPut(ev,null);
		}
		/// <summary>
		/// �X���b�h�ɃC�x���g��ʒm����
		/// </summary>
		/// <param name="ev">�C�x���g�R�[�h</param>
		/// <param name="data">�C�x���g�f�[�^</param>
		/// <returns>true/false</returns>
		public	virtual	bool	IPCPut(EventCode	ev,object data)
		{
			EventDataType	evd;
			evd.EventCode	=	ev;
			evd.EventData	=	data;
			ToQue.Put(evd);
			return	true;
		}
		/// <summary>
		/// �X���b�h���炱�̃X���b�h�̊Ď������ɑ΂��ăC�x���g��ʒm����
		/// </summary>
		/// <param name="ev">�C�x���g�R�[�h</param>
		/// <returns>true/false</returns>
		protected	virtual	bool	IPCResp(EventCode	ev)
		{
			return	IPCResp(ev,null);
		}
		/// <summary>
		/// �X���b�h���炱�̃X���b�h�̊Ď������ɑ΂��ăC�x���g��ʒm����
		/// </summary>
		/// <param name="ev">�C�x���g�R�[�h</param>
		/// <param name="data">�C�x���g�f�[�^</param>
		/// <returns>true/false</returns>
		protected	virtual	bool	IPCResp(EventCode	ev,object data)
		{
			EventDataType	evd;
			evd.EventCode	=	ev;
			evd.EventData	=	data;
			if	(this.IPCEvent==null)
			{
				FromQue.Put(evd);
			}
			else
			{	//	�C�x���g�n���h���̌Ăяo��	�񓯊��f�B�Q�[�g�ōs�Ȃ�
				this.IPCEvent.BeginInvoke(this,evd,new AsyncCallback(IPCEventCallback),IPCEvent);
			}
			return	true;
		}
		/// <summary>
		/// �񓯊��f���Q�[�g���ʂ��󂯎��
		/// </summary>
		/// <param name="ar"></param>
		protected	virtual	void	IPCEventCallback(IAsyncResult ar) 
		{
			AppThreadEventHandler	handler	=	(AppThreadEventHandler)ar.AsyncState;
			handler.EndInvoke(ar);
		}

		/// <summary>
		/// �X���b�h���̃C�x���g�̎擾
		/// </summary>
		/// <returns></returns>
		public	EventDataType	GetIPCEvent()
		{
			if	(FromQue.IsDataRegident())
				return	(EventDataType)FromQue.Get();
			else
			{
				return	new	EventDataType();
			}
		}
		/// <summary>
		/// �ҋ@�C�x���g�n���h�����擾����
		/// </summary>
		/// <returns></returns>
		public	WaitHandle GetEventHandle()
		{
			return	FromQue.GetHandle();
		}
		/// <summary>
		/// �e�X���b�h����̃C�x���g���o��
		/// </summary>
		/// <returns></returns>
		protected	EventDataType	IPCParentEventGet()
		{
			return	(EventDataType)ToQue.Get();
		}
		/// <summary>
		/// �e�X���b�h����̃C�x���g�̗L���̎Q��
		/// </summary>
		/// <returns></returns>
		protected	bool	IPCParentEventRedient()
		{
			return	ToQue.IsDataRegident();
		}

		#endregion
		#region �C�x���g�Ď��p�����o�[
		/// <summary>
		/// �ҋ@�C�x���g�̒ǉ�
		/// </summary>
		/// <param name="id">�C�x���gID</param>
		/// <param name="ev">�ҋ@�n���h��</param>
		protected	void	AddEventList(int	id,WaitHandle	ev)
		{
			int	i;
			EventTableItem	ei;
			ei.EventID	=	id;
			ei.Handle	=	ev;
			for	(i=0;i<EventTable.Count;i++)
			{
				if	(((EventTableItem)EventTable[i]).EventID==id)
				{
					EventTable[i]	=	ev;
					return;
				}
				else
				if	(((EventTableItem)EventTable[i]).EventID>id)
				{
					EventTable.Insert(i,ei);
					return;
				}
			}
			EventTable.Add(ei);
		}
		/// <summary>
		/// �ҋ@�C�x���g�̍폜
		/// </summary>
		/// <param name="id">�C�x���gID</param>
		protected	void	RemoveEventList(int	id)
		{
			int	i;
			for	(i=0;i<EventTable.Count;i++)
			{
				if	(((EventTableItem)EventTable[i]).EventID==id)
				{
					EventTable.RemoveAt(i);
					break;
				}
			}
		}
		/// <summary>
		/// �ҋ@�C�x���g�̃n���h���z��̎擾
		/// </summary>
		/// <returns>�C�x���g�z��</returns>
		protected	WaitHandle[]	GetEventHandles()
		{
			WaitHandle[]	wh=new	WaitHandle[EventTable.Count];
			int	i;
			for	(i=0;i<EventTable.Count;i++)
			{
				wh[i]	=	((EventTableItem)EventTable[i]).Handle;
			}
			return	wh;
		}
        /// <summary>
        /// �C�x���g�̑ҋ@
        /// </summary>
        /// <param name="WaitTime">�҂�����(�~���b)</param>
        /// <returns>�C�x���g��ԂƂȂ����n���h����EventTable��̃C���f�b�N�X</returns>
        protected int EventWait(int WaitTime)
		{
			try
			{
				return	WaitHandle.WaitAny(GetEventHandles(),WaitTime,false);
			}
			catch	(Exception	e)
			{
				return	WaitError(e);
			}
		}
		#endregion
		#region �ҋ@���Ԑ���p�̃��e�B���e�B�����o�[
        [DllImport("kernel32.dll", EntryPoint = "GetTickCount")]
        /// <summary>
        /// TickCount�̎擾
        /// </summary>
        /// <returns></returns>
        public extern static uint NativeGetTickCount();
        /// <summary>
		/// TickCount�̎擾
		/// </summary>
		/// <returns></returns>
		public	static	int	GetTickCount()
		{
			return	(int)NativeGetTickCount();
		}
		/// <summary>
		/// �ҋ@���Ԃ̌v�Z<br/>
		/// �v�Z���ʂ��AMinimumTime���Z�����Ԃ̏ꍇ�́A�v�Z���ʂ������łȂ���΁AMinimumTime��Ԃ�
		/// </summary>
		/// <param name="StartTime">�Ď��J�n���ԃ~���b</param>
		/// <param name="WaitTime">�ҋ@���ԃ~���b</param>
		/// <param name="MinimumTime">���̊Ď����ԃ~���b</param>
		/// <returns>�ҋ@���ԃ~���b</returns>
		public	static	int	CalcWaitTime(int	StartTime,int	WaitTime,int	MinimumTime)
		{
            if (WaitTime==Timeout.Infinite)
            {
                return Timeout.Infinite;
            }
			int	nowtm	=	GetTickCount();
			int	chktm	=	(nowtm-StartTime);
			int	wtm;
			if	(WaitTime>chktm)
			{
				wtm	=	WaitTime-chktm;
				if	(MinimumTime!=-1)
				{
					if	(MinimumTime<wtm)
					{
						return	MinimumTime;
					}
					else
					{
						return	wtm;
					}
				}
				else
				{
					return	wtm;
				}
			}
			return	0;
		}
		/// <summary>
		/// �ҋ@���Ԃ̌v�Z
		/// </summary>
		/// <param name="StartTime">�Ď��J�n���ԃ~���b</param>
		/// <param name="WaitTime">�ҋ@���ԃ~���b</param>
		/// <returns>�ҋ@���ԃ~���b</returns>
		public	static int	CalcWaitTime(int	StartTime,int	WaitTime)
		{
			return	CalcWaitTime(StartTime,WaitTime,WaitTime);
		}
		/// <summary>
		/// �ҋ@���Ԃ��o�߂������̔�����s�Ȃ�
		/// </summary>
		/// <param name="StartTime">�Ď��J�n���ԃ~���b</param>
		/// <param name="WaitTime">�ҋ@���ԃ~���b</param>
		/// <returns>�ҋ@���Ԃ��߂��Ă����true</returns>
		public	static	bool	IsWaitTimeOut(int	StartTime,int	WaitTime)
		{
			if	(CalcWaitTime(StartTime,WaitTime,WaitTime)==0)
				return	true;
			else
				return	false;
		}

		#endregion
		#region �����C�x���g���[�`��
		/// <summary>
		/// �C�x���g���C�����[�`��
		/// </summary>
		/// <param name="idx">EventTable��̃C���f�b�N�X</param>
		/// <returns>false�X���b�h�̏I��</returns>
		protected	bool	EventMain(int	idx)
		{
			try
			{
				return	Event((EventTableItem)EventTable[idx]);
			}
			catch	(Exception	e)
			{
				return	EventError(e);
			}
		}
		#endregion
		#region �X���b�h����
		/// <summary>
		/// �X���b�h���C�����[�`��
		/// </summary>
		public	virtual void ThreadMain()
		{
            try
            {
                if (InitInstanse())
                {
                    this.ThreadIdleLoop();
                }
                ExitInstance();
            }
            catch (Exception exp)
            {
                ThreadError(exp);
            }
			IPCResp(EventCode.Stop,null);
			return;
		}
		/// <summary>
		/// �X���b�h�A�C�h�����[�v
		/// </summary>
        public virtual void ThreadIdleLoop()
        {
            bool fLoop = true;
            for (; fLoop; )
            {
                if (BeforeIdle())
                {
                    continue;
                }
                fLoop = this.ThreadIdle(this.GetWaitTime());
            }
        }
        /// <summary>
        /// �X���b�h�A�C�h������
        /// </summary>
        /// <param name="WaitTime"></param>
        public virtual bool ThreadIdle(int WaitTime)
        {
            int ev = EventWait(WaitTime);
            switch (ev)
            {
                case WaitHandle.WaitTimeout:	//	�^�C���A�E�g
                    return EventTimeout();
                case -1:		//	�ҋ@�G���[
                    return false;
                default:	//	�C�x���g����
                    if (EventMain(ev))
                        return AfterIdle();
                    else
                        return false;
            }

        }
		#endregion
		#region �I�[�o�[���C�h�����o�[
		#region ��O�����̃I�[�o�[���C�h
		/// <summary>
		/// �ҋ@���̗�O�������̏���
		/// </summary>
		/// <returns></returns>
        protected abstract int WaitError(Exception exp);
		/// <summary>
		/// �C�x���g�������̗�O����������
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
        protected abstract bool EventError(Exception exp);
        /// <summary>
        /// catch����Ă��Ȃ���O����������
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected abstract void ThreadError(Exception exp);
        #endregion
		#region �N���������E�㏈��
		/// <summary>
		/// �X���b�h�̏���������
		/// </summary>
		/// <returns></returns>
		protected	virtual bool	InitInstanse()
		{
			AddEventList(0,ToQue.GetHandle());
			return	true;
		}
		/// <summary>
		/// �X���b�h�̏I������
		/// </summary>
		/// <returns></returns>
		protected	virtual bool	ExitInstance()
		{
			return	true;
		}
		#endregion
		#region �A�C�h��������
		/// <summary>
		/// �C�x���g�����O����
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	BeforeIdle()
		{
			return	false;
		}
		/// <summary>
		/// �C�x���g�����㏈��
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	AfterIdle()
		{
			return	true;
		}
		/// <summary>
		/// �ҋ@�^�C���A�E�g���Ԃ��~���b�ŕԂ�
		/// </summary>
		/// <returns></returns>
		protected	virtual	int	GetWaitTime()
		{
            return Timeout.Infinite;
		}
		/// <summary>
		/// �C�x���g�����^�C���A�E�g���̏���
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	EventTimeout()
		{
			return	true;
		}
		#endregion
		#region �C�x���g����
		/// <summary>
		/// �C�x���g����
		/// </summary>
		/// <param name="ei">�C�x���g</param>
		/// <returns></returns>
		protected	virtual	bool	Event(EventTableItem	ei)
		{
			if	(ei.EventID==0)
			{
				return	EventParent();		
			}
			return	true;
		}
		/// <summary>
		/// �e�X���b�h����̃C�x���g
		/// </summary>
		/// <returns></returns>
		protected	virtual	bool	EventParent()
		{
			EventDataType	ed	=	this.IPCParentEventGet();
			switch	(ed.EventCode)
			{
				case	EventCode.Start:
					return	EventStart(ed);
				case	EventCode.Stop:
					return	EventStop(ed);
				case	EventCode.Data:
					return	EventData(ed);
				default:
					return	EventUser(ed);
			}
//			return	true;
		}

		/// <summary>
		/// �X���b�h�J�n�C�x���g�i�I�v�V�����@�N�����������p)
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventStart(EventDataType ed)
		{
			return	true;
		}
		/// <summary>
		/// �X���b�h��~�C�x���g
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventStop(EventDataType ed)
		{
			return	false;
		}
		/// <summary>
		/// �f�[�^�C�x���g
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventData(EventDataType ed)
		{
			return	true;
		}
		/// <summary>
		/// ���[�U�[�g���C�x���g
		/// </summary>
		/// <param name="ed"></param>
		/// <returns></returns>
		protected	virtual	bool	EventUser(EventDataType ed)
		{
			return	true;
		}
		#endregion
		#endregion
		#region IDisposable �����o
		/// <summary>
		/// Dispose�����t���O
		/// </summary>
		private bool disposed = false;
		/// <summary>
		/// Dispose���\�b�h
		/// </summary>
		public virtual  void Dispose()
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
			if(!disposed)
			{
				//	�X���b�h��~����
				Stop();
				if(disposing)
				{
					ReleseManagedResorce();
				}
				ReleseResorce();
				this.ToQue.Dispose();
				this.FromQue.Dispose();
			}
			disposed = true;         
		}
		/// <summary>
		/// ���\�[�X�������
		/// </summary>
		protected	virtual	void	ReleseResorce()
		{
		}
		/// <summary>
		/// �}�l�[�W�h���\�[�X��������i�����I�Ăяo�����̂ݎ��s�����j
		/// </summary>
		protected	virtual	void	ReleseManagedResorce()
		{
		}

		#endregion
	}
    #endif
    #endregion
}
