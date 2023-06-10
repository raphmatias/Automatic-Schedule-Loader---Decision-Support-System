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

    public partial class DefSchedFrm : Form
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
        int counter = 0;
        int total = 0;
        int totalhrs = 0;
        string strtotalhrs = "0";
      public  List<String> timedayid = new List<String>(); // for storing the time and day id of each schedule
        public List<String> sched = new List<String>(); // for storing the number of loop subjects base on the number of hours per subject
        public List<String> time = new List<String>(); // for storing the selected time 
        public List<String> day = new List<String>(); // for storing the selected day 
        public List<String> timeLB = new List<String>(); // for storing the selected time 
        public List<String> dayLB = new List<String>(); // for storing the selected day 
        public List<String> subject = new List<String>(); // for storing and determining the subjects inside datagridview
        public List<String> classtype = new List<String>(); // for type of class ex: lecture or laboratory class
        public List<String> room = new List<String>(); // for storing room of subject
        public List<String> section = new List<String>(); // for storing section of subject
        public List<String> semester = new List<String>(); // for storing section of subject
        public List<String> crs = new List<String>(); // for storing section of course
        public List<int> hrs = new List<int>(); // for subject hours
        public List<string> roomcategory = new List<string>(); // for room categ
        public int TotalHrsPerSubj = 0;
        public int subjHrs = 0;
        public int credhrs = 0;
        public int rowcount = 0;
        public int cellcount = 5;
        public int count = 0;
        int dgvrows = 0;
        int counterindex = 0;
        string SchedDuplicateForRoom = "0";
        string SchedDuplicateForSection = "0";
        bool btnplot = false;
        string loginAct = "";
        string typeofAcc = "";
        bool check = true;
        bool cancel = false;
        public DefSchedFrm()
        {

            InitializeComponent();
           // this.FormBorderStyle = FormBorderStyle.None;
           // Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        
            
        }

    
        void PopulateCBXFaculty()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT FacultyCode,FacultyName FROM Faculty_Tbl", sqlcon);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cbxFaculty.ValueMember = "FacultyName";
                cbxFaculty.DisplayMember = "FacultyCode";

                cbxFaculty.DataSource = dt;


            }
        }
        public void PopulateGridViewSpecializationt() // filter gridview faculty by code
        {
            strtotalhrs = "0";
        //   try
            {

            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Specialization_Tbl  WHERE  FacultyCode = '" + cbxFaculty.Text + "'", sqlcon);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                    this.dataGridView1.Columns["FacultyCode"].Visible = false;
                    this.dataGridView1.Columns["CredUnitLec"].Visible = false;
                    this.dataGridView1.Columns["CredUnitLab"].Visible = false;
                    this.dataGridView1.Columns["ContHrsLec"].Visible = false;
                    this.dataGridView1.Columns["ContHrsLab"].Visible = false;
                    this.dataGridView1.Columns["RoomCategory"].Visible = false;
                    this.dataGridView1.Columns["ID"].Visible = false;
                    this.dataGridView1.Columns["Semester"].Visible = false;
                    this.dataGridView1.Columns["SubjectCode"].HeaderText = "Subject Code";
                    this.dataGridView1.Columns["SubjectCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dataGridView1.Columns["Course"].Width = 100;
                    this.dataGridView1.Columns["Section"].Width = 100;
                    this.dataGridView1.Columns["Room"].Width = 100;

                    dataGridView1.Columns["SubjectCode"].ReadOnly = true;
                    dataGridView1.Columns["Course"].ReadOnly = true;
                    dataGridView1.Columns["Section"].ReadOnly = true;
                    dataGridView1.Columns["Room"].ReadOnly = true;
                    SqlDataAdapter adapterTime = new SqlDataAdapter("SELECT a.ID,a.FacultyCode,c.Day,b.Time,a.SubjectCode,a.ClassType,a.Room,a.Section,a.Semester,a.Course,a.RoomCategory FROM FacultySchedule_Tbl a, Time_Tbl b,Day_Tbl c WHERE FacultyCode ='" + cbxFaculty.Text + "' AND a.TimeID = b.TimeID AND a.DayID = c.DayID", sqlcon);
                    DataTable dtTime = new DataTable();
                    adapterTime.Fill(dtTime);
                    dataGridView3.DataSource = dtTime;
                    dataGridView3.AllowUserToAddRows = false;
                    dataGridView3.Columns["SubjectCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                 
                    this.dataGridView3.Columns["FacultyCode"].Visible = false;
                    this.dataGridView3.Columns["ID"].Visible = false;
                    this.dataGridView3.Columns["Section"].Visible = false;
                    this.dataGridView3.Columns["Room"].Visible = false;
                    this.dataGridView3.Columns["Semester"].Visible = false;
                    this.dataGridView3.Columns["RoomCategory"].Visible = false;

                    //--------------------------------------------------------- //

                    //--------------------------------------------------------- //

                    string query1 = "select FacultyCode,FacultyName,FullTime FROM Faculty_Tbl WHERE FacultyCode='" + cbxFaculty.Text + "'";
                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                SqlDataReader reader1 = command1.ExecuteReader();

                if (reader1.Read() == true)
                {


                    txtFacultyCode.Text = reader1["FacultyCode"].ToString();
                    txtFName.Text = reader1["FacultyName"].ToString();
                        txtFullTime.Text = reader1["FullTime"].ToString();

                    }
                reader1.Close();
                    if(txtFullTime.Text == "1")
                    {
                        txtFullTime.Text = "Full Time";
                    }
                    else
                    {
                        txtFullTime.Text = "Part Time";
                    }
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {



                    string query2 = "select sum(cast(CredUnitLec as int)) TotalCredUnitLec from Specialization_Tbl WHERE FacultyCode='" + row.Cells["FacultyCode"].Value + "'";
                    SqlCommand command2 = new SqlCommand(query2, sqlcon);
                    SqlDataReader reader2 = command2.ExecuteReader();

                    if (reader2.Read() == true)
                    {


                        txtTotalLec.Text = reader2["TotalCredUnitLec"].ToString();

                    }
                    reader2.Close();

                    string query3 = "select sum(cast(CredUnitLab as int)) TotalCredUnitLab from Specialization_Tbl where FacultyCode='" + row.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand command3 = new SqlCommand(query3, sqlcon);
                    SqlDataReader reader3 = command3.ExecuteReader();

                    if (reader3.Read() == true)
                    {


                        txtTotalLab.Text = reader3["TotalCredUnitLab"].ToString();

                    }
                    reader3.Close();
                    string query4 = "select sum(cast(ContHrsLec as int)) TotalContHrsLec from Specialization_Tbl where FacultyCode='" + row.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand command4 = new SqlCommand(query4, sqlcon);
                    SqlDataReader reader4 = command4.ExecuteReader();

                    if (reader4.Read() == true)
                    {


                        txtTotalHrsLec.Text = reader4["TotalContHrsLec"].ToString();

                    }
                    // string txtconnthrslab = "0";
                    reader4.Close();
                    string query5 = "select sum(cast(ContHrsLab as int)) TotalContHrsLab from Specialization_Tbl where FacultyCode='" + row.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand command5 = new SqlCommand(query5, sqlcon);
                    SqlDataReader reader5 = command5.ExecuteReader();

                    if (reader5.Read() == true)
                    {


                        txtContHrsLab.Text = reader5["TotalContHrsLab"].ToString();

                    }

                    reader5.Close();

                    string query6 = "select(case when(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int))) IS NULL THEN 0 ELSE(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int)))  end) AS Total from Specialization_Tbl where FacultyCode = '" + row.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand command6 = new SqlCommand(query6, sqlcon);
                    SqlDataReader reader6 = command6.ExecuteReader();

                    if (reader6.Read() == true)
                    {


                        txtTotal.Text = reader6["Total"].ToString();

                    }

                    reader6.Close();
                    total = Convert.ToInt32(txtTotal.Text);

                }
                sqlcon.Close();
            }
            if (total != 0)
            {
                btnPlot.Enabled = true;
                btnArchive.Enabled = true;
            }
            if (dataGridView1.Rows.Count == 0)
            {
                txtTotal.Text = "0";
                txtTotalHrsLec.Text = "0";
                txtTotalLab.Text = "0";
                txtTotalLec.Text = "0";
                txtContHrsLab.Text = "0";

            }




        }
     /*   catch (ArgumentOutOfRangeException)
        {
            label26.Text = "Specialization Required";

            label22.Text = "0";
            label23.Text = "0";
            label24.Text = "0";
            if (total == 0)
            {
                btnPlot.Enabled = false;
                btnSave.Enabled = true;
                btnArchive.Enabled = false;
                btnUpdate.Enabled = false;
            }


  }
        */
    }

        public void AdminActivity()
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


        private void DefSchedFrm_Load(object sender, EventArgs e)
        {
       
            // TODO: This line of code loads data into the 'scheduleLoaderDBDataSet3.Day_Tbl' table. You can move, or remove it, as needed.
           // this.day_TblTableAdapter.Fill(this.scheduleLoaderDBDataSet3.Day_Tbl);
            // TODO: This line of code loads data into the 'scheduleLoaderDBDataSet2.Time_Tbl' table. You can move, or remove it, as needed.
           
            this.time_TblTableAdapter.Fill(this.scheduleLoaderDBDataSet2.Time_Tbl);

            if (dataGridView1.Rows.Count == 0)
            {
                txtTotal.Text = "0";
                TotalHrsPerSubj = 0;
                subjHrs = 0;
                credhrs = 0;
                rowcount = 0;
                cellcount = 5;
                count = 0;
                btnPlot.Enabled = false;
                btnArchive.Enabled = false;
                //  groupBox13.Visible = false;
                foreach (Control c in Controls)
                {
                    if (c is CheckBox)
                    {
                        CheckBox cb = (CheckBox)c;
                        cb.Enabled = false;
                        // cb.Checked = false;
                    }
                }
                btnSave.Enabled = false;
                btnUpdate.Enabled = false;
                sched.Clear();
                subject.Clear();
                hrs.Clear();
                time.Clear();
                day.Clear();
                classtype.Clear();
                timedayid.Clear();
                room.Clear();
                section.Clear();
                listBox1.DataSource = null;
                listBox2.DataSource = null;
                listBox3.DataSource = null;
                listBox4.DataSource = null;
            }
            AdminActivity();
            PopulateCBXFaculty();
            PopulateGridViewSpecializationt();
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
            // panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width,
            //  panel2.Height, 20, 20));
            panel6.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel6.Width,
         panel6.Height, 5, 5));
            panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
        panel1.Height, 5, 5));
            panel7.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width,
      panel7.Height, 5, 5));
          
            panel4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width,
  panel4.Height, 5, 5));
            panel5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel5.Width,
  panel5.Height, 5, 5));
            /*
            txtContHrsLab.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtContHrsLab.Width,
txtContHrsLab.Height, 15, 15));
            txtFacultyCode.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtFacultyCode.Width,
txtFacultyCode.Height, 15, 15));
            txtFullTime.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtFullTime.Width,
txtFullTime.Height, 15, 15));
            txtTotal.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtTotal.Width,
txtTotal.Height, 15, 15));
            txtTotalHrsLec.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtTotalHrsLec.Width,
txtTotalHrsLec.Height, 15, 15));
            txtTotalLab.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtTotalLab.Width,
txtTotalLab.Height, 15, 15));
            txtTotalLec.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtTotalLec.Width,
txtTotalLec.Height, 15, 15));
            txtFName.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtFName.Width,
txtFName.Height, 15, 15));
            cbxFaculty.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cbxFaculty.Width,
cbxFaculty.Height, 15, 15));
            */
            btnPlotExit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPlotExit.Width,
        btnPlotExit.Height, 40, 40));
            btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
           btnSave.Height, 30, 30));
            btnArchive.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchive.Width,
        btnArchive.Height, 30, 30));
            btnUpdate.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnUpdate.Width,
          btnUpdate.Height, 30, 30));
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
         btnClose.Height, 30, 30));
            btnPlot.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPlot.Width,
