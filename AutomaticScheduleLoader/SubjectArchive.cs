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
    public partial class SubjectArchive : Form
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
        frmSubj frmSb;
        string loginAct = "";
        string typeofAcc = "";
        public SubjectArchive(frmSubj FS)
        {
            InitializeComponent();
            this.frmSb = FS;
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
            }catch(Exception ex){
                MessageBox.Show(ex.Message);
            }
        }
        void PopulateSubjArchive() // populate all in gridview faculty
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM SubjectArchive_Tbl", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    this.dataGridView1.Columns[1].Width = 140;
                    this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    this.dataGridView1.Columns[3].Width = 100;
                    this.dataGridView1.Columns[4].Width = 100;
                    this.dataGridView1.Columns[5].Width = 120;
                    this.dataGridView1.Columns[6].Width = 120;
                    this.dataGridView1.Columns[7].Width = 130;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.Columns["ID"].Visible = false;
                    dataGridView1.Columns["SubjectSlot"].Visible = false;
                    dataGridView1.Columns["RoomCategory"].Visible = false;
                    dataGridView1.Columns["SubjectCode"].HeaderText = "Subject Code";
                    dataGridView1.Columns["SubjectName"].HeaderText = "Subject Name";
                    dataGridView1.Columns["CredUnitLec"].HeaderText = "Unit (Lec)";
                    dataGridView1.Columns["CredUnitLab"].HeaderText = "Unit (Lab)";
                    dataGridView1.Columns["ContHrsLec"].HeaderText = "Hours (Lec)";
                    dataGridView1.Columns["ContHrsLab"].HeaderText = "Hours (Lab)";
                    dataGridView1.Columns["YearLevel"].HeaderText = "Year Level";

                    dataGridView1.Columns["SubjectCode"].ReadOnly = true;
                    dataGridView1.Columns["SubjectName"].ReadOnly = true;
                    dataGridView1.Columns["CredUnitLec"].ReadOnly = true;
                    dataGridView1.Columns["CredUnitLab"].ReadOnly = true;
                    dataGridView1.Columns["ContHrsLec"].ReadOnly = true;
                    dataGridView1.Columns["ContHrsLab"].ReadOnly = true;
                    dataGridView1.Columns["YearLevel"].ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SubjectArchive_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateSubjArchive();
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
            if (dataGridView1.Rows.Count != 0)
            {

                try
                {
                    DialogResult dr = MessageBox.Show("Do you really want to recover this data?", "Recover", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO Subject_Tbl (SubjectCode,SubjectName,CredUnitLec,CredUnitLab,ContHrsLec,ContHrsLab,Semester,Course,YearLevel,Section,SubjectSlot,RoomCategory) VALUES (@SubjectCode,@SubjectName,@CredUnitLec,@CredUnitLab,@ContHrsLec,@ContHrsLab,@Semester,@Course,@YearLevel,@Section,@SubjectSlot,@RoomCategory)", sqlcon);
                            cmd.Parameters.AddWithValue("@SubjectCode", dataGridView1.CurrentRow.Cells[1].Value.ToString());
                            cmd.Parameters.AddWithValue("@SubjectName", dataGridView1.CurrentRow.Cells[2].Value.ToString());
                            cmd.Parameters.AddWithValue("@CredUnitLec", dataGridView1.CurrentRow.Cells[3].Value.ToString());
                            cmd.Parameters.AddWithValue("@CredUnitLab", dataGridView1.CurrentRow.Cells[4].Value.ToString());
                            cmd.Parameters.AddWithValue("@ContHrsLec", dataGridView1.CurrentRow.Cells[5].Value.ToString());
                            cmd.Parameters.AddWithValue("@ContHrsLab", dataGridView1.CurrentRow.Cells[6].Value.ToString());
                            cmd.Parameters.AddWithValue("@Semester", dataGridView1.CurrentRow.Cells[7].Value.ToString());
                            cmd.Parameters.AddWithValue("@Course", dataGridView1.CurrentRow.Cells[8].Value.ToString());
                            cmd.Parameters.AddWithValue("@YearLevel", dataGridView1.CurrentRow.Cells[9].Value.ToString());
                            cmd.Parameters.AddWithValue("@Section", dataGridView1.CurrentRow.Cells[10].Value.ToString());
                            cmd.Parameters.AddWithValue("@SubjectSlot", dataGridView1.CurrentRow.Cells[11].Value.ToString());
                            cmd.Parameters.AddWithValue("@RoomCategory", dataGridView1.CurrentRow.Cells[12].Value.ToString());
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Recovered","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            SqlCommand cmddel = new SqlCommand("DELETE FROM SubjectArchive_Tbl WHERE ID = @ID", sqlcon);
                            cmddel.CommandType = CommandType.Text;
                            cmddel.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString()));
                            cmddel.ExecuteNonQuery();
                            PopulateSubjArchive();
                            frmSb.PopulateGridViewSubject();

                            DateTime time = DateTime.Now;
                            string format = "yyyy-MM-dd";
                            SqlCommand cm = new SqlCommand("insert into ActivityLog_Tbl(Username,ActivityLog,DateTime) values(@Username,@ActivityLog,'" + time.ToString(format) + "')", sqlcon);
                            cm.Parameters.AddWithValue("@Username", loginAct);
                            cm.Parameters.AddWithValue("@ActivityLog", loginAct + " recover a subject from archive");
                            cm.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
                        SqlCommand cmddel = new SqlCommand("DELETE FROM SubjectArchive_Tbl WHERE SubjectCode = @SubjectCode", sqlcon);
                        cmddel.CommandType = CommandType.Text;
                        cmddel.Parameters.AddWithValue("@SubjectCode", dataGridView1.CurrentRow.Cells[0].Value.ToString());
                        cmddel.ExecuteNonQuery();
                        MessageBox.Show("Deleted");
                        PopulateSubjArchive();
                        SqlCommand cmd1 = new SqlCommand("INSERT INTO ActivityLog_Tbl (Username,DateTime,ActivityLog) VALUES (@Username,@DateTime,@ActivityLog)", sqlcon);
                        cmd1.Parameters.AddWithValue("@Username", loginAct);
                        cmd1.Parameters.Add("@DateTime", SqlDbType.DateTime);
                        cmd1.Parameters["@DateTime"].Value = DateTime.Now;
                        cmd1.Parameters.AddWithValue("@ActivityLog", loginAct + " deleted a subject from archive");
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
