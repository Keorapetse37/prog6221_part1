using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberBot_GUI
{
    public class ActivityLogger
    {
        private List<LogEntry> _entries = new List<LogEntry>();

        // Record a new action
        public void Log(string description)
        {
            _entries.Add(new LogEntry(description));
        }

        // Build a readable summary of the last few actions, newest first
        public string GetSummary(int count = 10)
        {
            if (_entries.Count == 0)
            {
                return "No activity recorded yet.";
            }

            // Take the last `count` entries, then reverse so newest shows first
            int skip = Math.Max(0, _entries.Count - count);
            List<LogEntry> recent = _entries.Skip(skip).ToList();
            recent.Reverse();

            string summary = "Here's a summary of recent actions:";
            foreach (LogEntry entry in recent)
            {
                summary += $"\n- [{entry.Timestamp:yyyy-MM-dd HH:mm}] {entry.Description}";
            }
            return summary;
        }
    }
}