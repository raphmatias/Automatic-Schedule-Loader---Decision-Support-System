using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
namespace AutomaticScheduleLoader
{
    public partial class frmFaculty : Form
    {
       
            [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
            private static extern IntPtr CreateRoundRectRgn
         (
             int nLeftRect,     // x-coordinate of upper-left corner
             int nTopRect,      // y-coordinate of upper-left corner
             int nRightRect,    // x-coordinate of lower-right corner
             int nBottomRect,   // y-coordinate of lower-right corner
             int nWidthEllipse, // width of ellipse
             int nHeightEllipse // height of ellipse
         );
        string checker = "";
        string loginAct = "";
        string typeofAcc = "";
        string facultyDuplicate = "";
        string facultyDuplicateUpdate = "";
        bool check = false;
        List<string> idSched = new List<string>();
        List<string> ID = new List<string>();

        List<string> IDArchive = new List<string>();
        List<string> SectionArchive = new List<string>();
        List<string> SubjectArchive = new List<string>();
        List<string> CourseArchive = new List<string>();
        List<string> SubjSlotArchive = new List<string>();
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
        public frmFaculty()
        {
                InitializeComponent();
              //  this.FormBorderStyle = FormBorderStyle.None;

               // Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            }
        void SchedulePlotted()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "select count(FacultyCode) as duplicate From Faculty_Tbl Where FacultyCode = '" + txtFCode.Text + "'";
                    SqlCommand command = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        facultyDuplicate = reader["duplicate"].ToString();
                    }
                    reader.Close();

                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void scheduleplottedUPDATE()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query1 = "select count(FacultyCode) as duplicate From Faculty_Tbl Where FacultyCode = '" + txtFCode.Text + "' AND ID != '" + Convert.ToInt32(dgvFaculty.CurrentRow.Cells["ID"].Value.ToString()) + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {
                        facultyDuplicateUpdate = reader1["duplicate"].ToString();
                    }
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            public void SPUpdateOnArchive()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT ID,Section,SubjectCode,Course FROM Specialization_Tbl Where   FacultyCode=@FacultyCode AND SubjectCode != '"+"Consultation Hours"+"' AND SubjectCode != '"+"Research And Extension"+"'";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    cmd.Parameters.AddWithValue("@FacultyCode", dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            IDArchive.Add(reader.GetInt32(0).ToString());
                            //    course.Add(reader.GetString(1));
                            SectionArchive.Add(reader.GetString(1));
                            SubjectArchive.Add(reader.GetString(2));
                            CourseArchive.Add(reader.GetString(3));
                        }
                    }

