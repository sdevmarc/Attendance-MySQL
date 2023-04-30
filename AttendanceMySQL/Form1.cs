using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AttendanceMySQL
{
    public partial class Form1 : Form
    {
        private readonly string _connectionString = "server=localhost;username=root;database=db_attendance;password=tite";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tsDT.Text = DateTime.Now.ToString();
            txtID.Focus();
        }

        private void IDEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            
            var sql = "SELECT * FROM tbl_students WHERE id_number = @id";

            var connection = new MySqlConnection(_connectionString);
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", txtID.Text);

            try
            {
                connection.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    MessageBox.Show($"The student {reader.GetString("Name")} from {reader.GetString("Course")} is Present!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtID.Text = "";
                    txtID.Focus();
                }
                else
                {
                    txtName.Enabled = true;
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NameEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCourse.Enabled = true;
                txtCourse.Focus();
            }
        }

        private void CourseEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            var sql = "INSERT INTO tbl_students (id_number, Name, Course) VALUES (@id, @name, @course)";

            var connection = new MySqlConnection(_connectionString);
            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", txtID.Text);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@course", txtCourse.Text);

            try
            {
                connection.Open();
                var i = cmd.ExecuteNonQuery();

                if (i == 1)
                {
                    MessageBox.Show("The student has been registered!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtID.Focus();
                    txtName.Enabled = false;
                    txtCourse.Enabled = false;
                    txtID.Text = "";
                    txtName.Text = "";
                    txtCourse.Text = "";
                }
                else
                {
                    MessageBox.Show("Error Encountered!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            var sql = "DELETE FROM tbl_students";

            var connection = new MySqlConnection(_connectionString);
            var cmd = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                var i = cmd.ExecuteNonQuery();

                MessageBox.Show($"{i} rows have been deleted from the table!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
