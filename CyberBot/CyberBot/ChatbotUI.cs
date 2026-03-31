using System;
using System.Threading;

namespace CyberBot
{
    public class ChatbotUI
    {
        // Method for conversational typing effect
        public static void TypeLine(string text, ConsoleColor color = ConsoleColor.White, int delay = 20)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void DrawHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
   ______      __               ____        __ 
  / ____/_  __/ /_  ___  ____  / __ )____  / /_
 / /   / / / / __ \/ _ \/ ___// __  / __ \/ __/
/ /___/ /_/ / /_/ /  __/ /   / /_/ / /_/ / /_  
\____/\__, /_.___/\___/_/   /_____/\____/\__/  
     /____/                                    
            ");
            Console.WriteLine(new string('=', 50));
            Console.ResetColor();
        }
    }
}