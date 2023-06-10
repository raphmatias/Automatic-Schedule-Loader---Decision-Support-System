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
    public partial class RegisterAcccs : Form
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
        public RegisterAcccs()
        {
            InitializeComponent();
             this.FormBorderStyle = FormBorderStyle.None;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void RegisterAcccs_Load(object sender, EventArgs e)
        {
            btnReg.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReg.Width,
        btnReg.Height, 30, 30));
          
        }

        private void btnEXIT_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }
        public void UserCheck()
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(conn))
                {
                    sqlcon.Open();
                    string query1 = "select Username FROM User_Tbl WHERE Username='" + txtUSERNAME.Text + "'";
                    SqlCommand command1 = new SqlCommand(query1, sqlcon);
                    SqlDataReader reader1 = command1.ExecuteReader();

                    if (reader1.Read() == true)
                    {



                        user = reader1["Username"].ToString();

                    }
                    reader1.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }

        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                UserCheck();
                if(txtUSERNAME.Text == ""&& textBox1.Text == ""&& textBox2.Text == "")
                {
                    MessageBox.Show("You cant leave spaces blank", "Cant leave spaces blank", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (txtUSERNAME.Text != "" && textBox1.Text == "" && textBox2.Text == "")
                {
                    MessageBox.Show("You cant leave spaces blank", "Cant leave spaces blank", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (txtUSERNAME.Text != "" && textBox1.Text != "" && textBox2.Text == "")
                {
                    MessageBox.Show("You cant leave spaces blank", "Cant leave spaces blank", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (txtUSERNAME.Text != "" && textBox1.Text == "" && textBox2.Text != "")
                {
                    MessageBox.Show("You cant leave spaces blank", "Cant leave spaces blank", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (txtUSERNAME.Text == user)
                {
                    MessageBox.Show("A username with that username already exist","Username Exist",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
             
                else
                {
                    if (textBox1.Text != textBox2.Text)
                    {
                        MessageBox.Show("Your password does not match", "Passwrod", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (textBox1.Text.Length <= 5)
                    {
                        MessageBox.Show("Your password must be 6 characters long", "Passwrod", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        using (SqlConnection sqlcon = new SqlConnection(conn))
                        {
                            sqlcon.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO User_Tbl (Username,Password,UserType) VALUES (@Username,@Password,@UserType)", sqlcon);
                            cmd.Parameters.AddWithValue("@Username", txtUSERNAME.Text);
                            cmd.Parameters.AddWithValue("@Password", textBox1.Text);
                            cmd.Parameters.AddWithValue("@UserType", "1");
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Succesfully registered","Registered",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            this.Close();
                            Login login = new Login();
                            login.Show();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox1.UseSystemPasswordChar = true;
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                textBox1.UseSystemPasswordChar = false;
                textBox2.UseSystemPasswordChar = false;
            }
        }
    }
}
