using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.services
{
    public class DB : IDisposable
    {
        private readonly string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        private readonly string fullpath;
        private readonly string connectionString;
        private readonly SQLiteConnection conn;

        public DB()
        {
            fullpath = Path.Combine(dbPath, "database.db");
            connectionString = $"Data Source={fullpath};Version=3;";

            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }
            if (!File.Exists(fullpath))
            {
                using (File.Create(fullpath)) { }
            }

            conn = new SQLiteConnection(connectionString);
            conn.Open();
        }

        public void Execute(string query, params string[] parameters)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@param{i}", parameters[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }


        public bool TableExists(string tableName)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            string query = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@tableName;";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
        }


        public string[] Get(string query, params string[] parameters)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@param{i}", parameters[i]);
                }

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        int columnCount = reader.FieldCount;
                        string[] result = new string[columnCount];

                        for (int i = 0; i < columnCount; i++)
                        {
                            result[i] = reader[i]?.ToString() ?? string.Empty; 
                        }

                        return result; 
                    }
                }
            }

            return new string[0]; 
        }



        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();
                conn.Dispose();
            }
        }

    }
}

