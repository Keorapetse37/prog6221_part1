using System;

namespace CyberBot_GUI
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Description { get; set; } = string.Empty;

        public LogEntry(string description)
        {
            Timestamp = DateTime.Now;
            Description = description;
        }
    }
}