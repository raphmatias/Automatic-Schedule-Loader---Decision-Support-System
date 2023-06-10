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
    public partial class AccManagementArchived : Form
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
        AccManagement am;
        string checker = "0";
        public AccManagementArchived(AccManagement Amform)
        {
            InitializeComponent();
            am = Amform;    
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void PopulateGridViewAccArchived() // populate all in gridview faculty
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT UserID,Username,Password, (select case when UserType = 1 then 'Normal' when UserType = 0 then 'Admin' when UserType = 3 then 'Main Admin' end) UserType FROM UserArchive_Tbl", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.EnableHeadersVisualStyles = false;

                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[3].Width = 170;
                this.dataGridView1.Columns[3].HeaderText = "User Type";

            }

        }
        public void Userduplicate() // populate all in gridview faculty
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                string querycont = "SELECT COUNT(Username) AS UserDuplicate FROM User_Tbl WHERE Username=@Username";
                SqlCommand commandcont = new SqlCommand(querycont, sqlcon);
                commandcont.Parameters.AddWithValue("@Username", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                SqlDataReader readercont = commandcont.ExecuteReader();

                if (readercont.Read() == true)
                {


                    checker = readercont["UserDuplicate"].ToString();


                }
                readercont.Close();
            }
        }

            private void AccManagementArchived_Load(object sender, EventArgs e)
        {
            PopulateGridViewAccArchived();
            if(dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
          btnClose.Height, 30, 30));
            btnDel.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDel.Width,
           btnDel.Height, 30, 30));
            btnRecover.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRecover.Width,
            btnRecover.Height, 30, 30));
            dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
dataGridView1.Height, 5, 5));
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
           DialogResult dr =  MessageBox.Show("Do you really want to recover this data?", "Recover Account", MessageBoxButtons.YesNo);
            if(dr == DialogResult.Yes)
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    if (Convert.ToInt32(checker) >= 1)
                    {
                        MessageBox.Show("There is already a existing username account", "Existing Username Account");
                    }
                    else
                    {
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO User_Tbl (Username,Password,UserType) VALUES (@Username,@Password,@UserType)", sqlcon);
                        
                        cmd.Parameters.AddWithValue("@Username", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                        cmd.Parameters.AddWithValue("@Password", dataGridView1.CurrentRow.Cells[2].Value.ToString());
                        string num = "0";
                        if(dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Admin")
                        {
                            num = "0";
                        }
                        else if (dataGridView1.CurrentRow.Cells[3].Value.ToString() == "Normal")
                        {
                            num = "1";
                        }
                        cmd.Parameters.AddWithValue("@UserType", num);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Recovered");
                        SqlCommand cmddel = new SqlCommand("DELETE FROM UserArchive_Tbl WHERE @UserID = UserID", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@UserID", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        cmddel.ExecuteNonQuery();
                        PopulateGridViewAccArchived();
                        am.PopulateGridViewAcc();
                    }

                }
            }
            
           
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you really want to delete this data?", "Delete Account", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlCommand cmddel = new SqlCommand("DELETE FROM UserArchive_Tbl WHERE @UserID = UserID", sqlcon);
                    cmddel.CommandType = CommandType.Text;
                    cmddel.Parameters.AddWithValue("@UserID", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                    cmddel.ExecuteNonQuery();
                    MessageBox.Show("Permanently Deleted");
                    PopulateGridViewAccArchived();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Userduplicate();
        }
    }
}
