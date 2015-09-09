using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace wordlist
{
    public partial class frmHint : Form
    {
        public string another, word;
        Controller c = new Controller();
        Word thisword;
        public frmHint(string word)
        {
            InitializeComponent();
            this.word = word;
        }

        private void frmHint_Load(object sender, EventArgs e)
        {
            thisword = c.GetWord(word);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            string a = "", b = thisword.word ;//lblSentence.Text;
            for (int i = 0; i < b.Length; ++i)
            {
                if (i != 0 && i != b.Length / 2 && i != b.Length - 1)  a += '_';
                else a += b[i];
                a += " ";
            }
            linkLabel2.Visible = true;
            lblSentence.Text =  a;
            lblSentence.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void lblMeaning_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string[] meanings = thisword.meaning.Split(";".ToCharArray());
            Random rd = new Random();
            for (int i = 0; i < 10; ++i) rd.Next();
            lblMeaning.Text = thisword.meaning;
            lblMeaning.Show();
            lblUsage.Visible = true;
        }

        private void lblUsage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtUsage.Text = thisword.sentence;
            txtUsage.Visible = true;
        }
    }
}
