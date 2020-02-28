using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace rm2.UI
{
    public partial class AboutUI : Form
    {
        public AboutUI()
        {
            InitializeComponent();
        }

        private void clsAbout_Load(object sender, EventArgs e)
        {
            //set the title accordingly ;)
            this.Text = Program.APPLICATION_NAME + " " + Program.version;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}