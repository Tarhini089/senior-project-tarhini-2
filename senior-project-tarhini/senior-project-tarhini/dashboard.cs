using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace senior_project_tarhini
{
    public partial class dashboard : Form
    {

        private string connection;
        private readonly string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";

        private DateTime fromDate;
        private DateTime toDate;

        private DateTime fromDate1;
        private DateTime toDate1;

        private DateTime fromDate2;
        private DateTime toDate2;

        public dashboard()
        {
            InitializeComponent();
            LoadClientCount();
            LoadTotalPMT();
            LoadTotalExp();
            LoadAgentsCount();
            LoadAgentsSalary();
            LoadUnpaidBillsCount();
            LoadTotalDebts();
            LoadDriversSalary();
            LoadDriversCount();
            LoadItemsCount();
            LoadItemsPrices();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Agents Agents = new Agents();
            Agents.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            Clients.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        private void LoadClientCount()
        {
            int clientCount = GetClientCount();
            lblClients.Text = $"{clientCount}";
        }

        private int GetClientCount()
        {
            int count = 0;

            // SQL query to count rows in the clients table
            string query = "SELECT COUNT(*) FROM clients";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or show a message)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return count;
        }

        private void LoadTotalPMT()
        {
            decimal totalPMT = GetTotalPMT();
            lblTotalPMT.Text = $"{totalPMT:C}";
        }

        private decimal GetTotalPMT()
        {
            decimal total = 0;
            string query = "SELECT SUM(Total) FROM Daily_Bills";

            if (chkDateFilter.Checked)
            {
                fromDate = dateFromFilter.Value.Date;
                toDate = dateToFilter.Value.Date.AddDays(1); // Include the whole day of the end date
                query += $" WHERE date_time BETWEEN @FromDate AND @ToDate";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (chkDateFilter.Checked)
                {
                    // Use fromDate and toDate from the class level
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);
                }
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = (decimal)result;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }

        private void lblClients_Click(object sender, EventArgs e)
        {

        }

        private void lblTotalPMT_Click(object sender, EventArgs e)
        {

        }

        private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            LoadTotalPMT();
        }

        private void dateFromFilter_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter.Checked)
            {
                LoadTotalPMT();
            }
        }

        private void dateToFilter_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter.Checked)
            {
                LoadTotalPMT();
            }
        }

        private void LoadTotalExp()
        {
            decimal totalExp = GetTotalExp();
            lblTotalExp.Text = $"{totalExp:C}";
        }

        private decimal GetTotalExp()
        {
            decimal total = 0;
            string query = "SELECT SUM(price) FROM Expenses";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }


        private decimal GetTotalExpWithFilter()
        {
            decimal total = 0;
            string query = "SELECT SUM(price) FROM Expenses";

            if (chkDateFilter2.Checked)
            {
                fromDate1 = dateTimePicker1.Value.Date; // Remove "DateTime" type declaration
                toDate1 = dateTimePicker2.Value.Date.AddDays(1); // Remove "DateTime" type declaration

                // Validate selected dates
                if (fromDate1 < SqlDateTime.MinValue.Value || toDate1 > SqlDateTime.MaxValue.Value)
                {
                    MessageBox.Show("Selected date range is not valid.");
                    return total; // Return 0 if dates are invalid
                }

                query += $" WHERE date_time BETWEEN @FromDate1 AND @ToDate1";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (chkDateFilter2.Checked)
                {
                    command.Parameters.AddWithValue("@FromDate1", fromDate1);
                    command.Parameters.AddWithValue("@ToDate1", toDate1);
                }
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }

        private void LoadTotalExpFromTo()
        {
            if (chkDateFilter2.Checked)
            {
                decimal totalExp = GetTotalExpWithFilter();
                lblExpFromTo.Text = $"{totalExp:C}";
            }
            else
            {
                lblExpFromTo.Text = $"$0.00"; // Display 0 when filter is unchecked
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chkDateFilter2_CheckedChanged(object sender, EventArgs e)
        {
            LoadTotalExpFromTo();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter2.Checked)
            {
                LoadTotalExpFromTo();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter2.Checked)
            {
                LoadTotalExpFromTo();
            }
        }

        private void LoadAgentsCount()
        {
            int AgentsCount = GetAgentsCount();
            lblAgents.Text = $"{AgentsCount}";
        }

        private int GetAgentsCount()
        {
            int count = 0;

            // SQL query to count rows in the clients table
            string query = "SELECT COUNT(*) FROM Agents_info";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or show a message)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return count;
        }


        private void LoadAgentsSalary()
        {
            decimal Salery = GetLoadAgentsSalary();
            lblTotalSalaries.Text = $"{Salery:C}";
        }

        private decimal GetLoadAgentsSalary()
        {
            decimal total = 0;
            string query = "SELECT SUM(Salery) FROM Agents_Fin";

            if (chkDateFilter3.Checked)
            {
                fromDate2 = dateTimePicker3.Value.Date;
                toDate2 = dateTimePicker4.Value.Date.AddDays(1); // Include the whole day of the end date
                query += $" WHERE Date BETWEEN @FromDate2 AND @ToDate2";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                if (chkDateFilter3.Checked)
                {
                    // Use correct parameter names: @FromDate2 and @ToDate2
                    command.Parameters.AddWithValue("@FromDate2", fromDate2);
                    command.Parameters.AddWithValue("@ToDate2", toDate2);
                }
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }

        private void lblAgents_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter3.Checked)
            {
                LoadAgentsSalary();
            }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            if (chkDateFilter3.Checked)
            {
                LoadAgentsSalary();
            }
        }

        private void chkDateFilter3_CheckedChanged(object sender, EventArgs e)
        {
            LoadAgentsSalary();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }

        private void LoadUnpaidBillsCount()
        {
            int UnpaidBillsCount = GetUnpaidBillsCount();
            lblUnpaid.Text = $"{UnpaidBillsCount}";
        }

        private int GetUnpaidBillsCount()
        {
            int count = 0;

            // SQL query to count rows in the clients table
            string query = "SELECT COUNT(*) FROM Client_debts";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or show a message)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return count;
        }

        private void LoadTotalDebts()
        {
            decimal totalDebts = GetTotalDebts();
            lblTotal.Text = $"{totalDebts:C}";
        }

        private decimal GetTotalDebts()
        {
            decimal total = 0;
            string query = "SELECT SUM(Payment) FROM Client_debts";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }


        private void LoadDriversCount()
        {
            int DriversCount = GetDriversCount();
            lblDriver.Text = $"{DriversCount}";
        }

        private int GetDriversCount()
        {
            int count = 0;

            // SQL query to count rows in the clients table
            string query = "SELECT COUNT(*) FROM delrviry_driver";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or show a message)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return count;
        }

        private void LoadDriversSalary()
        {
            decimal DriversSalary = GetTotalSalary();
            lblTotalDriversSalary.Text = $"{DriversSalary:C}";
        }

        private decimal GetTotalSalary()
        {
            decimal total = 0;
            string query = "SELECT SUM(salary) FROM delrviry_driver";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }

        ////////////////////////////////////////
        ///
        private void LoadItemsCount()
        {
            int ItemsCount = GetItemsCount();
            lblItems.Text = $"{ItemsCount}";
        }

        private int GetItemsCount()
        {
            int count = 0;

            // SQL query to count rows in the clients table
            string query = "SELECT COUNT(*) FROM Items";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    count = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it or show a message)
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return count;
        }

        private void LoadItemsPrices()
        {
            decimal ItemsPrices = GetItemsPrices();
            lblItemsTotal.Text = $"{ItemsPrices:C}";
        }

        private decimal GetItemsPrices()
        {
            decimal total = 0;
            string query = "SELECT SUM(price) FROM Items";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDecimal(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }

            return total;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
