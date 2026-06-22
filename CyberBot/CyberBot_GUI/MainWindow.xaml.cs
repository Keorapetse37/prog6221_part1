using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberBot_GUI
{
    public partial class MainWindow : Window
    {
        private ChatbotLogic botLogic;
        private TaskRepository taskRepo = new TaskRepository();
        private ActivityLogger activityLog = new ActivityLogger();
        private QuizManager quizManager = new QuizManager();

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

            string reminderText = task.ReminderDate.HasValue
                ? $" (reminder {task.ReminderDate.Value:yyyy-MM-dd})"
                : " (no reminder)";
            activityLog.Log($"Task added: '{task.Title}'{reminderText}");

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
                activityLog.Log($"Task completed: '{selected.Title}'");
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
                activityLog.Log($"Task deleted: '{selected.Title}'");
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Select a task first.");
            }
        }

        // ===== Quiz tab =====

        // Convenience: the four option buttons as an array so we can loop them
        private Button[] OptionButtons => new[] { QuizOption0, QuizOption1, QuizOption2, QuizOption3 };

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.Start();
            activityLog.Log("Quiz started.");
            StartQuizButton.Visibility = Visibility.Collapsed;
            DisplayCurrentQuestion();
        }

        // Show the current question and set up the answer buttons
        private void DisplayCurrentQuestion()
        {
            QuizQuestion q = quizManager.GetCurrentQuestion();

            QuizScoreText.Text = $"Question {quizManager.CurrentQuestionNumber} of {quizManager.TotalQuestions}   |   Score: {quizManager.Score}";
            QuizQuestionText.Text = q.QuestionText;
            QuizFeedbackText.Text = "";
            NextQuestionButton.Visibility = Visibility.Collapsed;

            // Fill the buttons that have options; hide the rest
            for (int i = 0; i < OptionButtons.Length; i++)
            {
                if (i < q.Options.Count)
                {
                    OptionButtons[i].Content = q.Options[i];
                    OptionButtons[i].Visibility = Visibility.Visible;
                    OptionButtons[i].IsEnabled = true;
                }
                else
                {
                    OptionButtons[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void QuizOption_Click(object sender, RoutedEventArgs e)
        {
            // Which button was clicked? Its Tag holds the option index
            Button clicked = (Button)sender;
            int chosenIndex = int.Parse(clicked.Tag.ToString());

            bool correct = quizManager.SubmitAnswer(chosenIndex, out string explanation);

            QuizFeedbackText.Text = correct
                ? $"Correct! {explanation}"
                : $"Not quite. {explanation}";

            // Lock the buttons so they can't answer twice, then offer Next
            foreach (Button b in OptionButtons)
            {
                b.IsEnabled = false;
            }
            QuizScoreText.Text = $"Question {quizManager.CurrentQuestionNumber} of {quizManager.TotalQuestions}   |   Score: {quizManager.Score}";
            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            quizManager.MoveNext();

            if (quizManager.IsFinished)
            {
                // Wrap up: show final score, hide the quiz controls, allow a restart
                foreach (Button b in OptionButtons)
                {
                    b.Visibility = Visibility.Collapsed;
                }
                QuizQuestionText.Text = quizManager.GetFinalFeedback();
                QuizFeedbackText.Text = "";
                QuizScoreText.Text = "";
                NextQuestionButton.Visibility = Visibility.Collapsed;
                StartQuizButton.Content = "Restart Quiz";
                StartQuizButton.Visibility = Visibility.Visible;

                activityLog.Log($"Quiz completed. Score: {quizManager.Score}/{quizManager.TotalQuestions}.");
            }
            else
            {
                DisplayCurrentQuestion();
            }
        }
    }
}