using System;

namespace CyberBot_GUI
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? ReminderDate { get; set; }   // null = no reminder
        public bool IsCompleted { get; set; }
    }
}
