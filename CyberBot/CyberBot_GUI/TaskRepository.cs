using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberBot_GUI
{
    public class TaskRepository
    {
       
        private readonly string _connectionString =
            "server=localhost;port=3306;database=CyberBotDB;user=root;password=@Labs2026!;";

       
        public int AddTask(TaskItem task)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = @"INSERT INTO tasks (Title, Description, ReminderDate, IsCompleted)
                               VALUES (@title, @desc, @reminder, @done);
                               SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@title", task.Title);
                    cmd.Parameters.AddWithValue("@desc", task.Description);
                    cmd.Parameters.AddWithValue("@reminder", (object?)task.ReminderDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@done", task.IsCompleted);

                   
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

       
        public List<TaskItem> GetAllTasks()
        {
            var tasks = new List<TaskItem>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "SELECT Id, Title, Description, ReminderDate, IsCompleted FROM tasks ORDER BY CreatedAt DESC;";

                using (var cmd = new MySqlCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                          ? string.Empty
                                          : reader.GetString("Description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("ReminderDate"))
                                           ? (DateTime?)null
                                           : reader.GetDateTime("ReminderDate"),
                            IsCompleted = reader.GetBoolean("IsCompleted")
                        });
                    }
                }
            }
            return tasks;
        }

       
        public void SetCompleted(int taskId, bool isCompleted)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "UPDATE tasks SET IsCompleted = @done WHERE Id = @id;";

                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@done", isCompleted);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        
        public void DeleteTask(int taskId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM tasks WHERE Id = @id;";

                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}