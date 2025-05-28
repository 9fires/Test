using System;
using System.Collections;
using System.Configuration;
using cklib;
using cklib.Util;

namespace cklib.Util
{
	/// <summary>
	/// �ݒ���̎擾
	/// </summary>
	public static class ConfigUtil
	{
		/// <summary>
		/// �ݒ�l�擾�G���[�C�x���g�n���h����`
		/// </summary>
		public delegate void ConfigErrorEventHandler(object sender, ConfigErrorEventArgs e);
		/// <summary>
		/// �ݒ�l�擾�G���[�C�x���g�n���h����`
		/// </summary>
		public	static	event	ConfigErrorEventHandler	ConfigErrorEvent;
		/// <summary>
		/// �ݒ���̎擾
		/// </summary>
		/// <param name="name">�L�[��</param>
		/// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
		/// <returns>�擾���ꂽ�I�u�W�F�N�g</returns>
		public	static	object GetConfigObject(string name,object defaultvalue)
		{
			object	val;
			try
			{
                val = ConfigurationManager.AppSettings[name];
				if	(val==null)
					throw	new	Exception("�ݒ�Ȃ�");
			}
			catch	(Exception	exp)
			{
				if	(ConfigUtil.ConfigErrorEvent!=null)
				{
					if	(defaultvalue!=null)
						ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,defaultvalue.ToString(),exp));
					else
						ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,string.Empty,exp));
				}
				val	=	defaultvalue;
			}
			return	val;
		}
		/// <summary>
		/// �ݒ蕶����̎擾
		/// </summary>
		/// <param name="name">�L�[��</param>
		/// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
		/// <returns>�擾���ꂽ������</returns>
		public	static	string	GetConfigString(string name,string defaultvalue)
		{
			return	(string)GetConfigObject(name,defaultvalue);
		}
		/// <summary>
		/// �ݒ萔�l�̎擾
		/// </summary>
		/// <param name="name">�L�[��</param>
		/// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
		/// <returns>�擾���ꂽ���l</returns>
		public	static	int	GetConfigInt(string name,int defaultvalue)
		{
			try
			{
				return	int.Parse((string)GetConfigObject(name,defaultvalue));
			}
			catch	(Exception	exp)
			{
				if	(ConfigUtil.ConfigErrorEvent!=null)
				{
					ConfigUtil.ConfigErrorEvent(null,new cklib.Util.ConfigErrorEventArgs(name,defaultvalue.ToString(),exp));
				}
				return	defaultvalue;
			}
        }

        #region DotNet1.1�݊��p�N���X
        #pragma warning disable  618
        /// <summary>
        /// �O���ݒ���N���X
        /// ��DotNet1.1�݊��p�N���X
        /// </summary>
        public class ExtendConfig
        {
            /// <summary>
            /// �Z�N�V������
            /// </summary>
            protected string section;
            /// <summary>
            /// �Z�N�V�������擾�f�[�^
            /// </summary>
            protected IDictionary dic;
            /// <summary>
            /// �R���X�g���N�^
            /// </summary>
            /// <param name="sectionname">�Z�N�V������</param>
            public ExtendConfig(string sectionname)
            {
                section = sectionname;
                dic = (IDictionary)ConfigurationSettings.GetConfig(sectionname);
            }
            /// <summary>
            /// �ݒ���̎擾
            /// </summary>
            /// <param name="name">�L�[��</param>
            /// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
            /// <returns>�擾���ꂽ�I�u�W�F�N�g</returns>
            public object GetConfigObject(string name, object defaultvalue)
            {
                object val;
                try
                {
                    val = dic[name];
                    if (val == null)
                        throw new Exception("�ݒ�Ȃ�");
                }
                catch (Exception exp)
                {
                    if (ConfigUtil.ConfigErrorEvent != null)
                    {
                        if (defaultvalue != null)
                            ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, defaultvalue.ToString(), exp));
                        else
                            ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, string.Empty, exp));
                    }
                    val = defaultvalue;
                }
                return val;
            }
            /// <summary>
            /// �ݒ蕶����̎擾
            /// </summary>
            /// <param name="name">�L�[��</param>
            /// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
            /// <returns>�擾���ꂽ������</returns>
            public string GetConfigString(string name, string defaultvalue)
            {
                return (string)GetConfigObject(name, defaultvalue);
            }
            /// <summary>
            /// �ݒ萔�l�̎擾
            /// </summary>
            /// <param name="name">�L�[��</param>
            /// <param name="defaultvalue">���ݒ莞�̃f�t�H���g�l</param>
            /// <returns>�擾���ꂽ���l</returns>
            public int GetConfigInt(string name, int defaultvalue)
            {
                try
                {
                    return int.Parse((string)GetConfigObject(name, defaultvalue));
                }
                catch (Exception exp)
                {
                    if (ConfigUtil.ConfigErrorEvent != null)
                    {
                        ConfigUtil.ConfigErrorEvent(null, new cklib.Util.ConfigErrorEventArgs(name, defaultvalue.ToString(), exp));
                    }
                    return defaultvalue;
                }
            }
        }
        /// <summary>
        /// Default�t�ꗗ�`���̊g���ݒ���Q�ƃN���X
        /// </summary>
        public class ExtendListTypeConfig : ExtendConfig
        {
            /// <summary>
            /// �R���X�g���N�^
            /// </summary>
            /// <param name="sectionname">�Z�N�V������</param>
            public ExtendListTypeConfig(string sectionname)
                : base(sectionname)
            { }

            /// <summary>
            /// �o�[�W��������URL���擾����
            /// �w�肳�ꂽ�o�[�W���������݂��Ȃ��ꍇ�́A"Default"�ݒ�����[�h����
            /// </summary>
            public string this[string key]
            {
                get
                {
                    string ret = (string)dic[key];
                    if (ret == null)
                    {
                        ret = (string)dic["Default"];
                    }
                    return ret;
                }
            }
        }

        /// <summary>
        /// �񋓒�`�Q�ƃN���X
        /// </summary>
        public class EnumrateConfig : ExtendConfig, IEnumerable
        {
            /// <summary>
            /// ��`�f�[�^�̔z��
            /// </summary>
            private ArrayList msgs = new ArrayList();
            /// <summary>
            /// �R���X�g���N�^
            /// </summary>
            /// <param name="sectionname">�Z�N�V������</param>
            public EnumrateConfig(string sectionname)
                : base(sectionname)
            {
                ArrayList keys = new ArrayList();
                int i;
                int key;
                foreach (System.Collections.DictionaryEntry msg in dic)
                {
                    key = String.ToInt(msg.Key.ToString(), 0);
                    for (i = 0; i < keys.Count; i++)
                    {
                        if (key < (int)keys[i])
                            break;
                    }
                    keys.Insert(i, key);
                    msgs.Insert(i, msg.Value);
                }
            }
            /// <summary>
            /// �ݒ�̔z��A�N�Z�X
            /// </summary>
            public string this[int idx]
            {
                get
                {
                    return msgs[idx].ToString();
                }
            }
            /// <summary>
            /// ��`���̎擾
            /// </summary>
            /// <returns></returns>
            public int Count
            {
                get { return msgs.Count; }
            }
            /// <summary>
            /// IEnumerable���������o
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator()
            {
                return new TokenEnumerator(this);
            }
            /// <summary>
            /// IEnumerator�����N���X
            /// </summary>
            private class TokenEnumerator : IEnumerator
            {
                private int position = -1;
                private EnumrateConfig t;

                /// <summary>
                /// �R���X�g���N�^
                /// </summary>
                /// <param name="t"></param>
                public TokenEnumerator(EnumrateConfig t)
                {
                    this.t = t;
                }
                /// <summary>
                /// �|�W�V�����ړ�
                /// </summary>
                /// <returns></returns>
                public bool MoveNext()
                {
                    if (position < t.Count - 1)
                    {
                        position++;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                /// <summary>
                /// �|�W�V�������Z�b�g
                /// </summary>
                public void Reset()
                {
                    position = -1;
                }
                /// <summary>
                /// ���݂̃|�W�V�����̃I�u�W�F�N�g�̎擾
                /// </summary>
                public object Current
                {
                    get
                    {
                        return t[position];
                    }
                }
            }
        }
        #endregion DotNet1.1�݊��p�N���X
    }
}