                    for (int i = 0;i< IDArchive.Count;i++)
                    {
                        string query0 = "SELECT SubjectSlot FROM Subject_Tbl Where   SubjectCode=@SubjectCode AND Course=@Course";
                        SqlCommand cmd0 = new SqlCommand(query0, sqlcon);
                        cmd0.Parameters.AddWithValue("@SubjectCode", SubjectArchive[i]);
                        cmd0.Parameters.AddWithValue("@Course", CourseArchive[i]);
                        using (SqlDataReader reader0 = cmd0.ExecuteReader())
                        {
                            while (reader0.Read())
                            {

                                SubjSlotArchive.Add(reader0.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PopulateGridViewFaculty() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvFaculty.DataSource = dt;
                    dgvFaculty.EnableHeadersVisualStyles = false;
                    this.dgvFaculty.Columns["FacultyCode"].Width = 150;
                    this.dgvFaculty.Columns["FacultyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvFaculty.Columns["FullTime"].Width = 170;
                    this.dgvFaculty.Columns["ID"].Visible = false;
                    this.dgvFaculty.Columns["EducAttain"].Visible = false;
                    dgvFaculty.AllowUserToAddRows = false;
                    dgvFaculty.Columns["FacultyCode"].HeaderText = "Faculty Code";
                    dgvFaculty.Columns["FacultyName"].HeaderText = "Faculty Name";
                    dgvFaculty.Columns["FullTime"].HeaderText = "Job Type";
                    dgvFaculty.Columns["FacultyCode"].ReadOnly = true;
                    dgvFaculty.Columns["FacultyName"].ReadOnly = true;
                    dgvFaculty.Columns["FullTime"].ReadOnly = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AdminActivity()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT Username FROM LoginActivity_Tbl WHERE UserActivityID=(SELECT max (UserActivityID) FROM LoginActivity_Tbl)";
                    SqlCommand command = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read() == true)
                    {



                        loginAct = reader["Username"].ToString();

                    }
                    reader.Close();

                    string query1 = "SELECT UserType FROM User_Tbl WHERE Username='" + loginAct + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {



                        typeofAcc = reader1["UserType"].ToString();

                    }
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewFacultySearchFCode() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FacultyCode like '%" + txtSearch.Text + "%'", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvFaculty.DataSource = dt;
                    dgvFaculty.EnableHeadersVisualStyles = false;
                    this.dgvFaculty.Columns["FacultyCode"].Width = 150;
                    this.dgvFaculty.Columns["FacultyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvFaculty.Columns["FullTime"].Width = 170;
                    this.dgvFaculty.Columns["ID"].Visible = false;
                    this.dgvFaculty.Columns["EducAttain"].Visible = false;
                    dgvFaculty.AllowUserToAddRows = false;
                    dgvFaculty.Columns["FacultyCode"].HeaderText = "Faculty Code";
                    dgvFaculty.Columns["FacultyName"].HeaderText = "Faculty Name";
                    dgvFaculty.Columns["FullTime"].HeaderText = "Job Type";
                    dgvFaculty.Columns["FacultyCode"].ReadOnly = true;
                    dgvFaculty.Columns["FacultyName"].ReadOnly = true;
                    dgvFaculty.Columns["FullTime"].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       
        }
      

        public void specializationUpdate()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query = "SELECT ID FROM Specialization_Tbl Where FacultyCode ='" + dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ID.Add(reader.GetInt32(0).ToString());



                    }
                }

                string queryy = "SELECT ID FROM FacultySchedule_Tbl Where FacultyCode ='" + dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "'";
                SqlCommand cmdd = new SqlCommand(queryy, sqlcon);
                using (SqlDataReader readerr = cmdd.ExecuteReader())
                {
                    while (readerr.Read())
                    {
                        idSched.Add(readerr.GetInt32(0).ToString());


                    }
                }
            }
        }
        
        public void UserCheck()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string querycont = "SELECT COUNT(FacultyCode) AS FacultycodeDuplicate FROM Faculty_Archive WHERE FacultyCode=@FacultyCode";
                    SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                    commandcont.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                    SqlDataReader readercont = commandcont.ExecuteReader();

                    if (readercont.Read() == true)
                    {


                        checker = readercont["FacultycodeDuplicate"].ToString();


                    }
                    readercont.Close();

                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewFacultySearchFName() // filter gridview faculty by name
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FacultyName like  '%" + txtSearch.Text + "%'", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvFaculty.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void frmFaculty_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGridViewFaculty();
              
