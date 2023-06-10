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
    public partial class frmSubj : Form
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
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
        string subjSlot = "0";
        string roomcateg = "0";
        string checkerArchive = "0";
        string checker = "0";
        string checkerUpdate = "0";
        string loginAct = "";
        string typeofAcc = "";
        string spSubj = "0";
        bool check = false;
        List<string> idSched = new List<string>();
        List<string> ID = new List<string>();
        List<string> ROOM = new List<string>();
        List<string> section = new List<string>();
        List<string> sectionSched = new List<string>();
        List<string> roomSched = new List<string>();
        List<string> timeID = new List<string>();
        List<string> dayID = new List<string>();
        List<string> semester = new List<string>();

        List<string> SPArchiveSecSlot = new List<string>();
        List<string> SPArchiveCourse = new List<string>();
        string existingSP = "0";
        public frmSubj()
        {
            InitializeComponent();
            //   this.FormBorderStyle = FormBorderStyle.None;
            //   Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        void SchedulePlotted()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "select count(SubjectCode) as numberofSubj From Subject_Tbl Where SubjectCode = '" + txtSCode.Text + "' AND Course='"+cbxCourse.Text+"'";
                    SqlCommand command = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        spSubj = reader["numberofSubj"].ToString();
                    }
                    reader.Close();
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
        public void specializationUpdate()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query = "SELECT ID,Room,Section FROM Specialization_Tbl Where SubjectCode ='" + dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString() + "' AND Course ='"+ dgvSubj.CurrentRow.Cells["Course"].Value.ToString() + "'";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ID.Add(reader.GetInt32(0).ToString());
                        ROOM.Add(reader.GetString(1));
                        section.Add(reader.GetString(2));

                    }
                }

               
              
            }
        }
        public void SPUpdateOnArchive()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query = "SELECT SectionSlot FROM Section_Tbl Where   YearLevel=@YearLevel AND Course = @Course ";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                cmd.Parameters.AddWithValue("@YearLevel", dgvSubj.CurrentRow.Cells["YearLevel"].Value.ToString());
                cmd.Parameters.AddWithValue("@Course", dgvSubj.CurrentRow.Cells["Course"].Value.ToString());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SPArchiveSecSlot.Add(reader.GetString(0));


                    }
                }

                    string query1 = "select count(SubjectCode) as numberofSubj From Specialization_Tbl Where SubjectCode = '" + dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString() + "' AND Course='" + dgvSubj.CurrentRow.Cells["Course"].Value.ToString() + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {
                        existingSP = reader1["numberofSubj"].ToString();
                    }
                    reader1.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void subjectSlot()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query1 = "select Course,YearLevel,SectionSlot FROM Section_Tbl WHERE Course='" + cbxCourse.Text + "' AND YearLevel = @YearLevel";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);

                    if (cbxYear.Text == "")
                    {
                        command1.Parameters.AddWithValue("YearLevel", "");
                    }
                    else if (cbxYear.Text == "First Year")
                    {
                        command1.Parameters.AddWithValue("YearLevel", "1");
                    }
                    else if (cbxYear.Text == "Second Year")
                    {
                        command1.Parameters.AddWithValue("YearLevel", "2");
                    }

                    else if (cbxYear.Text == "Third Year")
                    {
                        command1.Parameters.AddWithValue("YearLevel", "3");
                    }
                    else if (cbxYear.Text == "Fourth Year")
                    {
                        command1.Parameters.AddWithValue("YearLevel", "4");
                    }
                    SqlDataReader reader1 = command1.ExecuteReader();
                    if (reader1.Read() == true)
                    {


                        subjSlot = reader1["SectionSlot"].ToString();


                    }
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void roomCateg()
        {
            try
         {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();

                    string query1 = "select RoomCategory FROM Subject_Tbl WHERE SubjectCode='" + txtSCode.Text + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {



                        roomcateg = reader1["RoomCategory"].ToString();

                    }
                    reader1.Close();
                    string querycont = "SELECT COUNT(SubjectCode) AS SubjectDuplicate FROM SubjectArchive_Tbl WHERE SubjectCode=@SubjectCode AND Course=@Course";
                    SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                    commandcont.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                    commandcont.Parameters.AddWithValue("@Course", cbxCourse.Text);
                    SqlDataReader readercont = commandcont.ExecuteReader();

                    if (readercont.Read() == true)
                    {


                        checkerArchive = readercont["SubjectDuplicate"].ToString();


                    }
                    readercont.Close();
                    
                   

                    string query2 = "select count(SubjectCode) as duplicate From Subject_Tbl Where SubjectCode = '" + txtSCode.Text +  "' AND Course = '" + cbxCourse.Text + "'";
                    SqlCommand command2 = new SqlCommand(query2, sqlcon);
                    SqlDataReader reader2 = command2.ExecuteReader();

                    if (reader2.Read() == true)
                    {
                        checker = reader2["duplicate"].ToString();
                    }
                    reader2.Close();
                }
         }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
     
        }

        public void checkerUpdate1()
        {
            try
            {

                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "select count(SubjectCode) as duplicate From Subject_Tbl Where SubjectCode = '" + txtSCode.Text + "' AND ID != '" + Convert.ToInt32(dgvSubj.CurrentRow.Cells["ID"].Value.ToString()) + "' AND Course = '" + cbxCourse.Text + "'";
                    SqlCommand command = new SqlCommand(query, sqlcon);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read() == true)
                    {
                        checkerUpdate = reader["duplicate"].ToString();
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
        public void PopulateGridViewSubject() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,SubjectCode,SubjectName,CredUnitLec,CredUnitLab,ContHrsLec,ContHrsLab,Semester,Course,YearLevel,Section,SubjectSlot,RoomCategory FROM Subject_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSubj.DataSource = dt;
                    dgvSubj.EnableHeadersVisualStyles = false;
                    this.dgvSubj.Columns[1].Width = 140;
                    this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvSubj.Columns[3].Width = 100;
                    this.dgvSubj.Columns[4].Width = 100;
                    this.dgvSubj.Columns[5].Width = 120;
                    this.dgvSubj.Columns[6].Width = 150;
                    this.dgvSubj.Columns[7].Width = 180;
                    dgvSubj.AllowUserToAddRows = false;
                    dgvSubj.Columns["SubjectSlot"].Visible = false;
                    dgvSubj.Columns["RoomCategory"].Visible = false;
                    dgvSubj.Columns["Section"].Visible = false;
                    dgvSubj.Columns["ID"].Visible = false;
                    dgvSubj.Columns[1].HeaderText = "Subject Code";
                    dgvSubj.Columns[2].HeaderText = "Subject Name";
                    dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                    dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                    dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                    dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                    dgvSubj.Columns[9].HeaderText = "Year Level";

                    dgvSubj.Columns[1].ReadOnly = true;
                    dgvSubj.Columns[2].ReadOnly = true;
                    dgvSubj.Columns[3].ReadOnly = true;
                    dgvSubj.Columns[4].ReadOnly = true;
                    dgvSubj.Columns[5].ReadOnly = true;
                    dgvSubj.Columns[6].ReadOnly = true;
                    dgvSubj.Columns[9].ReadOnly = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateCBX()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT CourseID,Course FROM Course_Tbl", sqlcon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cbxCourse.ValueMember = "CourseID";
                    cbxCourse.DisplayMember = "Course";
                    cbxCourse.DataSource = dt;
                    cbxCourse.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewFacultySearchSCode() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%' AND YearLevel='" + comboBox4.Text + "' AND Semester='" + comboBox2.Text + "'AND Course='" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "' AND Course = '"+comboBox3.Text+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "' AND YearLevel = '" + comboBox4.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%'  AND YearLevel='" + comboBox4.Text +"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectCode like '%" + txtSearch.Text + "%'  AND Course='" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateGridViewFacultySemester() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Semester = '" + comboBox2.Text + "'", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvSubj.DataSource = dt;
                    dgvSubj.EnableHeadersVisualStyles = false;
                    this.dgvSubj.Columns[1].Width = 140;
                    this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvSubj.Columns[3].Width = 100;
                    this.dgvSubj.Columns[4].Width = 100;
                    this.dgvSubj.Columns[5].Width = 120;
                    this.dgvSubj.Columns[6].Width = 150;
                    this.dgvSubj.Columns[7].Width = 180;


                    dgvSubj.AllowUserToAddRows = false;
                    dgvSubj.Columns["SubjectSlot"].Visible = false;
                    dgvSubj.Columns["RoomCategory"].Visible = false;
                    dgvSubj.Columns["Section"].Visible = false;
                    dgvSubj.Columns["ID"].Visible = false;
                    dgvSubj.Columns[1].HeaderText = "Subject Code";
                    dgvSubj.Columns[2].HeaderText = "Subject Name";
                    dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                    dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                    dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                    dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                    dgvSubj.Columns[9].HeaderText = "Year Level";

                    dgvSubj.Columns[1].ReadOnly = true;
                    dgvSubj.Columns[2].ReadOnly = true;
                    dgvSubj.Columns[3].ReadOnly = true;
                    dgvSubj.Columns[4].ReadOnly = true;
                    dgvSubj.Columns[5].ReadOnly = true;
                    dgvSubj.Columns[6].ReadOnly = true;
                    dgvSubj.Columns[9].ReadOnly = true;
                }
                    if(comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter2 = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Semester = '" + comboBox2.Text + "' AND Course ='"+comboBox3.Text+"'", conn);
                        DataTable dt2 = new DataTable();
                        adapter2.Fill(dt2);

                        dgvSubj.DataSource = dt2;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter3 = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Semester = '" + comboBox2.Text + "' AND Course ='" + comboBox3.Text + "' AND YearLevel ='"+comboBox4.Text+"'", conn);
                        DataTable dt3 = new DataTable();
                        adapter3.Fill(dt3);

                        dgvSubj.DataSource = dt3;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter3 = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Semester = '" + comboBox2.Text +  "' AND YearLevel ='" + comboBox4.Text + "'", conn);
                        DataTable dt3 = new DataTable();
                        adapter3.Fill(dt3);

                        dgvSubj.DataSource = dt3;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateGridViewFacultyCourse() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (comboBox2.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox3.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Course = '" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox3.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Course = '" + comboBox3.Text + "' AND Semester = '"+comboBox2.Text+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex == -1 && comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Course = '" + comboBox3.Text + "' AND YearLevel = '" + comboBox4.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE Course = '" + comboBox3.Text + "' AND YearLevel = '" + comboBox4.Text + "' AND Semester ='"+comboBox2.Text+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateGridViewFacultyYR() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox2.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE YearLevel = '" + comboBox4.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox2.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE YearLevel = '" + comboBox4.Text + "' AND Course = '"+comboBox3.Text+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox2.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE YearLevel = '" + comboBox4.Text + "' AND Semester = '" + comboBox2.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE YearLevel = '" + comboBox4.Text + "' AND Semester = '" + comboBox2.Text + "' AND Course ='"+comboBox3.Text+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewFacultySearchSName() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%' AND YearLevel='" + comboBox4.Text + "' AND Semester='" + comboBox2.Text + "'AND Course='" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "' AND Course = '" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%'  AND Semester='" + comboBox2.Text + "' AND YearLevel = '" + comboBox4.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%'  AND YearLevel='" + comboBox4.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                    if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Subject_Tbl WHERE SubjectName like '%" + txtSearch.Text + "%'  AND Course='" + comboBox3.Text + "'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvSubj.DataSource = dt;
                        dgvSubj.EnableHeadersVisualStyles = false;
                        this.dgvSubj.Columns[1].Width = 140;
                        this.dgvSubj.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvSubj.Columns[3].Width = 100;
                        this.dgvSubj.Columns[4].Width = 100;
                        this.dgvSubj.Columns[5].Width = 120;
                        this.dgvSubj.Columns[6].Width = 150;
                        this.dgvSubj.Columns[7].Width = 180;


                        dgvSubj.AllowUserToAddRows = false;
                        dgvSubj.Columns["SubjectSlot"].Visible = false;
                        dgvSubj.Columns["RoomCategory"].Visible = false;
                        dgvSubj.Columns["Section"].Visible = false;
                        dgvSubj.Columns["ID"].Visible = false;
                        dgvSubj.Columns[1].HeaderText = "Subject Code";
                        dgvSubj.Columns[2].HeaderText = "Subject Name";
                        dgvSubj.Columns[3].HeaderText = "Unit (Lec)";
                        dgvSubj.Columns[4].HeaderText = "Unit (Lab)";
                        dgvSubj.Columns[5].HeaderText = "Hours (Lec)";
                        dgvSubj.Columns[6].HeaderText = "Hours (Lab)";
                        dgvSubj.Columns[9].HeaderText = "Year Level";

                        dgvSubj.Columns[1].ReadOnly = true;
                        dgvSubj.Columns[2].ReadOnly = true;
                        dgvSubj.Columns[3].ReadOnly = true;
                        dgvSubj.Columns[4].ReadOnly = true;
                        dgvSubj.Columns[5].ReadOnly = true;
                        dgvSubj.Columns[6].ReadOnly = true;
                        dgvSubj.Columns[9].ReadOnly = true;
                    }
                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Checker()
        {
            if (txtSCode.Text.Equals("") && txtSCode.Text.Length == 0)
            {
                label16.Visible = true;
                label16.ForeColor = Color.Red;
                label1.ForeColor = Color.Red;
                check = true;
            }

            if (txtSName.Text.Equals("") && txtSName.Text.Length == 0)
            {
                label17.Visible = true;
                label17.ForeColor = Color.Red;
                label2.ForeColor = Color.Red;
                check = true;
            }
            if (txtCULec.Text.Equals("0")  && txtCULab.Text.Equals("0"))
            {
                label18.Visible = true;
                label18.ForeColor = Color.Red;
                label4.ForeColor = Color.Red;

                label19.Visible = true;
                label19.ForeColor = Color.Red;
                label5.ForeColor = Color.Red;
                check = true;
            }
            if (rbFirstSem.Checked == false && rbSecondSem.Checked == false)
            {
                label20.Visible = true;
                label20.ForeColor = Color.Red;
                label8.ForeColor = Color.Red;
                check = true;
            }
            if (cbxCourse.Text.Equals("") && cbxCourse.SelectedIndex == -1)
            {
                label21.Visible = true;
                label21.ForeColor = Color.Red;
                label9.ForeColor = Color.Red;
                check = true;
            }
            if (cbxYear.Text.Equals("") && cbxYear.SelectedIndex == -1)
            {
                label22.Visible = true;
                label22.ForeColor = Color.Red;
                label7.ForeColor = Color.Red;
                check = true;
            }
            if (rbNonMajor.Checked == false && rbMajor.Checked == false)
            {
                label23.Visible = true;
                label23.ForeColor = Color.Red;
                label10.ForeColor = Color.Red;
                check = true;
            }
            if (rbMajor.Checked == true && (rbYes.Checked == false && rbNo.Checked == false) )
            {
                label24.Visible = true;
                label24.ForeColor = Color.Red;
                label11.ForeColor = Color.Red;
                check = true;
            }

            if (txtSCode.Text.Length != 0 && txtSName.Text.Length != 0 && (txtCULec.Text != "0" || txtCULab.Text != "0") && (rbFirstSem.Checked == true || rbSecondSem.Checked == true) && cbxCourse.Text.Length != 0 && cbxYear.Text.Length != 0 && (rbNonMajor.Checked == true || rbMajor.Checked == true) && ((rbMajor.Checked == true && (rbYes.Checked == true || rbNo.Checked == true) || rbNonMajor.Checked == true) ))
            {
                check = false;
                label16.Visible = false;
                label16.ForeColor = Color.Red;
                label1.ForeColor = Color.Gray;

                label17.Visible = false;
                label17.ForeColor = Color.Red;
                label2.ForeColor = Color.Gray;

                label18.Visible = false;
                label18.ForeColor = Color.Red;
                label4.ForeColor = Color.Gray;

                label19.Visible = false;
                label19.ForeColor = Color.Red;
                label5.ForeColor = Color.Gray;

                label20.Visible = false;
                label20.ForeColor = Color.Red;
                label8.ForeColor = Color.Gray;

                label21.Visible = false;
                label21.ForeColor = Color.Red;
                label9.ForeColor = Color.Gray;

                label22.Visible = false;
                label22.ForeColor = Color.Red;
                label7.ForeColor = Color.Gray;

                label23.Visible = false;
                label23.ForeColor = Color.Red;
                label10.ForeColor = Color.Gray;

                label24.Visible = false;
                label24.ForeColor = Color.Red;
                label11.ForeColor = Color.Gray;
            }


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
         //   try
          //  {
              
              
                DialogResult dr = MessageBox.Show("Save data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Checker();
                    if (check == false)
                    {
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                            sqlcon.Open();
                            subjectSlot();
                            SchedulePlotted();
                            roomCateg();
                            if (Convert.ToInt32(checker) >= 1)
                            {
                                MessageBox.Show("Subject code already exist", "Subject Code",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            }
                            else
                            {
                             
                                SqlCommand cmd = new SqlCommand("INSERT INTO Subject_Tbl (SubjectCode,SubjectName,CredUnitLec,CredUnitLab,ContHrsLec,ContHrsLab,Semester,Course,YearLevel,Section,SubjectSlot,RoomCategory) VALUES (@SubjectCode,@SubjectName,@CredUnitLec,@CredUnitLab,@ContHrsLec,@ContHrsLab,@Semester,@Course,@YearLevel,@Section,@SubjectSlot,@RoomCategory)", sqlcon);
                                cmd.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                                cmd.Parameters.AddWithValue("@SubjectName", txtSName.Text);
                                cmd.Parameters.AddWithValue("@CredUnitLec", txtCULec.Text);
                                cmd.Parameters.AddWithValue("@CredUnitLab", txtCULab.Text);

                                cmd.Parameters.AddWithValue("@ContHrsLec", txtCHLec.Text);
                                cmd.Parameters.AddWithValue("@ContHrsLab", txtCHLab.Text);

                                if (rbFirstSem.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@Semester", rbFirstSem.Text);
                                }
                                else if (rbSecondSem.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@Semester", rbSecondSem.Text);
                                }
                                cmd.Parameters.AddWithValue("@Course", cbxCourse.Text);
                                if (cbxYear.Text == "First Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "1");
                                }
                                else if (cbxYear.Text == "Second Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "2");
                                }
                                else if (cbxYear.Text == "Third Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "3");
                                }
                                else if (cbxYear.Text == "Fourth Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "4");
                                }
                                cmd.Parameters.AddWithValue("@Section", "1");
                                cmd.Parameters.AddWithValue("@SubjectSlot", subjSlot);
                                if (rbNonMajor.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@RoomCategory", "0");
                                }
                                else
                                {
                                    if (rbYes.Checked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@RoomCategory", "1");
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@RoomCategory", "2");
                                    }
                                }

                                cmd.ExecuteNonQuery();
                                lblresult.Text = "Succesfully Saved";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                PopulateGridViewSubject();
                                clear();

                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " added a subject");
                                cm.ExecuteNonQuery();
                            }
                        }

                    }
                }
        /*    }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        */
        }

        private void frmSubj_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateCBX();
                PopulateGridViewSubject();
                AdminActivity();
                if (typeofAcc == "1")
                {
                    btnUpdate.Enabled = false;
                    btnUpdate.BackColor = Color.Gray;
                    btnArchived.Visible = false;
                }
                else
                {
                    btnArchived.Visible = true;
                }
                if (dgvSubj.Rows.Count != 0)
                {
                    dgvSubj.Rows[0].Selected = false;
                }
                btnUpdate.BackColor = Color.Gray;
                btnArchive.BackColor = Color.Gray;
                groupBox5.Enabled = false;
                panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
               panel1.Height, 20, 20));
                /*    txtSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSearch.Width,
                   txtSearch.Height, 15, 15));
                    txtSName.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSName.Width,
                   txtSName.Height, 15, 15));
                    txtSCode.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSCode.Width,
                 txtSCode.Height, 15, 15));
                    txtCULec.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtCULec.Width,
               txtCULec.Height, 15, 15));
                    txtCULab.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtCULab.Width,
              txtCULab.Height, 15, 15));
                    txtCHLec.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtCHLec.Width,
              txtCHLec.Height, 15, 15));
                    txtCHLab.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtCHLab.Width,
          txtCHLab.Height, 15, 15));

                    cbxCourse.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cbxCourse.Width,
          cbxCourse.Height, 15, 15));
                    cbxYear.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cbxYear.Width,
          cbxYear.Height, 15, 15));
                    comboBox1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, comboBox1.Width,
          comboBox1.Height, 15, 15));
                  */
                btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
              btnSave.Height, 30, 30));
                btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width,
                btnSearch.Height, 30, 30));
                btnArchive.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchive.Width,
              btnArchive.Height, 30, 30));
                btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width,
             btnUpdate.Height, 30, 30));
                btnClear.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClear.Width,
            btnClear.Height, 30, 30));
                btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
           btnClose.Height, 30, 30));
                btnArchived.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchived.Width,
           btnArchived.Height, 30, 30));
                dgvSubj.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dgvSubj.Width,
           dgvSubj.Height, 5, 5));
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

        private void txtCULec_TextChanged(object sender, EventArgs e)
        {
            if (txtCULec.Text == "")
            {
                txtCHLec.Text = "0";

            }
            else
            {
                txtCHLec.Text = txtCULec.Text;
            }
          
        }

        private void txtCULab_TextChanged(object sender, EventArgs e)
        {
            if(txtCULab.Text == "")
            {
                txtCHLab.Text = "0";
            }
            else
            {
                int i = int.Parse(txtCULab.Text);
                int x = i * 3;
                txtCHLab.Text = x.ToString();
            }
          
        }
        void clear()
        {
            txtSCode.Text = txtSName.Text = txtSearch.Text = "" ;
            cbxYear.SelectedIndex = -1;
            cbxCourse.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            txtCULec.Text = txtCULab.Text = txtCULec.Text = txtCULec.Text = "0";
            rbFirstSem.Checked = rbSecondSem.Checked = false;
            lblresult.Visible = false;
            btnSave.Enabled = true;
            btnSave.Enabled = true;
            btnArchive.Enabled = false;
            btnUpdate.Enabled = false;
            btnArchive.BackColor = Color.Gray;
            btnUpdate.BackColor = Color.Gray;
            btnSave.BackColor = Color.MediumSeaGreen;
            if (dgvSubj.Rows.Count !=0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
            rbMajor.Checked = rbNonMajor.Checked = rbNo.Checked = rbYes.Checked = false;
            PopulateGridViewSubject(); ;
            txtSCode.Enabled = true;
            lblresult.Visible = false;
            label16.Visible = false;
            label16.ForeColor = Color.Red;
            label1.ForeColor = Color.Gray;

            label17.Visible = false;
            label17.ForeColor = Color.Red;
            label2.ForeColor = Color.Gray;

            label18.Visible = false;
            label18.ForeColor = Color.Red;
            label4.ForeColor = Color.Gray;

            label19.Visible = false;
            label19.ForeColor = Color.Red;
            label5.ForeColor = Color.Gray;

            label20.Visible = false;
            label20.ForeColor = Color.Red;
            label8.ForeColor = Color.Gray;

            label21.Visible = false;
            label21.ForeColor = Color.Red;
            label9.ForeColor = Color.Gray;

            label22.Visible = false;
            label22.ForeColor = Color.Red;
            label7.ForeColor = Color.Gray;

            label23.Visible = false;
            label23.ForeColor = Color.Red;
            label10.ForeColor = Color.Gray;

            label24.Visible = false;
            label24.ForeColor = Color.Red;
            label11.ForeColor = Color.Gray;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            if(dgvSubj.Rows.Count != 0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
            btnUpdate.Enabled = false;
            btnArchive.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Subject Code")
            {
                PopulateGridViewFacultySearchSCode();
            }
            else if (comboBox1.Text == "Subject Name")
            {
                PopulateGridViewFacultySearchSName();
            }
            if(dgvSubj.Rows.Count != 0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
        }

        private void dgvSubj_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblresult.Visible = false;
                btnSave.Enabled = false;
                btnUpdate.Enabled = true;
                btnArchive.Enabled = true;
                btnArchive.BackColor = Color.MediumSeaGreen;
                btnUpdate.BackColor = Color.MediumSeaGreen;
                btnSave.BackColor = Color.Gray;
     
                txtSCode.Text = dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString();
                txtSName.Text = dgvSubj.CurrentRow.Cells["SubjectName"].Value.ToString();
                txtCULec.Text = dgvSubj.CurrentRow.Cells["CredUnitLec"].Value.ToString();
                txtCULab.Text = dgvSubj.CurrentRow.Cells["CredUnitLab"].Value.ToString();
                txtCHLec.Text = dgvSubj.CurrentRow.Cells["ContHrsLec"].Value.ToString();
                txtCHLab.Text = dgvSubj.CurrentRow.Cells["ContHrsLab"].Value.ToString();
                roomCateg();
                if (dgvSubj.CurrentRow.Cells["Semester"].Value.ToString().Equals(rbFirstSem.Text))
                {
                    rbFirstSem.Checked = true;
                }
                else if (dgvSubj.CurrentRow.Cells["Semester"].Value.ToString().Equals(rbSecondSem.Text))
                {
                    rbSecondSem.Checked = true;
                }
                cbxCourse.Text = dgvSubj.CurrentRow.Cells["Course"].Value.ToString();
                if (dgvSubj.CurrentRow.Cells["YearLevel"].Value.ToString().Equals("1"))
                {
                    cbxYear.Text = "First Year";
                }
                else if (dgvSubj.CurrentRow.Cells["YearLevel"].Value.ToString().Equals("2"))
                {
                    cbxYear.Text = "Second Year";
                }
                else if (dgvSubj.CurrentRow.Cells["YearLevel"].Value.ToString().Equals("3"))
                {
                    cbxYear.Text = "Third Year";
                }
                else if (dgvSubj.CurrentRow.Cells["YearLevel"].Value.ToString().Equals("4"))
                {
                    cbxYear.Text = "Fourth Year";
                }

                if (dgvSubj.CurrentRow.Cells["RoomCategory"].Value.ToString().Equals("0"))
                {
                    rbNonMajor.Checked = true;
                    rbYes.Checked = false;
                    rbNo.Checked = false;
                }
                else if (dgvSubj.CurrentRow.Cells["RoomCategory"].Value.ToString().Equals("1"))
                {
                    rbMajor.Checked = true;
                    rbYes.Checked = true;

                }
                else if (dgvSubj.CurrentRow.Cells["RoomCategory"].Value.ToString().Equals("2"))
                {
                    rbMajor.Checked = true;
                    rbNo.Checked = true;

                }

                //  txtSecSlot.Text = dgvSubj.CurrentRow.Cells[10].Value.ToString();
                // txtRoomCode.Text = dgvSubj.CurrentRow.Cells[11].Value.ToString();

                Checker();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
          try
           {
                DialogResult dr = MessageBox.Show("Do you want to save changes?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    Checker();
                    if (check == false)
                    {
                  
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                        checkerUpdate1();
                        if (Convert.ToInt32(checkerUpdate) >= 1)
                            {
                                MessageBox.Show("Subject code already exist", "Subject Code",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            }
                            else
                            {
                            sqlcon.Open();
                            specializationUpdate();
                                string roomcateg = "";
                                string numberofRoom = "0";
                                if(rbMajor.Checked == true && rbYes.Checked == true)
                                {
                                    roomcateg = "1";
                                }
                               else if (rbMajor.Checked == true && rbNo.Checked == true)
                                {
                                    roomcateg = "2";
                                }
                               else if (rbMajor.Checked == true )
                                {
                                    roomcateg = "0";
                                }
                                bool sameroom = false;

                                string roomTbl = "";
                                if (dgvSubj.CurrentRow.Cells["RoomCategory"].Value.ToString() == roomcateg)
                                {
                                    sameroom = true;
                                }
                                else
                                {
                                    sameroom = false;
                                }
                            //MessageBox.Show(sameroom.ToString());
                                string queryyy = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcateg + "' AND Course='" + cbxCourse.Text + "'";
                                SqlCommand commanddd = new SqlCommand(queryyy, sqlcon);
                                SqlDataReader readerrr = commanddd.ExecuteReader();

                                if (readerrr.Read() == true)
                                {
                                    numberofRoom = readerrr["numberOfroom"].ToString();
                                }
                                readerrr.Close();
                            int roomID = 1;

                            for (int i = 0; i < ID.Count; i++)
                                {
                                
                                string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg + "' AND Course='" + cbxCourse.Text + "' AND RoomID='" + roomID.ToString() + "'";
                                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                SqlDataReader reader1 = command1.ExecuteReader();

                                if (reader1.Read() == true)
                                {



                                    roomTbl = reader1["Room"].ToString();

                                }
                                reader1.Close();


                                SqlCommand cmdd = new SqlCommand("UPDATE Specialization_Tbl SET SubjectCode=@SubjectCode,CredUnitLab=@CredUnitLab,CredUnitLec=@CredUnitLec,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab,Course=@Course,Semester=@Semester,RoomCategory=@RoomCategory,Room=@Room WHERE ID=@ID", sqlcon);
                                    cmdd.Parameters.AddWithValue("@ID", Convert.ToInt32(ID[i]));
                                    cmdd.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                                    cmdd.Parameters.AddWithValue("@CredUnitLab", txtCULab.Text);
                                    cmdd.Parameters.AddWithValue("@CredUnitLec", txtCULec.Text);
                                    cmdd.Parameters.AddWithValue("@ContHrsLec", txtCHLec.Text);
                                    cmdd.Parameters.AddWithValue("@ContHrsLab", txtCHLab.Text);
                                    cmdd.Parameters.AddWithValue("@Course", cbxCourse.Text);
                                    if(rbFirstSem.Checked == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@Semester", "First Semester");
                                    }
                                    else if (rbSecondSem.Checked == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@Semester", "Second Semester");
                                    }
                                    if(rbMajor.Checked == true && rbYes.Checked == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@RoomCategory", "1");
                                      
                                    }
                                    else if (rbMajor.Checked == true && rbNo.Checked == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@RoomCategory", "2");
                                     
                                    }
                                    else if (rbNonMajor.Checked == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@RoomCategory", "0");
                                    
                                    }
                                
                                    if(sameroom == true)
                                    {
                                        cmdd.Parameters.AddWithValue("@Room", ROOM[i]);
                                    roomSched.Add(ROOM[i]);
                                }
                                    else
                                    {

                                        cmdd.Parameters.AddWithValue("@Room", roomTbl);
                                    roomSched.Add(roomTbl);
                                    }
                                   
                                    cmdd.ExecuteNonQuery();
                                   if(roomID <= Convert.ToInt32(numberofRoom))
                                {
                                    roomID += 1;
                                }
                                   
                               
                            }
                            string numberofRoomSCHED = "0";
                            string queryyyyy = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcateg + "' AND Course='" + dgvSubj.CurrentRow.Cells["Course"].Value.ToString() + "'";
                            SqlCommand commanddddd = new SqlCommand(queryyyyy, sqlcon);
                            SqlDataReader readerrrrr = commanddddd.ExecuteReader();

                            if (readerrrrr.Read() == true)
                            {
                                numberofRoomSCHED = readerrrrr["numberOfroom"].ToString();
                            }
                            readerrrrr.Close();

                            bool noDuplicate = false;
                            bool newroom = false;
                            int num = 0;
                            int roomidSCHED = 1;
                            string SchedDuplicateForRoom = "0";

                            string queryyyy = "SELECT ID,Section,TimeID,DayID,Semester FROM FacultySchedule_Tbl Where SubjectCode ='" + dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString() + "' AND Course='" + dgvSubj.CurrentRow.Cells["Course"].Value.ToString() + "'";
                            SqlCommand cmdddd = new SqlCommand(queryyyy, sqlcon);
                            using (SqlDataReader readerrrr = cmdddd.ExecuteReader())
                            {
                                while (readerrrr.Read())
                                {
                                    idSched.Add(readerrrr.GetInt32(0).ToString());
                                    sectionSched.Add(readerrrr.GetString(1));
                                    timeID.Add(readerrrr.GetString(2));
                                    dayID.Add(readerrrr.GetString(3));
                                    semester.Add(readerrrr.GetString(4));

                                }
                            }

                            for (int i = 0; i < idSched.Count; i++)
                                {
                             
                              
                                    
                            //    MessageBox.Show(idSched[i] + timeID[i] + dayID);




                                SqlCommand cmddd = new SqlCommand("UPDATE FacultySchedule_Tbl SET SubjectCode=@SubjectCode,Semester=@Semester,Course=@Course,Room=@Room WHERE ID=@ID AND Section=@Section AND TimeID=@TimeID AND DayID=@DayID", sqlcon);
                                    cmddd.Parameters.AddWithValue("@ID", Convert.ToInt32(idSched[i]));
                                    cmddd.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                                    if (rbFirstSem.Checked == true)
                                    {
                                        cmddd.Parameters.AddWithValue("@Semester", "First Semester");
                                    }
                                    else if (rbSecondSem.Checked == true)
                                    {
                                        cmddd.Parameters.AddWithValue("@Semester", "Second Semester");
                                    }
                                cmddd.Parameters.AddWithValue("@Course", cbxCourse.Text);
                                cmddd.Parameters.AddWithValue("@Section", section[num]);
                                cmddd.Parameters.AddWithValue("@TimeID", dayID[i]);
                                cmddd.Parameters.AddWithValue("@DayID", timeID[i]);
                              
                                do
                                {
                                    string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg + "' AND Course='" + cbxCourse.Text + "' AND RoomID='" + roomidSCHED.ToString() + "'";
                                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                    SqlDataReader reader1 = command1.ExecuteReader();

                                    if (reader1.Read() == true)
                                    {



                                        roomTbl = reader1["Room"].ToString();

                                    }
                                    reader1.Close();

                            
                                    string query4 = "SELECT COUNT(ID) AS NumberOfDuplicateForRoom FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Room=@Room AND Semester=@Semester";
                                    SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                    command4.Parameters.AddWithValue("@DayID", dayID[i]);
                                    command4.Parameters.AddWithValue("@TimeID", timeID[i]);
                                    if(noDuplicate == false)
                                    {
                                        command4.Parameters.AddWithValue("@Room", roomSched[num]);
                                    }
                                    else
                                    {
                                        command4.Parameters.AddWithValue("@Room", roomTbl);
                                    }
                                 
                                    command4.Parameters.AddWithValue("@Semester", semester[i]);
                                    SqlDataReader reader4 = command4.ExecuteReader();

                                    if (reader4.Read() == true)
                                    {


                                        SchedDuplicateForRoom = reader4["NumberOfDuplicateForRoom"].ToString();


                                    }
                                    reader4.Close();
                                 
                                    if(Convert.ToInt32(SchedDuplicateForRoom) >= 1)
                                    {
                                   
                                        if (roomidSCHED > Convert.ToInt32(numberofRoomSCHED))
                                        {
                                            roomidSCHED = 1;
                                        }
                                        if (numberofRoomSCHED == "0")
                                        {
                                            cmddd.Parameters.AddWithValue("@Room", "TBA");
                                            noDuplicate = false;
                                            newroom = false;
                                        }
                                       else if (roomidSCHED <= Convert.ToInt32(numberofRoomSCHED))
                                        {
                                            roomidSCHED += 1;
                                            noDuplicate = true;
                                            newroom = true;
                                        }

                                       
                                       
                                    }
                                    else if(Convert.ToInt32(SchedDuplicateForRoom) == 0 && newroom == false)
                                    {
                                        cmddd.Parameters.AddWithValue("@Room", roomSched[num]);
                                        newroom = false;
                                        noDuplicate = false;
                                    }
                                    else if (Convert.ToInt32(SchedDuplicateForRoom) == 0 && newroom == true)
                                            {
                                        cmddd.Parameters.AddWithValue("@Room", roomTbl);
                                        newroom = false;
                                        noDuplicate = false;
                                    }




                                } while (noDuplicate == true);


                                cmddd.ExecuteNonQuery();
                              
                                if(section[num] != sectionSched[i])
                                {
                                    num += 1;
                                }
                                }
                          
                            ID.Clear();
                            idSched.Clear();
                            ROOM.Clear();
                            section.Clear();
                            sectionSched.Clear();
                            roomSched.Clear();
                            timeID.Clear();
                            dayID.Clear();
                            semester.Clear();
                           

                            SqlCommand cmd = new SqlCommand("UPDATE Subject_Tbl SET SubjectCode=@SubjectCode,SubjectName=@SubjectName,CredUnitLec=@CredUnitLec,CredUnitLab=@CredUnitLab,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab,Course=@Course,YearLevel=@YearLevel,Semester=@Semester,RoomCategory=@RoomCategory WHERE ID=@ID ", sqlcon);
                                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dgvSubj.CurrentRow.Cells["ID"].Value.ToString()));
                                cmd.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                                cmd.Parameters.AddWithValue("@SubjectName", txtSName.Text);
                                cmd.Parameters.AddWithValue("@CredUnitLec", txtCULec.Text);
                                cmd.Parameters.AddWithValue("@CredUnitLab", txtCULab.Text);
                                cmd.Parameters.AddWithValue("@ContHrsLec", txtCHLec.Text);
                                cmd.Parameters.AddWithValue("@ContHrsLab", txtCHLab.Text);
                                if (cbxYear.Text == "First Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "1");
                                }
                                else if (cbxYear.Text == "Second Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "2");
                                }
                                else if (cbxYear.Text == "Third Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "3");
                                }
                                else if (cbxYear.Text == "Fourth Year")
                                {
                                    cmd.Parameters.AddWithValue("@YearLevel", "4");
                                }
                                if (rbFirstSem.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@Semester", rbFirstSem.Text);
                                }
                                else if (rbSecondSem.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@Semester", rbSecondSem.Text);
                                }
                                if (rbNonMajor.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@RoomCategory", "0");
                                }
                                else
                                {
                                    if (rbYes.Checked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@RoomCategory", "1");
                                    }
                                    else if (rbNo.Checked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@RoomCategory", "2");
                                    }
                                }
                                cmd.Parameters.AddWithValue("@Course", cbxCourse.Text);
                                cmd.ExecuteNonQuery();

                               
                                btnSave.Enabled = true;
                                btnUpdate.Enabled = false;
                                btnArchive.Enabled = false;
                                clear();
                                if (dgvSubj.Rows.Count != 0)
                                {
                                    dgvSubj.Rows[0].Selected = false;
                                }
                                lblresult.Text = "Succesfully Updated";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                PopulateGridViewSubject();

                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a subject");
                                cm.ExecuteNonQuery();
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

        private void btnArchive_Click(object sender, EventArgs e)
        {
        
            
               try
               {
                   roomCateg();
                   DialogResult dr = MessageBox.Show("Archive data?", "Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                   if (dr == DialogResult.Yes)
                   {

                       using (SqlConnection sqlcon = new SqlConnection(conn))
                       {
                           sqlcon.Open();
                           bool existing = false;
                           SPUpdateOnArchive();
                           if (Convert.ToInt32(existingSP) >= 1)
                           {
                               DialogResult dr1 = MessageBox.Show("Are you sure you really want to archive Selected Subject? By doing this, it will delete all existing plotted schedule for this subject. Are you sure you want to proceed?", "Archive Selected Subject", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                               if (dr1 == DialogResult.Yes)
                               {

                                   SqlCommand cmddel0 = new SqlCommand("DELETE FROM Specialization_Tbl WHERE SubjectCode = @SubjectCode AND Course=@Course", sqlcon);
                                   cmddel0.CommandType = CommandType.Text;
                                   cmddel0.Parameters.AddWithValue("@SubjectCode", dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString());
                                   cmddel0.Parameters.AddWithValue("@Course", dgvSubj.CurrentRow.Cells["Course"].Value.ToString());
                                   cmddel0.ExecuteNonQuery();

                                   SqlCommand cmddel1 = new SqlCommand("DELETE FROM FacultySchedule_Tbl WHERE SubjectCode = @SubjectCode AND Course=@Course", sqlcon);
                                   cmddel1.CommandType = CommandType.Text;
                                   cmddel1.Parameters.AddWithValue("@SubjectCode", dgvSubj.CurrentRow.Cells["SubjectCode"].Value.ToString());
                                   cmddel1.Parameters.AddWithValue("@Course", dgvSubj.CurrentRow.Cells["Course"].Value.ToString());
                                   cmddel1.ExecuteNonQuery();
                                   existing = true;

                            }
                           }

                           if (existing == true || (existing == false && Convert.ToInt32(existingSP) == 0))
                           {
                               SqlCommand cmd = new SqlCommand("INSERT INTO SubjectArchive_Tbl (SubjectCode,SubjectName,CredUnitLec,CredUnitLab,ContHrsLec,ContHrsLab,Semester,Course,YearLevel,Section,SubjectSlot,RoomCategory) VALUES (@SubjectCode,@SubjectName,@CredUnitLec,@CredUnitLab,@ContHrsLec,@ContHrsLab,@Semester,@Course,@YearLevel,@Section,@SubjectSlot,@RoomCategory)", sqlcon);
                               cmd.Parameters.AddWithValue("@SubjectCode", txtSCode.Text);
                               cmd.Parameters.AddWithValue("@SubjectName", txtSName.Text);
                               cmd.Parameters.AddWithValue("@CredUnitLec", txtCULec.Text);
                               cmd.Parameters.AddWithValue("@CredUnitLab", txtCULab.Text);
                               cmd.Parameters.AddWithValue("@ContHrsLec", txtCHLec.Text);
                               cmd.Parameters.AddWithValue("@ContHrsLab", txtCHLab.Text);
                               if (rbFirstSem.Checked == true)
                               {
                                   cmd.Parameters.AddWithValue("@Semester", rbFirstSem.Text);
                               }
                               else if (rbSecondSem.Checked == true)
                               {
                                   cmd.Parameters.AddWithValue("@Semester", rbSecondSem.Text);
                               }
                               cmd.Parameters.AddWithValue("@Course", cbxCourse.Text);
                               if (cbxYear.Text == "First Year")
                               {
                                   cmd.Parameters.AddWithValue("@YearLevel", "1");
                               }
                               else if (cbxYear.Text == "Second Year")
                               {
                                   cmd.Parameters.AddWithValue("@YearLevel", "2");
                               }
                               else if (cbxYear.Text == "Third Year")
                               {
                                   cmd.Parameters.AddWithValue("@YearLevel", "3");
                               }
                               else if (cbxYear.Text == "Fourth Year")
                               {
                                   cmd.Parameters.AddWithValue("@YearLevel", "4");
                               }
                               cmd.Parameters.AddWithValue("@Section", "1");
                               cmd.Parameters.AddWithValue("@SubjectSlot", SPArchiveSecSlot[0]);
                               if (rbNonMajor.Checked == true)
                               {
                                   cmd.Parameters.AddWithValue("@RoomCategory", "0");
                               }
                               else
                               {
                                   if (rbYes.Checked == true)
                                   {
                                       cmd.Parameters.AddWithValue("@RoomCategory", "1");
                                   }
                                   else
                                   {
                                       cmd.Parameters.AddWithValue("@RoomCategory", "2");
                                   }
                               }
                               cmd.ExecuteNonQuery();
                               SqlCommand cmddel = new SqlCommand("DELETE FROM Subject_Tbl WHERE SubjectCode = '" + txtSCode.Text + "' AND Course='" + cbxCourse.Text + "'", sqlcon);
                               cmddel.CommandType = CommandType.Text;
                               //  cmddel.Parameters.AddWithValue("@FacultyCode", txtSCode.Text);
                               cmddel.ExecuteNonQuery();
                               clear();
                               lblresult.ForeColor = Color.Green;
                               lblresult.Visible = true;
                               lblresult.Text = "Archived";
                               PopulateGridViewSubject();
                            SPArchiveSecSlot.Clear();

                            DateTime time = DateTime.Now;
                               string format = "yyyy-MM-dd";
                               SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                               cm.Parameters.AddWithValue("@Username", loginAct);
                               cm.Parameters.AddWithValue("@ActivityLog", loginAct + " archive a subject");
                               cm.ExecuteNonQuery();

                           }

                       }
                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);
               }
            

        }

        private void txtCULec_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                e.Handled = true;
            txtCULec.MaxLength = 2;
        }

        private void txtCULab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                e.Handled = true;
            txtCULab.MaxLength = 2;
        }

        private void btnArchived_Click(object sender, EventArgs e)
        {
            SubjectArchive SA = new SubjectArchive(this);
            SA.ShowDialog();
        }

        private void txtCULab_Click(object sender, EventArgs e)
        {
          
            txtCULab.Text = "";
        }

        private void txtCULec_Click(object sender, EventArgs e)
        {

            txtCULec.Text = "";
        }

      

        private void txtCULec_Leave(object sender, EventArgs e)
        {
            Checker();
            if (txtCULec.Text.Equals("") || txtCULec.Text.Length == 0)
            {
                txtCULec.Text.Equals("0");
            }
            
        }

        private void txtCULab_Leave(object sender, EventArgs e)
        {
            Checker();
            if (txtCULab.Text == "")
            {
                txtCULab.Text = "0";
            }
       
        }

        private void rbMajor_CheckedChanged(object sender, EventArgs e)
        {
            label23.Visible = false;
            label10.ForeColor = Color.Gray;
            if (rbMajor.Checked == true)
            {
                groupBox5.Enabled = true;
            }
        }

        private void rbNonMajor_CheckedChanged(object sender, EventArgs e)
        {
            label23.Visible = false;
            label10.ForeColor = Color.Gray;
            label11.ForeColor = Color.Gray;
            label24.Visible = false;
            if (rbNonMajor.Checked == true)
            {
                groupBox5.Enabled = false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSCode_MouseClick(object sender, MouseEventArgs e)
        {
            label16.Visible = false;
            label1.ForeColor = Color.Gray;
        }

        private void txtSCode_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void txtSName_MouseClick(object sender, MouseEventArgs e)
        {
            label17.Visible = false;
            label2.ForeColor = Color.Gray;
        }

        private void txtSName_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void txtCULec_MouseClick(object sender, MouseEventArgs e)
        {
            label18.Visible = false;
            label4.ForeColor = Color.Gray;

            label19.Visible = false;
            label5.ForeColor = Color.Gray;
        }

        private void txtCULab_MouseClick(object sender, MouseEventArgs e)
        {
            label18.Visible = false;
            label4.ForeColor = Color.Gray;

            label19.Visible = false;
            label5.ForeColor = Color.Gray;
        }

        private void rbFirstSem_CheckedChanged(object sender, EventArgs e)
        {
            label20.Visible = false;
            label8.ForeColor = Color.Gray;
        }

        private void rbSecondSem_CheckedChanged(object sender, EventArgs e)
        {
            label20.Visible = false;
            label8.ForeColor = Color.Gray;
        }

        private void cbxCourse_MouseClick(object sender, MouseEventArgs e)
        {
            label21.Visible = false;
            label9.ForeColor = Color.Gray;
        }

        private void cbxCourse_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void cbxYear_MouseClick(object sender, MouseEventArgs e)
        {
            label22.Visible = false;
            label7.ForeColor = Color.Gray;
        }

        private void cbxYear_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void rbYes_CheckedChanged(object sender, EventArgs e)
        {
            label24.Visible = false;
            label11.ForeColor = Color.Gray;
        }

        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            label24.Visible = false;
            label11.ForeColor = Color.Gray;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtSCode_TextChanged(object sender, EventArgs e)
        {
            txtSCode.Text = txtSCode.Text.ToUpper();
        }

        private void txtSName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSName.Text.Length <= 0) return;
                string s = txtSName.Text.Substring(0, 1);
                if (s != s.ToUpper())
                {
                    int curSelStart = txtSName.SelectionStart;
                    int curSelLength = txtSName.SelectionLength;
                    txtSName.SelectionStart = 0;
                    txtSName.SelectionLength = 1;
                    txtSName.SelectedText = s.ToUpper();
                    txtSName.SelectionStart = curSelStart;
                    txtSName.SelectionLength = curSelLength;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbxCourse_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewFacultySemester();
            if (dgvSubj.Rows.Count != 0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewFacultyCourse();
            if (dgvSubj.Rows.Count != 0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewFacultyYR();
            if(dgvSubj.Rows.Count != 0)
            {
                dgvSubj.Rows[0].Selected = false;
            }
        }
    }
    }
