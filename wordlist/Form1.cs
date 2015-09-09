using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
// 
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Sql;
using System.Speech.Synthesis;
using System.IO;

namespace wordlist
{
    public partial class Form1 : Form
    {
        public Label lastSelected;
        string FROMTO = "";
        List<Word> wtl = new List<Word>();
        public string msg, wordsToLearn;
        int[] positions;
        public Controller c = new Controller();
        DateTime hold;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialise();
            //optAllWords.Checked = true;
            timerMaster.Enabled = true;
            /*string text = File.ReadAllText("ws2.txt");
            string[] lines = text.Split("\n".ToCharArray());
            foreach (string line in lines)
            {
                string[] t = line.Split(" ".ToCharArray(), 2);
                if (t[0] == "cabal")
                {
                    int k = 3;
                }
                Word w = new Word();
                w.word = t[0].Trim().ToLower();
                w.meaning = t[1].Trim().ToLower();
                
                c.AddWord(w);
            }
            */

        }

        private void cmbWordlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("SSS");
        }

        private void cmbWordlist_Click(object sender, EventArgs e)
        {
            Word w = c.GetWord(cmbWordlist.Items[cmbWordlist.SelectedIndex].ToString());
            if (w == null) return;
            lblWord.Text = w.word;
            lblMeaning.Text = w.meaning;
            lblPron.Text = w.pron;
            txtSentences.Text = w.sentence;

            Highlight();
        }
        public void Highlight()
        {
            int pos = txtSentences.Find(lblWord.Text, 0);
            while (pos != -1)
            {
                txtSentences.SelectionLength = lblWord.Text.Length;
                txtSentences.SelectionBackColor = Color.Yellow;
                txtSentences.SelectionColor = Color.Black;

                pos = txtSentences.Find(lblWord.Text, pos + 1, RichTextBoxFinds.None);

            }

        }
        public void EvaluateWritten()
        {
            int colActualWord = dgvWordlist.Columns.Count - 1;
            int colWord = colActualWord - 1;
                          
            int correct = 0, incorrect = 0, failed = 0, cnt = 1;
            dgvWordlist.Columns["colActualWord"].Visible = true;
            wtl.Clear();
            for (int i = 0; i < dgvWordlist.Rows.Count; ++i)
            {
                string a = "";
                string b = "";
                try
                {
                    a = dgvWordlist["colActualWord", i].Value.ToString().Trim();
                    b = dgvWordlist["colWord", i].Value.ToString().Trim();
                }
                catch (Exception dd)
                {
                }

                if (b == "")
                {
                    //wordsToLearn += cnt.ToString()+ "\t" + c.GetWord(dgvWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(a));
                    ++cnt;
                    ++failed;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGray;
                    dgvWordlist.Rows[i].DefaultCellStyle = cs;
                    //continue;
                }
                else if (a == b)
                {
                    ++correct;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGreen;
                    dgvWordlist.Rows[i].DefaultCellStyle = cs;
                }
                else
                {
                    //                    wordsToLearn += cnt.ToString() + "\t" + c.GetWord(dgvWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(a));
                    ++incorrect;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.IndianRed;
                    dgvWordlist.Rows[i].DefaultCellStyle = cs;

                }
                lblScore.Text = string.Format("{0} / {1} attempted, {2} failed", correct, correct + incorrect, failed);
            
                


            }


        }

        public void EvaluateMultipleChoice()
        {
                          
            int correct = 0, incorrect = 0, failed = 0, cnt = 1;
            dgvMultipleWordlist.Columns["colMultipleActualWord"].Visible = true;
            wtl.Clear();
            for (int i = 0; i < dgvMultipleWordlist.Rows.Count; ++i)
            {
                string a = "";
                string b = "";
                try
                {
                    a = dgvMultipleWordlist["colMultipleActualWord", i].Value.ToString().Trim();
                    b = dgvMultipleWordlist["colMultipleWord", i].Value.ToString().Trim();
                }
                catch (Exception dd)
                {
                }

                if (b == "")
                {
                    //wordsToLearn += cnt.ToString()+ "\t" + c.GetWord(dgvMultipleWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(a));
                    ++cnt;
                    ++failed;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGray;
                    dgvMultipleWordlist.Rows[i].DefaultCellStyle = cs;
                    //continue;
                }
                else if (a == b)
                {
                    ++correct;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGreen;
                    dgvMultipleWordlist.Rows[i].DefaultCellStyle = cs;
                }
                else
                {
                    //                    wordsToLearn += cnt.ToString() + "\t" + c.GetWord(dgvMultipleWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(a));
                    ++incorrect;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.IndianRed;
                    
                    dgvMultipleWordlist.Rows[i].DefaultCellStyle = cs;

                }
                lblScore.Text = string.Format("{0} / {1} attempted, {2} failed", correct, correct + incorrect, failed);
            }
                
        }

        //new function
        public void EvaluateSimpleTest()
        {

            int correct = 0, incorrect = 0, failed = 0, cnt = 1;
            dgvSimpleTest.Columns["colSimpleTestActualMeaning"].Visible = true;
            wtl.Clear();
            for (int i = 0; i < dgvSimpleTest.Rows.Count; ++i)
            {
                string a = "";
                string b = "";
                string word = "";
                try
                {
                    word = dgvSimpleTest["colSimpleTestWord", i].Value.ToString().Trim();
                    a = dgvSimpleTest["colSimpleTestActualMeaning", i].Value.ToString().Trim();
                    b = dgvSimpleTest["colSimpleTestMeaning", i].Value.ToString().Trim();                    
                }
                catch (Exception dd)
                {
                }

                if (b == "")
                {
                    //wordsToLearn += cnt.ToString()+ "\t" + c.GetWord(dgvMultipleWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(word));
                    ++cnt;
                    ++failed;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGray;
                    dgvSimpleTest.Rows[i].DefaultCellStyle = cs;
                    //continue;
                }
                else if (a == b)
                {
                    ++correct;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.LightGreen;
                    dgvSimpleTest.Rows[i].DefaultCellStyle = cs;
                }
                else
                {
                    //                    wordsToLearn += cnt.ToString() + "\t" + c.GetWord(dgvMultipleWordlist["colActualWord", i].Value.ToString()).Dump() + "\n";
                    wtl.Add(c.GetWord(word));
                    ++incorrect;
                    DataGridViewCellStyle cs = new DataGridViewCellStyle();
                    cs.BackColor = System.Drawing.Color.IndianRed;
                    dgvSimpleTest.Rows[i].DefaultCellStyle = cs;

                }
                lblScore.Text = string.Format("{0} / {1} attempted, {2} failed", correct, correct + incorrect, failed);
            }

        }


        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            ///Evaluate();
        }
        public Word[] shuffle(Word[] words, int len)
        {
            Random rd = new Random();
            Word[] w = new Word[len];
            Array.Copy(words, w, len);
            //Array.Clear(w, words, len);
            try
            {

                int max = len, i;
                //Array.Reverse(words);
                for (i = 0; i < len / 2; ++i)
                {
                    int a = (rd.Next(max) + rd.Next(max) - 1) % max;
                    int b = rd.Next(max);
                    Word t = words[b]; words[b] = words[a]; words[a] = t;
                }
                i = (rd.Next(max) + max - 1) % max;
                //if (i < 0) i = rd.Next(max);
                Array.Reverse(words, 0, i + 1);
                //if(rd.Next(2) == 0) 
                Array.Reverse(words, i, Math.Max(0, max - i - 1));
            }
            catch
            {
            }
            return words;

            int cnt = 0;
            //for (i = 0; i < words.Length; ++i) if (words[i] != w[i]) ++cnt;
            //MessageBox.Show(cnt + " out of " + words.Length + " mismatched");

            /*Random rd = new Random();
            int x =  words.Length;
            List<int> A = new List<int>();
            List<int> B = new List<int>();
            int i, j;
            for (i = 0; i < words.Length; ++i)
            {
                rd.Next();
                A.Insert(i, i);
            }
            for (i = 0; i < words.Length; ++i)
            {
                j = (rd.Next(A.Count)  + (A.Count - 1)) % A.Count;
                B.Insert(i, A[j]);
                A.Remove(j);
                A.Reverse();
                
            }
            Word[] w = new Word[words.Length];
            for (i = 0; i < B.Count; ++i)
            {
                w[i] = words[B[i]];
            }
            words = w;*/
        }
        void DisplayWords(int max, Word[] words)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            int i;
            dgvWordlist.Rows.Clear();
            dgvWordlist.Rows.Add(max);
            //for (i = 0, j = 0; (i < words.Length) && (j < max); ++i)
            
            for (i = 0; i < max; ++i)
            {
                // if (!starting.Contains(Char.ToLower(words[i].word[0]) + "")) continue;

                dgvWordlist["colSl", i].Value = (i + 1).ToString();
                string[] temp = words[i].meaning.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string toshow = temp[rd.Next(temp.Length)].Trim();

                //dgvWordlist["colMeaning", i].Value = words[i].meaning;
                dgvWordlist["colMeaning", i].Value = toshow;
                dgvWordlist["colActualWord", i].Value = words[i].word;
                dgvWordlist["colWordlength", i].Value = words[i].word.Length.ToString();
                //dgvWordlist["colSentence", i].Value = words[i].sentence;
                //++j;

            }

            dgvWordlist.ReadOnly = false;
            dgvWordlist.Columns["colSl"].ReadOnly = true;
            dgvWordlist.Columns["colMeaning"].ReadOnly = true;
            dgvWordlist.Columns["colWordLength"].ReadOnly = true;
            dgvWordlist.Select();
            //dgvWordlist.Refresh();
            dgvWordlist.CurrentCell = dgvWordlist[3, 0];
        }
        public void SetLoadingStatus()
        {
            lblStatus.Text = "Status: Loading... Please Wait";
        }
        public void SetIdleStatus()
        {
            lblStatus.Text = "Status: Idle";
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            
          

            //List<Word> wtl = new List<Word>();
            int i, j;
            Button btn = (Button)sender;
            string starting = "";
            if (btn.Text == "Start")
            {
                //new Thread(new ThreadStart(SetLoadingStatus)).Start(); 
                wordsToLearn = "";
                dgvWordlist.Refresh();
                dgvMultipleWordlist.Refresh();
                dgvSimpleTest.Refresh();

                string LIKE = "";
                FROMTO = "";
                if (chkRange.Checked && txtFrom.Text.Trim() != "" && txtTo.Text.Trim() != "")
                {
                    FROMTO = "(words.word >= '" + txtFrom.Text.Trim() + "' AND words.word <= '" + txtTo.Text.Trim() + "') ";

                }


                for (i = 0; i < chkList.CheckedItems.Count; ++i)
                {
                    char cc = Char.ToLower(chkList.CheckedItems[i].ToString()[0]);
                    starting += cc;
                    LIKE += "OR words.word LIKE '" + char.ToLower(cc) + "%' ";
                }
                if (LIKE.Length == 0)
                {
                    InvalidSelection();
                    return;
                }

                LIKE = LIKE.Substring(2);
                LIKE = "(" + LIKE;
                LIKE += ")";

                int m = -1;
                if(txtCount.Text != "All words")
                    m = Convert.ToInt32(txtCount.Text);

                dgvWordlist.Columns["colActualWord"].Visible = false;
                dgvMultipleWordlist.Columns["colMultipleActualWord"].Visible = false;
                dgvSimpleTest.Columns["colSimpleTestActualMeaning"].Visible = false;
                int version = 0;

                
                string where = " WHERE ";

                if (radioWS1.Checked) {
                    version = 1;
                    where += " version = " + version + " AND ";
                }
                else if (radioWS2.Checked) {
                    version = 2;
                    where += " version = " + version + " AND ";
                }
                
                
                
                

                Word[] words = c.GetAllWords(m, where + LIKE, FROMTO);

                if (words == null)
                {
                    InvalidSelection();
                    return;
                }
                int max = words.Length;
                Random rd = new Random((int)DateTime.Now.Ticks);

                int l = rd.Next(10);

                if (m < 0) m = max;
                
                
                max = Math.Min(m, max);
                
                if (optRandom.Checked)
                {

                    words = RandomSort(words, LIKE);
                    words = shuffle(words, words.Length);
                }
                else if (optInaccuracy.Checked)
                {
                    words = InaccuracySort(words, max, LIKE);
                    words = shuffle(words, Math.Min(max, words.Length));

                }
                else if (optLexicographical.Checked)
                {
                    words = LexicographicalSort(words, max, LIKE);
                    words = shuffle(words, words.Length);
                }
                else if (optLeastAttempted.Checked)
                {
                    words = LeastAttemptedSort(words, max, LIKE, version);
                    words = shuffle(words, Math.Min(words.Length, max));
                }
                max = Math.Min(max, words.Length);

                if (tabControl2.SelectedTab == tabpgWritingTest)
                {
                    //bckWorker.RunWorkerAsync();
                    DisplayWords(max, words);
                }
                else if (tabControl2.SelectedTab == tabpgMultipleChoice)
                {
                    DisplayMultipleChoiceWords(max, words);
                }
                else if (tabControl2.SelectedTab == tabpgSimpleTest)
                {
                    string[] relatedMeanings = c.GetRelatedMeanings(m, "WHERE " + LIKE, FROMTO);
                    DisplaySimpleTest(max, words, relatedMeanings);
                }
                else
                    DisplayGame(max, words);
                 
 
                //dgvWordlist.Rows.Clear();
                //dgvWordlist.Rows.Add(max);
                ////for (i = 0, j = 0; (i < words.Length) && (j < max); ++i)
                //for (i = 0; i < max; ++i)
                //{
                //   // if (!starting.Contains(Char.ToLower(words[i].word[0]) + "")) continue;

                //    dgvWordlist["colSl", i].Value = (i+1).ToString();
                //    string[] temp = words[i].meaning.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //    string toshow = temp[rd.Next(temp.Length)].Trim();

                //    //dgvWordlist["colMeaning", i].Value = words[i].meaning;
                //    dgvWordlist["colMeaning", i].Value = toshow;
                //    dgvWordlist["colActualWord", i].Value = words[i].word;
                //    dgvWordlist["colWordlength", i].Value = words[i].word.Length.ToString();
                //    //dgvWordlist["colSentence", i].Value = words[i].sentence;
                //    //++j;

                //}

                //dgvWordlist.ReadOnly = false;
                //dgvWordlist.Columns["colSl"].ReadOnly = true;
                //dgvWordlist.Columns["colMeaning"].ReadOnly = true;
                //dgvWordlist.Columns["colWordLength"].ReadOnly = true;
                //dgvWordlist.Select();
                ////dgvWordlist.Refresh();
                //dgvWordlist.CurrentCell = dgvWordlist[3, 0];

                lblTime.Text = "00:00:00";
                lblScore.Text = "";
                btn.Text = "Stop";
                //btnEvaluate.Enabled = false;
                hold = DateTime.Now;

                //dgvWordlist.Columns["colSl"].ReadOnly = true;
                timer.Start();

                //dgvWordlist.BeginEdit(false);
                lblWordsToLearn.Visible = false;
                //new Thread(new ThreadStart(SetIdleStatus)).Start(); 
            }
            else
            {
                if (MessageBox.Show("Are you sure you are done?", "End of Test", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return; ;
                btn.Text = "Start";
                if (tabControl2.SelectedTab == tabpgWritingTest)
                {
                    dgvWordlist.ReadOnly = true;
                    //btnEvaluate.Enabled = true;
                    timer.Stop();
                    EvaluateWritten();
                    UpdateReportWritten();
                    if (wtl.Count != 0)
                        lblWordsToLearn.Visible = true;
                }
                else if (tabControl2.SelectedTab == tabpgMultipleChoice)
                {
                    dgvMultipleWordlist.ReadOnly = true;
                    timer.Stop();
                    EvaluateMultipleChoice();
                    UpdateReportMultipleChoice();
                    if (wtl.Count != 0)
                        lblWordsToLearn.Visible = true;
                }
                else if(tabControl2.SelectedTab == tabpgSimpleTest)
                {
                    dgvSimpleTest.ReadOnly = true;
                    timer.Stop();
                    EvaluateSimpleTest();
                    UpdateReportSimpleTest();
                    if (wtl.Count != 0)
                        lblWordsToLearn.Visible = true;
                }

            }
        }

        private void UpdateReportMultipleChoice()
        {
            for (int i = 0; i < dgvMultipleWordlist.Rows.Count; ++i)
            {

                string a = "";
                string b = "";
                try
                {
                    a = dgvMultipleWordlist["colMultipleActualWord", i].Value.ToString();
                    b = dgvMultipleWordlist["colMultipleWord", i].Value.ToString();

                }
                catch (Exception dd)
                {
                }
                if (a != b)
                {
                    c.UpdateReport(b, 0, 1);
                }
                else c.UpdateReport(b, 1, 0);
            }
        }

        private void UpdateReportSimpleTest()
        {
            for (int i = 0; i < dgvSimpleTest.Rows.Count; ++i)
            {

                string a = "";
                string b = "";
                string word = "";
                try
                {
                    word = dgvSimpleTest["colSimpleTestWord", i].Value.ToString(); ;
                    a = dgvSimpleTest["colSimpleTestActualMeaning", i].Value.ToString();

                    if(dgvSimpleTest["colSimpleTestMeaning", i].Value != null)
                        b = dgvSimpleTest["colSimpleTestMeaning", i].Value.ToString();
                    
                }
                catch (Exception dd)
                {
                }
                if (a != b)
                {
                    
                    c.UpdateReport(word, 0, 1);
                }
                else c.UpdateReport(word, 1, 0);
            }
        }

        private void DisplayMultipleChoiceWords(int max, Word[] words)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            int i;
            dgvMultipleWordlist.Rows.Clear();
            dgvMultipleWordlist.Rows.Add(max);
            string[] allwords = c.GetOnlyWords();

            //for (i = 0, j = 0; (i < words.Length) && (j < max); ++i)
            for (i = 0; i < max; ++i)
            {
                // if (!starting.Contains(Char.ToLower(words[i].word[0]) + "")) continue;

                dgvMultipleWordlist["colMultipleSl", i].Value = (i + 1).ToString();
                string[] temp = words[i].meaning.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string toshow = temp[rd.Next(temp.Length)].Trim();

                //dgvMultipleWordlist["colMeaning", i].Value = words[i].meaning;
                dgvMultipleWordlist["colMultipleMeaning", i].Value = toshow;
                dgvMultipleWordlist["colMultipleActualWord", i].Value = words[i].word;
                dgvMultipleWordlist["colMultipleWordLength", i].Value = words[i].word.Length.ToString();
                
                List<string> meanings = new List<string>();
                //meanings.Add(words[i].word);
                meanings.Add(words[i].word);
                for (int j = 0; j < allwords.Length; ++j)
                {
                    int next = rd.Next(allwords.Length);
                    if(meanings.Contains(allwords[next])) continue;
                    meanings.Add(allwords[next]);
                    if(meanings.Count > 5) break;
                }
                meanings.RemoveAt(0);
                meanings.Insert(rd.Next(meanings.Count + 1), words[i].word);
                DataGridViewComboBoxCell cell = dgvMultipleWordlist.Rows[i].Cells["colMultipleWord"] as DataGridViewComboBoxCell;
                cell.Items.AddRange(meanings.ToArray());
                 
                //colMultipleWord.Items.AddRange(meanings.ToArray());
                //dgvMultipleWordlist["colMultipleWord", i].Value = meanings.ToArray();

                //dgvMultipleWordlist["colSentence", i].Value = words[i].sentence;
                //++j;

            }

            dgvMultipleWordlist.ReadOnly = false;
            dgvMultipleWordlist.Columns["colMultipleSl"].ReadOnly = true;
            dgvMultipleWordlist.Columns["colMultipleMeaning"].ReadOnly = true;
            dgvMultipleWordlist.Columns["colMultipleWordLength"].ReadOnly = true;
            dgvMultipleWordlist.Select();
            //dgvMultipleWordlist.Refresh();
            dgvMultipleWordlist.CurrentCell = dgvMultipleWordlist[3, 0];
        }

        //new function
        private void DisplaySimpleTest(int max, Word[] words, string[] relatedMeanings)
        {
            Random rd = new Random((int)DateTime.Now.Ticks);
            int i;
            dgvSimpleTest.Rows.Clear();
            dgvSimpleTest.Rows.Add(max);
            //string[] allwords = c.GetOnlyWords();
            //string[] relatedMeanings = c.GetRelatedMeanings();

            //for (i = 0, j = 0; (i < words.Length) && (j < max); ++i)
            for (i = 0; i < max; ++i)
            {
                // if (!starting.Contains(Char.ToLower(words[i].word[0]) + "")) continue;

                dgvSimpleTest["colSimpleTestSl", i].Value = (i + 1).ToString();
                string[] temp = words[i].meaning.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //string toshow = temp[rd.Next(temp.Length)].Trim();
                string toshow = words[i].meaning ;

                //dgvMultipleWordlist["colMeaning", i].Value = words[i].meaning;
                dgvSimpleTest["colSimpleTestActualMeaning", i].Value = toshow;
                dgvSimpleTest["colSimpleTestWord", i].Value = words[i].word;
                //dgvMultipleWordlist["colMultipleWordLength", i].Value = words[i].word.Length.ToString();

                List<string> meanings = new List<string>();
                //meanings.Add(words[i].word);
                int j = 0;
                meanings.Add(words[i].meaning);
                for (; ; ++j)
                {
                    int next = rd.Next(relatedMeanings.Length);
                    if (j > 100000000) break;

                    if (meanings.Contains(relatedMeanings[next])) continue;
                    //meanings.Insert(rd.Next(meanings.Count), words[i].meaning);
                    meanings.Add(relatedMeanings[next]);
                    if (meanings.Count == 5) break;
                }
                meanings.RemoveAt(0);
                meanings.Insert(rd.Next(meanings.Count + 1), words[i].meaning);

                DataGridViewComboBoxCell cell = dgvSimpleTest.Rows[i].Cells["colSimpleTestMeaning"] as DataGridViewComboBoxCell;
                cell.Items.AddRange(meanings.ToArray());

                //colMultipleWord.Items.AddRange(meanings.ToArray());
                //dgvMultipleWordlist["colMultipleWord", i].Value = meanings.ToArray();

                //dgvMultipleWordlist["colSentence", i].Value = words[i].sentence;
                //++j;

            }

            
            dgvSimpleTest.ReadOnly = false;
            dgvSimpleTest.Columns["colSimpleTestSl"].ReadOnly = true;
            dgvSimpleTest.Columns["colSimpleTestMeaning"].ReadOnly = false;
            
            dgvSimpleTest.Select();
            //dgvMultipleWordlist.Refresh();
            dgvSimpleTest.CurrentCell = dgvSimpleTest[2, 0];
        }

        private void DisplayGame(int max, Word[] w)
        {
            max = Math.Min(w.Length, max);
            Random rd = new Random();
            Label[] words = new Label[max * 2];
            bool[] isword = new bool[max * 2];
            //string[] mean = new string[max * 2];
            for (int i = 0; i < max; ++i)
            {
                words[i] = new Label();
                words[i].Text = w[i].word.ToUpper();
                
                isword[i] = true;
                isword[i + max] = false;
                string[] tmp = w[i].meaning.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                words[i + max] = new Label();
                words[i + max].Text = tmp[rd.Next(tmp.Length)];
                words[i + max].BackColor = Color.LightYellow;
                words[i].Tag = words[i + max].Text;
                words[i + max].Tag = words[i].Text;
            }
            max *= 2;
            for (int i = 0; i < max / 2; ++i)
            {
                int a = rd.Next(max);
                int b = rd.Next(max);
                Label t = words[a];
                words[a] = words[b];
                words[b] = t;
                bool tb = isword[a];
                isword[a] = isword[b];
                isword[b] = tb;
            }
            //max /= 2;
            int rows = (int)Math.Sqrt(max) + 1;
            int cols = 1;
            if (rows != 0) cols = max / rows;

            TableLayoutPanel tblPane = new TableLayoutPanel();
            //tblPane.Dock = DockStyle.Fill;

            tblPane.ColumnCount = cols;
            tblPane.RowCount = rows;


            tblPane.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            for (int i = 0; i < max; ++i)
            {
                if (i % cols == 0)
                {
                    tblPane.RowStyles.Add(new RowStyle(SizeType.Percent, (float)(100.0 / rows)));
                }
                Label tbl = words[i];
                /*tbl.Text = words[i];
                if (isword[i]) tbl.Tag = "WORD";
                else tbl.Tag = "MEANING";*/

                tbl.Dock = DockStyle.Fill;
                tbl.TextAlign = ContentAlignment.MiddleCenter;
                tbl.Click += new EventHandler(tbl_Click);
                //if (tbl.Tag == "WORD") tbl.Text = tbl.Text.ToUpper();
                tblPane.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)(100.0 / cols)));
                tblPane.Controls.Add(tbl);
            }
            tblPane.Dock = DockStyle.Fill;
            tabpgPickDaOne.Controls.Clear();
            tabpgPickDaOne.Controls.Add(tblPane);

        }

        void tbl_Click(object sender, EventArgs e)
        {
            Label s = (Label)sender;
            Color BGCOLOR = Color.DarkSeaGreen;
            if (s.BackColor == BGCOLOR || s == lastSelected) return;
            if (lastSelected == null)
            {
                lastSelected = s;
                s.BorderStyle = BorderStyle.FixedSingle;
            }
            else
            {
                if (((string)s.Tag == lastSelected.Text) || ((string)lastSelected.Tag == s.Text))
                {
                    lastSelected.BackColor = BGCOLOR;
                    s.BorderStyle = BorderStyle.None; ;
                    s.BackColor = BGCOLOR;

                }
                lastSelected.BorderStyle = BorderStyle.None;
                lastSelected = null;
                s.BorderStyle = BorderStyle.None;
            }


        }

        private void InvalidSelection()
        {
            MessageBox.Show("Your selection does not produce any question set. Please make sure you have entered proper values.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            new Thread(new ThreadStart(SetIdleStatus)).Start();
        }

        private Word[] LeastAttemptedSort(Word[] words, int limit, string like, int version)
        {
            return c.GetLeastAttemptedWords(limit, like, FROMTO, version);
        }

        private Word[] LexicographicalSort(Word[] words, int limit, string like)
        {
            string where = " WHERE ";
            int version = 0;
            if (radioWS1.Checked)
            {
                version = 1;
                where += " version = " + version + " AND ";
            }
            else if (radioWS2.Checked)
            {
                version = 2;
                where += " version = " + version + " AND ";
            }
            return c.GetAllWords(limit, where + like, FROMTO);
        }
        public void UpdateReportWritten()
        {
            
            for (int i = 0; i < dgvWordlist.Rows.Count; ++i)
            {

                string a = "";
                string b = "";
                try
                {
                    a = dgvWordlist["colActualWord", i].Value.ToString();
                    b = dgvWordlist["colWord", i].Value.ToString();

                }
                catch (Exception dd)
                {
                }
                if (a != b)
                {
                    c.UpdateReport(b, 0, 1);
                }
                else c.UpdateReport(b, 1, 0);
            }
        }

        public Word[] RandomSort(Word[] words, string like)
        {
            int version = 0;
            if (radioWS1.Checked)
            {
                version = 1;
             
            }
            else if (radioWS2.Checked)
            {
                version = 2;
             
            }
            return c.GetAllReportedWords(-1, like, FROMTO, version);

        }
        public Word[] InaccuracySort(Word[] words, int limit, string like)
        {
            
            int version = 0;
            if (radioWS1.Checked)
            {
                version = 1;
            
            }
            else if (radioWS2.Checked)
            {
                version = 2;
            
            }
            return c.GetAllReportedWords(limit, like, FROMTO, version);
            //words = c.GetAllReportedWords();

        }
        private void timer_Tick(object sender, EventArgs e)
        {

            TimeSpan ts = DateTime.Now.Subtract(hold);
            lblTime.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Word w = new Word();
            w.word = txtWord.Text.Trim().ToLower();
            w.meaning = txtMeaning.Text.Trim().ToLower();
            w.sentence = txtSentence.Text.Trim();
            w.pron = txtPron.Text.Trim();


            c.AddWord(w);
            txtWord.Text = "";
            txtMeaning.Text = "";
            txtSentence.Text = "";
            txtPron.Text = "";
            txtWord.Focus();


        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex != 0) return;
            Initialise();
        }
        public void Initialise()
        {
            cmbWordlist.Items.Clear();
            /*string constr = @"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\samir\Documents\wordlist.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            string query = "SELECT * FROM words";

            SqlDataAdapter sqlAdp = new SqlDataAdapter(query, constr);
            sqlAdp.SelectCommand.CommandText = query;
            DataSet toReturn = new DataSet();
            sqlAdp.Fill(toReturn);
            */
            Word[] words = c.GetAllWords(-1, "", "");
            foreach (Word w in words)
            {
                cmbWordlist.Items.Add(w.word);
            }
            /*for(int i = 0; i < 26; ++i)
                chkList.SetItemChecked(i, true);*/

        }

        private void cmbWordlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblWord_Click(object sender, EventArgs e)
        {



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbWordlist_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                Word w = c.GetWord(cmbWordlist.Items[cmbWordlist.SelectedIndex].ToString());
                if (w == null) return;
                lblWord.Text = w.word;
                lblMeaning.Text = w.meaning;
                lblPron.Text = w.pron;
                txtSentences.Text = w.sentence;

                Highlight();
            }
            catch (Exception ee)
            {
            }
        }

        private void cmbWordlist_Validating(object sender, CancelEventArgs e)
        {

        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Word w = new Word();
                w.meaning = lblMeaning.Text.Trim().ToLower();
                w.word = lblWord.Text.Trim().ToLower();
                w.sentence = txtSentences.Text.Trim();
                w.pron = lblPron.Text.Trim();
                c.UpdateWord(w, cmbWordlist.Text);
                MessageBox.Show("Word updated", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }
        public void ClearFields()
        {
            lblMeaning.Clear();
            txtSentences.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i, j;
            Word[] words = c.GetAllWords(-1, "", "");
            string[] keys = txtSearchWords.Text.Split(",:".ToCharArray());
            for (i = 0; i < keys.Length; ++i) keys[i] = keys[i].Trim();
            j = 0;
            dgvSearchResult.Rows.Clear();
            for (i = 0; i < words.Length; ++i)
            {
                for (int k = 0; k < keys.Length; ++k)
                {
                    if (words[i].meaning.Contains(keys[k]))
                    {
                        dgvSearchResult.Rows.Add();
                        dgvSearchResult["colSlS", j].Value = (1 + j).ToString();
                        dgvSearchResult["colWordS", j].Value = words[i].word;
                        dgvSearchResult["colPronS", j].Value = words[i].pron;
                        dgvSearchResult["colMeaningS", j].Value = words[i].meaning;
                        ++j;
                        break;
                    }
                }
            }
        }

        private void dgvWordlist_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            string word = ""; ;
            if (dgv.Name == "dgvWordlist")
                word = dgvWordlist["colActualWord", e.RowIndex].Value.ToString();
            else if (dgv.Name == "dgvMultipleWordlist")
            {
                word = dgvMultipleWordlist["colMultipleActualWord", e.RowIndex].Value.ToString();
                try
                {
                    if (dgvMultipleWordlist.CurrentCell == dgvMultipleWordlist["colMultipleWord", e.RowIndex])
                        return;
                }
                
                catch{
                }
            }


            try
            {

                //MessageBox.Show("Hint: The length of the word is " + dgvWordlist["colActualWord", e.RowIndex].Value.ToString().Length, "Hint", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmHint frm = new frmHint(word);
                //frm.lblLength.Text = dgvWordlist["colActualWord", e.RowIndex].Value.ToString().Length.ToString();
                //Random rd = new Random((int)DateTime.Now.Ticks);
                //string[] t = dgvWordlist["colSentence", e.RowIndex].Value.ToString().Split("\n".ToCharArray());
                //frm.lblSentence.Text = t[rd.Next(t.Length)];
                //frm.lblSentence.Text = dgvWordlist["colActualWord", e.RowIndex].Value.ToString();
                //frm.another = dgvWordlist["colActualWord", e.RowIndex].Value.ToString();
                //frm.lblSentence.Visible = false;
                frm.ShowDialog();
            }
            catch
            {
            }
        }

        private void dgvWordlist_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblPron_Click(object sender, EventArgs e)
        {
            //ShowPronGuide();
        }
        public void ShowPronGuide()
        {
            //  frmPronGuide.GetInstance().Show();
        }

        private void timerMaster_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = "[ " + DateTime.Now.ToLongDateString() + ", " + DateTime.Now.ToLongTimeString() + " ]";
        }

        private void optAllWords_CheckedChanged(object sender, EventArgs e)
        {
            if (optAllWords.Checked)
                for (int i = 0; i < 26; ++i) chkList.SetItemChecked(i, true);
        }

        private void optClear_CheckedChanged(object sender, EventArgs e)
        {
            if (optClear.Checked)
            {
                for (int i = 0; i < 26; ++i) chkList.SetItemChecked(i, false);
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            Initialise();
        }

        private void lblWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            if (b.Text == "Speak")
            {
                b.Text = "Speaking...";

                speaker.Rate = -1;
                speaker.Volume = 100;
                speaker.SpeakAsync(lblWord.Text + ", " + lblMeaning.Text);
                //speaker.SpeakAsync("It means " + lblMeaning.Text);
                //speaker.SpeakAsync(txtSentences.Text);
                speaker.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(speaker_SpeakCompleted);

            }
            else
            {
                b.Text = "Speak";
                speaker.SpeakAsyncCancelAll();

            }
        }

        void speaker_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            button1.Text = "Speak";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

                    }

        private void speakTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SpeechSynthesizer speaker = new SpeechSynthesizer();
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;

            Control c = (Control)ActiveControl;
            //string msg = c.Text;
            speaker.Rate = -3;
            speaker.Volume = 100;
            speaker.SpeakAsync(msg);


        }

        private void txtSentences_Click(object sender, EventArgs e)
        {

        }

        private void txtSentences_MouseDown(object sender, MouseEventArgs e)
        {
            msg = ((Control)sender).Text;
        }

        private void dgvWordlist_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {

                msg = dgvWordlist[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            catch
            {
            }
        }

        private void dgvSearchResult_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                msg = dgvSearchResult[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
            catch
            {
            }
        }

        private void txtWord_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                msg = ((Control)sender).Text;
            }
            catch
            {
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dgvWordlist_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (btnStart.Text == "Start") return;
            DataGridViewCellStyle cs = new DataGridViewCellStyle();
            cs.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            DataGridView dgv = (DataGridView)sender;
            dgv.Rows[e.RowIndex].DefaultCellStyle = cs;
        }

        private void dgvWordlist_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (btnStart.Text == "Start") return;
            DataGridViewCellStyle cs = new DataGridViewCellStyle();
            cs.BackColor = System.Drawing.Color.White;
            DataGridView dgv = (DataGridView)sender;
            dgv.Rows[e.RowIndex].DefaultCellStyle = cs;
        }

        private void lblWordsToLearn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmToLearn(wtl.ToArray()).Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRange.Checked)
            {
                txtFrom.Enabled = txtTo.Enabled = true;
            }
            else
            {
                txtFrom.Enabled = txtTo.Enabled = false;
            }
        }

        private void btnStart_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btnStart_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            //tblPane.ColumnCount = 10;
        }

        private void button2_Click_3(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
        private void labels_Click(object sender, object e)
        {


        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (btnStart.Text == "Stop") e.Cancel = true; ;
        }

        private void bckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arr = (object[])e.Argument;
            int max = (int)arr[0];
            Word[] words = (Word[])arr[1];

            if (tabControl2.SelectedTab == tabpgWritingTest)
            {
                //bckWorker.RunWorkerAsync();
                DisplayWords(max, words);
            }
            else if (tabControl2.SelectedTab == tabpgMultipleChoice)
            {
                DisplayMultipleChoiceWords(max, words);
            }
            else
                DisplayGame(max, words);
        }

        private void button2_Click_4(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabControl2_Selected(object sender, TabControlEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int version = 0;
            if (radioWS1.Checked)
            {
                version = 1;

            }
            else if (radioWS2.Checked)
            {
                version = 2;
            }
            string LIKE = "";
            string starting = "";

            FROMTO = "";
            if (chkRange.Checked && txtFrom.Text.Trim() != "" && txtTo.Text.Trim() != "")
            {
                FROMTO = "(words.word >= '" + txtFrom.Text.Trim() + "' AND words.word <= '" + txtTo.Text.Trim() + "') ";

            }

            int i;
            for (i = 0; i < chkList.CheckedItems.Count; ++i)
            {
                char cc = Char.ToLower(chkList.CheckedItems[i].ToString()[0]);
                starting += cc;
                LIKE += "OR words.word LIKE '" + char.ToLower(cc) + "%' ";
            }
            if (LIKE.Length == 0)
            {
                InvalidSelection();
                return;
            }

            LIKE = LIKE.Substring(2);
            LIKE = "(" + LIKE;
            LIKE += ")";

            Word[] word = c.GetAllReportedWords(-1, LIKE, FROMTO, version);

            dgvDifficultWords.Rows.Clear();
            i = 0;
            foreach (Word w in word)
            {
                dgvDifficultWords.Rows.Add();
                dgvDifficultWords["colDifficultWordSl", i].Value = (i + 1).ToString();
                dgvDifficultWords["colDifficultWordWord", i].Value = w.word;
                dgvDifficultWords["colDifficultWordPron", i].Value = w.pron;
                dgvDifficultWords["colDifficultWordMeaning", i].Value = w.meaning;
                ++i;
            }
           
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}

