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
   
    public partial class RoomFrm : Form
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
        string checkerUpdate = "0";
        string totalroomscateg = "0";
        string otherrooms = "0";
        string complab = "1";
        string notcomplab = "2";
        string roomcateg = "0";
        string loginAct = "";
        string typeofAcc = "";
        string spSubj = "";
        string numCateg = "0";
        string staticroom = "";
        List<string> IDUPDATEF = new List<string>();
        List<string> IDUPDATES = new List<string>();

        List<string> ID = new List<string>();
        List<string> IDsched = new List<string>();
        List<string> IDsched2 = new List<string>();
        List<string> roomcategory = new List<string>();
        List<string> sectionSched = new List<string>();

        List<string> semester = new List<string>();
        List<string> idchange = new List<string>();

        List<string> idSPAfter = new List<string>();
        List<string> sectionAfter = new List<string>();
        List<string> subjectAfter = new List<string>();

        List<string> idSPBefore = new List<string>();
        List<string> sectionBefore = new List<string>();
        List<string> subjectBefore = new List<string>();

        List<string> roomAfter = new List<string>();
        List<string> roomBefore = new List<string>();

        List<string> idFSchedAfter = new List<string>();
        List<string> FschedsectionAfter = new List<string>();
        List<string> FschedsubjectAfter = new List<string>();
        List<string> timeIDAfter = new List<string>();
        List<string> dayIDAfter = new List<string>();
        List<string> idChecker = new List<string>();

        List<string> idFSchedBefore = new List<string>();
        List<string> FschedsectionBefore = new List<string>();
        List<string> FschedsubjectBefore = new List<string>();
        List<string> timeIDBefore = new List<string>();
        List<string> dayIDBefore = new List<string>();
        List<string> idChecker2 = new List<string>();

        List<string> IDArchive = new List<string>();
        int roomcateg1 = 0;
        string roomCATEGORYB4Edit = "";
        public RoomFrm()
        {
            InitializeComponent();
           // this.FormBorderStyle = FormBorderStyle.None;
          //  Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        void SchedulePlotted()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "select count(SubjectCode) as numberofSubj From Specialization_Tbl Where Room = '" + textBox1.Text + "'";
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
        public void subjpercateg()
        {
            string roomtype = "";
            if(dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Other Rooms")
            {
                roomtype = otherrooms;
            }
          else  if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Computer Lab")
            {
                roomtype = complab;
            }
          else  if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Not Computer Lab")
            {
                roomtype = notcomplab;
            }
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();

                string query = "SELECT ID,RoomCategory FROM Specialization_Tbl Where   Course=@Course AND Room=@Room ";
                SqlCommand cmd = new SqlCommand(query, sqlcon);
                cmd.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                cmd.Parameters.AddWithValue("@Room", dataGridView1.CurrentRow.Cells["Room"].Value.ToString());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ID.Add(reader.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));
                        roomcategory.Add(reader.GetString(1));
                    }
                }



                string query5 = "SELECT ID FROM FacultySchedule_Tbl Where   Course=@Course AND RoomCategory=@RoomCategory ";
                SqlCommand cmd5 = new SqlCommand(query5, sqlcon);
                cmd5.Parameters.AddWithValue("@Course", comboBox2.Text);
                cmd5.Parameters.AddWithValue("@RoomCategory", roomcateg1.ToString());
                using (SqlDataReader reader5 = cmd5.ExecuteReader())
                {
                    while (reader5.Read())
                    {
                        IDsched.Add(reader5.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));
                     
                    }
                }

                string query6 = "SELECT ID FROM FacultySchedule_Tbl Where   Course=@Course AND RoomCategory=@RoomCategory ";
                SqlCommand cmd6 = new SqlCommand(query6, sqlcon);
                cmd6.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                cmd6.Parameters.AddWithValue("@RoomCategory", roomCATEGORYB4Edit);
                using (SqlDataReader reader6 = cmd6.ExecuteReader())
                {
                    while (reader6.Read())
                    {
                        IDsched2.Add(reader6.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));

                    }
                }

                string query7 = "SELECT ID FROM FacultySchedule_Tbl Where   Course=@Course AND Room=@Room ";
                SqlCommand cmd7 = new SqlCommand(query7, sqlcon);
                cmd7.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                cmd7.Parameters.AddWithValue("@Room", dataGridView1.CurrentRow.Cells["Room"].Value.ToString());
                using (SqlDataReader reader7 = cmd7.ExecuteReader())
                {
                    while (reader7.Read())
                    {
                        IDUPDATEF.Add(reader7.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));

                    }
                }
            }
            }
        public void SPEDITROOM()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
              

                string query1 = "SELECT ID,Section,SubjectCode FROM Specialization_Tbl Where   Course=@Course AND RoomCategory=@RoomCategory ";
                SqlCommand cmd1 = new SqlCommand(query1, sqlcon);
                cmd1.Parameters.AddWithValue("@Course", comboBox2.Text);
                cmd1.Parameters.AddWithValue("@RoomCategory", roomcateg1.ToString());
                using (SqlDataReader reader1 = cmd1.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        idSPAfter.Add(reader1.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));
                        sectionAfter.Add(reader1.GetString(1));
                        subjectAfter.Add(reader1.GetString(2));
                    }
                }

                string query2 = "SELECT ID,Section,SubjectCode FROM Specialization_Tbl Where   Course=@Course AND RoomCategory=@RoomCategory ";
                SqlCommand cmd2 = new SqlCommand(query2, sqlcon);
                cmd2.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                cmd2.Parameters.AddWithValue("@RoomCategory", roomCATEGORYB4Edit);
                using (SqlDataReader reader2 = cmd2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        idSPBefore.Add(reader2.GetInt32(0).ToString());
                        //    course.Add(reader.GetString(1));
                        sectionBefore.Add(reader2.GetString(1));
                        subjectBefore.Add(reader2.GetString(2));
                    }
                }

               

            }
            }
        public void TotalOfRooms()
        {
            try
            {

         
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query3 = "SELECT COUNT(Room) AS NumberOfRoom FROM Room_Tbl WHERE  RoomCategory=@RoomCategory AND Course=@Course";
                SqlCommand command3 = new SqlCommand(query3, sqlcon);

                    command3.Parameters.AddWithValue("@Course", comboBox2.Text);
                    if (rbnonmajor.Checked == true)
                {
                    command3.Parameters.AddWithValue("@RoomCategory", otherrooms);
                }
                else
                {
                    if (rbYes.Checked == true)
                    {
                        command3.Parameters.AddWithValue("@RoomCategory", complab);
                    }
                    else if (rbNo.Checked == true)
                    {
                        command3.Parameters.AddWithValue("@RoomCategory", notcomplab);
                    }
                }
                SqlDataReader reader3 = command3.ExecuteReader();

                if (reader3.Read() == true)
                {


                    totalroomscateg = reader3["NumberOfRoom"].ToString();


                }
                reader3.Close();
                sqlcon.Close();
            }
            }catch (Exception ex)
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
                    string querycont = "SELECT COUNT(Room) AS RoomDuplicate FROM Room_Tbl WHERE Room=@Room AND Course=@Course";
                    SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                    commandcont.Parameters.AddWithValue("@Room", textBox1.Text);
                    commandcont.Parameters.AddWithValue("@Course", comboBox2.Text);
                    SqlDataReader readercont = commandcont.ExecuteReader();

                    if (readercont.Read() == true)
                    {


                        checker = readercont["RoomDuplicate"].ToString();


                    }
                    readercont.Close();
                    string querycont1 = "SELECT COUNT(Room) AS RoomDuplicate FROM RoomArchive_Tbl WHERE Room=@Room AND Course=@Course";
                    SqlCommand commandcont1 = new SqlCommand(querycont1, sqlcon);
                    commandcont1.Parameters.AddWithValue("@Room", textBox1.Text);
                    commandcont1.Parameters.AddWithValue("@Course", comboBox2.Text);
                    SqlDataReader readercont1 = commandcont1.ExecuteReader();

                    if (readercont1.Read() == true)
                    {


                        checkerArchive = readercont1["RoomDuplicate"].ToString();


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
   
        public void UserCheckUpdate()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string querycont = "SELECT COUNT(Room) AS RoomDuplicate FROM Room_Tbl WHERE Room = @Room AND Course=@Course AND ID != @ID";
                    SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                    commandcont.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells["ID"].Value.ToString());
                    commandcont.Parameters.AddWithValue("@RoomID", dataGridView1.CurrentRow.Cells["RoomID"].Value.ToString());
                    commandcont.Parameters.AddWithValue("@Room", textBox1.Text);
                    commandcont.Parameters.AddWithValue("@Course", comboBox2.Text);
                    SqlDataReader readercont = commandcont.ExecuteReader();

                    if (readercont.Read() == true)
                    {


                        checkerUpdate = readercont["RoomDuplicate"].ToString();


                    }
                    readercont.Close();

                    sqlcon.Close();
                }
            }
            catch (Exception ex) {
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

                    string query1 = "select RoomCategory FROM Room_Tbl WHERE Room='" + textBox1.Text + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {



                        roomcateg = reader1["RoomCategory"].ToString();

                    }
                    reader1.Close();

                    
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
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,RoomID,Room,Course, (select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory FROM Room_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.AllowUserToAddRows = false;
                    this.dataGridView1.Columns["ID"].Visible = false;
                    this.dataGridView1.Columns["RoomID"].Visible = false;
                    this.dataGridView1.Columns["RoomCategory"].HeaderText = "Room Category";

                    this.dataGridView1.Columns["Room"].ReadOnly = true;
                    this.dataGridView1.Columns["Course"].ReadOnly = true;
                    this.dataGridView1.Columns["RoomCategory"].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RoomFrm_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGridViewRoom();

                groupBox5.Enabled = false;
                if (dataGridView1.Rows.Count != 0)
                {
                    clear();
                }
                AdminActivity();
                if (typeofAcc == "1")
                {
                    button1.Enabled = false;
                    btnArchived.Visible = false;
                }
                else
                {
                    btnArchived.Visible = true;
                }

                panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width,
                panel1.Height, 20, 20));
                /*
                textBox1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, textBox1.Width,
               textBox1.Height, 15, 15));
                textBox2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, textBox2.Width,
              textBox2.Height, 15, 15));
                comboBox2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, comboBox2.Width,
             comboBox2.Height, 15, 15));

                */
                btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width,
                btnSave.Height, 30, 30));
                button3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button3.Width,
                button3.Height, 30, 30));
                button4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button4.Width,
              button4.Height, 30, 30));
                button2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button2.Width,
             button2.Height, 30, 30));
                button1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button1.Width,
            button1.Height, 30, 30));
                button5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button5.Width,
           button5.Height, 30, 30));
                btnArchived.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnArchived.Width,
           btnArchived.Height, 30, 30));
                dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
           dataGridView1.Height, 5, 5));
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateGridViewSearchRoom() // filter gridview section by course
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    if (comboBox1.Text.Equals("Room"))
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,RoomID,Room,Course, (select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory FROM Room_Tbl WHERE Room like '%" + textBox2.Text + "%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;
                        this.dataGridView1.Columns["ID"].Visible = false;
                        this.dataGridView1.Columns["RoomID"].Visible = false;
                        this.dataGridView1.Columns["RoomCategory"].HeaderText = "Room Category";

                        this.dataGridView1.Columns["Room"].ReadOnly = true;
                        this.dataGridView1.Columns["Course"].ReadOnly = true;
                        this.dataGridView1.Columns["RoomCategory"].ReadOnly = true;
                    }
                    else if (comboBox1.Text.Equals("Course"))
                    {
                        sqlcon.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,RoomID,Room,Course, (select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory FROM Room_Tbl WHERE Course like '%" + textBox2.Text + "%'", conn);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                        this.dataGridView1.Columns["ID"].Visible = false;
                        this.dataGridView1.Columns["RoomID"].Visible = false;
                        this.dataGridView1.Columns["RoomCategory"].HeaderText = "Room Category";

                        this.dataGridView1.Columns["Room"].ReadOnly = true;
                        this.dataGridView1.Columns["Course"].ReadOnly = true;
                        this.dataGridView1.Columns["RoomCategory"].ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PopulateGridViewSearchRoom();
        }
        void Checker()
        {
            if (textBox1.Text.Equals(""))
            {
                label5.Visible = true;
                label5.ForeColor = Color.Red;
                label1.ForeColor = Color.Red;
            }

            if (comboBox2.Text.Equals(""))
            {
                label6.Visible = true;
                label6.ForeColor = Color.Red;
                Course.ForeColor = Color.Red;
            }
            if (rbmajor.Checked == false && rbnonmajor.Checked == false)
            {
                label7.Visible = true;
                label7.ForeColor = Color.Red;
                label3.ForeColor = Color.Red;
            }

            if (rbYes.Checked == false && rbNo.Checked == false && rbmajor.Checked == true)
            {
                label8.Visible = true;
                label8.ForeColor = Color.Red;
                label4.ForeColor = Color.Red;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult dr = MessageBox.Show("Save data?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    TotalOfRooms();
                    int total1 = Convert.ToInt32(totalroomscateg) + 1;
                    using (SqlConnection sqlcon = new SqlConnection(conn))

                    {
                        if (textBox1.Text.Length == 0 || comboBox2.Text.Length == 0)
                        {
                            Checker();
                        }
                        if (rbmajor.Checked == false && rbnonmajor.Checked == false)
                        {
                            Checker();
                        }
                        if (rbmajor.Checked == true && rbYes.Checked == false && rbNo.Checked == false)
                        {
                            Checker();
                        }
                        else
                        {
                            UserCheck();
                            if (Convert.ToInt32(checker) >= 1)
                            {
                                MessageBox.Show("Room already existing", "Room",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            }
                            else
                            {
                                sqlcon.Open();
                                SqlCommand cmd = new SqlCommand("INSERT INTO Room_Tbl (RoomID,Room,RoomCategory,Course) VALUES (@RoomID,@Room,@RoomCategory,@Course)", sqlcon);
                                if (rbnonmajor.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@RoomID", total1);
                                    cmd.Parameters.AddWithValue("@RoomCategory", otherrooms);
                                    cmd.Parameters.AddWithValue("@Room", textBox1.Text);
                                    cmd.Parameters.AddWithValue("@Course", comboBox2.Text);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    if (rbYes.Checked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@RoomID", total1);
                                        cmd.Parameters.AddWithValue("@RoomCategory", complab);

                                    }
                                    else if (rbNo.Checked == true)
                                    {
                                        cmd.Parameters.AddWithValue("@RoomID", total1);
                                        cmd.Parameters.AddWithValue("@RoomCategory", notcomplab);
                                    }
                                    cmd.Parameters.AddWithValue("@Room", textBox1.Text);
                                    cmd.Parameters.AddWithValue("@Course", comboBox2.Text);
                                    cmd.ExecuteNonQuery();
                                }
                                clear();
                                lblresult.Text = "Succesfully Saved";
                                lblresult.ForeColor = Color.Green;
                                lblresult.Visible = true;
                                label5.Visible = false;
                                label1.ForeColor = Color.Gray;
                                label6.Visible = false;
                                Course.ForeColor = Color.Gray;
                                label7.Visible = false;
                                label3.ForeColor = Color.Gray;
                                label8.Visible = false;
                                label4.ForeColor = Color.Gray;
                                PopulateGridViewRoom();
                                button1.Enabled = false;
                                button2.Enabled = false;


                                DateTime time = DateTime.Now;
                                string format = "yyyy-MM-dd";
                                SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                                cm.Parameters.AddWithValue("@Username", loginAct);
                                cm.Parameters.AddWithValue("@ActivityLog", loginAct + " added a room");
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

        private void button1_Click(object sender, EventArgs e)
        {
            
                string room = "";
                DialogResult dr = MessageBox.Show("If you have plotted schedule with this room, the details from that schedule will be changed. Do you still want to continue?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    UserCheckUpdate();
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (textBox1.Text.Length == 0 || comboBox2.Text.Length == 0)
                    {
                        Checker();
                    }
                    if (rbmajor.Checked == false && rbnonmajor.Checked == false)
                    {
                        Checker();
                    }
                    if (rbmajor.Checked == true && rbYes.Checked == false && rbNo.Checked == false)
                    {
                        Checker();
                    }
                    else
                    {
                        if (Convert.ToInt32(checkerUpdate) >= 1)
                        {
                            MessageBox.Show("Room already existing", "Room",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        }
                        else
                        {
                            subjpercateg();
                            
                           
                            string roomTblB4 = "";
                            string roomTblAfter = "";
                           
                            if(rbmajor.Checked == true && rbYes.Checked == true)
                            {
                                roomcateg1 = 1;
                            }
                           else if (rbmajor.Checked == true && rbNo.Checked == true)
                            {
                                roomcateg1 = 2;
                            }
                           else if (rbnonmajor.Checked == true)
                            {
                                roomcateg1 = 0;
                            }
                            
                            if(dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Computer Lab")
                            {
                                roomCATEGORYB4Edit = "1";
                            }
                          else  if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Not Computer Lab")
                            {
                                roomCATEGORYB4Edit = "2";
                            }
                        else    if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Other Rooms")
                            {
                                roomCATEGORYB4Edit = "0";
                            }
                            SqlCommand cmd = new SqlCommand("UPDATE Room_Tbl SET Room=@Room,Course=@Course,RoomCategory=@RoomCategory WHERE ID = @ID", sqlcon);
                            cmd.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells["ID"].Value);
                            if (rbnonmajor.Checked == true)
                            {
                                cmd.Parameters.AddWithValue("@RoomCategory", otherrooms);
                                cmd.Parameters.AddWithValue("@Room", textBox1.Text);
                                cmd.Parameters.AddWithValue("@Course", comboBox2.Text);
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                if (rbYes.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@RoomCategory", complab);

                                }
                                else if (rbNo.Checked == true)
                                {
                                    cmd.Parameters.AddWithValue("@RoomCategory", notcomplab);
                                }
                                cmd.Parameters.AddWithValue("@Room", textBox1.Text);
                                cmd.Parameters.AddWithValue("@Course", comboBox2.Text);
                                cmd.ExecuteNonQuery();
                            }
                            cmd.ExecuteNonQuery();

                            TotalOfRooms();
                            int total1 = Convert.ToInt32(totalroomscateg);
                            SqlCommand cmdd = new SqlCommand("UPDATE Room_Tbl SET RoomID=@RoomID WHERE ID = @ID", sqlcon);
                            cmdd.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells["ID"].Value);
                            cmdd.Parameters.AddWithValue("@RoomID", total1.ToString());
                            cmdd.ExecuteNonQuery();


                            string queryyyy2 = "SELECT ID FROM Room_Tbl Where RoomCategory ='" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "'";
                            SqlCommand cmdddd2 = new SqlCommand(queryyyy2, sqlcon);
                            using (SqlDataReader readerrrr2 = cmdddd2.ExecuteReader())
                            {
                                while (readerrrr2.Read())
                                {
                                    idchange.Add(readerrrr2.GetInt32(0).ToString());
                                }
                            }
                          
                            // automatically change the room id if the current room changed its room category
                            for (int i = 0;i< idchange.Count; i++)
                            {
                                SqlCommand cmdd1 = new SqlCommand("UPDATE Room_Tbl SET RoomID=@RoomID WHERE ID = @ID", sqlcon);
                                cmdd1.Parameters.AddWithValue("@ID", idchange[i]);
                                cmdd1.Parameters.AddWithValue("@RoomID", (i+1));
                                cmdd1.ExecuteNonQuery();
                            }

                            string numberofSP1 = "0";
                            string queryyy1 = "select count(FacultyCode) as numberofSPperCateg From Specialization_Tbl Where RoomCategory = '" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "'";
                            SqlCommand commanddd1 = new SqlCommand(queryyy1, sqlcon);
                            SqlDataReader readerrr1 = commanddd1.ExecuteReader();

                            if (readerrr1.Read() == true)
                            {
                                numberofSP1 = readerrr1["numberofSPperCateg"].ToString();
                            }
                            readerrr1.Close();

                            string numberofSP2 = "0";
                            string queryyy2 = "select count(FacultyCode) as numberofSPperCateg From Specialization_Tbl Where RoomCategory = '" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "'";
                            SqlCommand commanddd2 = new SqlCommand(queryyy2, sqlcon);
                            SqlDataReader readerrr2 = commanddd2.ExecuteReader();

                            if (readerrr2.Read() == true)
                            {
                                numberofSP2 = readerrr2["numberofSPperCateg"].ToString();
                            }
                            readerrr2.Close();


                    
                            // number of room before edit 

                            subjpercateg();

                            if (roomcateg1.ToString() == roomCATEGORYB4Edit && dataGridView1.CurrentRow.Cells["Course"].Value.ToString() == comboBox2.Text)
                            {
                           // for specialization table
                                for (int i = 0; i < ID.Count; i++)
                                {
                                    SqlCommand cmdddd1 = new SqlCommand("UPDATE Specialization_Tbl SET Room=@Room WHERE ID = @ID", sqlcon);
                                    cmdddd1.Parameters.AddWithValue("@ID", ID[i]);
                                    cmdddd1.Parameters.AddWithValue("@Room", textBox1.Text);

                                    cmdddd1.ExecuteNonQuery();
                                }
                                // for faculty schedule table
                                for (int i = 0; i < IDsched.Count; i++)
                                {
                                    SqlCommand cmdddd1 = new SqlCommand("UPDATE FacultySchedule_Tbl SET Room=@Room WHERE ID = @ID", sqlcon);
                                    cmdddd1.Parameters.AddWithValue("@ID", IDUPDATEF[i]);
                                    cmdddd1.Parameters.AddWithValue("@Room", textBox1.Text);
                                    cmdddd1.ExecuteNonQuery();
                                }

                            }
                            else 
                            {
                              
                                SPEDITROOM();
                                int roomNumAfter = 1;
                                int roomNumB4 = 1;
                                string numberofRoom = "0";
                                string queryyy = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "'";
                                SqlCommand commanddd = new SqlCommand(queryyy, sqlcon);
                                SqlDataReader readerrr = commanddd.ExecuteReader();

                                if (readerrr.Read() == true)
                                {
                                    numberofRoom = readerrr["numberOfroom"].ToString();
                                }
                                readerrr.Close();
                                string defaultnumberofroomSP = numberofRoom;

                                // number of room after edit

                                string numberofRoomAfter = "0";
                                string queryyy3 = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "'";
                                SqlCommand commanddd3 = new SqlCommand(queryyy3, sqlcon);
                                SqlDataReader readerrr3 = commanddd3.ExecuteReader();

                                if (readerrr3.Read() == true)
                                {
                                    numberofRoomAfter = readerrr3["numberOfroom"].ToString();
                                }
                                readerrr3.Close();
                                string defaultnumberofroomSPAfter = numberofRoom;
                         
                            
                                // b4 editing update the previous rooms records from specialization 
                                for (int i = 0; i< idSPBefore.Count; i++)
                                {
                                    string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "' AND RoomID='" + roomNumB4.ToString() + "'";
                                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                    SqlDataReader reader1 = command1.ExecuteReader();

                                    if (reader1.Read() == true)
                                    {



                                        roomTblB4 = reader1["Room"].ToString();

                                    }
                                    reader1.Close();

                                    SqlCommand cmdddd1 = new SqlCommand("UPDATE Specialization_Tbl SET Room=@Room WHERE ID = @ID", sqlcon);
                                    cmdddd1.Parameters.AddWithValue("@ID", idSPBefore[i]);
                                    cmdddd1.Parameters.AddWithValue("@Room", roomTblB4);
                                    if (numberofRoom != "1")
                                    {
                                        roomNumB4 += 1;
                                         numberofRoom = roomNumB4.ToString();
                                    }
                                    if ( roomNumB4 > Convert.ToInt32(numberofRoom))
                                    {
                                        roomNumB4 = 1;
                                    }
                                    cmdddd1.ExecuteNonQuery();
                                    roomBefore.Add(roomTblB4);
                                }
                           
                                for (int i =0; i < idSPAfter.Count; i++)
                                {
                                    string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "' AND RoomID='" + roomNumAfter.ToString() + "'";
                                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                    SqlDataReader reader1 = command1.ExecuteReader();

                                    if (reader1.Read() == true)
                                    {



                                        roomTblAfter = reader1["Room"].ToString();

                                    }
                                    reader1.Close();
                                //    MessageBox.Show(roomNumAfter.ToString());
                                    SqlCommand cmdddd1 = new SqlCommand("UPDATE Specialization_Tbl SET Room=@Room WHERE ID = @ID ", sqlcon);
                                    cmdddd1.Parameters.AddWithValue("@ID", idSPAfter[i].ToString());
                                    cmdddd1.Parameters.AddWithValue("@Room", roomTblAfter);
                                    if (numberofRoomAfter != "1")
                                    {
                                        roomNumAfter += 1;
                                    }
                                    if ( roomNumAfter > Convert.ToInt32(numberofRoomAfter))
                                    {
                                        roomNumAfter = 1;
                                    }
                                    cmdddd1.ExecuteNonQuery();
                                    roomAfter.Add(roomTblAfter);
                                }
                                
                                int numcount = 0;
                                string SchedDuplicateForRoom = "0";
                                bool duplicate = false;
                                string numberofsubj = "0";
                                int subjCountRepeat = 0;
                                bool duplicateRoomFound = false;
                                for (int i = 0; i < IDsched.Count; i++)
                                {
                               //     MessageBox.Show("ASD");
                                    string query44 = "SELECT COUNT(ID) AS numberSubj FROM FacultySchedule_Tbl WHERE SubjectCode=@SubjectCode AND Section=@Section AND Course=@Course";
                                    SqlCommand command44 = new SqlCommand(query44, sqlcon);
                                    command44.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                                    command44.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                                    command44.Parameters.AddWithValue("@Course", comboBox2.Text);
                                    SqlDataReader reader44 = command44.ExecuteReader();

                                    if (reader44.Read() == true)
                                    {


                                        numberofsubj = reader44["numberSubj"].ToString();


                                    }
                                    reader44.Close();

                                    string query3 = "SELECT ID,TimeID,DayID FROM FacultySchedule_Tbl Where   Course=@Course AND Section=@Section AND SubjectCode=@SubjectCode ";
                                    SqlCommand cmd3 = new SqlCommand(query3, sqlcon);
                                    cmd3.Parameters.AddWithValue("@Course", comboBox2.Text);
                                    cmd3.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                                    cmd3.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                                    using (SqlDataReader reader3 = cmd3.ExecuteReader())
                                    {
                                        while (reader3.Read())
                                        {
                                            idChecker.Add(reader3.GetInt32(0).ToString());
                                            //    course.Add(reader.GetString(1));
                                            timeIDAfter.Add(reader3.GetString(1));
                                            dayIDAfter.Add(reader3.GetString(2));
                                        }
                                    }
                                    string roomTBL = "";
                                    string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "' AND RoomID='" + numberofRoomAfter + "'";
                                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                    SqlDataReader reader1 = command1.ExecuteReader();

                                    if (reader1.Read() == true)
                                    {



                                        roomTBL = reader1["Room"].ToString();

                                    }
                                    reader1.Close();

                                    do
                                    {
                                   //    MessageBox.Show(numberofsubj);
                                        SqlCommand cmdddd1 = new SqlCommand("UPDATE FacultySchedule_Tbl SET Room=@Room Where Course=@Course AND SubjectCode=@SubjectCode AND Section=@Section AND TimeID=@TimeID AND DayID=@DayID", sqlcon);
                                        cmdddd1.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                                        cmdddd1.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                                        cmdddd1.Parameters.AddWithValue("@DayID", dayIDAfter[subjCountRepeat]);
                                        cmdddd1.Parameters.AddWithValue("@TimeID", timeIDAfter[subjCountRepeat]);
                                        cmdddd1.Parameters.AddWithValue("@Course", comboBox2.Text);

                                       // MessageBox.Show(idChecker[subjCountRepeat] + " "+ dayIDAfter[subjCountRepeat] +" "+ timeIDAfter[subjCountRepeat]);
                                        string query4 = "SELECT COUNT(ID) AS NumberOfDuplicateForRoom FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Room=@Room AND ID != ID";
                                        SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                        command4.Parameters.AddWithValue("@ID", idChecker[subjCountRepeat]);
                                        command4.Parameters.AddWithValue("@DayID", dayIDAfter[subjCountRepeat]);
                                        command4.Parameters.AddWithValue("@TimeID", timeIDAfter[subjCountRepeat]);
                                        if(duplicateRoomFound == false)
                                        {
                                            command4.Parameters.AddWithValue("@Room", roomAfter[numcount]);
                                        }
                                        else
                                        {
                                            command4.Parameters.AddWithValue("@Room", roomTBL);
                                        }
                                      

                                        SqlDataReader reader4 = command4.ExecuteReader();

                                        if (reader4.Read() == true)
                                        {


                                            SchedDuplicateForRoom = reader4["NumberOfDuplicateForRoom"].ToString();


                                        }
                                        reader4.Close();
                                        if(Convert.ToInt32(SchedDuplicateForRoom) >= 1)
                                        {
                                           
                                            if(numberofRoomAfter != "1")
                                            {
                                                int num = Convert.ToInt32(numberofRoomAfter) - 1;
                                                numberofRoomAfter = num.ToString();
                                                duplicateRoomFound = true;
                                                duplicate = true;
                                            }
                                            if(numberofRoomAfter == "1")
                                            {
                                                cmdddd1.Parameters.AddWithValue("@Room", "TBA");
                                                duplicateRoomFound = false;
                                                duplicate = false;
                                            }
                                        }
                                        else if (duplicateRoomFound == false && Convert.ToInt32(SchedDuplicateForRoom) == 0)
                                        {
                                            cmdddd1.Parameters.AddWithValue("@Room", roomAfter[numcount]);
                                            duplicate = false;
                                            duplicateRoomFound = false;
                                        }
                                        else if (duplicateRoomFound == true && Convert.ToInt32(SchedDuplicateForRoom) == 0)
                                        {
                                            cmdddd1.Parameters.AddWithValue("@Room", roomTBL);
                                            duplicate = false;
                                            duplicateRoomFound = false;
                                        }
                                        cmdddd1.ExecuteNonQuery();
                                    } while (duplicate == true);
                               
                                    subjCountRepeat += 1;
                                   
                                    if (subjCountRepeat == Convert.ToInt32(numberofsubj))
                                    {
                                        subjCountRepeat = 0;
                                       // 
                                        if (numcount < sectionAfter.Count  )
                                        {
                                         //   MessageBox.Show("ASD");
                                            numcount += 1;
                                            
                                        }
                                        dayIDAfter.Clear();
                                        timeIDAfter.Clear();
                                    }

                                
                                }

                                int numcount2 = 0;
                                string SchedDuplicateForRoom2 = "0";
                                bool duplicate2 = false;
                                string numberofsubj2 = "0";
                                int subjCountRepeat2 = 0;
                                bool duplicateRoomFound2 = false;

                                for (int i = 0; i < IDsched2.Count; i++)
                                {
                                    //     MessageBox.Show("ASD");
                                    string query44 = "SELECT COUNT(ID) AS numberSubj FROM FacultySchedule_Tbl WHERE SubjectCode=@SubjectCode AND Section=@Section AND Course=@Course";
                                    SqlCommand command44 = new SqlCommand(query44, sqlcon);
                                    command44.Parameters.AddWithValue("@SubjectCode", subjectBefore[numcount2]);
                                    command44.Parameters.AddWithValue("@Section", sectionBefore[numcount2]);
                                    command44.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                                    SqlDataReader reader44 = command44.ExecuteReader();

                                    if (reader44.Read() == true)
                                    {


                                        numberofsubj2 = reader44["numberSubj"].ToString();


                                    }
                                    reader44.Close();

                                    string query3 = "SELECT ID,TimeID,DayID FROM FacultySchedule_Tbl Where   Course=@Course AND Section=@Section AND SubjectCode=@SubjectCode ";
                                    SqlCommand cmd3 = new SqlCommand(query3, sqlcon);
                                    cmd3.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                                    cmd3.Parameters.AddWithValue("@SubjectCode", subjectBefore[numcount2]);
                                    cmd3.Parameters.AddWithValue("@Section", sectionBefore[numcount2]);
                                    using (SqlDataReader reader3 = cmd3.ExecuteReader())
                                    {
                                        while (reader3.Read())
                                        {
                                            idChecker2.Add(reader3.GetInt32(0).ToString());
                                            //    course.Add(reader.GetString(1));
                                            timeIDBefore.Add(reader3.GetString(1));
                                            dayIDBefore.Add(reader3.GetString(2));
                                        }
                                    }
                                    string roomTBL = "";
                                    string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "' AND RoomID='" + numberofRoom + "'";
                                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                                    SqlDataReader reader1 = command1.ExecuteReader();

                                    if (reader1.Read() == true)
                                    {



                                        roomTBL = reader1["Room"].ToString();

                                    }
                                    reader1.Close();

                                    do
                                    {
                                        //    MessageBox.Show(numberofsubj);
                                        SqlCommand cmdddd1 = new SqlCommand("UPDATE FacultySchedule_Tbl SET Room=@Room Where Course=@Course AND SubjectCode=@SubjectCode AND Section=@Section AND TimeID=@TimeID AND DayID=@DayID", sqlcon);
                                        cmdddd1.Parameters.AddWithValue("@SubjectCode", subjectBefore[numcount2]);
                                        cmdddd1.Parameters.AddWithValue("@Section", sectionBefore[numcount2]);
                                        cmdddd1.Parameters.AddWithValue("@DayID", dayIDBefore[subjCountRepeat2]);
                                        cmdddd1.Parameters.AddWithValue("@TimeID", timeIDBefore[subjCountRepeat2]);
                                        cmdddd1.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());

                                        // MessageBox.Show(idChecker[subjCountRepeat] + " "+ dayIDAfter[subjCountRepeat] +" "+ timeIDAfter[subjCountRepeat]);
                                        string query4 = "SELECT COUNT(ID) AS NumberOfDuplicateForRoom FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Room=@Room AND ID != ID";
                                        SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                        command4.Parameters.AddWithValue("@ID", idChecker2[subjCountRepeat2]);
                                        command4.Parameters.AddWithValue("@DayID", dayIDBefore[subjCountRepeat2]);
                                        command4.Parameters.AddWithValue("@TimeID", timeIDBefore[subjCountRepeat2]);
                                        if (duplicateRoomFound2 == false)
                                        {
                                            command4.Parameters.AddWithValue("@Room", roomBefore[numcount2]);
                                        }
                                        else
                                        {
                                            command4.Parameters.AddWithValue("@Room", roomTBL);
                                        }


                                        SqlDataReader reader4 = command4.ExecuteReader();

                                        if (reader4.Read() == true)
                                        {


                                            SchedDuplicateForRoom2 = reader4["NumberOfDuplicateForRoom"].ToString();


                                        }
                                        reader4.Close();
                                        if (Convert.ToInt32(SchedDuplicateForRoom2) >= 1)
                                        {

                                            if (numberofRoom != "1")
                                            {
                                                int num = Convert.ToInt32(numberofRoom) - 1;
                                                numberofRoom = num.ToString();
                                                duplicateRoomFound2 = true;
                                                duplicate2 = true;
                                            }
                                            if (numberofRoom == "1")
                                            {
                                                cmdddd1.Parameters.AddWithValue("@Room", "TBA");
                                                duplicateRoomFound2 = false;
                                                duplicate2 = false;
                                            }
                                        }
                                        else if (duplicateRoomFound2 == false && Convert.ToInt32(SchedDuplicateForRoom2) == 0)
                                        {
                                            cmdddd1.Parameters.AddWithValue("@Room", roomBefore[numcount2]);
                                            duplicate2 = false;
                                            duplicateRoomFound2 = false;
                                        }
                                        else if (duplicateRoomFound2 == true && Convert.ToInt32(SchedDuplicateForRoom2) == 0)
                                        {
                                            cmdddd1.Parameters.AddWithValue("@Room", roomTBL);
                                            duplicate2 = false;
                                            duplicateRoomFound2 = false;
                                        }
                                        cmdddd1.ExecuteNonQuery();
                                    } while (duplicate2 == true);
                                    subjCountRepeat2 += 1;

                                    if (subjCountRepeat2 == Convert.ToInt32(numberofsubj2))
                                    {
                                        subjCountRepeat2 = 0;
                                        // 
                                        if (numcount2 < sectionBefore.Count)
                                        {
                                            //   MessageBox.Show("ASD");
                                            numcount2 += 1;

                                        }
                                        dayIDBefore.Clear();
                                        timeIDBefore.Clear();
                                    }
                                }
                            }
                            ID.Clear();
                            IDsched.Clear();
                            IDsched2.Clear();
                            roomcategory.Clear();
                            sectionSched.Clear();
                            semester.Clear();
                            idchange.Clear();
                            idSPAfter.Clear();
                            sectionAfter.Clear();
                            subjectAfter.Clear();
                            idSPBefore.Clear();
                            sectionBefore.Clear();
                            subjectBefore.Clear();
                            roomAfter.Clear();
                            roomBefore.Clear();
                            idFSchedAfter.Clear();
                            FschedsectionAfter.Clear();
                            FschedsubjectAfter.Clear();
                            timeIDAfter.Clear();
                            dayIDAfter.Clear();
                            idChecker.Clear();
                            idFSchedBefore.Clear();
                            FschedsectionBefore.Clear();
                            FschedsubjectBefore.Clear();
                            timeIDBefore.Clear();
                            dayIDBefore.Clear();
                            idChecker2.Clear();
                            clear();


                            lblresult.Text = "Succesfully Updated";
                            lblresult.ForeColor = Color.Green;
                            lblresult.Visible = true;
                            label1.ForeColor = Color.Gray;
                            label6.Visible = false;
                            Course.ForeColor = Color.Gray;
                            label7.Visible = false;
                            label3.ForeColor = Color.Gray;
                            label8.Visible = false;
                            label4.ForeColor = Color.Gray;
                            PopulateGridViewRoom();
                        }
                        
                    }


                   

                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " updated a room");
                        cm.ExecuteNonQuery();

                    }
                
                        }
                    }
                
           
        

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Archive data?", "Archive", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                       
                        UserCheck();
                        
                        string roomTblB4 = "";
                        string roomTblAfter = "";
                        sqlcon.Open();

                        if (rbmajor.Checked == true && rbYes.Checked == true)
                        {
                            roomcateg1 = 1;
                        }
                        else if (rbmajor.Checked == true && rbNo.Checked == true)
                        {
                            roomcateg1 = 2;
                        }
                        else if (rbnonmajor.Checked == true)
                        {
                            roomcateg1 = 0;
                        }
                        string queryyyy22 = "SELECT ID FROM Specialization_Tbl Where RoomCategory ='" + roomcateg1.ToString() + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "' AND Room='"+ dataGridView1.CurrentRow.Cells["Room"].Value.ToString() + "'";
                            SqlCommand cmdddd22 = new SqlCommand(queryyyy22, sqlcon);
                            using (SqlDataReader readerrrr22 = cmdddd22.ExecuteReader())
                            {
                                while (readerrrr22.Read())
                                {
                                    IDArchive.Add(readerrrr22.GetInt32(0).ToString());
                                }
                            }
                     

                                  SqlCommand cmd = new SqlCommand("INSERT INTO RoomArchive_Tbl (RoomID,Room,RoomCategory,Course) VALUES (@RoomID,@Room,@RoomCategory,@Course)", sqlcon);
                                  cmd.Parameters.AddWithValue("@RoomID", dataGridView1.CurrentRow.Cells["RoomID"].Value.ToString());
                                  if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Other Rooms")
                                  {
                                      cmd.Parameters.AddWithValue("@RoomCategory", "0");
                                  }
                                  else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Computer Lab")
                                  {
                                      cmd.Parameters.AddWithValue("@RoomCategory", "1");
                                  }
                                  else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Not Computer Lab")
                                  {
                                      cmd.Parameters.AddWithValue("@RoomCategory", "2");
                                  }
                                  cmd.Parameters.AddWithValue("@Room", textBox1.Text);
                                  cmd.Parameters.AddWithValue("@Course", comboBox2.Text);
                                  cmd.ExecuteNonQuery();
                                 
                                  lblresult.Text = "Archived";
                                  lblresult.ForeColor = Color.Green;
                                  lblresult.Visible = true;
                                  label1.ForeColor = Color.Gray;
                                  label6.Visible = false;
                                  Course.ForeColor = Color.Gray;
                                  label7.Visible = false;
                                  label3.ForeColor = Color.Gray;
                                  label8.Visible = false;
                                  label4.ForeColor = Color.Gray;
                                  SqlCommand cmddel = new SqlCommand("DELETE FROM Room_Tbl WHERE ID = @ID", sqlcon);
                                  cmddel.CommandType = CommandType.Text;
                                  cmddel.Parameters.AddWithValue("@ID", (dataGridView1.CurrentRow.Cells["ID"].Value));
                                  cmddel.ExecuteNonQuery();

                                  string queryyyy2 = "SELECT ID FROM Room_Tbl Where RoomCategory ='" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "'";
                                  SqlCommand cmdddd2 = new SqlCommand(queryyyy2, sqlcon);
                                  using (SqlDataReader readerrrr2 = cmdddd2.ExecuteReader())
                                  {
                                      while (readerrrr2.Read())
                                      {
                                          idchange.Add(readerrrr2.GetInt32(0).ToString());
                                      }
                                  }
                        
                                
                                  //  MessageBox.Show(roomCATEGORYB4Edit);
                                  for (int i = 0; i < idchange.Count; i++)
                                  {
                                      SqlCommand cmdd1 = new SqlCommand("UPDATE Room_Tbl SET RoomID=@RoomID WHERE ID = @ID", sqlcon);
                                      cmdd1.Parameters.AddWithValue("@ID", idchange[i]);
                                      cmdd1.Parameters.AddWithValue("@RoomID", (i + 1));
                                      cmdd1.ExecuteNonQuery();
                                  }
                                  idchange.Clear();
                                  PopulateGridViewRoom();

                        SPEDITROOM();
                        int roomNumAfter = 1;
                        int roomNumB4 = 1;
                        string numberofRoom = "0";
                        string queryyy = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "'";
                        SqlCommand commanddd = new SqlCommand(queryyy, sqlcon);
                        SqlDataReader readerrr = commanddd.ExecuteReader();

                        if (readerrr.Read() == true)
                        {
                            numberofRoom = readerrr["numberOfroom"].ToString();
                        }
                        readerrr.Close();
                        string defaultnumberofroomSP = numberofRoom;

                        // number of room after edit

                        string numberofRoomAfter = "0";
                        string queryyy3 = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "'";
                        SqlCommand commanddd3 = new SqlCommand(queryyy3, sqlcon);
                        SqlDataReader readerrr3 = commanddd3.ExecuteReader();

                        if (readerrr3.Read() == true)
                        {
                            numberofRoomAfter = readerrr3["numberOfroom"].ToString();
                        }
                        readerrr3.Close();
                        string defaultnumberofroomSPAfter = numberofRoom;


                        // b4 editing update the previous rooms records from specialization 
                        for (int i = 0; i < idSPBefore.Count; i++)
                        {
                            string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomCATEGORYB4Edit + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "' AND RoomID='" + roomNumB4.ToString() + "'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {



                                roomTblB4 = reader1["Room"].ToString();

                            }
                            reader1.Close();

                            SqlCommand cmdddd1 = new SqlCommand("UPDATE Specialization_Tbl SET Room=@Room WHERE ID = @ID", sqlcon);
                            cmdddd1.Parameters.AddWithValue("@ID", idSPBefore[i]);
                            cmdddd1.Parameters.AddWithValue("@Room", roomTblB4);
                            if (numberofRoom != "1")
                            {
                                roomNumB4 += 1;
                                numberofRoom = roomNumB4.ToString();
                            }
                            if (roomNumB4 > Convert.ToInt32(numberofRoom))
                            {
                                roomNumB4 = 1;
                            }
                            cmdddd1.ExecuteNonQuery();
                            roomBefore.Add(roomTblB4);
                        }

                        for (int i = 0; i < idSPAfter.Count; i++)
                        {
                            string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "' AND RoomID='" + roomNumAfter.ToString() + "'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {



                                roomTblAfter = reader1["Room"].ToString();

                            }
                            reader1.Close();
                            //    MessageBox.Show(roomNumAfter.ToString());
                            SqlCommand cmdddd1 = new SqlCommand("UPDATE Specialization_Tbl SET Room=@Room WHERE ID = @ID ", sqlcon);
                            cmdddd1.Parameters.AddWithValue("@ID", idSPAfter[i].ToString());
                            cmdddd1.Parameters.AddWithValue("@Room", roomTblAfter);
                            if (numberofRoomAfter != "1")
                            {
                                roomNumAfter += 1;
                            }
                            if (roomNumAfter > Convert.ToInt32(numberofRoomAfter))
                            {
                                roomNumAfter = 1;
                            }
                            cmdddd1.ExecuteNonQuery();
                            roomAfter.Add(roomTblAfter);
                        }

                        int numcount = 0;
                        string SchedDuplicateForRoom = "0";
                        bool duplicate = false;
                        string numberofsubj = "0";
                        int subjCountRepeat = 0;
                        bool duplicateRoomFound = false;
                        for (int i = 0; i < IDsched.Count; i++)
                        {
                            //     MessageBox.Show("ASD");
                            string query44 = "SELECT COUNT(ID) AS numberSubj FROM FacultySchedule_Tbl WHERE SubjectCode=@SubjectCode AND Section=@Section AND Course=@Course";
                            SqlCommand command44 = new SqlCommand(query44, sqlcon);
                            command44.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                            command44.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                            command44.Parameters.AddWithValue("@Course", comboBox2.Text);
                            SqlDataReader reader44 = command44.ExecuteReader();

                            if (reader44.Read() == true)
                            {


                                numberofsubj = reader44["numberSubj"].ToString();


                            }
                            reader44.Close();

                            string query3 = "SELECT ID,TimeID,DayID FROM FacultySchedule_Tbl Where   Course=@Course AND Section=@Section AND SubjectCode=@SubjectCode ";
                            SqlCommand cmd3 = new SqlCommand(query3, sqlcon);
                            cmd3.Parameters.AddWithValue("@Course", comboBox2.Text);
                            cmd3.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                            cmd3.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                            using (SqlDataReader reader3 = cmd3.ExecuteReader())
                            {
                                while (reader3.Read())
                                {
                                    idChecker.Add(reader3.GetInt32(0).ToString());
                                    //    course.Add(reader.GetString(1));
                                    timeIDAfter.Add(reader3.GetString(1));
                                    dayIDAfter.Add(reader3.GetString(2));
                                }
                            }
                            string roomTBL = "";
                            string query1 = "select Room FROM Room_Tbl WHERE RoomCategory='" + roomcateg1.ToString() + "' AND Course='" + comboBox2.Text + "' AND RoomID='" + numberofRoomAfter + "'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {



                                roomTBL = reader1["Room"].ToString();

                            }
                            reader1.Close();

                            do
                            {
                                //    MessageBox.Show(numberofsubj);
                                SqlCommand cmdddd1 = new SqlCommand("UPDATE FacultySchedule_Tbl SET Room=@Room Where Course=@Course AND SubjectCode=@SubjectCode AND Section=@Section AND TimeID=@TimeID AND DayID=@DayID", sqlcon);
                                cmdddd1.Parameters.AddWithValue("@SubjectCode", subjectAfter[numcount]);
                                cmdddd1.Parameters.AddWithValue("@Section", sectionAfter[numcount]);
                                cmdddd1.Parameters.AddWithValue("@DayID", dayIDAfter[subjCountRepeat]);
                                cmdddd1.Parameters.AddWithValue("@TimeID", timeIDAfter[subjCountRepeat]);
                                cmdddd1.Parameters.AddWithValue("@Course", comboBox2.Text);

                                // MessageBox.Show(idChecker[subjCountRepeat] + " "+ dayIDAfter[subjCountRepeat] +" "+ timeIDAfter[subjCountRepeat]);
                                string query4 = "SELECT COUNT(ID) AS NumberOfDuplicateForRoom FROM FacultySchedule_Tbl WHERE DayID=@DayID AND TimeID=@TimeID  AND Room=@Room AND ID != ID";
                                SqlCommand command4 = new SqlCommand(query4, sqlcon);
                                command4.Parameters.AddWithValue("@ID", idChecker[subjCountRepeat]);
                                command4.Parameters.AddWithValue("@DayID", dayIDAfter[subjCountRepeat]);
                                command4.Parameters.AddWithValue("@TimeID", timeIDAfter[subjCountRepeat]);
                                if (duplicateRoomFound == false)
                                {
                                    command4.Parameters.AddWithValue("@Room", roomAfter[numcount]);
                                }
                                else
                                {
                                    command4.Parameters.AddWithValue("@Room", roomTBL);
                                }


                                SqlDataReader reader4 = command4.ExecuteReader();

                                if (reader4.Read() == true)
                                {


                                    SchedDuplicateForRoom = reader4["NumberOfDuplicateForRoom"].ToString();


                                }
                                reader4.Close();
                                if (Convert.ToInt32(SchedDuplicateForRoom) >= 1)
                                {

                                    if (numberofRoomAfter != "1")
                                    {
                                        int num = Convert.ToInt32(numberofRoomAfter) - 1;
                                        numberofRoomAfter = num.ToString();
                                        duplicateRoomFound = true;
                                        duplicate = true;
                                    }
                                    if (numberofRoomAfter == "1")
                                    {
                                        cmdddd1.Parameters.AddWithValue("@Room", "TBA");
                                        duplicateRoomFound = false;
                                        duplicate = false;
                                    }
                                }
                                else if (duplicateRoomFound == false && Convert.ToInt32(SchedDuplicateForRoom) == 0)
                                {
                                    cmdddd1.Parameters.AddWithValue("@Room", roomAfter[numcount]);
                                    duplicate = false;
                                    duplicateRoomFound = false;
                                }
                                else if (duplicateRoomFound == true && Convert.ToInt32(SchedDuplicateForRoom) == 0)
                                {
                                    cmdddd1.Parameters.AddWithValue("@Room", roomTBL);
                                    duplicate = false;
                                    duplicateRoomFound = false;
                                }
                                cmdddd1.ExecuteNonQuery();
                            } while (duplicate == true);

                            subjCountRepeat += 1;

                            if (subjCountRepeat == Convert.ToInt32(numberofsubj))
                            {
                                subjCountRepeat = 0;
                                // 
                                if (numcount < sectionAfter.Count)
                                {
                                    //   MessageBox.Show("ASD");
                                    numcount += 1;

                                }
                                dayIDAfter.Clear();
                                timeIDAfter.Clear();
                            }


                        }
                        ID.Clear();
                        IDsched.Clear();
                        IDsched2.Clear();
                        roomcategory.Clear();
                        sectionSched.Clear();
                        semester.Clear();
                        idchange.Clear();
                        idSPAfter.Clear();
                        sectionAfter.Clear();
                        subjectAfter.Clear();
                        idSPBefore.Clear();
                        sectionBefore.Clear();
                        subjectBefore.Clear();
                        roomAfter.Clear();
                        roomBefore.Clear();
                        idFSchedAfter.Clear();
                        FschedsectionAfter.Clear();
                        FschedsubjectAfter.Clear();
                        timeIDAfter.Clear();
                        dayIDAfter.Clear();
                        idChecker.Clear();
                        idFSchedBefore.Clear();
                        FschedsectionBefore.Clear();
                        FschedsubjectBefore.Clear();
                        timeIDBefore.Clear();
                        dayIDBefore.Clear();
                        idChecker2.Clear();
                        clear();
                      
                        DateTime time = DateTime.Now;
                            string format = "yyyy-MM-dd";
                            SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                            cm.Parameters.AddWithValue("@Username", loginAct);
                            cm.Parameters.AddWithValue("@ActivityLog", loginAct + " archive a room");
                            cm.ExecuteNonQuery();
                    
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
            rbYes.Enabled = true;
            rbNo.Enabled = true;
            btnSave.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
            textBox1.Enabled = comboBox2.Enabled = true;
            PopulateGridViewRoom();
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
            textBox1.Text = comboBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            rbmajor.Checked = rbnonmajor.Checked = rbYes.Checked = rbNo.Checked = false;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            lblresult.Visible = false;
            label5.Visible = false;
            label1.ForeColor = Color.Gray;
            label6.Visible = false;
            Course.ForeColor = Color.Gray;
            label7.Visible = false;
            label3.ForeColor = Color.Gray;
            label8.Visible = false;
            label4.ForeColor = Color.Gray;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                
                groupBox4.Enabled = true;
                groupBox5.Enabled = true;
                button1.Enabled = false;
                button2.Enabled = false;
                btnSave.Enabled = true;
                PopulateGridViewRoom();
                textBox1.Text = textBox2.Text = "";
                comboBox2.SelectedIndex = -1;
                if (dataGridView1.Rows.Count != 0)
                {
                    clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnArchived_Click(object sender, EventArgs e)
        {
            RoomArchive RA = new RoomArchive(this);
            RA.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          try
           {
            //    subjpercateg();
              //  MessageBox.Show(numCateg);
                lblresult.Visible = false;
                btnSave.Enabled = false;
                button1.Enabled = true;
                button2.Enabled = true;
         
                textBox1.Text = dataGridView1.CurrentRow.Cells["Room"].Value.ToString();
                comboBox2.Text = dataGridView1.CurrentRow.Cells["Course"].Value.ToString();
                roomCateg();
                if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Computer Lab")
                {
                    rbmajor.Checked = true;
                    rbYes.Checked = true;

                }
                else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Not Computer Lab")
                {
                    rbmajor.Checked = true;
                    rbYes.Checked = false;
                    rbNo.Checked = true;
                }
                else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Other Rooms")
                {
                    rbnonmajor.Checked = true;
                    rbYes.Checked = false;
                    rbNo.Checked = false;
                }
                Checker();
          }
          catch (Exception ex)
           {
                MessageBox.Show(ex.Message);
           }
        }

        private void Course_Click(object sender, EventArgs e)
        {

        }

        private void rbmajor_CheckedChanged(object sender, EventArgs e)
        {
            groupBox5.Enabled = true;
        }

        private void rbnonmajor_CheckedChanged(object sender, EventArgs e)
        {
            groupBox5.Enabled = false;
            rbYes.Checked = false;
            rbNo.Checked = false;
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            label5.Visible = false;
            label1.ForeColor = Color.Gray;
            label6.Visible = false;
            Course.ForeColor = Color.Gray;
            label7.Visible = false;
            label3.ForeColor = Color.Gray;
            label8.Visible = false;
            label4.ForeColor = Color.Gray;
            this.Close();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            label5.Visible = false;
            label1.ForeColor = Color.Gray;
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                label5.Visible = true;
                label5.ForeColor = Color.Red;
                label1.ForeColor = Color.Red;
            }
            else
            {
                label5.Visible = false;
                label1.ForeColor = Color.Gray;
            }
           
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            label6.Visible = false;
            Course.ForeColor = Color.Gray;
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if(comboBox2.Text == "")
            {
                label6.Visible = true;
                label6.ForeColor = Color.Red;
                Course.ForeColor = Color.Red;
            }
            else
            {
                label6.Visible = false;
                Course.ForeColor = Color.Gray;
            }
          
        }

        private void rbmajor_MouseClick(object sender, MouseEventArgs e)
        {
            label7.Visible = false;
            label3.ForeColor = Color.Gray;
        }

        private void rbnonmajor_MouseClick(object sender, MouseEventArgs e)
        {
            label7.Visible = false;
            label3.ForeColor = Color.Gray;
        }

        private void rbYes_MouseClick(object sender, MouseEventArgs e)
        {
            label8.Visible = false;
            label4.ForeColor = Color.Gray;
        }

        private void rbNo_MouseClick(object sender, MouseEventArgs e)
        {
            label8.Visible = false;
            label4.ForeColor = Color.Gray;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length <= 0) return;
                string s = textBox1.Text.Substring(0, 1);
                if (s != s.ToUpper())
                {
                    int curSelStart = textBox1.SelectionStart;
                    int curSelLength = textBox1.SelectionLength;
                    textBox1.SelectionStart = 0;
                    textBox1.SelectionLength = 1;
                    textBox1.SelectedText = s.ToUpper();
                    textBox1.SelectionStart = curSelStart;
                    textBox1.SelectionLength = curSelLength;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
