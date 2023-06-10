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
    public partial class Login : Form
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
        public Login()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btnLOGIN_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();

                    string query = "select Username,Password FROM User_Tbl WHERE Username='" + txtUSERNAME.Text.Trim() + "' AND Password='" + txtPASSWORD.Text.Trim() + "'";
                    SqlDataAdapter da = new SqlDataAdapter(query, sqlcon);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                     //   MessageBox.Show("Welcome to Automatic Schedule Loader");
                        Main main = new Main(this);
                        DateTime time = DateTime.Now;      
                        string format = "yyyy-MM-dd"; 
                        SqlCommand cm = new SqlCommand("insert into LoginActivity_Tbl(Username,DateTime) values(@Username,'" + time.ToString(format) + "')", sqlcon);
                        cm.Parameters.AddWithValue("@Username", txtUSERNAME.Text);
                        cm.ExecuteNonQuery();
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        label5.Text = "Incorrect Username or Password";
                        label5.Visible = true;
                        label5.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnEXIT_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterAcccs ra = new RegisterAcccs();
            ra.Show();
            this.Hide();
            
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtPASSWORD.UseSystemPasswordChar = false;
           
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                txtPASSWORD.UseSystemPasswordChar = true;
            }
            else
            {
                txtPASSWORD.UseSystemPasswordChar = false;
            }
        }
    }
}
