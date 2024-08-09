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

    public partial class Items : Form
    {
        private bool sidebarExpand = true;
        private string connection;
        private readonly string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";

        public Items()
        {
            InitializeComponent();
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Items", conn))
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM Items";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Daily_Bills");
            dataGridView1.DataSource = dataSet.Tables["Daily_Bills"];
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtType.Text = selectedRow.Cells["type"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["price"].Value.ToString();
                txtDescription.Text = selectedRow.Cells["description"].Value.ToString();


            }
        }
        private bool isEditing = false;
        private int editedRow = -1;

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Insert new row into the Items table
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        // Insert a new row into the Items table
                        command.CommandText = "INSERT INTO Items (type, price, description) VALUES (@type, @price, @description)";

                        // Set parameter values
                        command.Parameters.AddWithValue("@type", txtType.Text);
                        command.Parameters.AddWithValue("@price", txtPrice.Text);
                        command.Parameters.AddWithValue("@description", txtDescription.Text);

                        // Execute the command to insert the new row
                        command.ExecuteNonQuery();

                        LoadDataIntoDataGridView();

                    }
                }

                // Clear text boxes after saving
                txtType.Text = "";
                txtPrice.Text = "0";
                txtDescription.Text = "";

                MessageBox.Show("Data saved successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           // Application.Exit();
            //Daily daily = new Daily();
            //this.Hide();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             
        }

        private void btnEdit3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["id"].Value); // Assuming there's an ID column in your DataGridView

                    dataGridView1.Rows[rowIndex].Cells["type"].Value = txtType.Text;
                    dataGridView1.Rows[rowIndex].Cells["price"].Value = txtPrice.Text;
                    dataGridView1.Rows[rowIndex].Cells["description"].Value = txtDescription.Text;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            // Update the row with the specific item ID
                            command.CommandText = "UPDATE Items SET type = @type, price = @price, description =@description WHERE id = @id";

                            command.Parameters.AddWithValue("@type", txtType.Text);
                            command.Parameters.AddWithValue("@price", txtPrice.Text);
                            command.Parameters.AddWithValue("@description", txtDescription.Text);
                            command.Parameters.AddWithValue("@id", id);

                            command.ExecuteNonQuery();

                            txtType.Text = "";
                            txtPrice.Text = "0";
                            txtDescription.Text = "";
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

                        int id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM Items WHERE id = @id"; // Replace "Your_Table_Name" with the name of your table
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();

                                txtType.Text = "";
                                txtPrice.Text = "0";
                                txtDescription.Text = "";
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel22_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtType_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
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

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }
    }
    }

