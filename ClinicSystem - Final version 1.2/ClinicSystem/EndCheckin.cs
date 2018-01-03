using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClinicSystem
{
    public partial class EndCheckin : Form
    {
        public EndCheckin()
        {
            InitializeComponent();
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            MainForm.ClearAllTabs();

            this.Close();
        }
    }
}
