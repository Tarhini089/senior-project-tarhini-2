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
    public partial class Clients : Form
    {



        private bool sidebarExpand = true;
        private string connection;
        private readonly string connectionString = "Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True";



        public Clients()
        {
            InitializeComponent();

            timer1.Enabled = true; // Enable the Timer
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += timer1_Tick;

            PopulateclientFilterComboBox();
        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM clients";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Daily_Bills");
            dataGridView1.DataSource = dataSet.Tables["Daily_Bills"];
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            timerSidebar1.Start();
        }

        private void LoadDataIntoDataGridView3()
        {
            // Construct the base query
            string query = "SELECT * FROM clients WHERE 1 = 1";

            // Append filtering conditions based on user input
            if (!string.IsNullOrEmpty(cboClientFilter.Text))
            {
                query += $" AND client_name LIKE '%{cboClientFilter.Text}%'";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "clients");
                    dataGridView1.DataSource = dataSet.Tables["clients"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PopulateclientFilterComboBox()
        {
            cboClientFilter.Items.Clear(); // Clear existing items

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
                            cboClientFilter.Items.Add(clientName);
                        }
                    }
                }
            }
        }

        private void timerSidebar1_Tick(object sender, EventArgs e)
        {
           /* if (sidebarExpand)
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

        private void Form7_Load(object sender, EventArgs e)
        {
            PopulateDriversComboBox();

            using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM clients", conn))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "t0");

                        // Clear existing columns
                        dataGridView1.Columns.Clear();

                        // Manually add columns in desired order
                        dataGridView1.Columns.Add("client_id", "Client ID");
                        dataGridView1.Columns.Add("client_name", "Client name");
                        dataGridView1.Columns.Add("phone", "Phone");
                        dataGridView1.Columns.Add("Address", "Address");
                        dataGridView1.Columns.Add("Bottles_owened", "Bottles Owned");
                        dataGridView1.Columns.Add("Date_time", "Date/Time");
                        dataGridView1.Columns.Add("delivery_Driver", "Delivery Driver");
                        dataGridView1.Columns.Add("description1", "Description");

                        // Remove auto-generated columns
                        dataGridView1.AutoGenerateColumns = false;

                        // Bind data
                        dataGridView1.DataSource = ds.Tables["t0"];

                        // Map each column to the corresponding column in the DataGridView
                        dataGridView1.Columns["client_id"].DataPropertyName = "client_id";
                        dataGridView1.Columns["client_name"].DataPropertyName = "client_name";
                        dataGridView1.Columns["phone"].DataPropertyName = "phone";
                        dataGridView1.Columns["Address"].DataPropertyName = "Address";
                        dataGridView1.Columns["Bottles_owened"].DataPropertyName = "Bottles_owened";
                        dataGridView1.Columns["Date_time"].DataPropertyName = "Date_time";
                        dataGridView1.Columns["delivery_Driver"].DataPropertyName = "delivery_Driver";
                        dataGridView1.Columns["description1"].DataPropertyName = "description1";

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
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Sidebarpannel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void PopulateDriversComboBox()
        {
            cboDriverName.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, driver_name FROM delrviry_driver";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int driverId = (int)reader["id"];
                            string drivername = (string)reader["driver_name"];

                            // Store driver name and ID as a tuple
                            var driverTuple = Tuple.Create(driverId, drivername);

                            // Add the tuple to the ComboBox
                            cboDriverName.Items.Add(driverTuple);
                        }
                    }
                }
            }
            /*  cboDriverName.Items.Clear(); // Clear existing items

              using (SqlConnection connection = new SqlConnection(connectionString))
              {
                  connection.Open();
                  using (SqlCommand command = connection.CreateCommand())
                  {
                      command.CommandText = "SELECT driver_name FROM delrviry_driver";

                      using (SqlDataReader reader = command.ExecuteReader())
                      {
                          while (reader.Read())
                          {

                              string drivername = (string)reader["driver_name"];

                              // Add just the driver name string to the ComboBox
                              cboDriverName.Items.Add(drivername);
                          }
                      }
                  }
              }*/
        }

            private void btnSave_Click(object sender, EventArgs e)
           {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO clients (client_name, phone, address, Bottles_owened, date_time, delivery_Driver, description1) VALUES (@client_name, @phone, @address, @Bottles_owened, @date_time, @delivery_Driver, @description1)";

                        string DriverName = null; // Change the type to string to store the driver name instead of ID
                        if (cboDriverName.SelectedItem != null)
                        {
                            var selectedDriver = (Tuple<int, string>)cboDriverName.SelectedItem;
                            DriverName = selectedDriver.Item2; // Retrieve the name from the tuple
                        }
                        else
                        {
                            MessageBox.Show("No Driver selected.");
                            return;
                        }

                        // Add parameter values
                        command.Parameters.AddWithValue("@client_name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Bottles_owened", txtBottles.Text);
                        command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@delivery_Driver", DriverName); // Use the retrieved name
                        command.Parameters.AddWithValue("@description1", txtDecsription.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();
                        txtName.Clear();
                        txtPhone.Clear();
                        txtAddress.Clear();
                        txtBottles.Clear();
                        txtDecsription.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            /*try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO clients (client_name, phone, address, Bottles_owened, date_time, delivery_Driver, description1) VALUES (@client_name, @phone, @address, @Bottles_owened, @date_time, @delivery_Driver, @description1)";

                        int DriverName = 0;
                        if (cboDriverName.SelectedItem != null)
                        {
                            var selectedDriverName = (dynamic)cboDriverName.SelectedItem;
                            DriverName = selectedDriverName.id;
                        }
                        else
                        {
                            MessageBox.Show("No DriverName selected."); // Debugging output
                            return; // Exit method if no agent is selected
                        }


                        // Add parameter values
                        command.Parameters.AddWithValue("@client_name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Bottles_owened", txtBottles.Text);
                        command.Parameters.AddWithValue("@date_time", dateTimePicker1.Text);
                        command.Parameters.AddWithValue("@delivery_Driver", DriverName);
                        command.Parameters.AddWithValue("@description1", txtDecsription.Text);



                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();
                        txtName.Clear();
                        txtPhone.Clear();
                        txtAddress.Clear();
                        txtBottles.Clear();
                        txtDecsription.Clear();



                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }*/
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                txtName.Text = selectedRow.Cells["client_name"].Value.ToString();
                txtPhone.Text = selectedRow.Cells["phone"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["address"].Value.ToString();
                txtBottles.Text = selectedRow.Cells["Bottles_owened"].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells["date_time"].Value.ToString());
                cboDriverName.Text = selectedRow.Cells["delivery_Driver"].Value.ToString();
                txtDecsription.Text = selectedRow.Cells["description1"].Value.ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = (int)dataGridView1.Rows[rowIndex].Cells["client_id"].Value; // Assuming there's a column named ID in your DataGridView

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE clients SET client_name = @client_name, " +
                                                     "phone = @phone, address = @address, " +
                                                       "Bottles_owened = @Bottles_owened , date_time = @date_time, " + 
                                                         "delivery_Driver=@delivery_Driver , description1 = @description1 " + // Added space here
                                                           "WHERE client_id = @client_id";

                            command.Parameters.AddWithValue("@client_name", txtName.Text);
                            command.Parameters.AddWithValue("@phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@address", txtAddress.Text);
                            command.Parameters.AddWithValue("@Bottles_owened", txtBottles.Text);
                            command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@delivery_Driver", cboDriverName.Text);
                            command.Parameters.AddWithValue("@description1", txtDecsription.Text);
                            command.Parameters.AddWithValue("@client_id", id); // Assuming ID is a parameter needed for update

                            command.ExecuteNonQuery();
                        }
                    }

                    // Now update the corresponding row in the DataGridView
                    dataGridView1.Rows[rowIndex].Cells["client_name"].Value = txtName.Text;
                    dataGridView1.Rows[rowIndex].Cells["phone"].Value = txtPhone.Text;
                    dataGridView1.Rows[rowIndex].Cells["address"].Value = txtAddress.Text;
                    dataGridView1.Rows[rowIndex].Cells["Bottles_owened"].Value = txtBottles.Text;
                    dataGridView1.Rows[rowIndex].Cells["date_time"].Value = dateTimePicker1.Value;
                    dataGridView1.Rows[rowIndex].Cells["delivery_Driver"].Value = cboDriverName.Text;
                    dataGridView1.Rows[rowIndex].Cells["description1"].Value = txtDecsription.Text;


                    // Clear text boxes
                    txtName.Clear();
                    txtPhone.Clear();
                    txtAddress.Clear();
                    txtBottles.Clear();
                    txtDecsription.Clear();


                    MessageBox.Show("Data updated successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        

        private void txtName_TextChanged(object sender, EventArgs e)
        {
;           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Agents Agents = new Agents();
            Agents.Show();
            this.Hide();
        }

        private void btnDaily_Click(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clients_pmt clients_pmt = new clients_pmt();
            clients_pmt.Show();
            //this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DriverList DriverList = new DriverList();
            DriverList.Show(this);
        }

        private void cboDriverName_SelectedIndexChanged(object sender, EventArgs e)
        {
             
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int rowIndex = dataGridView1.SelectedRows[0].Index;

                        int recordID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["client_id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM clients WHERE client_id = @client_id";
                                command.Parameters.AddWithValue("@client_id", recordID);
                                command.ExecuteNonQuery();

                                txtName.Clear();
                                txtPhone.Clear();
                                txtAddress.Clear();
                                txtBottles.Clear();
                                cboDriverName.SelectedIndex = -1;
                                txtDecsription.Clear();
                            }
                        }

                        dataGridView1.Rows.RemoveAt(rowIndex);

                        MessageBox.Show("Record deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void cboClientFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }
    }
}
