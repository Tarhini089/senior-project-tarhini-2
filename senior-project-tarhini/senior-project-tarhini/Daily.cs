using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Management;
using System.Xml.Linq;
using static Guna.UI2.WinForms.Suite.Descriptions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace senior_project_tarhini
{
    public partial class Daily : Form
    {
        

        
        private string connection;
        private readonly string connectionString = "Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True";

        
        public object Connection { get; private set; }

        public Daily()
        {
            

            InitializeComponent();

          

            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;

            timer1.Enabled = true; // Enable the Timer
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += timer1_Tick;

            txtTotal.Text = "0";

            PopulateclientComboBox(); 
            PopulateclientFilterComboBox();
            PopulateitemsFilterComboBox();

            btnClearFilters.Click += btnClearFilters_Click;
            panel6.Hide();

            btnInvoice.Visible = false;

            /* nudFilling10.TextChanged += UpdateTotal;
             nudFilling20.TextChanged += UpdateTotal;
             nudBuying10.TextChanged += UpdateTotal;
             nudBuying20.TextChanged += UpdateTotal;
             nudSmallBag.TextChanged += UpdateTotal;
             nudBigBag.TextChanged += UpdateTotal;*/

            /*  nudQuantity.ValueChanged += UpdateTotal;
              cboItems.SelectedIndexChanged += UpdateTotal;
              txtTax.TextChanged += UpdateTotal;

              nudQuantity.ValueChanged += nudQuantity_ValueChanged;
              cboItems.SelectedIndexChanged += cboItems_SelectedIndexChanged;*/
            PopulateitemsComboBox();

            cboClientsName.Enabled = false;
            cboItems.Enabled = false;
            nudQuantity.Enabled = false;
            dateTimePicker1.Enabled = false;


           

            

        }

        private void LoadDataIntoDataGridView3()
        {
            // Construct the base query
            string query = "SELECT * FROM Daily_Bills WHERE 1 = 1";

            // Append filtering conditions based on user input
            if (!string.IsNullOrEmpty(cboClientFilter.Text))
            {
                query += $" AND client_name LIKE '%{cboClientFilter.Text}%'";
            }

            if (cboItemFilter.SelectedItem != null)
            {
                query += $" AND Items = '{cboItemFilter.SelectedItem}'";
            }

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
                    adapter.Fill(dataSet, "Daily_Bills");
                    dataGridView1.DataSource = dataSet.Tables["Daily_Bills"];
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

        private void PopulateitemsFilterComboBox()
        {
            cboItemFilter.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT type FROM items";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string itemtype = (string)reader["type"];
                            cboItemFilter.Items.Add(itemtype);
                        }
                    }
                }
            }
        }

        private void PopulateclientComboBox()
        {
            cboClientsName.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT client_id, client_name FROM clients";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clientname = (string)reader["client_name"];

                            // Add client name to the ComboBox
                            cboClientsName.Items.Add(clientname);
                        }
                    }
                }
            }
            /* cboClientsName.Items.Clear(); // Clear existing items

             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 connection.Open();
                 using (SqlCommand command = connection.CreateCommand())
                 {
                     command.CommandText = "SELECT client_id, client_name FROM clients";

                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             int clientId = (int)reader["client_id"];
                             string clientname = (string)reader["client_name"];

                             // Store driver name and ID as a tuple
                             var driverTuple = Tuple.Create(clientId, clientname);

                             // Add the tuple to the ComboBox
                             cboClientsName.Items.Add(driverTuple);
                         }
                     }
                 }
             }*/
        }

        private void PopulateitemsComboBox()
        {
           /* cboItems.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT type, price FROM Items";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Type = (string)reader["type"];
                            decimal Price = (decimal)reader["price"];

                            // Format the price to align right with a dollar sign
                            string formattedPrice = string.Format("{0,10:C2}", Price); // Adjust the width as needed

                            // Concatenate type and price into a single string
                            string displayText = $"{Type,-20} {formattedPrice}";

                            // Add item to the ComboBox
                            cboItems.Items.Add(displayText);

                            // Debugging statement
                            Console.WriteLine("DEBUG: Added item to combo box: " + displayText);
                        }
                    }
                }
            }*/



            cboItems.Items.Clear(); // Clear existing items

             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 connection.Open();
                 using (SqlCommand command = connection.CreateCommand())
                 {
                     command.CommandText = "SELECT type FROM Items";

                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             string Type = (string)reader["type"];

                             // Add item to the ComboBox
                             cboItems.Items.Add(Type);

                             // Debugging statement
                             Console.WriteLine("DEBUG: Added item to combo box: " + Type);
                         }
                     }
                 }
             }


            /*cboItems.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT type FROM Items";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Type = (string)reader["type"];

                            // Add client name to the ComboBox
                            cboItems.Items.Add(Type);
                        }
                    }
                }
            }*/
        }

            private void UpdateQuantity(object sender, EventArgs e)
        {
           /* int filling_10, filling_20, buying_10, buying_20, small_bag, big_bag;

            // Try parsing text from each textbox, assign default value if parsing fails
            int.TryParse(nudFilling10.Text, out filling_10);
            int.TryParse(nudFilling20.Text, out filling_20);
            int.TryParse(nudBuying10.Text, out buying_10);
            int.TryParse(nudBuying20.Text, out buying_20);
            int.TryParse(nudSmallBag.Text, out small_bag);
            int.TryParse(nudBigBag.Text, out big_bag);

            // Calculate the sum
            int sum = filling_10 + filling_20 + buying_10 + buying_20 + small_bag + big_bag;

            // Update txtQuantity with the sum
            txt_Quantity.Text = sum.ToString();*/
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
            PopulateclientComboBox();
            PopulateitemsComboBox();
            panel4.Visible = false;
            cboClientsName.Enabled = false;
            if (cboClientsName.Items.Count > 0)
            {
                cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
            }            
            //cboItems.Enabled = false;
            //nudQuantity.Enabled = false;
            //dateTimePicker1.Enabled = false;
            //txtDescription.Enabled = false;
                
            using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Daily_Bills", conn))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "t0");

                        // Clear existing columns
                        dataGridView1.Columns.Clear();

                        // Manually add columns in desired order
                        dataGridView1.Columns.Add("id", "id");
                        dataGridView1.Columns.Add("client_name", "client name");
                        dataGridView1.Columns.Add("Items", "Items");
                        dataGridView1.Columns.Add("Quantity", "Quantity");
                        dataGridView1.Columns.Add("date_time", "date_time");
                        dataGridView1.Columns.Add("tax", "tax");
                        dataGridView1.Columns.Add("Total", "Total");
                        dataGridView1.Columns.Add("Description", "Description");

                        // Remove auto-generated columns
                        dataGridView1.AutoGenerateColumns = false;

                        // Bind data
                        dataGridView1.DataSource = ds.Tables["t0"];

                        // Map each column to the corresponding column in the DataGridView
                        dataGridView1.Columns["id"].DataPropertyName = "id";
                        dataGridView1.Columns["client_name"].DataPropertyName = "client_name";
                        dataGridView1.Columns["Items"].DataPropertyName = "Items";
                        dataGridView1.Columns["Quantity"].DataPropertyName = "Quantity";
                        dataGridView1.Columns["date_time"].DataPropertyName = "date_time";
                        dataGridView1.Columns["tax"].DataPropertyName = "tax";
                        dataGridView1.Columns["Total"].DataPropertyName = "Total";
                        dataGridView1.Columns["Description"].DataPropertyName = "Description";

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

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            /* using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
              {
                  try
                  {
                      conn.Open();

                      using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Daily_Bills", conn))
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
              }*/


        }


        private void button2_Click(object sender, EventArgs e)
        {
            timerSidebar1.Start();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM Daily_Bills";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Daily_Bills");
            dataGridView1.DataSource = dataSet.Tables["Daily_Bills"];
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
                        if (checkBox1.Checked) // Check if checkbox is checked
                        {
                            // Insert into both tables
                            command.CommandText = "INSERT INTO Daily_Bills (client_name, Items, Quantity, date_time, tax, Total, Description) " +
                                                  "VALUES (@client_name, @Items, @Quantity, @date_time, @tax, @Total, @Description);" +
                                                  "INSERT INTO Client_Payment  (C_Name, payment_date, Payment) VALUES (@client_name, @date_time, @Total);";
                            // Add parameters for newtable
                            command.Parameters.AddWithValue("@C_Name", cboClientsName.Text); // Replace value1 with actual value
                            command.Parameters.AddWithValue("@payment_date", dateTimePicker1.Value); // Replace value2 with actual value
                            command.Parameters.AddWithValue("@Payment", txtTotal.Text); // Replace value3 with actual value
                        }
                        else
                        {
                            // Insert into Daily_Bills and Client_debts tables
                            command.CommandText = "INSERT INTO Daily_Bills (client_name, Items, Quantity, date_time, tax, Total, Description) " +
                                                  "VALUES (@client_name, @Items, @Quantity, @date_time, @tax, @Total, @Description);" +
                                                  "INSERT INTO Client_debts (C_name, Bill_Date, Payment) VALUES (@client_name, @date_time, @Total);";

                            // Add parameters for Client_debts
                            command.Parameters.AddWithValue("@C_name", cboClientsName.Text);
                            command.Parameters.AddWithValue("@Bill_Date", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@Payment", txtTotal.Text);
                        }

                        // Add parameter values for Daily_Bills
                        command.Parameters.AddWithValue("@client_name", cboClientsName.Text);
                        command.Parameters.AddWithValue("@Items", cboItems.Text);
                        command.Parameters.AddWithValue("@Quantity", nudQuantity.Text);
                        command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                        command.Parameters.AddWithValue("@tax", txtTax.Text);
                        command.Parameters.AddWithValue("@Total", txtTotal.Text);
                        command.Parameters.AddWithValue("@Description", txtDescription.Text);

                        command.ExecuteNonQuery();
                        MessageBox.Show("Data saved successfully");

                        LoadDataIntoDataGridView();

                        // Clear controls and set values
                        cboClientsName.Enabled = false;
                        if (cboClientsName.Items.Count > 0)
                        {
                            cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                        }
                        cboItems.SelectedIndex = -1;
                        nudQuantity.Value = 0;
                        txtTotal.Text = "0";
                        txtTax.Text = "11";
                        txtDescription.Clear();
                        checkBox1.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            /*  try
               {
                   using (SqlConnection connection = new SqlConnection(connectionString))
                   {
                       connection.Open();
                       using (SqlCommand command = connection.CreateCommand())
                       {
                           command.CommandText = "INSERT INTO Daily_Bills (client_name, Items, Quantity, date_time, tax, Total, Description) " +
                                                 "VALUES (@client_name, @Items, @Quantity, @date_time, @tax, @Total, @Description)";

                           // Add parameter values
                           command.Parameters.AddWithValue("@client_name", cboClientsName.Text);
                           command.Parameters.AddWithValue("@Items", cboItems.Text);
                           command.Parameters.AddWithValue("@Quantity", nudQuantity.Text);
                           command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                           command.Parameters.AddWithValue("@tax", txtTax.Text);
                           command.Parameters.AddWithValue("@Total", txtTotal.Text);
                           command.Parameters.AddWithValue("@Description", txtDescription.Text);

                           command.ExecuteNonQuery();
                           MessageBox.Show("Data saved successfully");

                           LoadDataIntoDataGridView();

                          // Clear controls and set values
                          cboClientsName.Enabled = false;
                          if (cboClientsName.Items.Count > 0)
                          {
                              cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                          }
                          cboItems.SelectedIndex = -1;
                           nudQuantity.Value = 0;
                           txtTotal.Text = "0";
                           txtTax.Text = "11";
                           txtDescription.Clear();
                       }
                   }
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Error: " + ex.Message);
               }*/
        }

        private void Txtbig_bag_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                cboClientsName.Enabled = true;
               // cboItems.Enabled = true;
                //nudQuantity.Enabled = true;
                //dateTimePicker1.Enabled = true;
                //txtDescription.Enabled = true;
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                cboClientsName.Text = selectedRow.Cells["client_name"].Value.ToString();
                cboItems.Text = selectedRow.Cells["Items"].Value.ToString();
                nudQuantity.Text = selectedRow.Cells["Quantity"].Value.ToString();                
                dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["date_time"].Value);
                txtTax.Text = selectedRow.Cells["tax"].Value.ToString();
                txtTotal.Text = selectedRow.Cells["Total"].Value.ToString();
                txtDescription.Text = selectedRow.Cells["Description"].Value.ToString();

                cboClientsName.Enabled = true;
                cboItems.Enabled = true;
                nudQuantity.Enabled = true;
                dateTimePicker1.Enabled = true;
            }
        }
        private bool isEditing = false;
        private int editedRow = -1;

        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;

                    dataGridView1.Rows[rowIndex].Cells["client_name"].Value = cboClientsName.Text;
                    dataGridView1.Rows[rowIndex].Cells["Items"].Value = cboItems.Text; // Updated column name
                    dataGridView1.Rows[rowIndex].Cells["Quantity"].Value = nudQuantity.Text;
                    dataGridView1.Rows[rowIndex].Cells["date_time"].Value = dateTimePicker1.Value;
                    dataGridView1.Rows[rowIndex].Cells["tax"].Value = txtTax.Text;
                    dataGridView1.Rows[rowIndex].Cells["Total"].Value = txtTotal.Text;
                    dataGridView1.Rows[rowIndex].Cells["Description"].Value = txtDescription.Text; // Updated column name

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = "UPDATE Daily_Bills SET client_name = @client_name, Items = @Items, " +
                                                  "Quantity = @Quantity, date_time = @date_time, " +
                                                  "tax = @tax, Total = @Total, Description = @Description " +
                                                  "WHERE id = @id";

                            command.Parameters.AddWithValue("@client_name", cboClientsName.Text);
                            command.Parameters.AddWithValue("@Items", cboItems.Text);
                            command.Parameters.AddWithValue("@Quantity", nudQuantity.Text);
                            command.Parameters.AddWithValue("@date_time", dateTimePicker1.Value);
                            command.Parameters.AddWithValue("@tax", txtTax.Text);
                            command.Parameters.AddWithValue("@Total", txtTotal.Text);
                            command.Parameters.AddWithValue("@Description", txtDescription.Text);
                            command.Parameters.AddWithValue("@id", dataGridView1.Rows[rowIndex].Cells["id"].Value); // Assuming there's a column named "ID"

                            command.ExecuteNonQuery();

                            cboClientsName.Enabled = false;
                            if (cboClientsName.Items.Count > 0)
                            {
                                cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                            }
                            cboItems.SelectedIndex = -1;
                            nudQuantity.Value = 1;
                            txtTotal.Text = "0";
                            txtTax.Text = "11";
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

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

                        int recordID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["ID"].Value);

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandText = "DELETE FROM Daily_Bills WHERE id = @id";
                                command.Parameters.AddWithValue("@id", recordID);
                                command.ExecuteNonQuery();

                                cboClientsName.Enabled = false;
                                if (cboClientsName.Items.Count > 0)
                                {
                                    cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                                }
                                cboItems.SelectedIndex = -1;
                                nudQuantity.Value = 0;
                                txtTotal.Text = "0";
                                txtTax.Text = "11";
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

        

        private void UpdateTotal(object sender, EventArgs e)
        {
            try
            {
                // Check if an item is selected in the ComboBox
                if (cboItems.SelectedItem == null)
                {
                    //MessageBox.Show("Please select an item.");
                    //Console.WriteLine("DEBUG: No item selected in the ComboBox.");
                    return;
                }

                Console.WriteLine("DEBUG: Item selected in the ComboBox: " + cboItems.SelectedItem.ToString());

                // Parse quantity from the NumericUpDown control
                int quantity = (int)Math.Round(nudQuantity.Value);

                Console.WriteLine("DEBUG: Quantity selected: " + quantity);

                // Fetch the selected item's name from the ComboBox
                string selectedItemName = cboItems.SelectedItem.ToString();

                Console.WriteLine("DEBUG: Selected item name: " + selectedItemName);

                // Fetch the price from the Items table based on the selected item's name
                decimal price = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT price FROM Items WHERE type = @itemName", connection))
                    {
                        command.Parameters.AddWithValue("@itemName", selectedItemName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            price = Convert.ToDecimal(result);
                        }
                        else
                        {
                            MessageBox.Show("Price not found for the selected item.");
                            Console.WriteLine("DEBUG: Price not found for the selected item.");
                            return;
                        }
                    }
                }

                Console.WriteLine("DEBUG: Price for selected item: " + price);

                // Calculate total without tax
                decimal totalWithoutTax = quantity * price;

                // Apply tax
                decimal taxRate = decimal.Parse(txtTax.Text);
                decimal taxAmount = totalWithoutTax * (taxRate / 100);

                // Calculate total including tax
                decimal total = totalWithoutTax + taxAmount;

                // Update txtTotal with the calculated total
                txtTotal.Text = total.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error calculating total: " + ex.Message);
                Console.WriteLine("DEBUG: Error calculating total: " + ex.Message);
            }

            /*try
            {
                // Check if an item is selected in the ComboBox
                if (cboItems.SelectedItem == null)
                {
                    //MessageBox.Show("Please select an item.");
                    //Console.WriteLine("DEBUG: No item selected in the ComboBox.");
                    return;
                }

                Console.WriteLine("DEBUG: Item selected in the ComboBox: " + cboItems.SelectedItem.ToString());

                // Parse quantity from the NumericUpDown control
                int quantity = (int)Math.Round(nudQuantity.Value);

                Console.WriteLine("DEBUG: Quantity selected: " + quantity);

                // Fetch the selected item's name from the ComboBox
                string selectedItemName = cboItems.SelectedItem.ToString();

                Console.WriteLine("DEBUG: Selected item name: " + selectedItemName);

                // Fetch the price from the Items table based on the selected item's name
                decimal price = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT price FROM Items WHERE type = @itemName", connection))
                    {
                        command.Parameters.AddWithValue("@itemName", selectedItemName);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            price = Convert.ToDecimal(result);
                        }
                        else
                        {
                            MessageBox.Show("Price not found for the selected item.");
                            Console.WriteLine("DEBUG: Price not found for the selected item.");
                            return;
                        }
                    }
                }

                Console.WriteLine("DEBUG: Price for selected item: " + price);

                // Calculate total without tax
                decimal totalWithoutTax = quantity * price;

                // Apply tax
                decimal taxRate = decimal.Parse(txtTax.Text);
                decimal taxAmount = totalWithoutTax * (taxRate / 100);

                // Calculate total including tax
                decimal total = totalWithoutTax + taxAmount;

                // Update txtTotal with the calculated total
                txtTotal.Text = total.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating total: " + ex.Message);
            }*/


        }

        private void txtFilling_10_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtC_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void Sidebarpannel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnViewItems_Click(object sender, EventArgs e)
        {
            /* Form2 form2 = new Form2();
             form2.ShowDialog();*/

            PriceList form2 = new PriceList();
            form2.Show(this);
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtTax.Enabled = true;
        }

        private void cboClientsName_SelectedIndexChanged(object sender, EventArgs e)
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void nudQuantity_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotal(sender, e);

            if (nudQuantity.Value == 0)
            {
                nudQuantity.Value = 1;
            }

        }

        private void cboItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotal(sender, e);

        }

        private void cboClientFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();
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

        private void cboItemFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView3();

        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboClientFilter.SelectedIndex = -1;
            cboItemFilter.SelectedIndex = -1;
            dateFromFilter.Value = DateTime.Today;
            dateToFilter.Value = DateTime.Today;
            chkDateFilter.Checked = false;

            // Reload data without filtering
            LoadDataIntoDataGridView();
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            if (panel4.Visible)
            {
                
                panel6.Hide();
                panel7.Show();
                panel4.Hide();
                panel4.Visible = false;
                cboClientsName.Enabled = false;
                cboItems.Enabled = false;
                nudQuantity.Enabled = false;
                dateTimePicker1.Enabled = false;
                txtDescription.Enabled = false;
            }
            else
            {
                panel6.Show();
                panel7.Hide();
                
                panel4.Show();
                cboClientsName.Enabled = true;
                //cboItems.Enabled = true;
                //nudQuantity.Enabled=true;
                //dateTimePicker1.Enabled = true;
                //txtDescription.Enabled = true;
                cboClientsName.Enabled = true;
                cboItems.Enabled = true;
                nudQuantity.Enabled = true;
                dateTimePicker1.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            object[] rowData = new object[] {
            cboClientsName.Text,
            cboItems.Text,
            nudQuantity.Value,
            dateTimePicker1.Value,
            txtTax.Text,
            txtTotal.Text,
            txtDescription.Text
                                };

            // Add the row to the DataGridView
            dataGridView2.Rows.Add(rowData);

            // Clear input controls after adding data
            //cboClientsName.SelectedIndex = -1;
            cboItems.SelectedIndex = -1;
            nudQuantity.Value = 1;
            dateTimePicker1.Value = DateTime.Now;
            txtTax.Text = "11";
            txtTotal.Clear();
            txtDescription.Clear();

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

                    cboClientsName.Text = selectedRow.Cells["c_name"].Value.ToString();
                    cboItems.Text = selectedRow.Cells["Items"].Value.ToString();
                    nudQuantity.Text = selectedRow.Cells["Quantity"].Value.ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(selectedRow.Cells["d_t"].Value);
                    txtTax.Text = selectedRow.Cells["Tax"].Value.ToString();
                    txtTotal.Text = selectedRow.Cells["Total"].Value.ToString();
                    txtDescription.Text = selectedRow.Cells["desc"].Value.ToString();

                }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        foreach (DataGridViewRow row in dataGridView2.Rows)
                        {
                            if (row.Cells["c_name"].Value != null &&
                                row.Cells["Items"].Value != null &&
                                row.Cells["Quantity"].Value != null &&
                                row.Cells["d_t"].Value != null &&
                                row.Cells["tax"].Value != null &&
                                row.Cells["Total"].Value != null &&
                                row.Cells["desc"].Value != null)
                            {
                                // Insert into Daily_Bills table
                                command.CommandText = "INSERT INTO Daily_Bills (client_name, Items, Quantity, date_time, tax, Total, Description) " +
                                                      "VALUES (@client_name, @Items, @Quantity, @date_time, @tax, @Total, @Description)";

                                // Add parameter values for Daily_Bills
                                command.Parameters.Clear(); // Clear parameters from previous iteration
                                command.Parameters.AddWithValue("@client_name", row.Cells["c_name"].Value.ToString());
                                command.Parameters.AddWithValue("@Items", row.Cells["Items"].Value.ToString());
                                command.Parameters.AddWithValue("@Quantity", row.Cells["Quantity"].Value.ToString());
                                command.Parameters.AddWithValue("@date_time", Convert.ToDateTime(row.Cells["d_t"].Value));
                                command.Parameters.AddWithValue("@tax", row.Cells["tax"].Value.ToString());
                                command.Parameters.AddWithValue("@Total", row.Cells["Total"].Value.ToString());
                                command.Parameters.AddWithValue("@Description", row.Cells["desc"].Value.ToString());

                                command.ExecuteNonQuery();

                                if (checkBox1.Checked) // Check if checkbox is checked
                                {
                                    // Insert into Client_Payment table
                                    command.CommandText = "INSERT INTO Client_Payment (C_Name, payment_date, Payment) " +
                                                          "VALUES (@client_name, @date_time, @Total)";
                                    // Add parameters for Client_Payment
                                    command.Parameters.Clear(); // Clear parameters from previous iteration
                                    command.Parameters.AddWithValue("@client_name", row.Cells["c_name"].Value.ToString());
                                    command.Parameters.AddWithValue("@date_time", Convert.ToDateTime(row.Cells["d_t"].Value));
                                    command.Parameters.AddWithValue("@Total", row.Cells["Total"].Value.ToString());

                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    command.CommandText = "INSERT INTO Client_debts (C_name, Bill_Date, Payment) " +
                                              "VALUES (@client_name, @date_time, @Total)";
                                    // Add parameters for Client_debts
                                    command.Parameters.Clear(); // Clear parameters from previous iteration
                                    command.Parameters.AddWithValue("@client_name", row.Cells["c_name"].Value.ToString());
                                    command.Parameters.AddWithValue("@date_time", Convert.ToDateTime(row.Cells["d_t"].Value));
                                    command.Parameters.AddWithValue("@Total", row.Cells["Total"].Value.ToString());

                                    command.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show("Data saved successfully");

                        // Optionally, reload the data into dataGridView1 to reflect changes in the database
                        LoadDataIntoDataGridView();
                    }
                }

                // Clear dataGridView2 after saving data
                dataGridView2.Rows.Clear();
                panel4.Hide();

                // Optionally, clear input controls and set values
                cboClientsName.Enabled = false;
                if (cboClientsName.Items.Count > 0)
                {
                    cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                }
                cboItems.SelectedIndex = -1;
                nudQuantity.Value = 1;
                dateTimePicker1.Value = DateTime.Now;
                txtTax.Text = "11";
                txtTotal.Clear();
                txtDescription.Clear();
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
                         // Loop through each row in dataGridView2 and insert it into the database
                         foreach (DataGridViewRow row in dataGridView2.Rows)
                         {
                             if (row.Cells["c_name"].Value != null &&
                                 row.Cells["Items"].Value != null &&
                                 row.Cells["Quantity"].Value != null &&
                                 row.Cells["d_t"].Value != null &&
                                 row.Cells["tax"].Value != null &&
                                 row.Cells["Total"].Value != null &&
                                 row.Cells["desc"].Value != null)
                             {
                                 command.CommandText = "INSERT INTO Daily_Bills (client_name, Items, Quantity, date_time, tax, Total, Description) " +
                                                       "VALUES (@client_name, @Items, @Quantity, @date_time, @tax, @Total, @Description)";

                                 // Add parameter values for the current row
                                 command.Parameters.Clear(); // Clear parameters from previous iteration
                                 command.Parameters.AddWithValue("@client_name", row.Cells["c_name"].Value.ToString());
                                 command.Parameters.AddWithValue("@Items", row.Cells["Items"].Value.ToString());
                                 command.Parameters.AddWithValue("@Quantity", row.Cells["Quantity"].Value.ToString());
                                 command.Parameters.AddWithValue("@date_time", Convert.ToDateTime(row.Cells["d_t"].Value));
                                 command.Parameters.AddWithValue("@tax", row.Cells["tax"].Value.ToString());
                                 command.Parameters.AddWithValue("@Total", row.Cells["Total"].Value.ToString());
                                 command.Parameters.AddWithValue("@Description", row.Cells["desc"].Value.ToString());

                                 command.ExecuteNonQuery();
                                 panel6.Hide();
                                 panel7.Show();
                             }

                         }

                         MessageBox.Show("Data saved successfully");

                         // Optionally, reload the data into dataGridView1 to reflect changes in the database
                         LoadDataIntoDataGridView();
                     }
                 }

                 // Clear dataGridView2 after saving data
                 dataGridView2.Rows.Clear();
                 panel4.Hide();

                 // Optionally, clear input controls and set values
                 cboClientsName.Enabled = false;
                 if (cboClientsName.Items.Count > 0)
                 {
                     cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                 }
                 cboItems.SelectedIndex = -1;
                 nudQuantity.Value = 0;
                 dateTimePicker1.Value = DateTime.Now;
                 txtTax.Text = "11";
                 txtTotal.Clear();
                 txtDescription.Clear();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error: " + ex.Message);
             }*/
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                // Update the selected row with the values from input controls
                selectedRow.Cells["c_name"].Value = cboClientsName.Text;
                selectedRow.Cells["Items"].Value = cboItems.Text;
                selectedRow.Cells["Quantity"].Value = nudQuantity.Value;
                selectedRow.Cells["d_t"].Value = dateTimePicker1.Value;
                selectedRow.Cells["Tax"].Value = txtTax.Text;
                selectedRow.Cells["Total"].Value = txtTotal.Text;
                selectedRow.Cells["desc"].Value = txtDescription.Text;

                // Optionally, clear input controls after editing
                cboClientsName.Enabled = false;
                if (cboClientsName.Items.Count > 0)
                {
                    cboClientsName.SelectedIndex = 0; // Set the default selected index to the first item
                }
                cboItems.SelectedIndex = -1;
                nudQuantity.Value = 1;
                dateTimePicker1.Value = DateTime.Now;
                txtTax.Text = "11";
                txtTotal.Clear();
                txtDescription.Clear();

                MessageBox.Show("Row edited successfully.");
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                // Remove the selected row from dataGridView2
                dataGridView2.Rows.Remove(selectedRow);

                MessageBox.Show("Row deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void cboClientsName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void cboItems_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            Payments form1 = new Payments();
            form1.Show();
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //e.Graphics.DrawString("welcome to ali", new Font("arial", 12, FontStyle.Regular), Brushes.Black, new Point(10, 10));

            

            // Set up initial y coordinate for printing
            int yPos = 100;

            // Print header
            e.Graphics.DrawString("Invoice", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, new PointF(10, yPos));
            yPos += 30;

            // Print column headers
            e.Graphics.DrawString("Client Name", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(10, yPos));
            e.Graphics.DrawString("Payment Date", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(200, yPos));
            e.Graphics.DrawString("Payment Amount", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(350, yPos));
            yPos += 20;

            // Print data from DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow) 
                {
                    string clientName = row.Cells["client_name"].Value?.ToString();
                    string paymentDate = row.Cells["date_time"].Value?.ToString();
                    string paymentAmount = row.Cells["Total"].Value?.ToString();

                    if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(paymentDate) && !string.IsNullOrEmpty(paymentAmount))
                    {
                        e.Graphics.DrawString(clientName, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new PointF(10, yPos));
                        e.Graphics.DrawString(paymentDate, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new PointF(200, yPos));
                        e.Graphics.DrawString(paymentAmount, new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new PointF(350, yPos));
                        yPos += 20; 
                    }
                }
            }
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // If checked, make the button visible
                btnInvoice.Visible = true;
               

            }
            else
            {
                // If not checked, hide the button
                btnInvoice.Visible = false;
                
            }
        }

        private void button12_Click(object sender, EventArgs e)
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

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            dashboard Dashboard = new dashboard();
            Dashboard.Show();
            this.Hide();
        }
    }

}



