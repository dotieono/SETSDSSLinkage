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
    public partial class AdditionalReferences : Form
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString);
        public AdditionalReferences()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdditionalReferences_Load(object sender, EventArgs e)
        {
            Retrieve_FileRef_Types();
        }

        private void Retrieve_FileRef_Types()
        {
            string sql = @"select department_id,department_name from registry.facility_departments";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);
            cboRef1.DisplayMember = "department_name";
            cboRef1.ValueMember = "department_id";
            cboRef1.DataSource = dtr;

            cboRef2.DisplayMember = "department_name";
            cboRef2.ValueMember = "department_id";
            cboRef2.DataSource = dtr;


            cboRef3.DisplayMember = "department_name";
            cboRef3.ValueMember = "department_id";
            cboRef3.DataSource = dtr;

            cboRef4.DisplayMember = "department_name";
            cboRef4.ValueMember = "department_id";
            cboRef4.DataSource = dtr;


            cboRef5.DisplayMember = "department_name";
            cboRef5.ValueMember = "department_id";
            cboRef5.DataSource = dtr;

            cboRef6.DisplayMember = "department_name";
            cboRef6.ValueMember = "department_id";
            cboRef6.DataSource = dtr;



        }

        private void btn1Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef1.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef1.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn1Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }

        private void btn1AnotherRef_Click(object sender, EventArgs e)
        {
            GB2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GB3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GB4.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            GB5.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            GB6.Visible = true;
        }

        private void btn2Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef2.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef2.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn2Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }

        private void btn3Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef3.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef3.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn3Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }

        private void btn4Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef4.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef4.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn4Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }

        private void btn5Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef5.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef5.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn5Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }

        private void btn6Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_additional_references", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", int.Parse(lblFacilityMfl.Text));
            cmd.Parameters.AddWithValue("@facility_department", lblDepartment.Text);
            cmd.Parameters.AddWithValue("@pfile_ref", lblpFileRef.Text);
            cmd.Parameters.AddWithValue("@pfile_ref_type", lblPRefType.Text);
            cmd.Parameters.AddWithValue("@s_ref_type", cboRef6.Text);
            cmd.Parameters.AddWithValue("@s_ref_no", txtFileRef6.Text);



            try
            {
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" Additional reference successfully saved", "SETS: Client Visit Information", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn6Save.Enabled = false;


                }


            }


            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



            finally
            { con.Close(); }


        }


    }
}
