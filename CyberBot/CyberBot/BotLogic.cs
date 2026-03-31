using System;

namespace CyberBot
{
    public class BotLogic
    {
        // Automatic Property requirement
        public string UserName { get; set; }

        public BotLogic()
        {
            UserName = "User"; // Default
        }

        public string GetResponse(string input)
        {
            // Input Validation & String Manipulation requirement
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't quite understand that. Could you rephrase or ask a specific question?";
            }

            string cleanInput = input.Trim().ToLower();

            // Basic Response System
            if (cleanInput.Contains("how are you"))
                return $"I am operating at 100% efficiency, {UserName}. How can I help you secure your digital life today?";

            if (cleanInput.Contains("purpose") || cleanInput.Contains("what are you"))
                return "I am a Cybersecurity Awareness Bot. My purpose is to educate South African citizens on digital threats.";

            if (cleanInput.Contains("phishing"))
                return "Phishing is a cyberattack where scammers pretend to be trusted entities to trick you into revealing sensitive information. Always verify sender email addresses and never click suspicious links!";

            if (cleanInput.Contains("password"))
                return "Safe password practices include using a minimum of 12 characters, mixing uppercase, lowercase, numbers, and symbols. Never reuse passwords across different sites.";

            if (cleanInput.Contains("safe browsing") || cleanInput.Contains("link"))
                return "When browsing, always look for 'https://' in the URL. Avoid clicking links from unknown senders, and be wary of pop-ups claiming you have a virus.";

            if (cleanInput == "exit")
                return "Goodbye! Stay safe online!";

            // Default fallback response
            return $"I'm sorry {UserName}, I don't have information on that specific topic yet. Try asking me about phishing, passwords, or safe browsing.";
        }
    }
}