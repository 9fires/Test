using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace cklib.Util
{
    /// <summary>
    /// CSVファイルロード    /// </summary>
    /// <remarks>
    /// rfc4180　http://www.ietf.org/rfc/rfc4180.txt/>
    /// </remarks>
    [Serializable]
    public class CSVDecoder
    {
        /// <summary>
        /// 項目一覧
        /// </summary>
        public List<List<string>> ItemList = new List<List<string>>();
        /// <summary>
        /// 行一覧
        /// </summary>
        public List<string> LineList = new List<string>();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>文字列から生成する</remarks>
        /// <param name="str">CSVイメージのテキスト</param>
        public CSVDecoder(string str)
        {
            InitializeAnalize();
            foreach (char ch in str)
            {
                this.StoreChar(ch);
            }
            TerminateAnalize();
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>エンコーディングを指定してファイルをロードする</remarks>
        /// <param name="strm">ロードするファイルストリームス</param>
        /// <param name="encoding">文字エンコード</param>
        public CSVDecoder(Stream strm, Encoding encoding)
        {
            InitializeAnalize();
            using (StreamReader sr = new StreamReader(strm, encoding))
            {
                while (!sr.EndOfStream)
                {
                    this.StoreChar((char)sr.Read());
                }
            }
            TerminateAnalize();
        }

        /// <summary>
        /// 解析フェーズ
        /// </summary>
        private enum AnalizePhase
        {
            idle = 0,
            field,
            strfield,
            quote,
            cr,
            lf,
        }
        /// <summary>
        /// 解析フェーズ値
        /// </summary>
        private AnalizePhase aPhase = AnalizePhase.idle;
        /// <summary>
        /// 現在行        /// </summary>
        private string currentLine = string.Empty;
        /// <summary>
        /// 現在の項目
        /// </summary>
        private string currentField = string.Empty;
        /// <summary>
        /// 現在行の項目一覧
        /// </summary>
        private List<string> currentItems;
        /// <summary>
        /// 解析初期化
        /// </summary>
        protected void InitializeAnalize()
        {
            this.aPhase = AnalizePhase.idle;
            this.InitalizeLine();
        }
        /// <summary>
        /// 解析終了処理        /// </summary>
        protected void TerminateAnalize()
        {
            if (aPhase != AnalizePhase.idle)
            {
                this.LineCommit();
            }
        }
        /// <summary>
        /// 行解析の初期化        /// </summary>
        protected void InitalizeLine()
        {
            this.currentLine = string.Empty;
            this.currentField = string.Empty;
            this.currentItems = new List<string>();
        }
        /// <summary>
        /// 行解析完了する        /// </summary>
        protected void LineCommit()
        {
            this.LineList.Add(this.currentLine);
            if (this.currentField.Length != 0)
            {
                this.currentItems.Add(this.currentField);
            }
            this.ItemList.Add(this.currentItems);
            this.InitalizeLine();
        }
        /// <summary>
        /// 文字をストアする
        /// </summary>
        /// <param name="ch"></param>
        protected void StoreChar(char ch)
        {
            switch (aPhase)
            {
                case AnalizePhase.idle:
                    switch (ch)
                    {
                        case '"':
                            this.currentLine += ch;
                            aPhase = AnalizePhase.strfield;
                            break;
                        case ',':
                            this.currentLine += ch;
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.aPhase = AnalizePhase.field;
                            break;
                        case (char)0x0d:
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.LineCommit();
                            aPhase = AnalizePhase.cr;
                            break;
                        case (char)0x0a:
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.LineCommit();
                            break;
                        default:
                            this.currentLine += ch;
                            this.currentField += ch;
                            aPhase = AnalizePhase.field;
                            break;
                    }
                    break;
                case AnalizePhase.field:
                    switch (ch)
                    {
                        case '"':
                            this.currentLine += ch;
                            if (this.currentField.Trim().Length == 0)
                            {   //  空文字
                                aPhase = AnalizePhase.strfield;
                                this.currentField = string.Empty;
                            }
                            else
                            {
                                this.currentField += ch;
                            }
                            break;
                        case ',':
                            this.currentLine += ch;
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.aPhase = AnalizePhase.field;
                            break;
                        case (char)0x0d:
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.LineCommit();
                            aPhase = AnalizePhase.cr;
                            break;
                        case (char)0x0a:
                            this.currentItems.Add(this.currentField);
                            this.currentField = string.Empty;
                            this.LineCommit();
                            break;
                        default:
                            this.currentLine += ch;
                            this.currentField += ch;
                            aPhase = AnalizePhase.field;
                            break;
                    }
                    break;
                case AnalizePhase.strfield:
                    this.currentLine += ch;
                    switch (ch)
                    {
                        case '"':
                            this.aPhase = AnalizePhase.quote;
                            break;
                        default:
                            this.currentField += ch;
                            break;
                    }
                    break;
                case AnalizePhase.quote:
                    switch (ch)
                    {
                        case '"':
                            this.currentLine += ch;
                            this.aPhase = AnalizePhase.strfield;
                            this.currentField += '"';
                            break;
                        default:
                            this.aPhase = AnalizePhase.field;
                            this.StoreChar(ch);
                            break;
                    }
                    break;
                case AnalizePhase.cr:
                    switch (ch)
                    {
                        case (char)0x0a:
                            this.aPhase = AnalizePhase.idle;
                            break;
                        case (char)0x0d:
                        default:
                            //  CR改行なので、前回改行を有効にする
                            this.aPhase = AnalizePhase.idle;
                            //  パラメータの文字は次行の先頭文字なので再度呼び出す。
                            this.StoreChar(ch);
                            break;
                    }
                    break;
            }
        }
    }
}
