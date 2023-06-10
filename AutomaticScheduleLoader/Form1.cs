using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
namespace AutomaticScheduleLoader
{
  
    public partial class Main : Form
    {
        string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
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
        Login login;
        string usertype = "";
        bool homeCollapsed;
        bool ScheduleCollapsed;
        string loginAct = "";
        public Main(Login lg)
        {
            InitializeComponent();
       //     this.FormBorderStyle = FormBorderStyle.None;
         //   Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
         this.login = lg;
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
            }
        }
        private void btnAddUpFaculty_Click(object sender, EventArgs e)
        {
            frmFaculty frmf = new frmFaculty(); 
            frmf.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmSubj frmsub = new frmSubj(); 
            frmsub.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DefSchedFrm dsf = new DefSchedFrm();
            dsf.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecFrm secFrm = new SecFrm();
            secFrm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SearchSchedule schdfrm = new SearchSchedule();
            schdfrm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RoomFrm rmFrm = new RoomFrm();
            rmFrm.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            HomeTimer.Start();
            ScheduleCollapsed = false;
            ScheduleTimer.Start();
        }

        private void HomeTimer_Tick(object sender, EventArgs e)
        {
            if (homeCollapsed)
            {
                HomeContainer.Height += 10;
                if (HomeContainer.Height == HomeContainer.MaximumSize.Height)
                {
                    homeCollapsed = false;
                    HomeTimer.Stop();
                }
            }
            else
            {
                HomeContainer.Height -= 10;
                if (HomeContainer.Height == HomeContainer.MinimumSize.Height)
                {
                    homeCollapsed = true;
                    HomeTimer.Stop();
                }
            }
        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
            HomeTimer.Start();
            ScheduleCollapsed = false;
            ScheduleTimer.Start();

        
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
          
           
        }

        private void button6_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public void UserCheck()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query1 = "select UserType FROM User_Tbl WHERE Username='" + loginAct + "'";
                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                SqlDataReader reader1 = command1.ExecuteReader();

                if (reader1.Read() == true)
                {



                    usertype = reader1["UserType"].ToString();
                   

                }
                reader1.Close();
            }

        }
        private void Main_Load(object sender, EventArgs e)
        {
            AdminActivity();
            UserCheck();
            if (usertype == "0" || usertype == "3")
            {
                button8.Visible = true;

            }
            else
            {
                button8.Visible = false;
            }
            login.txtUSERNAME.Font = new Font(login.txtUSERNAME.Font, FontStyle.Bold);
            label6.Visible = false;
       //     label6.Text = "Welcome "+ loginAct+"!";
           
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Close();
            login.Show();
            
        }

        

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
           

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
           DialogResult dr = MessageBox.Show("Do you really want to close the program?","Close the program",MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes)
            {
                e.Cancel = false;
                Login login = new Login();
                login.Show();
            }
            else if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
           
           
        }
      
        private void button8_Click_1(object sender, EventArgs e)
        {
            AccManagement am = new AccManagement();
            am.ShowDialog();
        }

        private void ScheduleTimer_Tick(object sender, EventArgs e)
        {
            if (ScheduleCollapsed)
            {
                ScheduleContainer.Height += 10;
                if (ScheduleContainer.Height == ScheduleContainer.MaximumSize.Height)
                {
                    ScheduleCollapsed = false;
                    ScheduleTimer.Stop();
                }
            }
            else
            {
                ScheduleContainer.Height -= 10;
                if (ScheduleContainer.Height == ScheduleContainer.MinimumSize.Height)
                {
                    ScheduleCollapsed = true;
                    ScheduleTimer.Stop();
                }
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            ScheduleTimer.Start();
            homeCollapsed = false;
            HomeTimer.Start();
        }

        private void button7_MouseHover(object sender, EventArgs e)
        {
            ScheduleTimer.Start();
            homeCollapsed = false;
            HomeTimer.Start();
         
        }
    }
    }