btnPlot.Height, 30, 30));
            btnAddSub.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnAddSub.Width,
btnAddSub.Height, 30, 30));
            dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
dataGridView1.Height, 5, 5));
            dataGridView3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView3.Width,
dataGridView3.Height, 5, 5));

            listBox2.DataSource = null;
            listBox3.DataSource = null;
            // setSubforSP();



        }
      
        private void btnAddSub_Click(object sender, EventArgs e)
        {
            cbCheckSched.Checked = false;
            cbCheckSched.Visible = false;
            TotalHrsPerSubj = 0;
            subjHrs = 0;
            credhrs = 0;
            rowcount = 0;
            cellcount = 5;
            count = 0;
            btnPlot.Enabled = true;
            btnPlotExit.Visible = false;
            //  groupBox13.Visible = false;
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    cb.Enabled = false;
                    cb.Checked = false;
                }
            }
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            sched.Clear();
            subject.Clear();
            hrs.Clear();
            time.Clear();
            day.Clear();
            classtype.Clear();
            timedayid.Clear();
            section.Clear();
            room.Clear();
            dayLB.Clear();
            timeLB.Clear();
            crs.Clear();
            roomcategory.Clear();
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;
            listBox4.DataSource = null;
            listBox5.DataSource = null;
            listBox6.DataSource = null;
            listBox7.DataSource = null;

            AddSubFrm asf = new AddSubFrm(this);
            asf.ShowDialog();
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnPlotExit.Visible = false;
            cbCheckSched.Checked = false;
            cbCheckSched.Visible = false;
           
           using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                if(!(lblcounter.Text.Equals(txtTotal.Text)) )
                {
                    MessageBox.Show("You have number of hours left to plot","Available Hours",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
                else
                {
                    for (int i = 0; i < total; i++)
                    {
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand("FacultySchedAddOrEdit", sqlcon);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", 0);
                        cmd.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                        cmd.Parameters.AddWithValue("@DayID", day[i]);
                        cmd.Parameters.AddWithValue("@TimeID", time[i]);
                        cmd.Parameters.AddWithValue("@SubjectCode", sched[i]);
                        cmd.Parameters.AddWithValue("@Section", section[i]);
                        cmd.Parameters.AddWithValue("@Semester", semester[i]);
                        cmd.Parameters.AddWithValue("@Course", crs[i]);
                        cmd.Parameters.AddWithValue("@Room", room[i]);
                        cmd.Parameters.AddWithValue("@ClassType", classtype[i]);
                        cmd.Parameters.AddWithValue("@RoomCategory", roomcategory[i]);
                        cmd.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    MessageBox.Show("Succesfully Saved","Schedule Saved", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    sqlcon.Open();

                    DateTime time1 = DateTime.Now;
                    string format = "yyyy-MM-dd";
                    SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time1.ToString(format) + "')", sqlcon);
                    cm.Parameters.AddWithValue("@Username", loginAct);
                    cm.Parameters.AddWithValue("@ActivityLog", loginAct + " created a shedule for " + txtFName.Text);
                    cm.ExecuteNonQuery();
                   
                  
                    sqlcon.Close();
                }

                PopulateGridViewSpecializationt();
               
                btnSave.Enabled = false;

            }
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    cb.Enabled = false;
                    cb.Checked = false;
                }
            }
            sched.Clear();
            subject.Clear();
            hrs.Clear();
            time.Clear();
            day.Clear();
            classtype.Clear();
            timedayid.Clear();
            section.Clear();
            room.Clear();
            dayLB.Clear();
            timeLB.Clear();
            crs.Clear();
            roomcategory.Clear();
            check = true;
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;
            listBox4.DataSource = null;
            listBox5.DataSource = null;
            listBox6.DataSource = null;
            listBox7.DataSource = null;
            btnPlot.Text = "Create New Time Schedule";
            
           
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
     
        private void cbxFaculty_TextChanged(object sender, EventArgs e)
        {


            PopulateGridViewSpecializationt();
      

        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
           try
            {
                cbCheckSched.Visible = true;
                btnplot = true;
                btnPlotExit.Visible = true;
            //    dataGridView3.Visible = true;
                dgvrows = dataGridView1.RowCount - 1;
                if (dataGridView1.Rows.Count == 0)
                {
                    btnPlot.Enabled = false;
                    btnSave.Enabled = false;
                    btnUpdate.Enabled = false;
                    foreach (Control c in Controls)
                    {
                        if (c is CheckBox)
                        {
                            CheckBox cb = (CheckBox)c;
                            cb.Checked = false;
                        }
                    }
                }
                else
                {


                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        string conthrs = "0";
                        btnPlot.Enabled = false;
                        sqlcon.Open();
                        string query0 = "select ContHrsLec FROM Specialization_Tbl  WHERE FacultyCode='" + cbxFaculty.Text + "' AND SubjectCode ='" + "Consultation Hours" + "' OR SubjectCode='" + "Research And Extension" + "'";
                        SqlCommand command0 = new SqlCommand(query0, sqlcon);
                        SqlDataReader reader0 = command0.ExecuteReader();

                        if (reader0.Read() == true)
                        {



                            conthrs = reader0["ContHrsLec"].ToString();


                        }
                        reader0.Close();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {

                            string query7 = "select case when(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int))) IS NULL THEN 0 ELSE(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int)))  end AS Total from Subject_Tbl where SubjectCode = @SubjectCode AND Course=@Course";
                            SqlCommand command7 = new SqlCommand(query7, sqlcon);
                            command7.Parameters.AddWithValue("@SubjectCode", row.Cells["SubjectCode"].Value.ToString());
                            command7.Parameters.AddWithValue("@Course", row.Cells["Course"].Value.ToString());
                            SqlDataReader reader7 = command7.ExecuteReader();
                            if (reader7.Read() == true)
                            {
                                strtotalhrs = reader7["Total"].ToString();
                            }
                            reader7.Close();
                            totalhrs = Convert.ToInt32(strtotalhrs);
                            if ((row.Cells["SubjectCode"].Value.ToString() == "Consultation Hours" || row.Cells["SubjectCode"].Value.ToString() == "Research And Extension") && totalhrs == 0)
                            {
                                totalhrs = Convert.ToInt32(conthrs);
                            }

                            hrs.Add(totalhrs);




                        }
                    }

                    TotalHrsPerSubj = 0;
                    subjHrs = 0;
                    credhrs = 0;
                    rowcount = 0;
                    cellcount = 5;
                    count = 0;
                    string[] typeofclass = { "Lecture", "Laboratory" };
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        subject.Add(row.Cells["SubjectCode"].Value.ToString());

                    }
                    bool contains = false;
                    for (int i = 0; i < total; i++)
                    {


                        if (subjHrs <= hrs[TotalHrsPerSubj] && TotalHrsPerSubj < dataGridView1.Rows.Count)
                        {
                            for (int x = 0; x < dataGridView1.Rows.Count; x++)
                            {
                                contains = false;
                                for (int z = 0; z < dataGridView3.Rows.Count; z++)
                                {
                                    if (dataGridView3.Rows[z].Cells["SubjectCode"].Value.ToString().Contains(subject[x]))
                                    {
                                        contains = true;
                                    }
                                    if (contains == true)
                                    {
                                        break;
                                    }
                                }
                                if (contains == false)
                                {
                                    break;
                                }
                            }
                            if (contains == false || dataGridView3.Rows.Count != total)
                            {
                                sched.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["SubjectCode"].Value.ToString());
                                section.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["Section"].Value.ToString());
                                room.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["Room"].Value.ToString());
                                semester.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["Semester"].Value.ToString());
                                crs.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["Course"].Value.ToString());
                                roomcategory.Add(dataGridView1.Rows[TotalHrsPerSubj].Cells["RoomCategory"].Value.ToString());
                                cbCheckSched.Enabled = false;
                            }
                            else
                            {
                                sched.Add(dataGridView3.Rows[i].Cells["SubjectCode"].Value.ToString());
                                crs.Add(dataGridView3.Rows[i].Cells["Course"].Value.ToString());
                                section.Add(dataGridView3.Rows[i].Cells["Section"].Value.ToString());
                                room.Add(dataGridView3.Rows[i].Cells["Room"].Value.ToString());
                                semester.Add(dataGridView3.Rows[i].Cells["Semester"].Value.ToString());
                                roomcategory.Add(dataGridView3.Rows[i].Cells["RoomCategory"].Value.ToString());
                                cbCheckSched.Enabled = true;
                            }
                            subjHrs++;
                        }

                        if (subjHrs == hrs[TotalHrsPerSubj] && TotalHrsPerSubj < dataGridView1.Rows.Count)
                        {
                            TotalHrsPerSubj++;
                            subjHrs = 0;
                            if (TotalHrsPerSubj == dataGridView1.Rows.Count)
                            {
                                TotalHrsPerSubj -= 1;
                                subjHrs = 0;
                            }

                        }



                        //   MessageBox.Show(rowcount.ToString() + " " + cellcount.ToString() + " "+ sched[i]  );


                    }
                    if (contains == false || dataGridView3.Rows.Count != total)
                    {
                        bool loop = false;
                        bool forloop = true;
                        for (int i = 0; i < total; i++)
                        {
                            do
                            {
                                if (credhrs < (Convert.ToInt32(dataGridView1.Rows[rowcount].Cells[cellcount].Value))) // 0 4
                                {

                                    if (sched[i] == "Consultation Hours" || sched[i] == "Research And Extension")
                                    {
                                        classtype.Add("N/A");
                                        credhrs++;
                                    }
                                    else
                                    {
                                        classtype.Add(typeofclass[count]);
                            
                                        credhrs++;
                                    }
                                    break;

                                }
                                if (cellcount == 5 && credhrs == Convert.ToInt32(dataGridView1.Rows[rowcount].Cells[cellcount].Value))
                                {
                                    credhrs = 0;
                                    cellcount += 1;
                                    count = 1;
                                    loop = true;
                                }
                                if (cellcount == 6 && credhrs == Convert.ToInt32(dataGridView1.Rows[rowcount].Cells[cellcount].Value))
                                {
                                    rowcount++;
                                    cellcount = 5;
                                    credhrs = 0;
                                    count = 0;
                                    loop = true;

                                    if (rowcount == dataGridView1.Rows.Count)
                                    {

                                        forloop = false;
                                        break;

                                    }
                                }
                            } while (loop == true);
                            if (forloop == false)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dataGridView3.Rows.Count; i++)
                        {
                            classtype.Add(dataGridView3.Rows[i].Cells["ClassType"].Value.ToString());
                        }
                    }

                    if (dataGridView3.Rows.Count == 0)
                    {
                        btnSave.Enabled = true;
                        btnUpdate.Enabled = false;
                        btnArchive.Enabled = false;
                        foreach (Control c in Controls)
                        {
                            if (c is CheckBox)
                            {
                                CheckBox cb = (CheckBox)c;
                                cb.Enabled = true;
                            }
                        }
                    }

                    else if (dataGridView3.Rows.Count != 0)
                    {
                        btnSave.Enabled = false;
                        btnArchive.Enabled = true;
                        btnUpdate.Enabled = true;

                        foreach (Control c in Controls)
                        {
                            if (c is CheckBox)
                            {
                                CheckBox cb = (CheckBox)c;
                                cb.Enabled = true;

                            }
                        }



                    }
                    listBox1.DataSource = sched;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = dayLB;
                    listBox4.DataSource = classtype;
                    listBox5.DataSource = crs;
                    listBox6.DataSource = section;
                    listBox7.DataSource = room;

                    listBox1.SelectedIndex = -1;
                    listBox4.SelectedIndex = -1;
                    listBox5.SelectedIndex = - 1;
                    listBox6.SelectedIndex = -1;
                    listBox7.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        void checkboxCheckValue()
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {

                    // Monday

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox1.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox2.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox3.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox4.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox5.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox6.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox7.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox8.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox9.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox10.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox11.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox12.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox13.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Monday"))
                    {

                        checkBox79.Checked = true;
                    }

                    // Tuesday 

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox26.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox25.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox24.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox23.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox22.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox21.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox20.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox19.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox18.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox17.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox16.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox15.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox14.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Tuesday"))
                    {

                        checkBox80.Checked = true;
                    }

                    // Wednesday 

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox39.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox38.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox37.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox36.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox35.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox34.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox33.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox32.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox31.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox30.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox29.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox28.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox27.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Wednesday"))
                    {

                        checkBox81.Checked = true;
                    }

                    // Thursday

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox52.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox51.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox50.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox49.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox48.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox47.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox46.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox45.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox44.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox43.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox42.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox41.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox40.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Thursday"))
                    {

                        checkBox82.Checked = true;
                    }
                    // Friday

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox65.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox64.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox63.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox62.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox61.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox60.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox59.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox58.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox57.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox56.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox55.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox54.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox53.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Friday"))
                    {

                        checkBox83.Checked = true;
                    }
                    // Saturday

                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 AM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox78.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 AM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox77.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("9:00 - 10:00 AM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox76.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("10:00 - 11:00 AM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox75.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("11:00 - 12:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox74.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("12:00 - 1:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox73.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("1:00 - 2:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox72.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("2:00 - 3:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox71.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("3:00 - 4:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox70.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("4:00 - 5:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox69.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("5:00 - 6:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox68.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("6:00 - 7:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox67.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("7:00 - 8:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox66.Checked = true;
                    }
                    if (row.Cells["Time"].Value.ToString().Equals("8:00 - 9:00 PM") && row.Cells["Day"].Value.ToString().Equals("Saturday"))
                    {

                        checkBox84.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cbxFaculty_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    dataGridView1.Rows[0].Selected = false;
                }
                btnplot = false;
                btnPlotExit.Visible = false;
                cbCheckSched.Visible = false;
                if (dataGridView1.Rows.Count == 0 && dataGridView3.Rows.Count == 0)
                {
                    txtTotal.Text = "0";
                    txtTotalHrsLec.Text = "0";
                    txtTotalLab.Text = "0";
                    txtTotalLec.Text = "0";
                    txtContHrsLab.Text = "0";
                    btnPlot.Text = "Plot Time Schedule";
                    btnPlot.Enabled = false;
                    btnArchive.Enabled = false;
                }
                else if (dataGridView1.Rows.Count == 0 && dataGridView3.Rows.Count != 0)
                {
                    txtTotal.Text = "0";
                    txtTotalHrsLec.Text = "0";
                    txtTotalLab.Text = "0";
                    txtTotalLec.Text = "0";
                    txtContHrsLab.Text = "0";
                    btnPlot.Text = "Plot Time Schedule";
                    btnPlot.Enabled = false;
                    btnArchive.Enabled = true;
                }
                else if (dataGridView3.Rows.Count != 0 && dataGridView1.Rows.Count != 0)
                {

                    btnPlot.Text = "Create New Time Schedule";
                    btnPlot.Enabled = true;
                    btnArchive.Enabled = true;
                }
                else if (dataGridView3.Rows.Count == 0 && dataGridView1.Rows.Count != 0)
                {

                    btnPlot.Text = "Plot Time Schedule";
                    btnPlot.Enabled = true;
                    btnArchive.Enabled = false;
                }
                foreach (Control c in Controls)
                {
                    if (c is CheckBox)
                    {
                        CheckBox cb = (CheckBox)c;
                        cb.Checked = false;
                        cb.Enabled = false;
                    }
                }
                TotalHrsPerSubj = 0;
                subjHrs = 0;
                credhrs = 0;
                rowcount = 0;
                cellcount = 5;
                count = 0;
                btnSave.Enabled = false;
                //   btnPlot.Enabled = true;
                btnUpdate.Enabled = false;
                sched.Clear();
                subject.Clear();
                hrs.Clear();
                time.Clear();
                day.Clear();
                classtype.Clear();
                timedayid.Clear();
                section.Clear();
                room.Clear();
                dayLB.Clear();
                timeLB.Clear();
                crs.Clear();
                roomcategory.Clear();
                check = true;


                listBox1.DataSource = null;
                listBox2.DataSource = null;
                listBox3.DataSource = null;
                listBox4.DataSource = null;
                listBox5.DataSource = null;
                listBox6.DataSource = null;
                listBox7.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
         //   try
         //   {
                

                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    if (total != dataGridView3.Rows.Count)
                    {
                        if (counter != total)
                        {
                            MessageBox.Show("You have number of hours left to plot","Available Hours",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        }
                        else
                        {

                            sqlcon.Open();
                            SqlCommand cmd0 = new SqlCommand("DELETE FROM FacultySchedule_Tbl WHERE FacultyCode=@FacultyCode", sqlcon);
                            cmd0.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                            cmd0.ExecuteNonQuery();

                            for (int i = 0; i < total; i++)
                            {

                                SqlCommand cmd = new SqlCommand("FacultySchedAddOrEdit", sqlcon);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ID", 0);
                                cmd.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                                cmd.Parameters.AddWithValue("@DayID", day[i]);
                                cmd.Parameters.AddWithValue("@TimeID", time[i]);
                                cmd.Parameters.AddWithValue("@SubjectCode", sched[i]);
                                cmd.Parameters.AddWithValue("@Section", section[i]);
                                cmd.Parameters.AddWithValue("@Semester", semester[i]);
                                cmd.Parameters.AddWithValue("@Course", crs[i]);
                                cmd.Parameters.AddWithValue("@Room", room[i]);
                                cmd.Parameters.AddWithValue("@ClassType", classtype[i]);
                            cmd.Parameters.AddWithValue("@RoomCategory", roomcategory[i]);
                            cmd.ExecuteNonQuery();

                            }
                            MessageBox.Show("Succesfully Updated","Schedule Updated", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);


                            PopulateGridViewSpecializationt();
                            btnSave.Enabled = false;
                            foreach (Control c in Controls)
                            {
                                if (c is CheckBox)
                                {
                                    CheckBox cb = (CheckBox)c;
                                    cb.Enabled = false;
                                    cb.Checked = false;
                                }
                            }
                            sqlcon.Close();
                        btnPlotExit.Visible = false;
                        cbCheckSched.Checked = false;
                        cbCheckSched.Visible = false;
                        sched.Clear();
                        subject.Clear();
                        hrs.Clear();
                        time.Clear();
                        day.Clear();
                        classtype.Clear();
                        timedayid.Clear();
                        section.Clear();
                        room.Clear();
                        dayLB.Clear();
                        timeLB.Clear();
                        crs.Clear();
                        roomcategory.Clear();
                        check = true;
                        btnUpdate.Enabled = false;
                        listBox1.DataSource = null;
                        listBox2.DataSource = null;
                        listBox3.DataSource = null;
                        listBox4.DataSource = null;
                        listBox5.DataSource = null;
                        listBox6.DataSource = null;
                        listBox7.DataSource = null;
                        sqlcon.Open();

                        

                        DateTime time1 = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time1.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a shedule for " + txtFName.Text);
                        cm.ExecuteNonQuery();
                    }
                    }



                    else
                    {
                    if (counter != total)
                    {
                        MessageBox.Show("You have number of hours left to plot","Available Hours",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                    else 
                    {
                        for (int i = 0; i < total; i++)
                        {
                            sqlcon.Open();
                            SqlCommand cmd1 = new SqlCommand("FacultySchedAddOrEdit", sqlcon);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView3.Rows[i].Cells["ID"].Value));
                            cmd1.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                            cmd1.Parameters.AddWithValue("@DayID", day[i]);
                            cmd1.Parameters.AddWithValue("@TimeID", time[i]);
                            cmd1.Parameters.AddWithValue("@SubjectCode", sched[i]);
                            cmd1.Parameters.AddWithValue("@Section", section[i]);
                            cmd1.Parameters.AddWithValue("@Semester", semester[i]);
                            cmd1.Parameters.AddWithValue("@Course", crs[i]);
                            cmd1.Parameters.AddWithValue("@Room", room[i]);
                            cmd1.Parameters.AddWithValue("@ClassType", classtype[i]);
                            cmd1.Parameters.AddWithValue("@RoomCategory", roomcategory[i]);
                            cmd1.ExecuteNonQuery();
                            sqlcon.Close();

                        }
                        MessageBox.Show("Succesfully Updated","Updated",MessageBoxButtons.OK,MessageBoxIcon.Information);

                        PopulateGridViewSpecializationt();
                        foreach (Control c in Controls)
                        {
                            if (c is CheckBox)
                            {
                                CheckBox cb = (CheckBox)c;
                                cb.Enabled = false;
                                cb.Checked = false;
                            }
                        }
                        btnPlotExit.Visible = false;
                        cbCheckSched.Checked = false;
                        cbCheckSched.Visible = false;
                        sched.Clear();
                        subject.Clear();
                        hrs.Clear();
                        time.Clear();
                        day.Clear();
                        classtype.Clear();
                        timedayid.Clear();
                        section.Clear();
                        room.Clear();
                        dayLB.Clear();
                        timeLB.Clear();
                        crs.Clear();
                        check = true;
                        btnUpdate.Enabled = false;
                        listBox1.DataSource = null;
                        listBox2.DataSource = null;
                        listBox3.DataSource = null;
                        listBox4.DataSource = null;
                        listBox5.DataSource = null;
                        listBox6.DataSource = null;
                        listBox7.DataSource = null;
                        sqlcon.Open();


                        DateTime time1 = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time1.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a shedule for " + txtFName.Text);
                        cm.ExecuteNonQuery();

                    }
                    }

                   

                }
                
          /*  }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          */
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btndel_Click(object sender, EventArgs e)
        {
            
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this schedule?", "Delete Schedule", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        SqlCommand cmddel8 = new SqlCommand("DELETE FROM FacultySchedule_Tbl WHERE FacultyCode = @FacultyCode", sqlcon);
                        cmddel8.Parameters.AddWithValue("FacultyCode", cbxFaculty.Text);
                        cmddel8.ExecuteNonQuery();
                        PopulateGridViewSpecializationt();
                        btnSave.Enabled = true;
                        btnUpdate.Enabled = false;
                        btnPlot.Text = "Plot Time Schedule";
                        MessageBox.Show("Schedule deleted succesfully", "Deleted");

                        DateTime time1 = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time1.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " deleted a shedule for " + txtFName.Text);
                        cm.ExecuteNonQuery();

                      
                    }
                    btnPlot.Text = "Plot Time Schedule";
                    btnPlotExit.Visible = false;
                    TotalHrsPerSubj = 0;
                    subjHrs = 0;
                    credhrs = 0;
                    rowcount = 0;
                    cellcount = 5;
                    count = 0;
                    btnPlot.Enabled = true;
                    cbCheckSched.Checked = false;
                    cbCheckSched.Visible = false;
                    foreach (Control c in Controls)
                    {
                        if (c is CheckBox)
                        {
                            CheckBox cb = (CheckBox)c;
                            cb.Enabled = false;
                            cb.Checked = false;
                        }
                    }
                    btnSave.Enabled = false;
                    btnUpdate.Enabled = false;
                    sched.Clear();
                    subject.Clear();
                    hrs.Clear();
                    time.Clear();
                    day.Clear();
                    classtype.Clear();
                    timedayid.Clear();
                    room.Clear();
                    section.Clear();
                    dayLB.Clear();
                    timeLB.Clear();
                    crs.Clear();
                    roomcategory.Clear();
                    check = true;
                    listBox1.DataSource = null;
                    listBox2.DataSource = null;
                    listBox3.DataSource = null;
                    listBox4.DataSource = null;
                    listBox5.DataSource = null;
                    listBox6.DataSource = null;
                    listBox7.DataSource = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
            private void txtTotal_TextChanged(object sender, EventArgs e)
        {
          /*  if (txtTotal.Text == "0")
            {
                groupBox2.Enabled = false;
            }
            else
            {
                groupBox2.Enabled = true;
            */
        }

        void DecisionSupport1() // this is for checking if there is already a time plotted for the instructor, so the time sched wont be repeated
        {
            try
            {

                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    if (counter <= total)
                    {
                        sqlcon.Open();
                       

                        if (cbCheckSched.Checked == false)
                        {
                            string roomTBL = "";
                            string numberofRoom = "0";
                            string queryyy3 = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcategory[counter - 1] + "' AND Course='" + crs[counter - 1] + "'";
                            SqlCommand commanddd3 = new SqlCommand(queryyy3, sqlcon);
                            SqlDataReader readerrr3 = commanddd3.ExecuteReader();

                            if (readerrr3.Read() == true)
                            {
                                numberofRoom = readerrr3["numberOfroom"].ToString();
                            }
                            readerrr3.Close();

                            string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcategory[counter - 1] + "' AND Course='" + crs[counter - 1] + "' AND RoomID='" + numberofRoom + "'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {



                                roomTBL = reader1["Room"].ToString();

                            }
                            reader1.Close();
                            bool duplicate = false;
                            do
                            {

                                string query4 = "SELECT COUNT(ID) AS NumberOfDuplicateForRoom FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Room=@Room AND Semester=@Semester AND  FacultyCode != @FacultyCode";
                                SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                command4.Parameters.AddWithValue("@DayID", day[counter - 1]);
                                command4.Parameters.AddWithValue("@TimeID", time[counter - 1]);
                                command4.Parameters.AddWithValue("@Room", room[counter - 1]);
                                command4.Parameters.AddWithValue("@Semester", semester[counter - 1]);
                                command4.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                                SqlDataReader reader4 = command4.ExecuteReader();

                                if (reader4.Read() == true)
                                {


                                    SchedDuplicateForRoom = reader4["NumberOfDuplicateForRoom"].ToString();


                                }
                                reader4.Close();
                                if(Convert.ToInt32(SchedDuplicateForRoom) >= 1)
                                {
                                    if(numberofRoom != "1")
                                    {
                                        int num = Convert.ToInt32(numberofRoom) - 1;
                                        numberofRoom = num.ToString();
                                        duplicate = true;
                                        cancel = false;
                                    }
                                        if (numberofRoom == "1")
                                        {
                                        DialogResult dr = MessageBox.Show("There is no availabe room for this time and day. Would you like to put TBA temporarily?", "DSS", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                                        if (dr == DialogResult.Yes)
                                        {
                                            command4.Parameters.AddWithValue("@Room", room[counter - 1] = "TBA");
                                            duplicate = false;
                                            cancel = false;
                                            listBox7.DataSource = null;
                                            listBox7.DataSource = room;
                                        }
                                        else if (dr == DialogResult.No)
                                        {
                                            cancel = true;
                                        }
                                    }
                                    
                                }
                            } while (duplicate == true);
                            
                            string query5 = "SELECT COUNT(ID) AS NumberOfDuplicateForSection FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Section=@Section AND Semester=@Semester AND FacultyCode != @FacultyCode AND (SubjectCode != '"+"Consultation Hours"+"' OR SubjectCode !='"+"Research And Extension"+"')";
                            SqlCommand command5 = new SqlCommand(query5, sqlcon);
                            command5.Parameters.AddWithValue("@DayID", day[counter - 1]);
                            command5.Parameters.AddWithValue("@TimeID", time[counter - 1]);
                            command5.Parameters.AddWithValue("@Section", section[counter - 1]);
                            command5.Parameters.AddWithValue("@Semester", semester[counter - 1]);
                            command5.Parameters.AddWithValue("@FacultyCode", cbxFaculty.Text);
                            SqlDataReader reader5 = command5.ExecuteReader();

                            if (reader5.Read() == true)
                            {


                                SchedDuplicateForSection = reader5["NumberOfDuplicateForSection"].ToString();


                            }
                            reader5.Close();
                            sqlcon.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            


            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
                if (currentCheckBox.Checked)
                {
                       currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
                }
                else
                {
                     currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
                }
            // ------------------------------ //
         //   MessageBox.Show(check.ToString());
            if (checkBox1.Checked == true && counter <= total)
                    {
                        timedayid.Add("101");
                        counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("1");
                    time.Add("1");
                    dayLB.Add("Monday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("1");
                    time.Add("1");
                    dayLB.Add("Monday");
                    timeLB.Add("7:00 - 8:00 AM");
                 
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "1";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";
                  
                    

                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {
                 
                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else 
                    {
                        check = true;
                      
                    }
                }
                if (btnplot == true)
                {
                    
                    DecisionSupport1();
                    if(cancel == true)
                    {
                        checkBox1.Checked = false;
                    }
                     /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                       DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?","Decision Support System",MessageBoxButtons.YesNo);
                        if(dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                      
                   
                           
                        
                    }
                       */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter -1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section","DSS",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        checkBox1.Checked = false;
                    }
                }
            }
                   else  if (checkBox1.Checked == false)
                    {
                timedayid.Remove("101");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("1");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                        
                    }
                }
            }
            if (checkBox1.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Hours Exceeded",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox1.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
         
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
          
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //

            if (checkBox2.Checked == true && counter <= total)
                {
                counter++;
                timedayid.Add("102");
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("2");
                    time.Add("2");
                    dayLB.Add("Monday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("2");
                    time.Add("2");
                    dayLB.Add("Monday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "2";
                    time[indexDay] = "2";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "8:00 - 9:00 AM";
                  

                }


                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox2.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox2.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section","DSS",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                        checkBox2.Checked = false;
                    }
                }
            }
                else if (checkBox2.Checked == false)
                {
                    timedayid.Remove("102");

                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("2");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox2.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Hours Exceeded",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox2.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
                foreach (string listboxitems in listBox3.Items)
            {
                if (listboxitems == "Remove" + (counter).ToString())
                {
                    check = false;
                }
            }
               
            {

            }
           
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
          
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //

            if (checkBox3.Checked == true && counter <= total)
                {
                    timedayid.Add("103");
                counter++;
                if (listBox3.Items.Count == 0)
                {
                    day.Add("3");
                    time.Add("3");
                    dayLB.Add("Monday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("3");
                    time.Add("3");
                    dayLB.Add("Monday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "3";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";
                   
                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox3.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox3.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox3.Checked = false;
                    }
                }
            }
                else if (checkBox3.Checked == false)
                {
                    timedayid.Remove("103");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("3");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox3.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox3.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
            
            foreach (string listboxitems in listBox3.Items)
            {

                if (listboxitems == "Remove" + (counter).ToString())
                {
                    check = false;
                }
            }

        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
          
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox4.Checked == true && counter <= total)
                {
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("4");
                    time.Add("4");
                    dayLB.Add("Monday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("4");
                    time.Add("4");
                    dayLB.Add("Monday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "4";
                    time[indexDay] = "4";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "10:00 - 11:00 AM";
                   
                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox4.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox4.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox4.Checked = false;
                    }
                }
            }
                else if (checkBox4.Checked == false)
                {
                    timedayid.Remove("104");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("4");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox4.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox4.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }

        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
          
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox5.Checked == true && counter <= total)
                {
                    timedayid.Add("105");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("5");
                    time.Add("5");
                    dayLB.Add("Monday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("5");
                    time.Add("5");
                    dayLB.Add("Monday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "5";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox5.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox5.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox5.Checked = false;
                    }
                }
            }
                else if (checkBox5.Checked == false)
                {
                    timedayid.Remove("105");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("5");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox5.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox5.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            foreach (string lbitems in listBox2.Items)
            {
                if (lbitems != "Remove")
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox6.Checked == true && counter <= total)
                {
                    timedayid.Add("106");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("6");
                    time.Add("6");
                    dayLB.Add("Monday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("6");
                    time.Add("6");
                    dayLB.Add("Monday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "6";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox6.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox6.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox6.Checked = false;
                    }
                }
            }
                else if (checkBox6.Checked == false)
                {
                    timedayid.Remove("106");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("6");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox6.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox6.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
         
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox7.Checked == true && counter <= total)
                {
                    timedayid.Add("107");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("7");
                    time.Add("7");
                    dayLB.Add("Monday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("7");
                    time.Add("7");
                    dayLB.Add("Monday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "7";
                    time[indexDay] = "7";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox7.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox7.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox7.Checked = false;
                    }
                }
            }
                else if (checkBox7.Checked == false)
                {
                    timedayid.Remove("107");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("7");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox7.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox7.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
        
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox8.Checked == true && counter <= total)
                {
                    timedayid.Add("108");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("8");
                    time.Add("8");
                    dayLB.Add("Monday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("8");
                    time.Add("8");
                    dayLB.Add("Monday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "8";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox8.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox8.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox8.Checked = false;
                    }
                }
            }
                else if (checkBox8.Checked == false)
                {
                    timedayid.Remove("108");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("8");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox8.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox8.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox9.Checked == true && counter <= total)
                {
                    timedayid.Add("109");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("9");
                    time.Add("9");
                    dayLB.Add("Monday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("9");
                    time.Add("9");
                    dayLB.Add("Monday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "9";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox9.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox9.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox9.Checked = false;
                    }
                }
            }
                else if (checkBox9.Checked == false)
                {
                    timedayid.Remove("109");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("9");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox9.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox9.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
           
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox10.Checked == true && counter <= total)
                {
                    timedayid.Add("110");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("10");
                    time.Add("10");
                    dayLB.Add("Monday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("10");
                    time.Add("10");
                    dayLB.Add("Monday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "10";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox10.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox10.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox10.Checked = false;
                    }
                }
            }
                else if (checkBox10.Checked == false)
                {
                    timedayid.Remove("110");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("10");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox10.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox10.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
           
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox11.Checked == true && counter <= total)
                {
                    timedayid.Add("111");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("11");
                    time.Add("11");
                    dayLB.Add("Monday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("11");
                    time.Add("11");
                    dayLB.Add("Monday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "11";
                    time[indexDay] = "11";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox11.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox11.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox11.Checked = false;
                    }
                }
            }
                else if (checkBox11.Checked == false)
                {
                    timedayid.Remove("111");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("11");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox11.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox11.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;

            }
        }
        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
          
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox12.Checked == true && counter <= total)
                {
                    timedayid.Add("112");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("12");
                    time.Add("12");
                    dayLB.Add("Monday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("12");
                    time.Add("12");
                    dayLB.Add("Monday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "12";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox12.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox12.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox12.Checked = false;
                    }
                }
            }
                else if (checkBox12.Checked == false)
                {
                    timedayid.Remove("112");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("12");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox12.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox12.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
           
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox13.Checked == true && counter <= total)
                {
                    timedayid.Add("13");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("13");
                    time.Add("13");
                    dayLB.Add("Monday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("13");
                    time.Add("13");
                    dayLB.Add("Monday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "13";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";
                  

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox13.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox13.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox13.Checked = false;
                    }
                }
            }
                else if (checkBox13.Checked == false)
                {
                    timedayid.Remove("113");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("13");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox13.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox13.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox26.Checked == true && counter <= total)
                {
                    timedayid.Add("114");
                   
                    counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("21");
                    time.Add("1");
                    dayLB.Add("Tuesday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("21");
                    time.Add("1");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "21";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox26.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox26.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox26.Checked = false;
                    }
                }
            }
                else if (checkBox26.Checked == false)
                {
                    timedayid.Remove("114");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("21");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                     foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox26.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox26.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox25.Checked == true && counter <= total)
                {
                    timedayid.Add("115");
                counter++;
                if (listBox3.Items.Count == 0 && check ==true)
                {
                    day.Add("22");
                    time.Add("2");
                    dayLB.Add("Tuesday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("22");
                    time.Add("2");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "22";
                    time[indexDay] = "2";
                    dayLB[indexDay] =  "Tuesday";
                    timeLB[indexDay] = "8:00 - 9:00 AM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox25.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox25.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox25.Checked = false;
                    }
                }
            }
                else if (checkBox25.Checked == false)
                {
                    timedayid.Remove("115");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("22");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox25.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox25.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox24.Checked == true && counter <= total)
                {
                    timedayid.Add("116");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true) 
                {
                    day.Add("23");
                    time.Add("3");
                    dayLB.Add("Tuesday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("23");
                    time.Add("3");
                    dayLB.Add("Tuesday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "23";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";
                    

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox24.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox24.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox24.Checked = false;
                    }
                }
            }
                else if (checkBox24.Checked == false)
                {
                    timedayid.Remove("116");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("23");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox24.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox24.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox23.Checked == true && counter <= total)
                {
                    timedayid.Add("117");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("24");
                    time.Add("4");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("24");
                    time.Add("4");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("10:00 - 11:00 AM");
                }

                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                   day[indexDay] = "24";
                   time[indexDay] = "4";
                   dayLB[indexDay] = "Tuesday";
                   timeLB[indexDay] = "10:00 - 11:00 AM";
                    

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox23.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox23.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox23.Checked = false;
                    }
                }
            }
                else if (checkBox23.Checked == false)
                {
                    timedayid.Remove("117");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("24");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox23.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox23.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox22.Checked == true && counter <= total)
                {
                    timedayid.Add("118");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("25");
                    time.Add("5");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("25");
                    time.Add("5");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "25";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";
                    

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox22.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox22.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox22.Checked = false;
                    }
                }
            }
                else if (checkBox22.Checked == false)
                {
                    timedayid.Remove("118");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("25");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox22.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox22.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox21.Checked == true && counter <= total)
                {
                    timedayid.Add("119");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("26");
                    time.Add("6");
                    dayLB.Add("Tuesday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("26");
                    time.Add("6");
                    dayLB.Add("Tuesday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "26";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";
                 

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox21.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox21.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox21.Checked = false;
                    }
                }
            }
                else if (checkBox21.Checked == false)
                {
                    timedayid.Remove("119");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("26");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox21.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox21.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox20.Checked == true && counter <= total)
                {
                    timedayid.Add("120");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("27");
                    time.Add("7");
                    dayLB.Add("Tuesday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("27");
                    time.Add("7");
                    dayLB.Add( "Tuesday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "27";
                    time[indexDay] = "7";
                    dayLB[indexDay] =  "Tuesday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";
                 

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox20.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox20.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox20.Checked = false;
                    }
                }
            }
                else if (checkBox20.Checked == false)
                {
                    timedayid.Remove("120");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("27");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox20.Checked == true && counter > total)
                {
                if (dataGridView1.Rows.Count != 0 && dataGridView3.Rows.Count == total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox20.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
            }
        }
        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox19.Checked == true && counter <= total)
                {
                    timedayid.Add("121");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("28");
                    time.Add("8");
                    dayLB.Add("Tuesday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("28");
                    time.Add("8");
                    dayLB.Add("Tuesday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "28";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";
                 

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox19.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox19.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox19.Checked = false;
                    }
                }
            }
                else if (checkBox19.Checked == false)
                {
                    timedayid.Remove("121");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("28");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }

            }
                if (checkBox19.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox19.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox18.Checked == true && counter <= total)
                {
                    timedayid.Add("122");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("29");
                    time.Add("9");
                    dayLB.Add("Tuesday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("29");
                    time.Add("9");
                    dayLB.Add("Tuesday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "29";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox18.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox18.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox18.Checked = false;
                    }
                }
            }
                else if (checkBox18.Checked == false)
                {
                    timedayid.Remove("122");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("29");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox18.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox18.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox17.Checked == true && counter <= total )
                {
                    timedayid.Add("123");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("210");
                    time.Add("10");
                    dayLB.Add("Tuesday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("210");
                    time.Add("10");
                    dayLB.Add("Tuesday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "210";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
              
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox17.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox17.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox17.Checked = false;
                    }
                }
            }
                else if (checkBox17.Checked == false)
                {
                    timedayid.Remove("123");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("210");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox17.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox17.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox16.Checked == true && counter <= total)
                {
                    timedayid.Add("124");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("211");
                    time.Add("11");
                    dayLB.Add("Tuesday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("211");
                    time.Add("11");
                    dayLB.Add("Tuesday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "211";
                    time[indexDay] = "11";
                    dayLB[indexDay] =  "Tuesday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox16.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox16.Checked = false;
                        }
                    }
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section");
                        checkBox16.Checked = false;
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox16.Checked = false;
                    }
                }
            }
                else if (checkBox16.Checked == false)
                {
                    timedayid.Remove("124");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("211");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox16.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox16.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox15.Checked == true && counter <= total)
                {
                    timedayid.Add("125");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("212");
                    time.Add("12");
                    dayLB.Add("Tuesday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("212");
                    time.Add("12");
                    dayLB.Add("Tuesday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "212";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox15.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox15.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox15.Checked = false;
                    }
                }
            }
                else if (checkBox15.Checked == false)
                {
                    timedayid.Remove("125");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("212");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox15.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox15.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox14.Checked == true && counter <= total)
                {
                    timedayid.Add("126");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("213");
                    time.Add("13");
                    dayLB.Add("Tuesday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("213");
                    time.Add("13");
                    dayLB.Add("Tuesday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "213";
                    time[indexDay] = "13";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox14.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox14.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox14.Checked = false;
                    }
                }
            }
                else if (checkBox14.Checked == false)
                {
                    timedayid.Remove("126");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("213");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox14.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox14.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
           
        }
        private void checkBox39_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox39.Checked == true && counter <= total)
                {
                timedayid.Add("127");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                   
                    day.Add("31");
                    time.Add("1");
                    dayLB.Add("Wednesday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("31");
                    time.Add("1");
                    dayLB.Add("Wednesday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "31";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";
                 

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox39.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox39.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox39.Checked = false;
                    }
                }
            }
                else if (checkBox39.Checked == false)
                {
                    timedayid.Remove("127");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("31");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox39.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox39.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox38_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox38.Checked == true && counter <= total)
                {
                    timedayid.Add("128");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("32");
                    time.Add("2");
                    dayLB.Add("Wednesday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("32");
                    time.Add("2");
                    dayLB.Add("Wednesday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "32";
                    time[indexDay] = "2";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "8:00 - 9:00 AM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox38.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox38.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox38.Checked = false;
                    }
                }
            }
                else if (checkBox38.Checked == false)
                {
                    timedayid.Remove("128");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("32");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox38.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox38.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox37_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox37.Checked == true && counter <= total)
                {
                    timedayid.Add("129");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("33");
                    time.Add("3");
                    dayLB.Add("Wednesday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("33");
                    time.Add("3");
                    dayLB.Add("Wednesday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "33";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";
                 

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox37.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox37.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox37.Checked = false;
                    }
                }
            }
                else if (checkBox37.Checked == false)
                {
                    timedayid.Remove("129");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("33");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox37.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox37.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox36.Checked == true && counter <= total)
                {
                    timedayid.Add("130");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("34");
                    time.Add("4");
                    dayLB.Add("Wednesday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("34");
                    time.Add("4");
                    dayLB.Add("Wednesday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "34";
                    time[indexDay] = "4";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "10:00 - 11:00 AM";
                    

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox36.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox36.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox36.Checked = false;
                    }
                }
            }
                else if (checkBox36.Checked == false)
                {
                    timedayid.Remove("130");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("34");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox36.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox36.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox35.Checked == true && counter <= total)
                {
                    timedayid.Add("131");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("35");
                    time.Add("5");
                    dayLB.Add("Wednesday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("35");
                    time.Add("5");
                    dayLB.Add("Wednesday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "35";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox35.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox35.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox35.Checked = false;
                    }
                }
            }
                else if (checkBox35.Checked == false)
                {
                    timedayid.Remove("131");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("35");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox35.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox35.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox34.Checked == true && counter <= total)
                {
                    timedayid.Add("132");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("36");
                    time.Add("6");
                    dayLB.Add("Wednesday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("36");
                    time.Add("6");
                    dayLB.Add("Wednesday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "36";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox34.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox34.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox34.Checked = false;
                    }
                }
            }
                else if (checkBox34.Checked == false)
                {
                    timedayid.Remove("132");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("36");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox34.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox34.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox33.Checked == true && counter <= total)
                {
                    timedayid.Add("133");
                 
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("37");
                    time.Add("7");
                    dayLB.Add("Wednesday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("37");
                    time.Add("7");
                    dayLB.Add("Wednesday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "37";
                    time[indexDay] = "7";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox33.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox33.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox33.Checked = false;
                    }
                }
            }
                else if (checkBox33.Checked == false)
                {
                    timedayid.Remove("133");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("37");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox33.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox33.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox32.Checked == true && counter <= total)
                {
                    timedayid.Add("134");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("38");
                    time.Add("8");
                    dayLB.Add("Wednesday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("38");
                    time.Add("8");
                    dayLB.Add("Wednesday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "38";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";
                 

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox32.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox32.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox32.Checked = false;
                    }
                }
            }
                else if (checkBox32.Checked == false)
                {
                    timedayid.Remove("134");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("38");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox32.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox32.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox31.Checked == true && counter <= total)
                {
                    timedayid.Add("135");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("39");
                    time.Add("9");
                    dayLB.Add("Wednesday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("39");
                    time.Add("9");
                    dayLB.Add("Wednesday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "39";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox31.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox31.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && sched[counter - 1] != "Consultation Hours")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox31.Checked = false;
                    }
                }
            }
                else if (checkBox31.Checked == false)
                {
                    timedayid.Remove("135");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("39");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox31.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox31.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox30.Checked == true && counter <= total)
                {
                    timedayid.Add("136");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("310");
                    time.Add("10");
                    dayLB.Add("Wednesday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("310");
                    time.Add("10");
                    dayLB.Add("Wednesday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "310";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox30.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox30.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox30.Checked = false;
                    }
                }
            }
                else if (checkBox30.Checked == false)
                {
                    timedayid.Remove("136");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("310");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox30.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox30.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox29.Checked == true && counter <= total)
                {
                    timedayid.Add("137");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("311");
                    time.Add("11");
                    dayLB.Add("Wednesday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("311");
                    time.Add("11");
                    dayLB.Add("Wednesday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "311";
                    time[indexDay] = "11";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox29.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox29.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox29.Checked = false;
                    }
                }
            }
                else if (checkBox29.Checked == false)
                {
                    timedayid.Remove("137");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("311");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox29.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox29.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox28.Checked == true && counter <= total)
                {
                    timedayid.Add("138");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("312");
                    time.Add("12");
                    dayLB.Add("Wednesday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("312");
                    time.Add("12");
                    dayLB.Add("Wednesday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "312";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";
                   

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox28.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox28.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox28.Checked = false;
                    }
                }
            }
                else if (checkBox28.Checked == false)
                {
                    timedayid.Remove("138");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("312");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox28.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox28.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox27.Checked == true && counter <= total)
                {
                    timedayid.Add("139");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("313");
                    time.Add("13");
                    dayLB.Add("Wednesday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("313");
                    time.Add("13");
                    dayLB.Add("Wednesday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "313";
                    time[indexDay] = "13";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";
                   

                }

                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox27.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox27.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox27.Checked = false;
                    }
                }
            }
                else if (checkBox27.Checked == false)
                {
                    timedayid.Remove("138");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("313");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox27.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox27.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox52_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox52.Checked == true && counter <= total)
                {
                    timedayid.Add("140");
                  
                    counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("41");
                    time.Add("1");
                    dayLB.Add("Thursday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("41");
                    time.Add("1");
                    dayLB.Add("Thursday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "41";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox52.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox52.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox52.Checked = false;
                    }
                }
            }
                else if (checkBox52.Checked == false)
                {
                    timedayid.Remove("140");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("41");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox52.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox52.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox51_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox51.Checked == true && counter <= total)
                {
                    timedayid.Add("141");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("42");
                    time.Add("2");
                    dayLB.Add("Thursday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("42");
                    time.Add("2");
                    dayLB.Add("Thursday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "42";
                    time[indexDay] = "2";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "8:00 - 9:00 AM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox51.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox51.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox51.Checked = false;
                    }
                }
            }
                else if (checkBox51.Checked == false)
                {
                    timedayid.Remove("141");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("42");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox51.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox51.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox50_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox50.Checked == true && counter <= total)
                {
                    timedayid.Add("142");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("43");
                    time.Add("3");
                    dayLB.Add("Thursday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("43");
                    time.Add("3");
                    dayLB.Add("Thursday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "43";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";
                 

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox50.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox50.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox50.Checked = false;
                    }
                }
            }
                else if (checkBox50.Checked == false)
                {
                    timedayid.Remove("142");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("43");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox50.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox50.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox49_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox49.Checked == true && counter <= total)
                {
                    timedayid.Add("143");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("44");
                    time.Add("4");
                    dayLB.Add("Thursday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("44");
                    time.Add("4");
                    dayLB.Add("Thursday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "44";
                    time[indexDay] = "4";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "10:00 - 11:00 AM";
                  

                }
                counterindex++;
                    lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox49.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox49.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox49.Checked = false;
                    }
                }
            }
                else if (checkBox49.Checked == false)
                {
                    timedayid.Remove("143");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("44");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox49.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox49.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox48_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox48.Checked == true && counter <= total)
                {
                    timedayid.Add("144");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("45");
                    time.Add("5");
                    dayLB.Add("Thursday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("45");
                    time.Add("5");
                    dayLB.Add("Thursday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "45";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox48.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox48.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox48.Checked = false;
                    }
                }
            }
                else if (checkBox48.Checked == false)
                {
                    timedayid.Remove("144");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("45");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox48.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox48.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox47_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox47.Checked == true && counter <= total)
                {
                    timedayid.Add("145");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("46");
                    time.Add("6");
                    dayLB.Add("Thursday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("46");
                    time.Add("6");
                    dayLB.Add("Thursday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "46";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox47.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox47.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox47.Checked = false;
                    }
                }
            }
                else if (checkBox47.Checked == false)
                {
                    timedayid.Remove("145");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("46");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox47.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox47.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox46.Checked == true && counter <= total)
                {
                    timedayid.Add("146");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("47");
                    time.Add("7");
                    dayLB.Add("Thursday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("47");
                    time.Add("7");
                    dayLB.Add("Thursday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "47";
                    time[indexDay] = "7";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox46.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox46.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox46.Checked = false;
                    }
                }
            }
                else if (checkBox46.Checked == false)
                {
                    timedayid.Remove("146");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("47");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox46.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox46.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox45_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox45.Checked == true && counter <= total)
                {
                    timedayid.Add("147");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("48");
                    time.Add("8");
                    dayLB.Add("Thursday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("48");
                    time.Add("8");
                    dayLB.Add("Thursday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "48";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox45.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox45.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        checkBox45.Checked = false;
                    }
                }
            }
                else if (checkBox45.Checked == false)
                {
                    timedayid.Remove("147");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("48");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox45.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox45.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox44_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox44.Checked == true && counter <= total)
                {
                    timedayid.Add("148");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("49");
                    time.Add("9");
                    dayLB.Add("Thursday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("49");
                    time.Add("9");
                    dayLB.Add("Thursday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "49";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox44.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox44.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox44.Checked = false;
                    }
                }
            }
                else if (checkBox44.Checked == false)
                {
                    timedayid.Remove("148");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("49");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox44.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox44.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox43_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox43.Checked == true && counter <= total)
                {
                    timedayid.Add("149");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("410");
                    time.Add("10");
                    dayLB.Add("Thursday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("410");
                    time.Add("10");
                    dayLB.Add("Thursday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "410";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox43.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox43.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox43.Checked = false;
                    }
                }
            }
                else if (checkBox43.Checked == false)
                {
                    timedayid.Remove("149");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("410");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }

            }
                if (checkBox43.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox43.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox42_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox42.Checked == true && counter <= total)
                {
                    timedayid.Add("150");
                 
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("411");
                    time.Add("11");
                    dayLB.Add("Thursday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("411");
                    time.Add("11");
                    dayLB.Add("Thursday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "411";
                    time[indexDay] = "11";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox42.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox42.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox42.Checked = false;
                    }
                }
            }
                else if (checkBox42.Checked == false)
                {
                    timedayid.Remove("150");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("411");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox42.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox42.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox41_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox41.Checked == true && counter <= total)
                {
                    timedayid.Add("151");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("412");
                    time.Add("12");
                    dayLB.Add("Thursday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("412");
                    time.Add("12");
                    dayLB.Add("Thursday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "412";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox41.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox41.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox41.Checked = false;
                    }
                }
            }
                else if (checkBox41.Checked == false)
                {
                    timedayid.Remove("151");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("412");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox41.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox41.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox40_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox40.Checked == true && counter <= total)
                {
                    timedayid.Add("152");
                 
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("413");
                    time.Add("13");
                    dayLB.Add("Thursday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("413");
                    time.Add("13");
                    dayLB.Add("Thursday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "413";
                    time[indexDay] = "13";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox40.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox40.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox40.Checked = false;
                    }
                }
            }
                else if (checkBox40.Checked == false)
                {
                    timedayid.Remove("152");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("413");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox40.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox40.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox65_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox65.Checked == true && counter <= total)
            {
                timedayid.Add("153");
             
                    counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("51");
                    time.Add("1");
                    dayLB.Add("Friday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("51");
                    time.Add("1");
                    dayLB.Add("Friday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "51";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox65.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox65.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox65.Checked = false;
                    }
                }
            }
            else if (checkBox65.Checked == false)
            {
                timedayid.Remove("153");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("51");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox65.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox65.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox64_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox64.Checked == true && counter <= total)
                {
                    timedayid.Add("154");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("52");
                    time.Add("2");
                    dayLB.Add("Friday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("52");
                    time.Add("2");
                    dayLB.Add("Friday");
                    timeLB.Add("8:00 - 9:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "52";
                    time[indexDay] = "2";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "8:00 - 9:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox64.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox64.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox64.Checked = false;
                    }
                }
            }
                else if (checkBox64.Checked == false)
                {
                    timedayid.Remove("154");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("52");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox64.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox64.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox63_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox63.Checked == true && counter <= total)
                {
                    timedayid.Add("155");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("53");
                    time.Add("3");
                    dayLB.Add("Friday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("53");
                    time.Add("3");
                    dayLB.Add("Friday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "53";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox63.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox63.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox63.Checked = false;
                    }
                }
            }
                else if (checkBox63.Checked == false)
                {
                    timedayid.Remove("155");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("53");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox63.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox63.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox62_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox62.Checked == true && counter <= total)
                {
                    timedayid.Add("156");
                 
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("54");
                    time.Add("4");
                    dayLB.Add("Friday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("54");
                    time.Add("4");
                    dayLB.Add("Friday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "54";
                    time[indexDay] = "4";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "10:00 - 11:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox62.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox62.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox62.Checked = false;
                    }
                }
            }
                else if (checkBox62.Checked == false)
                {
                    timedayid.Remove("156");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("54");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox62.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox62.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox61_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox61.Checked == true && counter <= total)
                {
                    timedayid.Add("157");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("55");
                    time.Add("5");
                    dayLB.Add("Friday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("55");
                    time.Add("5");
                    dayLB.Add("Friday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "55";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox61.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox61.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox61.Checked = false;
                    }
                }
            }
                else if (checkBox61.Checked == false)
                {
                    timedayid.Remove("157");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("55");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox61.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox61.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox60_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox60.Checked == true && counter <= total)
                {
                    timedayid.Add("158");
           
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("56");
                    time.Add("6");
                    dayLB.Add("Friday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("56");
                    time.Add("6");
                    dayLB.Add("Friday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "56";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox60.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox60.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox60.Checked = false;
                    }
                }
            }
                else if (checkBox60.Checked == false)
                {
                    timedayid.Remove("158");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("56");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox60.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox60.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox59_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox59.Checked == true && counter <= total)
                {
                    timedayid.Add("159");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("57");
                    time.Add("7");
                    dayLB.Add("Friday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("57");
                    time.Add("7");
                    dayLB.Add("Friday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "57";
                    time[indexDay] = "7";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox59.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox59.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox59.Checked = false;
                    }
                }
            }
                else if (checkBox59.Checked == false)
                {
                    timedayid.Remove("159");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("57");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox59.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox59.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox58_CheckedChanged(object sender, EventArgs e)
        {
            
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox58.Checked == true && counter <= total)
                {
                    timedayid.Add("160");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("58");
                    time.Add("8");
                    dayLB.Add("Friday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("58");
                    time.Add("8");
                    dayLB.Add("Friday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "58";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox58.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox58.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox58.Checked = false;
                    }
                }
            }
            else if (checkBox58.Checked == false)
                {
                    timedayid.Remove("160");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("58");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox58.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox58.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox57_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox57.Checked == true && counter <= total)
                {
                    timedayid.Add("161");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("59");
                    time.Add("9");
                    dayLB.Add("Friday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("59");
                    time.Add("9");
                    dayLB.Add("Friday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "59";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox57.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox57.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox57.Checked = false;
                    }
                }
            }
                else if (checkBox57.Checked == false)
                {
                    timedayid.Remove("161");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("59");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox57.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox57.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox56_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox56.Checked == true && counter <= total)
                {
                    timedayid.Add("162");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("510");
                    time.Add("10");
                    dayLB.Add("Friday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("510");
                    time.Add("10");
                    dayLB.Add("Friday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "510";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox56.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox56.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox56.Checked = false;
                    }
                }
            }
                else if (checkBox56.Checked == false)
                {
                    timedayid.Remove("162");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("510");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox56.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox56.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox55_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox55.Checked == true && counter <= total)
                {
                    timedayid.Add("163");
                 
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("511");
                    time.Add("11");
                    dayLB.Add("Friday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("511");
                    time.Add("11");
                    dayLB.Add("Friday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "511";
                    time[indexDay] = "11";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox55.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox55.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox55.Checked = false;
                    }
                }
            }
                else if (checkBox55.Checked == false)
                {
                    timedayid.Remove("163");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("511");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox55.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox55.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox54_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox54.Checked == true && counter <= total)
                {
                    timedayid.Add("164");
                
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("512");
                    time.Add("12");
                    dayLB.Add("Friday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("512");
                    time.Add("12");
                    dayLB.Add("Friday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "512";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox54.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox54.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox54.Checked = false;
                    }
                }
            }
                else if (checkBox54.Checked == false)
                {
                    timedayid.Remove("164");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("512");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox54.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox54.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox53_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox53.Checked == true && counter <= total)
                {
                    timedayid.Add("165");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("513");
                    time.Add("13");
                    dayLB.Add("Friday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("513");
                    time.Add("13");
                    dayLB.Add("Friday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "513";
                    time[indexDay] = "13";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox53.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox53.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox53.Checked = false;
                    }
                }
            }
                else if (checkBox53.Checked == false)
                {
                    timedayid.Remove("165");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("513");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox53.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox53.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox78_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox78.Checked == true && counter <= total)
                {
                    timedayid.Add("166");
              
                    counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("61");
                    time.Add("1");
                    dayLB.Add("Saturday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("61");
                    time.Add("1");
                    dayLB.Add("Saturday");
                    timeLB.Add("7:00 - 8:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "61";
                    time[indexDay] = "1";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "7:00 - 8:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox78.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox78.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox78.Checked = false;
                    }
                }
            }
                else if (checkBox78.Checked == false)
                {
                    timedayid.Remove("166");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("61");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox78.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox78.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox77_CheckedChanged(object sender, EventArgs e)
        {
           
                // for the image of checkbox
                CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox77.Checked == true && counter <= total)
                {
                    timedayid.Add("167");

                    counter++;
                    if (listBox3.Items.Count == 0 && check == true)
                    {
                        day.Add("62");
                        time.Add("2");
                        dayLB.Add("Saturday");
                        timeLB.Add("8:00 - 9:00 AM");
                    }
                    else if (listBox3.Items.Count != 0 && check == true)
                    {
                        day.Add("62");
                        time.Add("2");
                        dayLB.Add("Saturday");
                        timeLB.Add("8:00 - 9:00 AM");
                    }
                    else if (listBox3.Items.Count != 0 && check == false)
                    {
                    int indexDay = day.IndexOf("R");
                        day[indexDay] = "62";
                        time[indexDay] = "2";
                        dayLB[indexDay] = "Saturday";
                        timeLB[indexDay] = "8:00 - 9:00 AM";
                    }
                    counterindex++;
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                    {
                        DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox77.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                        {
                            DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                            if (dr == DialogResult.Yes)
                            {
                                room[counter - 1] = "TBA";
                            }
                            else
                            {
                                checkBox77.Checked = false;
                            }
                        }
                    */
                        else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                        {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox77.Checked = false;
                        }
                    }
                }
                else if (checkBox77.Checked == false)
                {
                    timedayid.Remove("167");
                    check = false;
                    counter--;
                    if (counter == -1)
                    {
                        counter = 0;
                    }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("62");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
                }
                if (checkBox77.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox77.Checked = false;
                    day.Remove("R");
                    time.Remove("R");
                    dayLB.Remove("Remove");
                    timeLB.Remove("Remove");
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    check = true;
                }
           
        }
        private void checkBox76_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox76.Checked == true && counter <= total)
                {
                    timedayid.Add("168");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("63");
                    time.Add("3");
                    dayLB.Add("Saturday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("63");
                    time.Add("3");
                    dayLB.Add("Saturday");
                    timeLB.Add("9:00 - 10:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "63";
                    time[indexDay] = "3";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "9:00 - 10:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox76.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox76.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox76.Checked = false;
                    }
                }
            }
                else if (checkBox76.Checked == false)
                {
                    timedayid.Remove("168");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("63");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox76.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox76.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox75_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox75.Checked == true && counter <= total)
                {
                    timedayid.Add("169");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("64");
                    time.Add("4");
                    dayLB.Add("Saturday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("64");
                    time.Add("4");
                    dayLB.Add("Saturday");
                    timeLB.Add("10:00 - 11:00 AM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "64";
                    time[indexDay] = "4";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "10:00 - 11:00 AM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox75.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox75.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox75.Checked = false;
                    }
                }
            }
                else if (checkBox75.Checked == false)
                {
                    timedayid.Remove("169");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("64");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox75.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox75.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox74_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox74.Checked == true && counter <= total)
                {
                    timedayid.Add("170");
                
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("65");
                    time.Add("5");
                    dayLB.Add("Saturday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("65");
                    time.Add("5");
                    dayLB.Add("Saturday");
                    timeLB.Add("11:00 - 12:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "65";
                    time[indexDay] = "5";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "11:00 - 12:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox74.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox74.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox74.Checked = false;
                    }
                }
            }
                else if (checkBox74.Checked == false)
                {
                    timedayid.Remove("170");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("65");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox74.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox74.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox73_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox73.Checked == true && counter <= total)
                {
                    timedayid.Add("171");
               
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("66");
                    time.Add("6");
                    dayLB.Add("Saturday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("66");
                    time.Add("6");
                    dayLB.Add("Saturday");
                    timeLB.Add("12:00 - 1:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "66";
                    time[indexDay] = "6";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "12:00 - 1:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox73.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox73.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox73.Checked = false;
                    }
                }
            }
                else if (checkBox73.Checked == false)
                {
                    timedayid.Remove("171");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("66");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox73.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox73.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox72_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox72.Checked == true && counter <= total)
                {
                    timedayid.Add("172");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("67");
                    time.Add("7");
                    dayLB.Add("Saturday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("67");
                    time.Add("7");
                    dayLB.Add("Saturday");
                    timeLB.Add("1:00 - 2:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "67";
                    time[indexDay] = "7";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "1:00 - 2:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox72.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox72.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox72.Checked = false;
                    }
                }
            }
                else if (checkBox72.Checked == false)
                {
                    timedayid.Remove("172");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("67");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox72.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox72.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox71_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox71.Checked == true && counter <= total)
                {
                    timedayid.Add("173");
                  
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("68");
                    time.Add("8");
                    dayLB.Add("Saturday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("68");
                    time.Add("8");
                    dayLB.Add("Saturday");
                    timeLB.Add("2:00 - 3:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "68";
                    time[indexDay] = "8";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "2:00 - 3:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox71.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox71.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox71.Checked = false;
                    }
                }
            }
                else if (checkBox71.Checked == false)
                {
                    timedayid.Remove("173");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("68");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox71.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox71.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox70_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox70.Checked == true && counter <= total)
                {
                    timedayid.Add("174");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("69");
                    time.Add("9");
                    dayLB.Add("Saturday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("69");
                    time.Add("9");
                    dayLB.Add("Saturday");
                    timeLB.Add("3:00 - 4:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "69";
                    time[indexDay] = "9";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "3:00 - 4:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox70.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox70.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox70.Checked = false;
                    }
                }
            }
                else if (checkBox70.Checked == false)
                {
                    timedayid.Remove("174");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("69");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox70.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox70.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox69_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox69.Checked == true && counter <= total)
                {
                    timedayid.Add("175");
              
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("610");
                    time.Add("10");
                    dayLB.Add("Saturday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("610");
                    time.Add("10");
                    dayLB.Add("Saturday");
                    timeLB.Add("4:00 - 5:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "610";
                    time[indexDay] = "10";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "4:00 - 5:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox69.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox69.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox69.Checked = false;
                    }
                }
            }
                else if (checkBox69.Checked == false)
                {
                    timedayid.Remove("175");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("610");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox69.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox69.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox68_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox68.Checked == true && counter <= total)
                {
                    timedayid.Add("176");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("611");
                    time.Add("11");
                    dayLB.Add("Saturday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("611");
                    time.Add("11");
                    dayLB.Add("Saturday");
                    timeLB.Add("5:00 - 6:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "611";
                    time[indexDay] = "11";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "5:00 - 6:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox68.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox68.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox68.Checked = false;
                    }
                }
            }
                else if (checkBox68.Checked == false)
                {
                    timedayid.Remove("176");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("611");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox68.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox68.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox67_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox67.Checked == true && counter <= total)
                {
                    timedayid.Add("177");
                   
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("612");
                    time.Add("12");
                    dayLB.Add("Saturday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("612");
                    time.Add("12");
                    dayLB.Add("Saturday");
                    timeLB.Add("6:00 - 7:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "612";
                    time[indexDay] = "12";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "6:00 - 7:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox67.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox67.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox67.Checked = false;
                    }
                }
            }
                else if (checkBox67.Checked == false)
                {
                    timedayid.Remove("177");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("612");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox67.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox67.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }
        private void checkBox66_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox66.Checked == true && counter <= total)
                {
                    timedayid.Add("178");
                    
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("613");
                    time.Add("13");
                    dayLB.Add("Saturday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("613");
                    time.Add("13");
                    dayLB.Add("Saturday");
                    timeLB.Add("7:00 - 8:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "613";
                    time[indexDay] = "13";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "7:00 - 8:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox66.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox66.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox66.Checked = false;
                    }
                }
            }
                else if (checkBox66.Checked == false)
                {
                    timedayid.Remove("178");

                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("613");

                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
                if (checkBox66.Checked == true && counter > total)
                {
                    MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    checkBox66.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void lblcounter_TextChanged(object sender, EventArgs e)
        {
          
            try
            {
              
              
               

            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

      


      

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void btnPlotExit_Click(object sender, EventArgs e)
        {
            
            btnPlotExit.Visible = false;
            TotalHrsPerSubj = 0;
            subjHrs = 0;
            credhrs = 0;
            rowcount = 0;
            cellcount = 5;
            count = 0;
            btnPlot.Enabled = true;
            cbCheckSched.Checked = false;
            cbCheckSched.Visible = false;
            //  groupBox13.Visible = false;
            foreach (Control c in Controls)
            {
                if (c is CheckBox)
                {
                    CheckBox cb = (CheckBox)c;
                    cb.Enabled = false;
                     cb.Checked = false;
                }
            }
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            sched.Clear();
            subject.Clear();
            hrs.Clear();
            time.Clear();
            day.Clear();
            classtype.Clear();
            timedayid.Clear();
            room.Clear();
            section.Clear();
            dayLB.Clear();
            timeLB.Clear();
            crs.Clear();
            check = true;
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;
            listBox4.DataSource = null;
            listBox5.DataSource = null;
            listBox6.DataSource = null;
            listBox7.DataSource = null;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbCheckSched_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check__4_;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.rectangle__1_;
            }
            if (cbCheckSched.Checked == true && dataGridView3.Rows.Count == total)
            {
                checkboxCheckValue();
            }
            else if (cbCheckSched.Checked == true && dataGridView3.Rows.Count != total)
            {
                MessageBox.Show("Current total hours does not match the saved schedule hours. Please plot a new schedule.");
                cbCheckSched.Checked = false;
                foreach (Control c in Controls)
                {
                    if (c is CheckBox)
                    {
                        CheckBox cb = (CheckBox)c;
                        cb.Checked = false;
                    }
                }
            }
            else
            {
                foreach (Control c in Controls)
                {
                    if (c is CheckBox)
                    {
                        CheckBox cb = (CheckBox)c;
                        cb.Checked = false;
                    }
                }
                

            }
            if(cbCheckSched.Checked == false)
            {
                day.Clear();
                time.Clear();
                dayLB.Clear();
                timeLB.Clear();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cbCheckSched.Checked = false;
         //   dataGridView3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox79_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox79.Checked == true && counter <= total)
            {
                timedayid.Add("179");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("14");
                    time.Add("14");
                    dayLB.Add("Monday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("14");
                    time.Add("14");
                    dayLB.Add("Monday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "14";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Monday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";
                  

                }

                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox79.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox79.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox79.Checked = false;
                    }
                }
            }
            else if (checkBox79.Checked == false)
            {
                timedayid.Remove("179");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("14");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox79.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox79.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox80_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox80.Checked == true && counter <= total)
            {
                timedayid.Add("180");
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("214");
                    time.Add("14");
                    dayLB.Add("Tuesday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("214");
                    time.Add("14");
                    dayLB.Add("Tuesday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "214";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Tuesday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";
                  

                }

                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox80.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox80.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox80.Checked = false;
                    }
                }
            }
            else if (checkBox80.Checked == false)
            {
                timedayid.Remove("180");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("214");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox80.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox80.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox81_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox81.Checked == true && counter <= total)
            {
                timedayid.Add("181");
               
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("314");
                    time.Add("14");
                    dayLB.Add("Wednesday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("314");
                    time.Add("14");
                    dayLB.Add("Wednesday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "314";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Wednesday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";
                  

                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox81.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox81.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox81.Checked = false;
                    }
                }
            }
            else if (checkBox81.Checked == false)
            {
                timedayid.Remove("181");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("314");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox81.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox81.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox82_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox82.Checked == true && counter <= total)
            {
                timedayid.Add("182");
                
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("414");
                    time.Add("14");
                    dayLB.Add("Thursday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("414");
                    time.Add("14");
                    dayLB.Add("Thursday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "414";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Thursday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox82.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox82.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox82.Checked = false;
                    }
                }
            }
            else if (checkBox82.Checked == false)
            {
                timedayid.Remove("182");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("414");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox82.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox82.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox83_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox83.Checked == true && counter <= total)
            {
                timedayid.Add("183");
             
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("514");
                    time.Add("14");
                    dayLB.Add("Friday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("514");
                    time.Add("14");
                    dayLB.Add("Friday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "514";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Friday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }
                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox83.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox83.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox83.Checked = false;
                    }
                }
            }
            else if (checkBox83.Checked == false)
            {
                timedayid.Remove("183");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("514");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox83.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox83.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void checkBox84_CheckedChanged(object sender, EventArgs e)
        {
            // for the image of checkbox
            CheckBox currentCheckBox = (sender as CheckBox);
            if (currentCheckBox.Checked)
            {
                currentCheckBox.Image = Properties.Resources.check_box_with_check_sign;
            }
            else
            {
                currentCheckBox.Image = Properties.Resources.check_box_empty__2_;
            }
            // ------------------------------ //
            if (checkBox84.Checked == true && counter <= total)
            {
                timedayid.Add("184");
          
                counter++;
                if (listBox3.Items.Count == 0 && check == true)
                {
                    day.Add("614");
                    time.Add("14");
                    dayLB.Add("Saturday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == true)
                {
                    day.Add("614");
                    time.Add("14");
                    dayLB.Add("Saturday");
                    timeLB.Add("8:00 - 9:00 PM");
                }
                else if (listBox3.Items.Count != 0 && check == false)
                {
                    int indexDay = day.IndexOf("R");
                    day[indexDay] = "614";
                    time[indexDay] = "14";
                    dayLB[indexDay] = "Saturday";
                    timeLB[indexDay] = "8:00 - 9:00 PM";


                }
                counterindex++;
                lblcounter.Text = counter.ToString();
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                foreach (string items in listBox2.Items)
                {

                    if (items.Contains("Remove"))
                    {
                        check = false;
                        break;
                    }
                    else
                    {
                        check = true;

                    }
                }

                if (btnplot == true)
                {
                    DecisionSupport1();
                    if (cancel == true)
                    {
                        checkBox84.Checked = false;
                    }
                    /*
                    if (Convert.ToInt32(SchedDuplicateForRoom) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && room[counter - 1] != "TBA" && room[counter - 1] != "")
                    {
                        DialogResult dr = MessageBox.Show("There is already day and time plotted for this room, \n Do you want to put TBA to room instead?", "Decision Support System", MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            room[counter - 1] = "TBA";
                        }
                        else
                        {
                            checkBox84.Checked = false;
                        }
                    }
                    */
                    else if (Convert.ToInt32(SchedDuplicateForSection) >= 1 && (sched[counter - 1] != "Consultation Hours" || sched[counter - 1] != "Research And Extension") && semester[counter - 1] != "")
                    {
                        MessageBox.Show("There is already day and time plotted for this section", "DSS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox84.Checked = false;
                    }
                }
            }
            else if (checkBox84.Checked == false)
            {
                timedayid.Remove("184");
                check = false;
                counter--;
                if (counter == -1)
                {
                    counter = 0;
                }
                if (day.Count != 0)
                {
                    int indexDay = day.IndexOf("614");
                    day[indexDay] = "R";
                    time[indexDay] = "R";
                    dayLB[indexDay] = "Remove";
                    timeLB[indexDay] = "Remove";
                    lblcounter.Text = counter.ToString();
                    listBox2.DataSource = null;
                    listBox2.DataSource = timeLB;
                    listBox3.DataSource = null;
                    listBox3.DataSource = dayLB;
                    foreach (string items in listBox2.Items)
                    {

                        if (items.Contains("Remove"))
                        {
                            check = false;
                            break;
                        }
                        else
                        {
                            check = true;

                        }
                    }
                }
            }
            if (checkBox84.Checked == true && counter > total)
            {
                MessageBox.Show("You have exceeded the amount of contact hours","Exceeded Hours",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                checkBox84.Checked = false;
                day.Remove("R");
                time.Remove("R");
                dayLB.Remove("Remove");
                timeLB.Remove("Remove");
                listBox2.DataSource = null;
                listBox2.DataSource = timeLB;
                listBox3.DataSource = null;
                listBox3.DataSource = dayLB;
                check = true;
            }
        }

        private void cbxFaculty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
    