                if(dgvFaculty.Rows.Count != 0)
                {
                    clear();
                  
                }
                btnUpdate.Enabled = false;
                AdminActivity();
                if (typeofAcc == "1")
                {
                    btnUpdate.Enabled = false;
                    btnArchived.Visible = false;
                }
                else
                {
                    btnArchived.Visible = true;
                }
                btnUpdate.BackColor = Color.Gray;
                btnArchive.BackColor = Color.Gray;
                //    this.AcceptButton = btnSearch;
                //  panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
                //   panel1.Height, 20, 20));
                btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
                btnSave.Height, 30, 30));
                btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width,
                btnSearch.Height, 30, 30));
                btnArchive.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchive.Width,
              btnArchive.Height, 30, 30));
                btnClear.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClear.Width,
             btnClear.Height, 30, 30));
                btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width,
             btnUpdate.Height, 30, 30));
                btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
           btnClose.Height, 30, 30));
                btnArchived.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchived.Width,
           btnArchived.Height, 30, 30));
                /*
                txtEducAttain.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtEducAttain.Width,
        txtEducAttain.Height, 15, 15));
                txtFCode.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtFCode.Width,
     txtFCode.Height, 15, 15));
                txtFname.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtFname.Width,
     txtFname.Height, 15, 15));
                txtSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSearch.Width,
    txtSearch.Height, 15, 15));
                comboBox1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, comboBox1.Width,
    comboBox1.Height, 15, 15));
                */
                dgvFaculty.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dgvFaculty.Width,
    dgvFaculty.Height, 5, 5));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void Checker()
        {
            if (txtFCode.Text.Equals(""))
            {
                label9.Visible = true;
                label9.ForeColor = Color.Red;
                label1.ForeColor = Color.Red;
                check = true;
            }

            if (txtFname.Text.Equals("") )
            {
                label10.Visible = true;
                label10.ForeColor = Color.Red;
                label2.ForeColor = Color.Red;
                check = true;
            }
            if (txtEducAttain.Text.Equals(""))
            {
                label11.Visible = true;
                label11.ForeColor = Color.Red;
                label3.ForeColor = Color.Red;
                check = true;
            }
            if (rbFullTime.Checked == false && rbPartTime.Checked == false)
            {
                label12.Visible = true;
                label12.ForeColor = Color.Red;
                label8.ForeColor = Color.Red;
                check = true;
            }

            if (txtFname.Text.Length != 0 && txtFCode.Text.Length != 0 && txtEducAttain.Text.Length != 0 && (rbFullTime.Checked == true || rbPartTime.Checked == true))
            {
                check = false;
                label9.Visible = false;
                label9.ForeColor = Color.Red;
                label1.ForeColor = Color.Gray;

                label10.Visible = false;
                label10.ForeColor = Color.Red;
                label2.ForeColor = Color.Gray;

                label11.Visible = false;
                label11.ForeColor = Color.Red;
                label3.ForeColor = Color.Gray;

                label12.Visible = false;
                label12.ForeColor = Color.Red;
                label8.ForeColor = Color.Gray;
             
            }


        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Do you really want to save this data?", "Save Data", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        Checker();
                        if (check == false)
                        {
                            SchedulePlotted();
                            if (Convert.ToInt32(facultyDuplicate) >= 1)
                            {
                                  lblresult.Text = "Faculty Code Existing";
                                  lblresult.ForeColor = Color.Red;
                                  lblresult.Visible = true;
                            }
                            else 
                                {

                             
                                sqlcon.Open();
                                SqlCommand cmd = new SqlCommand("INSERT INTO Faculty_Tbl (FacultyCode,FacultyName,EducAttain,FullTime) VALUES (@FacultyCode,@FacultyName,@EducAttain,@FullTime)", sqlcon);
                                cmd.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                cmd.Parameters.AddWithValue("@FacultyName", txtFname.Text);
                                cmd.Parameters.AddWithValue("@EducAttain", txtEducAttain.Text);
                                if (rbFullTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "1");
                                }
                                else if (rbPartTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "0");
                                }
                                cmd.ExecuteNonQuery();
                                clear();
                                lblresult.Text = "Succesfully Saved";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                PopulateGridViewFaculty();

                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " added a faculty");
                                cm.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }catch(Exception)
            {
              //  lblresult.Text = "Faculty Code Existing";
              //  lblresult.ForeColor = Color.Red;
             //   lblresult.Visible = true;
            }
            }
        private void dgvFaculty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
         //   try
        //    {
                
                lblresult.Visible = false;
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
                btnArchive.Enabled = true;
            btnUpdate.BackColor = Color.MediumSeaGreen;
            btnArchive.BackColor = Color.MediumSeaGreen;
            btnSave.BackColor = Color.Gray;
            txtFCode.Text = dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString();
                txtFname.Text = dgvFaculty.CurrentRow.Cells["FacultyName"].Value.ToString();
                txtEducAttain.Text = dgvFaculty.CurrentRow.Cells["EducAttain"].Value.ToString();
                if (dgvFaculty.CurrentRow.Cells["FullTime"].Value.ToString().Equals("Full Time"))
                {
                    rbFullTime.Checked = true;
                }
                else if (dgvFaculty.CurrentRow.Cells["FullTime"].Value.ToString().Equals("Part Time"))
                    rbPartTime.Checked = true;
               
                Checker();
      /*      }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
      */
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
          
         
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to update this faculty?", "Update Faculty", MessageBoxButtons.YesNo,  MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        Checker();

                        if (check == false)
                        {
                            scheduleplottedUPDATE();
                            if (Convert.ToInt32(facultyDuplicateUpdate) >= 1)
                            {
                                lblresult.Text = "Faculty Code Existing";
                                lblresult.ForeColor = Color.Red;
                                lblresult.Visible = true;
                            }
                            else
                            {
                                sqlcon.Open();
                                specializationUpdate();
                                for (int i = 0;i < ID.Count; i++)
                                {
                                    SqlCommand cmdd = new SqlCommand("UPDATE Specialization_Tbl SET FacultyCode=@FacultyCode WHERE ID=@ID", sqlcon);
                                    cmdd.Parameters.AddWithValue("@ID", Convert.ToInt32(ID[i]));
                                    cmdd.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                    cmdd.ExecuteNonQuery();
                                    
                                }
                                ID.Clear();
                                for (int i = 0; i < idSched.Count; i++)
                                {
                                    SqlCommand cmddd = new SqlCommand("UPDATE FacultySchedule_Tbl SET FacultyCode=@FacultyCode WHERE ID=@ID", sqlcon);
                                    cmddd.Parameters.AddWithValue("@ID", Convert.ToInt32(idSched[i]));
                                    cmddd.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                    cmddd.ExecuteNonQuery();
                                    
                                }
                                idSched.Clear();


                                SqlCommand cmd = new SqlCommand("UPDATE Faculty_Tbl SET FacultyCode=@FacultyCode,FacultyName=@FacultyName,EducAttain=@EducAttain,FullTime=@FullTime WHERE ID=@ID", sqlcon);
                                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dgvFaculty.CurrentRow.Cells["ID"].Value.ToString()));
                                cmd.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                cmd.Parameters.AddWithValue("@FacultyName", txtFname.Text);
                                cmd.Parameters.AddWithValue("@EducAttain", txtEducAttain.Text);
                                if (rbFullTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "1");
                                }
                                else if (rbPartTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "0");
                                }
                                cmd.ExecuteNonQuery();
                                clear();
                                lblresult.Text = "Succesfully Updated";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                btnSave.Enabled = true;
                                btnUpdate.Enabled = false;

                                PopulateGridViewFaculty();



                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a faculty");
                                cm.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblresult.Text = "Faculty Code Existing";
                lblresult.ForeColor = Color.Red;
                lblresult.Visible = true;
         
            }
         
        }
        void clear()
        {
            txtFCode.Text = txtFname.Text = comboBox1.Text = txtSearch.Text =txtEducAttain.Text = "";
            rbFullTime.Checked = rbPartTime.Checked = false;
            comboBox2.SelectedIndex = -1;
            btnUpdate.Enabled = false;
            btnArchive.Enabled = false;
            btnSave.Enabled = true;
            btnUpdate.BackColor = Color.Gray;
            btnArchive.BackColor = Color.Gray;
            btnSave.BackColor = Color.MediumSeaGreen;
            PopulateGridViewFaculty();
            if(dgvFaculty.Rows.Count != 0)
            {
                dgvFaculty.Rows[0].Selected = false;
            }
           
            comboBox1.SelectedIndex = -1;
            btnUpdate.Enabled = false;
            btnArchive.Enabled = false;
            txtFCode.Enabled = true;
            lblresult.Visible = false;
            label9.Visible = false;
            label9.ForeColor = Color.Red;
            label1.ForeColor = Color.Gray;

            label10.Visible = false;
            label10.ForeColor = Color.Red;
            label2.ForeColor = Color.Gray;

            label11.Visible = false;
            label11.ForeColor = Color.Red;
            label3.ForeColor = Color.Gray;

            label12.Visible = false;
            label12.ForeColor = Color.Red;
            label8.ForeColor = Color.Gray;
        }


        private void btnArchive_Click(object sender, EventArgs e)
        {
   
       
            try
            {
                DialogResult dr = MessageBox.Show("Archive data?", "Archive", MessageBoxButtons.YesNo , MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        UserCheck();
                     
                            bool existing = false;
                            sqlcon.Open();
                            SPUpdateOnArchive();
                            if (IDArchive.Count >= 1)
                            {
                                DialogResult dr1 = MessageBox.Show("Are you sure you really want to archive Selected Faculty? By doing this, it will delete all existing plotted schedule. Are you sure you want to proceed?","Archive Selected Faculty",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                                if (dr1 == DialogResult.Yes)
                                {
                                    for (int i = 0; i < IDArchive.Count; i++)
                                    {
                                        SqlCommand cmd0 = new SqlCommand("UPDATE Subject_Tbl SET Section=@Section,SubjectSlot=@SubjectSlot WHERE SubjectCode=@SubjectCode AND Course=@Course", sqlcon);
                                        cmd0.Parameters.AddWithValue("@SubjectCode", SubjectArchive[i]);
                                        cmd0.Parameters.AddWithValue("@Course", CourseArchive[i]);
                                        cmd0.Parameters.AddWithValue("@Section", SectionArchive[i]);
                                        cmd0.Parameters.AddWithValue("@SubjectSlot", (Convert.ToInt32(SubjSlotArchive[i]) + 1).ToString());
                                    cmd0.ExecuteNonQuery();
                                 
                                    }

                                    SqlCommand cmddel0 = new SqlCommand("DELETE FROM Specialization_Tbl WHERE FacultyCode = '" + txtFCode.Text + "'", sqlcon);
                                    cmddel0.CommandType = CommandType.Text;
                                    cmddel0.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                    cmddel0.ExecuteNonQuery();

                                    SqlCommand cmddel1 = new SqlCommand("DELETE FROM FacultySchedule_Tbl WHERE FacultyCode = '" + txtFCode.Text + "'", sqlcon);
                                    cmddel1.CommandType = CommandType.Text;
                                    cmddel1.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                    cmddel1.ExecuteNonQuery();
                                    existing = true;
                                SubjectArchive.Clear();
                                IDArchive.Clear();
                                SectionArchive.Clear();
                                SubjectArchive.Clear();
                                CourseArchive.Clear();
                                SubjSlotArchive.Clear();
                            }
                            }

                        if (existing == true || (existing == false && IDArchive.Count == 0))
                            {
                                SqlCommand cmd = new SqlCommand("INSERT INTO Faculty_Archive (FacultyCode,FacultyName,EducAttain,FullTime) VALUES (@FacultyCode,@FacultyName,@EducAttain,@FullTime)", sqlcon);
                                cmd.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                cmd.Parameters.AddWithValue("@FacultyName", txtFname.Text);
                                cmd.Parameters.AddWithValue("@EducAttain", txtEducAttain.Text);
                                if (rbFullTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "1");
                                }
                                else if (rbPartTime.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@FullTime", "0");
                                }
                                cmd.ExecuteNonQuery();
                                lblresult.Text = "Archived";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                PopulateGridViewFaculty();
                                SqlCommand cmddel = new SqlCommand("DELETE FROM Faculty_Tbl WHERE FacultyCode = '" + txtFCode.Text + "'", sqlcon);
                                cmddel.CommandType = CommandType.Text;
                                cmddel.Parameters.AddWithValue("@FacultyCode", txtFCode.Text);
                                cmddel.ExecuteNonQuery();
                                clear();
                                PopulateGridViewFaculty();

                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " archive a faculty");
                                cm.ExecuteNonQuery();
                            }
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
      
        }

        private void btnArchived_Click(object sender, EventArgs e)
        {
            FacultyArchived FA = new FacultyArchived(this);
            FA.ShowDialog();
        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Faculty Code")
            {
                PopulateGridViewFacultySearchFCode();
            }
            else if (comboBox1.Text == "Faculty Name")
            {
                PopulateGridViewFacultySearchFName();
            }
            if(dgvFaculty.Rows.Count != 0)
            {
                dgvFaculty.Rows[0].Selected = false;
            }
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {

            clear();
            if (dgvFaculty.Rows.Count != 0)
            {
                dgvFaculty.Rows[0].Selected = false;
            }
            
        }

        private void txtFCode_TextChanged(object sender, EventArgs e)
        {
            txtFCode.SelectionStart = txtFCode.Text.Length;
            txtFCode.Text = txtFCode.Text.ToUpper();
        }

        private void txtFCode_MouseClick(object sender, MouseEventArgs e)
        {
            label9.Visible = false;
            label1.ForeColor = Color.Gray;
        }

        private void txtFCode_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void txtFname_MouseClick(object sender, MouseEventArgs e)
        {
            label10.Visible = false;
            label2.ForeColor = Color.Gray;
        }

        private void txtFname_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void txtEducAttain_MouseClick(object sender, MouseEventArgs e)
        {
            label11.Visible = false;
            label3.ForeColor = Color.Gray;
        }

        private void txtEducAttain_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void rbFullTime_CheckedChanged(object sender, EventArgs e)
        {
            label12.Visible = false;
            label8.ForeColor = Color.Gray;
        }

        private void rbPartTime_CheckedChanged(object sender, EventArgs e)
        {
            label12.Visible = false;
            label8.ForeColor = Color.Gray;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtFname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFname.Text.Length <= 0) return;
                string s = txtFname.Text.Substring(0, 1);
                if (s != s.ToUpper())
                {
                    int curSelStart = txtFname.SelectionStart;
                    int curSelLength = txtFname.SelectionLength;
                    txtFname.SelectionStart = 0;
                    txtFname.SelectionLength = 1;
                    txtFname.SelectedText = s.ToUpper();
                    txtFname.SelectionStart = curSelStart;
                    txtFname.SelectionLength = curSelLength;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


    }

}