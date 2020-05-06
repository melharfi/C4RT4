using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
//using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace C4RT4
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();
        }

        private void AboutBox1_Load(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutBox1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }
    }
}
