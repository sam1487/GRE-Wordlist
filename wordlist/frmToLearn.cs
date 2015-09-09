using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;

namespace wordlist
{
    public partial class frmToLearn : Form
    {
        Word[] words;
        string msg = "";
        public frmToLearn(Word[] w)
        {
            InitializeComponent();
            //txtDump.Text = msg;
            this.words = w;
        }

        private void frmToLearn_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < words.Length; ++i)
            {
                txtDump.Text += (i + 1).ToString();
                txtDump.Text += "\t";
                txtDump.SelectionStart = txtDump.Text.Length;
                txtDump.Text += words[i].word;
                //txtDump.SelectionLength = txtDump.Text.Length - Sel;
                txtDump.SelectionFont = new Font(txtDump.Font.FontFamily, 14, FontStyle.Bold);
                txtDump.SelectionLength = 0;
                //txtDump.SelectionFont = new Font(txtDump.Font.FontFamily, 14, FontStyle.Regular);
                txtDump.Text += ", " + words[i].pron;
                txtDump.Text += "\n" + words[i].meaning;
                txtDump.Text += "\n" + words[i].sentence;
                txtDump.Text += "\n\n";
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose(false);
        }
    }
}
