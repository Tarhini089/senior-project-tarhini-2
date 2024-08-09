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
using System.Xml.Linq;

namespace senior_project_tarhini
{
    public partial class Expenses : Form
    {

        private string connection;
        private readonly string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";

        private bool sidebarExpand = true;
        public Expenses()
        {
            InitializeComponent();

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

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM Expenses";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Expenses");
            dataGridView1.DataSource = dataSet.Tables["Expenses"];
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Expenses", conn))
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
        }

        private void LoadDataIntoDataGridView3()
        {
            // Construct the base query
            string query = "SELECT * FROM Expenses WHERE 1 = 1";

            // Append filtering conditions based on user input          

            if (chkDateFilter.Checked)
            {
                DateTime fromDate = dateFromFilter.Value.Date;
                DateTime toDate = dateToFilter.Value.Date.AddDays(1); // Include the whole day of the end date
                query += $" AND date_time BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "Expenses");
                    dataGridView1.DataSource = dataSet.Tables["Expenses"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void panel22_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            dateFromFilter.Enabled = chkDateFilter.Checked;
            dateToFilter.Enabled = chkDateFilter.Checked;
            LoadDataIntoDataGridView3();
        }

        private void dateFromFilter_ValueChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
        }

        private void dateToFilter_ValueChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                txtType.Text = selectedRow.Cells["Type"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["price"].Value.ToString();              
                dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells["date_time"].Value.ToString());
                txtDescription.Text = selectedRow.Cells["description"].Value.ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;

                    dataGridView1.Rows[rowIndex].Cells["Type"].Value = txtType.Text;
                    dataGridView1.Rows[rowIndex].Cells["price"].Value = txtPrice.Text; // Updated column name
                    dataGridView1.Rows[rowIndex].Cells["date_time"].Value = dateTimePicker1.Value;
                    dataGridView1.Rows[rowIndex].Cells["description"].Value = txtDescription.Text; // Updated column name

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE Expenses SET Type = @Type, price = @price, " +
                                                  "date_time = @date_time, description = @description " +
                                                  "WHERE id = @id";

                            command.Parameters.AddWithValue("@Type", txtType.Text);
                            command.Parameters.AddWithValue("@price", txtPrice.Text);
                            command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@description", txtDescription.Text);
                            command.Parameters.AddWithValue("@id", dataGridView1.Rows[rowIndex].Cells["id"].Value); // Assuming there's a column named "ID"

                            command.ExecuteNonQuery();

                            
                            txtType.Clear();
                            txtPrice.Clear();
                            txtDescription.Clear();

                        }
                    }

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Expenses (Type,price,date_time,description) VALUES (@Type, @price, @date_time, @description)";

                       

                        // Add parameter values
                        command.Parameters.AddWithValue("@Type", txtType.Text);
                        command.Parameters.AddWithValue("@price", txtPrice.Text);
                        command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@description", txtDescription.Text);                      

                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();
                        txtType.Clear();
                        txtPrice.Clear();
                        txtDescription.Clear();
                        dateTimePicker1.Value = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

                        int recordID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM Expenses WHERE id = @id";
                                command.Parameters.AddWithValue("@id", recordID);
                                command.ExecuteNonQuery();

                                txtType.Clear();
                                txtPrice.Clear();
                                txtDescription.Clear();                              
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

        private void button8_Click(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }
    }
}
