using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace wordlist
{
    public partial class frmPronGuide : Form
    {
        private static frmPronGuide me = null;
        public static frmPronGuide GetInstance()
        {
            if (me == null) me = new frmPronGuide();
            return me;
        }
        public frmPronGuide()
        {
            InitializeComponent();
        }

        private void frmPronGuide_Load(object sender, EventArgs e)
        {
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmPronGuide_FormClosed(object sender, FormClosedEventArgs e)
        {
            me = null;
        }
    }
}
