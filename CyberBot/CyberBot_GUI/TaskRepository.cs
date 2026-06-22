using System;
using MySql.Data.MySqlClient;

namespace CyberBot_GUI
{
    public class TaskRepository
    {

        private readonly string _connectionString =
            "server=localhost;port=3306;database=CyberBotDB;user=root;password=@Labs2026!;";

        public bool TestConnection(out string message)
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    message = "Connected to MySQL successfully!";
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = "Connection failed: " + ex.Message;
                return false;
            }
        }
    }
}