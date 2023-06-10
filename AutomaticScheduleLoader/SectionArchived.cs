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
    public partial class SectionArchived : Form
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
        SecFrm secFrm;
        string loginAct = "";
        string typeofAcc = "";
        public SectionArchived(SecFrm SF)
        {
            InitializeComponent();
            this.secFrm = SF;
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
        void PopulateGridViewSectionArchive() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM SectionArchive_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.AllowUserToAddRows = false;

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

        private void SectionArchived_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGridViewSectionArchive();
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
        //    try
        //    {
                DialogResult dr = MessageBox.Show("Do you really want to recover this data?", "Recover", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    using (SqlConnection sqlcon = new SqlConnection(conn))
                    {
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO Section_Tbl (Course,YearLevel,SectionSlot) VALUES (@Course,@YearLevel,@SectionSlot)", sqlcon);
                        cmd.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells["Course"].Value.ToString());
                        cmd.Parameters.AddWithValue("@YearLevel", dataGridView1.CurrentRow.Cells["YearLevel"].Value.ToString());
                        cmd.Parameters.AddWithValue("@SectionSlot", dataGridView1.CurrentRow.Cells["SectionSlot"].Value.ToString());
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Recovered","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        SqlCommand cmddel = new SqlCommand("DELETE FROM SectionArchive_Tbl WHERE ID = '"+ Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value.ToString()) + "'", sqlcon);
                        cmddel.CommandType = CommandType.Text;
               
               
                        cmddel.ExecuteNonQuery();
                        PopulateGridViewSectionArchive();
                        secFrm.PopulateGridViewSection();


                        DateTime time = DateTime.Now;
                        string format = "yyyy-MM-dd";
                        SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", loginAct);
                        cm.Parameters.AddWithValue("@ActivityLog", loginAct + " recover a section from archive");
                        cm.ExecuteNonQuery();

                    }
                }
         //   }
         //   catch (Exception ex)
         //   {
          //      MessageBox.Show(ex.Message);
        //    }
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
                        SqlCommand cmddel = new SqlCommand("DELETE FROM SectionArchive_Tbl WHERE Course = @Course AND YearLevel = @YearLevel", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        cmddel.Parameters.AddWithValue("@YearLevel", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                        cmddel.ExecuteNonQuery();
                        MessageBox.Show("Deleted");
                        PopulateGridViewSectionArchive();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO ActivityLog_Tbl (Username,DateTime,ActivityLog) VALUES (@Username,@DateTime,@ActivityLog)", sqlcon);
                        cmd1.Parameters.AddWithValue("@Username", loginAct);
                        cmd1.Parameters.Add("@DateTime", SqlDbType.DateTime);
                        cmd1.Parameters["@DateTime"].Value = DateTime.Now;
                        cmd1.Parameters.AddWithValue("@ActivityLog", loginAct + " deleted a section from archive");
                        cmd1.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception    ex)
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
