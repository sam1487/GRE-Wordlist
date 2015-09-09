using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wordlist
{
    public partial class frmWordsToLearn : Form
    {
        
        public frmWordsToLearn(string msg)
        {
            InitializeComponent();

            txtWordsToLearn.Text = msg;
        }

        private void frmWordsToLearn_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Dispose(false);
        }
    }
}
