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
    public partial class Cancellation : Form
    {
        public Cancellation()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            OleDbCommand myCmd = new OleDbCommand();
            myCmd.Connection = connect;
            myCmd.CommandText = "DELETE FROM Reservation WHERE Booking_No = @delete";

            double num = 0;
            if (!(double.TryParse(textBox2.Text, out num)))
            {
                MessageBox.Show("Error: Please enter only numbers");
                return;
            }
            else
            {
                myCmd.Parameters.Add("@delete", OleDbType.Char).Value = textBox2.Text;
            }            

            connect.Open();
            int rowsChanged = myCmd.ExecuteNonQuery();

            if (rowsChanged == 1)
            {
                MessageBox.Show("Booking Cancelled");
                myCmd.CommandText = "INSERT INTO Cancellation(Booking_No, [Cancellation_Date])" + "VALUES(@booking_No, @cancelDate)";

                myCmd.Parameters.Add("@cancelDate", OleDbType.DBDate).Value = DateTime.Today;
                myCmd.Parameters.Add("@booking_No", OleDbType.Char).Value = textBox2.Text;

                myCmd.ExecuteNonQuery();
                connect.Close();
            }
            else
            {
                MessageBox.Show("Error: Booking Not Cancelled");
            }
        }

        private void Cancelation_Load(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            OleDbCommand myCmd = new OleDbCommand();
            myCmd.Connection = connect;

            // view Booking table
            DataTable dt2 = new DataTable();
            OleDbDataAdapter sda2 = new OleDbDataAdapter("SELECT * from Booking", connect);
            sda2.Fill(dt2);
            dataGridView1.DataSource = dt2;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Sort(dataGridView1.Columns["Booking_No"], ListSortDirection.Ascending);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }
    }
}
