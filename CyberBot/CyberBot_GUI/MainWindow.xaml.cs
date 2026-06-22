using System;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace CyberBot_GUI
{
    public partial class MainWindow : Window
    {
        private ChatbotLogic botLogic;
        private TaskRepository taskRepo = new TaskRepository();
        private ActivityLogger activityLog = new ActivityLogger();

        public MainWindow()
        {
            InitializeComponent();
            botLogic = new ChatbotLogic(new BotResponseHandler(DisplayBotMessage), activityLog);
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

            RefreshTaskList();
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

        // ===== Tasks tab =====

        // Reloads the task list from the database into the ListView
        private void RefreshTaskList()
        {
            TaskListView.ItemsSource = null;
            TaskListView.ItemsSource = taskRepo.GetAllTasks();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            var task = new TaskItem
            {
                Title = title,
                Description = TaskDescBox.Text.Trim()
            };

            
            if (int.TryParse(TaskReminderBox.Text.Trim(), out int days))
            {
                task.ReminderDate = DateTime.Now.AddDays(days);
            }

            taskRepo.AddTask(task);

            
            TaskTitleBox.Clear();
            TaskDescBox.Clear();
            TaskReminderBox.Clear();
            RefreshTaskList();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is TaskItem selected)
            {
                taskRepo.SetCompleted(selected.Id, true);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Select a task first.");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListView.SelectedItem is TaskItem selected)
            {
                taskRepo.DeleteTask(selected.Id);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Select a task first.");
            }
        }
    }
}