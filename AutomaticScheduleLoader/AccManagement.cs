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

    public partial class AccManagement : Form
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
        string user = "";
        string userID = "";
        public string pass = "";
        string userAdd = "";
        string usertype = "";
       public string mainAdmin = "";
       public string MainAddminPass = "";
        string checker = "";
        public AccManagement()
        {
            InitializeComponent();
        }
        public void PopulateGridViewAcc() // populate all in gridview faculty
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserID,Username,Password, (select case when UserType = 1 then 'Normal' when UserType = 0 then 'Admin' when UserType = 3 then 'Main Admin' end) UserType FROM User_Tbl", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.EnableHeadersVisualStyles = false;

                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[3].Width = 170;

                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[3].ReadOnly = true;
                this.dataGridView1.Columns[3].HeaderText = "User Type";
            }
           
        }
        public void SearchUsername() // populate all in gridview faculty
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserID,Username,Password, (select case when UserType = 1 then 'Normal' when UserType = 0 then 'Admin' when UserType = 3 then 'Main Admin' end) UserType FROM User_Tbl where Username like'%"+txtSearch.Text+"%'", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.EnableHeadersVisualStyles = false;

                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[3].Width = 170;

                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[3].ReadOnly = true;
                this.dataGridView1.Columns[3].HeaderText = "User Type";
            }

        }
        private void AccManagement_Load(object sender, EventArgs e)
        {
            PopulateGridViewAcc();
            if(dataGridView1.Rows.Count != 0)
            {
                clear();
            }
            txtPassConfirmAdd.UseSystemPasswordChar = true;
            txtPassAdd.UseSystemPasswordChar = true;
            button2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button2.Width,
            button2.Height, 30, 30));
            btnSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSearch.Width,
            btnSearch.Height, 30, 30));
            btnRecover.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRecover.Width,
          btnRecover.Height, 30, 30));
            btnDel.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDel.Width,
           btnDel.Height, 30, 30));
            button1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button1.Width,
            button1.Height, 30, 30));
            button5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button5.Width,
          button5.Height, 30, 30));
            button4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button4.Width,
          button4.Height, 30, 30));
            button3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button3.Width,
           button3.Height, 30, 30));
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
            btnClose.Height, 30, 30));

            dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
           dataGridView1.Height, 5, 5));
            /*
            txtboxPassCU.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtboxPassCU.Width,
       txtboxPassCU.Height, 5, 5));
            txtboxUserCU.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtboxUserCU.Width,
       txtboxUserCU.Height, 5, 5));
            txtPassAdd.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtPassAdd.Width,
       txtPassAdd.Height, 5, 5));
            txtPassConfirmAdd.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtPassConfirmAdd.Width,
       txtPassConfirmAdd.Height, 5, 5));
            txtUserAdd.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtUserAdd.Width,
      txtUserAdd.Height, 5, 5));
            txtSearch.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, txtSearch.Width,
    txtSearch.Height, 5, 5));
            */
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.ColumnIndex == 2 && e.Value != null)
            {
                e.Value = new string('*', e.Value.ToString().Length);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            groupBox1.Enabled = true;
            txtboxUserCU.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtboxPassCU.UseSystemPasswordChar = true;
            checkBox1.Checked = false;
            txtboxPassCU.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            userID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            UserCheck();
            UserCheckAdmin();
            txtboxPassCU.Enabled = false;
            if (dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Main Admin")
            {
                btnDel.Enabled = false;
            }
            else
            {
                btnDel.Enabled = true;
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            if(checkBox1.Checked == true)
            {
                PassConfirm pc = new PassConfirm(this);
                pc.ShowDialog();
            }
            else
            {
                
                txtboxPassCU.Enabled = false;
                txtboxPassCU.UseSystemPasswordChar = true;
                
            }
        }
        public void UserCheck()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query1 = "select Username,Password,UserType FROM User_Tbl WHERE UserID='" + userID + "'";
                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                SqlDataReader reader1 = command1.ExecuteReader();

                if (reader1.Read() == true)
                {


                    //userID = reader1["UserID"].ToString();
                    user = reader1["Username"].ToString();
                    pass = reader1["Password"].ToString();
                    usertype = reader1["UserType"].ToString();
                }
                reader1.Close();
                string querycont = "SELECT COUNT(Username) AS UserDuplicate FROM UserArchive_Tbl WHERE Username=@Username";
                SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                commandcont.Parameters.AddWithValue("@Username", txtboxUserCU.Text);
                SqlDataReader readercont = commandcont.ExecuteReader();

                if (readercont.Read() == true)
                {


                    checker = readercont["UserDuplicate"].ToString();


                }
                readercont.Close();
                sqlcon.Close();
            }
            
        }
        public void UserCheckAdd()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string query1 = "select UserID,Username,Password,UserType FROM User_Tbl WHERE Username='" + txtUserAdd.Text + "'";
                SqlCommand command1 = new SqlCommand(query1, sqlcon);
                SqlDataReader reader1 = command1.ExecuteReader();

                if (reader1.Read() == true)
                {


                    userAdd = reader1["Username"].ToString();
                }
                reader1.Close();
            }

        }
        public void UserCheckAdmin()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                
                    string query1 = "select Password FROM User_Tbl WHERE Usertype='" + "3" + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {


                        MainAddminPass = reader1["Password"].ToString();
                    }
                    reader1.Close();
                
                
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            UserCheckAdd();
            DialogResult dr = MessageBox.Show("Add new Account?", "New Account", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (txtUserAdd.Text == userAdd)
                {
                    MessageBox.Show("A username with that username already exist","Username Exist",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else
                {
                    if (txtPassAdd.Text != txtPassConfirmAdd.Text)
                    {
                        MessageBox.Show("Your password does not match","Password",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    }

                    else
                    {
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO User_Tbl (Username,Password,UserType) VALUES (@Username,@Password,@UserType)", sqlcon);
                            cmd.Parameters.AddWithValue("@Username", txtUserAdd.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassAdd.Text);
                            cmd.Parameters.AddWithValue("@UserType", "0");
                            cmd.ExecuteNonQuery();
                            PopulateGridViewAcc();
                            MessageBox.Show("Succesfully registered","Registered",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            txtPassAdd.Text = "";
                            txtPassConfirmAdd.Text = "";
                            txtUserAdd.Text = "";
                        }
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            UserCheck();
            DialogResult dr = MessageBox.Show("Archive data?", "Archive", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (txtboxUserCU.Text != user)
                {
                    MessageBox.Show("The Username does nat match with the current record. Please Update the details first before archiving.", "Does not match Record",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else if (txtboxPassCU.UseSystemPasswordChar == false && txtboxPassCU.Text != pass)
                {
                    MessageBox.Show("The Password does nat match with the current record. Please Update the details first before archiving.", "Does not match Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtboxUserCU.Text == user || txtboxPassCU.UseSystemPasswordChar == false && txtboxPassCU.Text == pass)
                {

                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO UserArchive_Tbl (UserID,Username,Password,UserType) VALUES (@UserID,@Username,@Password,@UserType)", sqlcon);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Username", txtboxUserCU.Text);
                        cmd.Parameters.AddWithValue("@Password", txtboxPassCU.Text);
                        cmd.Parameters.AddWithValue("@UserType", usertype);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Archived");
                        SqlCommand cmddel = new SqlCommand("DELETE FROM User_Tbl WHERE @UserID = UserID", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@UserID", userID);
                        cmddel.ExecuteNonQuery();
                        clear();
                        PopulateGridViewAcc();
                    }
                }
            }
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            UserCheck();
            DialogResult dr = MessageBox.Show("Do you really want to save changes?","Save Changes", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(dr == DialogResult.Yes)
            {
            if (Convert.ToInt32(checker) >= 1)
            {
                MessageBox.Show("A user with that username already exist","Username Already Exist",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
             else
             {
                     using (SqlConnection sqlcon = new SqlConnection(conn))
                     {
                         sqlcon.Open();
                         SqlCommand cmd = new SqlCommand("UPDATE User_Tbl SET Username=@Username,Password=@Password WHERE UserID=@UserID", sqlcon);
                         cmd.Parameters.AddWithValue("@UserID", userID);
                         cmd.Parameters.AddWithValue("@Username", txtboxUserCU.Text);
                         cmd.Parameters.AddWithValue("@Password", txtboxPassCU.Text);
                         cmd.ExecuteNonQuery();
                         PopulateGridViewAcc();
                         MessageBox.Show("Succesfully Updated");
                         groupBox1.Enabled = false;
                         clear();
                    }
             }
            }
        }
        public void clear()
        {
            txtPassAdd.Text = "";
            txtPassConfirmAdd.Text = "";
            txtUserAdd.Text = "";
            txtboxPassCU.Text = "";
            txtboxUserCU.Text = "";
            txtboxPassCU.Enabled = false;
            txtSearch.Text = "";
            txtboxPassCU.UseSystemPasswordChar = true;
            groupBox1.Enabled = false;
            PopulateGridViewAcc();
            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchUsername();
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AccManagementArchived acm = new AccManagementArchived(this);
            acm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ActivityLog al = new ActivityLog();
            al.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoginActivity la = new LoginActivity();
            la.ShowDialog();
        }
    }


}
