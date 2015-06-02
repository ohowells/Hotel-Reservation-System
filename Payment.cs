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
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
                                      @"\TeamProjectTeam5.accdb; Persist Security Info=False");

            OleDbCommand myCmd = new OleDbCommand();
            myCmd.Connection = connect;

            DateTime expiryDate = dateTimePicker4.Value.Date;
            DateTime datePayment = dateTimePicker3.Value.Date;

            myCmd.CommandText = "INSERT INTO Payment(Booking_No, Cash, Card_Type, Card_Number, [Card_Expiry], Card_Security_Code, [Payment_Date])" + "VALUES(@BookingNo, @cash, @cardType, @cardNumber, @cardExpiry, @cardSecurity, @paymentDate)";
            myCmd.Parameters.Add("@BookingNo", OleDbType.Integer).Value = Reservation.GetBookingNumber(); // get the current booking number from the reservation form 

            if (checkBox1.Checked)
            {
                myCmd.Parameters.Add("@cash", OleDbType.Boolean).Value = checkBox1.Checked;
                myCmd.Parameters.Add("@cardType", OleDbType.Char).Value = DBNull.Value;
                myCmd.Parameters.Add("@cardNumber", OleDbType.Char).Value = DBNull.Value;
                myCmd.Parameters.Add("@cardExpiry", OleDbType.DBDate).Value = DBNull.Value;
                myCmd.Parameters.Add("@cardSecurity", OleDbType.Char).Value = DBNull.Value;
                myCmd.Parameters.Add("@paymentDate", OleDbType.DBDate).Value = datePayment;
            }
            else
            {
                myCmd.Parameters.Add("@cash", OleDbType.Boolean).Value = checkBox1.Checked;
                myCmd.Parameters.Add("@cardType", OleDbType.Char).Value = comboBox3.SelectedItem;
                // input validation
                double num = 0;
                if (!(double.TryParse(textBox5.Text, out num)))
                {
                    MessageBox.Show("Error: Please enter only numbers in the Card Number field");
                    return;
                }
                else
                {
                    myCmd.Parameters.Add("@cardNumber", OleDbType.Char).Value = textBox5.Text;
                }
            
                if (dateTimePicker4.Value < DateTime.Today) // check if expiry date is on or after current date
                {
                    MessageBox.Show("Expiry date is invalid");
                }
                else
                {
                    myCmd.Parameters.Add("@cardExpiry", OleDbType.DBDate).Value = expiryDate;
                }

                if (!(double.TryParse(textBox7.Text, out num)))
                {
                    MessageBox.Show("Error: Please enter only numbers in the Card Security Code field");
                    return;
                }
                else
                {
                    myCmd.Parameters.Add("@cardSecurity", OleDbType.Char).Value = textBox7.Text;
                }
                
                myCmd.Parameters.Add("@paymentDate", OleDbType.DBDate).Value = datePayment;
            }

            connect.Open();
            int rowsChanged = myCmd.ExecuteNonQuery();
            connect.Close();

            if (rowsChanged == 1)
            {
                MessageBox.Show("Record Inserted");
                Home home = new Home();
                this.Hide();
                home.Show();
            }
            else
            {
                MessageBox.Show("Error: Record Not Inserted");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // grey out text boxes if cash tick box is checked
                comboBox3.Enabled = false;
                textBox5.Enabled = false;
                dateTimePicker4.Enabled = false;
                textBox7.Enabled = false;

                // if data has been typed into below fields and tick box is then checked, remove data typed.
                comboBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                dateTimePicker4.CustomFormat = " ";
                dateTimePicker4.Format = DateTimePickerFormat.Custom;
                textBox7.Text = string.Empty;
            }
            else
            {
                // ungrey out text boxes if cash tick box is unchecked
                comboBox3.Enabled = true;
                textBox5.Enabled = true;
                dateTimePicker4.Enabled = true;
                dateTimePicker4.CustomFormat = "dd MMMM yyyy";
                dateTimePicker4.Format = DateTimePickerFormat.Custom;
                textBox7.Enabled = true;
            }
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            //OleDbConnection connect = new OleDbConnection();
            //connect.ConnectionString = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath +
            //                          @"\TeamProjectTeam5.accdb; Persist Security Info=False");
            //OleDbCommand myCmd = new OleDbCommand();
            //myCmd.Connection = connect;
            //OleDbCommand myCmd2 = new OleDbCommand();
            //myCmd2.Connection = connect;
            //OleDbCommand myCmd3 = new OleDbCommand();
            //myCmd3.Connection = connect;

            //myCmd.CommandText = "SELECT Arrival_Date, Departure_Date FROM Booking Where Booking_No=@ID";
            //myCmd.Parameters.Add("@ID", OleDbType.Integer).Value = Reservation.GetBookingNumber();

            //string query = "SELECT Room_No, Room_Rate FROM Room R,Hotel H WHERE H.Hotel_No = R.Hotel_No";
            //myCmd2.CommandText = query;

            //string query2 = "SELECT  B.Booking_No FROM Booking AS B WHERE B.Booking_No IN (SELECT B.Booking_No FROM Reservation Re, Booking B, Room R WHERE B.Booking_No = Re.Booking_No AND R.Room_No = Re.Room_No)";
            //myCmd3.CommandText = query2;

            //connect.Open();
            //OleDbDataReader dR = myCmd.ExecuteReader();
            //OleDbDataReader dR2 = myCmd2.ExecuteReader();
            //OleDbDataReader dR3 = myCmd2.ExecuteReader();
            //dR.Read();
            //dR2.Read();
            //dR3.Read();

            //DateTime ArrivalDate = (DateTime)dR[0];
            //DateTime DepartureDate = (DateTime)dR[1];

            //int noDays = (DepartureDate - ArrivalDate).Days;
            //int RoomRate = Convert.ToInt32(query);



            //connect.Close();
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "MM/yyyy";
        }
    }
}
