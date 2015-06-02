using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Hotel_Reservation_System
{
    public partial class Guest : Form
    {
        public Guest()
        {
            InitializeComponent();
        }

        private void firstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            OleDbCommand myCmd = new OleDbCommand();
            myCmd.Connection = connect;
            myCmd.CommandText = "INSERT INTO Guest(Guest_First_Name, Guest_Surname, Guest_Address, Guest_Postcode, Guest_Telephone, [Guest_E-mail])" + "VALUES(@Fname, @Sname, @Address, @Postcode, @Telephone, @Email)";

            // input validation
            double num = 0;
            if ((double.TryParse(firstName.Text, out num)))
            {
                MessageBox.Show("Error: Please enter only letters in the First Name field");
                return;
            }
            else
            {
                myCmd.Parameters.Add("@Fname", OleDbType.Char).Value = firstName.Text;
            }

            if ((double.TryParse(surname.Text, out num)))
            {
                MessageBox.Show("Error: Please enter only letters in the Surname field");
                return;
            }
            else
            {
                myCmd.Parameters.Add("@Sname", OleDbType.Char).Value = surname.Text;
            }

            myCmd.Parameters.Add("@Address", OleDbType.Char).Value = address.Text;
            myCmd.Parameters.Add("@Postcode", OleDbType.Char).Value = postcode.Text;

            if (!(double.TryParse(telephone.Text, out num)))
            {
                MessageBox.Show("Error: Please enter only numbers in the Telephone field");
                return;
            }
            else
            {
                myCmd.Parameters.Add("@Telephone", OleDbType.Char).Value = telephone.Text;
            }

            myCmd.Parameters.Add("@Email", OleDbType.Char).Value = email.Text;

            connect.Open();
            int rowsChanged = myCmd.ExecuteNonQuery();
            connect.Close();

            if (rowsChanged == 1)
            {
                MessageBox.Show("Guest Inserted");

                // once record is inserted hide the current form and display the home form
                Home();
            }
            else
            {
                MessageBox.Show("Error: Guest Not Inserted");
            }
        }

        public void Home()
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home();
        }

        private void Guest_Load(object sender, EventArgs e)
        {

        }
    }
}
