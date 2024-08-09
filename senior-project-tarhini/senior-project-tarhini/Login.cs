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

namespace senior_project_tarhini
{
    public partial class Login : Form
    {

        private const string connectionString = "Data Source=DESKTOP-9TFICR1;Initial Catalog=senior;Integrated Security=True";
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (AuthenticateUser(username, password))
            {
                // If authentication succeeds, open Daily
                Daily Daily = new Daily();
                Daily.Show();
                this.Hide(); // Hide the login form
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Agents_info WHERE Username = @Username AND password2 = @password2";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@password2", password);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    isAuthenticated = count > 0;
                }
            }

            return isAuthenticated;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
