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
    public partial class RoomArchive : Form
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
        RoomFrm roomFrm;
        string loginAct = "";
        string typeofAcc = "";
        public RoomArchive(RoomFrm RF)
        {
            InitializeComponent();
            this.roomFrm = RF;
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
        void PopulateGridViewRoomArchive() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT ID,RoomID,Room,Course, (select case when RoomCategory = 0 then 'Other Rooms' when RoomCategory = 1 then 'Computer Lab' when RoomCategory = 2 then 'Not Computer Lab' end) RoomCategory FROM RoomArchive_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.Columns["ID"].Visible = false;
                    dataGridView1.Columns["RoomID"].Visible = false;
                    dataGridView1.AllowUserToAddRows = false;

                    this.dataGridView1.Columns["Room"].ReadOnly = true;
                    this.dataGridView1.Columns["Course"].ReadOnly = true;
                    this.dataGridView1.Columns["RoomCategory"].ReadOnly = true;
                    this.dataGridView1.Columns["RoomCategory"].HeaderText = "Room Category";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RoomArchive_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGridViewRoomArchive();
                AdminActivity();
                btnRecover.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRecover.Width,
                btnRecover.Height, 30, 30));
                btnDel.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDel.Width,
                btnDel.Height, 30, 30));
                btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
              btnClose.Height, 30, 30));
                dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
         dataGridView1.Height, 5, 5));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Do you really want to recover this data?", "Recover", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        string roomcateg = "";
                        if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Other Rooms")
                        {
                            roomcateg = "0";
                        }
                        else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Computer Lab")
                        {
                            roomcateg = "1";
                        }
                        else if (dataGridView1.CurrentRow.Cells["RoomCategory"].Value.ToString() == "Not Computer Lab")
                        {
                            roomcateg = "2";
                        }
                        string numberofRoom = "0";
                        string queryyy = "select count(Room) as numberOfroom From Room_Tbl Where RoomCategory = '" + roomcateg + "' AND Course='" + dataGridView1.CurrentRow.Cells["Course"].Value.ToString() + "'";
                        SqlCommand commanddd = new SqlCommand(queryyy, sqlcon);
                        SqlDataReader readerrr = commanddd.ExecuteReader();

                        if (readerrr.Read() == true)
                        {
                            numberofRoom = readerrr["numberOfroom"].ToString();
                        }
                        readerrr.Close();
                        int roomcategFinal = (Convert.ToInt32(numberofRoom) + 1);
                        SqlCommand cmd = new SqlCommand("INSERT INTO Room_Tbl (RoomID,Room,RoomCategory,Course) VALUES (@RoomID,@Room,@RoomCategory,@Course)", sqlcon);
                        cmd.Parameters.AddWithValue("@RoomID", roomcategFinal.ToString());
                        cmd.Parameters.AddWithValue("@Room", dataGridView1.CurrentRow.Cells["Room"].Value.ToString());
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
                        cmd.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Recovered","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        SqlCommand cmddel = new SqlCommand("DELETE FROM RoomArchive_Tbl WHERE Room = @Room", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@Room", dataGridView1.CurrentRow.Cells["Room"].Value.ToString());
                        cmddel.ExecuteNonQuery();
                        PopulateGridViewRoomArchive();
                        roomFrm.PopulateGridViewRoom();

                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " recover a room from archive");
                        cm.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Do you really want to delete this data?", "Delete", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        SqlCommand cmddel = new SqlCommand("DELETE FROM RoomArchive_Tbl WHERE ID = @ID", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells["ID"].Value);
                        cmddel.ExecuteNonQuery();
                        MessageBox.Show("Deleted");
                        PopulateGridViewRoomArchive();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO ActivityLog_Tbl (Username,DateTime,ActivityLog) VALUES (@Username,@DateTime,@ActivityLog)", sqlcon);
                        cmd1.Parameters.AddWithValue("@Username", loginAct);
                        cmd1.Parameters.Add("@DateTime", SqlDbType.DateTime);
                        cmd1.Parameters["@DateTime"].Value = DateTime.Now;
                        cmd1.Parameters.AddWithValue("@ActivityLog", loginAct + " deleted a room from archive");
                        cmd1.ExecuteNonQuery();
                    }
                }
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
    }
}
