using System;

namespace CyberBot
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatbotUI.DrawHeader();

            AudioPlayer audio = new AudioPlayer();
            audio.PlayGreeting();

            ChatbotUI.TypeLine("Hello! Welcome to the Cybersecurity Awareness Bot.", ConsoleColor.Green);
            ChatbotUI.TypeLine("Before we begin, what is your name?", ConsoleColor.Green);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("> ");
            string nameInput = Console.ReadLine();
            Console.ResetColor();

            BotLogic bot = new BotLogic();
            if (!string.IsNullOrWhiteSpace(nameInput))
            {
                bot.UserName = nameInput.Trim(); // String manipulation
            }

            ChatbotUI.TypeLine($"\nNice to meet you, {bot.UserName}! I'm here to help you stay safe online.", ConsoleColor.Cyan);
            ChatbotUI.TypeLine("You can ask me about: Phishing, Passwords, Safe Browsing, or my Purpose. (Type 'exit' to quit)\n", ConsoleColor.DarkGray);

            // Interaction Loop
            bool isRunning = true;
            while (isRunning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{bot.UserName}] > ");
                string userInput = Console.ReadLine();
                Console.ResetColor();

                string response = bot.GetResponse(userInput);
                ChatbotUI.TypeLine($"[CyberBot] > {response}\n", ConsoleColor.Green);

                if (userInput?.Trim().ToLower() == "exit")
                {
                    isRunning = false;
                }
            }
        }
    }
}