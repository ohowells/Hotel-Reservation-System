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
    public partial class Reservation : Form
    {
        private static int _curBookingNumber;

        public static int GetBookingNumber()
        {
            return _curBookingNumber;
        }

        public Reservation()
        {
            InitializeComponent();
            _curBookingNumber = 0;
            button2.Hide();
        }

        private void Reservation_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");
            OleDbCommand myCmd = new OleDbCommand();
            OleDbCommand myC = new OleDbCommand();
            myCmd.Connection = connect;
            myC.Connection = connect;

            DateTime ArrivalDate = dateTimePicker1.Value.Date;
            DateTime DepartureDate = dateTimePicker2.Value.Date;

            connect.Open();

            if (_curBookingNumber == 0)
            {
                myCmd.CommandText = "INSERT INTO Booking(Guest_No, [Arrival_Date], [Departure_Date], [Booking_Received])" + "VALUES(@GuestNo, @ArrivalDate, @DepartureDate, @BookingReceived)";

                double num = 0;
                if (!(double.TryParse(textBox1.Text, out num)))
                {
                    MessageBox.Show("Error: Please enter only a number in the Guest Number field");
                    return;
                }
                else
                {
                    myCmd.Parameters.Add("@GuestNo", OleDbType.Integer).Value = textBox1.Text;
                }

                myCmd.Parameters.Add("@ArrivalDate", OleDbType.DBDate).Value = ArrivalDate;
                myCmd.Parameters.Add("@DepartureDate", OleDbType.DBDate).Value = DepartureDate;
                myCmd.Parameters.Add("@BookingReceived", OleDbType.DBDate).Value = DateTime.Today;

                myCmd.ExecuteNonQuery();
                myCmd.CommandText = "SELECT @@Identity"; // retrieves the recently inserted primary key in Booking
                _curBookingNumber = Convert.ToInt32(myCmd.ExecuteScalar()); // assigns this value to _curBookingNumber to be used in the Reservation query   
            }

            myC.CommandText = "INSERT INTO Reservation(Booking_No, Room_No, Hotel_No)" + "VALUES(@BookingNo, @RoomNo, @HotelNo)";
            myC.Parameters.Add("@BookingNo", OleDbType.Char).Value = _curBookingNumber;
            myC.Parameters.Add("@RoomNo", OleDbType.Char).Value = comboBox4.SelectedItem;
            myC.Parameters.Add("@HotelNo", OleDbType.Char).Value = comboBox1.SelectedItem;

            int rowsChanged = myC.ExecuteNonQuery();

            connect.Close();

            if (rowsChanged == 1)
            {
                MessageBox.Show("Booking Made");
                GetBookingNumber();
                button2.Show(); // make the 'Next' button avaialble to click to proceed to Payment
            }
            else
            {
                MessageBox.Show("Error: Booking Not Made");
            }
            // grey out Guest_No and date fields to prevent user trying to insert different values which could conflict with the booking process
            textBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            // reset these fields so that the user can choose a different hotel and room to reserve
            comboBox1.Text = string.Empty;
            comboBox4.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            string sql = "SELECT * FROM Reservation WHERE Hotel_No = " + comboBox1.Text;
            DataTable dtReservations = new DataTable();
            OleDbDataAdapter dA = new OleDbDataAdapter(sql, connect);
            dA.Fill(dtReservations);

            string sql2 = "SELECT Room_No FROM Room WHERE Hotel_No = " + comboBox1.Text;
            DataTable dtRooms = new DataTable();
            OleDbDataAdapter dA2 = new OleDbDataAdapter(sql2, connect);
            dA2.Fill(dtRooms);

            comboBox4.Items.Clear();
            for (int i = 0; i < dtRooms.Rows.Count; i++)
            {
                comboBox4.Items.Add(dtRooms.Rows[i][0]);
            }
            comboBox4.Sorted = true;

            for (int i = 0; i < dtReservations.Rows.Count; i++)
            {
                int bookID = (int)dtReservations.Rows[i][1];
                int roomNo = (int)dtReservations.Rows[i][0];

                OleDbCommand myCmd = new OleDbCommand();
                myCmd.CommandText = "SELECT Arrival_Date, Departure_Date FROM Booking"
                                     + " WHERE Booking_No =" + bookID.ToString();
                myCmd.Connection = connect;

                connect.Open();
                OleDbDataReader myReader = myCmd.ExecuteReader();

                while (myReader.Read())
                {
                    DateTime startBook = (DateTime)myReader[0];
                    DateTime endBook = (DateTime)myReader[1];
                    if ((dateTimePicker1.Value >= startBook && dateTimePicker1.Value <= endBook) || 
                        (dateTimePicker2.Value >= startBook && dateTimePicker2.Value <= endBook))
                    {
                        comboBox4.Items.Remove(roomNo);
                    }
                    comboBox4.DroppedDown = true;
                }
                connect.Close();
            }
        }

        private void Reservation_Load_1(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            OleDbCommand myCmd = new OleDbCommand();
            myCmd.Connection = connect;

            // display Guest table
            DataTable dt = new DataTable();
            OleDbDataAdapter sda = new OleDbDataAdapter("SELECT * from Guest", connect);
            sda.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Sort(dataGridView1.Columns["Guest_No"], ListSortDirection.Ascending);
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            this.Hide();
            payment.Show();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Hide();
            home.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
