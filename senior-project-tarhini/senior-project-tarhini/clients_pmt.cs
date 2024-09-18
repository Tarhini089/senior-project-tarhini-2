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
    public partial class clients_pmt : Form
    {

        private string connection;
        private readonly string connectionString = "Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True";


        public clients_pmt()
        {
            InitializeComponent();

            timer1.Enabled = true; // Enable the Timer
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += timer1_Tick;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void PopulateclientidComboBox()
        {
            cboClienID.Items.Clear(); // Clear existing items

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT client_id FROM clients";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int clientid = (int)reader["client_id"];
                            cboClienID.Items.Add(clientid); // Add client_id directly to the ComboBox
                        }
                    }
                }
            }
        }

        private void LoadDataIntoDataGridView()
        {
            string query = "SELECT * FROM client_pmt";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "client_pmt");
            dataGridView1.DataSource = dataSet.Tables["client_pmt"];
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            PopulateclientidComboBox();

            using (SqlConnection conn = new SqlConnection("Data Source=TARHINIALI;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [client_pmt]", conn))
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSave_2_Click(object sender, EventArgs e)
        {
            try
            {
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO client_pmt (client_id, Fee_payed, pmt_date) VALUES (@client_id, @Fee_payed, @pmt_date)";
                        command.Parameters.AddWithValue("@client_id", cboClienID.Text);
                        command.Parameters.AddWithValue("@Fee_payed", txtFees.Text);
                        command.Parameters.AddWithValue("@pmt_date", dateTimePicker1.Value);

                        command.ExecuteNonQuery();

                        LoadDataIntoDataGridView();
                    }
                }

                MessageBox.Show("Data saved successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void cboClienID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtFees_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
