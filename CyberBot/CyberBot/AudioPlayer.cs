using System;
using System.Media;
using System.IO;

namespace CyberBot
{
    public class AudioPlayer
    {
        public void PlayGreeting()
        {
            try
            {
                // Requires System.Media (Windows only). Ensure greeting.wav is copied to the output directory.
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.Play();
                }
                else
                {
                    ChatbotUI.TypeLine("[System: Audio file 'greeting.wav' not found. Skipping voice greeting.]", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                ChatbotUI.TypeLine($"[System: Could not play audio: {ex.Message}]", ConsoleColor.Red);
            }
        }
    }

}