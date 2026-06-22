using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberBot_GUI
{
    public delegate void BotResponseHandler(string response);

    public class ChatbotLogic
    {
        private BotResponseHandler _onRespond;
        private ActivityLogger _activityLog;
        private TaskRepository _taskRepo;

       
        private Action _onStartQuiz;
        private Action _onTasksChanged;

        private string userName = string.Empty;
        private string currentTopic = string.Empty;
        private string favoriteTopic = string.Empty;

        private Dictionary<string, string> keywordResponses = null!;
        private List<string> phishingTips = null!;

        public ChatbotLogic(BotResponseHandler onRespond, ActivityLogger activityLog,
                            TaskRepository taskRepo, Action onStartQuiz, Action onTasksChanged)
        {
            _onRespond = onRespond;
            _activityLog = activityLog;
            _taskRepo = taskRepo;
            _onStartQuiz = onStartQuiz;
            _onTasksChanged = onTasksChanged;
            InitializeKnowledgeBase();
        }

        private void InitializeKnowledgeBase()
        {
            keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", "Make sure to use strong, unique passwords for each account. Consider using a password manager!" },
                { "scam", "If an offer looks too good to be true, it probably is a scam. Never send money to unverified sources." },
                { "privacy", "Always check the privacy settings on your social media accounts to ensure you aren't oversharing." }
            };

            phishingTips = new List<string>
            {
                "Be cautious of emails asking for personal info. Scammers often disguise themselves as trusted entities.",
                "Hover over links in emails to see the actual URL before clicking.",
                "Look out for poor spelling or grammar in official-looking emails; it's a common sign of phishing."
            };
        }

        public void ProcessUserInput(string input)
        {
            string raw = input.Trim();            
            string lower = raw.ToLower();          

            if (string.IsNullOrWhiteSpace(lower))
            {
                _onRespond("I didn't quite catch that. Could you please type something?");
                return;
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = raw;
                _onRespond($"Nice to meet you, {userName}! Ask me about a topic, add a task, or start the quiz. (e.g., \"remind me to enable 2FA in 3 days\")");
                return;
            }

           
            if (lower.Contains("activity log") || lower.Contains("what have you done") || lower.Contains("show log"))
            {
                _onRespond(_activityLog.GetSummary());
                return;
            }

           
            if (lower.Contains("quiz") || lower.Contains("game") || lower.Contains("test me"))
            {
                _onRespond("Sure! Starting the cybersecurity quiz for you. Head to the Quiz tab if it doesn't jump there.");
                _activityLog.Log("Quiz started via chat command.");
                _onStartQuiz();
                return;
            }

            
            if (IsTaskIntent(lower))
            {
                HandleAddTaskIntent(raw, lower);
                return;
            }
           

            if (DetectSentiment(lower, out string sentimentResponse))
            {
                _onRespond(sentimentResponse);
                return;
            }

            if (lower.Contains("more") || lower.Contains("another") || lower.Contains("explain"))
            {
                HandleFollowUp();
                return;
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (lower.Contains(keyword))
                {
                    currentTopic = keyword;
                    favoriteTopic = keyword;
                    _onRespond(keywordResponses[keyword]);
                    return;
                }
            }

            if (lower.Contains("phishing"))
            {
                currentTopic = "phishing";
                Random rand = new Random();
                _onRespond(phishingTips[rand.Next(phishingTips.Count)]);
                return;
            }

            _onRespond($"I'm still learning! You can ask me about passwords, phishing, scams or privacy, add a task, or start the quiz.");
        }

       
        private bool IsTaskIntent(string lower)
        {
            return lower.Contains("add task") || lower.Contains("add a task")
                || lower.Contains("remind me") || lower.Contains("set a reminder")
                || lower.Contains("set reminder") || lower.Contains("create a task")
                || lower.Contains("new task");
        }

        
        private void HandleAddTaskIntent(string raw, string lower)
        {
            
            int? reminderDays = ExtractReminderDays(lower);

            
            string title = ExtractTaskTitle(raw);

            if (string.IsNullOrWhiteSpace(title))
            {
                _onRespond("Sure — what task would you like me to add? (e.g., \"add a task to review privacy settings\")");
                return;
            }

            var task = new TaskItem
            {
                Title = title,
                Description = "Added via chat assistant."
            };
            if (reminderDays.HasValue)
            {
                task.ReminderDate = DateTime.Now.AddDays(reminderDays.Value);
            }

            _taskRepo.AddTask(task);

            string reminderText = reminderDays.HasValue
                ? $" I'll remind you in {reminderDays.Value} day(s)."
                : " Would you like to set a reminder for it?";
            _onRespond($"Task added: '{title}'.{reminderText}");

            _activityLog.Log($"Task added via chat: '{title}'"
                + (reminderDays.HasValue ? $" (reminder in {reminderDays.Value} days)" : " (no reminder)"));

            _onTasksChanged();   
        }

       
        private int? ExtractReminderDays(string lower)
        {
            Match m = Regex.Match(lower, @"in\s+(\d+)\s*day");
            if (m.Success && int.TryParse(m.Groups[1].Value, out int days))
            {
                return days;
            }
            return null;
        }

        
        private string ExtractTaskTitle(string raw)
        {
            string text = raw;

           
            text = Regex.Replace(text, @"\s*in\s+\d+\s*days?\s*", " ", RegexOptions.IgnoreCase);

            
            string[] commandPhrases =
            {
                "add a task to", "add a task", "add task to", "add task",
                "create a task to", "create a task", "new task to", "new task",
                "remind me to", "remind me", "set a reminder to", "set a reminder",
                "set reminder to", "set reminder"
            };

            foreach (string phrase in commandPhrases)
            {
                if (text.ToLower().StartsWith(phrase))
                {
                    text = text.Substring(phrase.Length);
                    break;
                }
            }

            return text.Trim().TrimEnd('.', '!', '?');
        }

        private bool DetectSentiment(string input, out string response)
        {
            response = "";
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                response = "It's completely understandable to feel worried. Cyber threats can be intimidating, but I'm here to help! Let's look at a tip: Always verify who you are talking to online.";
                return true;
            }
            if (input.Contains("frustrated") || input.Contains("confused"))
            {
                response = "Take a deep breath! Technology can be frustrating. Let me break it down simply: just don't click on links from people you don't know.";
                return true;
            }
            return false;
        }

        private void HandleFollowUp()
        {
            if (currentTopic == "phishing")
            {
                Random rand = new Random();
                _onRespond("Here's another phishing tip: " + phishingTips[rand.Next(phishingTips.Count)]);
            }
            else if (!string.IsNullOrEmpty(currentTopic))
            {
                _onRespond($"To elaborate on {currentTopic}: It's a crucial part of your digital hygiene. Always stay alert and verify sources!");
            }
            else
            {
                _onRespond("I'd love to tell you more! What specific topic should we dive into?");
            }
        }
    }
}