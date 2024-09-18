using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace senior_project_tarhini
{
    public partial class Payments : Form
    {
        private bool sidebarExpand = true;
        private string connection;
        private readonly string connectionString = "Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True";
        public Payments()
        {
            InitializeComponent();

            dataGridView3.RowsAdded += DataGridView3_RowsAdded;
            dataGridView3.RowsRemoved += DataGridView3_RowsRemoved;

            // Calculate and set initial total sum
            UpdateTotalSum();


            PopulateclientFilterComboBox();

            cboNameFilter.SelectedIndexChanged += cboNameFilter_SelectedIndexChanged;


            timer1.Enabled = true; // Enable the Timer
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += timer1_Tick;


        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            timerSidebar1.Start();

        }

        private void timerSidebar1_Tick(object sender, EventArgs e)
        {
            /*if (sidebarExpand)
            {
                Sidebarpannel.Width -= 10;
                if (Sidebarpannel.Width == Sidebarpannel.MinimumSize.Width)
                {
                    sidebarExpand = false;

                    timerSidebar1.Stop();
                }
            }
            else
            {
                Sidebarpannel.Width += 10;
                if (Sidebarpannel.Width == Sidebarpannel.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    timerSidebar1.Stop();
                }
            }*/
        }

        private void Sidebarpannel_Paint(object sender, PaintEventArgs e)
        {

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

        private void PopulateclientFilterComboBox()
        {
            cboNameFilter.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT client_name FROM clients";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clientName = (string)reader["client_name"];
                            cboNameFilter.Items.Add(clientName);
                        }
                    }
                }
            }
        }

        private void UpdatePaymentInformation(string selectedClientName)
        {
            decimal totalAmountPaid = 0;
            decimal totalRemainingAmount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    // Calculate total amount paid
                    command.CommandText = "SELECT SUM(Payment) FROM Client_Payment WHERE C_Name = @clientName";
                    command.Parameters.AddWithValue("@clientName", selectedClientName);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalAmountPaid = Convert.ToDecimal(result);
                    }

                    // Calculate total remaining amount
                    command.CommandText = "SELECT SUM(Payment) FROM Client_debts WHERE C_name = @clientName";
                    result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalRemainingAmount = Convert.ToDecimal(result);
                    }
                }
            }

            // Update text boxes
            txtAmountPaid.Text = totalAmountPaid.ToString();
            txtRemainingAmount.Text = totalRemainingAmount.ToString();
            txtTotalAmount.Text = (totalAmountPaid + totalRemainingAmount).ToString();
            /* // Calculate sum of payments in Client_Payment table
             decimal totalAmountPaid = 0;
             decimal totalRemainingAmount = 0;
             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 connection.Open();
                 using (SqlCommand command = connection.CreateCommand())
                 {
                     // Calculate total amount paid
                     command.CommandText = "SELECT SUM(Payment) FROM Client_Payment WHERE C_Name = @clientName";
                     command.Parameters.AddWithValue("@clientName", selectedClientName);
                     object result = command.ExecuteScalar();
                     if (result != DBNull.Value)
                     {
                         totalAmountPaid = Convert.ToDecimal(result);
                     }

                     // Calculate total remaining amount
                     command.CommandText = "SELECT SUM(Payment) FROM Client_debts WHERE C_name = @clientName";
                     result = command.ExecuteScalar();
                     if (result != DBNull.Value)
                     {
                         totalRemainingAmount = Convert.ToDecimal(result);
                     }
                 }
             }

             // Update text boxes
             txtAmountPaid.Text = totalAmountPaid.ToString();
             txtRemainingAmount.Text = totalRemainingAmount.ToString();
             txtTotalAmount.Text = (totalAmountPaid + totalRemainingAmount).ToString();*/
        }

        private string selectedClientName;




        private void cboNameFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboNameFilter.SelectedItem != null)
            {
                selectedClientName = cboNameFilter.SelectedItem.ToString();

                // Check if the DataSource for dataGridView1 is not null and is a DataTable
                if (dataGridView1.DataSource is DataTable dataTable1)
                {
                    dataTable1.DefaultView.RowFilter = $"C_name = '{selectedClientName}'";
                }
                else
                {
                    MessageBox.Show("DataSource for dataGridView1 is not set or is not a DataTable.");
                }

                // Check if the DataSource for dataGridView2 is not null and is a DataTable
                if (dataGridView2.DataSource is DataTable dataTable2)
                {
                    dataTable2.DefaultView.RowFilter = $"c_name = '{selectedClientName}'";
                }
                else
                {
                    MessageBox.Show("DataSource for dataGridView2 is not set or is not a DataTable.");
                }

                // Update payment information
                UpdatePaymentInformation(selectedClientName);
            }
            
            /*selectedClientName = cboNameFilter.SelectedItem.ToString();

            // Filter dataGridView1
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"C_name = '{selectedClientName}'";

            // Filter dataGridView2
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"c_name = '{selectedClientName}'";

            // Update payment information
            UpdatePaymentInformation(selectedClientName);*/

            ///////////////////////////////////////////////

            /*string selectedClientName = cboNameFilter.SelectedItem.ToString();

            // Filter dataGridView1
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"C_name = '{selectedClientName}'";

            // Filter dataGridView2
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"c_name = '{selectedClientName}'";

            // Calculate sum of payments in Client_Payment table
            decimal totalAmountPaid = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT SUM(Payment) FROM Client_Payment WHERE C_Name = @clientName";
                    command.Parameters.AddWithValue("@clientName", selectedClientName);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalAmountPaid = Convert.ToDecimal(result);
                    }
                }
            }

            // Calculate sum of payments in Client_debts table
            decimal totalRemainingAmount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT SUM(Payment) FROM Client_debts WHERE C_name = @clientName";
                    command.Parameters.AddWithValue("@clientName", selectedClientName);
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        totalRemainingAmount = Convert.ToDecimal(result);
                    }
                }
            }

            // Update text boxes
            txtAmountPaid.Text = totalAmountPaid.ToString();
            txtRemainingAmount.Text = totalRemainingAmount.ToString();
            txtTotalAmount.Text = (totalAmountPaid + totalRemainingAmount).ToString();
            */
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Client_Payment", conn))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "t0");
                        dataGridView1.DataSource = ds.Tables["t0"];


                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Client_debts", conn))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "t0");
                        dataGridView2.DataSource = ds.Tables["t0"];


                        foreach (DataGridViewColumn column in dataGridView2.Columns)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
              }

            foreach (DataGridViewColumn column in dataGridView3.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Agents Agents = new Agents();
            Agents.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            Clients.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];

                txtName.Text = selectedRow.Cells["C_name"].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells["Bill_Date"].Value.ToString());
                txtPayment.Text = selectedRow.Cells["Payment"].Value.ToString();              
            }
        }
        private void LoadDataIntoDataGridView()
        {
            string filter = cboNameFilter.SelectedItem?.ToString();

            string query = "SELECT * FROM Client_Payment";
            if (!string.IsNullOrEmpty(filter))
            {
                query += " WHERE C_name = @clientName";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    if (!string.IsNullOrEmpty(filter))
                    {
                        command.Parameters.AddWithValue("@clientName", filter);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }

            /* using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Client_Payment"; // Replace Your_Table_Name with the actual table name
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }*/

            // Load data into dataGridView2
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Client_debts"; // Replace Your_Other_Table_Name with the actual table name
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView2.DataSource = dataTable;
                }
            }

        }

        /*private void LoadDataIntoDataGridView(DataGridView dataGridView, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {tableName}";
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
            }
        }*/

       
        private void btnPay_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        foreach (DataGridViewRow row in dataGridView3.Rows)
                        {
                            if (row.Cells["c_name"].Value != null &&
                                row.Cells["d_t"].Value != null &&
                                row.Cells["Payment"].Value != null)
                            {
                                // Insert into Client_Payment table
                                command.CommandText = "INSERT INTO Client_Payment (C_name, payment_date, payment) " +
                                                      "VALUES (@C_name, @payment_date, @payment)";

                                // Add parameter values for Client_Payment
                                command.Parameters.Clear(); // Clear parameters from previous iteration
                                command.Parameters.AddWithValue("@C_name", row.Cells["c_name"].Value.ToString());
                                command.Parameters.AddWithValue("@payment_date", Convert.ToDateTime(row.Cells["d_t"].Value));
                                command.Parameters.AddWithValue("@payment", row.Cells["payment"].Value.ToString());

                                command.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Data saved successfully");

                        // Optionally, reload the data into dataGridView1 to reflect changes in the database
                        LoadDataIntoDataGridView();

                        // Reapply the filter after payment
                        cboNameFilter_SelectedIndexChanged(sender, e);
                    }
                }

                // Clear dataGridView3 after saving data
                dataGridView3.Rows.Clear();

                // Clear input controls and set values
                txtName.Clear();
                dateTimePicker1.Value = DateTime.Now;
                txtPayment.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            /* try
             {
                 using (SqlConnection connection = new SqlConnection(connectionString))
                 {
                     connection.Open();
                     using (SqlCommand command = connection.CreateCommand())
                     {
                         foreach (DataGridViewRow row in dataGridView3.Rows)
                         {
                             if (row.Cells["c_name"].Value != null &&
                                 row.Cells["d_t"].Value != null &&
                                 row.Cells["Payment"].Value != null)
                             {
                                 // Insert into Client_Payment table
                                 command.CommandText = "INSERT INTO Client_Payment (C_name, payment_date, payment) " +
                                                       "VALUES (@C_name, @payment_date, @payment)";

                                 // Add parameter values for Client_Payment
                                 command.Parameters.Clear(); // Clear parameters from previous iteration
                                 command.Parameters.AddWithValue("@C_name", row.Cells["c_name"].Value.ToString());
                                 command.Parameters.AddWithValue("@payment_date", Convert.ToDateTime(row.Cells["d_t"].Value));
                                 command.Parameters.AddWithValue("@payment", row.Cells["payment"].Value.ToString());

                                 command.ExecuteNonQuery();
                             }
                         }

                         MessageBox.Show("Data saved successfully");

                         // Optionally, reload the data into dataGridView1 to reflect changes in the database
                         LoadDataIntoDataGridView();
                     }
                 }

                 // Clear dataGridView3 after saving data
                 dataGridView3.Rows.Clear();

                 // Clear input controls and set values
                 txtName.Clear();
                 dateTimePicker1.Value = DateTime.Now;
                 txtPayment.Clear();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error: " + ex.Message);

             }*/

        }



        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboNameFilter.SelectedIndexChanged -= cboNameFilter_SelectedIndexChanged;

            // Clear selection
            cboNameFilter.SelectedIndex = -1;

            // Reattach event handler
            cboNameFilter.SelectedIndexChanged += cboNameFilter_SelectedIndexChanged;

            txtName.Clear();
            txtPayment.Clear();
            txtTotalAmount.Text = "0";
            txtAmountPaid.Text = "0";
            txtRemainingAmount.Text = "0";

            // Load data into DataGridViews
            LoadDataIntoDataGridView();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cboNameFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            object[] rowData = new object[]
    {
        txtName.Text,
        dateTimePicker1.Value,
        txtPayment.Text,
    };
            dataGridView3.Rows.Add(rowData);

            // Delete selected row from dataGridView2 and database
            if (dataGridView2.SelectedRows.Count > 0)
            {
                try
                {
                    // Get the index of the selected row
                    int rowIndex = dataGridView2.SelectedRows[0].Index;

                    // Get the client name, payment date, and payment amount from the selected row
                    string clientName = dataGridView2.Rows[rowIndex].Cells["C_name"].Value.ToString();
                    DateTime paymentDate = Convert.ToDateTime(dataGridView2.Rows[rowIndex].Cells["Bill_Date"].Value);
                    decimal paymentAmount = Convert.ToDecimal(dataGridView2.Rows[rowIndex].Cells["Payment"].Value);

                    // Delete the selected row from the database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "DELETE FROM Client_debts WHERE C_name = @clientName AND Bill_Date = @paymentDate AND Payment = @paymentAmount";
                            command.Parameters.AddWithValue("@clientName", clientName);
                            command.Parameters.AddWithValue("@paymentDate", paymentDate);
                            command.Parameters.AddWithValue("@paymentAmount", paymentAmount);
                            command.ExecuteNonQuery();
                        }
                    }

                    // Remove the selected row from the DataGridView
                    dataGridView2.Rows.RemoveAt(rowIndex);

                    UpdatePaymentInformation(txtName.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row in dataGridView2 to delete.");
            }

            // Clear input controls after adding data
            txtName.Clear();
            dateTimePicker1.Value = DateTime.Now;
            txtPayment.Clear();

            /* object[] rowData = new object[] 
             {                
                 txtName.Text,               
                 dateTimePicker1.Value,
                 txtPayment.Text,                
             };

             // Add the row to the DataGridView
                 dataGridView3.Rows.Add(rowData);

             // Clear input controls after adding data
             //cboClientsName.SelectedIndex = -1;
                 txtName.Clear();             
                 dateTimePicker1.Value = DateTime.Now;
                 txtPayment.Clear();   */
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDaily_Click(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void btnAgents_Click(object sender, EventArgs e)
        {
            Agents Agents = new Agents();
            Agents.Show();
            this.Hide();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            Clients.Show();
            this.Hide();
        }

        private void btnDrivers_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void btnExp_Click(object sender, EventArgs e)
        {

            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void guna2ControlBox1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTotalSum()
        {
            decimal totalSum = 0;
            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                // Assuming payment column is at index 2, adjust accordingly
                if (row.Cells.Count > 2 && row.Cells[2].Value != null)
                {
                    totalSum += Convert.ToDecimal(row.Cells[2].Value);
                }
            }
            txtTotalPay.Text = totalSum.ToString();
        }

        private void txtTotalPay_TextChanged(object sender, EventArgs e)
        {

        }
        private void DataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotalSum();
        }

        // Event handler for row deletion
        private void DataGridView3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalSum();
        }
        Stack<DataGridViewRow> addedRowsStack = new Stack<DataGridViewRow>();


        private void btnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        // Insert into Client_debts table
                        command.CommandText = "INSERT INTO Client_debts (C_name, Bill_Date, Payment) " +
                                              "VALUES (@C_name, @Bill_Date, @Payment)";

                        // Add parameter values
                        command.Parameters.AddWithValue("@C_name", txtName.Text);
                        command.Parameters.AddWithValue("@Bill_Date", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@Payment", Convert.ToDecimal(txtPayment.Text));

                        command.ExecuteNonQuery();
                    }
                }

                LoadDataIntoDataGridView();

                // Remove the selected row from dataGridView3
                int rowIndex = (int)txtName.Tag;
                dataGridView3.Rows.RemoveAt(rowIndex);

                UpdatePaymentInformation(txtName.Text);


                // Clear input controls
                txtName.Clear();
                dateTimePicker1.Value = DateTime.Now;
                txtPayment.Clear();

                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row index is selected
            {
                DataGridViewRow row = dataGridView3.Rows[e.RowIndex];

                // Populate the text boxes and DateTimePicker with the selected row's data
                txtName.Text = row.Cells["c_name"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["d_t"].Value);
                txtPayment.Text = row.Cells["payment"].Value.ToString();

                // Optionally, store the selected row index for later use
                txtName.Tag = e.RowIndex;
            }
        }

        private void txtRemainingAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }



        /*private void dataGridView3_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateTotalSum();
        }

        private void dataGridView3_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateTotalSum();
        }*/
    }
}
