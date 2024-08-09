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
    public partial class DriverList : Form
    {
        public DriverList()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True"))
            {
                try
                {
                    conn.Open();

                    // Modify the SQL query to select specific columns
                    string query = "SELECT driver_name ,Address FROM delrviry_driver";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds, "t0");
                        dataGridView1.DataSource = ds.Tables["t0"];

                        // Auto size columns to fill the DataGridView
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

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
