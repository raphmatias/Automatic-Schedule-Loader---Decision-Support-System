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
using System.Drawing.Printing;
namespace AutomaticScheduleLoader
{
    public partial class SchedFrm : Form
    {
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
        int time = 1;
        int day = 1;
        string subj = "";
        string room = "";
        string sec = "";
        string year = "";
        string course = "";

        string facultyCode = "";
        string facultyName = "";
        string educattain = "";
        string classType = "";

        string semester = "";
        List<string> subject = new List<string>();
        List<string> subjectname = new List<string>();
        List<string> lechrs = new List<string>();
        List<string> labhrs = new List<string>();
        List<string> crs = new List<string>();
        List<string> yrlvl = new List<string>();
        List<string> SEC = new List<string>();
        List<string> ROOM = new List<string>();
        string subname = "";
        string yr = "";
        string total = "";
        SearchSchedule sched;
        public SchedFrm(SearchSchedule ss)
        {
            InitializeComponent();
            // this.Size = new Size(1434, 900);
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            //  this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            //  this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            //  this.WindowState = FormWindowState.Maximized;
            this.sched = ss;
        }
        void totalhrs()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query6 = "select(case when(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int))) IS NULL THEN 0 ELSE(sum(cast(ContHrsLec as int)) + sum(cast(ContHrsLab as int)))  end) AS Total from Specialization_Tbl where FacultyCode = '" + sched.dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand command6 = new SqlCommand(query6, sqlcon);
                    SqlDataReader reader6 = command6.ExecuteReader();

                    if (reader6.Read() == true)
                    {


                        total = reader6["Total"].ToString();

                    }

