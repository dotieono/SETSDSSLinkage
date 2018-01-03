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
    public partial class UserLogin : Form
    {

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString);
        public UserLogin()
        {
            InitializeComponent();
        }

        private void UserLogin_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                          (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);

            cboUserRole.SelectedIndex = 0;

            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            SqlCommand com = new SqlCommand("usp_checkUser", con);

            com.CommandType = CommandType.StoredProcedure;

            SqlParameter p1 = new SqlParameter("username", txtUsername.Text);

            SqlParameter p2 = new SqlParameter("password", txtPassword.Text);

            SqlParameter p3 = new SqlParameter("userrole", cboUserRole.Text);

            com.Parameters.Add(p1);

            com.Parameters.Add(p2);

            com.Parameters.Add(p3);

            try
            {

                con.Open();

                SqlDataReader rd = com.ExecuteReader();

                if (rd.HasRows)
                {

                    rd.Read();

                    this.Visible = false;



                    MainForm mf = new MainForm(txtUsername.Text);

                    if (cboUserRole.Text == "Standard User")
                    {
                        mf.RemoveAdminPanel();
                    }

                    mf.Visible = true;
                    //logged_user = txtUsername.Text;
                    //mf.lblUser.Text = "User: " + logged_user;



                }



                else
                {

                    MessageBox.Show(" " +
     "Wrong username and password combination", "SETS Linkage System: User authentication", MessageBoxButtons.OKCancel,
     MessageBoxIcon.Information);

                    txtPassword.Text = "";
                    txtUsername.Text = "";
                    txtUsername.Focus();


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            finally
            { con.Close(); }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
