using System;
using System.Collections.Generic;

namespace CyberBot_GUI
{
    public delegate void BotResponseHandler(string response);

    public class ChatbotLogic
    {
        private BotResponseHandler _onRespond;

        private string userName = string.Empty;
        private string currentTopic = string.Empty;
        private string favoriteTopic = string.Empty;

        private Dictionary<string, string> keywordResponses;
        private List<string> phishingTips;

        public ChatbotLogic(BotResponseHandler onRespond)
        {
            _onRespond = onRespond;
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
            input = input.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(input))
            {
                _onRespond("I didn't quite catch that. Could you please type something?");
                return;
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = input;
                _onRespond($"Nice to meet you, {userName}! What cybersecurity topic can I help you with today? (e.g., passwords, phishing, scams)");
                return;
            }
            // 
            if (DetectSentiment(input, out string sentimentResponse))
            {
                _onRespond(sentimentResponse);
                return;
            }

            if (input.Contains("more") || input.Contains("another") || input.Contains("explain"))
            {
                HandleFollowUp();
                return;
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;
                    favoriteTopic = keyword;
                    _onRespond(keywordResponses[keyword]);
                    return;
                }
            }

            if (input.Contains("phishing"))
            {
                currentTopic = "phishing";
                Random rand = new Random();
                _onRespond(phishingTips[rand.Next(phishingTips.Count)]);
                return;
            }

            _onRespond($"I'm still learning! You can ask me about passwords, phishing, scams, or privacy. As someone interested in {favoriteTopic}, what else would you like to know?");
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