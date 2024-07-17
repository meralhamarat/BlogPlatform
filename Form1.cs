using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Windows.Forms;

namespace BlogPlatform
{
    public partial class Form1 : Form
    {
        private SqliteConnection sqliteConnection;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadBlogPosts();
        }

        private void InitializeDatabase()
        {
            sqliteConnection = new SqliteConnection("Data Source=blog.db;");
            sqliteConnection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS BlogPosts (Id INTEGER PRIMARY KEY, Title TEXT, Content TEXT)";
            using (SqliteCommand command = new SqliteCommand(createTableQuery, sqliteConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        private void LoadBlogPosts()
        {
            string selectQuery = "SELECT * FROM BlogPosts";
            using (SqliteCommand command = new SqliteCommand(selectQuery, sqliteConnection))
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void buttonAddPost_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text;
            string content = textBoxContent.Text;

            string insertQuery = "INSERT INTO BlogPosts (Title, Content) VALUES (@title, @content)";
            using (SqliteCommand command = new SqliteCommand(insertQuery, sqliteConnection))
            {
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@content", content);
                command.ExecuteNonQuery();
            }

            LoadBlogPosts();

            textBoxTitle.Clear();
            textBoxContent.Clear();
        }
    }
}
