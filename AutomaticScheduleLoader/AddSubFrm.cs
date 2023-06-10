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
    public partial class AddSubFrm : Form
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
        DefSchedFrm frmS;
        int counter = 0;
        int secslot = 0;
        int count = 0;
        List<int> hrs = new List<int>();
        bool btnadd = false;
        bool btnremove = false;
        bool add = false;
        string totalsection = "1";
        int hrslimiter = 0;
        List<string> someList = new List<string>();
        int numericvalue = 0;
        Timer Clock = new Timer();
        string Subj = "";
        string loginAct = "";
        string typeofAcc = "";
        bool ischecked = false;
        int num = 0;
        public AddSubFrm(DefSchedFrm formsub)
        {
            InitializeComponent();
        //    this.FormBorderStyle = FormBorderStyle.None;
           // Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.frmS = formsub;
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
        void PopulateGridViewSubject() // filter gridview faculty by code
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query0 = "select FacultyCode,SubjectCode,ContHrsLec FROM Specialization_Tbl  WHERE FacultyCode='" + frmS.cbxFaculty.Text + "' AND SubjectCode ='" + "Consultation Hours" + "'";
                    SqlCommand command0 = new SqlCommand(query0, sqlcon);
                    SqlDataReader reader0 = command0.ExecuteReader();

                    if (reader0.Read() == true)
                    {



                        numericUpDown1.Text = reader0["ContHrsLec"].ToString();
                        Subj = reader0["SubjectCode"].ToString();

                    }
                    reader0.Close();

                    string query01 = "select FacultyCode,SubjectCode,ContHrsLec FROM Specialization_Tbl  WHERE FacultyCode='" + frmS.cbxFaculty.Text + "' AND SubjectCode ='" + "Research And Extension" + "'";
                    SqlCommand command01 = new SqlCommand(query01, sqlcon);
                    SqlDataReader reader01 = command01.ExecuteReader();

                    if (reader01.Read() == true)
                    {



                        numericUpDown1.Text = reader01["ContHrsLec"].ToString();
                        Subj = reader01["SubjectCode"].ToString();

                    }
                    reader01.Close();

                    if (btnadd == true)
                    {
                        string sem = "";
                        if(rb1stsem.Checked == true)
                        {
                            sem = "First Semester";
                        }
                        else if (rb2ndsem.Checked == true)
                        {
                            sem = "Second Semester";
                        }
                       // dgvSubject.DataSource = null;
                        // ---------------------------------------------------------- //
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,SubjectCode,SubjectName,CredUnitLec,CredUnitLab,ContHrsLec,ContHrsLab,Semester,Course,YearLevel,Section,SubjectSlot,RoomCategory FROM Subject_Tbl where Semester='"+ sem+"'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvSubject.DataSource = dt;
                        DataGridViewCheckBoxColumn dgvsat = new DataGridViewCheckBoxColumn();
                        dgvsat.ValueType = typeof(bool);
                        dgvsat.Name = "chkAdd";
                        dgvsat.HeaderText = "Add";
                        dgvSubject.Columns.Insert(12, dgvsat);
                        this.dgvSubject.Columns["SubjectName"].HeaderText = "Subject Name";
                        this.dgvSubject.Columns["YearLevel"].HeaderText = "Year Level";
                        this.dgvSubject.Columns["CredUnitLec"].Visible = false;
                        this.dgvSubject.Columns["CredUnitLab"].Visible = false;
                        this.dgvSubject.Columns["ContHrsLec"].Visible = false;
                        this.dgvSubject.Columns["ContHrsLab"].Visible = false;
                        this.dgvSubject.Columns["Course"].Visible = true;
                        this.dgvSubject.Columns["ID"].Visible = false;
                        this.dgvSubject.Columns["SubjectSlot"].HeaderText = "Subject Slot";
                        this.dgvSubject.Columns["RoomCategory"].Visible = false;
                        dgvSubject.Columns["SubjectCode"].ReadOnly = true;
                        dgvSubject.Columns["SubjectName"].ReadOnly = true;
                        dgvSubject.Columns["Semester"].ReadOnly = true;
                        dgvSubject.Columns["Course"].ReadOnly = true;
                        dgvSubject.Columns["YearLevel"].ReadOnly = true;
                        dgvSubject.Columns["Section"].ReadOnly = true;
                        dgvSubject.Columns["SubjectSlot"].ReadOnly = true;
                        dgvSubject.Columns["SubjectSlot"].Visible = true;
                        foreach (DataGridViewRow row in dgvSubject.Rows)
                        {

                            if (row.Cells["SubjectSlot"].Value.ToString() == "0")
                            {
                                CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dgvSubject.DataSource];
                                currencyManager1.SuspendBinding();
                                row.Visible = false;
                                currencyManager1.ResumeBinding();
                            }

                        }



                    }
                    else if (btnremove == true)
                    {
                     //   dgvSubject.DataSource = null;
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT distinct a.ID,a.FacultyCode,a.SubjectCode,b.SubjectName,b.Course,a.CredUnitLab,a.CredUnitLec,a.ContHrsLec,a.ContHrsLab,a.Course,b.YearLevel,a.Section,a.Semester,a.Room,a.RoomCategory,b.SubjectSlot  FROM Specialization_Tbl a JOIN Subject_Tbl b ON a.SubjectCode = b.SubjectCode WHERE a.FacultyCode='" + frmS.cbxFaculty.Text + "'  AND a.Course=b.Course", sqlcon);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvSubject.DataSource = dt;
                        DataGridViewCheckBoxColumn dgvsat = new DataGridViewCheckBoxColumn();
                        dgvsat.ValueType = typeof(bool);
                        dgvsat.Name = "chkAdd";
                        dgvsat.HeaderText = "Remove";
                        dgvSubject.Columns.Insert(16, dgvsat);

                        this.dgvSubject.Columns["SubjectName"].HeaderText = "Subject Name";
                        this.dgvSubject.Columns["YearLevel"].HeaderText = "Year Level";
                        this.dgvSubject.Columns["FacultyCode"].Visible = false;
                        this.dgvSubject.Columns["CredUnitLec"].Visible = false;
                        this.dgvSubject.Columns["CredUnitLab"].Visible = false;
                        this.dgvSubject.Columns["ContHrsLec"].Visible = false;
                        this.dgvSubject.Columns["ContHrsLab"].Visible = false;
                        this.dgvSubject.Columns["SubjectSlot"].Visible = false;
                        this.dgvSubject.Columns["RoomCategory"].Visible = false;
                        this.dgvSubject.Columns["Course"].Visible = false;
                        this.dgvSubject.Columns["Course1"].HeaderText = "Course";
                        this.dgvSubject.Columns["ID"].Visible = false;
                        this.dgvSubject.Columns["SubjectCode"].HeaderText = "Subject Code";

                        dgvSubject.Columns["SubjectCode"].ReadOnly = true;
                        dgvSubject.Columns["SubjectName"].ReadOnly = true;
                        dgvSubject.Columns["Semester"].ReadOnly = true;
                        dgvSubject.Columns["Course"].ReadOnly = true;
                        dgvSubject.Columns["YearLevel"].ReadOnly = true;
                        dgvSubject.Columns["Section"].ReadOnly = true;
                        dgvSubject.Columns["SubjectSlot"].ReadOnly = true;
                        dgvSubject.Columns["Room"].ReadOnly = true;
                    }
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    

        private void AddSubFrm_Load(object sender, EventArgs e)
        {
            if(btnadd == true)
            {
                rb1stsem.Checked = true;
            }
            
            try
            {
                Clock.Interval = 0200;
                Clock.Tick += new EventHandler(timer1_Tick);
                btnadd = true;
                AdminActivity();
                PopulateGridViewSubject();
                if (Subj == "Consultation Hours")
                {
                    rbContHrs.Checked = true;
                }
                else if (Subj == "Research And Extension")
                {
                    rbResearch.Checked = true;
                }
                else
                {
                    rbContHrs.Checked = false;
                    rbResearch.Checked = false;
                }
                panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
               panel1.Height, 20, 20));
                btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
               btnSave.Height, 30, 30));

                btnExit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnExit.Width,
             btnExit.Height, 30, 30));
                dgvSubject.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dgvSubject.Width,
          dgvSubject.Height, 5, 5));
                rbAdd.Checked = true;
                label1.Text = frmS.txtTotal.Text;
                if (frmS.txtFullTime.Text == "Full Time")
                {
                    hrslimiter = 31;
                }
                else
                {
                    hrslimiter = 18;
                }
                label1.ForeColor = Color.Black;
                if (dgvSubject.Rows.Count != 0)
                {
                    dgvSubject.Rows[0].Selected = false;
                }
                if(dgvSubject.Rows.Count != 0)
                {
                    dgvSubject.Rows[0].Selected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                string totalsub = "0";
                string room = "";
                string ContORRes = "";



                if ((Convert.ToInt32(label1.Text)) > hrslimiter)
                {
                    MessageBox.Show("You have exceeded the amount of hours. Please remove a subject to continue","Hours Exceeded",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {

                        sqlcon.Open();
                        string queryyyy = "SELECT SubjectCode FROM Specialization_Tbl  WHERE Room= '" + "" + "' AND FacultyCode=@FacultyCode";
                        SqlCommand commandyyy = new SqlCommand(queryyyy, sqlcon);
                        commandyyy.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                        SqlDataReader readeryyy = commandyyy.ExecuteReader();

                        if (readeryyy.Read() == true)
                        {


                            ContORRes = readeryyy["SubjectCode"].ToString();


                        }
                        readeryyy.Close();
                        string consultationhrs = "0";
                        string querycont = "SELECT COUNT(SubjectCode) AS ContHrs FROM Specialization_Tbl WHERE SubjectCode=@SubjectCode AND FacultyCode=@FacultyCode";
                        SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                        commandcont.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                        commandcont.Parameters.AddWithValue("@SubjectCode", "Consultation Hours");
                        SqlDataReader readercont = commandcont.ExecuteReader();

                        if (readercont.Read() == true)
                        {


                            consultationhrs = readercont["ContHrs"].ToString();


                        }
                        readercont.Close();
                        string researchAndExtension = "0";
                        string querycont1 = "SELECT COUNT(SubjectCode) AS ContHrs FROM Specialization_Tbl WHERE SubjectCode=@SubjectCode AND FacultyCode=@FacultyCode";
                        SqlCommand commandcont1 = new SqlCommand(querycont1, sqlcon);
                        commandcont1.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                        commandcont1.Parameters.AddWithValue("@SubjectCode", "Research And Extension");
                        SqlDataReader readercont1 = commandcont1.ExecuteReader();

                        if (readercont1.Read() == true)
                        {


                            researchAndExtension = readercont1["ContHrs"].ToString();


                        }
                        readercont1.Close();
                        sqlcon.Close();
                        int conthoursint = Convert.ToInt32(consultationhrs);
                        int research = Convert.ToInt32(researchAndExtension);
                        if (rbContHrs.Checked == true || rbResearch.Checked == true)
                        {
                            if (rbContHrs.Checked == true && numericUpDown1.Value == 0)
                            {
                                sqlcon.Open();
                                SqlCommand cmd0 = new SqlCommand("DELETE FROM Specialization_Tbl WHERE FacultyCode=@FacultyCode AND SubjectCode=@SubjectCode", sqlcon);
                                cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                                cmd0.Parameters.AddWithValue("@SubjectCode", "Consultation Hours");
                                cmd0.ExecuteNonQuery();
                                sqlcon.Close();
                                //    MessageBox.Show("Consultation hours removed.", "Specialization");
                            }
                            else if (rbResearch.Checked == true && numericUpDown1.Value == 0)
                            {
                                sqlcon.Open();
                                SqlCommand cmd0 = new SqlCommand("DELETE FROM Specialization_Tbl WHERE FacultyCode=@FacultyCode AND SubjectCode=@SubjectCode", sqlcon);
                                cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                                cmd0.Parameters.AddWithValue("@SubjectCode", "Research And Extension");
                                cmd0.ExecuteNonQuery();
                                sqlcon.Close();
                                //    MessageBox.Show("Research and extension removed.", "Specialization");
                            }
                        }
                        if (rbContHrs.Checked == true && numericUpDown1.Value > 0 && ContORRes == "") // for checking if the user has an existing consultation hours
                        {
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("INSERT INTO Specialization_Tbl(FacultyCode,SubjectCode,CredUnitLab,CredUnitLec,ContHrsLec,ContHrsLab,Section,Semester,Room,RoomCategory) VALUES (@FacultyCode,@SubjectCode,@CredUnitLab,@CredUnitLec,@ContHrsLec,@ContHrsLab,@Section,@Semester,@Room,@RoomCategory)", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Consultation Hours");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.Parameters.AddWithValue("@Section", "");
                            cmd0.Parameters.AddWithValue("@Semester", "");
                            cmd0.Parameters.AddWithValue("@Room", "");
                            cmd0.Parameters.AddWithValue("@RoomCategory", "");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //    MessageBox.Show("Consultation hours added.", "Specialization");
                        }
                        else if (rbContHrs.Checked == true && numericUpDown1.Value > 0 && ContORRes == "Research And Extension")
                        { // if the user has consultation hours then it will just update the existing
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("UPDATE Specialization_Tbl SET SubjectCode=@SubjectCode,CredUnitLab=@CredUnitLab,CredUnitLec=@CredUnitLec,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab WHERE FacultyCode=@FacultyCode AND Room=@Room", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Consultation Hours");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.Parameters.AddWithValue("@Room", "");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //    MessageBox.Show("Consultation hours updated.", "Specialization");

                        }
                        else if (rbContHrs.Checked == true && numericUpDown1.Value > 0 && ContORRes == "Consultation Hours")
                        { // if the user has consultation hours then it will just update the existing
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("UPDATE Specialization_Tbl SET CredUnitLab=@CredUnitLab,CredUnitLec=@CredUnitLec,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab WHERE FacultyCode=@FacultyCode AND SubjectCode=@SubjectCode", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Consultation Hours");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //   MessageBox.Show("Consultation hours updated.", "Specialization");

                        }

                        if (rbResearch.Checked == true && numericUpDown1.Value > 0 && ContORRes == "") // for checking if the user has an existing consultation hours
                        {
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("INSERT INTO Specialization_Tbl(FacultyCode,SubjectCode,CredUnitLab,CredUnitLec,ContHrsLec,ContHrsLab,Section,Semester,Room,RoomCategory) VALUES (@FacultyCode,@SubjectCode,@CredUnitLab,@CredUnitLec,@ContHrsLec,@ContHrsLab,@Section,@Semester,@Room,@RoomCategory)", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Research And Extension");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.Parameters.AddWithValue("@Section", "");
                            cmd0.Parameters.AddWithValue("@Semester", "");
                            cmd0.Parameters.AddWithValue("@Room", "");
                            cmd0.Parameters.AddWithValue("@RoomCategory", "");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //    MessageBox.Show("Research and extension added.", "Specialization");
                        }
                        else if (rbResearch.Checked == true && numericUpDown1.Value > 0 && ContORRes == "Consultation Hours")
                        { // if the user has consultation hours then it will just update the existing
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("UPDATE Specialization_Tbl SET SubjectCode=@SubjectCode,CredUnitLab=@CredUnitLab,CredUnitLec=@CredUnitLec,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab WHERE FacultyCode=@FacultyCode AND Room=@Room", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Research And Extension");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.Parameters.AddWithValue("@Room", "");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //    MessageBox.Show("Research and extension updated.", "Specialization");
                        }
                        else if (rbResearch.Checked == true && numericUpDown1.Value > 0 && ContORRes == "Research And Extension")
                        { // if the user has consultation hours then it will just update the existing
                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("UPDATE Specialization_Tbl SET CredUnitLab=@CredUnitLab,CredUnitLec=@CredUnitLec,ContHrsLec=@ContHrsLec,ContHrsLab=@ContHrsLab WHERE FacultyCode=@FacultyCode AND SubjectCode=@SubjectCode", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                            cmd0.Parameters.AddWithValue("@SubjectCode", "Research And Extension");
                            cmd0.Parameters.AddWithValue("@CredUnitLab", "0");
                            cmd0.Parameters.AddWithValue("@CredUnitLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLec", numericUpDown1.Value.ToString());
                            cmd0.Parameters.AddWithValue("@ContHrsLab", "0");
                            cmd0.ExecuteNonQuery();
                            sqlcon.Close();
                            //     MessageBox.Show("Research and extension updated.", "Specialization");
                        }


                        foreach (DataGridViewRow row in dgvSubject.Rows)
                        {

                            add = Convert.ToBoolean(row.Cells["chkAdd"].Value);


                            if (add && btnadd == true)
                            {



                                sqlcon.Open();



                                // ---------------------------------------------------------------- //


                                string totalroompercategSP = "0";
                                string query3 = "SELECT COUNT(Room) AS NumberOfRoom FROM Specialization_Tbl WHERE SubjectCode=@SubjectCode AND RoomCategory=@RoomCategory AND Course=@Course";
                                SqlCommand command3 = new SqlCommand(query3, sqlcon);
                                command3.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                command3.Parameters.AddWithValue("@RoomCategory", row.Cells["RoomCategory"].Value);
                                command3.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                SqlDataReader reader3 = command3.ExecuteReader();

                                if (reader3.Read() == true)
                                {


                                    totalroompercategSP = reader3["NumberOfRoom"].ToString();


                                }
                                reader3.Close();
                                string totalroompercategRoom = "0";
                                string query4 = "SELECT COUNT(Room) AS NumberOfRoom FROM Room_Tbl WHERE  RoomCategory=@RoomCategory AND Course=@Course";
                                SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                command4.Parameters.AddWithValue("@RoomCategory", row.Cells["RoomCategory"].Value);
                                command4.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                SqlDataReader reader4 = command4.ExecuteReader();

                                if (reader4.Read() == true)
                                {
                                    totalroompercategRoom = reader4["NumberOfRoom"].ToString();
                                }
                                reader4.Close();
                                int totals = Convert.ToInt32(totalroompercategSP) + 1;
                                //   MessageBox.Show(totalroompercategRoom);
                                int totalroomsSP = 0;
                                if (totals > Convert.ToInt32(totalroompercategRoom))
                                {
                                    totalroomsSP = Convert.ToInt32(totalroompercategSP) - Convert.ToInt32(totalroompercategRoom);
                                    if (totalroomsSP == 0)
                                    {

                                        totalroomsSP = Convert.ToInt32(totalroompercategRoom);
                                    }
                                    else if (Convert.ToInt32(totalroompercategRoom) == 1)
                                    {
                                        //   MessageBox.Show("asd");
                                        totalroomsSP = 1;
                                    }
                                    else if (totalroomsSP > Convert.ToInt32(totalroompercategRoom))
                                    {
                                        //   MessageBox.Show("asd");
                                        totalroomsSP = Convert.ToInt32(totalroompercategRoom);
                                    }
                                }

                                else
                                {
                                    totalroomsSP = Convert.ToInt32(totalroompercategSP) + 1;
                                    if (Convert.ToInt32(totalroompercategRoom) == 1)
                                    {
                                        totalroomsSP = 1;
                                    }
                                }


                                string query2 = "SELECT Room FROM Room_Tbl  WHERE RoomID='" + totalroomsSP.ToString() + "' AND RoomCategory ='" + row.Cells["RoomCategory"].Value.ToString() + "'AND Course = '" + row.Cells["Course"].Value.ToString() + "'";
                                SqlCommand command2 = new SqlCommand(query2, sqlcon);
                                SqlDataReader reader2 = command2.ExecuteReader();

                                if (reader2.Read() == true)
                                {


                                    room = reader2["Room"].ToString();


                                }
                                reader2.Close();


                                // ----------------------- // 


                                // ----------------------- // 

                                // 
                                SqlCommand cmd = new SqlCommand("INSERT INTO Specialization_Tbl(FacultyCode,SubjectCode,CredUnitLab,CredUnitLec,ContHrsLec,ContHrsLab,Course,Section,Semester,Room,RoomCategory) VALUES (@FacultyCode,@SubjectCode,@CredUnitLab,@CredUnitLec,@ContHrsLec,@ContHrsLab,@Course,@Section,@Semester,@Room,@RoomCategory)", sqlcon);
                                cmd.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                                cmd.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                cmd.Parameters.AddWithValue("@CredUnitLab", row.Cells["CredUnitLab"].Value);
                                cmd.Parameters.AddWithValue("@CredUnitLec", row.Cells["CredUnitLec"].Value);
                                cmd.Parameters.AddWithValue("@ContHrsLec", row.Cells["ContHrsLec"].Value);
                                cmd.Parameters.AddWithValue("@ContHrsLab", row.Cells["ContHrsLab"].Value);
                                cmd.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                cmd.Parameters.AddWithValue("@Section", row.Cells["Section"].Value);
                                cmd.Parameters.AddWithValue("@Semester", row.Cells["Semester"].Value);
                                cmd.Parameters.AddWithValue("@Room", room);
                                cmd.Parameters.AddWithValue("@RoomCategory", row.Cells["RoomCategory"].Value);
                                cmd.ExecuteNonQuery();
                                //        MessageBox.Show("Subject added to specialization","Specialization");
                                string query1 = "SELECT COUNT(Section) AS NumberOfSection FROM Specialization_Tbl WHERE SubjectCode=@SubjectCode AND Course = @Course";
                                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                command1.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                command1.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                SqlDataReader reader1 = command1.ExecuteReader();

                                if (reader1.Read() == true)
                                {


                                    totalsection = reader1["NumberOfSection"].ToString();


                                }
                                reader1.Close();
                                // ----------------------------------------------------------------------------- //

                                secslot = Convert.ToInt32(row.Cells["SubjectSlot"].Value.ToString());
                                int secslotminus = secslot - 1;
                                if (totalsection == "0")
                                {
                                    totalsection = "1";
                                }
                                int sectionsum = Convert.ToInt32(totalsection) + 1;


                                SqlCommand cmd1 = new SqlCommand("UPDATE Subject_Tbl SET Section=@Section, SubjectSlot=@SubjectSlot WHERE  SubjectCode=@SubjectCode AND Course=@Course", sqlcon);
                                cmd1.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                cmd1.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                cmd1.Parameters.AddWithValue("@SubjectSlot", secslotminus.ToString());
                                cmd1.Parameters.AddWithValue("@Section", sectionsum.ToString());
                                cmd1.ExecuteNonQuery();
                                sqlcon.Close();


                            }
                            else if (add && btnremove == true)
                            {
                                sqlcon.Open();
                                SqlCommand cmd = new SqlCommand("INSERT INTO SpTrash_Tbl(SubjectCode,Course) VALUES (@SubjectCode,@Course)", sqlcon);
                                cmd.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                cmd.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                cmd.ExecuteNonQuery();
                                string query3 = "SELECT COUNT(SubjectCode) AS NumberOfSubj FROM SpTrash_Tbl WHERE SubjectCode=@SubjectCode AND Course=@Course";
                                SqlCommand command3 = new SqlCommand(query3, sqlcon);
                                command3.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                command3.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                SqlDataReader reader3 = command3.ExecuteReader();

                                if (reader3.Read() == true)
                                {


                                    totalsub = reader3["NumberOfSubj"].ToString();


                                }
                                reader3.Close();

                                string section = "0";

                                string query2 = "SELECT Section FROM Subject_Tbl  WHERE SubjectCode='" + row.Cells["SubjectCode"].Value.ToString() + "' AND YearLevel ='" + row.Cells["YearLevel"].Value.ToString() + "' AND Course = '" + row.Cells["Course"].Value.ToString() + "'";
                                SqlCommand command2 = new SqlCommand(query2, sqlcon);
                                SqlDataReader reader2 = command2.ExecuteReader();

                                if (reader2.Read() == true)
                                {


                                    section = reader2["Section"].ToString();


                                }
                                reader2.Close();
                                // -------------------------------------------------- // 
                                // -------------------------------------------------- // 


                                secslot = Convert.ToInt32(row.Cells["SubjectSlot"].Value.ToString());
                                int secslotnum = secslot + Convert.ToInt32(totalsub);
                                SqlCommand cmd1 = new SqlCommand("UPDATE Subject_Tbl SET SubjectSlot=@SubjectSlot,Section=@Section WHERE  SubjectCode=@SubjectCode AND Course=@Course", sqlcon);
                                cmd1.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                cmd1.Parameters.AddWithValue("@SubjectSlot", secslotnum.ToString());
                                cmd1.Parameters.AddWithValue("@Course", row.Cells["Course"].Value);
                                if (Convert.ToInt32(row.Cells["SubjectSlot"].Value.ToString()) > Convert.ToInt32(section))
                                {
                                    cmd1.Parameters.AddWithValue("@Section", "1");
                                }
                                else if (Convert.ToInt32(row.Cells["Section"].Value.ToString()) > Convert.ToInt32(section))
                                {
                                    cmd1.Parameters.AddWithValue("@Section", section);
                                }
                                else
                                {
                                    cmd1.Parameters.AddWithValue("@Section", row.Cells["Section"].Value);
                                }
                                cmd1.ExecuteNonQuery();
                                SqlCommand cmddel = new SqlCommand("DELETE FROM Specialization_Tbl WHERE FacultyCode=@FacultyCode AND SubjectCode=@SubjectCode AND Section=@Section", sqlcon);
                                cmddel.Parameters.AddWithValue("@FacultyCode", frmS.cbxFaculty.Text);
                                cmddel.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value);
                                cmddel.Parameters.AddWithValue("@Section", row.Cells["Section"].Value);
                                cmddel.ExecuteNonQuery();


                                sqlcon.Close();
                                //     MessageBox.Show("Subject removed to specialization", "Specialization");
                            }


                        }

                        sqlcon.Open();

                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " added a subject for " + frmS.txtFName.Text);
                        cm.ExecuteNonQuery();

                    
                        SqlCommand cmdDelAll = new SqlCommand("TRUNCATE TABLE SpTrash_Tbl", sqlcon);
                        cmdDelAll.ExecuteNonQuery();
                        sqlcon.Close();
                        if(rbAdd.Checked == true)
                        {
                            MessageBox.Show("Subject Added", "Added",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                        else if (rbRemove.Checked == true)
                        {
                            MessageBox.Show("Subject Remove", "Removedd", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                 
                    hrs.Clear();
                    frmS.PopulateGridViewSpecializationt();
                    label1.Text = frmS.txtTotal.Text;
                    dgvSubject.Columns.Remove("chkAdd");
                    PopulateGridViewSubject();
                }
                if (numericUpDown1.Value == 0)
                {
                    rbContHrs.Checked = false;
                    rbResearch.Checked = false;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvSubject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                 ischecked = (bool)dgvSubject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue;
                
                CheckCount(ischecked);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Click on the checkbox only","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
           
           /* foreach (DataGridViewRow row in this.dgvSubject.Rows)
            {
                var cell = row.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;

                if (Convert.ToBoolean(cell.Value) == true)
                {
                    if (cell.State != DataGridViewElementStates.Selected)
                    {
                        someList.Add(row.Cells[1].Value.ToString());
                    }
                }
                else if (cell.State == DataGridViewElementStates.Selected)
                {
                    someList.Add(row.Cells[1].Value.ToString());
                }

            }
           */


        }
        private void CheckCount(bool isChecked)
        {
           
           
            if (isChecked)
            {
                counter++;
                count = counter;
                    dgvSubject.Enabled = false;
                    Clock.Start();
            }
            else
            {
                counter--;
                dgvSubject.Enabled = false;
                Clock.Start();
            }


        }

    
        private void btnUpdate_Click(object sender, EventArgs e)
        {
        }

        private void dgvSubject_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string hrsadd = "0";
                if (count == counter && ischecked == true)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();

                        string query6 = "select case when(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int))) IS NULL THEN 0 ELSE(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int)))  end AS Total from Subject_Tbl where SubjectCode='" + dgvSubject.Rows[e.RowIndex].Cells["SubjectCode"].Value.ToString() + "' AND Course='" + "BSIT" + "'";
                        SqlCommand command6 = new SqlCommand(query6, sqlcon);
                        SqlDataReader reader6 = command6.ExecuteReader();

                        if (reader6.Read() == true)
                        {


                            hrsadd = reader6["Total"].ToString();

                        }

                        hrs.Add(Convert.ToInt32(hrsadd));
                        reader6.Close();
                        sqlcon.Close();
                      //  MessageBox.Show("1");
                    }
                }
                else if (count > counter && ischecked == false && hrs.Count != 0)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();

                        string query6 = "select case when(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int))) IS NULL THEN 0 ELSE(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int)))  end AS Total from Subject_Tbl where SubjectCode='" + dgvSubject.Rows[e.RowIndex].Cells["SubjectCode"].Value.ToString() + "' AND Course='" + dgvSubject.Rows[e.RowIndex].Cells["Course"].Value.ToString() + "'";
                        SqlCommand command6 = new SqlCommand(query6, sqlcon);
                        SqlDataReader reader6 = command6.ExecuteReader();

                        if (reader6.Read() == true)
                        {


                            hrsadd = reader6["Total"].ToString();

                        }
                        count--;
                        hrs.Remove(Convert.ToInt32(hrsadd));
                        reader6.Close();
                        sqlcon.Close();
                    //    MessageBox.Show("2");

                    }
                }

                int sum = 0;
                if (btnadd == true)
                {
                    sum = hrs.Sum(x => Convert.ToInt32(x)) + Convert.ToInt32(frmS.txtTotal.Text);

                }
                else if (btnremove == true)
                {
                    sum = Convert.ToInt32(frmS.txtTotal.Text) - hrs.Sum(x => Convert.ToInt32(x));

                }
                label1.Text = sum.ToString();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvSubject_Click(object sender, EventArgs e)
        {
            

          
            
        }

        private void dgvSubject_MouseUp(object sender, MouseEventArgs e)
        {
            dgvSubject.EndEdit();
          
        }

        private void rbAdd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox3.Visible = true;
                rb1stsem.Checked = true;
                frmS.PopulateGridViewSpecializationt();
                btnremove = false;
                btnadd = true;
                dgvSubject.Columns.Remove("chkAdd");
                PopulateGridViewSubject();
                if(dgvSubject.Rows.Count != 0)
                {
                    dgvSubject.Rows[0].Selected = false;
                }
                counter = 0;
                label1.Text = frmS.txtTotal.Text;
                hrs.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbRemove_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                groupBox3.Visible = false;
                frmS.PopulateGridViewSpecializationt();
                btnremove = true;
                btnadd = false;
                dgvSubject.Columns.Remove("chkAdd");
                PopulateGridViewSubject();
                if (dgvSubject.Rows.Count != 0)
                {
                    dgvSubject.Rows[0].Selected = false;
                }
                counter = 0;
                label1.Text = frmS.txtTotal.Text;
                hrs.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(label1.Text) > hrslimiter)
            {
                label1.ForeColor = Color.Red;
            }
            else if (Convert.ToInt32(label1.Text) <= hrslimiter)
            {
                label1.ForeColor = Color.Black;
            }
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(label1.Text) == hrslimiter)
            {
                this.Close();
            }
            else if (label1.Text == "0")
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("You still have available hours to take subjects.");
            }
        }
        int[] values = { 0, 0 };
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                values[0] = values[1];
                values[1] = Convert.ToInt32(numericUpDown1.Value);

                if (values[0] < values[1])
                {

                    numericvalue = Convert.ToInt32(label1.Text);
                    numericvalue += 1;
                    label1.Text = numericvalue.ToString();
                    num = 1;
                    hrs.Add(num);


                }
                else if (values[0] > values[1])
                {
                    numericvalue = Convert.ToInt32(label1.Text);
                    numericvalue -= 1;
                    label1.Text = numericvalue.ToString();
                    num = 1;
                    hrs.Add(num);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
          
            if(frmS.dataGridView1.Rows.Count == 0)
            {
                frmS.btnPlot.Enabled = false;
            }
            this.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(rbContHrs.Checked == true)
            {
                numericUpDown1.Enabled = true;

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sender == Clock)
            {
                dgvSubject.Enabled = true;
                Clock.Stop();
             
            }
        }

        private void rbResearch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbResearch.Checked == true)
            {
                numericUpDown1.Enabled = true;

            }
        }

        private void dgvSubject_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rb1stsem_CheckedChanged(object sender, EventArgs e)
        {
            dgvSubject.Columns.Remove("chkAdd");
            PopulateGridViewSubject();
        }

        private void rb2ndsem_CheckedChanged(object sender, EventArgs e)
        {
            dgvSubject.Columns.Remove("chkAdd");
            PopulateGridViewSubject();
        }
    }
}