                    reader6.Close();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void scheduleTable()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query = "SELECT SubjectCode,ContHrsLec,ContHrsLab,Course,Section,Room FROM Specialization_Tbl Where FacultyCode ='" + sched.dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "'";
                    SqlCommand cmd = new SqlCommand(query, sqlcon);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subject.Add(reader.GetString(0));
                            lechrs.Add(reader.GetString(1));
                            labhrs.Add(reader.GetString(2));
                            if (reader.GetString(0).Equals("Consultation Hours") || reader.GetString(0).Equals("Research And Extension"))
                            {
                                crs.Add(" ");
                                ROOM.Add(" ");
                            }
                            else
                            {
                                crs.Add(reader.GetString(3));
                                ROOM.Add(reader.GetString(5));
                            }
                            SEC.Add(reader.GetString(4));

                        }
                    }
                    //    MessageBox.Show(subject[0]);
                    for (int i = 0; i < subject.Count; i++)
                    {
                        string queryFaculty = "select SubjectName,YearLevel from Subject_Tbl WHERE SubjectCode='" + subject[i] + "'";
                        SqlCommand commandFaculty = new SqlCommand(queryFaculty, sqlcon);
                        SqlDataReader readerFaculty = commandFaculty.ExecuteReader();

                        if (readerFaculty.Read() == true)
                        {



                            subname = readerFaculty["SubjectName"].ToString();
                            yr = readerFaculty["YearLevel"].ToString();

                        }
                        readerFaculty.Close();
                        if (subject[i] == "Consultation Hours" || subject[i] == "Research And Extension")
                        {
                            subjectname.Add(" ");
                            yrlvl.Add(" ");
                        }
                        else
                        {
                            subjectname.Add(subname);
                            yrlvl.Add(yr);
                        }

                    }


                }

                p1.Show();
                p2.Show();
                p4.Show();
                p5.Show();
                p3.Show();
                l1.Show();
                t1.Show();
                t2.Show();

                //  txtSubCode1.Text = subject[0];
                // MessageBox.Show(su);
                if (subject.Any())
                {
                    try
                    {
                        txtSubCode1.Text = subject[0];
                        txtSubCode2.Text = subject[1];
                        txtSubCode3.Text = subject[2];
                        txtSubCode4.Text = subject[3];
                        txtSubCode5.Text = subject[4];
                        txtSubCode6.Text = subject[5];
                        txtSubCode7.Text = subject[6];
                        txtSubCode8.Text = subject[7];
                        txtSubCode9.Text = subject[8];
                        txtSubCode10.Text = subject[9];
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtSubName1.Text = subjectname[0];
                        txtSubName2.Text = subjectname[1];
                        txtSubName3.Text = subjectname[2];
                        txtSubName4.Text = subjectname[3];
                        txtSubName5.Text = subjectname[4];
                        txtSubName6.Text = subjectname[5];
                        txtSubName7.Text = subjectname[6];
                        txtSubName8.Text = subjectname[7];
                        txtSubName9.Text = subjectname[8];
                        txtSubName10.Text = subjectname[9];
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtcrs1.Text = crs[0] + " " + yrlvl[0] + "-" + SEC[0];
                        txtcrs2.Text = crs[1] + " " + yrlvl[1] + "-" + SEC[1];
                        txtcrs3.Text = crs[2] + " " + yrlvl[2] + "-" + SEC[2];
                        txtcrs4.Text = crs[3] + " " + yrlvl[3] + "-" + SEC[3];
                        txtcrs5.Text = crs[4] + " " + yrlvl[4] + "-" + SEC[4];
                        txtcrs6.Text = crs[5] + " " + yrlvl[5] + "-" + SEC[5];
                        txtcrs7.Text = crs[6] + " " + yrlvl[6] + "-" + SEC[6];
                        txtcrs8.Text = crs[7] + " " + yrlvl[7] + "-" + SEC[7];
                        txtcrs9.Text = crs[8] + " " + yrlvl[8] + "-" + SEC[8];
                        txtcrs10.Text = crs[9] + " " + yrlvl[9] + "-" + SEC[9];

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtlec1.Text = lechrs[0];
                        txtlec2.Text = lechrs[1];
                        txtlec3.Text = lechrs[2];
                        txtlec4.Text = lechrs[3];
                        txtlec5.Text = lechrs[4];
                        txtlec6.Text = lechrs[5];
                        txtlec7.Text = lechrs[6];
                        txtlec8.Text = lechrs[7];
                        txtlec9.Text = lechrs[8];
                        txtlec10.Text = lechrs[9];

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtlab1.Text = labhrs[0];
                        txtlab2.Text = labhrs[1];
                        txtlab3.Text = labhrs[2];
                        txtlab4.Text = labhrs[3];
                        txtlab5.Text = labhrs[4];
                        txtlab6.Text = labhrs[5];
                        txtlab7.Text = labhrs[6];
                        txtlab8.Text = labhrs[7];
                        txtlab9.Text = labhrs[8];
                        txtlab10.Text = labhrs[9];

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtlab1.Text = labhrs[0];
                        txtlab2.Text = labhrs[1];
                        txtlab3.Text = labhrs[2];
                        txtlab4.Text = labhrs[3];
                        txtlab5.Text = labhrs[4];
                        txtlab6.Text = labhrs[5];
                        txtlab7.Text = labhrs[6];
                        txtlab8.Text = labhrs[7];
                        txtlab9.Text = labhrs[8];
                        txtlab10.Text = labhrs[9];

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        total1.Text = (Convert.ToInt32(lechrs[0]) + Convert.ToInt32(labhrs[0])).ToString();
                        total2.Text = (Convert.ToInt32(lechrs[1]) + Convert.ToInt32(labhrs[1])).ToString();
                        total3.Text = (Convert.ToInt32(lechrs[2]) + Convert.ToInt32(labhrs[2])).ToString();
                        total4.Text = (Convert.ToInt32(lechrs[3]) + Convert.ToInt32(labhrs[3])).ToString();
                        total5.Text = (Convert.ToInt32(lechrs[4]) + Convert.ToInt32(labhrs[4])).ToString();
                        total6.Text = (Convert.ToInt32(lechrs[5]) + Convert.ToInt32(labhrs[5])).ToString();
                        total7.Text = (Convert.ToInt32(lechrs[6]) + Convert.ToInt32(labhrs[6])).ToString();
                        total8.Text = (Convert.ToInt32(lechrs[7]) + Convert.ToInt32(labhrs[7])).ToString();
                        total9.Text = (Convert.ToInt32(lechrs[8]) + Convert.ToInt32(labhrs[8])).ToString();
                        total10.Text = (Convert.ToInt32(lechrs[9]) + Convert.ToInt32(labhrs[9])).ToString();
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        txtroom1.Text = ROOM[0];
                        txtroom2.Text = ROOM[1];
                        txtroom3.Text = ROOM[2];
                        txtroom4.Text = ROOM[3];
                        txtroom5.Text = ROOM[4];
                        txtroom6.Text = ROOM[5];
                        txtroom7.Text = ROOM[6];
                        txtroom8.Text = ROOM[7];
                        txtroom9.Text = ROOM[8];
                        txtroom10.Text = ROOM[9];

                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (ROOM[0] != "")
                        {
                            txtno1.Text = " ";
                        }
                        if (ROOM[1] != "")
                        {
                            txtno2.Text = " ";
                        }
                        if (ROOM[2] != "")
                        {
                            txtno3.Text = " ";
                        }
                        if (ROOM[3] != "")
                        {
                            txtno4.Text = " ";
                        }
                        if (ROOM[4] != "")
                        {
                            txtno5.Text = " ";
                        }
                        if (ROOM[5] != "")
                        {
                            txtno6.Text = " ";
                        }
                        if (ROOM[6] != "")
                        {
                            txtno7.Text = " ";
                        }
                        if (ROOM[7] != "")
                        {
                            txtno8.Text = " ";
                        }
                        if (ROOM[8] != "")
                        {
                            txtno9.Text = " ";
                        }
                        if (ROOM[9] != "")
                        {
                            txtno10.Text = " ";
                        }



                    }
                    catch (Exception)
                    {

                    }
                    foreach (Control p in Controls)
                    {
                        if (p is TextBox)
                        {
                            if (p.Text == "")
                            {
                                p.Hide();
                            }
                            else
                            {
                                p.Show();
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
            p1.Hide();
            p2.Hide();
            p3.Hide();
            p4.Hide();
            p5.Hide();
            l1.Hide();
            if (sched.comboBox1.Text != "Faculty")
            {
                t1.Text = "";
                t2.Text = "";
            }
            else
            {
                t1.Text = "Lecture";
                t2.Text = "Laboratory";
            }

            subject.Clear();
            subjectname.Clear();
            crs.Clear();
            yrlvl.Clear();
            SEC.Clear();
            lechrs.Clear();
            labhrs.Clear();
            ROOM.Clear();
            txtSubCode1.Text = "";
            txtSubCode2.Text = "";
            txtSubCode3.Text = "";
            txtSubCode4.Text = "";
            txtSubCode5.Text = "";
            txtSubCode6.Text = "";
            txtSubCode7.Text = "";
            txtSubCode8.Text = "";
            txtSubCode9.Text = "";
            txtSubCode10.Text = "";
            // -------------------------- //
            txtSubName1.Text = "";
            txtSubName2.Text = "";
            txtSubName3.Text = "";
            txtSubName4.Text = "";
            txtSubName5.Text = "";
            txtSubName6.Text = "";
            txtSubName7.Text = "";
            txtSubName8.Text = "";
            txtSubName9.Text = "";
            txtSubName10.Text = "";
            // ------------------------- //
            txtcrs1.Text = "";
            txtcrs2.Text = "";
            txtcrs3.Text = "";
            txtcrs4.Text = "";
            txtcrs5.Text = "";
            txtcrs6.Text = "";
            txtcrs7.Text = "";
            txtcrs8.Text = "";
            txtcrs9.Text = "";
            txtcrs10.Text = "";
            // ------------------------ // 
            txtlec1.Text = "";
            txtlec2.Text = "";
            txtlec3.Text = "";
            txtlec4.Text = "";
            txtlec5.Text = "";
            txtlec6.Text = "";
            txtlec7.Text = "";
            txtlec8.Text = "";
            txtlec9.Text = "";
            txtlec10.Text = "";
            // ------------------------ // 
            txtlab1.Text = "";
            txtlab2.Text = "";
            txtlab3.Text = "";
            txtlab4.Text = "";
            txtlab5.Text = "";
            txtlab6.Text = "";
            txtlab7.Text = "";
            txtlab8.Text = "";
            txtlab9.Text = "";
            txtlab10.Text = "";
            // ------------------------ // 
            // ------------------------ // 
            total1.Text = "";
            total2.Text = "";
            total3.Text = "";
            total4.Text = "";
            total5.Text = "";
            total6.Text = "";
            total7.Text = "";
            total8.Text = "";
            total9.Text = "";
            total10.Text = "";
            // ------------------------ // 
            txtroom1.Text = "";
            txtroom2.Text = "";
            txtroom3.Text = "";
            txtroom4.Text = "";
            txtroom5.Text = "";
            txtroom6.Text = "";
            txtroom7.Text = "";
            txtroom8.Text = "";
            txtroom9.Text = "";
            txtroom10.Text = "";
            // ------------------------ // 
            txtno1.Text = "";
            txtno2.Text = "";
            txtno3.Text = "";
            txtno4.Text = "";
            txtno5.Text = "";
            txtno6.Text = "";
            txtno7.Text = "";
            txtno8.Text = "";
            txtno9.Text = "";
            txtno10.Text = "";
            foreach (Control p in Controls)
            {
                if (p is TextBox)
                {
                    if (p.Text == "")
                    {
                        p.Hide();
                    }
                    else
                    {
                        p.Show();
                    }

                }
            }
        }
        void schedule()
        {

            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (sched.comboBox1.Text == "Faculty")
                    {
                        string queryFaculty = "select SubjectCode,Section,Room,ClassType FROM FacultySchedule_Tbl WHERE FacultyCode='" + sched.dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "' AND TimeID='" + time.ToString() + "' AND DayID='" + day.ToString() + "'";
                        SqlCommand commandFaculty = new SqlCommand(queryFaculty, sqlcon);
                        SqlDataReader readerFaculty = commandFaculty.ExecuteReader();

                        if (readerFaculty.Read() == true)
                        {



                            subj = readerFaculty["SubjectCode"].ToString();
                            room = readerFaculty["Room"].ToString();
                            sec = readerFaculty["Section"].ToString();
                            classType = readerFaculty["ClassType"].ToString();
                        }
                        readerFaculty.Close();

                        if (subj == "Consultation Hours" || subj == "Research And Extension")
                        {
                            course = "";
                            year = "";
                        }

                        else
                        {
                            string queryFaculty1 = "select Course,YearLevel FROM Subject_Tbl  WHERE SubjectCode = '" + subj + "'";
                            SqlCommand commandFaculty1 = new SqlCommand(queryFaculty1, sqlcon);
                            SqlDataReader readerFaculty1 = commandFaculty1.ExecuteReader();

                            if (readerFaculty1.Read() == true)
                            {



                                year = readerFaculty1["YearLevel"].ToString();
                                course = readerFaculty1["Course"].ToString();

                            }
                            readerFaculty1.Close();
                        }

                    }
                    else if (sched.comboBox1.Text == "Section")
                    {
                       // MessageBox.Show(sched.dgvFaculty.CurrentRow.Cells["Section"].Value.ToString());

                        string querySection = "SELECT distinct a.ID,a.FacultyCode,a.SubjectCode,a.DayID,a.TimeID,a.SubjectCode,b.YearLevel,a.Section,a.Semester,a.Course,a.Course,a.Room,a.ClassType  FROM FacultySchedule_Tbl a JOIN Subject_Tbl b ON a.SubjectCode = b.SubjectCode where a.Section='" + sched.dgvFaculty.CurrentRow.Cells["Section"].Value.ToString() + "' AND a.TimeID='" + time.ToString() + "' AND a.DayID='" + day.ToString() + "' AND a.Semester ='"+sched.comboBox4.Text+"' AND a.Course='"+ sched.dgvFaculty.CurrentRow.Cells["Course"].Value.ToString() + "' AND b.YearLevel='"+ sched.dgvFaculty.CurrentRow.Cells[1].Value.ToString() + "'";
                        SqlCommand commandSection = new SqlCommand(querySection, sqlcon);
                        SqlDataReader readerSection = commandSection.ExecuteReader();

                        if (readerSection.Read() == true)
                        {



                            subj = readerSection["SubjectCode"].ToString();
                            room = readerSection["Room"].ToString();
                            facultyCode = readerSection["FacultyCode"].ToString();
                            classType = readerSection["ClassType"].ToString();
                        }
                        readerSection.Close();
                        string queryFaculty1 = "select Course,YearLevel,Semester FROM Subject_Tbl  WHERE SubjectCode = '" + subj + "'";
                        SqlCommand commandFaculty1 = new SqlCommand(queryFaculty1, sqlcon);
                        SqlDataReader readerFaculty1 = commandFaculty1.ExecuteReader();

                        if (readerFaculty1.Read() == true)
                        {



                            year = readerFaculty1["YearLevel"].ToString();
                            course = readerFaculty1["Course"].ToString();
                            semester = readerFaculty1["Semester"].ToString();
                        }
                        readerFaculty1.Close();
                        string query1 = "select FacultyCode,FacultyName FROM Faculty_Tbl WHERE FacultyCode='" + facultyCode + "'";
                        SqlCommand command1 = new SqlCommand(query1, sqlcon);
                        SqlDataReader reader1 = command1.ExecuteReader();

                        if (reader1.Read() == true)
                        {



                            facultyName = reader1["FacultyName"].ToString();

                        }
                        reader1.Close();
                    }
                    else if (sched.comboBox1.Text == "Room")
                    {
                        string querySection = "select SubjectCode,FacultyCode,Section,ClassType FROM FacultySchedule_Tbl WHERE Room='" + sched.dgvFaculty.CurrentRow.Cells["Room"].Value.ToString() + "' AND TimeID='" + time.ToString() + "' AND DayID='" + day.ToString() + "' AND Course='"+ sched.dgvFaculty.CurrentRow.Cells["Course"].Value.ToString() + "'";
                        SqlCommand commandSection = new SqlCommand(querySection, sqlcon);
                        SqlDataReader readerSection = commandSection.ExecuteReader();

                        if (readerSection.Read() == true)
                        {



                            subj = readerSection["SubjectCode"].ToString();
                            sec = readerSection["Section"].ToString();
                            facultyCode = readerSection["FacultyCode"].ToString();
                            classType = readerSection["ClassType"].ToString();
                        }
                        readerSection.Close();
                        string queryFaculty1 = "select Course,YearLevel FROM Subject_Tbl  WHERE SubjectCode = '" + subj + "'";
                        SqlCommand commandFaculty1 = new SqlCommand(queryFaculty1, sqlcon);
                        SqlDataReader readerFaculty1 = commandFaculty1.ExecuteReader();

                        if (readerFaculty1.Read() == true)
                        {



                            year = readerFaculty1["YearLevel"].ToString();
                            course = readerFaculty1["Course"].ToString();

                        }
                        readerFaculty1.Close();
                        string query1 = "select FacultyCode,FacultyName FROM Faculty_Tbl WHERE FacultyCode='" + facultyCode + "'";
                        SqlCommand command1 = new SqlCommand(query1, sqlcon);
                        SqlDataReader reader1 = command1.ExecuteReader();

                        if (reader1.Read() == true)
                        {



                            facultyName = reader1["FacultyName"].ToString();

                        }
                        reader1.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateGridViewSection() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    if (sched.comboBox1.Text == "Faculty")
                    {
                        if (sched.comboBox2.Text == "Faculty Code")
                        {
                            string query1 = "select FacultyCode,FacultyName,EducAttain FROM Faculty_Tbl WHERE FacultyCode = '" + sched.dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString() + "'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {
                                facultyName = reader1["FacultyName"].ToString();
                                facultyCode = reader1["FacultyCode"].ToString();
                                educattain = reader1["EducAttain"].ToString();
                            }
                            reader1.Close();
                        }
                        else if (sched.comboBox2.Text == "Faculty Name")
                        {

                            string query1 = "select FacultyCode,FacultyName,EducAttain FROM Faculty_Tbl WHERE FacultyName like '%" + sched.txtSearch.Text + "%'";
                            SqlCommand command1 = new SqlCommand(query1, sqlcon);
                            SqlDataReader reader1 = command1.ExecuteReader();

                            if (reader1.Read() == true)
                            {
                                facultyName = reader1["FacultyName"].ToString();
                                facultyCode = reader1["FacultyCode"].ToString();
                                educattain = reader1["EducAttain"].ToString();
                            }
                            reader1.Close();
                        }



                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        
        private void SchedFrm_Load(object sender, EventArgs e)
        {

            scheduleprint();
          //  labelsView();
            try
            {

              

                // Scale our form to look like it did when we designed it.

                // This adjusts between the screen resolution of the design computer and the workstation.
                int ourScreenWidth = Screen.FromControl(this).WorkingArea.Width;
                int ourScreenHeight = Screen.FromControl(this).WorkingArea.Height;
                float scaleFactorWidth = (float)ourScreenWidth / 1920;
                float scaleFactorHeigth = (float)ourScreenHeight / 1080;
                SizeF scaleFactor = new SizeF(scaleFactorWidth, scaleFactorHeigth);
                Scale(scaleFactor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(bitmap, 0, 0);



        }
        Bitmap bitmap;
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //Add a Panel control.
                Panel panel = new Panel();
                this.Controls.Add(panel);

                //Create a Bitmap of size same as that of the Form.
                Graphics grp = panel.CreateGraphics();
                Size formSize = this.ClientSize;
                bitmap = new Bitmap(formSize.Width, formSize.Height, grp);
                grp = Graphics.FromImage(bitmap);

                //Copy screen area that that the Panel covers.
                Point panelLocation = PointToScreen(panel.Location);
                grp.CopyFromScreen(panelLocation.X, panelLocation.Y, 0, 0, formSize);

                //Show the Print Preview Dialog.

                printPreviewDialog1.Document = printDocument1;

                printDocument1.DefaultPageSettings.Landscape = true;

                printPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox41_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*
            schedule();
            for (int i = 1; i > 2; i++)
            {
                if (time == 1 && day == i)
                {
                  //  txtMon730Subj.Text = subj;
                    day += 1;
                    schedule();
                }
                else if (time == 1 && day == i)
                {
                 //   txtTue730Subj.Text = subj;
                    day += 1;
                    schedule();
                }
            }
            */
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Visible = false;
           
                //Add a Panel control.
                Panel panel = new Panel();
                this.Controls.Add(panel);

                //Create a Bitmap of size same as that of the Form.
                Graphics grp = panel.CreateGraphics();
                Size formSize = this.ClientSize;
                bitmap = new Bitmap(formSize.Width, formSize.Height, grp);
                grp = Graphics.FromImage(bitmap);

                //Copy screen area that that the Panel covers.
                Point panelLocation = PointToScreen(panel.Location);
                grp.CopyFromScreen(panelLocation.X, panelLocation.Y, 0, 0, formSize);

                //Show the Print Preview Dialog.

                printPreviewDialog1.Document = printDocument1;
                printDocument1.DefaultPageSettings.Landscape = true;

                printPreviewDialog1.ShowDialog();
                button1.Visible = true;
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void scheduleprint()
        {
            clear();


            //  7-8 am 
            if (sched.comboBox1.Text == "Faculty")
            {
                totalhrs();
                lbl4.Text = "Total no. of Contact hours per week: " + total;
                scheduleTable();
                subj = "";
                room = "";
                sec = "";
                year = "";
                course = "";
                facultyName = "";
                facultyCode = "";
                classType = "";
                day = 1;
                time = 1;
                 PopulateGridViewSection();
                lbl1.Text = "Name: " + sched.dgvFaculty.CurrentRow.Cells["FacultyName"].Value.ToString() + "/" + sched.dgvFaculty.CurrentRow.Cells["FacultyCode"].Value.ToString();

                lbl3.Text = "Highest Educ. Attainment: " + educattain;
        
                schedule();
                if (time == 1 && day == 1)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label29.Visible = false;
                    }
                    else
                    {
                        label29.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon7.Text = subj;
                    }
                    SecProfMon7.Text = sec;
                    RoomMon7.Text = room;
                    mon7crs.Text = course;
                    mon7yr.Text = year;
                    mon7prof.Text = "";
                    mon7yrRoom.Text = "";
                    mon7secRoom.Text = "";
                    label10.Visible = false;

                    if (classType == "Lecture")
                    {
                        panel3.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel3.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel3.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel3.BackColor = Color.White;
                    }
                    day = 21;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    classType = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }

                if (time == 1 && day == 21)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label32.Visible = false;
                    }
                    else
                    {
                        label32.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue7.Text = subj;
                    }
                    SecProfTue7.Text = sec;
                    RoomTue7.Text = room;
                    tue7crs.Text = course;
                    tue7yr.Text = year;
                    tue7prof.Text = "";
                    tue7yrRoom.Text = "";
                    tue7secRoom.Text = "";
                    label13.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel5.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel5.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel5.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel5.BackColor = Color.White;
                    }
                    day = 31;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    classType = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 31)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label33.Visible = false;
                    }
                    else
                    {
                        label33.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed7.Text = subj;
                    }
                    SecProfWed7.Text = sec;
                    RoomWed7.Text = room;
                    wed7crs.Text = course;
                    Wed7yr.Text = year;
                    wed7prof.Text = "";
                    wed7yrRoom.Text = "";
                    wed7secRoom.Text = "";
                    label18.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel6.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel6.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel6.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel6.BackColor = Color.White;
                    }
                    classType = "";
                    day = 41;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 41)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label35.Visible = false;
                    }
                    else
                    {
                        label35.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu7.Text = subj;
                    }
                    SecProfThu7.Text = sec;
                    RoomThu7.Text = room;
                    Thu7crs.Text = course;
                    Thu7yr.Text = year;
                    thu7prof.Text = "";
                    thu7yrRoom.Text = "";
                    thu7secRoom.Text = "";
                    label23.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel7.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel7.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel7.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel7.BackColor = Color.White;
                    }
                    classType = "";
                    day = 51;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 51)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label38.Visible = false;
                    }
                    else
                    {
                        label38.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri7.Text = subj;
                    }
                    SecProfFri7.Text = sec;
                    RoomFri7.Text = room;
                    Fri7crs.Text = course;
                    Fri7yr.Text = year;
                    fri7prof.Text = "";
                    fri7yrRoom.Text = "";
                    fri7secRoom.Text = "";
                    label41.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel8.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel8.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel8.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel8.BackColor = Color.White;
                    }
                    classType = "";
                    day = 61;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 61)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label39.Visible = false;
                    }
                    else
                    {
                        label39.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat7.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat7.Text = subj;
                    }
                    SecProfSat7.Text = sec;
                    RoomSat7.Text = room;
                    Sat7crs.Text = course;
                    Sat7yr.Text = year;
                    sat7prof.Text = "";
                    sat7yrRoom.Text = "";
                    sat7secRoom.Text = "";
                    label46.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel9.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel9.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel9.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel9.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 2;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                //  8-9 am 
                if (time == 2 && day == 2)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label30.Visible = false;
                    }
                    else
                    {
                        label30.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon8.Text = subj;
                    }
                    SecProfMon8.Text = sec;
                    RoomMon8.Text = room;
                    mon8crs.Text = course;
                    mon8yr.Text = year;
                    mon8prof.Text = "";
                    mon8yrRoom.Text = "";
                    mon8secRoom.Text = "";
                    label52.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel14.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel14.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel14.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel14.BackColor = Color.White;
                    }
                    classType = "";
                    day = 22;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }

                if (time == 2 && day == 22)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label31.Visible = false;
                    }
                    else
                    {
                        label31.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue8.Text = subj;
                    }
                    SecProfTue8.Text = sec;
                    RoomTue8.Text = room;
                    tue8crs.Text = course;
                    tue8yr.Text = year;
                    tue8prof.Text = "";
                    tue8yrRoom.Text = "";
                    tue8secRoom.Text = "";
                    label55.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel13.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel13.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel13.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel13.BackColor = Color.White;
                    }
                    classType = "";
                    day = 32;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 32)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label34.Visible = false;
                    }
                    else
                    {
                        label34.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed8.Text = subj;
                    }
                    SecProfWed8.Text = sec;
                    RoomWed8.Text = room;
                    wed8crs.Text = course;
                    wed8yr.Text = year;
                    wed8prof.Text = "";
                    wed8yrRoom.Text = "";
                    wed8secRoom.Text = "";
                    label61.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel12.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel12.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel12.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel12.BackColor = Color.White;
                    }
                    classType = "";
                    day = 42;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 42)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label36.Visible = false;
                    }
                    else
                    {
                        label36.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu8.Text = subj;
                    }
                    SecProfThu8.Text = sec;
                    RoomThu8.Text = room;
                    thu8crs.Text = course;
                    thu8yr.Text = year;
                    thu8prof.Text = "";
                    thu8yrRoom.Text = "";
                    thu8secRoom.Text = "";
                    label67.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel11.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel11.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel11.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel11.BackColor = Color.White;
                    }
                    classType = "";
                    day = 52;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 52)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label37.Visible = false;
                    }
                    else
                    {
                        label37.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri8.Text = subj;
                    }
                    SecProfFri8.Text = sec;
                    RoomFri8.Text = room;
                    fri8crs.Text = course;
                    fri8yr.Text = year;
                    fri8prof.Text = "";
                    fri8yrRoom.Text = "";
                    fri8secRoom.Text = "";
                    Label73.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel10.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel10.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel10.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel10.BackColor = Color.White;
                    }
                    classType = "";
                    day = 62;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 2 && day == 62)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label40.Visible = false;
                    }
                    else
                    {
                        label40.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat8.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat8.Text = subj;
                    }
                    SecProfSat8.Text = sec;
                    RoomSat8.Text = room;
                    sat8crs.Text = course;
                    sat8yr.Text = year;
                    sat8prof.Text = "";
                    sat8yrRoom.Text = "";
                    sat8secRoom.Text = "";
                    label76.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel4.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel4.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel4.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel4.BackColor = Color.White;
                    }
                    classType = "";
                    day = 3;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  9-10 am 
                if (time == 3 && day == 3)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label87.Visible = false;
                    }
                    else
                    {
                        label87.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon9.Text = subj;
                    }
                    SecProfMon9.Text = sec;
                    RoomMon9.Text = room;
                    mon9crs.Text = course;
                    mon9yr.Text = year;
                    mon9prof.Text = "";
                    mon9yrRoom.Text = "";
                    mon9secRoom.Text = "";
                    label104.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel26.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel26.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel26.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel26.BackColor = Color.White;
                    }
                    classType = "";
                    day = 23;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 3 && day == 23)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label81.Visible = false;
                    }
                    else
                    {
                        label81.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue9.Text = subj;
                    }
                    SecProfTue9.Text = sec;
                    RoomTue9.Text = room;
                    tue9crs.Text = course;
                    tue9yr.Text = year;
                    tue9prof.Text = "";
                    tue9yrRoom.Text = "";
                    tue9secRoom.Text = "";
                    label100.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel25.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel25.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel25.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel25.BackColor = Color.White;
                    }
                    classType = "";
                    day = 33;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 3 && day == 33)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label82.Visible = false;
                    }
                    else
                    {
                        label82.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed9.Text = subj;
                    }
                    SecProfWed9.Text = sec;
                    RoomWed9.Text = room;
                    wed9crs.Text = course;
                    wed9yr.Text = year;
                    wed9prof.Text = "";
                    wed9yrRoom.Text = "";
                    wed9secRoom.Text = "";
                    label96.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel24.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel24.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel24.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel24.BackColor = Color.White;
                    }
                    classType = "";
                    day = 43;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 3 && day == 43)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label69.Visible = false;
                    }
                    else
                    {
                        label69.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu9.Text = subj;
                    }
                    SecProfThu9.Text = sec;
                    RoomThu9.Text = room;
                    thu9crs.Text = course;
                    thu9yr.Text = year;
                    thu9prof.Text = "";
                    thu9yrRoom.Text = "";
                    thu9secRoom.Text = "";
                    label91.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel23.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel23.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel23.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel23.BackColor = Color.White;
                    }
                    classType = "";
                    day = 53;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 3 && day == 53)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label51.Visible = false;
                    }
                    else
                    {
                        label51.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri9.Text = subj;
                    }
                    SecProfFri9.Text = sec;
                    RoomFri9.Text = room;
                    fri9crs.Text = course;
                    fri9yr.Text = year;
                    fri9prof.Text = "";
                    fri9yrRoom.Text = "";
                    fri9secRoom.Text = "";
                    label88.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel20.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel20.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel20.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel20.BackColor = Color.White;
                    }
                    classType = "";
                    day = 63;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 3 && day == 63)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label21.Visible = false;
                    }
                    else
                    {
                        label21.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat9.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat9.Text = subj;
                    }
                    SecProfSat9.Text = sec;
                    RoomSat9.Text = room;
                    sat9crs.Text = course;
                    sat9yr.Text = year;
                    sat9prof.Text = "";
                    sat9yrRoom.Text = "";
                    sat9secRoom.Text = "";
                    label80.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel17.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel17.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel17.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel17.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 4;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  10-11 am 
                if (time == 4 && day == 4)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label63.Visible = false;
                    }
                    else
                    {
                        label63.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon10.Text = subj;
                    }
                    SecProfMon10.Text = sec;
                    RoomMon10.Text = room;
                    mon10crs.Text = course;
                    mon10yr.Text = year;
                    mon10prof.Text = "";
                    mon10yrRoom.Text = "";
                    mon10secRoom.Text = "";
                    label109.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel22.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel22.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel22.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel22.BackColor = Color.White;
                    }
                    classType = "";
                    day = 24;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 4 && day == 24)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label57.Visible = false;
                    }
                    else
                    {
                        label57.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue10.Text = subj;
                    }
                    SecProfTue10.Text = sec;
                    RoomTue10.Text = room;
                    tue10crs.Text = course;
                    tue10yr.Text = year;
                    tue10prof.Text = "";
                    tue10yrRoom.Text = "";
                    tue10secRoom.Text = "";
                    label113.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel21.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel21.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel21.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel21.BackColor = Color.White;
                    }
                    classType = "";
                    day = 34;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 4 && day == 34)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label45.Visible = false;
                    }
                    else
                    {
                        label45.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed10.Text = subj;
                    }
                    SecProfWed10.Text = sec;
                    RoomWed10.Text = room;
                    wed10crs.Text = course;
                    wed10yr.Text = year;
                    wed10prof.Text = "";
                    wed10yrRoom.Text = "";
                    wed10secRoom.Text = "";
                    label118.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel19.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel19.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel19.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel19.BackColor = Color.White;
                    }
                    classType = "";
                    day = 44;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 4 && day == 44)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label27.Visible = false;
                    }
                    else
                    {
                        label27.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu10.Text = subj;
                    }
                    SecProfThu10.Text = sec;
                    RoomThu10.Text = room;
                    thu10crs.Text = course;
                    thu10yr.Text = year;
                    thu10prof.Text = "";
                    thu10yrRoom.Text = "";
                    thu10secRoom.Text = "";
                    label121.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel18.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel18.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel18.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel18.BackColor = Color.White;
                    }
                    classType = "";
                    day = 54;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 4 && day == 54)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label15.Visible = false;
                    }
                    else
                    {
                        label15.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri10.Text = subj;
                    }
                    SecProfFri10.Text = sec;
                    RoomFri10.Text = room;
                    fri10crs.Text = course;
                    fri10yr.Text = year;
                    fri10prof.Text = "";
                    fri10yrRoom.Text = "";
                    fri10secRoom.Text = "";
                    label126.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel16.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel16.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel16.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel16.BackColor = Color.White;
                    }
                    classType = "";
                    day = 64;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 4 && day == 64)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label2.Visible = false;
                    }
                    else
                    {
                        label2.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat10.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat10.Text = subj;
                    }
                    SecProfSat10.Text = sec;
                    RoomSat10.Text = room;
                    sat10crs.Text = course;
                    sat10yr.Text = year;
                    sat10prof.Text = "";
                    sat10yrRoom.Text = "";
                    sat10secRoom.Text = "";
                    label131.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel15.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel15.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel15.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel15.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 5;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  11-12 pm 
                if (time == 5 && day == 5)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label171.Visible = false;
                    }
                    else
                    {
                        label171.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon11.Text = subj;
                    }
                    SecProfMon11.Text = sec;
                    RoomMon11.Text = room;
                    mon11crs.Text = course;
                    mon11yr.Text = year;
                    mon11prof.Text = "";
                    mon11yrRoom.Text = "";
                    mon11secRoom.Text = "";
                    label156.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel50.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel50.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel50.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel50.BackColor = Color.White;
                    }
                    classType = "";
                    day = 25;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 5 && day == 25)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label165.Visible = false;
                    }
                    else
                    {
                        label165.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue11.Text = subj;
                    }
                    SecProfTue11.Text = sec;
                    RoomTue11.Text = room;
                    tue11crs.Text = course;
                    tue11yr.Text = year;
                    tue11prof.Text = "";
                    tue11yrRoom.Text = "";
                    tue11secRoom.Text = "";
                    label151.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel49.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel49.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel49.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel49.BackColor = Color.White;
                    }
                    classType = "";
                    day = 35;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 5 && day == 35)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label159.Visible = false;
                    }
                    else
                    {
                        label159.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed11.Text = subj;
                    }
                    SecProfWed11.Text = sec;
                    RoomWed11.Text = room;
                    wed11crs.Text = course;
                    wed11yr.Text = year;
                    wed11prof.Text = "";
                    wed11yrRoom.Text = "";
                    wed11secRoom.Text = "";
                    label148.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel48.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel48.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel48.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel48.BackColor = Color.White;
                    }
                    classType = "";
                    day = 45;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 5 && day == 45)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label153.Visible = false;
                    }
                    else
                    {
                        label153.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu11.Text = subj;
                    }
                    SecProfThu11.Text = sec;
                    RoomThu11.Text = room;
                    thu11crs.Text = course;
                    thu11yr.Text = year;
                    thu11prof.Text = "";
                    thu11yrRoom.Text = "";
                    thu11secRoom.Text = "";
                    label144.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel47.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel47.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel47.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel47.BackColor = Color.White;
                    }
                    classType = "";
                    day = 55;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 5 && day == 55)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label111.Visible = false;
                    }
                    else
                    {
                        label111.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri11.Text = subj;
                    }
                    SecProfFri11.Text = sec;
                    RoomFri11.Text = room;
                    fri11crs.Text = course;
                    fri11yr.Text = year;
                    fri11prof.Text = "";
                    fri11yrRoom.Text = "";
                    fri11secRoom.Text = "";
                    label139.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel40.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel40.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel40.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel40.BackColor = Color.White;
                    }
                    classType = "";
                    day = 65;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 5 && day == 65)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label65.Visible = false;
                    }
                    else
                    {
                        label65.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat11.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat11.Text = subj;
                    }
                    SecProfSat11.Text = sec;
                    RoomSat11.Text = room;
                    sat11crs.Text = course;
                    sat11yr.Text = year;
                    sat11prof.Text = "";
                    sat11yrRoom.Text = "";
                    sat11secRoom.Text = "";
                    label134.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel33.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel33.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel33.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel33.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 6;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  12-1 pm 
                if (time == 6 && day == 6)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label135.Visible = false;
                    }
                    else
                    {
                        label135.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon12.Text = subj;
                    }
                    SecProfMon12.Text = sec;
                    RoomMon12.Text = room;
                    mon12crs.Text = course;
                    mon12yr.Text = year;
                    mon12prof.Text = "";
                    mon12yrRoom.Text = "";
                    mon12secRoom.Text = "";
                    label161.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel44.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel44.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel44.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel44.BackColor = Color.White;
                    }
                    classType = "";
                    day = 26;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 6 && day == 26)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label123.Visible = false;
                    }
                    else
                    {
                        label123.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue12.Text = subj;
                    }
                    SecProfTue12.Text = sec;
                    RoomTue12.Text = room;
                    tue12crs.Text = course;
                    tue12yr.Text = year;
                    tue12prof.Text = "";
                    tue12yrRoom.Text = "";
                    tue12secRoom.Text = "";
                    label164.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel42.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel42.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel42.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel42.BackColor = Color.White;
                    }
                    classType = "";
                    day = 36;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 6 && day == 36)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label99.Visible = false;
                    }
                    else
                    {
                        label99.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed12.Text = subj;
                    }
                    SecProfWed12.Text = sec;
                    RoomWed12.Text = room;
                    wed12crs.Text = course;
                    wed12yr.Text = year;
                    wed12prof.Text = "";
                    wed12yrRoom.Text = "";
                    wed12secRoom.Text = "";
                    label169.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel38.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel38.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel38.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel38.BackColor = Color.White;
                    }
                    classType = "";
                    day = 46;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 6 && day == 46)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label78.Visible = false;
                    }
                    else
                    {
                        label78.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu12.Text = subj;
                    }
                    SecProfThu12.Text = sec;
                    RoomThu12.Text = room;
                    thu12crs.Text = course;
                    thu12yr.Text = year;
                    thu12prof.Text = "";
                    thu12yrRoom.Text = "";
                    thu12secRoom.Text = "";
                    label173.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel35.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel35.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel35.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel35.BackColor = Color.White;
                    }
                    classType = "";
                    day = 56;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 6 && day == 56)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label50.Visible = false;
                    }
                    else
                    {
                        label50.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri12.Text = subj;
                    }
                    SecProfFri12.Text = sec;
                    RoomFri12.Text = room;
                    fri12crs.Text = course;
                    fri12yr.Text = year;
                    fri12prof.Text = "";
                    fri12yrRoom.Text = "";
                    fri12secRoom.Text = "";
                    label177.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel31.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel31.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel31.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel31.BackColor = Color.White;
                    }
                    classType = "";
                    day = 66;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 6 && day == 66)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label17.Visible = false;
                    }
                    else
                    {
                        label17.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat12.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat12.Text = subj;
                    }
                    SecProfSat12.Text = sec;
                    RoomSat12.Text = room;
                    sat12crs.Text = course;
                    sat12yr.Text = year;
                    sat12prof.Text = "";
                    sat12yrRoom.Text = "";
                    sat12secRoom.Text = "";
                    label181.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel28.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel28.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel28.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel28.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 7;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  1-2 pm 
                if (time == 7 && day == 7)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label147.Visible = false;
                    }
                    else
                    {
                        label147.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon1.Text = subj;
                    }
                    SecProfMon1.Text = sec;
                    RoomMon1.Text = room;
                    mon1crs.Text = course;
                    mon1yr.Text = year;
                    mon1prof.Text = "";
                    mon1yrRoom.Text = "";
                    mon1secRoom.Text = "";
                    label202.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel46.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel46.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel46.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel46.BackColor = Color.White;
                    }
                    classType = "";
                    day = 27;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 7 && day == 27)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label141.Visible = false;
                    }
                    else
                    {
                        label141.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue1.Text = subj;
                    }
                    SecProfTue1.Text = sec;
                    RoomTue1.Text = room;
                    tue1crs.Text = course;
                    tue1yr.Text = year;
                    tue1prof.Text = "";
                    tue1yrRoom.Text = "";
                    tue1secRoom.Text = "";
                    label199.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel45.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel45.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel45.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel45.BackColor = Color.White;
                    }
                    classType = "";
                    day = 37;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 7 && day == 37)
                {

                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label129.Visible = false;
                    }
                    else
                    {
                        label129.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed1.Text = subj;
                    }
                    SecProfWed1.Text = sec;
                    RoomWed1.Text = room;
                    wed1crs.Text = course;
                    wed1yr.Text = year;
                    wed1prof.Text = "";
                    wed1yrRoom.Text = "";
                    wed1secRoom.Text = "";
                    label195.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel43.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel43.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel43.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel43.BackColor = Color.White;
                    }
                    classType = "";
                    day = 47;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 7 && day == 47)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label117.Visible = false;
                    }
                    else
                    {
                        label117.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu1.Text = subj;
                    }
                    SecProfThu1.Text = sec;
                    RoomThu1.Text = room;
                    thu1crs.Text = course;
                    thu1yr.Text = year;
                    thu1prof.Text = "";
                    thu1yrRoom.Text = "";
                    thu1secRoom.Text = "";
                    label191.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel41.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel41.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel41.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel41.BackColor = Color.White;
                    }
                    classType = "";
                    day = 57;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 7 && day == 57)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label86.Visible = false;
                    }
                    else
                    {
                        label86.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri1.Text = subj;
                    }
                    SecProfFri1.Text = sec;
                    RoomFri1.Text = room;
                    fri1crs.Text = course;
                    fri1yr.Text = year;
                    fri1prof.Text = "";
                    fri1yrRoom.Text = "";
                    fri1secRoom.Text = "";
                    label188.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel36.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel36.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel36.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel36.BackColor = Color.White;
                    }
                    classType = "";
                    day = 67;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 7 && day == 67)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label43.Visible = false;
                    }
                    else
                    {
                        label43.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat1.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat1.Text = subj;
                    }
                    SecProfSat1.Text = sec;
                    RoomSat1.Text = room;
                    sat1crs.Text = course;
                    sat1yr.Text = year;
                    sat1prof.Text = "";
                    sat1yrRoom.Text = "";
                    sat1secRoom.Text = "";
                    label184.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel30.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel30.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel30.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel30.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 8;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  2-3 pm 
                if (time == 8 && day == 8)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label105.Visible = false;
                    }
                    else
                    {
                        label105.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon2.Text = subj;
                    }
                    SecProfMon2.Text = sec;
                    RoomMon2.Text = room;
                    mon2crs.Text = course;
                    mon2yr.Text = year;
                    mon2prof.Text = "";
                    mon2yrRoom.Text = "";
                    mon2secRoom.Text = "";
                    label206.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel39.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel39.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel39.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel39.BackColor = Color.White;
                    }
                    classType = "";
                    day = 28;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 8 && day == 28)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label93.Visible = false;
                    }
                    else
                    {
                        label93.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue2.Text = subj;
                    }
                    SecProfTue2.Text = sec;
                    RoomTue2.Text = room;
                    tue2crs.Text = course;
                    tue2yr.Text = year;
                    tue2prof.Text = "";
                    tue2yrRoom.Text = "";
                    tue2secRoom.Text = "";
                    label209.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel37.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel37.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel37.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel37.BackColor = Color.White;
                    }
                    classType = "";
                    day = 38;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 8 && day == 38)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label72.Visible = false;
                    }
                    else
                    {
                        label72.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed2.Text = subj;
                    }
                    SecProfWed2.Text = sec;
                    RoomWed2.Text = room;
                    wed2crs.Text = course;
                    wed2yr.Text = year;
                    wed2prof.Text = "";
                    wed2yrRoom.Text = "";
                    wed2secRoom.Text = "";
                    label213.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel34.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel34.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel34.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel34.BackColor = Color.White;
                    }
                    classType = "";
                    day = 48;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 8 && day == 48)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label58.Visible = false;
                    }
                    else
                    {
                        label58.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu2.Text = subj;
                    }
                    SecProfThu2.Text = sec;
                    RoomThu2.Text = room;
                    thu2crs.Text = course;
                    thu2yr.Text = year;
                    thu2prof.Text = "";
                    thu2yrRoom.Text = "";
                    thu2secRoom.Text = "";
                    label217.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel32.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel32.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel32.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel32.BackColor = Color.White;
                    }
                    classType = "";
                    day = 58;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 8 && day == 58)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label24.Visible = false;
                    }
                    else
                    {
                        label24.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri2.Text = subj;
                    }
                    SecProfFri2.Text = sec;
                    RoomFri2.Text = room;
                    fri2crs.Text = course;
                    fri2yr.Text = year;
                    fri2prof.Text = "";
                    fri2yrRoom.Text = "";
                    fri2secRoom.Text = "";
                    label220.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel29.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel29.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel29.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel29.BackColor = Color.White;
                    }
                    classType = "";
                    day = 68;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 8 && day == 68)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label8.Visible = false;
                    }
                    else
                    {
                        label8.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat2.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat2.Text = subj;
                    }
                    SecProfSat2.Text = sec;
                    RoomSat2.Text = room;
                    sat2crs.Text = course;
                    sat2yr.Text = year;
                    sat2prof.Text = "";
                    sat2yrRoom.Text = "";
                    sat2secRoom.Text = "";
                    label224.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel27.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel27.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel27.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel27.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 9;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  3-4 pm 
                if (time == 9 && day == 9)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label264.Visible = false;
                    }
                    else
                    {
                        label264.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon3.Text = subj;
                    }
                    SecProfMon3.Text = sec;
                    RoomMon3.Text = room;
                    mon3crs.Text = course;
                    mon3yr.Text = year;
                    mon3prof.Text = "";
                    mon3yrRoom.Text = "";
                    mon3secRoom.Text = "";
                    label245.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel85.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel85.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel85.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel85.BackColor = Color.White;
                    }
                    classType = "";
                    day = 29;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 9 && day == 29)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label246.Visible = false;
                    }
                    else
                    {
                        label246.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue3.Text = subj;
                    }
                    SecProfTue3.Text = sec;
                    RoomTue3.Text = room;
                    tue3crs.Text = course;
                    tue3yr.Text = year;
                    tue3prof.Text = "";
                    tue3yrRoom.Text = "";
                    tue3secRoom.Text = "";
                    label242.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel82.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel82.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel82.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel82.BackColor = Color.White;
                    }
                    classType = "";
                    day = 39;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 9 && day == 39)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label228.Visible = false;
                    }
                    else
                    {
                        label228.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed3.Text = subj;
                    }
                    SecProfWed3.Text = sec;
                    RoomWed3.Text = room;
                    wed3crs.Text = course;
                    wed3yr.Text = year;
                    wed3prof.Text = "";
                    wed3yrRoom.Text = "";
                    wed3secRoom.Text = "";
                    label238.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel79.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel79.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel79.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel79.BackColor = Color.White;
                    }
                    classType = "";
                    day = 49;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 9 && day == 49)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label210.Visible = false;
                    }
                    else
                    {
                        label210.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu3.Text = subj;
                    }
                    SecProfThu3.Text = sec;
                    RoomThu3.Text = room;
                    thu3crs.Text = course;
                    thu3yr.Text = year;
                    thu3prof.Text = "";
                    thu3yrRoom.Text = "";
                    thu3secRoom.Text = "";
                    label235.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel76.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel76.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel76.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel76.BackColor = Color.White;
                    }
                    classType = "";
                    day = 59;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 9 && day == 59)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label152.Visible = false;
                    }
                    else
                    {
                        label152.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri3.Text = subj;
                    }
                    SecProfFri3.Text = sec;
                    RoomFri3.Text = room;
                    fri3crs.Text = course;
                    fri3yr.Text = year;
                    fri3prof.Text = "";
                    fri3yrRoom.Text = "";
                    fri3secRoom.Text = "";
                    label231.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel67.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel67.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel67.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel67.BackColor = Color.White;
                    }
                    classType = "";
                    day = 69;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 9 && day == 69)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label84.Visible = false;
                    }
                    else
                    {
                        label84.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat3.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat3.Text = subj;
                    }
                    SecProfSat3.Text = sec;
                    RoomSat3.Text = room;
                    sat3crs.Text = course;
                    sat3yr.Text = year;
                    sat3prof.Text = "";
                    sat3yrRoom.Text = "";
                    sat3secRoom.Text = "";
                    label227.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel58.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel58.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel58.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel58.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 10;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  4-5 pm 
                if (time == 10 && day == 10)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label192.Visible = false;
                    }
                    else
                    {
                        label192.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon4.Text = subj;
                    }
                    SecProfMon4.Text = sec;
                    RoomMon4.Text = room;
                    mon4crs.Text = course;
                    mon4yr.Text = year;
                    mon4prof.Text = "";
                    mon4yrRoom.Text = "";
                    mon4secRoom.Text = "";
                    label249.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel73.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel73.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel73.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel73.BackColor = Color.White;
                    }
                    classType = "";
                    day = 210;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 10 && day == 210)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label174.Visible = false;
                    }
                    else
                    {
                        label174.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue4.Text = subj;
                    }
                    SecProfTue4.Text = sec;
                    RoomTue4.Text = room;
                    tue4crs.Text = course;
                    tue4yr.Text = year;
                    tue4prof.Text = "";
                    tue4yrRoom.Text = "";
                    tue4secRoom.Text = "";
                    label253.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel70.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel70.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel70.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel70.BackColor = Color.White;
                    }
                    classType = "";
                    day = 310;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 10 && day == 310)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label130.Visible = false;
                    }
                    else
                    {
                        label130.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed4.Text = subj;
                    }
                    SecProfWed4.Text = sec;
                    RoomWed4.Text = room;
                    wed4crs.Text = course;
                    wed4yr.Text = year;
                    wed4prof.Text = "";
                    wed4yrRoom.Text = "";
                    wed4secRoom.Text = "";
                    label256.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel64.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel64.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel64.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel64.BackColor = Color.White;
                    }
                    classType = "";
                    day = 410;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 10 && day == 410)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label108.Visible = false;
                    }
                    else
                    {
                        label108.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu4.Text = subj;
                    }
                    SecProfThu4.Text = sec;
                    RoomThu4.Text = room;
                    thu4crs.Text = course;
                    thu4yr.Text = year;
                    thu4prof.Text = "";
                    thu4yrRoom.Text = "";
                    thu4secRoom.Text = "";
                    label260.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel61.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel61.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel61.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel61.BackColor = Color.White;
                    }
                    classType = "";
                    day = 510;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 10 && day == 510)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label56.Visible = false;
                    }
                    else
                    {
                        label56.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri4.Text = subj;
                    }
                    SecProfFri4.Text = sec;
                    RoomFri4.Text = room;
                    fri4crs.Text = course;
                    fri4yr.Text = year;
                    fri4prof.Text = "";
                    fri4yrRoom.Text = "";
                    fri4secRoom.Text = "";
                    label263.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel55.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel55.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel55.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel55.BackColor = Color.White;
                    }
                    classType = "";
                    day = 610;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 10 && day == 610)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label19.Visible = false;
                    }
                    else
                    {
                        label19.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat4.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat4.Text = subj;
                    }
                    SecProfSat4.Text = sec;
                    RoomSat4.Text = room;
                    sat4crs.Text = course;
                    sat4yr.Text = year;
                    sat4prof.Text = "";
                    sat4yrRoom.Text = "";
                    sat4secRoom.Text = "";
                    label267.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel52.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel52.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel52.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel52.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 11;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  5-6 pm 
                if (time == 11 && day == 11)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label270.Visible = false;
                    }
                    else
                    {
                        label270.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon5.Text = subj;
                    }
                    SecProfMon5.Text = sec;
                    RoomMon5.Text = room;
                    mon5crs.Text = course;
                    mon5yr.Text = year;
                    mon5prof.Text = "";
                    mon5yrRoom.Text = "";
                    mon5secRoom.Text = "";
                    label286.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel86.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel86.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel86.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel86.BackColor = Color.White;
                    }
                    classType = "";
                    day = 211;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 11 && day == 211)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label258.Visible = false;
                    }
                    else
                    {
                        label258.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue5.Text = subj;
                    }
                    SecProfTue5.Text = sec;
                    RoomTue5.Text = room;
                    tue5crs.Text = course;
                    tue5yr.Text = year;
                    tue5prof.Text = "";
                    tue5yrRoom.Text = "";
                    tue5secRoom.Text = "";
                    label283.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel84.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel84.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel84.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel84.BackColor = Color.White;
                    }
                    classType = "";
                    day = 311;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 11 && day == 311)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label252.Visible = false;
                    }
                    else
                    {
                        label252.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed5.Text = subj;
                    }
                    SecProfWed5.Text = sec;
                    RoomWed5.Text = room;
                    wed5crs.Text = course;
                    wed5yr.Text = year;
                    wed5prof.Text = "";
                    wed5yrRoom.Text = "";
                    wed5secRoom.Text = "";
                    label280.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel83.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel83.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel83.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel83.BackColor = Color.White;
                    }
                    classType = "";
                    day = 411;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 11 && day == 411)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label240.Visible = false;
                    }
                    else
                    {
                        label240.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu5.Text = subj;
                    }
                    SecProfThu5.Text = sec;
                    RoomThu5.Text = room;
                    thu5crs.Text = course;
                    thu5yr.Text = year;
                    thu5prof.Text = "";
                    thu5yrRoom.Text = "";
                    thu5secRoom.Text = "";
                    label277.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel81.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel81.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel81.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel81.BackColor = Color.White;
                    }
                    classType = "";
                    day = 511;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 11 && day == 511)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label180.Visible = false;
                    }
                    else
                    {
                        label180.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri5.Text = subj;
                    }
                    SecProfFri5.Text = sec;
                    RoomFri5.Text = room;
                    fri5crs.Text = course;
                    fri5yr.Text = year;
                    fri5prof.Text = "";
                    fri5yrRoom.Text = "";
                    fri5secRoom.Text = "";
                    label274.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel71.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel71.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel71.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel71.BackColor = Color.White;
                    }
                    classType = "";
                    day = 611;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 11 && day == 611)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label101.Visible = false;
                    }
                    else
                    {
                        label101.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat5.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat5.Text = subj;
                    }
                    SecProfSat5.Text = sec;
                    RoomSat5.Text = room;
                    sat5crs.Text = course;
                    sat5yr.Text = year;
                    sat5prof.Text = "";
                    sat5yrRoom.Text = "";
                    sat5secRoom.Text = "";
                    label271.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel60.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel60.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel60.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel60.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 12;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  6-7 pm 
                if (time == 12 && day == 12)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label216.Visible = false;
                    }
                    else
                    {
                        label216.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon6.Text = subj;
                    }
                    SecProfMon6.Text = sec;
                    RoomMon6.Text = room;
                    mon6crs.Text = course;
                    mon6yr.Text = year;
                    mon6prof.Text = "";
                    mon6yrRoom.Text = "";
                    mon6secRoom.Text = "";
                    label289.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel77.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel77.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel77.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel77.BackColor = Color.White;
                    }
                    classType = "";
                    day = 212;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 12 && day == 212)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label198.Visible = false;
                    }
                    else
                    {
                        label198.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue6.Text = subj;
                    }
                    SecProfTue6.Text = sec;
                    RoomTue6.Text = room;
                    tue6crs.Text = course;
                    tue6yr.Text = year;
                    tue6prof.Text = "";
                    tue6yrRoom.Text = "";
                    tue6secRoom.Text = "";
                    label292.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel74.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel74.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel74.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel74.BackColor = Color.White;
                    }
                    classType = "";
                    day = 312;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 12 && day == 312)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label160.Visible = false;
                    }
                    else
                    {
                        label160.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed6.Text = subj;
                    }
                    SecProfWed6.Text = sec;
                    RoomWed6.Text = room;
                    wed6crs.Text = course;
                    wed6yr.Text = year;
                    wed6prof.Text = "";
                    wed6yrRoom.Text = "";
                    wed6secRoom.Text = "";
                    label295.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel68.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel68.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel68.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel68.BackColor = Color.White;
                    }
                    classType = "";
                    day = 412;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 12 && day == 412)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label122.Visible = false;
                    }
                    else
                    {
                        label122.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu6.Text = subj;
                    }
                    SecProfThu6.Text = sec;
                    RoomThu6.Text = room;
                    thu6crs.Text = course;
                    thu6yr.Text = year;
                    thu6prof.Text = "";
                    thu6yrRoom.Text = "";
                    thu6secRoom.Text = "";
                    label298.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel63.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel63.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel63.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel63.BackColor = Color.White;
                    }
                    classType = "";
                    day = 512;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 12 && day == 512)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label74.Visible = false;
                    }
                    else
                    {
                        label74.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri6.Text = subj;
                    }
                    SecProfFri6.Text = sec;
                    RoomFri6.Text = room;
                    fri6crs.Text = course;
                    fri6yr.Text = year;
                    fri6prof.Text = "";
                    fri6yrRoom.Text = "";
                    fri6secRoom.Text = "";
                    label301.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel57.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel57.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel57.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel57.BackColor = Color.White;
                    }
                    classType = "";
                    day = 612;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 12 && day == 612)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label28.Visible = false;
                    }
                    else
                    {
                        label28.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat6.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat6.Text = subj;
                    }
                    SecProfSat6.Text = sec;
                    RoomSat6.Text = room;
                    sat6crs.Text = course;
                    sat6yr.Text = year;
                    sat6prof.Text = "";
                    sat6yrRoom.Text = "";
                    sat6secRoom.Text = "";
                    label304.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel53.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel53.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel53.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel53.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 13;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  7-8 pm 
                if (time == 13 && day == 13)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label234.Visible = false;
                    }
                    else
                    {
                        label234.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon7pm.Text = subj;
                    }
                    SecProfMon7pm.Text = sec;
                    RoomMon7pm.Text = room;
                    mon7pmcrs.Text = course;
                    mon7pmyr.Text = year;
                    mon7pmprof.Text = "";
                    mon7pmyrRoom.Text = "";
                    mon7pmsecRoom.Text = "";
                    label322.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel80.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel80.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel80.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel80.BackColor = Color.White;
                    }
                    classType = "";
                    day = 213;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 13 && day == 213)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label222.Visible = false;
                    }
                    else
                    {
                        label222.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue7pm.Text = subj;
                    }
                    SecProfTue7pm.Text = sec;
                    RoomTue7pm.Text = room;
                    tue7pmcrs.Text = course;
                    tue7pmyr.Text = year;
                    tue7pmprof.Text = "";
                    tue7pmyrRoom.Text = "";
                    tue7pmsecRoom.Text = "";
                    label319.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel78.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel78.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel78.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel78.BackColor = Color.White;
                    }
                    classType = "";
                    day = 313;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 13 && day == 313)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label204.Visible = false;
                    }
                    else
                    {
                        label204.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed7pm.Text = subj;
                    }
                    SecProfWed7pm.Text = sec;
                    RoomWed7pm.Text = room;
                    wed7pmcrs.Text = course;
                    wed7pmyr.Text = year;
                    wed7pmprof.Text = "";
                    wed7pmyrRoom.Text = "";
                    wed7pmsecRoom.Text = "";
                    label316.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel75.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel75.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel75.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel75.BackColor = Color.White;
                    }
                    classType = "";
                    day = 413;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 13 && day == 413)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label186.Visible = false;
                    }
                    else
                    {
                        label186.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu7pm.Text = subj;
                    }
                    SecProfThu7pm.Text = sec;
                    RoomThu7pm.Text = room;
                    thu7pmcrs.Text = course;
                    thu7pmyr.Text = year;
                    thu7pmprof.Text = "";
                    thu7pmyrRoom.Text = "";
                    thu7pmsecRoom.Text = "";
                    label313.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel72.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel72.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel72.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel72.BackColor = Color.White;
                    }
                    classType = "";
                    day = 513;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 13 && day == 513)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label137.Visible = false;
                    }
                    else
                    {
                        label137.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri7pm.Text = subj;
                    }
                    SecProfFri7pm.Text = sec;
                    RoomFri7pm.Text = room;
                    fri7pmcrs.Text = course;
                    fri7pmyr.Text = year;
                    fri7pmprof.Text = "";
                    fri7pmyrRoom.Text = "";
                    fri7pmsecRoom.Text = "";
                    label310.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel65.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel65.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel65.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel65.BackColor = Color.White;
                    }
                    classType = "";
                    day = 613;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 13 && day == 613)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label66.Visible = false;
                    }
                    else
                    {
                        label66.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat7pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat7pm.Text = subj;
                    }
                    SecProfSat7pm.Text = sec;
                    RoomSat7pm.Text = room;
                    sat7pmcrs.Text = course;
                    sat7pmyr.Text = year;
                    sat7pmprof.Text = "";
                    sat7pmyrRoom.Text = "";
                    sat7pmsecRoom.Text = "";
                    label307.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel56.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel56.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel56.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel56.BackColor = Color.White;
                    }
                    classType = "";
                    time += 1;
                    day = 14;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                //  7-8 pm 
                if (time == 14 && day == 14)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label167.Visible = false;
                    }
                    else
                    {
                        label167.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectMon8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectMon8pm.Text = subj;
                    }
                    SecProfMon8pm.Text = sec;
                    RoomMon8pm.Text = room;
                    mon8pmcrs.Text = course;
                    mon8pmyr.Text = year;
                    mon8pmprof.Text = "";
                    mon8pmyrRoom.Text = "";
                    mon8pmsecRoom.Text = "";
                    label325.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel69.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel69.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel69.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel69.BackColor = Color.White;
                    }
                    classType = "";
                    day = 214;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }

                if (time == 14 && day == 214)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label145.Visible = false;
                    }
                    else
                    {
                        label145.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectTue8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectTue8pm.Text = subj;
                    }
                    SecProfTue8pm.Text = sec;
                    RoomTue8pm.Text = room;
                    tue8pmcrs.Text = course;
                    tue8pmyr.Text = year;
                    tue8pmprof.Text = "";
                    tue8pmyrRoom.Text = "";
                    tue8pmsecRoom.Text = "";
                    label328.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel66.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel66.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel66.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel66.BackColor = Color.White;
                    }
                    classType = "";
                    day = 314;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 14 && day == 314)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label115.Visible = false;
                    }
                    else
                    {
                        label115.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectWed8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectWed8pm.Text = subj;
                    }
                    SecProfWed8pm.Text = sec;
                    RoomWed8pm.Text = room;
                    wed8pmcrs.Text = course;
                    wed8pmyr.Text = year;
                    wed8pmprof.Text = "";
                    wed8pmyrRoom.Text = "";
                    wed8pmsecRoom.Text = "";
                    label331.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel62.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel62.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel62.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel62.BackColor = Color.White;
                    }
                    classType = "";
                    day = 414;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 14 && day == 414)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label92.Visible = false;
                    }
                    else
                    {
                        label92.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectThu8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectThu8pm.Text = subj;
                    }
                    SecProfThu8pm.Text = sec;
                    RoomThu8pm.Text = room;
                    thu8pmcrs.Text = course;
                    thu8pmyr.Text = year;
                    thu8pmprof.Text = "";
                    thu8pmyrRoom.Text = "";
                    thu8pmsecRoom.Text = "";
                    label334.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel59.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel59.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel59.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel59.BackColor = Color.White;
                    }
                    classType = "";
                    day = 514;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 14 && day == 514)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label48.Visible = false;
                    }
                    else
                    {
                        label48.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectFri8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectFri8pm.Text = subj;
                    }
                    SecProfFri8pm.Text = sec;
                    RoomFri8pm.Text = room;
                    fri8pmcrs.Text = course;
                    fri8pmyr.Text = year;
                    fri8pmprof.Text = "";
                    fri8pmyrRoom.Text = "";
                    fri8pmsecRoom.Text = "";
                    label337.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel54.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel54.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel54.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel54.BackColor = Color.White;
                    }
                    classType = "";
                    day = 614;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
                if (time == 14 && day == 614)
                {
                    if (subj == "" || subj == "Consultation Hours" || subj == "Research And Extension")
                    {
                        label9.Visible = false;
                    }
                    else
                    {
                        label9.Visible = true;
                    }
                    if (subj == "Research And Extension")
                    {
                        subjectSat8pm.Text = "Research And \n Extension";
                    }
                    else
                    {
                        subjectSat8pm.Text = subj;
                    }
                    SecProfSat8pm.Text = sec;
                    RoomSat8pm.Text = room;
                    sat8pmcrs.Text = course;
                    sat8pmyr.Text = year;
                    sat8pmprof.Text = "";
                    sat8pmyrRoom.Text = "";
                    sat8pmsecRoom.Text = "";
                    label340.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel51.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel51.BackColor = Color.Yellow;
                    }
                    else if (classType == "N/A")
                    {
                        panel51.BackColor = Color.DeepSkyBlue;
                    }
                    else if (classType == "")
                    {
                        panel51.BackColor = Color.White;
                    }
                    classType = "";
                    day = 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    schedule();
                }
            }
            else if (sched.comboBox1.Text == "Section")
            {
                subj = "";
                room = "";
                sec = "";
                year = "";
                course = "";
                facultyName = "";
                facultyCode = "";
                day = 1;
                time = 1;
                 PopulateGridViewSection();
                lbl1.Text = "COURSE: " + sched.dgvFaculty.CurrentRow.Cells[0].Value.ToString();
                lbl2.Text = "YEAR: " + sched.dgvFaculty.CurrentRow.Cells[1].Value.ToString();
                lbl3.Text = "MAJOR:";
                lbl4.Text = "SECTION: " + sched.dgvFaculty.CurrentRow.Cells[2].Value.ToString();
                schedule();
               // MessageBox.Show(subj);
                if (time == 1 && day == 1)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon7.Text = subj;
                        mon7prof.Text = facultyName;
                        RoomMon7.Text = room;
                        SecProfMon7.Text = "";
                        mon7yr.Text = "";
                        label29.Visible = false;
                        mon7yrRoom.Text = "";
                        mon7secRoom.Text = "";
                        label10.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel3.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel3.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel3.BackColor = Color.White;
                        }
                        classType = "";
                        day = 21;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 21;

                        subjectMon7.Text = "";
                        mon7prof.Text = "";
                        mon7crs.Text = "";
                        RoomMon7.Text = "";
                        SecProfMon7.Text = "";
                        mon7yr.Text = "";
                        label29.Visible = false;
                        mon7yrRoom.Text = "";
                        mon7secRoom.Text = "";
                        label10.Visible = false;
                        panel3.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 1 && day == 21)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue7.Text = subj;
                        tue7prof.Text = facultyName;
                        RoomTue7.Text = room;
                        SecProfTue7.Text = "";
                        tue7yr.Text = "";
                        label32.Visible = false;
                        tue7yrRoom.Text = "";
                        tue7secRoom.Text = "";
                        label13.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel5.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel5.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel5.BackColor = Color.White;
                        }
                        classType = "";
                        day = 31;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 31;
                        subjectTue7.Text = "";
                        tue7prof.Text = "";
                        tue7crs.Text = "";
                        RoomTue7.Text = "";
                        SecProfTue7.Text = "";
                        tue7yr.Text = "";
                        label32.Visible = false;
                        tue7yrRoom.Text = "";
                        tue7secRoom.Text = "";
                        label13.Visible = false;
                        panel5.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 1 && day == 31)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed7.Text = subj;
                        wed7prof.Text = facultyName;
                        RoomWed7.Text = room;
                        SecProfWed7.Text = "";
                        Wed7yr.Text = "";
                        label33.Visible = false;
                        wed7yrRoom.Text = "";
                        wed7secRoom.Text = "";
                        label18.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel6.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel6.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel6.BackColor = Color.White;
                        }
                        classType = "";
                        day = 41;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 41;
                        subjectWed7.Text = "";
                        wed7prof.Text = "";
                        wed7crs.Text = "";
                        RoomWed7.Text = "";
                        SecProfWed7.Text = "";
                        Wed7yr.Text = "";
                        label33.Visible = false;
                        wed7yrRoom.Text = "";
                        wed7secRoom.Text = "";
                        label18.Visible = false;
                        panel6.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 1 && day == 41)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu7.Text = subj;
                        thu7prof.Text = facultyName;
                        RoomThu7.Text = room;
                        SecProfThu7.Text = "";
                        Thu7yr.Text = "";
                        label35.Visible = false;
                        thu7yrRoom.Text = "";
                        thu7secRoom.Text = "";
                        label23.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel7.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel7.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel7.BackColor = Color.White;
                        }
                        classType = "";
                        day = 51;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 51;
                        subjectThu7.Text = "";
                        thu7prof.Text = "";
                        Thu7crs.Text = "";
                        RoomThu7.Text = "";
                        SecProfThu7.Text = "";
                        Thu7yr.Text = "";
                        label35.Visible = false;
                        thu7yrRoom.Text = "";
                        thu7secRoom.Text = "";
                        label23.Visible = false;
                        panel7.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 1 && day == 51)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri7.Text = subj;
                        fri7prof.Text = facultyName;
                        RoomFri7.Text = room;
                        SecProfFri7.Text = "";
                        Fri7yr.Text = "";
                        label38.Visible = false;
                        fri7yrRoom.Text = "";
                        fri7secRoom.Text = "";
                        label41.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel8.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel8.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel8.BackColor = Color.White;
                        }
                        classType = "";
                        day = 61;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 61;
                        subjectFri7.Text = "";
                        fri7prof.Text = "";
                        Fri7crs.Text = "";
                        RoomFri7.Text = "";
                        SecProfFri7.Text = "";
                        Fri7yr.Text = "";
                        label38.Visible = false;
                        fri7yrRoom.Text = "";
                        fri7secRoom.Text = "";
                        label41.Visible = false;
                        panel8.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
               
                }
                if (time == 1 && day == 61)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat7.Text = subj;
                        sat7prof.Text = facultyName;
                        RoomSat7.Text = room;
                        SecProfSat7.Text = "";
                        Sat7yr.Text = "";
                        label39.Visible = false;
                        sat7yrRoom.Text = "";
                        sat7secRoom.Text = "";
                        label46.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel9.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel9.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel9.BackColor = Color.White;
                        }
                        classType = "";
                        day = 2;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 2;
                        time += 1;
                        subjectSat7.Text = "";
                        sat7prof.Text = "";
                        Sat7crs.Text = "";
                        RoomSat7.Text = "";
                        SecProfSat7.Text = "";
                        Sat7yr.Text = "";
                        label39.Visible = false;
                        sat7yrRoom.Text = "";
                        sat7secRoom.Text = "";
                        label46.Visible = false;
                        panel9.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 2 && day == 2)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon8.Text = subj;
                        mon8prof.Text = facultyName;
                        RoomMon8.Text = room;
                        SecProfMon8.Text = "";
                        mon8yr.Text = "";
                        label30.Visible = false;
                        mon8yrRoom.Text = "";
                        mon8secRoom.Text = "";
                        label52.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel14.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel14.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel14.BackColor = Color.White;
                        }
                        classType = "";
                        day = 22;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 22;
                        subjectMon8.Text = "";
                        mon8prof.Text = "";
                        mon8crs.Text = "";
                        RoomMon8.Text = "";
                        SecProfMon8.Text = "";
                        mon8yr.Text = "";
                        label30.Visible = false;
                        mon8yrRoom.Text = "";
                        mon8secRoom.Text = "";
                        label52.Visible = false;
                        panel14.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 2 && day == 22)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue8.Text = subj;
                        tue8prof.Text = facultyName;
                        RoomTue8.Text = room;
                        SecProfTue8.Text = "";
                        tue8yr.Text = "";
                        label31.Visible = false;
                        tue8yrRoom.Text = "";
                        tue8secRoom.Text = "";
                        label55.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel13.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel13.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel13.BackColor = Color.White;
                        }
                        classType = "";
                        day = 32;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 32;
                        subjectTue8.Text = "";
                        tue8prof.Text = "";
                        tue8crs.Text = "";
                        RoomTue8.Text = "";
                        SecProfTue8.Text = "";
                        tue8yr.Text = "";
                        label31.Visible = false;
                        tue8yrRoom.Text = "";
                        tue8secRoom.Text = "";
                        label55.Visible = false;
                        panel13.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 2 && day == 32)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed8.Text = subj;
                        wed8prof.Text = facultyName;
                        RoomWed8.Text = room;
                        SecProfWed8.Text = "";
                        wed8yr.Text = "";
                        label34.Visible = false;
                        wed8yrRoom.Text = "";
                        wed8secRoom.Text = "";
                        label61.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel12.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel12.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel12.BackColor = Color.White;
                        }
                        classType = "";
                        day = 42;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 42;
                        subjectWed8.Text = "";
                        wed8prof.Text = "";
                        wed8crs.Text = "";
                        RoomWed8.Text = "";
                        SecProfWed8.Text = "";
                        wed8yr.Text = "";
                        label34.Visible = false;
                        wed8yrRoom.Text = "";
                        wed8secRoom.Text = "";
                        label61.Visible = false;
                        panel12.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 2 && day == 42)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu8.Text = subj;
                        thu8prof.Text = facultyName;
                        RoomThu8.Text = room;
                        SecProfThu8.Text = "";
                        thu8yr.Text = "";
                        label36.Visible = false;
                        thu8yrRoom.Text = "";
                        thu8secRoom.Text = "";
                        label67.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel11.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel11.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel11.BackColor = Color.White;
                        }
                        classType = "";
                        day = 52;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 52;
                        subjectThu8.Text = "";
                        thu8prof.Text = "";
                        thu8crs.Text = "";
                        RoomThu8.Text = "";
                        SecProfThu8.Text = "";
                        thu8yr.Text = "";
                        label36.Visible = false;
                        thu8yrRoom.Text = "";
                        thu8secRoom.Text = "";
                        label67.Visible = false;
                        panel11.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 2 && day == 52)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri8.Text = subj;
                        fri8prof.Text = facultyName;
                        RoomFri8.Text = room;
                        SecProfFri8.Text = "";
                        fri8yr.Text = "";
                        label37.Visible = false;
                        fri8yrRoom.Text = "";
                        fri8secRoom.Text = "";
                        Label73.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel10.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel10.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel10.BackColor = Color.White;
                        }
                        classType = "";
                        day = 62;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 62;
                        subjectFri8.Text = "";
                        fri8prof.Text = "";
                        fri8crs.Text = "";
                        RoomFri8.Text = "";
                        SecProfFri8.Text = "";
                        fri8yr.Text = "";
                        label37.Visible = false;
                        fri8yrRoom.Text = "";
                        fri8secRoom.Text = "";
                        Label73.Visible = false;
                        panel10.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 2 && day == 62)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat8.Text = subj;
                        sat8prof.Text = facultyName;
                        RoomSat8.Text = room;
                        SecProfSat8.Text = "";
                        sat8yr.Text = "";
                        label40.Visible = false;
                        sat8yrRoom.Text = "";
                        sat8secRoom.Text = "";
                        label76.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel4.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel4.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel4.BackColor = Color.White;
                        }
                        classType = "";
                        day = 3;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 3;
                        time += 1;
                        subjectSat8.Text = "";
                        sat8prof.Text = "";
                        sat8crs.Text = "";
                        RoomSat8.Text = "";
                        SecProfSat8.Text = "";
                        sat8yr.Text = "";
                        label40.Visible = false;
                        sat8yrRoom.Text = "";
                        sat8secRoom.Text = "";
                        label76.Visible = false;
                        panel4.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 3 && day == 3)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon9.Text = subj;
                        mon9prof.Text = facultyName;
                        RoomMon9.Text = room;
                        SecProfMon9.Text = "";
                        mon9yr.Text = "";
                        label87.Visible = false;
                        mon9yrRoom.Text = "";
                        mon9secRoom.Text = "";
                        label104.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel26.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel26.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel26.BackColor = Color.White;
                        }
                        classType = "";
                        day = 23;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 23;
                        subjectMon9.Text = "";
                        mon9prof.Text = "";
                        mon9crs.Text = "";
                        RoomMon9.Text = "";
                        SecProfMon9.Text = "";
                        mon9yr.Text = "";
                        label87.Visible = false;
                        mon9yrRoom.Text = "";
                        mon9secRoom.Text = "";
                        label104.Visible = false;
                        panel26.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 3 && day == 23)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue9.Text = subj;
                        tue9prof.Text = facultyName;
                        RoomTue9.Text = room;
                        SecProfTue9.Text = "";
                        tue9yr.Text = "";
                        label81.Visible = false;
                        tue9yrRoom.Text = "";
                        tue9secRoom.Text = "";
                        label100.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel25.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel25.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel25.BackColor = Color.White;
                        }
                        classType = "";
                        day = 33;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 33;
                        subjectTue9.Text = "";
                        tue9prof.Text = "";
                        tue9crs.Text = "";
                        RoomTue9.Text = "";
                        SecProfTue9.Text = "";
                        tue9yr.Text = "";
                        label81.Visible = false;
                        tue9yrRoom.Text = "";
                        tue9secRoom.Text = "";
                        label100.Visible = false;
                        panel25.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 3 && day == 33)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed9.Text = subj;
                        wed9prof.Text = facultyName;
                        RoomWed9.Text = room;
                        SecProfWed9.Text = "";
                        wed9yr.Text = "";
                        label82.Visible = false;
                        wed9yrRoom.Text = "";
                        wed9secRoom.Text = "";
                        label96.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel24.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel24.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel24.BackColor = Color.White;
                        }
                        classType = "";
                        day = 43;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 43;
                        subjectWed9.Text = "";
                        wed9prof.Text = "";
                        wed9crs.Text = "";
                        RoomWed9.Text = "";
                        SecProfWed9.Text = "";
                        wed9yr.Text = "";
                        label82.Visible = false;
                        wed9yrRoom.Text = "";
                        wed9secRoom.Text = "";
                        label96.Visible = false;
                        panel24.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 3 && day == 43)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu9.Text = subj;
                        thu9prof.Text = facultyName;
                        RoomThu9.Text = room;
                        SecProfThu9.Text = "";
                        thu9yr.Text = "";
                        label69.Visible = false;
                        thu9yrRoom.Text = "";
                        thu9secRoom.Text = "";
                        label91.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel23.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel23.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel23.BackColor = Color.White;
                        }
                        classType = "";
                        day = 53;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 53;
                        subjectThu9.Text = "";
                        thu9prof.Text = "";
                        thu9crs.Text = "";
                        RoomThu9.Text = "";
                        SecProfThu9.Text = "";
                        thu9yr.Text = "";
                        label69.Visible = false;
                        thu9yrRoom.Text = "";
                        thu9secRoom.Text = "";
                        label91.Visible = false;
                        panel23.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 3 && day == 53)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri9.Text = subj;
                        fri9prof.Text = facultyName;
                        RoomFri9.Text = room;
                        SecProfFri9.Text = "";
                        fri9yr.Text = "";
                        label51.Visible = false;
                        fri9yrRoom.Text = "";
                        fri9secRoom.Text = "";
                        label88.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel20.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel20.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel20.BackColor = Color.White;
                        }
                        classType = "";
                        day = 63;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 63;
                        subjectFri9.Text = "";
                        fri9prof.Text = "";
                        fri9crs.Text = "";
                        RoomFri9.Text = "";
                        SecProfFri9.Text = "";
                        fri9yr.Text = "";
                        label51.Visible = false;
                        fri9yrRoom.Text = "";
                        fri9secRoom.Text = "";
                        label88.Visible = false;
                        panel20.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 3 && day == 63)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat9.Text = subj;
                        sat9prof.Text = facultyName;
                        RoomSat9.Text = room;
                        SecProfSat9.Text = "";
                        sat9yr.Text = "";
                        label21.Visible = false;
                        sat9yrRoom.Text = "";
                        sat9secRoom.Text = "";
                        label80.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel17.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel17.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel17.BackColor = Color.White;
                        }
                        classType = "";
                        day = 4;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 4;
                        time += 1;
                        subjectSat9.Text = "";
                        sat9prof.Text = "";
                        sat9crs.Text = "";
                        RoomSat9.Text = "";
                        SecProfSat9.Text = "";
                        sat9yr.Text = "";
                        label21.Visible = false;
                        sat9yrRoom.Text = "";
                        sat9secRoom.Text = "";
                        label80.Visible = false;
                        panel17.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 4 && day == 4)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon10.Text = subj;
                        mon10prof.Text = facultyName;
                        RoomMon10.Text = room;
                        SecProfMon10.Text = "";
                        mon10yr.Text = "";
                        label63.Visible = false;
                        mon10yrRoom.Text = "";
                        mon10secRoom.Text = "";
                        label109.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel22.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel22.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel22.BackColor = Color.White;
                        }
                        classType = "";
                        day = 24;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 24;
                        subjectMon10.Text = "";
                        mon10prof.Text = "";
                        mon10crs.Text = "";
                        RoomMon10.Text = "";
                        SecProfMon10.Text = "";
                        mon10yr.Text = "";
                        label63.Visible = false;
                        mon10yrRoom.Text = "";
                        mon10secRoom.Text = "";
                        label109.Visible = false;
                        panel22.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 4 && day == 24)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue10.Text = subj;
                        tue10prof.Text = facultyName;
                        RoomTue10.Text = room;
                        SecProfTue10.Text = "";
                        tue10yr.Text = "";
                        label57.Visible = false;
                        tue10yrRoom.Text = "";
                        tue10secRoom.Text = "";
                        label113.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel21.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel21.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel21.BackColor = Color.White;
                        }
                        classType = "";
                        day = 34;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 34;
                        subjectTue10.Text = "";
                        tue10prof.Text = "";
                        tue10crs.Text = "";
                        RoomTue10.Text = "";
                        SecProfTue10.Text = "";
                        tue10yr.Text = "";
                        label57.Visible = false;
                        tue10yrRoom.Text = "";
                        tue10secRoom.Text = "";
                        label113.Visible = false;
                        panel21.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 4 && day == 34)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed10.Text = subj;
                        wed10prof.Text = facultyName;
                        RoomWed10.Text = room;
                        SecProfWed10.Text = "";
                        wed10yr.Text = "";
                        label45.Visible = false;
                        wed10yrRoom.Text = "";
                        wed10secRoom.Text = "";
                        label118.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel19.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel19.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel19.BackColor = Color.White;
                        }
                        classType = "";
                        day = 44;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 44;
                        subjectWed10.Text = "";
                        wed10prof.Text = "";
                        wed10crs.Text = "";
                        RoomWed10.Text = "";
                        SecProfWed10.Text = "";
                        wed10yr.Text = "";
                        label45.Visible = false;
                        wed10yrRoom.Text = "";
                        wed10secRoom.Text = "";
                        label118.Visible = false;
                        panel19.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 4 && day == 44)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu10.Text = subj;
                        thu10prof.Text = facultyName;
                        RoomThu10.Text = room;
                        SecProfThu10.Text = "";
                        thu10yr.Text = "";
                        label27.Visible = false;
                        thu10yrRoom.Text = "";
                        thu10secRoom.Text = "";
                        label121.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel18.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel18.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel18.BackColor = Color.White;
                        }
                        classType = "";
                        day = 54;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 54;
                        subjectThu10.Text = "";
                        thu10prof.Text = "";
                        thu10crs.Text = "";
                        RoomThu10.Text = "";
                        SecProfThu10.Text = "";
                        thu10yr.Text = "";
                        label27.Visible = false;
                        thu10yrRoom.Text = "";
                        thu10secRoom.Text = "";
                        label121.Visible = false;
                        panel18.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 4 && day == 54)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri10.Text = subj;
                        fri10prof.Text = facultyName;
                        RoomFri10.Text = room;
                        SecProfFri10.Text = "";
                        fri10yr.Text = "";
                        label15.Visible = false;
                        fri10yrRoom.Text = "";
                        fri10secRoom.Text = "";
                        label126.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel16.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel16.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel16.BackColor = Color.White;
                        }
                        classType = "";
                        day = 64;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 64;
                        subjectFri10.Text = "";
                        fri10prof.Text = "";
                        fri10crs.Text = "";
                        RoomFri10.Text = "";
                        SecProfFri10.Text = "";
                        fri10yr.Text = "";
                        label15.Visible = false;
                        fri10yrRoom.Text = "";
                        fri10secRoom.Text = "";
                        label126.Visible = false;
                        panel16.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
              
                }
                if (time == 4 && day == 64)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat10.Text = subj;
                        sat10prof.Text = facultyName;
                        RoomSat10.Text = room;
                        SecProfSat10.Text = "";
                        sat10yr.Text = "";
                        label2.Visible = false;
                        sat10yrRoom.Text = "";
                        sat10secRoom.Text = "";
                        label131.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel15.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel15.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel15.BackColor = Color.White;
                        }
                        classType = "";
                        day = 5;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 5;
                        time += 1;
                        subjectSat10.Text = "";
                        sat10prof.Text = "";
                        sat10crs.Text = "";
                        RoomSat10.Text = "";
                        SecProfSat10.Text = "";
                        sat10yr.Text = "";
                        label2.Visible = false;
                        sat10yrRoom.Text = "";
                        sat10secRoom.Text = "";
                        label131.Visible = false;
                        panel15.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
               
                }
                if (time == 5 && day == 5)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon11.Text = subj;
                        mon11prof.Text = facultyName;
                        RoomMon11.Text = room;
                        SecProfMon11.Text = "";
                        mon11yr.Text = "";
                        label171.Visible = false;
                        mon11yrRoom.Text = "";
                        mon11secRoom.Text = "";
                        label156.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel50.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel50.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel50.BackColor = Color.White;
                        }
                        classType = "";
                        day = 25;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 25;
                        subjectMon11.Text = "";
                        mon11prof.Text = "";
                        mon11crs.Text = "";
                        RoomMon11.Text = "";
                        SecProfMon11.Text = "";
                        mon11yr.Text = "";
                        label171.Visible = false;
                        mon11yrRoom.Text = "";
                        mon11secRoom.Text = "";
                        label156.Visible = false;
                        panel50.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 5 && day == 25)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue11.Text = subj;
                        tue11prof.Text = facultyName;
                        RoomTue11.Text = room;
                        SecProfTue11.Text = "";
                        tue11yr.Text = "";
                        label165.Visible = false;
                        tue11yrRoom.Text = "";
                        tue11secRoom.Text = "";
                        label151.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel49.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel49.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel49.BackColor = Color.White;
                        }
                        classType = "";
                        day = 35;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 35;
                        subjectTue11.Text = "";
                        tue11prof.Text = "";
                        tue11crs.Text = "";
                        RoomTue11.Text = "";
                        SecProfTue11.Text = "";
                        tue11yr.Text = "";
                        label165.Visible = false;
                        tue11yrRoom.Text = "";
                        tue11secRoom.Text = "";
                        label151.Visible = false;
                        panel49.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 5 && day == 35)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed11.Text = subj;
                        wed11prof.Text = facultyName;
                        RoomWed11.Text = room;
                        SecProfWed11.Text = "";
                        wed11yr.Text = "";
                        label159.Visible = false;
                        wed11yrRoom.Text = "";
                        wed11secRoom.Text = "";
                        label148.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel48.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel48.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel48.BackColor = Color.White;
                        }
                        classType = "";
                        day = 45;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 45;
                        subjectWed11.Text = "";
                        wed11prof.Text = "";
                        wed11crs.Text = "";
                        RoomWed11.Text = "";
                        SecProfWed11.Text = "";
                        wed11yr.Text = "";
                        label159.Visible = false;
                        wed11yrRoom.Text = "";
                        wed11secRoom.Text = "";
                        label148.Visible = false;
                        panel48.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 5 && day == 45)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu11.Text = subj;
                        thu11prof.Text = facultyName;
                        RoomThu11.Text = room;
                        SecProfThu11.Text = "";
                        thu11yr.Text = "";
                        label153.Visible = false;
                        thu11yrRoom.Text = "";
                        thu11secRoom.Text = "";
                        label144.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel47.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel47.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel47.BackColor = Color.White;
                        }
                        classType = "";
                        day = 55;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 55;
                        subjectThu11.Text = "";
                        thu11prof.Text = "";
                        thu11crs.Text = "";
                        RoomThu11.Text = "";
                        SecProfThu11.Text = "";
                        thu11yr.Text = "";
                        label153.Visible = false;
                        thu11yrRoom.Text = "";
                        thu11secRoom.Text = "";
                        label144.Visible = false;
                        panel47.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 5 && day == 55)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri11.Text = subj;
                        fri11prof.Text = facultyName;
                        RoomFri11.Text = room;
                        SecProfFri11.Text = "";
                        fri11yr.Text = "";
                        label111.Visible = false;
                        fri11yrRoom.Text = "";
                        fri11secRoom.Text = "";
                        label139.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel40.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel40.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel40.BackColor = Color.White;
                        }
                        classType = "";
                        day = 65;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 65;
                        subjectFri11.Text = "";
                        fri11prof.Text = "";
                        fri11crs.Text = "";
                        RoomFri11.Text = "";
                        SecProfFri11.Text = "";
                        fri11yr.Text = "";
                        label111.Visible = false;
                        fri11yrRoom.Text = "";
                        fri11secRoom.Text = "";
                        label139.Visible = false;
                        panel40.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 5 && day == 65)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat11.Text = subj;
                        sat11prof.Text = facultyName;
                        RoomSat11.Text = room;
                        SecProfSat11.Text = "";
                        sat11yr.Text = "";
                        label65.Visible = false;
                        sat11yrRoom.Text = "";
                        sat11secRoom.Text = "";
                        label134.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel33.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel33.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel33.BackColor = Color.White;
                        }
                        classType = "";
                        day = 6;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 6;
                        time += 1;
                        subjectSat11.Text = "";
                        sat11prof.Text = "";
                        sat11crs.Text = "";
                        RoomSat11.Text = "";
                        SecProfSat11.Text = "";
                        sat11yr.Text = "";
                        label65.Visible = false;
                        sat11yrRoom.Text = "";
                        sat11secRoom.Text = "";
                        label134.Visible = false;
                        panel33.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 6 && day == 6)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon12.Text = subj;
                        mon12prof.Text = facultyName;
                        RoomMon12.Text = room;
                        SecProfMon12.Text = "";
                        mon12yr.Text = "";
                        label135.Visible = false;
                        mon12yrRoom.Text = "";
                        mon12secRoom.Text = "";
                        label161.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel44.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel44.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel44.BackColor = Color.White;
                        }
                        classType = "";
                        day = 26;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 26;
                        subjectMon12.Text = "";
                        mon12prof.Text = "";
                        mon12crs.Text = "";
                        RoomMon12.Text = "";
                        SecProfMon12.Text = "";
                        mon12yr.Text = "";
                        label135.Visible = false;
                        mon12yrRoom.Text = "";
                        mon12secRoom.Text = "";
                        label161.Visible = false;
                        panel44.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
              
                }
                if (time == 6 && day == 26)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue12.Text = subj;
                        tue12prof.Text = facultyName;
                        RoomTue12.Text = room;
                        SecProfTue12.Text = "";
                        tue12yr.Text = "";
                        label123.Visible = false;
                        tue12yrRoom.Text = "";
                        tue12secRoom.Text = "";
                        label164.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel42.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel42.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel42.BackColor = Color.White;
                        }
                        classType = "";
                        day = 36;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 36;
                        subjectTue12.Text = "";
                        tue12prof.Text = "";
                        tue12crs.Text = "";
                        RoomTue12.Text = "";
                        SecProfTue12.Text = "";
                        tue12yr.Text = "";
                        label123.Visible = false;
                        tue12yrRoom.Text = "";
                        tue12secRoom.Text = "";
                        label164.Visible = false;
                        panel42.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 6 && day == 36)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed12.Text = subj;
                        wed12prof.Text = facultyName;
                        RoomWed12.Text = room;
                        SecProfWed12.Text = "";
                        wed12yr.Text = "";
                        label99.Visible = false;
                        wed12yrRoom.Text = "";
                        wed12secRoom.Text = "";
                        label169.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel38.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel38.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel38.BackColor = Color.White;
                        }
                        classType = "";
                        day = 46;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 46;
                        subjectWed12.Text = "";
                        wed12prof.Text = "";
                        wed12crs.Text = "";
                        RoomWed12.Text = "";
                        SecProfWed12.Text = "";
                        wed12yr.Text = "";
                        label99.Visible = false;
                        wed12yrRoom.Text = "";
                        wed12secRoom.Text = "";
                        label169.Visible = false;
                        panel38.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 6 && day == 46)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu12.Text = subj;
                        thu12prof.Text = facultyName;
                        RoomThu12.Text = room;
                        SecProfThu12.Text = "";
                        thu12yr.Text = "";
                        label78.Visible = false;
                        thu12yrRoom.Text = "";
                        thu12secRoom.Text = "";
                        label173.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel35.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel35.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel35.BackColor = Color.White;
                        }
                        classType = "";
                        day = 56;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 56;
                        subjectThu12.Text = "";
                        thu12prof.Text = "";
                        thu12crs.Text = "";
                        RoomThu12.Text = "";
                        SecProfThu12.Text = "";
                        thu12yr.Text = "";
                        label78.Visible = false;
                        thu12yrRoom.Text = "";
                        thu12secRoom.Text = "";
                        label173.Visible = false;
                        panel35.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 6 && day == 56)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri12.Text = subj;
                        fri12prof.Text = facultyName;
                        RoomFri12.Text = room;
                        SecProfFri12.Text = "";
                        fri12yr.Text = "";
                        label50.Visible = false;
                        fri12yrRoom.Text = "";
                        fri12secRoom.Text = "";
                        label177.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel31.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel31.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel31.BackColor = Color.White;
                        }
                        classType = "";
                        day = 66;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 66;
                        subjectFri12.Text = "";
                        fri12prof.Text = "";
                        fri12crs.Text = "";
                        RoomFri12.Text = "";
                        SecProfFri12.Text = "";
                        fri12yr.Text = "";
                        label50.Visible = false;
                        fri12yrRoom.Text = "";
                        fri12secRoom.Text = "";
                        label177.Visible = false;
                        panel31.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 6 && day == 66)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat12.Text = subj;
                        sat12prof.Text = facultyName;
                        RoomSat12.Text = room;
                        SecProfSat12.Text = "";
                        sat12yr.Text = "";
                        label17.Visible = false;
                        sat12yrRoom.Text = "";
                        sat12secRoom.Text = "";
                        label181.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel28.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel28.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel28.BackColor = Color.White;
                        }
                        classType = "";
                        day = 7;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 7;
                        time += 1;
                        subjectSat12.Text = "";
                        sat12prof.Text = "";
                        sat12crs.Text = "";
                        RoomSat12.Text = "";
                        SecProfSat12.Text = "";
                        sat12yr.Text = "";
                        label17.Visible = false;
                        sat12yrRoom.Text = "";
                        sat12secRoom.Text = "";
                        label181.Visible = false;
                        panel28.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 7 && day == 7)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon1.Text = subj;
                        mon1prof.Text = facultyName;
                        RoomMon1.Text = room;
                        SecProfMon1.Text = "";
                        mon1yr.Text = "";
                        label147.Visible = false;
                        mon1yrRoom.Text = "";
                        mon1secRoom.Text = "";
                        label202.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel46.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel46.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel46.BackColor = Color.White;
                        }
                        classType = "";
                        day = 27;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 27;
                        subjectMon1.Text = "";
                        mon1prof.Text = "";
                        mon1crs.Text = "";
                        RoomMon1.Text = "";
                        SecProfMon1.Text = "";
                        mon1yr.Text = "";
                        label147.Visible = false;
                        mon1yrRoom.Text = "";
                        mon1secRoom.Text = "";
                        label202.Visible = false;
                        panel46.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 7 && day == 27)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue1.Text = subj;
                        tue1prof.Text = facultyName;
                        RoomTue1.Text = room;
                        SecProfTue1.Text = "";
                        tue1yr.Text = "";
                        label141.Visible = false;
                        tue1yrRoom.Text = "";
                        tue1secRoom.Text = "";
                        label199.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel45.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel45.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel45.BackColor = Color.White;
                        }
                        classType = "";
                        day = 37;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 37;
                        subjectTue1.Text = "";
                        tue1prof.Text = "";
                        tue1crs.Text = "";
                        RoomTue1.Text = "";
                        SecProfTue1.Text = "";
                        tue1yr.Text = "";
                        label141.Visible = false;
                        tue1yrRoom.Text = "";
                        tue1secRoom.Text = "";
                        label199.Visible = false;
                        panel45.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 7 && day == 37)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed1.Text = subj;
                        wed1prof.Text = facultyName;
                        RoomWed1.Text = room;
                        SecProfWed1.Text = "";
                        wed1yr.Text = "";
                        label129.Visible = false;
                        wed1yrRoom.Text = "";
                        wed1secRoom.Text = "";
                        label195.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel43.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel43.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel43.BackColor = Color.White;
                        }
                        classType = "";
                        day = 47;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 47;
                        subjectWed1.Text = "";
                        wed1prof.Text = "";
                        wed1crs.Text = "";
                        RoomWed1.Text = "";
                        SecProfWed1.Text = "";
                        wed1yr.Text = "";
                        label129.Visible = false;
                        wed1yrRoom.Text = "";
                        wed1secRoom.Text = "";
                        label195.Visible = false;
                        panel43.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 7 && day == 47)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu1.Text = subj;
                        thu1prof.Text = facultyName;
                        RoomThu1.Text = room;
                        SecProfThu1.Text = "";
                        thu1yr.Text = "";
                        label117.Visible = false;
                        thu1yrRoom.Text = "";
                        thu1secRoom.Text = "";
                        label191.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel41.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel41.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel41.BackColor = Color.White;
                        }
                        classType = "";
                        day = 57;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 57;
                        subjectThu1.Text = "";
                        thu1prof.Text = "";
                        thu1crs.Text = "";
                        RoomThu1.Text = "";
                        SecProfThu1.Text = "";
                        thu1yr.Text = "";
                        label117.Visible = false;
                        thu1yrRoom.Text = "";
                        thu1secRoom.Text = "";
                        label191.Visible = false;
                        panel41.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 7 && day == 57)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri1.Text = subj;
                        fri1prof.Text = facultyName;
                        RoomFri1.Text = room;
                        SecProfFri1.Text = "";
                        fri1yr.Text = "";
                        label86.Visible = false;
                        fri1yrRoom.Text = "";
                        fri1secRoom.Text = "";
                        label188.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel36.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel36.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel36.BackColor = Color.White;
                        }
                        classType = "";
                        day = 67;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 67;
                        subjectFri1.Text = "";
                        fri1prof.Text = "";
                        fri1crs.Text = "";
                        RoomFri1.Text = "";
                        SecProfFri1.Text = "";
                        fri1yr.Text = "";
                        label86.Visible = false;
                        fri1yrRoom.Text = "";
                        fri1secRoom.Text = "";
                        label188.Visible = false;
                        panel36.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 7 && day == 67)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat1.Text = subj;
                        sat1prof.Text = facultyName;
                        RoomSat1.Text = room;
                        SecProfSat1.Text = "";
                        sat1yr.Text = "";
                        label43.Visible = false;
                        sat1yrRoom.Text = "";
                        sat1secRoom.Text = "";
                        label184.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel30.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel30.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel30.BackColor = Color.White;
                        }
                        classType = "";
                        day = 8;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 8;
                        time += 1;
                        subjectSat1.Text = "";
                        sat1prof.Text = "";
                        sat1crs.Text = "";
                        RoomSat1.Text = "";
                        SecProfSat1.Text = "";
                        sat1yr.Text = "";
                        label43.Visible = false;
                        sat1yrRoom.Text = "";
                        sat1secRoom.Text = "";
                        label184.Visible = false;
                        panel30.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 8 && day == 8)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon2.Text = subj;
                        mon2prof.Text = facultyName;
                        RoomMon2.Text = room;
                        SecProfMon2.Text = "";
                        mon2yr.Text = "";
                        label105.Visible = false;
                        mon2yrRoom.Text = "";
                        mon2secRoom.Text = "";
                        label206.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel39.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel39.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel39.BackColor = Color.White;
                        }
                        classType = "";
                        day = 28;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 28;
                        subjectMon2.Text = "";
                        mon2prof.Text = "";
                        mon2crs.Text = "";
                        RoomMon2.Text = "";
                        SecProfMon2.Text = "";
                        mon2yr.Text = "";
                        label105.Visible = false;
                        mon2yrRoom.Text = "";
                        mon2secRoom.Text = "";
                        label206.Visible = false;
                        panel39.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                    
                }
                if (time == 8 && day == 28)
                {
                    //  MessageBox.Show(facultyName);
                    if (subj != "")
                    {

                        subjectTue2.Text = subj;
                        tue2prof.Text = facultyName;
                        RoomTue2.Text = room;
                        SecProfTue2.Text = "";
                        tue2yr.Text = "";
                        label93.Visible = false;
                        tue2yrRoom.Text = "";
                        tue2secRoom.Text = "";
                        label209.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel37.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel37.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel37.BackColor = Color.White;
                        }
                        classType = "";
                        day = 38;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 38;
                        subjectTue2.Text = "";
                        tue2prof.Text = "";
                        tue2crs.Text = "";
                        RoomTue2.Text = "";
                        SecProfTue2.Text = "";
                        tue2yr.Text = "";
                        label93.Visible = false;
                        tue2yrRoom.Text = "";
                        tue2secRoom.Text = "";
                        label209.Visible = false;
                        panel37.BackColor = Color.White;
                        schedule();
                    }
                  
                }
                if (time == 8 && day == 38)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed2.Text = subj;
                        wed2prof.Text = facultyName;
                        RoomWed2.Text = room;
                        SecProfWed2.Text = "";
                        wed2yr.Text = "";
                        label72.Visible = false;
                        wed2yrRoom.Text = "";
                        wed2secRoom.Text = "";
                        label213.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel34.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel34.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel34.BackColor = Color.White;
                        }
                        classType = "";
                        day = 48;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 48;
                        subjectWed2.Text = "";
                        wed2prof.Text = "";
                        wed2crs.Text = "";
                        RoomWed2.Text = "";
                        SecProfWed2.Text = "";
                        wed2yr.Text = "";
                        label72.Visible = false;
                        wed2yrRoom.Text = "";
                        wed2secRoom.Text = "";
                        label213.Visible = false;
                        panel34.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 8 && day == 48)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu2.Text = subj;
                        thu2prof.Text = facultyName;
                        RoomThu2.Text = room;
                        SecProfThu2.Text = "";
                        thu2yr.Text = "";
                        label58.Visible = false;
                        thu2yrRoom.Text = "";
                        thu2secRoom.Text = "";
                        label217.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel32.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel32.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel32.BackColor = Color.White;
                        }
                        classType = "";
                        day = 58;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 58;
                        subjectThu2.Text = "";
                        thu2prof.Text = "";
                        thu2crs.Text = "";
                        RoomThu2.Text = "";
                        SecProfThu2.Text = "";
                        thu2yr.Text = "";
                        label58.Visible = false;
                        thu2yrRoom.Text = "";
                        thu2secRoom.Text = "";
                        label217.Visible = false;
                        panel32.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 8 && day == 58)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri2.Text = subj;
                        fri2prof.Text = facultyName;
                        RoomFri2.Text = room;
                        SecProfFri2.Text = "";
                        fri2yr.Text = "";
                        label24.Visible = false;
                        fri2yrRoom.Text = "";
                        fri2secRoom.Text = "";
                        label220.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel29.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel29.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel29.BackColor = Color.White;
                        }
                        classType = "";
                        day = 68;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 68;
                        subjectFri2.Text = "";
                        fri2prof.Text = "";
                        fri2crs.Text = "";
                        RoomFri2.Text = "";
                        SecProfFri2.Text = "";
                        fri2yr.Text = "";
                        label24.Visible = false;
                        fri2yrRoom.Text = "";
                        fri2secRoom.Text = "";
                        label220.Visible = false;
                        panel29.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 8 && day == 68)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat2.Text = subj;
                        sat2prof.Text = facultyName;
                        RoomSat2.Text = room;
                        SecProfSat2.Text = "";
                        sat2yr.Text = "";
                        label8.Visible = false;
                        sat2yrRoom.Text = "";
                        sat2secRoom.Text = "";
                        label224.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel27.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel27.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel27.BackColor = Color.White;
                        }
                        classType = "";
                        day = 9;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 9;
                        time += 1;
                        subjectSat2.Text = "";
                        sat2prof.Text = "";
                        sat2crs.Text = "";
                        RoomSat2.Text = "";
                        SecProfSat2.Text = "";
                        sat2yr.Text = "";
                        label8.Visible = false;
                        sat2yrRoom.Text = "";
                        sat2secRoom.Text = "";
                        label224.Visible = false;
                        panel27.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 9 && day == 9)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon3.Text = subj;
                        mon3prof.Text = facultyName;
                        RoomMon3.Text = room;
                        SecProfMon3.Text = "";
                        mon3yr.Text = "";
                        label264.Visible = false;
                        mon3yrRoom.Text = "";
                        mon3secRoom.Text = "";
                        label245.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel85.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel85.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel85.BackColor = Color.White;
                        }
                        classType = "";
                        day = 29;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 29;
                        subjectMon3.Text = "";
                        mon3prof.Text = "";
                        mon3crs.Text = "";
                        RoomMon3.Text = "";
                        SecProfMon3.Text = "";
                        mon3yr.Text = "";
                        label264.Visible = false;
                        mon3yrRoom.Text = "";
                        mon3secRoom.Text = "";
                        label245.Visible = false;
                        panel85.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 9 && day == 29)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue3.Text = subj;
                        tue3prof.Text = facultyName;
                        RoomTue3.Text = room;
                        SecProfTue3.Text = "";
                        tue3yr.Text = "";
                        label246.Visible = false;
                        tue3yrRoom.Text = "";
                        tue3secRoom.Text = "";
                        label242.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel82.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel82.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel82.BackColor = Color.White;
                        }
                        classType = "";
                        day = 39;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 39;
                        subjectTue3.Text = "";
                        tue3prof.Text = "";
                        tue3crs.Text = "";
                        RoomTue3.Text = "";
                        SecProfTue3.Text = "";
                        tue3yr.Text = "";
                        label246.Visible = false;
                        tue3yrRoom.Text = "";
                        tue3secRoom.Text = "";
                        label242.Visible = false;
                        panel82.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 9 && day == 39)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed3.Text = subj;
                        wed3prof.Text = facultyName;
                        RoomWed3.Text = room;
                        SecProfWed3.Text = "";
                        wed3yr.Text = "";
                        label228.Visible = false;
                        wed3yrRoom.Text = "";
                        wed3secRoom.Text = "";
                        label238.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel79.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel79.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel79.BackColor = Color.White;
                        }
                        classType = "";
                        day = 49;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 49;
                        subjectWed3.Text = "";
                        wed3prof.Text = "";
                        wed3crs.Text = "";
                        RoomWed3.Text = "";
                        SecProfWed3.Text = "";
                        wed3yr.Text = "";
                        label228.Visible = false;
                        wed3yrRoom.Text = "";
                        wed3secRoom.Text = "";
                        label238.Visible = false;
                        panel79.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 9 && day == 49)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu3.Text = subj;
                        thu3prof.Text = facultyName;
                        RoomThu3.Text = room;
                        SecProfThu3.Text = "";
                        thu3yr.Text = "";
                        label210.Visible = false;
                        thu3yrRoom.Text = "";
                        thu3secRoom.Text = "";
                        label235.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel76.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel76.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel76.BackColor = Color.White;
                        }
                        classType = "";
                        day = 59;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 59;
                        subjectThu3.Text = "";
                        thu3prof.Text = "";
                        thu3crs.Text = "";
                        RoomThu3.Text = "";
                        SecProfThu3.Text = "";
                        thu3yr.Text = "";
                        label210.Visible = false;
                        thu3yrRoom.Text = "";
                        thu3secRoom.Text = "";
                        label235.Visible = false;
                        panel76.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 9 && day == 59)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri3.Text = subj;
                        fri3prof.Text = facultyName;
                        RoomFri3.Text = room;
                        SecProfFri3.Text = "";
                        fri3yr.Text = "";
                        label152.Visible = false;
                        fri3yrRoom.Text = "";
                        fri3secRoom.Text = "";
                        label231.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel67.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel67.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel67.BackColor = Color.White;
                        }
                        classType = "";
                        day = 69;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 69;
                        subjectFri3.Text = "";
                        fri3prof.Text = "";
                        fri3crs.Text = "";
                        RoomFri3.Text = "";
                        SecProfFri3.Text = "";
                        fri3yr.Text = "";
                        label152.Visible = false;
                        fri3yrRoom.Text = "";
                        fri3secRoom.Text = "";
                        label231.Visible = false;
                        classType = "";
                        panel67.BackColor = Color.White;
                        schedule();
                    }
                  
                }
                if (time == 9 && day == 69)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat3.Text = subj;
                        sat3prof.Text = facultyName;
                        RoomSat3.Text = room;
                        SecProfSat3.Text = "";
                        sat3yr.Text = "";
                        label84.Visible = false;
                        sat3yrRoom.Text = "";
                        sat3secRoom.Text = "";
                        label227.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel58.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel58.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel58.BackColor = Color.White;
                        }
                        classType = "";
                        day = 10;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 10;
                        time += 1;
                        subjectSat3.Text = "";
                        sat3prof.Text = "";
                        sat3crs.Text = "";
                        RoomSat3.Text = "";
                        SecProfSat3.Text = "";
                        sat3yr.Text = "";
                        label84.Visible = false;
                        sat3yrRoom.Text = "";
                        sat3secRoom.Text = "";
                        label227.Visible = false;
                        panel58.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 10 && day == 10)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon4.Text = subj;
                        mon4prof.Text = facultyName;
                        RoomMon4.Text = room;
                        SecProfMon4.Text = "";
                        mon4yr.Text = "";
                        label192.Visible = false;
                        mon4yrRoom.Text = "";
                        mon4secRoom.Text = "";
                        label249.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel73.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel73.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel73.BackColor = Color.White;
                        }
                        classType = "";
                        day = 210;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 210;
                        subjectMon4.Text = "";
                        mon4prof.Text = "";
                        mon4crs.Text = "";
                        RoomMon4.Text = "";
                        SecProfMon4.Text = "";
                        mon4yr.Text = "";
                        label192.Visible = false;
                        mon4yrRoom.Text = "";
                        mon4secRoom.Text = "";
                        label249.Visible = false;
                        panel73.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 10 && day == 210)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue4.Text = subj;
                        tue4prof.Text = facultyName;
                        RoomTue4.Text = room;
                        SecProfTue4.Text = "";
                        tue4yr.Text = "";
                        label174.Visible = false;
                        tue4yrRoom.Text = "";
                        tue4secRoom.Text = "";
                        label253.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel70.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel70.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel70.BackColor = Color.White;
                        }
                        classType = "";
                        day = 310;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 310;
                        subjectTue4.Text = "";
                        tue4prof.Text = "";
                        tue4crs.Text = "";
                        RoomTue4.Text = "";
                        SecProfTue4.Text = "";
                        tue4yr.Text = "";
                        label174.Visible = false;
                        tue4yrRoom.Text = "";
                        tue4secRoom.Text = "";
                        label253.Visible = false;
                        panel70.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 10 && day == 310)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed4.Text = subj;
                        wed4prof.Text = facultyName;
                        RoomWed4.Text = room;
                        SecProfWed4.Text = "";
                        wed4yr.Text = "";
                        label130.Visible = false;
                        wed4yrRoom.Text = "";
                        wed4secRoom.Text = "";
                        label256.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel64.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel64.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel64.BackColor = Color.White;
                        }
                        classType = "";
                        day = 410;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 410;
                        subjectWed4.Text = "";
                        wed4prof.Text = "";
                        wed4crs.Text = "";
                        RoomWed4.Text = "";
                        SecProfWed4.Text = "";
                        wed4yr.Text = "";
                        label130.Visible = false;
                        wed4yrRoom.Text = "";
                        wed4secRoom.Text = "";
                        label256.Visible = false;
                        panel64.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 10 && day == 410)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu4.Text = subj;
                        thu4prof.Text = facultyName;
                        RoomThu4.Text = room;
                        SecProfThu4.Text = "";
                        thu4yr.Text = "";
                        label108.Visible = false;
                        thu4yrRoom.Text = "";
                        thu4secRoom.Text = "";
                        label260.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel61.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel61.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel61.BackColor = Color.White;
                        }
                        classType = "";
                        day = 510;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 510;
                        subjectThu4.Text = "";
                        thu4prof.Text = "";
                        thu4crs.Text = "";
                        RoomThu4.Text = "";
                        SecProfThu4.Text = "";
                        thu4yr.Text = "";
                        label108.Visible = false;
                        thu4yrRoom.Text = "";
                        thu4secRoom.Text = "";
                        label260.Visible = false;
                        panel61.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 10 && day == 510)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri4.Text = subj;
                        fri4prof.Text = facultyName;
                        RoomFri4.Text = room;
                        SecProfFri4.Text = "";
                        fri4yr.Text = "";
                        label56.Visible = false;
                        fri4yrRoom.Text = "";
                        fri4secRoom.Text = "";
                        label263.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel55.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel55.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel55.BackColor = Color.White;
                        }
                        classType = "";
                        day = 610;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 610;
                        subjectFri4.Text = "";
                        fri4prof.Text = "";
                        fri4crs.Text = "";
                        RoomFri4.Text = "";
                        SecProfFri4.Text = "";
                        fri4yr.Text = "";
                        label56.Visible = false;
                        fri4yrRoom.Text = "";
                        fri4secRoom.Text = "";
                        label263.Visible = false;
                        panel55.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 10 && day == 610)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat4.Text = subj;
                        sat4prof.Text = facultyName;
                        RoomSat4.Text = room;
                        SecProfSat4.Text = "";
                        sat4yr.Text = "";
                        label19.Visible = false;
                        sat4yrRoom.Text = "";
                        sat4secRoom.Text = "";
                        label267.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel52.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel52.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel52.BackColor = Color.White;
                        }
                        classType = "";
                        day = 11;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 11;
                        time += 1;
                        subjectSat4.Text = "";
                        sat4prof.Text = "";
                        sat4crs.Text = "";
                        RoomSat4.Text = "";
                        SecProfSat4.Text = "";
                        sat4yr.Text = "";
                        label19.Visible = false;
                        sat4yrRoom.Text = "";
                        sat4secRoom.Text = "";
                        label267.Visible = false;
                        panel52.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 11 && day == 11)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon5.Text = subj;
                        mon5prof.Text = facultyName;
                        RoomMon5.Text = room;
                        SecProfMon5.Text = "";
                        mon5yr.Text = "";
                        label270.Visible = false;
                        mon5yrRoom.Text = "";
                        mon5secRoom.Text = "";
                        label286.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel86.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel86.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel86.BackColor = Color.White;
                        }
                        classType = "";
                        day = 211;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 211;
                        subjectMon5.Text = "";
                        mon5prof.Text = "";
                        mon5crs.Text = "";
                        RoomMon5.Text = "";
                        SecProfMon5.Text = "";
                        mon5yr.Text = "";
                        label270.Visible = false;
                        mon5yrRoom.Text = "";
                        mon5secRoom.Text = "";
                        label286.Visible = false;
                        panel86.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 11 && day == 211)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue5.Text = subj;
                        tue5prof.Text = facultyName;
                        RoomTue5.Text = room;
                        SecProfTue5.Text = "";
                        tue5yr.Text = "";
                        label258.Visible = false;
                        tue5yrRoom.Text = "";
                        tue5secRoom.Text = "";
                        label283.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel84.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel84.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel84.BackColor = Color.White;
                        }
                        classType = "";
                        day = 311;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 311;
                        subjectTue5.Text = "";
                        tue5prof.Text = "";
                        tue5crs.Text = "";
                        RoomTue5.Text = "";
                        SecProfTue5.Text = "";
                        tue5yr.Text = "";
                        label258.Visible = false;
                        tue5yrRoom.Text = "";
                        tue5secRoom.Text = "";
                        label283.Visible = false;
                        panel84.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 11 && day == 311)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed5.Text = subj;
                        wed5prof.Text = facultyName;
                        RoomWed5.Text = room;
                        SecProfWed5.Text = "";
                        wed5yr.Text = "";
                        label252.Visible = false;
                        wed5yrRoom.Text = "";
                        wed5secRoom.Text = "";
                        label280.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel83.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel83.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel83.BackColor = Color.White;
                        }
                        classType = "";
                        day = 411;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 411;
                        subjectWed5.Text = "";
                        wed5prof.Text = "";
                        wed5crs.Text = "";
                        RoomWed5.Text = "";
                        SecProfWed5.Text = "";
                        wed5yr.Text = "";
                        label252.Visible = false;
                        wed5yrRoom.Text = "";
                        wed5secRoom.Text = "";
                        label280.Visible = false;
                        panel83.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 11 && day == 411)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu5.Text = subj;
                        thu5prof.Text = facultyName;
                        RoomThu5.Text = room;
                        SecProfThu5.Text = "";
                        thu5yr.Text = "";
                        label240.Visible = false;
                        thu5yrRoom.Text = "";
                        thu5secRoom.Text = "";
                        label277.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel81.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel81.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel81.BackColor = Color.White;
                        }
                        classType = "";
                        day = 511;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 511;
                        subjectThu5.Text = "";
                        thu5prof.Text = "";
                        thu5crs.Text = "";
                        RoomThu5.Text = "";
                        SecProfThu5.Text = "";
                        thu5yr.Text = "";
                        label240.Visible = false;
                        thu5yrRoom.Text = "";
                        thu5secRoom.Text = "";
                        label277.Visible = false;
                        panel81.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 11 && day == 511)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri5.Text = subj;
                        fri5prof.Text = facultyName;
                        RoomFri5.Text = room;
                        SecProfFri5.Text = "";
                        fri5yr.Text = "";
                        label180.Visible = false;
                        fri5yrRoom.Text = "";
                        fri5secRoom.Text = "";
                        label274.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel71.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel71.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel71.BackColor = Color.White;
                        }
                        classType = "";
                        day = 611;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 611;
                        subjectFri5.Text = "";
                        fri5prof.Text = "";
                        fri5crs.Text = "";
                        RoomFri5.Text = "";
                        SecProfFri5.Text = "";
                        fri5yr.Text = "";
                        label180.Visible = false;
                        fri5yrRoom.Text = "";
                        fri5secRoom.Text = "";
                        label274.Visible = false;
                        panel71.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 11 && day == 611)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat5.Text = subj;
                        sat5prof.Text = facultyName;
                        RoomSat5.Text = room;
                        SecProfSat5.Text = "";
                        sat5yr.Text = "";
                        label101.Visible = false;
                        sat5yrRoom.Text = "";
                        sat5secRoom.Text = "";
                        label271.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel60.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel60.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel60.BackColor = Color.White;
                        }
                        classType = "";
                        day = 12;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 12;
                        time += 1;
                        subjectSat5.Text = "";
                        sat5prof.Text = "";
                        sat5crs.Text = "";
                        RoomSat5.Text = "";
                        SecProfSat5.Text = "";
                        sat5yr.Text = "";
                        label101.Visible = false;
                        sat5yrRoom.Text = "";
                        sat5secRoom.Text = "";
                        label271.Visible = false;
                        panel60.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 12 && day == 12)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon6.Text = subj;
                        mon6prof.Text = facultyName;
                        RoomMon6.Text = room;
                        SecProfMon6.Text = "";
                        mon6yr.Text = "";
                        label216.Visible = false;
                        mon6yrRoom.Text = "";
                        mon6secRoom.Text = "";
                        label289.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel77.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel77.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel77.BackColor = Color.White;
                        }
                        classType = "";
                        day = 212;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 212;
                        subjectMon6.Text = "";
                        mon6prof.Text = "";
                        mon6crs.Text = "";
                        RoomMon6.Text = "";
                        SecProfMon6.Text = "";
                        mon6yr.Text = "";
                        label216.Visible = false;
                        mon6yrRoom.Text = "";
                        mon6secRoom.Text = "";
                        label289.Visible = false;
                        panel77.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 12 && day == 212)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue6.Text = subj;
                        tue6prof.Text = facultyName;
                        RoomTue6.Text = room;
                        SecProfTue6.Text = "";
                        tue6yr.Text = "";
                        label198.Visible = false;
                        tue6yrRoom.Text = "";
                        tue6secRoom.Text = "";
                        label292.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel74.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel74.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel74.BackColor = Color.White;
                        }
                        classType = "";
                        day = 312;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 312;
                        subjectTue6.Text = "";
                        tue6prof.Text = "";
                        tue6crs.Text = "";
                        RoomTue6.Text = "";
                        SecProfTue6.Text = "";
                        tue6yr.Text = "";
                        label198.Visible = false;
                        tue6yrRoom.Text = "";
                        tue6secRoom.Text = "";
                        label292.Visible = false;
                        panel74.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 12 && day == 312)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed6.Text = subj;
                        wed6prof.Text = facultyName;
                        RoomWed6.Text = room;
                        SecProfWed6.Text = "";
                        wed6yr.Text = "";
                        label160.Visible = false;
                        wed6yrRoom.Text = "";
                        wed6secRoom.Text = "";
                        label295.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel68.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel68.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel68.BackColor = Color.White;
                        }
                        classType = "";
                        day = 412;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 412;
                        subjectWed6.Text = "";
                        wed6prof.Text = "";
                        wed6crs.Text = "";
                        RoomWed6.Text = "";
                        SecProfWed6.Text = "";
                        wed6yr.Text = "";
                        label160.Visible = false;
                        wed6yrRoom.Text = "";
                        wed6secRoom.Text = "";
                        label295.Visible = false;
                        panel68.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 12 && day == 412)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu6.Text = subj;
                        thu6prof.Text = facultyName;
                        RoomThu6.Text = room;
                        SecProfThu6.Text = "";
                        thu6yr.Text = "";
                        label122.Visible = false;
                        thu6yrRoom.Text = "";
                        thu6secRoom.Text = "";
                        label298.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel63.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel63.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel63.BackColor = Color.White;
                        }
                        classType = "";
                        day = 512;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 512;
                        subjectThu6.Text = "";
                        thu6prof.Text = "";
                        thu6crs.Text = "";
                        RoomThu6.Text = "";
                        SecProfThu6.Text = "";
                        thu6yr.Text = "";
                        label122.Visible = false;
                        thu6yrRoom.Text = "";
                        thu6secRoom.Text = "";
                        label298.Visible = false;
                        panel63.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 12 && day == 512)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri6.Text = subj;
                        fri6prof.Text = facultyName;
                        RoomFri6.Text = room;
                        SecProfFri6.Text = "";
                        fri6yr.Text = "";
                        label74.Visible = false;
                        fri6yrRoom.Text = "";
                        fri6secRoom.Text = "";
                        label301.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel57.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel57.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel57.BackColor = Color.White;
                        }
                        classType = "";
                        day = 612;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 612;
                        subjectFri6.Text = "";
                        fri6prof.Text = "";
                        fri6crs.Text = "";
                        RoomFri6.Text = "";
                        SecProfFri6.Text = "";
                        fri6yr.Text = "";
                        label74.Visible = false;
                        fri6yrRoom.Text = "";
                        fri6secRoom.Text = "";
                        label301.Visible = false;
                        panel57.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 12 && day == 612)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat6.Text = subj;
                        sat6prof.Text = facultyName;
                        RoomSat6.Text = room;
                        SecProfSat6.Text = "";
                        sat6yr.Text = "";
                        label28.Visible = false;
                        sat6yrRoom.Text = "";
                        sat6secRoom.Text = "";
                        label304.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel53.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel53.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel53.BackColor = Color.White;
                        }
                        classType = "";
                        day = 13;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 13;
                        time += 1;
                        subjectSat6.Text = "";
                        sat6prof.Text = "";
                        sat6crs.Text = "";
                        RoomSat6.Text = "";
                        SecProfSat6.Text = "";
                        sat6yr.Text = "";
                        label28.Visible = false;
                        sat6yrRoom.Text = "";
                        sat6secRoom.Text = "";
                        label304.Visible = false;
                        panel53.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 13 && day == 13)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon7pm.Text = subj;
                        mon7pmprof.Text = facultyName;
                        RoomMon7pm.Text = room;
                        SecProfMon7pm.Text = "";
                        mon7pmyr.Text = "";
                        label234.Visible = false;
                        mon7pmyrRoom.Text = "";
                        mon7pmsecRoom.Text = "";
                        label322.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel80.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel80.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel80.BackColor = Color.White;
                        }
                        classType = "";
                        day = 213;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 213;
                        subjectMon7pm.Text = "";
                        mon7pmprof.Text = "";
                        mon7pmcrs.Text = "";
                        RoomMon7pm.Text = "";
                        SecProfMon7pm.Text = "";
                        mon7pmyr.Text = "";
                        label234.Visible = false;
                        mon7pmyrRoom.Text = "";
                        mon7pmsecRoom.Text = "";
                        label322.Visible = false;
                        panel80.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 13 && day == 213)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue7pm.Text = subj;
                        tue7pmprof.Text = facultyName;
                        RoomTue7pm.Text = room;
                        SecProfTue7pm.Text = "";
                        tue7pmyr.Text = "";
                        label222.Visible = false;
                        tue7pmyrRoom.Text = "";
                        tue7pmsecRoom.Text = "";
                        label319.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel78.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel78.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel78.BackColor = Color.White;
                        }
                        classType = "";
                        day = 313;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 313;
                        subjectTue7pm.Text = "";
                        tue7pmprof.Text = "";
                        tue7pmcrs.Text = "";
                        RoomTue7pm.Text = "";
                        SecProfTue7pm.Text = "";
                        tue7pmyr.Text = "";
                        label222.Visible = false;
                        tue7pmyrRoom.Text = "";
                        tue7pmsecRoom.Text = "";
                        label319.Visible = false;
                        panel78.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 13 && day == 313)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed7pm.Text = subj;
                        wed7pmprof.Text = facultyName;
                        RoomWed7pm.Text = room;
                        SecProfWed7pm.Text = "";
                        wed7pmyr.Text = "";
                        label204.Visible = false;
                        wed7pmyrRoom.Text = "";
                        wed7pmsecRoom.Text = "";
                        label316.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel75.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel75.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel75.BackColor = Color.White;
                        }
                        classType = "";
                        day = 413;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 413;
                        subjectWed7pm.Text = "";
                        wed7pmprof.Text = "";
                        wed7pmcrs.Text = "";
                        RoomWed7pm.Text = "";
                        SecProfWed7pm.Text = "";
                        wed7pmyr.Text = "";
                        label204.Visible = false;
                        wed7pmyrRoom.Text = "";
                        wed7pmsecRoom.Text = "";
                        label316.Visible = false;
                        panel75.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
               
                }
                if (time == 13 && day == 413)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu7pm.Text = subj;
                        thu7pmprof.Text = facultyName;
                        RoomThu7pm.Text = room;
                        SecProfThu7pm.Text = "";
                        thu7pmyr.Text = "";
                        label186.Visible = false;
                        thu7pmyrRoom.Text = "";
                        thu7pmsecRoom.Text = "";
                        label313.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel72.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel72.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel72.BackColor = Color.White;
                        }
                        classType = "";
                        day = 513;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 513;
                        subjectThu7pm.Text = "";
                        thu7pmprof.Text = "";
                        thu7pmcrs.Text = "";
                        RoomThu7pm.Text = "";
                        SecProfThu7pm.Text = "";
                        thu7pmyr.Text = "";
                        label186.Visible = false;
                        thu7pmyrRoom.Text = "";
                        thu7pmsecRoom.Text = "";
                        label313.Visible = false;
                        panel72.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 13 && day == 513)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri7pm.Text = subj;
                        fri7pmprof.Text = facultyName;
                        RoomFri7pm.Text = room;
                        SecProfFri7pm.Text = "";
                        fri7pmyr.Text = "";
                        label137.Visible = false;
                        fri7pmyrRoom.Text = "";
                        fri7pmsecRoom.Text = "";
                        label310.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel65.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel65.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel65.BackColor = Color.White;
                        }
                        classType = "";
                        day = 613;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 613;
                        subjectFri7pm.Text = "";
                        fri7pmprof.Text = "";
                        fri7pmcrs.Text = "";
                        RoomFri7pm.Text = "";
                        SecProfFri7pm.Text = "";
                        fri7pmyr.Text = "";
                        label137.Visible = false;
                        fri7pmyrRoom.Text = "";
                        fri7pmsecRoom.Text = "";
                        label310.Visible = false;
                        panel65.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 13 && day == 613)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat7pm.Text = subj;
                        sat7pmprof.Text = facultyName;
                        RoomSat7pm.Text = room;
                        SecProfSat7pm.Text = "";
                        sat7pmyr.Text = "";
                        label66.Visible = false;
                        sat7pmyrRoom.Text = "";
                        sat7pmsecRoom.Text = "";
                        label307.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel56.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel56.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel56.BackColor = Color.White;
                        }
                        classType = "";
                        day = 14;
                        time += 1;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 14;
                        time += 1;
                        subjectSat7pm.Text = "";
                        sat7pmprof.Text = "";
                        sat7pmcrs.Text = "";
                        RoomSat7pm.Text = "";
                        SecProfSat7pm.Text = "";
                        sat7pmyr.Text = "";
                        label66.Visible = false;
                        sat7pmyrRoom.Text = "";
                        sat7pmsecRoom.Text = "";
                        label307.Visible = false;
                        panel56.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 14 && day == 14)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectMon8pm.Text = subj;
                        mon8pmprof.Text = facultyName;
                        RoomMon8pm.Text = room;
                        SecProfMon8pm.Text = "";
                        mon8pmyr.Text = "";
                        label167.Visible = false;
                        mon8pmyrRoom.Text = "";
                        mon8pmsecRoom.Text = "";
                        label325.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel69.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel69.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel69.BackColor = Color.White;
                        }
                        classType = "";
                        day = 214;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 214;
                        subjectMon8pm.Text = "";
                        mon8pmprof.Text = "";
                        mon8pmcrs.Text = "";
                        RoomMon8pm.Text = "";
                        SecProfMon8pm.Text = "";
                        mon8pmyr.Text = "";
                        label167.Visible = false;
                        mon8pmyrRoom.Text = "";
                        mon8pmsecRoom.Text = "";
                        label325.Visible = false;
                        panel69.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                   
                }
                if (time == 14 && day == 214)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectTue8pm.Text = subj;
                        tue8pmprof.Text = facultyName;
                        RoomTue8pm.Text = room;
                        SecProfTue8pm.Text = "";
                        tue8pmyr.Text = "";
                        label145.Visible = false;
                        tue8pmyrRoom.Text = "";
                        tue8pmsecRoom.Text = "";
                        label328.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel66.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel66.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel66.BackColor = Color.White;
                        }
                        classType = "";
                        day = 314;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 314;
                        subjectTue8pm.Text = "";
                        tue8pmprof.Text = "";
                        tue8pmcrs.Text = "";
                        RoomTue8pm.Text = "";
                        SecProfTue8pm.Text = "";
                        tue8pmyr.Text = "";
                        label145.Visible = false;
                        tue8pmyrRoom.Text = "";
                        tue8pmsecRoom.Text = "";
                        label328.Visible = false;
                        panel66.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
              
                }
                if (time == 14 && day == 314)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectWed8pm.Text = subj;
                        wed8pmprof.Text = facultyName;
                        RoomWed8pm.Text = room;
                        SecProfWed8pm.Text = "";
                        wed8pmyr.Text = "";
                        label115.Visible = false;
                        wed8pmyrRoom.Text = "";
                        wed8pmsecRoom.Text = "";
                        label331.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel62.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel62.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel62.BackColor = Color.White;
                        }
                        classType = "";
                        day = 414;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 414;
                        subjectWed8pm.Text = "";
                        wed8pmprof.Text = "";
                        wed8pmcrs.Text = "";
                        RoomWed8pm.Text = "";
                        SecProfWed8pm.Text = "";
                        wed8pmyr.Text = "";
                        label115.Visible = false;
                        wed8pmyrRoom.Text = "";
                        wed8pmsecRoom.Text = "";
                        label331.Visible = false;
                        panel62.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
                if (time == 14 && day == 414)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectThu8pm.Text = subj;
                        thu8pmprof.Text = facultyName;
                        RoomThu8pm.Text = room;
                        SecProfThu8pm.Text = "";
                        thu8pmyr.Text = "";
                        label92.Visible = false;
                        thu8pmyrRoom.Text = "";
                        thu8pmsecRoom.Text = "";
                        label334.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel59.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel59.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel59.BackColor = Color.White;
                        }
                        classType = "";
                        day = 514;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 514;
                        subjectThu8pm.Text = "";
                        thu8pmprof.Text = "";
                        thu8pmcrs.Text = "";
                        RoomThu8pm.Text = "";
                        SecProfThu8pm.Text = "";
                        thu8pmyr.Text = "";
                        label92.Visible = false;
                        thu8pmyrRoom.Text = "";
                        thu8pmsecRoom.Text = "";
                        label334.Visible = false;
                        panel59.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                
                }
                if (time == 14 && day == 514)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectFri8pm.Text = subj;
                        fri8pmprof.Text = facultyName;
                        RoomFri8pm.Text = room;
                        SecProfFri8pm.Text = "";
                        fri8pmyr.Text = "";
                        label48.Visible = false;
                        fri8pmyrRoom.Text = "";
                        fri8pmsecRoom.Text = "";
                        label337.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel54.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel54.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel54.BackColor = Color.White;
                        }
                        classType = "";
                        day = 614;
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        day = 614;
                        subjectFri8pm.Text = "";
                        fri8pmprof.Text = "";
                        fri8pmcrs.Text = "";
                        RoomFri8pm.Text = "";
                        SecProfFri8pm.Text = "";
                        fri8pmyr.Text = "";
                        label48.Visible = false;
                        fri8pmyrRoom.Text = "";
                        fri8pmsecRoom.Text = "";
                        label337.Visible = false;
                        panel54.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                 
                }
                if (time == 14 && day == 614)
                {
                    //  MessageBox.Show(facultyName);

                    if (subj != "")
                    {

                        subjectSat8pm.Text = subj;
                        sat8pmprof.Text = facultyName;
                        RoomSat8pm.Text = room;
                        SecProfSat8pm.Text = "";
                        sat8pmyr.Text = "";
                        label9.Visible = false;
                        sat8pmyrRoom.Text = "";
                        sat8pmsecRoom.Text = "";
                        label340.Visible = false;
                        if (classType == "Lecture")
                        {
                            panel51.BackColor = Color.Green;
                        }
                        else if (classType == "Laboratory")
                        {
                            panel51.BackColor = Color.Yellow;
                        }
                        else if (classType == "")
                        {
                            panel51.BackColor = Color.White;
                        }
                        classType = "";
                        subj = "";
                        room = "";
                        sec = "";
                        year = "";
                        course = "";
                        facultyName = "";
                        facultyCode = "";
                        schedule();
                    }
                    else
                    {
                        subjectSat8pm.Text = "";
                        sat8pmprof.Text = "";
                        sat8pmcrs.Text = "";
                        RoomSat8pm.Text = "";
                        SecProfSat8pm.Text = "";
                        sat8pmyr.Text = "";
                        label9.Visible = false;
                        sat8pmyrRoom.Text = "";
                        sat8pmsecRoom.Text = "";
                        label340.Visible = false;
                        panel51.BackColor = Color.White;
                        classType = "";
                        schedule();
                    }
                  
                }
            }
            else if (sched.comboBox1.Text == "Room")
            {
                subj = "";
                room = "";
                sec = "";
                year = "";
                course = "";
                facultyName = "";
                facultyCode = "";
                classType = "";
                day = 1;
                time = 1;
                schedule();
                if (time == 1 && day == 1)
                {
                    subjectMon7.Text = subj;
                    mon7secRoom.Text = sec;
                    RoomMon7.Text = course;
                    mon7crs.Text = "";
                    mon7yrRoom.Text = year;
                    mon7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label10.Visible = false;
                    }
                    else
                    {
                        label10.Visible = true;
                    }
                    mon7yr.Text = "";
                    SecProfMon7.Text = "";
                    label29.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel3.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel3.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel3.BackColor = Color.White;
                    }
                    classType = "";
                    day = 21;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 21)
                {
                    subjectTue7.Text = subj;
                    tue7secRoom.Text = sec;
                    RoomTue7.Text = course;
                    tue7crs.Text = "";
                    tue7yrRoom.Text = year;
                    tue7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label13.Visible = false;
                    }
                    else
                    {
                        label13.Visible = true;
                    }
                    tue7yr.Text = "";
                    SecProfTue7.Text = "";
                    label32.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel5.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel5.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel5.BackColor = Color.White;
                    }
                    classType = "";
                    day = 31;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 31)
                {
                    subjectWed7.Text = subj;
                    wed7secRoom.Text = sec;
                    RoomWed7.Text = course;
                    wed7crs.Text = "";
                    wed7yrRoom.Text = year;
                    wed7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label18.Visible = false;
                    }
                    else
                    {
                        label18.Visible = true;
                    }
                    Wed7yr.Text = "";
                    SecProfWed7.Text = "";
                    label33.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel6.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel6.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel6.BackColor = Color.White;
                    }
                    classType = "";
                    day = 41;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 41)
                {
                    subjectThu7.Text = subj;
                    thu7secRoom.Text = sec;
                    RoomThu7.Text = course;
                    Thu7crs.Text = "";
                    thu7yrRoom.Text = year;
                    thu7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label23.Visible = false;
                    }
                    else
                    {
                        label23.Visible = true;
                    }
                    Thu7yr.Text = "";
                    SecProfThu7.Text = "";
                    label35.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel7.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel7.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel7.BackColor = Color.White;
                    }
                    classType = "";
                    day = 51;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 51)
                {
                    subjectFri7.Text = subj;
                    fri7secRoom.Text = sec;
                    RoomFri7.Text = course;
                    Fri7crs.Text = "";
                    fri7yrRoom.Text = year;
                    fri7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label41.Visible = false;
                    }
                    else
                    {
                        label41.Visible = true;
                    }
                    Fri7yr.Text = "";
                    SecProfFri7.Text = "";
                    label38.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel8.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel8.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel8.BackColor = Color.White;
                    }
                    classType = "";
                    day = 61;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 1 && day == 61)
                {
                    subjectSat7.Text = subj;
                    sat7secRoom.Text = sec;
                    RoomSat7.Text = course;
                    Sat7crs.Text = "";
                    sat7yrRoom.Text = year;
                    sat7prof.Text = facultyName;
                    if (subj == "")
                    {
                        label46.Visible = false;
                    }
                    else
                    {
                        label46.Visible = true;
                    }
                    Sat7yr.Text = "";
                    SecProfSat7.Text = "";
                    label39.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel9.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel9.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel9.BackColor = Color.White;
                    }
                    classType = "";
                    day = 2;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 2)
                {
                    subjectMon8.Text = subj;
                    mon8secRoom.Text = sec;
                    RoomMon8.Text = course;
                    mon8crs.Text = "";
                    mon8yrRoom.Text = year;
                    mon8prof.Text = facultyName;
                    if (subj == "")
                    {
                        label52.Visible = false;
                    }
                    else
                    {
                        label52.Visible = true;
                    }
                    mon8yr.Text = "";
                    SecProfMon8.Text = "";
                    label30.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel14.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel14.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel14.BackColor = Color.White;
                    }
                    classType = "";
                    day = 22;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 22)
                {
                    subjectTue8.Text = subj;
                    tue8secRoom.Text = sec;
                    RoomTue8.Text = course;
                    tue8crs.Text = "";
                    tue8yrRoom.Text = year;
                    tue8prof.Text = facultyName;
                    if (subj == "")
                    {
                        label55.Visible = false;
                    }
                    else
                    {
                        label55.Visible = true;
                    }
                    tue8yr.Text = "";
                    SecProfTue8.Text = "";
                    label31.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel13.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel13.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel13.BackColor = Color.White;
                    }
                    classType = "";
                    day = 32;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 32)
                {
                    subjectWed8.Text = subj;
                    wed8secRoom.Text = sec;
                    RoomWed8.Text = course;
                    wed8crs.Text = "";
                    wed8yrRoom.Text = year;
                    wed8prof.Text = facultyName;
                    if (subj == "")
                    {
                        label61.Visible = false;
                    }
                    else
                    {
                        label61.Visible = true;
                    }
                    wed8yr.Text = "";
                    SecProfWed8.Text = "";
                    label34.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel12.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel12.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel12.BackColor = Color.White;
                    }
                    classType = "";
                    day = 42;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 42)
                {
                    subjectThu8.Text = subj;
                    thu8secRoom.Text = sec;
                    RoomThu8.Text = course;
                    thu8crs.Text = "";
                    thu8yrRoom.Text = year;
                    thu8prof.Text = facultyName;
                    if (subj == "")
                    {
                        label67.Visible = false;
                    }
                    else
                    {
                        label67.Visible = true;
                    }
                    thu8yr.Text = "";
                    SecProfThu8.Text = "";
                    label36.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel11.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel11.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel11.BackColor = Color.White;
                    }
                    classType = "";
                    day = 52;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 52)
                {
                    subjectFri8.Text = subj;
                    fri8secRoom.Text = sec;
                    RoomFri8.Text = course;
                    fri8crs.Text = "";
                    fri8yrRoom.Text = year;
                    fri8prof.Text = facultyName;
                    if (subj == "")
                    {
                        Label73.Visible = false;
                    }
                    else
                    {
                        Label73.Visible = true;
                    }
                    fri8yr.Text = "";
                    SecProfFri8.Text = "";
                    label37.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel10.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel10.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel10.BackColor = Color.White;
                    }
                    classType = "";
                    day = 62;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 2 && day == 62)
                {
                    subjectSat8.Text = subj;
                    sat8secRoom.Text = sec;
                    RoomSat8.Text = course;
                    sat8crs.Text = "";
                    sat8yrRoom.Text = year;
                    sat8prof.Text = facultyName;
                    if (subj == "")
                    {
                        label76.Visible = false;
                    }
                    else
                    {
                        label76.Visible = true;
                    }
                    sat8yr.Text = "";
                    SecProfSat8.Text = "";
                    label40.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel4.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel4.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel4.BackColor = Color.White;
                    }
                    classType = "";
                    day = 3;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 3)
                {
                    subjectMon9.Text = subj;
                    mon9secRoom.Text = sec;
                    RoomMon9.Text = course;
                    mon9crs.Text = "";
                    mon9yrRoom.Text = year;
                    mon9prof.Text = facultyName;
                    if (subj == "")
                    {
                        label104.Visible = false;
                    }
                    else
                    {
                        label104.Visible = true;
                    }
                    mon9yr.Text = "";
                    SecProfMon9.Text = "";
                    label87.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel26.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel26.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel26.BackColor = Color.White;
                    }
                    classType = "";
                    day = 23;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 23)
                {
                    subjectTue9.Text = subj;
                    tue9secRoom.Text = sec;
                    RoomTue9.Text = course;
                    tue9crs.Text = "";
                    tue9yrRoom.Text = year;
                    tue9prof.Text = facultyName;
                    if (subj == "")
                    {
                        label100.Visible = false;
                    }
                    else
                    {
                        label100.Visible = true;
                    }
                    tue9yr.Text = "";
                    SecProfTue9.Text = "";
                    label81.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel25.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel25.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel25.BackColor = Color.White;
                    }
                    classType = "";
                    day = 33;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 33)
                {
                    subjectWed9.Text = subj;
                    wed9secRoom.Text = sec;
                    RoomWed9.Text = course;
                    wed9crs.Text = "";
                    wed9yrRoom.Text = year;
                    wed9prof.Text = facultyName;
                    SecProfWed9.Text = "";
                    wed9yr.Text = "";
                    if (subj == "")
                    {
                        label96.Visible = false;
                    }
                    else
                    {
                        label96.Visible = true;
                    }
                    wed9yr.Text = "";
                    SecProfWed9.Text = "";
                    label82.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel24.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel24.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel24.BackColor = Color.White;
                    }
                    classType = "";
                    day = 43;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 43)
                {
                    subjectThu9.Text = subj;
                    thu9secRoom.Text = sec;
                    RoomThu9.Text = course;
                    thu9crs.Text = "";
                    thu9yrRoom.Text = year;
                    thu9prof.Text = facultyName;
                    if (subj == "")
                    {
                        label91.Visible = false;
                    }
                    else
                    {
                        label91.Visible = true;
                    }
                    thu9yr.Text = "";
                    SecProfThu9.Text = "";
                    label69.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel23.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel23.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel23.BackColor = Color.White;
                    }
                    classType = "";
                    day = 53;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 53)
                {
                    subjectFri9.Text = subj;
                    fri9secRoom.Text = sec;
                    RoomFri9.Text = course;
                    fri9crs.Text = "";
                    fri9yrRoom.Text = year;
                    fri9prof.Text = facultyName;
                    if (subj == "")
                    {
                        label88.Visible = false;
                    }
                    else
                    {
                        label88.Visible = true;
                    }
                    fri9yr.Text = "";
                    SecProfFri9.Text = "";
                    label51.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel20.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel20.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel20.BackColor = Color.White;
                    }
                    classType = "";
                    day = 63;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 3 && day == 63)
                {
                    subjectSat9.Text = subj;
                    sat9secRoom.Text = sec;
                    RoomSat9.Text = course;
                    sat9crs.Text = "";
                    sat9yrRoom.Text = year;
                    sat9prof.Text = facultyName;
                    if (subj == "")
                    {
                        label80.Visible = false;
                    }
                    else
                    {
                        label80.Visible = true;
                    }
                    sat9yr.Text = "";
                    SecProfSat9.Text = "";
                    label21.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel17.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel17.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel17.BackColor = Color.White;
                    }
                    classType = "";
                    day = 4;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 4)
                {
                    subjectMon10.Text = subj;
                    mon10secRoom.Text = sec;
                    RoomMon10.Text = course;
                    mon10crs.Text = "";
                    mon10yrRoom.Text = year;
                    mon10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label109.Visible = false;
                    }
                    else
                    {
                        label109.Visible = true;
                    }
                    mon10yr.Text = "";
                    SecProfMon10.Text = "";
                    label63.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel22.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel22.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel22.BackColor = Color.White;
                    }
                    classType = "";
                    day = 24;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 24)
                {
                    subjectTue10.Text = subj;
                    tue10secRoom.Text = sec;
                    RoomTue10.Text = course;
                    tue10crs.Text = "";
                    tue10yrRoom.Text = year;
                    tue10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label113.Visible = false;
                    }
                    else
                    {
                        label113.Visible = true;
                    }
                    tue10yr.Text = "";
                    SecProfTue10.Text = "";
                    label57.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel21.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel21.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel21.BackColor = Color.White;
                    }
                    classType = "";
                    day = 34;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 34)
                {
                    subjectWed10.Text = subj;
                    wed10secRoom.Text = sec;
                    RoomWed10.Text = course;
                    wed10crs.Text = "";
                    wed10yrRoom.Text = year;
                    wed10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label118.Visible = false;
                    }
                    else
                    {
                        label118.Visible = true;
                    }
                    wed10yr.Text = "";
                    SecProfWed10.Text = "";
                    label45.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel19.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel19.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel19.BackColor = Color.White;
                    }
                    classType = "";
                    day = 44;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 44)
                {
                    subjectThu10.Text = subj;
                    thu10secRoom.Text = sec;
                    RoomThu10.Text = course;
                    thu10crs.Text = "";
                    thu10yrRoom.Text = year;
                    thu10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label121.Visible = false;
                    }
                    else
                    {
                        label121.Visible = true;
                    }
                    thu10yr.Text = "";
                    SecProfThu10.Text = "";
                    label27.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel18.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel18.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel18.BackColor = Color.White;
                    }
                    classType = "";
                    day = 54;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 54)
                {
                    subjectFri10.Text = subj;
                    fri10secRoom.Text = sec;
                    RoomFri10.Text = course;
                    fri10crs.Text = "";
                    fri10yrRoom.Text = year;
                    fri10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label126.Visible = false;
                    }
                    else
                    {
                        label126.Visible = true;
                    }
                    fri10yr.Text = "";
                    SecProfFri10.Text = "";
                    label15.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel16.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel16.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel16.BackColor = Color.White;
                    }
                    classType = "";
                    day = 64;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 4 && day == 64)
                {
                    subjectSat10.Text = subj;
                    sat10secRoom.Text = sec;
                    RoomSat10.Text = course;
                    sat10crs.Text = "";
                    sat10yrRoom.Text = year;
                    sat10prof.Text = facultyName;
                    if (subj == "")
                    {
                        label31.Visible = false;
                    }
                    else
                    {
                        label31.Visible = true;
                    }
                    sat10yr.Text = "";
                    SecProfSat10.Text = "";
                    label2.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel15.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel15.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel15.BackColor = Color.White;
                    }
                    classType = "";
                    day = 5;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 5)
                {
                    subjectMon11.Text = subj;
                    mon11secRoom.Text = sec;
                    RoomMon11.Text = course;
                    mon11crs.Text = "";
                    mon11yrRoom.Text = year;
                    mon11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label156.Visible = false;
                    }
                    else
                    {
                        label156.Visible = true;
                    }
                    mon11yr.Text = "";
                    SecProfMon11.Text = "";
                    label171.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel50.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel50.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel50.BackColor = Color.White;
                    }
                    classType = "";
                    day = 25;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 25)
                {
                    subjectTue11.Text = subj;
                    tue11secRoom.Text = sec;
                    RoomTue11.Text = course;
                    tue11crs.Text = "";
                    tue11yrRoom.Text = year;
                    tue11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label151.Visible = false;
                    }
                    else
                    {
                        label151.Visible = true;
                    }
                    tue11yr.Text = "";
                    SecProfTue11.Text = "";
                    label165.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel49.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel49.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel49.BackColor = Color.White;
                    }
                    classType = "";
                    day = 35;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 35)
                {
                    subjectWed11.Text = subj;
                    wed11secRoom.Text = sec;
                    RoomWed11.Text = course;
                    wed11crs.Text = "";
                    wed11yrRoom.Text = year;
                    wed11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label148.Visible = false;
                    }
                    else
                    {
                        label148.Visible = true;
                    }
                    wed11yr.Text = "";
                    SecProfWed11.Text = "";
                    label159.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel48.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel48.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel48.BackColor = Color.White;
                    }
                    classType = "";
                    day = 45;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 45)
                {
                    subjectThu11.Text = subj;
                    thu11secRoom.Text = sec;
                    RoomThu11.Text = course;
                    thu11crs.Text = "";
                    thu11yrRoom.Text = year;
                    thu11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label144.Visible = false;
                    }
                    else
                    {
                        label144.Visible = true;
                    }
                    thu11yr.Text = "";
                    SecProfThu11.Text = "";
                    label153.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel47.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel47.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel47.BackColor = Color.White;
                    }
                    classType = "";
                    day = 55;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 55)
                {
                    subjectFri11.Text = subj;
                    fri11secRoom.Text = sec;
                    RoomFri11.Text = course;
                    fri11crs.Text = "";
                    fri11yrRoom.Text = year;
                    fri11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label139.Visible = false;
                    }
                    else
                    {
                        label139.Visible = true;
                    }
                    fri11yr.Text = "";
                    SecProfFri11.Text = "";
                    label111.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel40.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel40.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel40.BackColor = Color.White;
                    }
                    classType = "";
                    day = 65;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 5 && day == 65)
                {
                    subjectSat11.Text = subj;
                    sat11secRoom.Text = sec;
                    RoomSat11.Text = course;
                    sat11crs.Text = "";
                    sat11yrRoom.Text = year;
                    sat11prof.Text = facultyName;
                    if (subj == "")
                    {
                        label34.Visible = false;
                    }
                    else
                    {
                        label34.Visible = true;
                    }
                    sat11yr.Text = "";
                    SecProfSat11.Text = "";
                    label65.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel33.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel33.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel33.BackColor = Color.White;
                    }
                    classType = "";
                    day = 6;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 6)
                {
                    subjectMon12.Text = subj;
                    mon12secRoom.Text = sec;
                    RoomMon12.Text = course;
                    mon12crs.Text = "";
                    mon12yrRoom.Text = year;
                    mon12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label161.Visible = false;
                    }
                    else
                    {
                        label161.Visible = true;
                    }
                    mon12yr.Text = "";
                    SecProfMon12.Text = "";
                    label135.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel44.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel44.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel44.BackColor = Color.White;
                    }
                    classType = "";
                    day = 26;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 26)
                {
                    subjectTue12.Text = subj;
                    tue12secRoom.Text = sec;
                    RoomTue12.Text = course;
                    tue12crs.Text = "";
                    tue12yrRoom.Text = year;
                    tue12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label164.Visible = false;
                    }
                    else
                    {
                        label164.Visible = true;
                    }
                    tue12yr.Text = "";
                    SecProfTue12.Text = "";
                    label123.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel42.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel42.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel42.BackColor = Color.White;
                    }
                    classType = "";
                    day = 36;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 36)
                {
                    subjectWed12.Text = subj;
                    wed12secRoom.Text = sec;
                    RoomWed12.Text = course;
                    wed12crs.Text = "";
                    wed12yrRoom.Text = year;
                    wed12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label169.Visible = false;
                    }
                    else
                    {
                        label169.Visible = true;
                    }
                    wed12yr.Text = "";
                    SecProfWed12.Text = "";
                    label99.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel38.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel38.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel38.BackColor = Color.White;
                    }
                    classType = "";
                    day = 46;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 46)
                {
                    subjectThu12.Text = subj;
                    thu12secRoom.Text = sec;
                    RoomThu12.Text = course;
                    thu12crs.Text = "";
                    thu12yrRoom.Text = year;
                    thu12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label173.Visible = false;
                    }
                    else
                    {
                        label173.Visible = true;
                    }
                    thu12yr.Text = "";
                    SecProfThu12.Text = "";
                    label78.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel35.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel35.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel35.BackColor = Color.White;
                    }
                    classType = "";
                    day = 56;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 56)
                {
                    subjectFri12.Text = subj;
                    fri12secRoom.Text = sec;
                    RoomFri12.Text = course;
                    fri12crs.Text = "";
                    fri12yrRoom.Text = year;
                    fri12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label177.Visible = false;
                    }
                    else
                    {
                        label177.Visible = true;
                    }
                    fri12yr.Text = "";
                    SecProfFri12.Text = "";
                    label50.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel31.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel31.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel31.BackColor = Color.White;
                    }
                    classType = "";
                    day = 66;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 6 && day == 66)
                {
                    subjectSat12.Text = subj;
                    sat12secRoom.Text = sec;
                    RoomSat12.Text = course;
                    sat12crs.Text = "";
                    sat12yrRoom.Text = year;
                    sat12prof.Text = facultyName;
                    if (subj == "")
                    {
                        label181.Visible = false;
                    }
                    else
                    {
                        label181.Visible = true;
                    }
                    sat12yr.Text = "";
                    SecProfSat12.Text = "";
                    label17.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel28.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel28.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel28.BackColor = Color.White;
                    }
                    classType = "";
                    day = 7;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 7)
                {
                    subjectMon1.Text = subj;
                    mon1secRoom.Text = sec;
                    RoomMon1.Text = course;
                    mon1crs.Text = "";
                    mon1yrRoom.Text = year;
                    mon1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label202.Visible = false;
                    }
                    else
                    {
                        label202.Visible = true;
                    }
                    mon1yr.Text = "";
                    SecProfMon1.Text = "";
                    label147.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel46.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel46.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel46.BackColor = Color.White;
                    }
                    classType = "";
                    day = 27;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 27)
                {
                    subjectTue1.Text = subj;
                    tue1secRoom.Text = sec;
                    RoomTue1.Text = course;
                    tue1crs.Text = "";
                    tue1yrRoom.Text = year;
                    tue1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label199.Visible = false;
                    }
                    else
                    {
                        label199.Visible = true;
                    }
                    tue1yr.Text = "";
                    SecProfTue1.Text = "";
                    label141.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel45.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel45.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel45.BackColor = Color.White;
                    }
                    classType = "";
                    day = 37;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 37)
                {
                    subjectWed1.Text = subj;
                    wed1secRoom.Text = sec;
                    RoomWed1.Text = course;
                    wed1crs.Text = "";
                    wed1yrRoom.Text = year;
                    wed1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label195.Visible = false;
                    }
                    else
                    {
                        label195.Visible = true;
                    }
                    wed1yr.Text = "";
                    SecProfWed1.Text = "";
                    label129.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel43.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel43.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel43.BackColor = Color.White;
                    }
                    classType = "";
                    day = 47;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 47)
                {
                    subjectThu1.Text = subj;
                    thu1secRoom.Text = sec;
                    RoomThu1.Text = course;
                    thu1crs.Text = "";
                    thu1yrRoom.Text = year;
                    thu1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label191.Visible = false;
                    }
                    else
                    {
                        label191.Visible = true;
                    }
                    thu1yr.Text = "";
                    SecProfThu1.Text = "";
                    label117.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel41.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel41.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel41.BackColor = Color.White;
                    }
                    classType = "";
                    day = 57;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 57)
                {
                    subjectFri1.Text = subj;
                    fri1secRoom.Text = sec;
                    RoomFri1.Text = course;
                    fri1crs.Text = "";
                    fri1yrRoom.Text = year;
                    fri1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label188.Visible = false;
                    }
                    else
                    {
                        label188.Visible = true;
                    }
                    fri1yr.Text = "";
                    SecProfFri1.Text = "";
                    label86.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel36.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel36.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel36.BackColor = Color.White;
                    }
                    classType = "";
                    day = 67;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 7 && day == 67)
                {
                    subjectSat1.Text = subj;
                    sat1secRoom.Text = sec;
                    RoomSat1.Text = course;
                    sat1crs.Text = "";
                    sat1yrRoom.Text = year;
                    sat1prof.Text = facultyName;
                    if (subj == "")
                    {
                        label184.Visible = false;
                    }
                    else
                    {
                        label184.Visible = true;
                    }
                    sat1yr.Text = "";
                    SecProfSat1.Text = "";
                    label43.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel30.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel30.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel30.BackColor = Color.White;
                    }
                    classType = "";
                    day = 8;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 8)
                {
                    subjectMon2.Text = subj;
                    mon2secRoom.Text = sec;
                    RoomMon2.Text = course;
                    mon2crs.Text = "";
                    mon2yrRoom.Text = year;
                    mon2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label206.Visible = false;
                    }
                    else
                    {
                        label206.Visible = true;
                    }
                    mon2yr.Text = "";
                    SecProfMon2.Text = "";
                    label105.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel39.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel39.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel39.BackColor = Color.White;
                    }
                    classType = "";
                    day = 28;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 28)
                {
                    subjectTue2.Text = subj;
                    tue2secRoom.Text = sec;
                    RoomTue2.Text = course;
                    tue2crs.Text = "";
                    tue2yrRoom.Text = year;
                    tue2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label209.Visible = false;
                    }
                    else
                    {
                        label209.Visible = true;
                    }
                    tue2yr.Text = "";
                    SecProfTue2.Text = "";
                    label93.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel37.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel37.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel37.BackColor = Color.White;
                    }
                    classType = "";
                    day = 38;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 38)
                {
                    subjectWed2.Text = subj;
                    wed2secRoom.Text = sec;
                    RoomWed2.Text = course;
                    wed2crs.Text = "";
                    wed2yrRoom.Text = year;
                    wed2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label213.Visible = false;
                    }
                    else
                    {
                        label213.Visible = true;
                    }
                    wed2yr.Text = "";
                    SecProfWed2.Text = "";
                    label72.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel34.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel34.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel34.BackColor = Color.White;
                    }
                    classType = "";
                    day = 48;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 48)
                {
                    subjectThu2.Text = subj;
                    thu2secRoom.Text = sec;
                    RoomThu2.Text = course;
                    thu2crs.Text = "";
                    thu2yrRoom.Text = year;
                    thu2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label217.Visible = false;
                    }
                    else
                    {
                        label217.Visible = true;
                    }
                    thu2yr.Text = "";
                    SecProfThu2.Text = "";
                    label58.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel32.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel32.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel32.BackColor = Color.White;
                    }
                    classType = "";
                    day = 58;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 58)
                {
                    subjectFri2.Text = subj;
                    fri2secRoom.Text = sec;
                    RoomFri2.Text = course;
                    fri2crs.Text = "";
                    fri2yrRoom.Text = year;
                    fri2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label220.Visible = false;
                    }
                    else
                    {
                        label220.Visible = true;
                    }
                    fri2yr.Text = "";
                    SecProfFri2.Text = "";
                    label24.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel29.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel29.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel29.BackColor = Color.White;
                    }
                    classType = "";
                    day = 68;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 8 && day == 68)
                {
                    subjectSat2.Text = subj;
                    sat2secRoom.Text = sec;
                    RoomSat2.Text = course;
                    sat2crs.Text = "";
                    sat2yrRoom.Text = year;
                    sat2prof.Text = facultyName;
                    if (subj == "")
                    {
                        label224.Visible = false;
                    }
                    else
                    {
                        label224.Visible = true;
                    }
                    sat2yr.Text = "";
                    SecProfSat2.Text = "";
                    label8.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel27.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel27.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel27.BackColor = Color.White;
                    }
                    classType = "";
                    day = 9;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 9)
                {
                    subjectMon3.Text = subj;
                    mon3secRoom.Text = sec;
                    RoomMon3.Text = course;
                    mon3crs.Text = "";
                    mon3yrRoom.Text = year;
                    mon3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label245.Visible = false;
                    }
                    else
                    {
                        label245.Visible = true;
                    }
                    mon3yr.Text = "";
                    SecProfMon3.Text = "";
                    label264.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel85.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel85.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel85.BackColor = Color.White;
                    }
                    classType = "";
                    day = 29;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 29)
                {
                    subjectTue3.Text = subj;
                    tue3secRoom.Text = sec;
                    RoomTue3.Text = course;
                    tue3crs.Text = "";
                    tue3yrRoom.Text = year;
                    tue3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label242.Visible = false;
                    }
                    else
                    {
                        label242.Visible = true;
                    }
                    tue3yr.Text = "";
                    SecProfTue3.Text = "";
                    label246.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel82.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel82.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel82.BackColor = Color.White;
                    }
                    classType = "";
                    day = 39;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 39)
                {
                    subjectWed3.Text = subj;
                    wed3secRoom.Text = sec;
                    RoomWed3.Text = course;
                    wed3crs.Text = "";
                    wed3yrRoom.Text = year;
                    wed3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label238.Visible = false;
                    }
                    else
                    {
                        label238.Visible = true;
                    }
                    wed3yr.Text = "";
                    SecProfWed3.Text = "";
                    label228.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel79.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel79.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel79.BackColor = Color.White;
                    }
                    classType = "";
                    day = 49;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 49)
                {
                    subjectThu3.Text = subj;
                    thu3secRoom.Text = sec;
                    RoomThu3.Text = course;
                    thu3crs.Text = "";
                    thu3yrRoom.Text = year;
                    thu3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label235.Visible = false;
                    }
                    else
                    {
                        label235.Visible = true;
                    }
                    thu3yr.Text = "";
                    SecProfThu3.Text = "";
                    label210.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel76.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel76.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel76.BackColor = Color.White;
                    }
                    classType = "";
                    day = 59;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 59)
                {
                    subjectFri3.Text = subj;
                    fri3secRoom.Text = sec;
                    RoomFri3.Text = course;
                    fri3crs.Text = "";
                    fri3yrRoom.Text = year;
                    fri3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label231.Visible = false;
                    }
                    else
                    {
                        label231.Visible = true;
                    }
                    fri3yr.Text = "";
                    SecProfFri3.Text = "";
                    label152.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel67.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel67.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel67.BackColor = Color.White;
                    }
                    classType = "";
                    day = 69;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 9 && day == 69)
                {
                    subjectSat3.Text = subj;
                    sat3secRoom.Text = sec;
                    RoomSat3.Text = course;
                    sat3crs.Text = "";
                    sat3yrRoom.Text = year;
                    sat3prof.Text = facultyName;
                    if (subj == "")
                    {
                        label227.Visible = false;
                    }
                    else
                    {
                        label227.Visible = true;
                    }
                    sat3yr.Text = "";
                    SecProfSat3.Text = "";
                    label84.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel58.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel58.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel58.BackColor = Color.White;
                    }
                    classType = "";
                    day = 10;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 10)
                {
                    subjectMon4.Text = subj;
                    mon4secRoom.Text = sec;
                    RoomMon4.Text = course;
                    mon4crs.Text = "";
                    mon4yrRoom.Text = year;
                    mon4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label249.Visible = false;
                    }
                    else
                    {
                        label249.Visible = true;
                    }
                    mon4yr.Text = "";
                    SecProfMon4.Text = "";
                    label192.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel73.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel73.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel73.BackColor = Color.White;
                    }
                    classType = "";
                    day = 210;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 210)
                {
                    subjectTue4.Text = subj;
                    tue4secRoom.Text = sec;
                    RoomTue4.Text = course;
                    tue4crs.Text = "";
                    tue4yrRoom.Text = year;
                    tue4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label253.Visible = false;
                    }
                    else
                    {
                        label253.Visible = true;
                    }
                    tue4yr.Text = "";
                    SecProfTue4.Text = "";
                    label174.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel70.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel70.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel70.BackColor = Color.White;
                    }
                    classType = "";
                    day = 310;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 310)
                {
                    subjectWed4.Text = subj;
                    wed4secRoom.Text = sec;
                    RoomWed4.Text = course;
                    wed4crs.Text = "";
                    wed4yrRoom.Text = year;
                    wed4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label256.Visible = false;
                    }
                    else
                    {
                        label256.Visible = true;
                    }
                    wed4yr.Text = "";
                    SecProfWed4.Text = "";
                    label130.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel64.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel64.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel64.BackColor = Color.White;
                    }
                    classType = "";
                    day = 410;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 410)
                {
                    subjectThu4.Text = subj;
                    thu4secRoom.Text = sec;
                    RoomThu4.Text = course;
                    thu4crs.Text = "";
                    thu4yrRoom.Text = year;
                    thu4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label260.Visible = false;
                    }
                    else
                    {
                        label260.Visible = true;
                    }
                    thu4yr.Text = "";
                    SecProfThu4.Text = "";
                    label108.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel61.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel61.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel61.BackColor = Color.White;
                    }
                    classType = "";
                    day = 510;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 510)
                {
                    subjectFri4.Text = subj;
                    fri4secRoom.Text = sec;
                    RoomFri4.Text = course;
                    fri4crs.Text = "";
                    fri4yrRoom.Text = year;
                    fri4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label263.Visible = false;
                    }
                    else
                    {
                        label263.Visible = true;
                    }
                    fri4yr.Text = "";
                    SecProfFri4.Text = "";
                    label56.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel55.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel55.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel55.BackColor = Color.White;
                    }
                    classType = "";
                    day = 610;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 10 && day == 610)
                {
                    subjectSat4.Text = subj;
                    sat4secRoom.Text = sec;
                    RoomSat4.Text = course;
                    sat4crs.Text = "";
                    sat4yrRoom.Text = year;
                    sat4prof.Text = facultyName;
                    if (subj == "")
                    {
                        label267.Visible = false;
                    }
                    else
                    {
                        label267.Visible = true;
                    }
                    sat4yr.Text = "";
                    SecProfSat4.Text = "";
                    label19.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel52.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel52.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel52.BackColor = Color.White;
                    }
                    classType = "";
                    day = 11;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 11)
                {
                    subjectMon5.Text = subj;
                    mon5secRoom.Text = sec;
                    RoomMon5.Text = course;
                    mon5crs.Text = "";
                    mon5yrRoom.Text = year;
                    mon5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label286.Visible = false;
                    }
                    else
                    {
                        label286.Visible = true;
                    }
                    mon5yr.Text = "";
                    SecProfMon5.Text = "";
                    label270.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel86.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel86.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel86.BackColor = Color.White;
                    }
                    classType = "";
                    day = 211;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 211)
                {
                    subjectTue5.Text = subj;
                    tue5secRoom.Text = sec;
                    RoomTue5.Text = course;
                    tue5crs.Text = "";
                    tue5yrRoom.Text = year;
                    tue5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label283.Visible = false;
                    }
                    else
                    {
                        label283.Visible = true;
                    }
                    tue5yr.Text = "";
                    SecProfTue5.Text = "";
                    label258.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel84.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel84.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel84.BackColor = Color.White;
                    }
                    classType = "";
                    day = 311;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 311)
                {
                    subjectWed5.Text = subj;
                    wed5secRoom.Text = sec;
                    RoomWed5.Text = course;
                    wed5crs.Text = "";
                    wed5yrRoom.Text = year;
                    wed5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label280.Visible = false;
                    }
                    else
                    {
                        label280.Visible = true;
                    }
                    wed5yr.Text = "";
                    SecProfWed5.Text = "";
                    label252.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel83.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel83.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel83.BackColor = Color.White;
                    }
                    classType = "";
                    day = 411;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 411)
                {
                    subjectThu5.Text = subj;
                    thu5secRoom.Text = sec;
                    RoomThu5.Text = course;
                    thu5crs.Text = "";
                    thu5yrRoom.Text = year;
                    thu5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label277.Visible = false;
                    }
                    else
                    {
                        label277.Visible = true;
                    }
                    thu5yr.Text = "";
                    SecProfThu5.Text = "";
                    label240.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel81.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel81.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel81.BackColor = Color.White;
                    }
                    classType = "";
                    day = 511;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 511)
                {
                    subjectFri5.Text = subj;
                    fri5secRoom.Text = sec;
                    RoomFri5.Text = course;
                    fri5crs.Text = "";
                    fri5yrRoom.Text = year;
                    fri5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label274.Visible = false;
                    }
                    else
                    {
                        label274.Visible = true;
                    }
                    fri5yr.Text = "";
                    SecProfFri5.Text = "";
                    label180.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel71.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel71.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel71.BackColor = Color.White;
                    }
                    classType = "";
                    day = 611;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 11 && day == 611)
                {
                    subjectSat5.Text = subj;
                    sat5secRoom.Text = sec;
                    RoomSat5.Text = course;
                    sat5crs.Text = "";
                    sat5yrRoom.Text = year;
                    sat5prof.Text = facultyName;
                    if (subj == "")
                    {
                        label271.Visible = false;
                    }
                    else
                    {
                        label271.Visible = true;
                    }
                    sat5yr.Text = "";
                    SecProfSat5.Text = "";
                    label101.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel60.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel60.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel60.BackColor = Color.White;
                    }
                    classType = "";
                    day = 12;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 12)
                {
                    subjectMon6.Text = subj;
                    mon6secRoom.Text = sec;
                    RoomMon6.Text = course;
                    mon6crs.Text = "";
                    mon6yrRoom.Text = year;
                    mon6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label289.Visible = false;
                    }
                    else
                    {
                        label289.Visible = true;
                    }
                    mon6yr.Text = "";
                    SecProfMon6.Text = "";
                    label216.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel77.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel77.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel77.BackColor = Color.White;
                    }
                    classType = "";
                    day = 212;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 212)
                {
                    subjectTue6.Text = subj;
                    tue6secRoom.Text = sec;
                    RoomTue6.Text = course;
                    tue6crs.Text = "";
                    tue6yrRoom.Text = year;
                    tue6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label292.Visible = false;
                    }
                    else
                    {
                        label292.Visible = true;
                    }
                    tue6yr.Text = "";
                    SecProfTue6.Text = "";
                    label198.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel74.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel74.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel74.BackColor = Color.White;
                    }
                    classType = "";
                    day = 312;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 312)
                {
                    subjectWed6.Text = subj;
                    wed6secRoom.Text = sec;
                    RoomWed6.Text = course;
                    wed6crs.Text = "";
                    wed6yrRoom.Text = year;
                    wed6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label295.Visible = false;
                    }
                    else
                    {
                        label295.Visible = true;
                    }
                    wed6yr.Text = "";
                    SecProfWed6.Text = "";
                    label160.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel68.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel68.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel68.BackColor = Color.White;
                    }
                    classType = "";
                    day = 412;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 412)
                {
                    subjectThu6.Text = subj;
                    thu6secRoom.Text = sec;
                    RoomThu6.Text = course;
                    thu6crs.Text = "";
                    thu6yrRoom.Text = year;
                    thu6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label298.Visible = false;
                    }
                    else
                    {
                        label298.Visible = true;
                    }
                    thu6yr.Text = "";
                    SecProfThu6.Text = "";
                    label122.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel63.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel63.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel63.BackColor = Color.White;
                    }
                    classType = "";
                    day = 512;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 512)
                {
                    subjectFri6.Text = subj;
                    fri6secRoom.Text = sec;
                    RoomFri6.Text = course;
                    fri6crs.Text = "";
                    fri6yrRoom.Text = year;
                    fri6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label301.Visible = false;
                    }
                    else
                    {
                        label301.Visible = true;
                    }
                    fri6yr.Text = "";
                    SecProfFri6.Text = "";
                    label74.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel57.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel57.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel57.BackColor = Color.White;
                    }
                    classType = "";
                    day = 612;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 12 && day == 612)
                {
                    subjectSat6.Text = subj;
                    sat6secRoom.Text = sec;
                    RoomSat6.Text = course;
                    sat6crs.Text = "";
                    sat6yrRoom.Text = year;
                    sat6prof.Text = facultyName;
                    if (subj == "")
                    {
                        label304.Visible = false;
                    }
                    else
                    {
                        label304.Visible = true;
                    }
                    sat6yr.Text = "";
                    SecProfSat6.Text = "";
                    label28.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel53.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel53.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel53.BackColor = Color.White;
                    }
                    classType = "";
                    day = 13;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 13)
                {
                    subjectMon7pm.Text = subj;
                    mon7pmsecRoom.Text = sec;
                    RoomMon7pm.Text = course;
                    mon7pmcrs.Text = "";
                    mon7pmyrRoom.Text = year;
                    mon7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label322.Visible = false;
                    }
                    else
                    {
                        label322.Visible = true;
                    }
                    mon7pmyr.Text = "";
                    SecProfMon7pm.Text = "";
                    label234.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel80.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel80.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel80.BackColor = Color.White;
                    }
                    classType = "";
                    day = 213;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 213)
                {
                    subjectTue7pm.Text = subj;
                    tue7pmsecRoom.Text = sec;
                    RoomTue7pm.Text = course;
                    tue7pmcrs.Text = "";
                    tue7pmyrRoom.Text = year;
                    tue7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label319.Visible = false;
                    }
                    else
                    {
                        label319.Visible = true;
                    }
                    tue7pmyr.Text = "";
                    SecProfTue7pm.Text = "";
                    label222.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel78.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel78.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel78.BackColor = Color.White;
                    }
                    classType = "";
                    day = 313;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 313)
                {
                    subjectWed7pm.Text = subj;
                    wed7pmsecRoom.Text = sec;
                    RoomWed7pm.Text = course;
                    wed7pmcrs.Text = "";
                    wed7pmyrRoom.Text = year;
                    wed7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label316.Visible = false;
                    }
                    else
                    {
                        label316.Visible = true;
                    }
                    wed7pmyr.Text = "";
                    SecProfWed7pm.Text = "";
                    label204.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel75.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel75.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel75.BackColor = Color.White;
                    }
                    classType = "";
                    day = 413;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 413)
                {
                    subjectThu7pm.Text = subj;
                    thu7pmsecRoom.Text = sec;
                    RoomThu7pm.Text = course;
                    thu7pmcrs.Text = "";
                    thu7pmyrRoom.Text = year;
                    thu7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label298.Visible = false;
                    }
                    else
                    {
                        label298.Visible = true;
                    }
                    thu7pmyr.Text = "";
                    SecProfThu7pm.Text = "";
                    label186.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel72.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel72.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel72.BackColor = Color.White;
                    }
                    classType = "";
                    day = 513;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 513)
                {
                    subjectFri7pm.Text = subj;
                    fri7pmsecRoom.Text = sec;
                    RoomFri7pm.Text = course;
                    fri7pmcrs.Text = "";
                    fri7pmyrRoom.Text = year;
                    fri7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label310.Visible = false;
                    }
                    else
                    {
                        label310.Visible = true;
                    }
                    fri7pmyr.Text = "";
                    SecProfFri7pm.Text = "";
                    label137.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel65.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel65.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel65.BackColor = Color.White;
                    }
                    classType = "";
                    day = 613;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 13 && day == 613)
                {
                    subjectSat7pm.Text = subj;
                    sat7pmsecRoom.Text = sec;
                    RoomSat7pm.Text = course;
                    sat7pmcrs.Text = "";
                    sat7pmyrRoom.Text = year;
                    sat7pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label307.Visible = false;
                    }
                    else
                    {
                        label307.Visible = true;
                    }
                    sat7pmyr.Text = "";
                    SecProfSat7pm.Text = "";
                    label66.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel56.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel56.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel56.BackColor = Color.White;
                    }
                    classType = "";
                    day = 14;
                    time += 1;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 14)
                {
                    subjectMon8pm.Text = subj;
                    mon8pmsecRoom.Text = sec;
                    RoomMon8pm.Text = course;
                    mon8pmcrs.Text = "";
                    mon8pmyrRoom.Text = year;
                    mon8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label325.Visible = false;
                    }
                    else
                    {
                        label325.Visible = true;
                    }
                    mon8pmyr.Text = "";
                    SecProfMon8pm.Text = "";
                    label167.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel69.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel69.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel69.BackColor = Color.White;
                    }
                    classType = "";
                    day = 214;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 214)
                {
                    subjectTue8pm.Text = subj;
                    tue8pmsecRoom.Text = sec;
                    RoomTue8pm.Text = course;
                    tue8pmcrs.Text = "";
                    tue8pmyrRoom.Text = year;
                    tue8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label328.Visible = false;
                    }
                    else
                    {
                        label328.Visible = true;
                    }
                    tue8pmyr.Text = "";
                    SecProfTue8pm.Text = "";
                    label145.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel66.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel66.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel66.BackColor = Color.White;
                    }
                    classType = "";
                    day = 314;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 314)
                {
                    subjectWed8pm.Text = subj;
                    wed8pmsecRoom.Text = sec;
                    RoomWed8pm.Text = course;
                    wed8pmcrs.Text = "";
                    wed8pmyrRoom.Text = year;
                    wed8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label331.Visible = false;
                    }
                    else
                    {
                        label331.Visible = true;
                    }
                    wed8pmyr.Text = "";
                    SecProfWed8pm.Text = "";
                    label115.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel62.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel62.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel62.BackColor = Color.White;
                    }
                    classType = "";
                    day = 414;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 414)
                {
                    subjectThu8pm.Text = subj;
                    thu8pmsecRoom.Text = sec;
                    RoomThu8pm.Text = course;
                    thu8pmcrs.Text = "";
                    thu8pmyrRoom.Text = year;
                    thu8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label334.Visible = false;
                    }
                    else
                    {
                        label334.Visible = true;
                    }
                    thu8pmyr.Text = "";
                    SecProfThu8pm.Text = "";
                    label92.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel59.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel59.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel59.BackColor = Color.White;
                    }
                    classType = "";
                    day = 514;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 514)
                {
                    subjectFri8pm.Text = subj;
                    fri8pmsecRoom.Text = sec;
                    RoomFri8pm.Text = course;
                    fri8pmcrs.Text = "";
                    fri8pmyrRoom.Text = year;
                    fri8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label337.Visible = false;
                    }
                    else
                    {
                        label337.Visible = true;
                    }
                    fri8pmyr.Text = "";
                    SecProfFri8pm.Text = "";
                    label48.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel54.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel54.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel54.BackColor = Color.White;
                    }
                    classType = "";
                    day = 614;
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
                if (time == 14 && day == 614)
                {
                    subjectSat8pm.Text = subj;
                    sat8pmsecRoom.Text = sec;
                    RoomSat8pm.Text = course;
                    sat8pmcrs.Text = "";
                    sat8pmyrRoom.Text = year;
                    sat8pmprof.Text = facultyName;
                    if (subj == "")
                    {
                        label340.Visible = false;
                    }
                    else
                    {
                        label340.Visible = true;
                    }
                    sat8pmyr.Text = "";
                    SecProfSat8pm.Text = "";
                    label9.Visible = false;
                    if (classType == "Lecture")
                    {
                        panel51.BackColor = Color.Green;
                    }
                    else if (classType == "Laboratory")
                    {
                        panel51.BackColor = Color.Yellow;
                    }
                    else if (classType == "")
                    {
                        panel51.BackColor = Color.White;
                    }
                    classType = "";
                    subj = "";
                    room = "";
                    sec = "";
                    year = "";
                    course = "";
                    facultyName = "";
                    facultyCode = "";
                    schedule();
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();


        }

        public void labelsView()
        {
            clear();

            if (sched.comboBox1.Text == "Section")
            {
                foreach (Control p in Controls)
                {
                    if (p is Panel)
                    {
                        Panel pl = (Panel)p;
                        pl.BackColor = Color.White;
                    }
                    panel1.BackColor = Color.Gainsboro;
                    panel2.BackColor = Color.Gainsboro;
                }
                lbl1.Show();
                lbl2.Show();
                lbl3.Show();
                lbl4.Show();
                lbl1.Text = "COURSE: " + sched.dgvFaculty.CurrentRow.Cells[0].Value.ToString();
                lbl2.Text = "YEAR: ";
                lbl3.Text = "MAJOR: ";

                 PopulateGridViewSection();

            }
            else if (sched.comboBox1.Text == "Faculty")
            {
                totalhrs();
                lbl1.Show();
                lbl2.Show();
                lbl3.Show();
                lbl4.Show();
                lbl2.Text = "";
                lbl4.Text = "Total no. of Contact hours per week: " + total;
              
                  PopulateGridViewSection();
             
                lbl2.Text = "";
           
            }
            else if (sched.comboBox1.Text == "Room")
            {
                lbl1.Hide();
                lbl2.Hide();
                lbl3.Hide();
                lbl4.Hide();
               
             
            }
        }

    

        
        

        private void textBox107_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

