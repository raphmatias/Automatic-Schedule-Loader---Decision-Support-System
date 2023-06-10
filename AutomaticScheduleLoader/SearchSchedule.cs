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
    public partial class SearchSchedule : Form
    {
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";

        string section = "0";
        string jobtype = "0";

        bool search = false;
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
        public SearchSchedule()
        {
            InitializeComponent();
           
        }
        public void PopulateGridViewFaculty() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    if (comboBox1.Text == "Faculty")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        this.dgvFaculty.Columns[0].Width = 150;
                        this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[3].Width = 170;
                        dgvFaculty.AllowUserToAddRows = false;
                        dgvFaculty.Columns["EducAttain"].Visible = false;
                        dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                        dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                        dgvFaculty.Columns[3].HeaderText = "Job Type";
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[3].ReadOnly = true;
                        this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        this.dgvFaculty.MultiSelect = false;
                    }
                    else if (comboBox1.Text == "Room")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                        this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[1].Width = 170;
                        this.dgvFaculty.Columns[2].Width = 170;
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[2].ReadOnly = true;
                        dgvFaculty.Columns["RoomCategory"].HeaderText = "Room Category";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void PopulateGridViewRoom() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                   
                     if (comboBox2.Text == "Room")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where Room like'%"+txtSearch.Text+"%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                        this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[1].Width = 170;
                        this.dgvFaculty.Columns[2].Width = 170;
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[2].ReadOnly = true;

                    }
                   else if (comboBox2.Text == "Room Category")
                    {

                        bool complab = false;
                        bool notcomplab = false;
                        bool otherrooms = false;
                        char[] cl = { 'c', 'o', 'm', 'p', 'u','t', 'e', 'r',' ', 'l','a','b' };
                        char[] notcl = { 'n','o','t',' ','c', 'o', 'm', 'p','u' ,'t', 'e', 'r',' ' ,'l', 'a', 'b' };
                        char[] otherroom = { 'o', 't', 'h', 'e', 'r',' ', 'r', 'o', 'o', 'm', 's' };

                        string search = txtSearch.Text.ToLower();
                        char[] charArr = search.ToCharArray();
              
                        for (int i = 0; i < charArr.Length; i++)
                        {
                    
                            string charStr = charArr[i].ToString();
                            complab = cl.Any(c => charStr.Contains(c));
                            if (complab == false)
                            {
                                break;
                            }
                        }
                   
                        for (int i = 0; i < charArr.Length; i++)
                        {
                       
                            string charStr = charArr[i].ToString();
                            notcomplab = notcl.Any(c => charStr.Contains(c));
                            if (notcomplab == false)
                            {
                                break;

                            }
                        }
                        for (int i = 0; i < charArr.Length; i++)
                        {
                          
                            string charStr = charArr[i].ToString();
                            otherrooms = otherroom.Any(c => charStr.Contains(c));
                            if (otherrooms == false)
                            {
                                break;

                            }
                        }
                     //   MessageBox.Show(complab.ToString() + " "+notcomplab.ToString() + otherrooms.ToString());
                        if (complab == true && notcomplab == false && otherrooms == false)
                        {

                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where RoomCategory ='" + "1" + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                            this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[1].Width = 170;
                            this.dgvFaculty.Columns[2].Width = 170;
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[2].ReadOnly = true;
                        }
                       else if (complab == true && notcomplab == true && otherrooms == true)
                        {

                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where RoomCategory <'" + "3" + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                            this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[1].Width = 170;
                            this.dgvFaculty.Columns[2].Width = 170;
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[2].ReadOnly = true;
                        }
                     else   if (complab == true && notcomplab == true && otherrooms == false)
                        {

                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where RoomCategory >'" + "0" + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                            this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[1].Width = 170;
                            this.dgvFaculty.Columns[2].Width = 170;
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[2].ReadOnly = true;
                        }
                        
                      else  if (complab == false && notcomplab == true && otherrooms == false)
                        {

                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where RoomCategory ='" + "2" + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                            this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[1].Width = 170;
                            this.dgvFaculty.Columns[2].Width = 170;
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[2].ReadOnly = true;
                        }
                    else    if (complab == false && notcomplab == false && otherrooms == true)
                        {

                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where RoomCategory ='" + "0" + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                            this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[1].Width = 170;
                            this.dgvFaculty.Columns[2].Width = 170;
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[2].ReadOnly = true;
                        }
                       
                       
                    }
                    else if (comboBox2.Text == "Course")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT Room,(select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory,Course FROM Room_Tbl where Course like'%" + txtSearch.Text + "%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                        this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[1].Width = 170;
                        this.dgvFaculty.Columns[2].Width = 170;
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[2].ReadOnly = true;

                    }
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
                    if (comboBox2.Text == "Faculty Code")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FacultyCode like '%" + txtSearch.Text + "%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        this.dgvFaculty.Columns[0].Width = 150;
                        this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[3].Width = 170;
                        dgvFaculty.Columns["EducAttain"].Visible = false;
                        dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                        dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                        dgvFaculty.Columns[3].HeaderText = "Job Type";
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[3].ReadOnly = true;
                        this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        this.dgvFaculty.MultiSelect = false;
                    }
                    else if (comboBox2.Text == "Faculty Name")
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FacultyName like '%" + txtSearch.Text + "%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvFaculty.DataSource = dt;
                        dgvFaculty.EnableHeadersVisualStyles = false;
                        this.dgvFaculty.Columns[0].Width = 150;
                        this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        this.dgvFaculty.Columns[3].Width = 170;
                        dgvFaculty.Columns["EducAttain"].Visible = false;
                        dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                        dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                        dgvFaculty.Columns[3].HeaderText = "Job Type";
                        dgvFaculty.Columns[0].ReadOnly = true;
                        dgvFaculty.Columns[1].ReadOnly = true;
                        dgvFaculty.Columns[3].ReadOnly = true;
                        this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        this.dgvFaculty.MultiSelect = false;
                    }
                    else if (comboBox2.Text == "Job Type")
                    {
                        bool fulltime = false;
                        bool parttime = false;
                        char[] ft = { 'f', 'u','l','l','t','i','m','e' };
                        char[] pt = { 'p', 'a', 'r', 't', 't', 'i', 'm', 'e' };
                     
                        string search = txtSearch.Text.ToLower();
                        char[] charArr = search.ToCharArray();
                    
                        for (int i = 0; i < charArr.Length; i++)
                        {
                            string charStr = charArr[i].ToString();
                            fulltime = ft.Any(c => charStr.Contains(c));
                            if(fulltime == false)
                            {
                                break;
                            }
                        }
                        for (int i = 0; i < charArr.Length; i++)
                        {
                            string charStr = charArr[i].ToString();
                            parttime = pt.Any(c => charStr.Contains(c));
                            if (parttime == false)
                            {
                                break;
                                
                            }
                        }

                        if (fulltime == true && parttime == false)
                        {
                            jobtype = "1";
                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FullTime = '" + jobtype + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            this.dgvFaculty.Columns[0].Width = 150;
                            this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[3].Width = 170;
                            dgvFaculty.Columns["EducAttain"].Visible = false;
                            dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                            dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                            dgvFaculty.Columns[3].HeaderText = "Job Type";
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[3].ReadOnly = true;
                            this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            this.dgvFaculty.MultiSelect = false;
                        }
                        else if (fulltime == false && parttime == true)
                        {
                            jobtype = "0";
                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FullTime = '" + jobtype + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            this.dgvFaculty.Columns[0].Width = 150;
                            this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[3].Width = 170;
                            dgvFaculty.Columns["EducAttain"].Visible = false;
                            dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                            dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                            dgvFaculty.Columns[3].HeaderText = "Job Type";
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[3].ReadOnly = true;
                            this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            this.dgvFaculty.MultiSelect = false;
                        }
                        else if (fulltime == true && parttime == true)
                        {
                            jobtype = "2";
                            sqlcon.Open();
                            SqlDataAdapter adapter = new SqlDataAdapter("SELECT FacultyCode,FacultyName,EducAttain,(select case when FullTime = 1 then 'Full Time' when FullTime = 0 then 'Part Time' end) FullTime FROM Faculty_Tbl WHERE FullTime < '" + jobtype + "'", conn);
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            dgvFaculty.DataSource = dt;
                            dgvFaculty.EnableHeadersVisualStyles = false;
                            this.dgvFaculty.Columns[0].Width = 150;
                            this.dgvFaculty.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            this.dgvFaculty.Columns[3].Width = 170;
                            dgvFaculty.Columns["EducAttain"].Visible = false;
                            dgvFaculty.Columns[0].HeaderText = "Faculty Code";
                            dgvFaculty.Columns[1].HeaderText = "Faculty Name";
                            dgvFaculty.Columns[3].HeaderText = "Job Type";
                            dgvFaculty.Columns[0].ReadOnly = true;
                            dgvFaculty.Columns[1].ReadOnly = true;
                            dgvFaculty.Columns[3].ReadOnly = true;
                            this.dgvFaculty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                            this.dgvFaculty.MultiSelect = false;
                        }
                   
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SearchSchedule_Load(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            label14.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            dgvFaculty.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dgvFaculty.Width,
        dgvFaculty.Height, 5, 5));

            button1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button1.Width,
      button1.Height, 30, 30));
            btnClear.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClear.Width,
   btnClear.Height, 30, 30));
            btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width,
    btnSearch.Height, 30, 30));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           
            if (comboBox1.Text == "Faculty")
            {

               // MessageBox.Show(fulltime.ToString() + parttime.ToString());

                PopulateGridViewFacultySearchFCode();
            }
            if(comboBox1.Text == "Section")
            {
              
                        search = true;
                        populateSection();
                
                

            }
            if(comboBox1.Text == "Room")
            {
                PopulateGridViewRoom();
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Faculty")
            {
                label2.Text = "Search by:";
                label2.Visible = true;
                label1.Visible = false;
                label14.Visible = false;
                comboBox2.Visible = true;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                comboBox2.Items.Clear();
                 PopulateGridViewFaculty();
                comboBox2.Items.Add("Faculty Code");
                comboBox2.Items.Add("Faculty Name");
                comboBox2.Items.Add("Job Type");
                
            
            }
            if (comboBox1.Text == "Section")
            {
                label2.Text = "Search course:";
                label1.Text = "Search semester:";
                label14.Text = "Search year:";
                label2.Visible = true;
                label1.Visible = true;
                label14.Visible = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("BSIT");
                comboBox2.Items.Add("BSCS");

                comboBox3.Items.Clear();
                comboBox3.Items.Add("1");
                comboBox3.Items.Add("2");
                comboBox3.Items.Add("3");
                comboBox3.Items.Add("4");

                comboBox4.Items.Clear();
                comboBox4.Items.Add("First Semester");
                comboBox4.Items.Add("Second Semester");

                if (comboBox1.Text == "Section")
                {
                    search = false;
                    populateSection();

                }

            }
            if(comboBox1.Text == "Room")
            {
                PopulateGridViewFaculty();
                label2.Text = "Search by:";
                label2.Visible = true;
                label1.Visible = false;
                label14.Visible = false;
                comboBox2.Visible = true;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
                comboBox2.Items.Clear();
                comboBox2.Items.Add("Room");
                comboBox2.Items.Add("Room Category");
                comboBox2.Items.Add("Course");
            }
        }
        public void populateSection()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query2 = "select SectionSlot FROM Section_Tbl WHERE Course='" + comboBox2.Text + "' AND YearLevel ='" + comboBox3.Text + "'";
                SqlCommand command2 = new SqlCommand(query2, sqlcon);
                SqlDataReader reader2 = command2.ExecuteReader();

                if (reader2.Read() == true)
                {
                    section = reader2["SectionSlot"].ToString();
                }
                reader2.Close();
            }
          //  MessageBox.Show(section);


            DataTable dtable = new DataTable();
            dtable.Columns.Add(new DataColumn("Course"));
            dtable.Columns.Add(new DataColumn("Year Level"));
            dtable.Columns.Add(new DataColumn("Section"));



            DataRow dRow;
            if (search == false)
            {
                for (int i = 0; i < Convert.ToInt32(section); i++)
                {
                  dRow = dtable.Rows.Add(comboBox2.Text, comboBox3.Text, (i + 1));
                   dtable.AcceptChanges();
                    dgvFaculty.DataSource = dtable;
                    dgvFaculty.EnableHeadersVisualStyles = false;
                    dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                    this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvFaculty.Columns[1].Width = 170;
                    this.dgvFaculty.Columns[2].Width = 170;
                    dgvFaculty.Columns[0].ReadOnly = true;
                    dgvFaculty.Columns[1].ReadOnly = true;
                    dgvFaculty.Columns[2].ReadOnly = true;
                }
            }
            else
            {
                if (Convert.ToInt32(txtSearch.Text) <= Convert.ToInt32(section) && txtSearch.Text != "0" && txtSearch.Text != "")
                {
                    dgvFaculty.DataSource = null;
                    dRow = dtable.Rows.Add(comboBox2.Text, comboBox3.Text, (txtSearch.Text));
                    dtable.AcceptChanges();
                    dgvFaculty.DataSource = dtable;
                    dgvFaculty.EnableHeadersVisualStyles = false;
                    dgvFaculty.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSeaGreen;
                    this.dgvFaculty.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dgvFaculty.Columns[1].Width = 170;
                    this.dgvFaculty.Columns[2].Width = 170;
                    dgvFaculty.Columns[0].ReadOnly = true;
                    dgvFaculty.Columns[1].ReadOnly = true;
                    dgvFaculty.Columns[2].ReadOnly = true;
                    search = false;
                }
                else if (Convert.ToInt32(txtSearch.Text) > Convert.ToInt32(section) || txtSearch.Text == "0")
                {
                    dgvFaculty.DataSource = null;
                }
            }

            if(section == "0")
            {
                dgvFaculty.DataSource = null;
            }
     
          
            section = "0";
        }
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Section")
            {
                search = false;
                populateSection();

            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Section")
            {
                search = false;
                populateSection();

            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(comboBox1.Text == "Section")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
         (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }
            }
           
        }

        private void dgvFaculty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SchedFrm schdfrm = new SchedFrm(this);
            schdfrm.ShowDialog();
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Faculty")
            {
                txtSearch.Text = "";
                PopulateGridViewFaculty();
                
            }
            else if (comboBox1.Text == "Section")
            {
                search = false;
                populateSection();
                txtSearch.Text = "";
            }
            else if (comboBox1.Text == "Room")
            {
                txtSearch.Text = "";
                PopulateGridViewFaculty();
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
