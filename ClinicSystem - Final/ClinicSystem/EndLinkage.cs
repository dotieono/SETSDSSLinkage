using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ClinicSystem
{
    public partial class EndLinkage : Form
    {

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString);
        public EndLinkage()
        {
            InitializeComponent();
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnContinueClient_Click(object sender, EventArgs e)
        {
            this.Close();
            //MainForm.ClearAllTabs();
            MainForm.RegisterAndCheckin();
            
            
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            this.Close();
            MainForm.ClearAllTabs();
        }
    }
}
