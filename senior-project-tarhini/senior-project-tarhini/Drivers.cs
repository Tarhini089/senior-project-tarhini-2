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

    public partial class Drivers : Form
    {
        
        private bool sidebarExpand = true;

        private string connection;
        private readonly string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";

        //private bool sidebarExpand = true;
        

        public Drivers()
        {
            InitializeComponent();
        }

        private bool isEditing = false;
        private int editedRow = -1;

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM delrviry_driver";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "delrviry_driver");
            dataGridView1.DataSource = dataSet.Tables["delrviry_driver"];
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM delrviry_driver", conn))
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO delrviry_driver VALUES (@driver_name, @Address, @Phone, @salary, @Description)";

                        // Add parameter values
                        command.Parameters.AddWithValue("@driver_name", txtDriverName.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@salary", txtSalary.Text);
                        command.Parameters.AddWithValue("@Description", txtDescription.Text);


                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();
                        txtDriverName.Clear();
                        txtAddress.Clear();
                        txtPhone.Clear();
                        txtSalary.Clear();
                        txtDescription.Clear();



                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }



        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                txtDriverName.Text = selectedRow.Cells["driver_name"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                txtPhone.Text = selectedRow.Cells["Phone"].Value.ToString();
                txtSalary.Text = selectedRow.Cells["salary"].Value.ToString();
                txtDescription.Text = selectedRow.Cells["Description"].Value.ToString();

            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = (int)dataGridView1.Rows[rowIndex].Cells["id"].Value; // Assuming there's a column named AgentID in your DataGridView

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE delrviry_driver SET driver_name = @driver_name, " +
                                                  "Address = @Address, Phone = @Phone, salary = @salary, " +
                                                  "Description = @Description " +
                                                  "WHERE id = @id";

                            command.Parameters.AddWithValue("@driver_name", txtDriverName.Text);
                            command.Parameters.AddWithValue("@Address", txtAddress.Text);
                            command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@salary", txtSalary.Text);
                            command.Parameters.AddWithValue("@Description", txtDescription.Text);
                            command.Parameters.AddWithValue("@id", id);

                            command.ExecuteNonQuery();
                        }
                    }

                    // Now update the corresponding row in the DataGridView
                    dataGridView1.Rows[rowIndex].Cells["driver_name"].Value = txtDriverName.Text;
                    dataGridView1.Rows[rowIndex].Cells["Address"].Value = txtAddress.Text;
                    dataGridView1.Rows[rowIndex].Cells["Phone"].Value = txtPhone.Text;
                    dataGridView1.Rows[rowIndex].Cells["salary"].Value = txtSalary.Text;
                    dataGridView1.Rows[rowIndex].Cells["Description"].Value = txtDescription.Text;

                    // Clear text boxes
                    txtDriverName.Clear();
                    txtAddress.Clear();
                    txtPhone.Clear();
                    txtSalary.Clear();
                    txtDescription.Clear();

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

                        int recordID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM delrviry_driver WHERE id = @id";
                                command.Parameters.AddWithValue("@id", recordID);
                                command.ExecuteNonQuery();

                                txtDriverName.Clear();
                                txtAddress.Clear();
                                txtPhone.Clear();
                                txtSalary.Clear();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void timerSidebar1_Tick(object sender, EventArgs e)
        {
            /*if (sidebarExpand) // Assuming 'sidebarExpand' is the variable indicating the state
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

        private void MenuButton_Click(object sender, EventArgs e)
        {
            timerSidebar1.Start();
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

        private void guna2ControlBox1_Click(object sender, EventArgs e)
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
