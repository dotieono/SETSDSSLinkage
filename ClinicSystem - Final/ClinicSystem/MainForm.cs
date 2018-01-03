using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;

namespace ClinicSystem
{
    public partial class MainForm : Form
    {

        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString);
        private static MainForm _instance;

        public MainForm(string loggedin_user)
        {
            InitializeComponent();
            _instance = this;
            logged_user = loggedin_user;
            
        }

       // public MainForm() { }


        string search_criteria = "";
        int match_state = 0;
        public int facility_mfl = 0;
        public int is_duplicate_ref = 0;

        public int is_duplicate_person = 0;
        public int is_duplicate_HDSS = 0;
        public int is_duplicate_hiv_entry = 0;
        public int person_ID = 0;
        public string System_ID = "";
        public string pfile_ref = "";
        public string pfile_ref_type = "";
        public string facility_dept = "";
        public string system_ID = "";
        public string active_system_ID = "";
        string hiv_status = "";
        string c_firstname = "";
        string c_middlename = "";
        string c_lastname = "";
        //string logged_user = "user123";
        string logged_user = "";
        string c_matchstatus = "";
        public int PID_missing = 0;
        public int is_hiventry_new = 1;
        public string remark = "";
        string RawXML = "";
        public string synctype = "";
        



        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }




        public void CheckDuplicateRef()
        {
            if (con != null && con.State == ConnectionState.Open)

            { con.Close(); }
                
            // open connection
                con.Open();
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "check_duplicate_reference";
                //add health facility parameter
                sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                sqlcmd.Parameters.Add("@pfile_ref", SqlDbType.VarChar).Value = txtFileRef.Text;
                sqlcmd.Parameters.Add("@pfile_ref_type", SqlDbType.VarChar).Value = cboPReferenceType.Text;
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        is_duplicate_ref = int.Parse(sqlrdr.GetValue(0).ToString());
                    }


                } //this is the end of the while statement

                con.Close();

        }



        //check if there is any duplicate HDSS match

        public void CheckDuplicateHDSSMatch()
        {

            //close connection in case open

            if (con != null && con.State == ConnectionState.Open)

            { con.Close(); }

            // open connection
            con.Open();
            // create command
            SqlCommand sqlcmd = con.CreateCommand();
            // specify stored procedure to execute
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "check_duplicate_HDSS_Match";
            //add health facility parameter

            if (System_ID.Length > 0)

            { sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = System_ID; }
            else

            { sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = active_system_ID; }

            //sqlcmd.Parameters.Add("@HDSS_id", SqlDbType.VarChar).Value = txtHDSS_IDLink.Text;
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure

            while (sqlrdr.Read())
            {

                if (sqlrdr.IsDBNull(0) == false)
                {
                    is_duplicate_HDSS = int.Parse(sqlrdr.GetValue(0).ToString());
                }


            } //this is the end of the while statement

            con.Close();

        }

        

        //checking if there is an existing positive status entry

        public void CheckDuplicatePositiveStatusEntry()
        {

            //close connection in case open

            if (con != null && con.State == ConnectionState.Open)

            { con.Close(); }

            // open connection
            con.Open();
            // create command
            SqlCommand sqlcmd = con.CreateCommand();
            // specify stored procedure to execute
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "check_duplicate_finalpositivehivstatus";
            //add health facility parameter

            if (System_ID.Length > 0)

            { sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = System_ID; }
            sqlcmd.Parameters.Add("@test_result", SqlDbType.VarChar).Value = cboFinalTestResult.Text;
            SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure

            while (sqlrdr.Read())
            {

                if (sqlrdr.IsDBNull(0) == false)
                {
                    is_duplicate_hiv_entry = int.Parse(sqlrdr.GetValue(0).ToString());
                }


            } //this is the end of the while statement

            con.Close();

        }





        private void rbUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUser.Checked == true)
            {
                if (AdminTabs.Visible == false) { AdminTabs.Visible = true; }
                AdminTabs.SelectTab(tpAddUser);

            }

        }

        private void rbMaritalStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMaritalStatus.Checked == true)
            {
                if (AdminTabs.Visible == false) { AdminTabs.Visible = true; }
                AdminTabs.SelectTab(tpAddMaritalStatus);

            }
        }

        private void rbDepartment_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDepartment.Checked == true)

            {
                if (AdminTabs.Visible == false) { AdminTabs.Visible = true; }
                AdminTabs.SelectTab(tpAddFacilityDepartments);
            }
        }

        private void rbHIVTestType_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHIVTestType.Checked == true)
            {
                if (AdminTabs.Visible == false) { AdminTabs.Visible = true; }
                AdminTabs.SelectTab(tpAddHIVTest);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (dpRegDate.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show(" " +
                       "A day of birth cannot be set to a future date", "SETS: Client registry", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information);
                dpRegDate.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(txtFileRef.Text))
            {
                MessageBox.Show("You need to supply at least one reference number for this client.");
                txtFileRef.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(txtFirstname.Text))
            {
                MessageBox.Show("You need to supply a client firstname.");
                txtFirstname.Focus();
                return;
            }


            if (!string.IsNullOrWhiteSpace(txtFirstname.Text))
            {

                if (txtFirstname.Text.Length < 3)
                {

                    MessageBox.Show("The firstname field should have 3 or more characters");
                    txtFirstname.Focus();
                    return;

                }


                // ensure that users do not supply numbers as firstnames

                int i;

                if (int.TryParse(txtFirstname.Text, out i))
                {
                    MessageBox.Show("You cannot present a number as a client firstname");
                    txtFirstname.Focus();
                    return;


                }


            }


           
            if (!string.IsNullOrWhiteSpace(txtMiddlename.Text))
            {

                if (txtMiddlename.Text.Length < 3)
                {

                    MessageBox.Show("The middlename field should have 3 or more characters");
                    txtMiddlename.Focus();
                    return;

                }



                int i;

                if (int.TryParse(txtMiddlename.Text, out i))
                {
                    MessageBox.Show("You cannot present a number as a client middlename");
                    txtMiddlename.Focus();
                    return;


                }


            }


            if (string.IsNullOrWhiteSpace(txtLastname.Text))
            {
                MessageBox.Show("You need to supply a client lastname.");
                txtLastname.Focus();
                return;
            }



            if (!string.IsNullOrWhiteSpace(txtLastname.Text))
            {

                if (txtLastname.Text.Length < 3)
                {

                    MessageBox.Show("The lastname field should have 3 or more characters");
                    txtLastname.Focus();
                    return;

                }



                int i;

                if (int.TryParse(txtLastname.Text, out i))
                {
                    MessageBox.Show("You cannot present a number as a client lastname");
                    txtLastname.Focus();
                    return;


                }


            }


            if (cboMaritalStatus.Text == "")
            {

                MessageBox.Show(" " +
                    "Please select a marital status", "SETS: Client registry", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information);
                //lblGender.ForeColor = System.Drawing.Color.Red;
                //lblConsentStatus.Font = new Font("Arial", 24,FontStyle.Bold);
                cboGender.Focus();
                return;



            }



       if (dpDob.Value.Date == DateTime.Now.Date)
            {
                //birthdate is today

                MessageBox.Show(" " +
                     "Birthdate should not be left at a current date", "SETS: Client registry", MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Information);
                dpDob.Focus();
                return;
            }

            if (dpDob.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show(" " +
                       "A day of birth cannot be set to a future date", "SETS: Client registry", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Information);
                dpDob.Focus();
                return;
            }


            if (cboGender.SelectedIndex == 0)
            {

                MessageBox.Show(" " +
                    "Please select gender", "SETS: Client registry", MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information);
                //lblGender.ForeColor = System.Drawing.Color.Red;
                //lblConsentStatus.Font = new Font("Arial", 24,FontStyle.Bold);
                cboGender.Focus();
                return;



            }




            //cross checking the facility selected by the user

            if (cboFacilityName.Text=="Wagai Dispensary")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you are saving this record in  " + cboFacilityName.Text + " ", "SETS Registry Prompt", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {


                }

                else if (dialogResult == DialogResult.No)
                {

                    cboFacilityName.Focus();
                    return;

                }
            }





            //String facility = cboFacilityName.Text.ToUpper().Trim();
            String department = cboFacilityDepartment.Text.ToUpper().Trim();
            //String file_ref_no = txtFileRef.Text.ToUpper().Trim();
            String first_name = txtFirstname.Text.ToUpper().Trim();
            String middle_name = txtMiddlename.Text.ToUpper().Trim();
            String last_name = txtLastname.Text.ToUpper().Trim();
            String mfname = txtMFirstname.Text.ToUpper().Trim();
            String mjname = txtMMiddlename.Text.ToUpper().Trim();
            String mlname = txtMLastname.Text.ToUpper().Trim();
            String ffname = txtFFirstname.Text.ToUpper().Trim();
            String fjname = txtFMiddlename.Text.ToUpper().Trim();
            String flname = txtFLastname.Text.ToUpper().Trim();
            String chfname = txtCHFirstname.Text.ToUpper().Trim();
            String chjname = txtCHMiddlename.Text.ToUpper().Trim();
            String chlname = txtCHLastname.Text.ToUpper().Trim();
            String sex = cboGender.Text.ToUpper().Trim();
            //String marital_status = cboMaritalStatus.Text.ToUpper().Trim();
            String village = txtVillageName.Text.ToUpper().Trim();

            String meta_location_type = cboAlternativeSearchParameter.Text.ToUpper().Trim();
            String meta_location_value = txtAltLocSearchValue.Text.ToUpper().Trim();


            String machine_name = System.Environment.MachineName;


            //string serverName = System.Windows.Forms.SystemInformation.ComputerName;



            //Bypass this code block if the save attempt is rising from an EDIT prompt

            bool IsNew;

            // a variable to render special treatments to updates

            if (btnEdit.Enabled == true)
            {


                CheckDuplicateRef();


                if (is_duplicate_ref == 1)
                {

                    DialogResult dialogResult = MessageBox.Show("There is already a client with the " + cboPReferenceType.Text + " number " + txtFileRef.Text + " " + "Do you wish to register a client with a similar reference number?", "SETS Registry Details", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {


                    }

                    else if (dialogResult == DialogResult.No)
                    {

                        txtFileRef.Focus();
                        return;

                    }


                }




                IsNew = true;


            }


            else IsNew = false;

            SqlCommand cmd = new SqlCommand();

            //if this is a new record

            if (btnEdit.Enabled == true)
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "create_person";

                cmd.Parameters.AddWithValue("reg_date", dpRegDate.Value.Date);
                cmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                cmd.Parameters.AddWithValue("facility_department", department);
                cmd.Parameters.AddWithValue("pfile_ref_type", cboPReferenceType.Text);
                cmd.Parameters.AddWithValue("pfile_ref", txtFileRef.Text);
                cmd.Parameters.AddWithValue("firstname", first_name);
                cmd.Parameters.AddWithValue("middlename", middle_name);
                cmd.Parameters.AddWithValue("lastname", last_name);
                cmd.Parameters.AddWithValue("mfname", mfname);
                cmd.Parameters.AddWithValue("mjname", mjname);
                cmd.Parameters.AddWithValue("mlname", mlname);
                cmd.Parameters.AddWithValue("ffname", ffname);
                cmd.Parameters.AddWithValue("fjname", fjname);
                cmd.Parameters.AddWithValue("flname", flname);
                cmd.Parameters.AddWithValue("dob", dpDob.Value.Date);
                cmd.Parameters.AddWithValue("gender", sex);
                cmd.Parameters.AddWithValue("dateAtCurResidence", dpResidentStartDate.Value.Date);
                cmd.Parameters.AddWithValue("village_name", village);
                //cmd.Parameters.AddWithValue("mstatus", marital_status);
                cmd.Parameters.AddWithValue("mstatus", cboMaritalStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@chfname", chfname);
                cmd.Parameters.AddWithValue("@chjname", chjname);
                cmd.Parameters.AddWithValue("@chlname", chlname);
                cmd.Parameters.AddWithValue("@machine_name", machine_name);

                cmd.Parameters.AddWithValue("@meta_location_type", meta_location_type);
                cmd.Parameters.AddWithValue("@meta_location_value", meta_location_value);

                cmd.Parameters.AddWithValue("@logged_user", logged_user);

                cmd.Parameters.Add("@is_new", SqlDbType.Bit).Value = IsNew;

            }

                //in case of an edit

            else if (btnEdit.Enabled == false)
            {

                cmd.Connection = con;
                
                
                //cmd.Connection = con;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "update_person";


                // cmd.Parameters.AddWithValue("@System_ID", System_ID);

                //assess the point from which a data clerk is editing the system

                if (System_ID.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@System_ID", System_ID);
                }

                else if (active_system_ID.Length > 0)
                {

                    cmd.Parameters.AddWithValue("@System_ID", active_system_ID);

                }

                cmd.Parameters.AddWithValue("reg_date", dpRegDate.Value.Date);
                cmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                cmd.Parameters.AddWithValue("facility_department", department);
                cmd.Parameters.AddWithValue("pfile_ref_type", cboPReferenceType.Text);
                cmd.Parameters.AddWithValue("pfile_ref", txtFileRef.Text);
                cmd.Parameters.AddWithValue("firstname", first_name);
                cmd.Parameters.AddWithValue("middlename", middle_name);
                cmd.Parameters.AddWithValue("lastname", last_name);
                cmd.Parameters.AddWithValue("mfname", mfname);
                cmd.Parameters.AddWithValue("mjname", mjname);
                cmd.Parameters.AddWithValue("mlname", mlname);
                cmd.Parameters.AddWithValue("ffname", ffname);
                cmd.Parameters.AddWithValue("fjname", fjname);
                cmd.Parameters.AddWithValue("flname", flname);
                cmd.Parameters.AddWithValue("dob", dpDob.Value.Date);
                cmd.Parameters.AddWithValue("gender", sex);
                cmd.Parameters.AddWithValue("dateAtCurResidence", dpResidentStartDate.Value.Date);
                cmd.Parameters.AddWithValue("village_name", village);
                //cmd.Parameters.AddWithValue("mstatus", marital_status);
                cmd.Parameters.AddWithValue("mstatus", cboMaritalStatus.SelectedValue);
                cmd.Parameters.AddWithValue("@chfname", chfname);
                cmd.Parameters.AddWithValue("@chjname", chjname);
                cmd.Parameters.AddWithValue("@chlname", chlname);
                cmd.Parameters.AddWithValue("@machine_name", machine_name);

                cmd.Parameters.AddWithValue("@meta_location_type", meta_location_type);
                cmd.Parameters.AddWithValue("@meta_location_value", meta_location_value);

                cmd.Parameters.AddWithValue("@logged_user", logged_user);

                cmd.Parameters.Add("@is_new", SqlDbType.Bit).Value = IsNew;

            }


            //SqlCommand cmd = new SqlCommand("create_person", con);
            //cmd.CommandType = CommandType.StoredProcedure;


            //cmd.Parameters.AddWithValue("reg_date", dpRegDate.Value.Date);
            //cmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
            //cmd.Parameters.AddWithValue("facility_department", department);
            //cmd.Parameters.AddWithValue("pfile_ref_type", cboPReferenceType.Text);
            //cmd.Parameters.AddWithValue("pfile_ref", txtFileRef.Text);
            //cmd.Parameters.AddWithValue("firstname", first_name);
            //cmd.Parameters.AddWithValue("middlename", middle_name);
            //cmd.Parameters.AddWithValue("lastname", last_name);
            //cmd.Parameters.AddWithValue("mfname", mfname);
            //cmd.Parameters.AddWithValue("mjname", mjname);
            //cmd.Parameters.AddWithValue("mlname", mlname);
            //cmd.Parameters.AddWithValue("ffname", ffname);
            //cmd.Parameters.AddWithValue("fjname", fjname);
            //cmd.Parameters.AddWithValue("flname", flname);
            //cmd.Parameters.AddWithValue("dob", dpDob.Value.Date);
            //cmd.Parameters.AddWithValue("gender", sex);
            //cmd.Parameters.AddWithValue("dateAtCurResidence", dpResidentStartDate.Value.Date);
            //cmd.Parameters.AddWithValue("village_name", village);
            ////cmd.Parameters.AddWithValue("mstatus", marital_status);
            //cmd.Parameters.AddWithValue("mstatus", cboMaritalStatus.SelectedValue);
            //cmd.Parameters.AddWithValue("@chfname", chfname);
            //cmd.Parameters.AddWithValue("@chjname", chjname);
            //cmd.Parameters.AddWithValue("@chlname", chlname);
            //cmd.Parameters.AddWithValue("@machine_name", machine_name);

            //cmd.Parameters.AddWithValue("@meta_location_type", meta_location_type);
            //cmd.Parameters.AddWithValue("@meta_location_value", meta_location_value);

            //cmd.Parameters.AddWithValue("@logged_user",logged_user);

            //cmd.Parameters.Add("@is_new", SqlDbType.Bit).Value = IsNew;
            //cmd.Parameters.Add("@RaiseDuplicateAlert", SqlDbType.Bit).Direction = ParameterDirection.Output;




            try
            {
                
                //close connection in case open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection
                
                con.Open();


                int k = cmd.ExecuteNonQuery();

               // int queryfailed = Convert.ToInt32(cmd.Parameters["@RaiseDuplicateAlert"].Value);


            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            
            //if (dt != null && dt.Rows.Count > 0)
            //{

            //    DialogResult dialogResult = MessageBox.Show("There is already a client with the name " + first_name + " " + middle_name + " " + last_name + "Do you wish to review the client?", "SETS Registry Details", MessageBoxButtons.YesNo);
                
            //    if (dialogResult == DialogResult.Yes)
            //    {

            //        if (dgvClientSearchResults.Visible == false) { dgvClientSearchResults.Visible = true; }

            //        dgvClientSearchResults.Rows.Clear();

            //        foreach (DataRow item in dt.Rows)
            //        {
            //            int n = dgvClientSearchResults.Rows.Add();
            //            dgvClientSearchResults.Rows[n].Cells[0].Value = item["pfile_ref_type"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[1].Value = item["pfile_ref"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[2].Value = item["firstname"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[3].Value = item["middlename"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[4].Value = item["lastname"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[5].Value = item["Match_Status"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[6].Value = item["HIV_Status"].ToString();
            //            //dgvClientSearchResults.Rows[n].Cells[7].Value = item["person_id"].ToString();
            //            dgvClientSearchResults.Rows[n].Cells[8].Value = item["system_id"].ToString();
            //            //int client_id = int.Parse(item["HIV_Status"].ToString());
            //            //txtPersonID.Text = item["person_id"].ToString();

            //        }

            //        tabControl1.SelectTab(tpSearchClient);

            //        return;
                    



            //    }

            //    else if (dialogResult == DialogResult.No)
            //    {

                    

            //    }

            //}
                //the outer if statement ends here

              

                //Clear();
                if (k != 0)
                {
                    if (btnEdit.Enabled == true)
                    {

                        MessageBox.Show(" " + first_name + " " + middle_name + " " + last_name + " " +
    "successfully registered", "SETS: Client registry", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);


                        

                    }

                    else if (btnEdit.Enabled == false)

                    {

                        MessageBox.Show(" Records of " + first_name + " " + middle_name + " " + last_name + " " +
    "successfully updated", "SETS: Client registry", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                        
                    
                    }


                   

                    facility_mfl = int.Parse(cboFacilityName.SelectedValue.ToString());
                    facility_dept = cboFacilityDepartment.Text;
                    pfile_ref_type = cboPReferenceType.Text;
                    pfile_ref = txtFileRef.Text;
                    c_firstname = first_name;
                    c_middlename = middle_name;
                    c_lastname = last_name;

                    RetrievePersonID();

                    lblBanner.Text = "File ref:  " + pfile_ref + "(" + pfile_ref_type + ")  Name: " + c_firstname + " " + c_middlename + " " + c_lastname + " " + active_system_ID;



                    if (lblBanner.Visible == false)
                    {
                        lblBanner.Visible = true;

                    }

                   // gbAlternativeLocation.Enabled = false;

                    DialogResult dialogResult = MessageBox.Show("Do you wish to add other references for the client?", "SETS Registry Details", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {

                        //PassLinkageVariables();
                        //ClearFields();
                        btnSave.Enabled = false;

                        tabControl1.SelectTab(tpHDSSLinkage);


                        AdditionalReferences cr = new AdditionalReferences();
                        cr.lblDepartment.Text = facility_dept;
                        cr.lblPRefType.Text = pfile_ref_type;
                        cr.lblpFileRef.Text = pfile_ref;
                        cr.lblFacilityMfl.Text = facility_mfl.ToString();
                        //cr.StartPosition = FormStartPosition.CenterParent;
                        cr.ShowDialog();

                    }

                    else if (dialogResult == DialogResult.No)
                    {

                       
                        //ClearFields();
                        btnSave.Enabled = false;
                        btnEdit.Enabled = true;
                        tpHDSSLinkage.Enabled = true;
                        tabControl1.SelectTab(tpHDSSLinkage);

                    }

                    ReloadDSSLinkageWindow();
                    PassLinkageVariables();

                }


            }


             //this is the end of the ifvalidate text box condition

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            

            finally
            { con.Close(); }

            //active_system_ID = RetrievePersonID();

        }

        

        private void PassLinkageVariables()
        {
            txtFirstnameLink.Text = txtFirstname.Text;
            txtMiddlenameLink.Text = txtMiddlename.Text;
            txtLastnameLink.Text = txtLastname.Text;
            if (cboGender.Text != "")
            {
                txtGenderLink.Text = cboGender.Text;
            }

            txtVillageLink.Text = txtVillageName.Text;
            txtMotherFirstnameLink.Text = txtMFirstname.Text;
            txtMotherMiddlenameLink.Text = txtMMiddlename.Text;
            txtMotherLastnameLink.Text = txtMLastname.Text;
            //var bYear=DateTime.Parse(dpDob.Text).Year;
            //var bMonth = DateTime.Parse(dpDob.Text).Month;
            //var bDay = DateTime.Parse(dpDob.Text).Day;
            ////txtYearLink.Text = bYear.ToString();
            //txtMonthLink.Text = bMonth.ToString();
            //txtDayLink.Text = bDay.ToString();
            //txtFileRefLink.Text = txtFileRef.Text;
            txtDobLink.Text = dpDob.Value.Date.ToString("yyyy-MM-dd");
            txtCHFirstnameLink.Text = txtCHFirstname.Text;
            txtCHMiddlenameLink.Text = txtCHMiddlename.Text;
            txtCHLastnameLink.Text = txtCHLastname.Text;
            txtAltLocSearchValueLink.Text = txtAltLocSearchValue.Text;

            if (gbAlternativeLocation.Enabled == true)

            {
                if (txtAltLocSearchValue.Text.Length > 0)

                {
                    gbAltLoc.Text=cboAlternativeSearchParameter.Text.ToLower();

                    chBlockAltLoc.Text = cboAlternativeSearchParameter.Text.ToLower();
                
                }
            
            }


        }



        private void DisplayHideAdmin()
        {

            tabControl1.TabPages.Remove(tpAdminPanel);
            tabControl1.TabPages.Remove(tpAdminReport);
        }


        public void RemoveAdminPanel()
        {

            _instance.DisplayHideAdmin();
        }



        private void GetPersonDetails()
        {

            try
            {
                //close connection in case in an open state

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                // open connection
                con.Open();
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "retrieve_person_details";
                //add parameters
                //sqlcmd.Parameters.Add("@person_id", SqlDbType.VarChar).Value = int.Parse(txtPersonID.Text);
                //sqlcmd.Parameters.Add("@person_id", SqlDbType.VarChar).Value = person_ID;
                sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = System_ID;
                sqlcmd.Parameters.Add("@facility_mflcode", SqlDbType.VarChar).Value = cboFacilityName.SelectedValue;
                sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;

                // Read in the SELECT results.
                //
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        dpRegDate.Text = sqlrdr.GetValue(0).ToString();
                    }

                    if (sqlrdr.IsDBNull(1) == false)
                    {
                        cboPReferenceType.Text = sqlrdr.GetValue(1).ToString();
                    }


                    if (sqlrdr.IsDBNull(2) == false)
                    {
                        txtFileRef.Text = sqlrdr.GetValue(2).ToString();
                        txtPatientFileRef.Text = sqlrdr.GetValue(2).ToString();
                    }


                    if (sqlrdr.IsDBNull(3) == false)
                    {
                        txtMatchedHDSSID.Text = sqlrdr.GetValue(3).ToString();
                        txtHDSS_IDLink.Text = sqlrdr.GetValue(3).ToString();
                    }

                    if (sqlrdr.IsDBNull(4) == false)
                    {
                        txtFirstname.Text = sqlrdr.GetValue(4).ToString();
                    }


                    if (sqlrdr.IsDBNull(5) == false)
                    {
                        txtMiddlename.Text = sqlrdr.GetValue(5).ToString();
                    }

                    if (sqlrdr.IsDBNull(6) == false)
                    {
                        txtLastname.Text = sqlrdr.GetValue(6).ToString();
                    }

                    if (sqlrdr.IsDBNull(7) == false)
                    {
                        dpDob.Text = sqlrdr.GetValue(7).ToString();
                    }

                    if (sqlrdr.IsDBNull(8) == false)
                    {
                        cboGender.Text = sqlrdr.GetValue(8).ToString();
                    }

                    if (sqlrdr.IsDBNull(9) == false)
                    {
                        dpResidentStartDate.Text = sqlrdr.GetValue(9).ToString();
                    }


                    if (sqlrdr.IsDBNull(10) == false)
                    {
                        txtMFirstname.Text = sqlrdr.GetValue(10).ToString();
                    }


                    if (sqlrdr.IsDBNull(11) == false)
                    {
                        txtMMiddlename.Text = sqlrdr.GetValue(11).ToString();
                    }

                    if (sqlrdr.IsDBNull(12) == false)
                    {
                        txtMLastname.Text = sqlrdr.GetValue(12).ToString();
                    }

                    if (sqlrdr.IsDBNull(13) == false)
                    {
                        txtFFirstname.Text = sqlrdr.GetValue(13).ToString();
                    }


                    if (sqlrdr.IsDBNull(14) == false)
                    {
                        txtFMiddlename.Text = sqlrdr.GetValue(14).ToString();
                    }

                    if (sqlrdr.IsDBNull(15) == false)
                    {
                        txtFLastname.Text = sqlrdr.GetValue(15).ToString();
                    }

                    if (sqlrdr.IsDBNull(16) == false)
                    {
                        txtVillageName.Text = sqlrdr.GetValue(16).ToString();
                    }

                    if (sqlrdr.IsDBNull(17) == false)
                    {
                        cboMaritalStatus.Text = sqlrdr.GetValue(17).ToString();
                    }


                    if (sqlrdr.IsDBNull(18) == false)
                    {
                        cboFacilityName.Text = sqlrdr.GetValue(18).ToString();
                    }


                    if (sqlrdr.IsDBNull(19) == false)
                    {
                        txtMatchStatus.Text = sqlrdr.GetValue(19).ToString();
                    }


                    if (sqlrdr.IsDBNull(20) == false)
                    {
                        dpLastSearchedDate.Text = sqlrdr.GetValue(20).ToString();
                    }


                    if (sqlrdr.IsDBNull(21) == false)
                    {
                        txtCHFirstname.Text = sqlrdr.GetValue(21).ToString();
                    }

                    if (sqlrdr.IsDBNull(22) == false)
                    {
                        txtCHMiddlename.Text = sqlrdr.GetValue(22).ToString();
                    }

                    if (sqlrdr.IsDBNull(23) == false)
                    {
                        txtCHLastname.Text = sqlrdr.GetValue(23).ToString();
                    }

                    if (sqlrdr.IsDBNull(24) == false)
                    {
                        cboFacilityDepartment.Text = sqlrdr.GetValue(24).ToString();
                    }

                    if (sqlrdr.IsDBNull(25) == false)
                    {
                        txtPreviousMatchNotes.Text = sqlrdr.GetValue(25).ToString();
                    }


                    if (sqlrdr.IsDBNull(26) == false)
                    {
                        gbClientHIVStatus.Visible = true;
                        cboFinalTestResult.Text = sqlrdr.GetValue(26).ToString();
                    }

                    if (sqlrdr.IsDBNull(27) == false)
                    {
                        gbAlternativeLocation.Enabled = true;
                        cboAlternativeSearchParameter.Text = sqlrdr.GetValue(27).ToString();
                    }

                    if (sqlrdr.IsDBNull(28) == false)
                    {

                        txtAltLocSearchValue.Text = sqlrdr.GetValue(28).ToString();
                    }


                    if (sqlrdr.IsDBNull(29) == false)
                    {
                        dpNextAppointmentDate.Text = sqlrdr.GetValue(29).ToString();
                    }


                    if (sqlrdr.IsDBNull(30) == false)
                    {
                        txtRemarks.Text = sqlrdr.GetValue(30).ToString();
                    }

                    


                    gbMatchStatus.Enabled = true;
                    btnSave.Enabled = false;
                    dgvPersonHDSSInfo.Rows.Clear();
                    dgvCompoundMembers.Rows.Clear();




                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            ReloadDSSLinkageWindow();
            PassLinkageVariables();

        }




        public void ClearFields()
        {
            //Retrieve_Facility_Departments();
            //Retrieve_FileRef_Types();
            txtFileRef.Text = "";
            txtFirstname.Text = "";
            txtMiddlename.Text = "";
            txtLastname.Text = "";
            txtMFirstname.Text = "";
            txtMMiddlename.Text = "";
            txtMLastname.Text = "";
            txtFFirstname.Text = "";
            txtFMiddlename.Text = "";
            txtFLastname.Text = "";
            txtCHFirstname.Text = "";
            txtCHMiddlename.Text = "";
            txtCHLastname.Text = "";
            //txtSubLocation.Text = "";
            cboGender.SelectedIndex = 0;
            cboMaritalStatus.Text = "";
            txtVillageName.Text = "";
            txtMatchStatus.Text = "";
            txtMatchedHDSSID.Text = "";
            txtPreviousMatchNotes.Text = "";
            dpLastSearchedDate.Text = DateTime.Now.Date.ToString();
            dpDob.Text = DateTime.Now.Date.ToString();
            dpRegDate.Text = DateTime.Now.Date.ToString();
            dpResidentStartDate.Text = DateTime.Now.Date.ToString();
            gbMatchStatus.Enabled = false;
            lblBanner.Visible = false;
            btnSave.Enabled = true;
            txtAltLocSearchValue.Text = "";
            gbAlternativeLocation.Enabled = false;

            if (gbAlternativeLocation.Enabled == true) { gbAlternativeLocation.Enabled = false; }

            if (chHDSSVillageNotFound.Checked == true) { chHDSSVillageNotFound.Checked = false; }
        }


        private void Retrieve_Facility_Departments()
        {

            string sql = @"select department_id,department_name from registry.facility_departments";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);
            cboFacilityDepartment.DisplayMember = "department_name";
            cboFacilityDepartment.ValueMember = "department_id";
            cboFacilityDepartment.DataSource = dtr;


            cboDepartmentVisited.DisplayMember = "department_name";
            cboDepartmentVisited.ValueMember = "department_id";
            cboDepartmentVisited.DataSource = dtr;

        }


        public void ReloadSearchWindow()
        {

            //these controls apply to the search client tab

            pnlMatchNotFound.Visible = false;
            dgvClientSearchResults.Visible = false;
            lnkCheckin.Visible = false;
            lnkLink.Visible = false;
            pnlCheck.Visible = false;
            //rbFileRef.Checked = true;
            txtSearchTerm.Text = "";
            lnkEnterHIVStatus.Visible = false;
            //rbFileRef.Checked = true;

            
            lblBanner.Visible = false;
            pnlFileRefType.Visible = true;
            rbFileRef.Checked = true;
            Retrieve_FileRef_Types();

        }

        public void PopulateFacilities()

        {

            string sql = @"EXEC retrieve_pilot_facilities";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);


            cboFacilityName.DisplayMember = "facilityname";
            cboFacilityName.ValueMember = "mflcode";
            cboFacilityName.DataSource = dtr;

        }


        public void Populate_TestTypes()
        {

            string sql = @"EXEC retrieve_test_types";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);


            cboTestTypeGB1.DisplayMember = "test_name";
            cboTestTypeGB1.ValueMember = "test_code";
            cboTestTypeGB1.DataSource = dtr;


            cboTestTypeGB2.DisplayMember = "test_name";
            cboTestTypeGB2.ValueMember = "test_code";
            cboTestTypeGB2.DataSource = dtr;


            cboTestTypeGB3.DisplayMember = "test_name";
            cboTestTypeGB3.ValueMember = "test_code";
            cboTestTypeGB3.DataSource = dtr;


            cboTestTypeGB4.DisplayMember = "test_name";
            cboTestTypeGB4.ValueMember = "test_code";
            cboTestTypeGB4.DataSource = dtr;



            cboTestTypeGB5.DisplayMember = "test_name";
            cboTestTypeGB5.ValueMember = "test_code";
            cboTestTypeGB5.DataSource = dtr;


            cboTestTypeGB6.DisplayMember = "test_name";
            cboTestTypeGB6.ValueMember = "test_code";
            cboTestTypeGB6.DataSource = dtr;

        }


        private void Retrieve_Marital_Status()
        {

            string sql = @"EXEC retrieve_marital_status";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);
            cboMaritalStatus.DisplayMember = "name";
            cboMaritalStatus.ValueMember = "mstatus_code";
            cboMaritalStatus.DataSource = dtr;


        }

        private void Retrieve_FileRef_Types()
        {
            string sql = @"select department_id,department_name from registry.facility_departments";
            DataTable dtr = new DataTable();


            SqlDataAdapter dar = new SqlDataAdapter(sql, con);

            dar.Fill(dtr);
            cboFileRefType.DisplayMember = "department_name";
            cboFileRefType.ValueMember = "department_id";
            cboFileRefType.DataSource = dtr;

            //Do this under the registry tab
            cboPReferenceType.DisplayMember = "department_name";
            cboPReferenceType.ValueMember = "department_id";
            cboPReferenceType.DataSource = dtr;

        }



        private void ReloadDSSLinkageWindow()
        {

            txtFirstnameLink.Text = "";
            txtMiddlenameLink.Text = "";
            txtLastnameLink.Text = "";
            txtMotherFirstnameLink.Text = "";
            txtMotherMiddlenameLink.Text = "";
            txtMotherLastnameLink.Text = "";
            txtCHMiddlenameLink.Text = "";
            txtCHFirstnameLink.Text = "";
            txtCHLastnameLink.Text = "";
            txtGenderLink.Text = "";
            txtVillageLink.Text = "";
            txtHDSS_IDLink.Text = "";
            txtDobLink.Text = "";
            txtAltLocSearchValueLink.Text = "";


            //if (pnlNoDSSMatch.Visible == true)
            //{
            //    pnlNoDSSMatch.Visible = false;
            //}

            if (pnlNoDSSMatch.Visible == true) { pnlNoDSSMatch.Visible = false; }
            if (dgvPersonHDSSInfo.Visible == false) { dgvPersonHDSSInfo.Visible = true; }
            if (dgvCompoundMembers.Visible == false) { dgvCompoundMembers.Visible = true; }


            if (dgvPersonHDSSInfo.Rows.Count != 0) { dgvPersonHDSSInfo.Rows.Clear(); }
            if (dgvCompoundMembers.Rows.Count != 0) { dgvCompoundMembers.Rows.Clear(); }



            //unchek the search variables

            if (cbFirstName.Checked == true) { cbFirstName.Checked = false; }


            if (cbMiddlename.Checked == true) { cbMiddlename.Checked = false; }

            if (cbLastName.Checked == true) { cbLastName.Checked = false; }

            if (cbMotherFirstname.Checked == true) { cbMotherFirstname.Checked = false; }

            if (cbMotherMiddlename.Checked == true) { cbMotherMiddlename.Checked = false; }

            if (cbMotherLastname.Checked == true) { cbMotherLastname.Checked = false; }

            if (cbCHFirstname.Checked == true) { cbCHFirstname.Checked = false; }

            if (cbCHMiddlename.Checked == true) { cbCHMiddlename.Checked = false; }

            if (cbCHLastname.Checked == true) { cbCHLastname.Checked = false; }

            if (cbVillage.Checked == true) { cbVillage.Checked = false; }

            if (cbGender.Checked == true) { cbGender.Checked = false; }

            if (chDob.Checked == true) { chDob.Checked = false; }

            if (cbAltLoc.Checked == true) { cbAltLoc.Checked = false; }




            //uncheck the blocking variables

            if (chBlockVillage.Checked == true) { chBlockVillage.Checked = false; }

            if (chBlockGender.Checked == true) { chBlockGender.Checked = false; }

            if (chBlockDob.Checked == true) { chBlockDob.Checked = false; }

            if (chBlockNone.Checked == true) { chBlockNone.Checked = false; }

            if (chBlockAltLoc.Checked == true) { chBlockAltLoc.Checked = false; }



        }


        public void SelectSearchTab()
        {
            tabControl1.SelectTab(tpSearchClient);

        }



        private void Search4DSSindividualPotentialMatches()
        {

            //Object value;
            //    value = DBNull.Value;
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    DataTable DssIndividuals = new DataTable();

            bool UseFirstName;
            if (cbFirstName.Checked == true)
            {
                UseFirstName = true;
                search_criteria = search_criteria + "First Name";
            }
            else
                UseFirstName = false;

            bool UseMiddleName;
            if (cbMiddlename.Checked == true)
            {
                UseMiddleName = true;
                search_criteria = search_criteria + ", Middle Name";
            }
            else
                UseMiddleName = false;

            bool UseLastName;
            if (cbLastName.Checked == true)
            {
                UseLastName = true;
                search_criteria = search_criteria + ", Last Name";
            }
            else
                UseLastName = false;




            
            bool UseBirthDate;

            if (chDob.Checked == true)
            {
                UseBirthDate = true;

                search_criteria = search_criteria + ", Birthdate";
            }

            else UseBirthDate = false;

            //blocking village

            bool BlockVillage;

            if (chBlockVillage.Checked == true)
            {

                BlockVillage = true;

            }

            else BlockVillage = false;

            //Blocking gender


            bool BlockGender;

            if (chBlockGender.Checked == true)
            {

                BlockGender = true;

            }

            else BlockGender = false;


            //Blocking birthdate



            bool BlockBirthDate;

            if (chBlockDob.Checked == true)
            {

                BlockBirthDate = true;
                //BlockBirthDate = false;

            }

            else BlockBirthDate = false;


            //blocking by a non


            bool BlockNone;

            if (chBlockNone.Checked == true)
            {

                BlockNone = true;

            }

            else BlockNone = false;


            


            //bool UseMFirstName;
            //if (cbMotherFirstname.Checked == true)
            //{
            //    UseMFirstName = true;
            //    //search_criteria = search_criteria + ", TCL First Name";
            //}
            //else
            //    UseMFirstName = false;

            //bool UseMMiddleName;
            //if (cbMotherMiddlename.Checked == true)
            //{
            //    UseMMiddleName = true;
            //    //search_criteria = search_criteria + ", TCL Middle Name";
            //}
            //else
            //    UseMMiddleName = false;

            //bool UseMLastName;
            //if (cbMotherLastname.Checked == true)
            //{
            //    UseMLastName = true;
            //    //search_criteria = search_criteria + ", TCL Last Name";
            //}
            //else
            //    UseMLastName = false;


            //Unchecking birthdate parameters


            //bool UseBirthYear;
            //if (cbYear.Checked == true)
            //{
            //    UseBirthYear = true;
            //    search_criteria = search_criteria + ", Birth year";
            //}
            //else
            //    UseBirthYear = false;


            //bool UseBirthMonth;
            //if (cbMonth.Checked == true)
            //{
            //    UseBirthMonth = true;
            //    search_criteria = search_criteria + ", Birth month";
            //}
            //else
            //    UseBirthMonth = false;



            //bool UseBirthDay;
            //if (cbDay.Checked == true)
            //{
            //    UseBirthDay = true;
            //    search_criteria = search_criteria + ", Birthday";
            //}
            //else
            //    UseBirthDay = false;



            bool UseGender;
            if (cbGender.Checked == true)
            {
                UseGender = true;
                search_criteria = search_criteria + ", Sex";
            }
            else
                UseGender = false;


            bool UseVillage;
            if (cbVillage.Checked == true)
            {
                UseVillage = true;
                search_criteria = search_criteria + ", Village";
            }
            else
                UseVillage = false;



            try
            {

                //close an old connection if still opened

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now create a new connection
                con.Open();

                //start of match by none

                if (chBlockNone.Checked == true)
                {

                    SqlCommand sqlcmd = new SqlCommand("MatchByNone", con);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 280;
                    sqlcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = txtFirstnameLink.Text;
                    sqlcmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddlenameLink.Text;
                    sqlcmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastnameLink.Text;
                    sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = cboGender.Text;
                    sqlcmd.Parameters.Add("@dob", SqlDbType.VarChar).Value = txtDobLink.Text;


                    //sqlcmd.Parameters.Add("@BYear", SqlDbType.VarChar).Value = txtYearLink.Text;
                    //sqlcmd.Parameters.Add("@BMonth", SqlDbType.VarChar).Value = txtMonthLink.Text;
                    //sqlcmd.Parameters.Add("@BDay", SqlDbType.VarChar).Value = txtDayLink.Text;

                    //sqlcmd.Parameters.Add("@DoB", SqlDbType.VarChar).Value = txtDoBLink.Text;

                    sqlcmd.Parameters.Add("@village", SqlDbType.VarChar).Value = txtVillageLink.Text;
                    sqlcmd.Parameters.Add("@UseFirstName", SqlDbType.Bit).Value = UseFirstName;
                    sqlcmd.Parameters.Add("@UseMiddleName", SqlDbType.Bit).Value = UseMiddleName;
                    sqlcmd.Parameters.Add("@UseLastName", SqlDbType.Bit).Value = UseLastName;
                    sqlcmd.Parameters.Add("@UseGender", SqlDbType.Bit).Value = UseGender;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = 1;
                    sqlcmd.Parameters.Add("@UseVillage", SqlDbType.Bit).Value = UseVillage;
                    sqlcmd.Parameters.Add("@UseBirthDate", SqlDbType.Bit).Value = UseBirthDate;


                    sqlcmd.Parameters.Add("@BlockVillage", SqlDbType.Bit).Value = BlockVillage;
                    sqlcmd.Parameters.Add("@BlockGender", SqlDbType.Bit).Value = BlockGender;
                    sqlcmd.Parameters.Add("@BlockBirthDate", SqlDbType.Bit).Value = BlockBirthDate;
                    sqlcmd.Parameters.Add("@BlockNone", SqlDbType.Bit).Value = BlockNone;

                    //sqlcmd.Parameters.Add("@UseBYear", SqlDbType.Bit).Value = UseBirthYear;
                    //sqlcmd.Parameters.Add("@UseBMonth", SqlDbType.Bit).Value = UseBirthMonth;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = UseBirthDay;


                    SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPersonHDSSInfo.Rows.Clear();



                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //EndLinkage.btnContinueClient.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvPersonHDSSInfo.Rows.Add();
                            dgvPersonHDSSInfo.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[1].Value = item["Middlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[2].Value = item["Lastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[3].Value = item["DoB"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[4].Value = item["MotherFirstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[5].Value = item["MotherMiddlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[6].Value = item["MotherLastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[7].Value = item["village_name"].ToString();
                            //dgvPersonHDSSInfo.Rows[n].Cells[9].Value = item["Score"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[8].Value = item["HDSS_ID"].ToString();
                        }





                    }

                    else
                    {
                        pnlNoDSSMatch.Visible = true;
                        dgvPersonHDSSInfo.Visible = false;
                        dgvCompoundMembers.Visible = false;

                    }




                }

                //end of match by none



                //start of match by village



                if (chBlockVillage.Checked == true)
                {

                    SqlCommand sqlcmd = new SqlCommand("MatchByVillage", con);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 280;
                    sqlcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = txtFirstnameLink.Text;
                    sqlcmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddlenameLink.Text;
                    sqlcmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastnameLink.Text;
                    sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = cboGender.Text;
                    sqlcmd.Parameters.Add("@dob", SqlDbType.VarChar).Value = txtDobLink.Text;


                    //sqlcmd.Parameters.Add("@BYear", SqlDbType.VarChar).Value = txtYearLink.Text;
                    //sqlcmd.Parameters.Add("@BMonth", SqlDbType.VarChar).Value = txtMonthLink.Text;
                    //sqlcmd.Parameters.Add("@BDay", SqlDbType.VarChar).Value = txtDayLink.Text;

                    //sqlcmd.Parameters.Add("@DoB", SqlDbType.VarChar).Value = txtDoBLink.Text;

                    sqlcmd.Parameters.Add("@village", SqlDbType.VarChar).Value = txtVillageLink.Text;
                    sqlcmd.Parameters.Add("@UseFirstName", SqlDbType.Bit).Value = UseFirstName;
                    sqlcmd.Parameters.Add("@UseMiddleName", SqlDbType.Bit).Value = UseMiddleName;
                    sqlcmd.Parameters.Add("@UseLastName", SqlDbType.Bit).Value = UseLastName;
                    sqlcmd.Parameters.Add("@UseGender", SqlDbType.Bit).Value = UseGender;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = 1;
                    sqlcmd.Parameters.Add("@UseVillage", SqlDbType.Bit).Value = UseVillage;
                    sqlcmd.Parameters.Add("@UseBirthDate", SqlDbType.Bit).Value = UseBirthDate;


                    sqlcmd.Parameters.Add("@BlockVillage", SqlDbType.Bit).Value = BlockVillage;
                    sqlcmd.Parameters.Add("@BlockGender", SqlDbType.Bit).Value = BlockGender;
                    sqlcmd.Parameters.Add("@BlockBirthDate", SqlDbType.Bit).Value = BlockBirthDate;
                    sqlcmd.Parameters.Add("@BlockNone", SqlDbType.Bit).Value = BlockNone;

                    //sqlcmd.Parameters.Add("@UseBYear", SqlDbType.Bit).Value = UseBirthYear;
                    //sqlcmd.Parameters.Add("@UseBMonth", SqlDbType.Bit).Value = UseBirthMonth;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = UseBirthDay;


                    SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPersonHDSSInfo.Rows.Clear();



                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //EndLinkage.btnContinueClient.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvPersonHDSSInfo.Rows.Add();
                            dgvPersonHDSSInfo.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[1].Value = item["Middlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[2].Value = item["Lastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[3].Value = item["DoB"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[4].Value = item["MotherFirstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[5].Value = item["MotherMiddlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[6].Value = item["MotherLastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[7].Value = item["village_name"].ToString();
                            //dgvPersonHDSSInfo.Rows[n].Cells[9].Value = item["Score"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[8].Value = item["HDSS_ID"].ToString();
                        }

                    }

                    else
                    {
                        pnlNoDSSMatch.Visible = true;
                        dgvPersonHDSSInfo.Visible = false;
                        dgvCompoundMembers.Visible = false;

                    }




                }

                //end of match by none




                //Start of block by sub location



                if (chBlockAltLoc.Checked == true && gbAltLoc.Text == "sublocation")
                {

                    
                    SqlCommand sqlcmd = new SqlCommand("MatchByDSSSubLocation", con);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 280;
                    sqlcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = txtFirstnameLink.Text;
                    sqlcmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddlenameLink.Text;
                    sqlcmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastnameLink.Text;
                    sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = cboGender.Text;
                    sqlcmd.Parameters.Add("@dob", SqlDbType.VarChar).Value = txtDobLink.Text;


                    //sqlcmd.Parameters.Add("@BYear", SqlDbType.VarChar).Value = txtYearLink.Text;
                    //sqlcmd.Parameters.Add("@BMonth", SqlDbType.VarChar).Value = txtMonthLink.Text;
                    //sqlcmd.Parameters.Add("@BDay", SqlDbType.VarChar).Value = txtDayLink.Text;

                    //sqlcmd.Parameters.Add("@DoB", SqlDbType.VarChar).Value = txtDoBLink.Text;

                    sqlcmd.Parameters.Add("@village", SqlDbType.VarChar).Value = txtVillageLink.Text;
                    sqlcmd.Parameters.Add("@UseFirstName", SqlDbType.Bit).Value = UseFirstName;
                    sqlcmd.Parameters.Add("@UseMiddleName", SqlDbType.Bit).Value = UseMiddleName;
                    sqlcmd.Parameters.Add("@UseLastName", SqlDbType.Bit).Value = UseLastName;
                    sqlcmd.Parameters.Add("@UseGender", SqlDbType.Bit).Value = UseGender;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = 1;
                    sqlcmd.Parameters.Add("@UseVillage", SqlDbType.Bit).Value = UseVillage;
                    sqlcmd.Parameters.Add("@UseBirthDate", SqlDbType.Bit).Value = UseBirthDate;


                    sqlcmd.Parameters.Add("@BlockVillage", SqlDbType.Bit).Value = BlockVillage;
                    sqlcmd.Parameters.Add("@BlockGender", SqlDbType.Bit).Value = BlockGender;
                    sqlcmd.Parameters.Add("@BlockBirthDate", SqlDbType.Bit).Value = BlockBirthDate;
                    sqlcmd.Parameters.Add("@BlockNone", SqlDbType.Bit).Value = BlockNone;

                    //sqlcmd.Parameters.Add("@UseBYear", SqlDbType.Bit).Value = UseBirthYear;
                    //sqlcmd.Parameters.Add("@UseBMonth", SqlDbType.Bit).Value = UseBirthMonth;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = UseBirthDay;

                    sqlcmd.Parameters.Add("@sublocation", SqlDbType.VarChar).Value = txtAltLocSearchValueLink.Text;

                    SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPersonHDSSInfo.Rows.Clear();



                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //EndLinkage.btnContinueClient.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvPersonHDSSInfo.Rows.Add();
                            dgvPersonHDSSInfo.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[1].Value = item["Middlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[2].Value = item["Lastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[3].Value = item["DoB"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[4].Value = item["MotherFirstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[5].Value = item["MotherMiddlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[6].Value = item["MotherLastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[7].Value = item["village_name"].ToString();
                            //dgvPersonHDSSInfo.Rows[n].Cells[9].Value = item["Score"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[8].Value = item["HDSS_ID"].ToString();
                        }





                    }

                    else
                    {
                        pnlNoDSSMatch.Visible = true;
                        dgvPersonHDSSInfo.Visible = false;
                        dgvCompoundMembers.Visible = false;

                    }






                }


                //this is the end of match by sub location




                //this is the begining of match by location



                if (chBlockAltLoc.Checked == true && gbAltLoc.Text == "location")
                {


                    SqlCommand sqlcmd = new SqlCommand("MatchByDSSLocation", con);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 280;
                    sqlcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = txtFirstnameLink.Text;
                    sqlcmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddlenameLink.Text;
                    sqlcmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastnameLink.Text;
                    sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = cboGender.Text;
                    sqlcmd.Parameters.Add("@dob", SqlDbType.VarChar).Value = txtDobLink.Text;


                    //sqlcmd.Parameters.Add("@BYear", SqlDbType.VarChar).Value = txtYearLink.Text;
                    //sqlcmd.Parameters.Add("@BMonth", SqlDbType.VarChar).Value = txtMonthLink.Text;
                    //sqlcmd.Parameters.Add("@BDay", SqlDbType.VarChar).Value = txtDayLink.Text;

                    //sqlcmd.Parameters.Add("@DoB", SqlDbType.VarChar).Value = txtDoBLink.Text;

                    sqlcmd.Parameters.Add("@village", SqlDbType.VarChar).Value = txtVillageLink.Text;
                    sqlcmd.Parameters.Add("@UseFirstName", SqlDbType.Bit).Value = UseFirstName;
                    sqlcmd.Parameters.Add("@UseMiddleName", SqlDbType.Bit).Value = UseMiddleName;
                    sqlcmd.Parameters.Add("@UseLastName", SqlDbType.Bit).Value = UseLastName;
                    sqlcmd.Parameters.Add("@UseGender", SqlDbType.Bit).Value = UseGender;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = 1;
                    sqlcmd.Parameters.Add("@UseVillage", SqlDbType.Bit).Value = UseVillage;
                    sqlcmd.Parameters.Add("@UseBirthDate", SqlDbType.Bit).Value = UseBirthDate;


                    sqlcmd.Parameters.Add("@BlockVillage", SqlDbType.Bit).Value = BlockVillage;
                    sqlcmd.Parameters.Add("@BlockGender", SqlDbType.Bit).Value = BlockGender;
                    sqlcmd.Parameters.Add("@BlockBirthDate", SqlDbType.Bit).Value = BlockBirthDate;
                    sqlcmd.Parameters.Add("@BlockNone", SqlDbType.Bit).Value = BlockNone;

                    //sqlcmd.Parameters.Add("@UseBYear", SqlDbType.Bit).Value = UseBirthYear;
                    //sqlcmd.Parameters.Add("@UseBMonth", SqlDbType.Bit).Value = UseBirthMonth;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = UseBirthDay;

                    sqlcmd.Parameters.Add("@location", SqlDbType.VarChar).Value = txtAltLocSearchValueLink.Text;

                    SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPersonHDSSInfo.Rows.Clear();



                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //EndLinkage.btnContinueClient.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvPersonHDSSInfo.Rows.Add();
                            dgvPersonHDSSInfo.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[1].Value = item["Middlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[2].Value = item["Lastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[3].Value = item["DoB"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[4].Value = item["MotherFirstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[5].Value = item["MotherMiddlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[6].Value = item["MotherLastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[7].Value = item["village_name"].ToString();
                            //dgvPersonHDSSInfo.Rows[n].Cells[9].Value = item["Score"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[8].Value = item["HDSS_ID"].ToString();
                        }





                    }

                    else
                    {
                        pnlNoDSSMatch.Visible = true;
                        dgvPersonHDSSInfo.Visible = false;
                        dgvCompoundMembers.Visible = false;

                    }






                }







                //this is the end of match by location



                //this is the start of block by region

                if (chBlockAltLoc.Checked == true && gbAltLoc.Text == "region")
                {

                    SqlCommand sqlcmd = new SqlCommand("MatchByDSSRegion", con);


                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandTimeout = 280;
                    sqlcmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = txtFirstnameLink.Text;
                    sqlcmd.Parameters.Add("@middlename", SqlDbType.VarChar).Value = txtMiddlenameLink.Text;
                    sqlcmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = txtLastnameLink.Text;
                    sqlcmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = cboGender.Text;
                    sqlcmd.Parameters.Add("@dob", SqlDbType.VarChar).Value = txtDobLink.Text;


                    //sqlcmd.Parameters.Add("@BYear", SqlDbType.VarChar).Value = txtYearLink.Text;
                    //sqlcmd.Parameters.Add("@BMonth", SqlDbType.VarChar).Value = txtMonthLink.Text;
                    //sqlcmd.Parameters.Add("@BDay", SqlDbType.VarChar).Value = txtDayLink.Text;

                    //sqlcmd.Parameters.Add("@DoB", SqlDbType.VarChar).Value = txtDoBLink.Text;

                    sqlcmd.Parameters.Add("@village", SqlDbType.VarChar).Value = txtVillageLink.Text;
                    sqlcmd.Parameters.Add("@UseFirstName", SqlDbType.Bit).Value = UseFirstName;
                    sqlcmd.Parameters.Add("@UseMiddleName", SqlDbType.Bit).Value = UseMiddleName;
                    sqlcmd.Parameters.Add("@UseLastName", SqlDbType.Bit).Value = UseLastName;
                    sqlcmd.Parameters.Add("@UseGender", SqlDbType.Bit).Value = UseGender;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = 1;
                    sqlcmd.Parameters.Add("@UseVillage", SqlDbType.Bit).Value = UseVillage;
                    sqlcmd.Parameters.Add("@UseBirthDate", SqlDbType.Bit).Value = UseBirthDate;


                    sqlcmd.Parameters.Add("@BlockVillage", SqlDbType.Bit).Value = BlockVillage;
                    sqlcmd.Parameters.Add("@BlockGender", SqlDbType.Bit).Value = BlockGender;
                    sqlcmd.Parameters.Add("@BlockBirthDate", SqlDbType.Bit).Value = BlockBirthDate;
                    sqlcmd.Parameters.Add("@BlockNone", SqlDbType.Bit).Value = BlockNone;

                    //sqlcmd.Parameters.Add("@UseBYear", SqlDbType.Bit).Value = UseBirthYear;
                    //sqlcmd.Parameters.Add("@UseBMonth", SqlDbType.Bit).Value = UseBirthMonth;
                    //sqlcmd.Parameters.Add("@UseBDay", SqlDbType.Bit).Value = UseBirthDay;

                    sqlcmd.Parameters.Add("@region", SqlDbType.VarChar).Value = txtAltLocSearchValueLink.Text;

                    SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvPersonHDSSInfo.Rows.Clear();



                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //EndLinkage.btnContinueClient.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvPersonHDSSInfo.Rows.Add();
                            dgvPersonHDSSInfo.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[1].Value = item["Middlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[2].Value = item["Lastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[3].Value = item["DoB"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[4].Value = item["MotherFirstname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[5].Value = item["MotherMiddlename"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[6].Value = item["MotherLastname"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[7].Value = item["village_name"].ToString();
                            //dgvPersonHDSSInfo.Rows[n].Cells[9].Value = item["Score"].ToString();
                            dgvPersonHDSSInfo.Rows[n].Cells[8].Value = item["HDSS_ID"].ToString();
                        }





                    }

                    else
                    {
                        pnlNoDSSMatch.Visible = true;
                        dgvPersonHDSSInfo.Visible = false;
                        dgvCompoundMembers.Visible = false;

                    }


                }


                //this is the end of the code that blocks by region






            }


            catch (Exception Ex)
            {

                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

        }



        private void InsertMatch()
        {
            if (cboFacilityName.Text != "")
            {
                Object value;
                value = DBNull.Value;

                try
                {
                    match_state = 1;
                    // open connection
                    //con.Open();


                    //check if there is an already existing HDSS Match

                    CheckDuplicateHDSSMatch();


                    if (is_duplicate_HDSS >= 1)
                    {

                        DialogResult dialogResult = MessageBox.Show("This client is already matched" + " " + " Do you wish to quit the linkage attempt?", "SETS Linkage Details", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            RegisterAndCheckin();

                        }

                        else if (dialogResult == DialogResult.No)
                        {

                            return;

                        }
                    }// the outer if statement ends here

                    //close an old connection if still open
                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }


                    // create command

                    con.Open();
                    SqlCommand sqlcmd = con.CreateCommand();
                    // specify stored procedure to execute
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "insert_match";
                    //add health facility parameter
                    sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                    sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                    sqlcmd.Parameters.Add("@pfile_ref", SqlDbType.VarChar).Value = txtFileRef.Text;
                    sqlcmd.Parameters.Add("@pfile_ref_type", SqlDbType.VarChar).Value = cboPReferenceType.Text;
                    sqlcmd.Parameters.Add("@search_criteria", SqlDbType.VarChar).Value = search_criteria;
                    sqlcmd.Parameters.Add("@HDSS_ID", SqlDbType.VarChar).Value = txtHDSS_IDLink.Text;
                    sqlcmd.Parameters.Add("@matched_by", SqlDbType.VarChar).Value = logged_user;
                    sqlcmd.Parameters.Add("@match_state", SqlDbType.VarChar).Value = match_state;
                    sqlcmd.ExecuteNonQuery();

                    if (txtHDSS_IDLink.Text.Length > 0)
                    {
                        String l_firstname = txtFirstnameLink.Text.ToUpper().Trim();
                        String l_middlename = txtMiddlenameLink.Text.ToUpper().Trim();
                        String l_lastname = txtLastnameLink.Text.ToUpper().Trim();




                        //ClearAllTabs();
                        //tabControl1.SelectTab(tpClientSearch);

                        

                        



                        //MessageBox.Show("Match assigned", "Match information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }//RecordSaved = true;
                }

                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString(), "File and HDSS match Record Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }


                EndLinkage el = new EndLinkage();

                el.StartPosition = FormStartPosition.CenterParent;
                el.ShowDialog();

                RegisterAndCheckin();

            }
        }


        public void RetrievePersonID()

        {

            try
            {

               
                //close old connection if it is still open
                if (con != null && con.State == ConnectionState.Open)
                {
                    // do something
                    con.Close();
                }


                // open connection
                con.Open();
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "retrieve_PersonID";
                //add health facility parameter
                sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                sqlcmd.Parameters.Add("@pfile_ref", SqlDbType.VarChar).Value = txtFileRef.Text;
                sqlcmd.Parameters.Add("@pfile_ref_type", SqlDbType.VarChar).Value = cboPReferenceType.Text;
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        active_system_ID = sqlrdr.GetValue(0).ToString();
                    }


                } //this is the end of the while statement

            

            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "HDSS Linkage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            
        
        }



        public void PopulateClientSearchGrid()
        {


            
           
                try
                {

                    //close an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close();  }


                    // open a new connection

                    con.Open();


                    // create command
                    SqlCommand sqlcmd = con.CreateCommand();
                    // specify stored procedure to execute
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "retrieve_PersonID";
                    //add health facility parameter
                    sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                    sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                    sqlcmd.Parameters.Add("@pfile_ref", SqlDbType.VarChar).Value = txtFileRef.Text;
                    sqlcmd.Parameters.Add("@pfile_ref_type", SqlDbType.VarChar).Value = cboPReferenceType.Text;
                    SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                    while (sqlrdr.Read())
                    {

                        if (sqlrdr.IsDBNull(0) == false)
                        {
                            System_ID = sqlrdr.GetValue(0).ToString();
                        }


                    } //this is the end of the while statement

                    sqlrdr.Close();



                    con.Close();



                    //close an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }


                    con.Open();
                    SqlCommand sqld = new SqlCommand("populate_client_datagrid", con);
                    sqld.CommandType = CommandType.StoredProcedure;
                    sqld.Parameters.AddWithValue("@system_id", System_ID);


                    SqlDataAdapter sda = new SqlDataAdapter(sqld);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dgvClientSearchResults.Rows.Clear();


                    if (dt != null && dt.Rows.Count > 0)
                    {
                        tabControl1.SelectTab(tpSearchClient);
                        //make the data grid view visible in case a match is found
                        dgvClientSearchResults.Visible = true;
                        pnlMatchNotFound.Visible = false;

                        foreach (DataRow item in dt.Rows)
                        {
                            int n = dgvClientSearchResults.Rows.Add();
                            dgvClientSearchResults.Rows[n].Cells[0].Value = item["pfile_ref_type"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[1].Value = item["pfile_ref"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[2].Value = item["firstname"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[3].Value = item["middlename"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[4].Value = item["lastname"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[5].Value = item["Match_Status"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[6].Value = item["HIV_Status"].ToString();
                            //dgvClientSearchResults.Rows[n].Cells[7].Value = item["person_id"].ToString();
                            dgvClientSearchResults.Rows[n].Cells[8].Value = item["system_id"].ToString();
                            //int client_id = int.Parse(item["HIV_Status"].ToString());


                        }

                    }

                       // pnlCheck.Visible = true;
                        chSave.Focus();


                             


                



            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "HDSS Linkage Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

        }

        public static void RegisterAndCheckin()
        {
            //_instance.RetrievePersonID();
            _instance.PopulateClientSearchGrid();
        }

        private void btnSearchDSS_Click(object sender, EventArgs e)
        {
            Search4DSSindividualPotentialMatches();
        }

        private void btnAssignMatch_Click(object sender, EventArgs e)
        {
            InsertMatch();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
            ReloadDSSLinkageWindow();
            ReloadSearchWindow();
           
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;

            btnEdit.Enabled = false;
        }


        public void SearchByName()
        {

            //close an old connection if still open

            if (con != null && con.State == ConnectionState.Open)

            { con.Close(); }


            //now opening a new connection
            con.Open();


            SqlCommand sqlcmd = new SqlCommand("Search_By_Name", con);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
            sqlcmd.Parameters.AddWithValue("@facility_department", cboFacilityDepartment.Text);
            sqlcmd.Parameters.AddWithValue("@search_name", txtSearchTerm.Text);


            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                       
            //DataTable dt = new DataTable();
            //sda.Fill(dt);
            //dgvClientSearchResults.Rows.Clear();

            SqlDataReader sdr = sqlcmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);
            dgvClientSearchResults.Rows.Clear();



            if (dt != null && dt.Rows.Count > 0)
            {
                //make the data grid view visible in case a match is found
                dgvClientSearchResults.Visible = true;
                pnlMatchNotFound.Visible = false;

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvClientSearchResults.Rows.Add();
                    dgvClientSearchResults.Rows[n].Cells[0].Value = item["pfile_ref_type"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[1].Value = item["pfile_ref"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[2].Value = item["firstname"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[3].Value = item["middlename"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[4].Value = item["lastname"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[5].Value = item["Match_Status"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[6].Value = item["HIV_Status"].ToString();
                    //dgvClientSearchResults.Rows[n].Cells[7].Value = item["person_id"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[7].Value = item["test_date"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[8].Value = item["system_id"].ToString();
                    //int client_id = int.Parse(item["HIV_Status"].ToString());
                    //txtPersonID.Text = item["person_id"].ToString();

                }


            }



                // this applies to ClientSize wishing to search by id

            else
            {

                pnlMatchNotFound.Visible = true;
                dgvClientSearchResults.Visible = false;
                lnkEnterHIVStatus.Visible = false;
                lnkLink.Visible = false;
                lnkCheckin.Visible = false;


            }


            //now close the connection used in the above data reader
            con.Close();
        }



        private void SearchbyFileRef()
        {

            //SqlDataAdapter sda = new SqlDataAdapter("select file_ref,firstname,middlename,lastname from registry.person where (file_ref='" + txtSearchTerm.Text + "')", con);

            //close old connection if still opened

            if (con != null && con.State == ConnectionState.Open)

            { con.Close(); }

            
            //attempt opening a new connection
            con.Open();
            
            SqlCommand sqlcmd = new SqlCommand("Search_By_FileRef", con);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
            sqlcmd.Parameters.AddWithValue("@facility_department", cboFacilityDepartment.Text);
            sqlcmd.Parameters.AddWithValue("@s_ref_type", cboFileRefType.Text);
            sqlcmd.Parameters.AddWithValue("@s_ref_no", txtSearchTerm.Text);


            //SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);

            SqlDataReader sdr = sqlcmd.ExecuteReader();

            
            DataTable dt = new DataTable();

            dt.Load(sdr);
            dgvClientSearchResults.Rows.Clear();


            if (dt != null && dt.Rows.Count > 0)
            {
                //make the data grid view visible in case a match is found
                dgvClientSearchResults.Visible = true;
                pnlMatchNotFound.Visible = false;

                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvClientSearchResults.Rows.Add();
                    dgvClientSearchResults.Rows[n].Cells[0].Value = item["pfile_ref_type"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[1].Value = item["pfile_ref"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[2].Value = item["firstname"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[3].Value = item["middlename"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[4].Value = item["lastname"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[5].Value = item["Match_Status"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[6].Value = item["HIV_Status"].ToString();
                    //dgvClientSearchResults.Rows[n].Cells[7].Value = item["person_id"].ToString();
                   // dgvClientSearchResults.Rows[n].Cells[7].Value = item["test_date"].ToString();
                    dgvClientSearchResults.Rows[n].Cells[8].Value = item["system_id"].ToString();
                    //txtPersonID.Text = item["person_id"].ToString();

                }


            }



                // this applies to ClientSize wishing to search by id

            else
            {

                pnlMatchNotFound.Visible = true;
                dgvClientSearchResults.Visible = false;
                lnkEnterHIVStatus.Visible = false;
                lnkLink.Visible = false;
                lnkCheckin.Visible = false;
            }

            con.Close();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lnkEnterHIVStatus.Visible = false;
            lnkLink.Visible = false;
            lnkCheckin.Visible = false;
            lblBanner.Visible = false;
            btnSaveAppointment.Enabled = true;
            ClearFields();
            ReloadDSSLinkageWindow();
            

            if (rbName.Checked == true)
            {
                SearchByName();
            }

            else if (rbFileRef.Checked == true)
            {
                SearchbyFileRef();
            }
        }

        private void lblRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl1.SelectTab(tbRegisterClient);
        }

        private void lnkCheckin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlCheck.Visible = true;
            lblBanner.Text = "File ref:  " + pfile_ref + "(" + pfile_ref_type + ")  Name: " + c_firstname + " " + c_middlename + " " + c_lastname + " ";
            if (lblBanner.Visible == false)
            {
                lblBanner.Visible = true;

            }
        }

        private void chSave_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert_checkin_details", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
            cmd.Parameters.AddWithValue("@facility_department", cboFacilityDepartment.Text);
            //cmd.Parameters.AddWithValue("@person_id", person_ID);
            cmd.Parameters.AddWithValue("@system_id", System_ID);
            cmd.Parameters.AddWithValue("department_visited", cboDepartmentVisited.Text);
            cmd.Parameters.AddWithValue("checkin_datetime", dpCheckinDate.Value);



            try
            {

                //close an old connetion if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //open a new connection
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    //ClearAllTabs();

                    EndCheckin te = new EndCheckin();
                    //t.Visible = true;

                    te.StartPosition = FormStartPosition.CenterParent;
                    te.ShowDialog();


                    }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

           
        }



        private void UpdateResultLabel()
        {

            try
            {

                //close an old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                // open connection
                con.Open();
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "get_testresult";
                //add parameters
                sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = System_ID;

                // Read in the SELECT results.
                //
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {


                        hiv_status = sqlrdr.GetValue(0).ToString().ToUpper();
                        //lblTestResult.Visible = true;
                        gbClientHIVStatus.Visible = true;

                        if (btnResultSave.Enabled == false) { btnResultSave.Enabled = true; }

                        //lblTestResult.Text = "HIV Status so far: " + hiv_status;
                        //lblTestResult.Visible = false;





                    }

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

        }

        private void btn1Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            //cmd.Parameters.AddWithValue("@person_id", person_ID);
            cmd.Parameters.AddWithValue("@system_id", System_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB1.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB1.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB1.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester1.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo1.Text);


            try
            {
                //close an existing connection if still opened

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //open a new conenction

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);



                    btn1Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }




        private void SaveHIVTestDetails()
        {

            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            //cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("@system_id", System_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB1.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB1.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB1.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester1.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo1.Text);



            try
            {
                //close an old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //try opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);



                    btn1Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();


        }

        private void btn1AnotherTest_Click(object sender, EventArgs e)
        {
            if (btn1Save.Enabled)
            {
                SaveHIVTestDetails();
            }
            GB2.Visible = true;
            if (btn2Save.Enabled == false) { btn2Save.Enabled = true; }
        }

        private void btn2Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("@system_id", System_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB2.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB2.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB2.Value.Date);


            try
            {

                //close an old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //try opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn2Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }




        private void SaveHIVTest2Details()
        {

            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            //cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("@system_id", System_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB2.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB2.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB2.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester2.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo2.Text);



            try
            {

                //close an existing connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                //try opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn2Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }

        private void btn2AnotherTest_Click(object sender, EventArgs e)
        {
            if (btn2Save.Enabled)
            {
                SaveHIVTest2Details();
            }
            GB3.Visible = true;
        }

        private void btn3Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            //cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("system_id", System_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB3.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB3.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB3.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester3.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo3.Text);



            try
            {

                //close an exisiting connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                //try opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn3Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();


        }

        private void btn3AnotherTest_Click(object sender, EventArgs e)
        {
            if (btn3Save.Enabled)
            {

                SaveHIVTest3Details();


            }
            GB4.Visible = true;
        }



        private void SaveHIVTest3Details()
        {

            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB2.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB2.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB2.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester3.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo3.Text);


            try
            {

                //close an old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now try opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn3Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }

        private void btn4Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB4.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB4.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB4.Value.Date);


            try
            {
                //close old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection

                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn4Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();


        }


        private void SaveHIVTest4Details()
        {

            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB4.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB4.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB4.Value.Date);
            cmd.Parameters.AddWithValue("@tester", cboTester4.Text);
            cmd.Parameters.AddWithValue("@lot_no", txtLotNo4.Text);



            try
            {

                //close old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now open a new connection
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn4Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }

        private void btn4AnotherTest_Click(object sender, EventArgs e)
        {
            if (btn4Save.Enabled)
            {

                SaveHIVTest3Details();


            }
            GB4.Visible = true;
        }

        private void btn5Save_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("update_create_testresult", con);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("person_id", person_ID);
            cmd.Parameters.AddWithValue("test_type", cboTestTypeGB5.Text);
            cmd.Parameters.AddWithValue("test_result", cboTestResultGB5.Text);
            cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB5.Value.Date);


            try
            {
                //close old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection
                
                con.Open();
                int k = cmd.ExecuteNonQuery();
                //Clear();
                if (k != 0)
                {

                    MessageBox.Show(" " +
"HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);

                    btn5Save.Enabled = false;


                    //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                    //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                }


            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            finally
            { con.Close(); }

            UpdateResultLabel();

        }

        private void btnBackToSearchWindow_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tpSearchClient);
            ReloadSearchWindow();
        }

        private void rbFileRef_CheckedChanged(object sender, EventArgs e)
        {

            if (rbFileRef.Checked == true)
            {



                if (rbName.Checked == true)
                {
                    rbName.Checked = false;

                }


                pnlFileRefType.Visible = true;
                pnlSearchPane.Top = 35;

                ReloadSearchWindow();
            }

        }

        private void rbName_CheckedChanged(object sender, EventArgs e)
        {
            if (rbName.Checked == true)
            {



                if (rbFileRef.Checked == true)
                {
                    rbFileRef.Checked = false;

                }


                //pnlSearchPane.Location = new Point(20, 20);

                pnlSearchPane.Top = 6;



                pnlFileRefType.Visible = false;

                txtSearchTerm.Text = "";

                //ReloadSearchWindow();

            }
        }


        private void GenerateXMLNodes()
        {

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString))
            {
                
                //close old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now attempting to open a new connection
                
                con.Open();

                //get the count of clients entered today
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "trail_conflicting_info";
                //add parameters
                sqlcmd.Parameters.Add("@system_id", SqlDbType.VarChar).Value = System_ID;
                sqlcmd.Parameters.Add("@facility_mflcode", SqlDbType.VarChar).Value = cboFacilityName.SelectedValue;
                sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                sqlcmd.Parameters.Add("@fileref", SqlDbType.VarChar).Value = pfile_ref;

                // Read in the SELECT results.
                //
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                
                    while (sqlrdr.Read())
                    {

                        if (sqlrdr.IsDBNull(0) == false)
                        {
                            RawXML = sqlrdr.GetValue(0).ToString();
                            //txtPatientRemarks.Text = sqlrdr.GetValue(0).ToString();


                            string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                            if (RawXML.StartsWith(_byteOrderMarkUtf8))
                            {
                                var lastIndexOfUtf8 = _byteOrderMarkUtf8.Length - 1;
                                RawXML = RawXML.Remove(0, lastIndexOfUtf8);
                            }

                            //XmlDocument xmlDoc = new XmlDocument();
                            //xmlDoc.LoadXml("RawXML");


                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(RawXML);


                            XmlNodeList firstname = xmlDoc.GetElementsByTagName("firstname");
                            XmlNodeList lastname = xmlDoc.GetElementsByTagName("lastname");
                            XmlNodeList middlename = xmlDoc.GetElementsByTagName("middlename");
                            XmlNodeList pfileref = xmlDoc.GetElementsByTagName("pfile_ref");
                            XmlNodeList mfname = xmlDoc.GetElementsByTagName("mfname");
                            XmlNodeList dob = xmlDoc.GetElementsByTagName("dob");


                            String middle_name = middlename[0].InnerText.ToString();
                           // String m_fname = mfname[0].InnerText.ToString();

                            //String birthdate = dob[0].InnerText.ToString();


                            //clear the remarks window in case an entry was made for a previous client
                            if (txtPatientRemarks.Text.Length > 0) { txtPatientRemarks.Text = ""; }

                            if (middle_name != "unchanged")
                            {
                                remark = "Middlename \r\n";
                                remark = remark + middle_name + "\r\n\r\n";
                            }

                            //if (m_fname != "unchanged")
                            //{
                            //    remark = remark + "Mother firstname \r\n";
                            //    remark = remark + m_fname + "\r\n\r\n";
                            //}

                         
                           //if (birthdate != "unchanged")
                           // {
                           //    remark = remark + "Birth dates\r\n";
                           //     remark = remark + birthdate + "\r\n\r\n";
                           // }
                          
                          


                            txtPatientRemarks.Text = remark;


                        }

                    }

                    sqlrdr.Close();

                


                //close the connection here

                con.Close();

            }

        }

        private void dgvClientSearchResults_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            pfile_ref_type = dgvClientSearchResults.SelectedRows[0].Cells[0].Value.ToString();
            pfile_ref = dgvClientSearchResults.SelectedRows[0].Cells[1].Value.ToString();
            c_firstname = dgvClientSearchResults.SelectedRows[0].Cells[2].Value.ToString();
            c_middlename = dgvClientSearchResults.SelectedRows[0].Cells[3].Value.ToString();
            c_lastname = dgvClientSearchResults.SelectedRows[0].Cells[4].Value.ToString();
            c_matchstatus = dgvClientSearchResults.SelectedRows[0].Cells[5].Value.ToString();
            hiv_status = dgvClientSearchResults.SelectedRows[0].Cells[6].Value.ToString();
            //person_ID = int.Parse(dgvClientSearchResults.SelectedRows[0].Cells[7].Value.ToString());
            //System_ID = dgvClientSearchResults.SelectedRows[0].Cells[8].Value.ToString();
            System_ID = dgvClientSearchResults.SelectedRows[0].Cells[8].Value.ToString();

            if (btnEdit.Enabled == false) { btnEdit.Enabled = true; }

            if (txtRemarks.Text.Length > 0) { txtRemarks.Text = ""; }

            if (txtPatientFileRef.Text.Length > 0) { txtPatientFileRef.Text = ""; }
           

            UpdateBanner();
            GetPersonDetails();

            if (txtPatientRemarks.Text.Length > 0) { txtPatientRemarks.Text = ""; }

            //clear the remarks window
           

            GenerateXMLNodes();

            if (txtPatientRemarks.Text.Length > 0)

            {

                DialogResult dialogResult = MessageBox.Show("Conflicting details entered for client with  " + pfile_ref_type + " " + "number " + pfile_ref + " Click Yes to review ", "SETS Conflict Info Alert", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    tabControl1.SelectTab(tpRemarks);

                    if (gbConflictingInfo.Visible == false) { gbConflictingInfo.Visible = true; }

                }

                else if (dialogResult == DialogResult.No)
                {

                    

                }
            
            }

            


            if (c_matchstatus == "UNMATCHED" || c_matchstatus=="")
            {
                lnkLink.Visible = true;
            }

            if (hiv_status == "NOT ENTERED" || hiv_status == "NEGATIVE")
            {
                lnkEnterHIVStatus.Visible = true;
            }

            lnkCheckin.Visible = true;


            //perform the following under the HIV test tab and checkout tab

            ReloadHIVTestWindow();
            ReloadCheckoutWindow();






        }

        private void ReloadHIVTestWindow()

        {

            gbClientHIVStatus.Visible = false;
            btn1Save.Enabled = true;
            if (GB2.Visible == true) { GB2.Visible = false; }
            if (GB3.Visible == true) { GB3.Visible = false; }
            if (GB4.Visible == true) { GB4.Visible = false; }
            if (GB5.Visible == true) { GB5.Visible = false; }
            if (GB6.Visible == true) { GB6.Visible = false; }

            cboTester1.Text = "";
            txtLotNo1.Text = "";
            cboTestTypeGB1.Text = "";
            cboTestResultGB1.Text = "";
            dpHIVTestDateGB1.Text = DateTime.Now.Date.ToString();

        }


        private void ReloadCheckoutWindow()

        {

            dpNextAppointmentDate.Text = DateTime.Now.Date.ToString();
            btnSaveAppointment.Enabled = true;
        
        }

        private void dgvPersonHDSSInfo_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtHDSS_IDLink.Text = dgvPersonHDSSInfo.SelectedRows[0].Cells[8].FormattedValue.ToString();

            //Code snippet to retrive compound members


            try
            {
                //close old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now attempting to open a new connection

                con.Open();

                SqlCommand sqlcmd = new SqlCommand("retrieve_compound_members", con);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandTimeout = 280;
                sqlcmd.Parameters.Add("@HDSS_Id", SqlDbType.VarChar).Value = txtHDSS_IDLink.Text;


                SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvCompoundMembers.Rows.Clear();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvCompoundMembers.Rows.Add();
                    dgvCompoundMembers.Rows[n].Cells[0].Value = item["Firstname"].ToString();
                    dgvCompoundMembers.Rows[n].Cells[1].Value = item["Juokname"].ToString();
                    dgvCompoundMembers.Rows[n].Cells[2].Value = item["Lastname"].ToString();

                }


                //lblCompoundMembers.Text = "Client's compound Members";


            }


            catch (Exception Ex)
            {

                MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }



        public void Retrieve_DSS_Villages()
        {


            //SqlCommand cmd = new SqlCommand("select distinct village_name from reference.dss_village", con);
            //con.Open();
            //SqlDataReader reader = cmd.ExecuteReader();
            //AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            //while (reader.Read())
            //{
            //    MyCollection.Add(reader.GetString(0));
            //}
            //txtVillageName.AutoCompleteCustomSource = MyCollection;
            //con.Close();


            //the section here below presents an alternative code

            con.Open();

            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            //string querry = @"select distinct village_name from reference.dss_village;";

            string querry = @"select distinct villname from reference.hdss;";

            SqlCommand cmd = new SqlCommand(querry, con);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {

                while (dr.Read())

                    namesCollection.Add(dr["villname"].ToString());



            }



            dr.Close();

            con.Close();



            txtVillageName.AutoCompleteMode = AutoCompleteMode.Append;

            txtVillageName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtVillageName.AutoCompleteCustomSource = namesCollection;

        }

        private void txtVillageName_KeyUp(object sender, KeyEventArgs e)
        {
            Retrieve_DSS_Villages();
        }

        private void chBlockNone_CheckedChanged(object sender, EventArgs e)
        {
            if (chBlockNone.Checked == true)
            {


                if (chBlockVillage.Checked == true)
                {
                    chBlockVillage.Checked = false;
                }


                if (chBlockGender.Checked == true)
                {
                    chBlockGender.Checked = false;
                }


                if (chBlockDob.Checked == true)
                {
                    chBlockDob.Checked = false;
                }



            }
        }




        private void SaveFinalTestResult()
        {


            if ((cboFinalTestResult.Text == "POSITIVE") && (is_hiventry_new==1))

            {

                CheckDuplicatePositiveStatusEntry();

                if (is_duplicate_hiv_entry >= 1)
                {

                    DialogResult dialogResult = MessageBox.Show("This client already has a " + cboFinalTestResult.Text +  " HIV status entry " + "Click Yes to update the entry or No to review", "SETS HIV ENtry Pane", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        
                        
                        //RegisterAndCheckin();

                        cboFinalTestResult.Focus();
                        return;

                    }

                    else if (dialogResult == DialogResult.No)
                    {

                        EndTestEntry e2 = new EndTestEntry();

                        e2.StartPosition = FormStartPosition.CenterParent;
                        e2.ShowDialog();
                        
                        return;

                    }
                }


            
            }


            if (is_hiventry_new == 1)
            {

                SqlCommand cmd = new SqlCommand(@"insert into registry.final_test_result(system_id,test_result,test_date,created_by,created_at) 
            values('" + System_ID + "','" + cboFinalTestResult.Text + "','" + dpHIVDiagnosisDate.Value.Date + "','" + logged_user + "','" + DateTime.Now.Date + "')", con);




                con.Open();
                cmd.ExecuteNonQuery();

                btnResultSave.Enabled = false;

                con.Close();


            }

            else if (is_hiventry_new == 0)

            {
            
              // we are now going to update the HIV status entry using the new details


                SqlCommand cmd = new SqlCommand(@"update registry.final_test_result SET test_result='" + cboFinalTestResult.Text + "',test_date='" + dpHIVDiagnosisDate.Value.Date + "',"+ 
                    "created_by='" + logged_user + "', modified_at='" + DateTime.Now.Date + "' where system_id='" + System_ID + "'", con);

                con.Open();
                cmd.ExecuteNonQuery();

                btnResultSave.Enabled = false;

                con.Close();
            
            }
            

//            MessageBox.Show(" " +
//"Client HIV test result successfully saved", "SETS: Client HIV Test Details", MessageBoxButtons.OKCancel,
//MessageBoxIcon.Information);
            


            EndTestEntry el = new EndTestEntry();

            el.StartPosition = FormStartPosition.CenterParent;
            el.ShowDialog();


        }

        private void rbYesFCHead_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYesFCHead.Checked == true)
            {
                rbNoFChead.Checked = false;
                txtCHFirstname.Text = txtFFirstname.Text;
                txtCHMiddlename.Text = txtFMiddlename.Text;
                txtCHLastname.Text = txtFLastname.Text;

            }
        }

        private void rbNoFChead_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoFChead.Checked == true)
            {
                rbYesFCHead.Checked = false;
                txtCHFirstname.Text = "";
                txtCHMiddlename.Text = "";
                txtCHLastname.Text = "";

            }
        }

        private void btnResultSave_Click(object sender, EventArgs e)
        {
            string selected_result = cboFinalTestResult.SelectedItem.ToString();

            if (selected_result == hiv_status)
            {
                SaveFinalTestResult();
            }

            else if (selected_result != hiv_status)
            {

                DialogResult dialogResult = MessageBox.Show("The recorded and the expected final result conflict. Are you sure of the selected status", "SETS Client HIV Test Details", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SaveFinalTestResult();

                }

                else if (dialogResult == DialogResult.No)
                {
                    //do something else

                    cboFinalTestResult.Focus();
                    return;
                }


               

            }
        }




        public static void ClearAllTabs()
        {
            _instance.ReloadDSSLinkageWindow();
            _instance.ReloadSearchWindow();
            _instance.ClearFields();
            _instance.SelectSearchTab();






        }

        private void lnkEnterHIVStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tpHIVStatus.Enabled = true;
            //tabControl1.SelectTab(tpHIVStatus);
            tabControl1.SelectTab(tpHIVStatus);
            lblBanner.Text = "File ref:  " + pfile_ref + "(" + pfile_ref_type + ")  Name: " + c_firstname + " " + c_middlename + " " + c_lastname + " ";
            if (lblBanner.Visible == false)
            {
                lblBanner.Visible = true;

            }
        }

        private void rbSearchHDSSLater_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSearchHDSSLater.Checked == true)
            {

                
                //closing an old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                //now attempting to open a new connection
                con.Open();
                SqlCommand cmd = new SqlCommand(@"update registry.person SET sent=0 where system_id=System_ID", con);
                cmd.ExecuteNonQuery();

                //Ensure that form tabs reload with the default configurations
                //ClearAllTabs();
                //tabControl1.SelectTab(tpClientSearch);

                EndLinkage el = new EndLinkage();

                el.StartPosition = FormStartPosition.CenterParent;
                el.ShowDialog();

                con.Close();


            }
        }

        private void rbMarkClientNonHDSS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMarkClientNonHDSS.Checked == true)
            {
                FlagAsNonHDSS();
                EndLinkage el = new EndLinkage();

                el.StartPosition = FormStartPosition.CenterParent;
                el.ShowDialog();

            }


        }



            private void FlagAsNonHDSS()

        {

            try
            {

                //close and old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                // now attempting to open connection
                con.Open();
                // create command
                SqlCommand sqlcmd = con.CreateCommand();
                // specify stored procedure to execute
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "flag_Non_HDSS";
                //add health facility parameter
                sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
                sqlcmd.Parameters.Add("@facility_department", SqlDbType.VarChar).Value = cboFacilityDepartment.Text;
                sqlcmd.Parameters.Add("@pfile_ref", SqlDbType.VarChar).Value = txtFileRef.Text;
                sqlcmd.Parameters.Add("@pfile_ref_type", SqlDbType.VarChar).Value = cboPReferenceType.Text;
                sqlcmd.ExecuteNonQuery();

                if (txtHDSS_IDLink.Text.Length > 0)
                {
                    String l_firstname = txtFirstnameLink.Text.ToUpper().Trim();
                    String l_middlename = txtMiddlenameLink.Text.ToUpper().Trim();
                    String l_lastname = txtLastnameLink.Text.ToUpper().Trim();

                    MessageBox.Show(" Client " + l_firstname + " " + l_middlename + " " + l_lastname + " " +
                    "flagged as a Non HDSS Member", "SETS: Client matching", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);



                    //MessageBox.Show("Match assigned", "Match information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }//RecordSaved = true;
            }

            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "File and HDSS match Record Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }


        
        }

            private void chEdit_Click(object sender, EventArgs e)
            {
                chSave.Enabled = true;
            }

            private void btnSaveAppointment_Click(object sender, EventArgs e)
            {

                if (con != null && con.State == ConnectionState.Open)
                {
                    // do something
                    con.Close();
                }

                
                
                SqlCommand cmd = new SqlCommand(@"insert into visit.checkout(system_id,next_app_date) 
            values('" + System_ID + "','" + dpNextAppointmentDate.Value.Date + "')", con);




                con.Open();
                cmd.ExecuteNonQuery();
                

                btnSaveAppointment.Enabled = false;

                //ClearAllTabs();

                EndCheckin te = new EndCheckin();
                //t.Visible = true;

                te.StartPosition = FormStartPosition.CenterParent;
                te.ShowDialog();



                con.Close();
            }

            private void UpdateBanner()

            {

                lblBanner.Text = "File ref:  " + pfile_ref + "(" + pfile_ref_type + ")  Name: " + c_firstname + " " + c_middlename + " " + c_lastname + " ";
                if (lblBanner.Visible == false)
                {
                    lblBanner.Visible = true;

                }            
            }

            private void lnkLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                lblBanner.Text = "File ref:  " + pfile_ref + "(" + pfile_ref_type + ")  Name: " + c_firstname + " " + c_middlename + " " + c_lastname + " ";
                if (lblBanner.Visible == false)
                {
                    lblBanner.Visible = true;

                }
                tpHDSSLinkage.Enabled = true;
                tabControl1.SelectTab(tpHDSSLinkage);
            }

            private void btnSaveTestType_Click(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand(@"insert into diagnosis.test_types(test_name,test_description) 
            values('" + txtAddedTestType.Text + "','" + txtTestDescription.Text + "')", con);

                //closing an old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show(" " +
    "The New test type successfully saved", "SETS: Client HIV Test Details", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                con.Close();
            }

            private void btnSaveDepartment_Click(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand(@"insert into registry.facility_departments(department_name) 
            values('" + txtDepartment.Text + "')", con);


                //closing an old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                //now opening a new connection
                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show(" " +
    "The new department successfully saved", "SETS: Client HIV Test Details", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                con.Close();
            }

            private void btnSaveMStatus_Click(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand(@"insert into reference.marital_status(name) 
            values('" + txtAddedMStatus.Text + "')", con);


                //close an old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show(" " +
    "The new marital status successfully saved", "SETS: Client HIV Test Details", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                con.Close();

            }

            private void btnUserSave_Click(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand(@"insert into registry.tbl_user(user_name,user_password,user_role) 
            values('" + txtUser.Text + "','" + txtPassword.Text + "','" + cboUserRole.Text + "')", con);

                //closing an old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                //now opening a new connection

            con.Open();
            cmd.ExecuteNonQuery();
            MessageBox.Show(" " +
"New user successfully saved", "SETS: Client HIV Test Details", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);


            con.Close();
        }




            private void MainForm_Load(object sender, EventArgs e)
            {

                               
                Retrieve_DSS_Villages();
                Retrieve_Facility_Departments();
                Retrieve_FileRef_Types();
                Populate_TestTypes();

                Retrieve_Marital_Status();
                PopulateFacilities();



                cboAlternativeSearchParameter.SelectedIndex = 0;
                cboGender.SelectedIndex = 0;


                //txtSubLocation.Enabled = false;

                //gbMatchStatus.Visible = false;

                //On the menu header
                lblBanner.Visible = false;



                //tabControl1.TabPages.Remove(tpClientHIVTest);
                // tabControl1.TabPages.Remove(tpHIVStatus);
                
                //Disable tabs that would require clients to be searched first

                tpHDSSLinkage.Enabled = false;
                
                tpHIVStatus.Enabled = false;

                //On the registration tab

                //pnlMatchNotFound.Visible = false;

                pnlSearchPane.Tag = pnlSearchPane.Bounds;

                ReloadSearchWindow();
                rbFileRef.Checked = true;

                lnkEnterHIVStatus.Visible = false;

                //these controls apply to 
            }

            private void cboTestTypeGB1_SelectedIndexChanged(object sender, EventArgs e)
            {

            }

            private void label58_Click(object sender, EventArgs e)
            {

            }

            private void btn2Save_Click_1(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand("update_create_testresult", con);
                cmd.CommandType = CommandType.StoredProcedure;




                cmd.Parameters.AddWithValue("@system_id", System_ID);
                cmd.Parameters.AddWithValue("test_type", cboTestTypeGB2.Text);
                cmd.Parameters.AddWithValue("test_result", cboTestResultGB2.Text);
                cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB2.Value.Date);
                cmd.Parameters.AddWithValue("@tester", cboTester2.Text);
                cmd.Parameters.AddWithValue("@lot_no", txtLotNo2.Text);


                try
                {
                   
                    //closing an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }


                    //now opening a new connection
                    
                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    //Clear();
                    if (k != 0)
                    {

                        MessageBox.Show(" " +
    "HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);



                        btn2Save.Enabled = false;


                        //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                        //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                    }


                }

                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                finally
                { con.Close(); }

                UpdateResultLabel();

            }

            private void btn2AnotherTest_Click_1(object sender, EventArgs e)
            {
                if (btn2Save.Enabled)
                {
                    SaveHIVTest2Details();
                }
                GB3.Visible = true;
                if (btn3Save.Enabled == false) { btn3Save.Enabled = true; }


            }

            private void btn3Save_Click_1(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand("update_create_testresult", con);
                cmd.CommandType = CommandType.StoredProcedure;




                cmd.Parameters.AddWithValue("system_id", System_ID);
                cmd.Parameters.AddWithValue("test_type", cboTestTypeGB3.Text);
                cmd.Parameters.AddWithValue("test_result", cboTestResultGB3.Text);
                cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB3.Value.Date);
                cmd.Parameters.AddWithValue("@tester", cboTester3.Text);
                cmd.Parameters.AddWithValue("@lot_no", txtLotNo3.Text);



                try
                {

                    //closing an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }


                    //now opening a new connection

                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    //Clear();
                    if (k != 0)
                    {

                        MessageBox.Show(" " +
    "HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                        btn3Save.Enabled = false;


                        //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                        //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                    }


                }

                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                finally
                { con.Close(); }

                UpdateResultLabel();


            }

            private void btn3AnotherTest_Click_1(object sender, EventArgs e)
            {
                if (btn3Save.Enabled)
                {
                    SaveHIVTest3Details();
                }
                GB4.Visible = true;

                if (btn4Save.Enabled == false) { btn4Save.Enabled = true; }

            }

            private void btn4Save_Click_1(object sender, EventArgs e)
            {
                SqlCommand cmd = new SqlCommand("update_create_testresult", con);
                cmd.CommandType = CommandType.StoredProcedure;




                cmd.Parameters.AddWithValue("@system_id", System_ID);
                cmd.Parameters.AddWithValue("test_type", cboTestTypeGB4.Text);
                cmd.Parameters.AddWithValue("test_result", cboTestResultGB4.Text);
                cmd.Parameters.AddWithValue("hiv_test_date", dpHIVTestDateGB4.Value.Date);
                cmd.Parameters.AddWithValue("@tester", cboTester4.Text);
                cmd.Parameters.AddWithValue("@lot_no", txtLotNo4.Text);


                try
                {

                    //closing an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }

                    //now opening a new connection

                    con.Open();
                    int k = cmd.ExecuteNonQuery();
                    //Clear();
                    if (k != 0)
                    {

                        MessageBox.Show(" " +
    "HIV test result successfully saved", "SETS: Client HIV Test Results", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);



                        btn4Save.Enabled = false;


                        //hiv_status=cboTestResultGB1.SelectedItem.ToString();
                        //lblTestResult.Text = "HIV Status so far: " + hiv_status;




                    }


                }

                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                finally
                { con.Close(); }

                UpdateResultLabel();

            }

            private void btnClientNotinList_Click(object sender, EventArgs e)
            {
                pnlNoDSSMatch.Visible = true;

                //check if the connection is still open then close it

                

                if (con != null && con.State == ConnectionState.Open)
                {
                    // do something
                    con.Close();
                }
                 
                

            }

            private void btn5Save_Click_1(object sender, EventArgs e)
            {

            }

            private void btn4AnotherTest_Click_1(object sender, EventArgs e)
            {
                GB5.Visible = true;

                if (btn5Save.Enabled == false) { btn5Save.Enabled = true; }

            }

            private void chBlockVillage_CheckedChanged(object sender, EventArgs e)
            {
                if (chBlockVillage.Checked == true)

                {

                    if (cbVillage.Checked == false)


                    {

                        MessageBox.Show(" " +
    "You need to check use by village before blocking by village", "SETS: Client Linkage", MessageBoxButtons.OKCancel,
    MessageBoxIcon.Information);

                        return; 

                    }


                    if (string.IsNullOrWhiteSpace(txtVillageLink.Text))
                    {
                        MessageBox.Show("You cannot block by a village if you never supplied a village name");
                        return;
                    }




                    if (chBlockNone.Checked == true)
                    {
                        chBlockNone.Checked = false;
                    }


                    if (chBlockGender.Checked == true)
                    {
                        chBlockGender.Checked = false;
                    }


                    if (chBlockDob.Checked == true)
                    {
                        chBlockDob.Checked = false;
                    }

                
                }
            }

            private void txtCHFirstname_TextChanged(object sender, EventArgs e)
            {

            }

            private void lblClientSubLocation_Click(object sender, EventArgs e)
            {

            }

            private void gbMotherDetails_Enter(object sender, EventArgs e)
            {

            }

            private void groupBox1_Enter(object sender, EventArgs e)
            {

            }

            private void label83_Click(object sender, EventArgs e)
            {

            }

            private void chHDSSVillageNotFound_CheckedChanged(object sender, EventArgs e)
            {
                if (chHDSSVillageNotFound.Checked == true)
                    //enable the alternative search groupbox
                {

                    gbAlternativeLocation.Enabled = true; 

                }

                if (chHDSSVillageNotFound.Checked == false)

                {
                    if (gbAlternativeLocation.Enabled == true) { gbAlternativeLocation.Enabled = false; }
                }
            }


            private void txtAltLocSearchValue_KeyUp(object sender, KeyEventArgs e)
            {

                LoadVillageMetaData();

            }


            private void LoadVillageMetaData()
            {
            con.Open();

            AutoCompleteStringCollection namesCollection = new AutoCompleteStringCollection();

            //string querry = @"select distinct village_name from reference.dss_village;";

            string querry = "";

            if (cboAlternativeSearchParameter.Text == "SUBLOCATION")
            {

                querry = @"select distinct sub_location from [reference].[village_metadata] where sub_location IS NOT NULL;";

            }

            else if (cboAlternativeSearchParameter.Text == "LOCATION")


            {

                querry = @"select distinct location from [reference].[village_metadata] where location IS NOT NULL;";
            
            }

            else if (cboAlternativeSearchParameter.Text == "REGION")

            {

                querry = @"select distinct region from [reference].[village_metadata] where region IS NOT NULL;";


            }

            SqlCommand cmd = new SqlCommand(querry, con);

            SqlDataReader dr = cmd.ExecuteReader();


            if (cboAlternativeSearchParameter.Text == "SUBLOCATION")
            {

                if (dr.HasRows == true)
                {

                    while (dr.Read())

                        namesCollection.Add(dr["sub_location"].ToString());

                }


            }




            if (cboAlternativeSearchParameter.Text == "LOCATION")
            {

                if (dr.HasRows == true)
                {

                    while (dr.Read())

                        namesCollection.Add(dr["location"].ToString());

                }


            }






            if (cboAlternativeSearchParameter.Text == "REGION")
            {

                if (dr.HasRows == true)
                {

                    while (dr.Read())

                        namesCollection.Add(dr["region"].ToString());

                }


            }



                
            dr.Close();

            con.Close();



            txtAltLocSearchValue.AutoCompleteMode = AutoCompleteMode.Append;

            txtAltLocSearchValue.AutoCompleteSource = AutoCompleteSource.CustomSource;

            txtAltLocSearchValue.AutoCompleteCustomSource = namesCollection;

        }

            private void chBlockAltLoc_CheckedChanged(object sender, EventArgs e)
            {

               
                
                if (chBlockNone.Checked == true)
                {
                    chBlockNone.Checked = false;
                }

                if (chBlockVillage.Checked == true)
                {
                    chBlockVillage.Checked = false;
                }


                if (chBlockGender.Checked == true)
                {
                    chBlockGender.Checked = false;
                }


                if (chBlockDob.Checked == true)
                {
                    chBlockDob.Checked = false;
                }

            }

            private void btnSeeEntries_Click(object sender, EventArgs e)
            {

                if ((rbRptEntryEverEntered.Checked == false) && (rbRptEntryToday.Checked == false) && (rbRptEntryDateRange.Checked == false))

                {

                    MessageBox.Show(" " +
"You need to choose a filter criteria before proceeding", "SETS: Reports", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);
                    return;

                }

                GenerateReport();

                               
            }

            private void ValidateDateRanges()

            {

                if (dpStartEntryDate.Value.Date == dpEndEntryDate.Value.Date)
                {

                    MessageBox.Show(" " +
"You must choose a date range to search by dates", "SETS: Entry Report", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);
                    dpStartEntryDate.Focus();
                    return;

                }

                else if (dpStartEntryDate.Value.Date > dpEndEntryDate.Value.Date)
                {

                    MessageBox.Show(" " +
"A start date cannot exceed an end date", "SETS: Entry Report", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);
                    dpStartEntryDate.Focus();
                    return;

                }

                else if (dpStartEntryDate.Value.Date > DateTime.Now.Date)
                {

                    MessageBox.Show(" " +
"A start date cannot be set to a future date", "SETS: Entry Report", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);
                    dpStartEntryDate.Focus();
                    return;

                }

                else if (dpEndEntryDate.Value.Date > DateTime.Now.Date)
                {

                    MessageBox.Show(" " +
"An end date cannot be set to a future date", "SETS: Entry Report", MessageBoxButtons.OKCancel,
MessageBoxIcon.Information);
                    dpEndEntryDate.Focus();
                    return;

                }

            
            }

            private void GenerateRegistryReport()

            {

                //close an old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

                // open connection
                con.Open();
                // create command

                SqlCommand sqlcmd = con.CreateCommand();

                if (rbRptEntryEverEntered.Checked == true)
                {
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person";

                }


                if (rbRptEntryToday.Checked == true)
                {

                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person where reg_date= '" + DateTime.Now.Date + "'";

                }


                if (rbRptEntryDateRange.Checked == true)
                {

                    ValidateDateRanges();
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person where reg_date between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'";

                }
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person", con);
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person where date_created between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'", con);
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        txtTotalEntries.Text = sqlrdr.GetValue(0).ToString();
                    }

                }//this is the end of the while black


            }

            private void GenerateLinkReport()

            { 

                //close old connection if still open

                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }

            
                // open connection
                con.Open();
                // create command

                SqlCommand sqlcmd = con.CreateCommand();

                if (rbRptEntryEverEntered.Checked == true)
                {
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person_match where HDSS_ID IS NOT NULL and match_state=1";

                }


                if (rbRptEntryToday.Checked == true)
                {

                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person_match where HDSS_ID IS NOT NULL and match_state=1 and date_matched= '" + DateTime.Now.Date + "'";

                }


                if (rbRptEntryDateRange.Checked == true)
                {

                    ValidateDateRanges();
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.person_match where HDSS_ID IS NOT NULL and match_state=1 and date_matched between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'";

                }
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person", con);
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person where date_created between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'", con);
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        txtTotalEntries.Text = sqlrdr.GetValue(0).ToString();
                    }

                }//this is the end of the while black

            
            }

            private void GenerateHIVStatusReport()

            {


                //close old connection if still open
                if (con != null && con.State == ConnectionState.Open)

                { con.Close(); }


                // open connection
                con.Open();
                // create command

                SqlCommand sqlcmd = con.CreateCommand();

                if (rbRptEntryEverEntered.Checked == true)
                {
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.final_test_result";

                }


                if (rbRptEntryToday.Checked == true)
                {

                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.final_test_result where test_date= '" + DateTime.Now.Date + "'";

                }


                if (rbRptEntryDateRange.Checked == true)
                {

                    ValidateDateRanges();
                    sqlcmd.CommandText = "select count(*) as 'Total Entries' from registry.final_test_result where test_date between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'";

                }
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person", con);
                //SqlCommand sqlcmd = new SqlCommand(@"select count(*) as 'Total Entries' from registry.person where date_created between '" + dpStartEntryDate.Value.Date + "' and '" + dpEndEntryDate.Value.Date + "'", con);
                SqlDataReader sqlrdr = sqlcmd.ExecuteReader();//sqlrdr reads from the select from procedure
                while (sqlrdr.Read())
                {

                    if (sqlrdr.IsDBNull(0) == false)
                    {
                        txtTotalEntries.Text = sqlrdr.GetValue(0).ToString();
                    }

                }//this is the end of the while black

            
            
            
            }

            private void GenerateReport()

            {

                try
                {

                    if (rbRptEntries.Checked == true)
                    {

                        GenerateRegistryReport();
                        lblAdminRptLabel.Text = "Total clients registered: ";

                    }

                    if (rbMatchStatus.Checked == true)
                    {

                        GenerateLinkReport();
                        lblAdminRptLabel.Text = "Total clients matched: ";

                    }


                    if (rbHIVReport.Checked == true)
                    {

                        GenerateHIVStatusReport();
                        lblAdminRptLabel.Text = "Total number of HIV entries: ";

                    }



                }//the try block ends here


                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString(), "Read database tables Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

            
            }

            private void groupBox3_Enter(object sender, EventArgs e)
            {

            }

            private void rbRptEntryDateRange_CheckedChanged(object sender, EventArgs e)
            {
                if (rbRptEntryDateRange.Checked == true) { gbEntryDateRange.Enabled = true; }
                else { gbEntryDateRange.Enabled = false; }

            }

            private void groupBox4_Enter(object sender, EventArgs e)
            {

            }

            private void tpUserReport_Click(object sender, EventArgs e)
            {

            }

            private void btnViewRpt_Click(object sender, EventArgs e)
            {
                //GetReportDetails();

                //SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString);

                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["linkageConnectionString"].ConnectionString))
                {
                   
                    //close an old connection if still open

                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }


                    //open connection
                    con.Open();

                    //get the count of clients entered today

                    using (SqlCommand cmdEnteredToday = new SqlCommand("select count(*) as 'EnteredToday' from registry.person where facility_mflcode='" + cboFacilityName.SelectedValue + "' and reg_date=CAST(GETDATE() as DATE)", con))
                    {

                        SqlDataReader sqlrdr = cmdEnteredToday.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptEnteredToday.Text = sqlrdr.GetValue(0).ToString();
                            }

                        }

                        sqlrdr.Close();

                    }
                    
                    //get the total number of clients ever entered

                    using (SqlCommand cmdEverEntered = new SqlCommand("select count(*) as 'Ever Entered' from registry.person where facility_mflcode='" + cboFacilityName.SelectedValue + "'", con))
                    {

                        SqlDataReader sqlrdr = cmdEverEntered.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptEverEntered.Text = sqlrdr.GetValue(0).ToString();
                            }

                        }

                        sqlrdr.Close();

                    }


                    //Get the total number of clients matched today


                    using (SqlCommand cmdMatchedToday = new SqlCommand("select count(*) as 'MatchedToday' from registry.person_match join registry.person on registry.person_match.system_id=registry.person.system_id where facility_mflcode='" + cboFacilityName.SelectedValue + "' and date_matched=CAST(GETDATE() as DATE) and HDSS_ID IS NOT NULL", con))
                    {

                        SqlDataReader sqlrdr = cmdMatchedToday.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptMatchedToday.Text = sqlrdr.GetValue(0).ToString();
                            }

                        }

                        sqlrdr.Close();

                    }


                    //get the total number of clients ever matched

                    using (SqlCommand cmdEverMatched = new SqlCommand("select count(*) as 'EverMatched' from registry.person_match join registry.person on registry.person_match.system_id=registry.person.system_id where facility_mflcode='" + cboFacilityName.SelectedValue + "' and HDSS_ID IS NOT NULL", con))
                    {

                        SqlDataReader sqlrdr = cmdEverMatched.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptEverMatched.Text = sqlrdr.GetValue(0).ToString();
                            }

                        }

                        sqlrdr.Close();

                    }

                    //Get the HIV status today


                    using (SqlCommand cmdHIVStatusToday = new SqlCommand("select COUNT(CASE WHEN [registry].[final_test_result].[test_result] = " +
                        "'POSITIVE' THEN [registry].[final_test_result].[test_result] END) AS 'Positive', " +
                          "COUNT(CASE WHEN [registry].[final_test_result].[test_result] = 'NEGATIVE' THEN [registry].[final_test_result].[test_result] END) AS 'Negative', " +
                            "COUNT(1) as 'TotalCount' FROM [registry].[final_test_result] join registry.person on registry.final_test_result.system_id=registry.person.system_id where facility_mflcode='" + cboFacilityName.SelectedValue + "' and created_at=CAST(GETDATE() as DATE)", con))
                    {

                        SqlDataReader sqlrdr = cmdHIVStatusToday.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptPositiveStatusToday.Text = sqlrdr.GetValue(0).ToString();
                            }

                            if (sqlrdr.IsDBNull(1) == false)
                            {
                                txtUserRptNegativetiveStatusToday.Text = sqlrdr.GetValue(1).ToString();
                            }

                            if (sqlrdr.IsDBNull(2) == false)
                            {
                                txtUserRptTotalHIVStatusToday.Text = sqlrdr.GetValue(2).ToString();
                            }


                        }

                        sqlrdr.Close();

                    }


                    //get the count of the HIV status ever known

                    

                    using (SqlCommand cmdHIVStatus = new SqlCommand("select COUNT(CASE WHEN [registry].[final_test_result].[test_result] = " +
                        "'POSITIVE' THEN [registry].[final_test_result].[test_result] END) AS 'Positive', " +
                          "COUNT(CASE WHEN [registry].[final_test_result].[test_result] = 'NEGATIVE' THEN [registry].[final_test_result].[test_result] END) AS 'Negative', " +
                            "COUNT(1) as 'TotalCount' FROM [registry].[final_test_result] join registry.person on registry.final_test_result.system_id=registry.person.system_id where facility_mflcode='" + cboFacilityName.SelectedValue + "'", con))
                    {

                        SqlDataReader sqlrdr = cmdHIVStatus.ExecuteReader();//sqlrdr reads from the select from procedure
                        while (sqlrdr.Read())
                        {

                            if (sqlrdr.IsDBNull(0) == false)
                            {
                                txtUserRptTotalHIVStatus.Text = sqlrdr.GetValue(0).ToString();
                            }

                            
                        }

                        sqlrdr.Close();

                    }


                    //calculate the unmached clients

                    int unmatched_clients = int.Parse(txtUserRptEverEntered.Text) - int.Parse(txtUserRptEverMatched.Text);
                    txtUserRptTotalUnMatched.Text = unmatched_clients.ToString();

                    
                    //calculate the number of HIV entries not yet done


                    int unentered_HIV_Status = int.Parse(txtUserRptEverEntered.Text) - int.Parse(txtUserRptTotalHIVStatus.Text);
                    txtUserRptTotalUnenteredHIVStatus.Text = unentered_HIV_Status.ToString();

                    //now close the 
                    con.Close();

                    // etc
                }



                PopulateReportGrid();

                

            }

            private void PopulateReportGrid()

            { 
            //con.Open();
            SqlCommand sqlcmd = new SqlCommand("Fill_Report_Grid", con);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@facility_mflcode", cboFacilityName.SelectedValue);
            sqlcmd.Parameters.AddWithValue("@facility_department", cboFacilityDepartment.Text);
            

            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);





            DataTable dt = new DataTable();

            sda.Fill(dt);
            dgvReportGrid.Rows.Clear();


            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgvReportGrid.Rows.Add();
                    dgvReportGrid.Rows[n].Cells[0].Value = item["pfile_ref_type"].ToString();
                    dgvReportGrid.Rows[n].Cells[1].Value = item["pfile_ref"].ToString();
                    dgvReportGrid.Rows[n].Cells[2].Value = item["firstname"].ToString();
                    dgvReportGrid.Rows[n].Cells[3].Value = item["middlename"].ToString();
                    dgvReportGrid.Rows[n].Cells[4].Value = item["lastname"].ToString();
                    dgvReportGrid.Rows[n].Cells[5].Value = item["dob"].ToString();
                    dgvReportGrid.Rows[n].Cells[6].Value = item["Match_Status"].ToString();
                    dgvReportGrid.Rows[n].Cells[7].Value = item["HIV_Status"].ToString();
                    dgvReportGrid.Rows[n].Cells[8].Value = item["next_app_date"].ToString();
                    dgvReportGrid.Rows[n].Cells[9].Value = item["system_id"].ToString();
                    

                }


            }

                

            
            }

            private void groupBox14_Enter(object sender, EventArgs e)
            {

            }

            private void label94_Click(object sender, EventArgs e)
            {

            }

            private void txtUserRptTotalHIVStatus_TextChanged(object sender, EventArgs e)
            {

            }

            private void label95_Click(object sender, EventArgs e)
            {

            }

            private void txtUserRptTotalUnenteredHIVStatus_TextChanged(object sender, EventArgs e)
            {

            }

            private void rbRptEntryToday_CheckedChanged(object sender, EventArgs e)
            {

            }

            

            

            private void dgvReportGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
            {


                pfile_ref_type = dgvReportGrid.SelectedRows[0].Cells[0].Value.ToString();
                pfile_ref = dgvReportGrid.SelectedRows[0].Cells[1].Value.ToString();
                c_firstname = dgvReportGrid.SelectedRows[0].Cells[2].Value.ToString();
                c_middlename = dgvReportGrid.SelectedRows[0].Cells[3].Value.ToString();
                c_lastname = dgvReportGrid.SelectedRows[0].Cells[4].Value.ToString();
                c_matchstatus = dgvReportGrid.SelectedRows[0].Cells[6].Value.ToString();
                hiv_status = dgvReportGrid.SelectedRows[0].Cells[7].Value.ToString();
                System_ID = dgvReportGrid.SelectedRows[0].Cells[9].Value.ToString();

                GetPersonDetails();
                UpdateBanner();

                if (c_matchstatus == "UNMATCHED")
                {
                    rbAttemptLink.Enabled = true;
                }

                else

                {
                    rbAttemptLink.Enabled = false;
                
                }

                if (hiv_status == "NOT ENTERED" || hiv_status == "NEGATIVE")
                {
                    rbEditAddHIVInfo.Enabled = true;
                }

                else

                {

                    rbEditAddHIVInfo.Enabled = false;

                }

                if (PID_missing == 1)

                { rbUpdateIdentifiers.Enabled = true; }

                else

                { rbUpdateIdentifiers.Enabled = false; }





            }

            private void CheckEmptyPIDFields()

            {

                if (txtFileRef.Text == string.Empty ||
                txtFirstname.Text == string.Empty ||
                txtMiddlename.Text == string.Empty ||
                txtLastname.Text == string.Empty ||
                txtMFirstname.Text == string.Empty ||
                txtMMiddlename.Text == string.Empty ||
                txtMLastname.Text == string.Empty ||
                txtFFirstname.Text == string.Empty ||
                txtFMiddlename.Text == string.Empty ||
                txtFLastname.Text == string.Empty ||
                txtCHFirstname.Text == string.Empty ||
                txtCHMiddlename.Text == string.Empty ||
                txtCHLastname.Text == string.Empty ||
                txtVillageName.Text == string.Empty ||
                txtAltLocSearchValue.Text == string.Empty
                )


                    PID_missing = 1;



            }

            private void rbAttemptLink_CheckedChanged(object sender, EventArgs e)
            {

                if (rbAttemptLink.Checked == true)
                {
                    tpHDSSLinkage.Enabled = true;
                    tabControl1.SelectTab(tpHDSSLinkage);

                }
            }

            private void rbEditAddHIVInfo_CheckedChanged(object sender, EventArgs e)
            {

                if (rbEditAddHIVInfo.Checked == true)
                {
                    tpHIVStatus.Enabled = true;
                    tabControl1.SelectTab(tpHIVStatus);

                }
            }

            private void rbUpdateIdentifiers_CheckedChanged(object sender, EventArgs e)
            {
                if (rbUpdateIdentifiers.Checked == true)
                {
                    tbRegisterClient.Enabled = true;
                    tabControl1.SelectTab(tbRegisterClient);

                }
            }

            private void btnSaveRemark_Click(object sender, EventArgs e)
            {
//                SqlCommand cmd = new SqlCommand(@"insert into registry.person_remarks(system_id,remarks) 
//            values('" + System_ID + "','" + txtPatientRemarks.Text + "')", con);

                
                    //SqlCommand cmd = new SqlCommand(@"update registry.person SET remarks='" + txtRemarks.Text + "'where system_id='" + System_ID + "'", con);

                
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = con;

                    if (System_ID.Length > 0)
                    {
                        cmd.CommandText = "update registry.person SET remarks='" + txtRemarks.Text + "'where system_id='" + System_ID + "'";

                    }

                    else if (active_system_ID.Length > 0)
                    {

                        cmd.CommandText = "update registry.person SET remarks='" + txtRemarks.Text + "'where system_id='" + active_system_ID + "'";

                    }



                //close old connection if still open
                    if (con != null && con.State == ConnectionState.Open)

                    { con.Close(); }

                //attempting to open a new connection

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(" " +
        "Client Remarks successfully saved", "SETS: Remarks Pane", MessageBoxButtons.OKCancel,
        MessageBoxIcon.Information);


                    con.Close();

                

                
                   
                  





            }

            private void btnResultEdit_Click(object sender, EventArgs e)
            {
                btnResultEdit.Enabled = false;

                if (btnResultEdit.Enabled == false)
                {
                    is_hiventry_new = 0;

                    if (btnResultEdit.Enabled == false) { btnResultSave.Enabled = true; }


                }

            }

            private void btnPushData_CheckedChanged(object sender, EventArgs e)
            {
                if (btnPushData.Checked == true)
                {

                    synctype = "push";
                    Synchronize sn = new Synchronize(synctype);
                    sn.ShowDialog();

                }
            }

            private void btnPullData_CheckedChanged(object sender, EventArgs e)
            {

                if (btnPullData.Checked == true)
                {

                    synctype = "pull";
                    Synchronize sn = new Synchronize(synctype);
                    sn.ShowDialog();

                }
            }

            private void label43_Click(object sender, EventArgs e)
            {

            }

            private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
            {
                UserLogin ul = new UserLogin();
                this.Close();
                ul.Show();


            }

            private void btnRemarkstoHome_Click(object sender, EventArgs e)
            {
                tabControl1.SelectTab(tpSearchClient);
                ReloadSearchWindow();
            }

            

           




    }
}
