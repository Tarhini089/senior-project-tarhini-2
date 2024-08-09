using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace senior_project_tarhini
{
    public partial class Agents : Form
    {
        private bool sidebarExpand = true;
        private string connection;
        private readonly string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";
        public Agents()
        {
            InitializeComponent();

            timer1.Enabled = true; // Enable the Timer
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += timer1_Tick;

            PopulateAgentFilterComboBox();

        }

        private bool isEditing = false;
        private int editedRow = -1;

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void MenuButton_Click(object sender, EventArgs e)
        {
            timerSidebar1.Start();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Sidebarpannel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void LoadDataIntoDataGridView3()
        {
            // Construct the base query
            string query = "SELECT * FROM Agents_Fin WHERE 1 = 1";

            // Append filtering conditions based on user input
            if (!string.IsNullOrEmpty(cboAgentFilter.Text))
            {
                query += $" AND Agent_id LIKE '%{cboAgentFilter.Text}%'";
            }          

            if (chkDateFilter.Checked)
            {
                DateTime fromDate = dateFromFilter.Value.Date;
                DateTime toDate = dateToFilter.Value.Date.AddDays(1); // Include the whole day of the end date
                query += $" AND Date BETWEEN '{fromDate:yyyy-MM-dd}' AND '{toDate:yyyy-MM-dd}'";
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "Agents_Fin");
                    dataGridView2.DataSource = dataSet.Tables["Agents_Fin"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PopulateAgentFilterComboBox()
        {
            cboAgentFilter.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id FROM Agents_info";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int Agentid = (int)reader["id"];
                            cboAgentFilter.Items.Add(Agentid);
                        }
                    }
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Agents_info", conn))
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
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Agents_Fin", conn))
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

            PopulateAgentComboBox();


        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM Agents_info";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Agents_info");
            dataGridView1.DataSource = dataSet.Tables["Agents_info"];
        }

        private void LoadDataIntoDataGridView1()
        {
            string query = "SELECT * FROM Agents_Fin";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Agents_Fin");
            dataGridView2.DataSource = dataSet.Tables["Agents_Fin"];
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

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
                        command.CommandText = "INSERT INTO Agents_info VALUES (@full_name, @phone, @Adress, @Username, @password2)";

                        // Add parameter values
                        command.Parameters.AddWithValue("@full_name", txtName.Text);
                        command.Parameters.AddWithValue("@phone", txtPhone.Text);
                        command.Parameters.AddWithValue("@Adress", txtAddress.Text);
                        command.Parameters.AddWithValue("@Username", txtUsername.Text);
                        command.Parameters.AddWithValue("@password2", txtPassword.Text);
                        

                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();
                        txtName.Clear();
                        txtPhone.Clear();
                        txtAddress.Clear();
                        txtUsername.Clear();
                        txtPassword.Clear();
                       


                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                txtName.Text = selectedRow.Cells["full_name"].Value.ToString();
                txtPhone.Text = selectedRow.Cells["phone"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Adress"].Value.ToString();
                txtUsername.Text = selectedRow.Cells["Username"].Value.ToString();
                txtPassword.Text = selectedRow.Cells["password2"].Value.ToString();
                
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
                            command.CommandText = "UPDATE Agents_info SET full_name = @full_name, " +
                                                  "phone = @phone, Adress = @Adress, Username = @Username, " +
                                                  "password2 = @password2 " +
                                                  "WHERE id = @id";

                            command.Parameters.AddWithValue("@full_name", txtName.Text);
                            command.Parameters.AddWithValue("@phone", txtPhone.Text);
                            command.Parameters.AddWithValue("@Adress", txtAddress.Text);
                            command.Parameters.AddWithValue("@Username", txtUsername.Text);
                            command.Parameters.AddWithValue("@password2", txtPassword.Text);
                            command.Parameters.AddWithValue("@id", id);

                            command.ExecuteNonQuery();
                        }
                    }

                    // Now update the corresponding row in the DataGridView
                    dataGridView1.Rows[rowIndex].Cells["full_name"].Value = txtName.Text;
                    dataGridView1.Rows[rowIndex].Cells["phone"].Value = txtPhone.Text;
                    dataGridView1.Rows[rowIndex].Cells["Adress"].Value = txtAddress.Text;
                    dataGridView1.Rows[rowIndex].Cells["Username"].Value = txtUsername.Text;
                    dataGridView1.Rows[rowIndex].Cells["password2"].Value = txtPassword.Text;

                    // Clear text boxes
                    txtName.Clear();
                    txtPhone.Clear();
                    txtAddress.Clear();
                    txtUsername.Clear();
                    txtPassword.Clear();

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
                                command.CommandText = "DELETE FROM Agents_info WHERE id = @id";
                                command.Parameters.AddWithValue("@id", recordID);
                                command.ExecuteNonQuery();

                                txtName.Clear();
                                txtPhone.Clear();
                                txtAddress.Clear();
                                txtUsername.Clear();
                                txtPassword.Clear();
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

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                txtName.Text = selectedRow.Cells["full_name"].Value.ToString();
                txtPhone.Text = selectedRow.Cells["phone"].Value.ToString();
                txtAddress.Text = selectedRow.Cells["Adress"].Value.ToString();
                txtUsername.Text = selectedRow.Cells["Username"].Value.ToString();
                txtPassword.Text = selectedRow.Cells["password2"].Value.ToString();

            }
        }

        private void PopulateAgentComboBox()
        {
            cboAgentID.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT id, full_name FROM Agents_info";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            string fullName = (string)reader["full_name"];

                            // Create a custom object to hold both id and full_name
                            var agent = new { ID = id, FullName = fullName };

                            // Add the custom object to the ComboBox
                            cboAgentID.Items.Add(agent);
                        }
                    }
                }
            }
            /* cboAgentID.Items.Clear(); // Clear existing items

             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 connection.Open();
                 using (SqlCommand command = connection.CreateCommand())
                 {
                     command.CommandText = "SELECT id FROM Agents_info";

                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             int id = (int)reader["id"];
                            // string fullName = (string)reader["full_name"];

                             // Create a custom object to hold both id and full_name
                             var agent = new { id};

                             // Add the custom object to the ComboBox
                             cboAgentID.Items.Add(agent);
                         }
                     }
                 }
             }*/
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO Agents_Fin (Agent_id,Salery, Date, Description) " +
                                              "VALUES (@Agent_id, @Salery, @Date, @Description)";

                        // Get the selected agent's ID from the ComboBox
                        int agentID = 0;
                        if (cboAgentID.SelectedItem != null)
                        {
                            var selectedAgent = (dynamic)cboAgentID.SelectedItem;
                            agentID = selectedAgent.ID;
                        }
                        else
                        {
                            MessageBox.Show("No agent selected."); // Debugging output
                            return; // Exit method if no agent is selected
                        }

                        // Add parameter values
                        command.Parameters.AddWithValue("@Agent_id", agentID);
                        command.Parameters.AddWithValue("@Salery", txtSalary.Text);
                        command.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@Description", txtDesc.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView1();

                        //cboAgentID.Items.Clear();
                        //cboAgentID.SelectedIndex = -1;
                        txtSalary.Clear();
                        //dateTimePicker1 = new DateTimePicker();
                        txtDesc.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cboAgentID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView2.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];

                cboAgentID.Text = selectedRow.Cells["Agent_id"].Value.ToString();
                txtSalary.Text = selectedRow.Cells["Salery"].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(selectedRow.Cells["Date"].Value.ToString());
                txtDesc.Text = selectedRow.Cells["Description"].Value.ToString();             

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView2.SelectedRows[0].Index;
                    int id = (int)dataGridView2.Rows[rowIndex].Cells["ID"].Value; // Assuming there's a column named ID in your DataGridView

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE Agents_Fin SET Salery = @Salery, " +
                                                  "Date = @Date, Description = @Description " + // Added space here
                                                  "WHERE ID = @ID";

                            command.Parameters.AddWithValue("@Salery", txtSalary.Text);
                            command.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@Description", txtDesc.Text);
                            command.Parameters.AddWithValue("@ID", id); // Assuming ID is a parameter needed for update

                            command.ExecuteNonQuery();
                        }
                    }

                    // Now update the corresponding row in the DataGridView
                    dataGridView2.Rows[rowIndex].Cells["Salery"].Value = txtSalary.Text;
                    dataGridView2.Rows[rowIndex].Cells["Date"].Value = dateTimePicker1.Value;
                    dataGridView2.Rows[rowIndex].Cells["Description"].Value = txtDesc.Text;

                    // Clear text boxes
                    txtSalary.Clear();
                    txtDesc.Clear();

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        int rowIndex = dataGridView2.SelectedRows[0].Index;

                        int recordID = Convert.ToInt32(dataGridView2.Rows[rowIndex].Cells["id"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM Agents_Fin WHERE id = @ID";
                                command.Parameters.AddWithValue("@ID", recordID);
                                command.ExecuteNonQuery();

                                txtSalary.Clear();
                                txtDesc.Clear();
                            }
                        }

                        dataGridView2.Rows.RemoveAt(rowIndex);

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

        private void button3_Click(object sender, EventArgs e)
        {
            Agents Agents = new Agents();
            Agents.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            Clients.Show();
            this.Hide();
        }

        private void btnDaily_Click(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
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

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Items items = new Items();
            items.Show();
            this.Hide();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Drivers Drivers = new Drivers();
            Drivers.Show();
            this.Hide();
        }

        private void btnDaily_Click_1(object sender, EventArgs e)
        {
            Daily Daily = new Daily();
            Daily.Show();
            this.Hide();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Clients Clients = new Clients();
            Clients.Show();
            this.Hide();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {

            Expenses Expenses = new Expenses();
            Expenses.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Payments Payments = new Payments();
            Payments.Show();
            this.Hide();
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void chkDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            dateFromFilter.Enabled = chkDateFilter.Checked;
            dateToFilter.Enabled = chkDateFilter.Checked;
            LoadDataIntoDataGridView3();
        }

        private void cboAgentFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
        }

        private void dateFromFilter_ValueChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
        }

        private void dateToFilter_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboAgentFilter.SelectedIndex = -1;
            dateFromFilter.Value = DateTime.Today;
            dateToFilter.Value = DateTime.Today;
            chkDateFilter.Checked = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }
    }
}
