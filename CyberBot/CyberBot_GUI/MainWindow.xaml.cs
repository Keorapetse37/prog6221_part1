using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CyberBot_GUI
{
    public partial class MainWindow : Window
    {
        private ChatbotLogic botLogic;

        public MainWindow()
        {
            InitializeComponent();

            botLogic = new ChatbotLogic(new BotResponseHandler(DisplayBotMessage));

            AsciiHeader.Text = @"
  ___      _               ___       _   
 / __|_  _| |__  ___ _ _  | _ ) ___ | |_ 
| (__| || | '_ \/ -_) '_| | _ \/ _ \|  _|
 \___|\_, |_.__/\___|_|   |___/\___/ \__|
      |__/                               
";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Audio could not play: " + ex.Message);
            }

            DisplayBotMessage("Hello! Welcome to the Cybersecurity Awareness Bot. Please enter your name to get started:");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessInput();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            string userInput = InputBox.Text;
            if (string.IsNullOrWhiteSpace(userInput)) return;

            ChatBox.Items.Add($"You: {userInput}");
            InputBox.Clear();

            botLogic.ProcessUserInput(userInput);

            ChatScrollViewer.ScrollToEnd();
        }

        private void DisplayBotMessage(string message)
        {
            ChatBox.Items.Add($"Bot: {message}");
            ChatBox.Items.Add(""); 
        }
    }

}