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
    public partial class SecFrm : Form
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
        string checker = "0";
        string checkerArchive = "0";
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
        string loginAct = "";
        string typeofAcc = "";
        string Subjcount = "";
        string spSubj = "0";
        bool check = false;
        string numberofSubjslot = "0";
        List<string> subj = new List<string>();
        List<string> subjslot = new List<string>();
        string numberSP = "0";
        List<string> subjscode = new List<string>();
        List<int> ID = new List<int>();
        List<int> IDArchiveSubject = new List<int>();
        public SecFrm()
        {
            InitializeComponent();
         //   this.FormBorderStyle = FormBorderStyle.None;
            
         //   Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        void subjectTblCount()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT SubjectCode FROM Subject_Tbl Where YearLevel ='" + cbxYear.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subj.Add(reader.GetString(0));

                        }
                    }

                    string query1 = "select count(SubjectCode) as numberofSubj From Subject_Tbl Where YearLevel = '" + cbxYear.Text + "'";
                    SqlCommand command = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command.ExecuteReader();

                    if (reader1.Read() == true)
                    {
                        Subjcount = reader1["numberofSubj"].ToString();
                    }
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void SchedulePlotted()
        {
            try
            {
                for (int i = 0; i < Convert.ToInt32(Subjcount); i++)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        string query = "select count(SubjectCode) as numberofSubj From Specialization_Tbl Where SubjectCode = '" + subj[i] + "'";
                        SqlCommand command = new SqlCommand(query, sqlcon);
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read() == true)
                        {
                            spSubj = reader["numberofSubj"].ToString();
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void subjSlot()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();

                string query = "SELECT SubjectSlot,SubjectCode,ID FROM Subject_Tbl WHERE Course='" + comboBox1.Text + "' AND YearLevel= '" + cbxYear.Text + "'";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subjslot.Add(reader.GetString(0));
                        subjscode.Add(reader.GetString(1));
                        ID.Add(reader.GetInt32(2));
                    }
                }

                string query1 = "SELECT ID FROM Subject_Tbl WHERE Course='" + comboBox1.Text + "' AND YearLevel= '" + cbxYear.Text + "'";
                SqlCommand cmd1 = new SqlCommand(query1, sqlcon);
                using (SqlDataReader reader1 = cmd1.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        IDArchiveSubject.Add(reader1.GetInt32(0));

                    }
                }

                string querycont1 = "SELECT COUNT(SubjectSlot) AS Subjslot FROM Subject_Tbl WHERE YearLevel=@YearLevel AND Course=@Course";
                SqlCommand commandcont1 = new SqlCommand(querycont1, sqlcon);
                commandcont1.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                commandcont1.Parameters.AddWithValue("@Course", comboBox1.Text);
                SqlDataReader readercont1 = commandcont1.ExecuteReader();

                if (readercont1.Read() == true)
                {


                    numberofSubjslot = readercont1["Subjslot"].ToString();


                }
                readercont1.Close();
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
        public void PopulateGridViewSection() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Section_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;

                    dataGridView1.AllowUserToAddRows = false;
                }
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Course";
                dataGridView1.Columns[2].HeaderText = "Year level";
                dataGridView1.Columns[3].HeaderText = "Sections";

                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UserCheck()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string querycont = "SELECT COUNT(YearLevel) AS SectionDuplicate FROM Section_Tbl WHERE YearLevel=@YearLevel AND Course=@Course";
                    SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                    commandcont.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                    commandcont.Parameters.AddWithValue("@Course", comboBox1.Text);
                    SqlDataReader readercont = commandcont.ExecuteReader();

                    if (readercont.Read() == true)
                    {


                        checker = readercont["SectionDuplicate"].ToString();


                    }
                    readercont.Close();

                    string querycont1 = "SELECT COUNT(YearLevel) AS SectionDuplicate FROM SectionArchive_Tbl WHERE YearLevel=@YearLevel AND Course=@Course";
                    SqlCommand commandcont1 = new SqlCommand(querycont1, sqlcon);
                    commandcont1.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                    commandcont1.Parameters.AddWithValue("@Course", comboBox1.Text);
                    SqlDataReader readercont1 = commandcont1.ExecuteReader();

                    if (readercont1.Read() == true)
                    {


                        checkerArchive = readercont1["SectionDuplicate"].ToString();


                    }
                    readercont1.Close();
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewSectonSearchCourse() // filter gridview section by course
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,Course,YearLevel,SectionSlot FROM Section_Tbl WHERE Course like '%" + txtSearch.Text + "%'", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "Course";
                    dataGridView1.Columns[2].HeaderText = "Year level";
                    dataGridView1.Columns[3].HeaderText = "Sections";

                    dataGridView1.Columns[1].ReadOnly = true;
                    dataGridView1.Columns[2].ReadOnly = true;
                    dataGridView1.Columns[3].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewSectonSearchYear() // filter gridview section by course
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,Course,YearLevel,SectionSlot FROM Section_Tbl WHERE YearLevel like '%" + txtSearch.Text + "%'", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].HeaderText = "Course";
                    dataGridView1.Columns[2].HeaderText = "Year level";
                    dataGridView1.Columns[3].HeaderText = "Sections";

                    dataGridView1.Columns[1].ReadOnly = true;
                    dataGridView1.Columns[2].ReadOnly = true;
                    dataGridView1.Columns[3].ReadOnly = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SecFrm_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGridViewSection();
                if (dataGridView1.Rows.Count != 0)
                {
                    dataGridView1.Rows[0].Selected = false;

                }
                AdminActivity();
                if (typeofAcc == "1")
                {
                    button2.Enabled = false;
                    btnArchived.Visible = false;
                }
                else
                {

                    btnArchived.Visible = true;
                }
                panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
                panel1.Height, 20, 20));
                /*
                comboBox1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, comboBox1.Width,
                  comboBox1.Height, 5, 5));
                txtSection.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSection.Width,
                txtSection.Height, 5, 5));
                txtSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSearch.Width,
                txtSearch.Height, 5, 5));
                cbxYear.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, cbxYear.Width,
                cbxYear.Height, 5, 5));
                comboBox2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, comboBox2.Width,
                comboBox2.Height, 5, 5));
                */
                btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
                btnSave.Height, 30, 30));
                btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width,
                btnSearch.Height, 30, 30));
                btnArchive.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchive.Width,
              btnArchive.Height, 30, 30));
                button2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button2.Width,
             button2.Height, 30, 30));
                button1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button1.Width,
            button1.Height, 30, 30));
                btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
           btnClose.Height, 30, 30));
                btnArchived.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchived.Width,
           btnArchived.Height, 30, 30));
                dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
           dataGridView1.Height, 5, 5));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void Checker()
        {
            if (comboBox1.Text.Length == 0)
            {
                label8.Visible = true;
                label8.ForeColor = Color.Red;
                label1.ForeColor = Color.Red;
                check = true;
            }

            if (cbxYear.Text.Length == 0)
            {
                label9.Visible = true;
                label9.ForeColor = Color.Red;
                label2.ForeColor = Color.Red;
                check = true;
            }
            if (txtSection.Text.Length == 0)
            {
                label3.Visible = true;
                label3.ForeColor = Color.Red;
                label10.ForeColor = Color.Red;
                check = true;
            }

            if(txtSection.Text.Length != 0 && cbxYear.Text.Length != 0 && comboBox1.Text.Length != 0)
            {
                check = false;
                label8.Visible = false;
                label8.ForeColor = Color.Red;
                label1.ForeColor = Color.Gray;

                label9.Visible = false;
                label9.ForeColor = Color.Red;
                label2.ForeColor = Color.Gray;

                label10.Visible = false;
                label10.ForeColor = Color.Red;
                label3.ForeColor = Color.Gray;
            }
         

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                UserCheck();
                DialogResult dr = MessageBox.Show("Save data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        Checker();
                        if (check == false)
                        {
                            subjSlot();
                            if (Convert.ToInt32(checker) >= 1)
                            {
                                MessageBox.Show("A section for this year level and course is already existing", "Section",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            }
                            else
                            {
                             //   MessageBox.Show(cbxYear.Text);
                                sqlcon.Open();
                                int num = 0;
                                for (int i = 0; i < subjslot.Count; i++)
                                {

                                    
                                }
                                for (int i = 0; i < Convert.ToInt32(numberofSubjslot); i++)
                                {

                                    //     MessageBox.Show(subjscode[i]);
                                    string querycont1 = "SELECT COUNT(ID) AS Subjslot FROM Specialization_Tbl WHERE SubjectCode=@SubjectCode AND Course=@Course";
                                    SqlCommand commandcont1 = new SqlCommand(querycont1, sqlcon);
                                    commandcont1.Parameters.AddWithValue("@SubjectCode", subjscode[i]);
                                    commandcont1.Parameters.AddWithValue("@Course", comboBox1.Text);
                                    SqlDataReader readercont1 = commandcont1.ExecuteReader();

                                    if (readercont1.Read() == true)
                                    {


                                        numberSP = readercont1["Subjslot"].ToString();


                                    }
                                    readercont1.Close();
                                    num = (Convert.ToInt32(txtSection.Text) - Convert.ToInt32(numberSP));

                                    
                                    if (num < 0)
                                    {
                                        MessageBox.Show("There is already a plotted section for this year level and course. Please increase the number of section. ","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                    }
                                    else
                                    {
                                        SqlCommand cmd2 = new SqlCommand("UPDATE Subject_Tbl SET SubjectSlot=@SubjectSlot WHERE ID=@ID", sqlcon);
                                        cmd2.Parameters.AddWithValue("@ID", ID[i]);
                                        cmd2.Parameters.AddWithValue("@Course", comboBox1.Text);
                                        cmd2.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                                            cmd2.Parameters.AddWithValue("@SubjectSlot", num.ToString());
                                        cmd2.ExecuteNonQuery();
                                    }
                                }

                                if (num > 0)
                                {
                                    SqlCommand cmd = new SqlCommand("INSERT INTO Section_Tbl (Course,YearLevel,SectionSlot) VALUES (@Course,@YearLevel,@SectionSlot)", sqlcon);
                                    cmd.Parameters.AddWithValue("@Course", comboBox1.Text);
                                    cmd.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                                    cmd.Parameters.AddWithValue("@SectionSlot", txtSection.Text);
                                    cmd.ExecuteNonQuery();
                                    clear();
                                    lblresult.Text = "Succesfully Saved";
                                    lblresult.ForeColor = Color.Green;
                                    lblresult.Visible = true;

                                    PopulateGridViewSection();

                                    DateTime time = DateTime.Now;
                                    string format = "yyyy-MM-dd";
                                    SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                    cm.Parameters.AddWithValue("@Username", loginAct);
                                    cm.Parameters.AddWithValue("@ActivityLog", loginAct + " added a section");
                                    cm.ExecuteNonQuery();
                                }
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
        void clear()
        {
            cbxYear.Enabled = true;
            comboBox1.Enabled = true;
            comboBox1.Text = txtSection.Text = txtSearch.Text = "";
            comboBox1.SelectedIndex = -1;
            cbxYear.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            PopulateGridViewSection();
            lblresult.Visible = false;
            label8.Visible = false;
            label8.ForeColor = Color.Red;
            label1.ForeColor = Color.Gray;
            btnSave.Enabled = true;
            button2.Enabled = false;
            btnArchive.Enabled = false;
            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
            label9.Visible = false;
            label9.ForeColor = Color.Red;
            label2.ForeColor = Color.Gray;

            label10.Visible = false;
            label10.ForeColor = Color.Red;
            label3.ForeColor = Color.Gray;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //     MessageBox.Show(dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                lblresult.Visible = false;
                cbxYear.Enabled = false;
                btnSave.Enabled = false;
                button2.Enabled = true;
                btnArchive.Enabled = true;

                comboBox1.Enabled = false;
                comboBox1.Text = dataGridView1.CurrentRow.Cells["Course"].Value.ToString();
                cbxYear.Text = dataGridView1.CurrentRow.Cells["YearLevel"].Value.ToString();
                txtSection.Text = dataGridView1.CurrentRow.Cells["SectionSlot"].Value.ToString();
                subjectTblCount();
           
                Checker();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Do you want to save changes?", "Update data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {

                    Checker();
                    if (check == false)
                    {
                        subjSlot();
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                            sqlcon.Open();
                            int num = 0;
                            string sum = "0";
                            for (int i = 0; i < Convert.ToInt32(numberofSubjslot); i++)
                            {
                                //     MessageBox.Show(subjscode[i]);
                                 num = (Convert.ToInt32(dataGridView1.CurrentRow.Cells["SectionSlot"].Value.ToString())) - (Convert.ToInt32(subjslot[i]));
                                sum = ((Convert.ToInt32(txtSection.Text)) - num).ToString();
                                if (Convert.ToInt32(sum) < 0)
                                {
                                    MessageBox.Show("There is already a plotted section for this year level and course. Please increase the number of section. ","Invalid",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    SqlCommand cmd2 = new SqlCommand("UPDATE Subject_Tbl SET SubjectSlot=@SubjectSlot WHERE ID=@ID", sqlcon);
                                    cmd2.Parameters.AddWithValue("@ID", ID[i]);
                                    cmd2.Parameters.AddWithValue("@Course", comboBox1.Text);
                                    cmd2.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                                    cmd2.Parameters.AddWithValue("@SubjectSlot", sum);
                                    cmd2.ExecuteNonQuery();

                                    
                                }
                            }

                            for (int i = 0; i < IDArchiveSubject.Count; i++)
                            {
                                SqlCommand cmd22 = new SqlCommand("UPDATE SubjectArchive_Tbl SET SubjectSlot=@SubjectSlot WHERE ID=@ID", sqlcon);
                                cmd22.Parameters.AddWithValue("@ID", IDArchiveSubject[i]);
                                cmd22.Parameters.AddWithValue("@Course", comboBox1.Text);
                                cmd22.Parameters.AddWithValue("@YearLevel", cbxYear.Text);
                                cmd22.Parameters.AddWithValue("@SubjectSlot", txtSection.Text);
                                cmd22.ExecuteNonQuery();
                            }

                            if (Convert.ToInt32(sum) > 0)
                            {

                                SqlCommand cmd = new SqlCommand("UPDATE Section_Tbl SET SectionSlot=@SectionSlot WHERE ID=@ID", sqlcon);
                                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value.ToString()));
                                cmd.Parameters.AddWithValue("@SectionSlot", txtSection.Text);
                                cmd.ExecuteNonQuery();
                                clear();
                                lblresult.Text = "Succesfully Updated";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                PopulateGridViewSection();

                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a section");
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
                UserCheck();
                DialogResult dr = MessageBox.Show("Archive data?", "Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        if (Convert.ToInt32(checkerArchive) >= 1)
                        {
                            MessageBox.Show("Section already existing in archive", "Section",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        }
                        else
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO SectionArchive_Tbl (Course,YearLevel,SectionSlot) VALUES (@Course,@YearLevel,@SectionSlot)", sqlcon);
                            cmd.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                            cmd.Parameters.AddWithValue("@YearLevel", dataGridView1.CurrentRow.Cells["YearLevel"].Value.ToString());
                            cmd.Parameters.AddWithValue("@SectionSlot", dataGridView1.CurrentRow.Cells["SectionSlot"].Value.ToString());
                            cmd.ExecuteNonQuery();

                            lblresult.Text = "Archived";
                            lblresult.ForeColor = Color.Green;
                            lblresult.Visible = true;

                            SqlCommand cmddel = new SqlCommand("DELETE FROM Section_Tbl WHERE ID =@ID ", sqlcon);
                            cmddel.CommandType = CommandType.Text;
                            cmddel.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value.ToString()));
                            cmddel.ExecuteNonQuery();

                            PopulateGridViewSection();
                            clear();
                            DateTime time = DateTime.Now;
                            string format = "yyyy-MM-dd";
                            SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                            cm.Parameters.AddWithValue("@Username", loginAct);
                            cm.Parameters.AddWithValue("@ActivityLog", loginAct + " archive a section");
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "Course")
            {
                PopulateGridViewSectonSearchCourse();
            }
            else if (comboBox2.Text == "Year")
            {
                PopulateGridViewSectonSearchYear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnArchive.Enabled = false;
            btnSave.Enabled = true;
            button2.Enabled = false;
            clear();
            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnArchived_Click(object sender, EventArgs e)
        {
            SectionArchived SA = new SectionArchived(this);
            SA.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            label8.Visible = false;
            label1.ForeColor = Color.Gray;
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void cbxYear_MouseClick(object sender, MouseEventArgs e)
        {
            label9.Visible = false;
            label2.ForeColor = Color.Gray;
        }

        private void cbxYear_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void txtSection_MouseClick(object sender, MouseEventArgs e)
        {
            label10.Visible = false;
            label3.ForeColor = Color.Gray;
        }
        private void txtSection_Leave(object sender, EventArgs e)
        {
            Checker();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                e.Handled = true;
            txtSection.MaxLength = 1;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
