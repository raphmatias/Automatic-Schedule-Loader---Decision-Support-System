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
namespace AutomaticScheduleLoader
{
    
    public partial class PassConfirm : Form
    {
      //  string conn = @"Data Source=DESKTOP-5PR1LSN;Initial Catalog=ScheduleLoaderDB;Persist Security Info=False;User ID=sa;Password=***********; Integrated Security=SSPI";
        AccManagement AMForm;
        public PassConfirm(AccManagement AM)
        {
            InitializeComponent();
            this.AMForm = AM;
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            if(AMForm.MainAddminPass == textBox1.Text)
            {
               AMForm.txtboxPassCU.UseSystemPasswordChar = false;
                this.Close();

            }
        }

        private void PassConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AMForm.MainAddminPass == textBox1.Text)
            {
                AMForm.txtboxPassCU.UseSystemPasswordChar = false;
                AMForm.txtboxPassCU.Enabled = true;
            }
            else
            {
                AMForm.checkBox1.Checked = false;
                AMForm.txtboxPassCU.UseSystemPasswordChar = true;
            }
            if(AMForm.mainAdmin == "Main Admin")
            {
                AMForm.btnDel.Enabled = false;
            }
        }

        private void PassConfirm_Load(object sender, EventArgs e)
        {
           
        }
    }
}
