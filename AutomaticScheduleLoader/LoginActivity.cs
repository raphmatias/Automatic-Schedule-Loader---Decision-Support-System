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
    public partial class LoginActivity : Form
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

        public LoginActivity()
        {
            InitializeComponent();
        }
        public void PopulateGridViewLoginActivty()
        {
            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM LoginActivity_Tbl", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.EnableHeadersVisualStyles = false;

                dataGridView1.AllowUserToAddRows = false;
               this.dataGridView1.Columns[0].Width = 170;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[0].HeaderText = "User Activity ID";
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                this.dataGridView1.Columns[0].ReadOnly = true;
                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[2].HeaderText = "Date";
            }

        }

        private void LoginActivity_Load(object sender, EventArgs e)
        {
            PopulateGridViewLoginActivty();
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.Rows[0].Selected = false;
            }
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
          dateTimePicker1.CustomFormat = "yyyy-MM-dd";
           dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            string date1 = dateTimePicker1.Text.ToString();
            btnClose.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClose.Width,
           btnClose.Height, 30, 30));
            dataGridView1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dataGridView1.Width,
dataGridView1.Height, 5, 5));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string format = "yyyy-MM-dd";



            using (SqlConnection sqlcon = new SqlConnection(conn))
            {
                sqlcon.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM LoginActivity_Tbl where DateTime between '" + dateTimePicker1.Value.ToString(format) + "' and '" + dateTimePicker2.Value.ToString(format) + "'", conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.EnableHeadersVisualStyles = false;
                dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.Columns[0].Width = 170;
                this.dataGridView1.Columns[1].Width = 170;
                this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[0].HeaderText = "User Activity ID";
                dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Descending);
                this.dataGridView1.Columns[0].ReadOnly = true;
                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[2].HeaderText = "Date";
            }
            
           
        }
    }
}
