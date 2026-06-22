# Cybersecurity Awareness Chatbot

A Windows desktop assistant that educates South African citizens on cybersecurity. Built with **WPF (.NET 10)**, it greets users with a voice message and an ASCII logo, answers questions on topics like phishing, passwords, scams and privacy, and (in Part 3) adds a database-backed task assistant, an interactive quiz, an activity log, and a natural-language command layer.

This repository contains all three parts of the project. The graded Part 3 application is the WPF project in `CyberBot/CyberBot_GUI`.

---

## Features

### Carried over from Parts 1 & 2
- **Voice greeting** played on launch (`greeting.wav` via `System.Media`).
- **ASCII art logo** shown in the window header.
- **Keyword recognition** for cybersecurity topics (password, scam, privacy) using a dictionary.
- **Random phishing tips** selected from a list for variety.
- **Sentiment detection** that adjusts replies when a user sounds worried or frustrated.
- **Conversation memory** (remembers the user's name and topic of interest).
- **Input validation** with a friendly fallback response.

### New in Part 3
- **Task Assistant with MySQL storage.** Add cybersecurity tasks with a title, description and optional reminder. Tasks are stored in a MySQL database and persist between sessions. View them in a list, mark them complete, or delete them.
- **Cybersecurity Quiz.** A 12-question quiz (multiple-choice and true/false) presented one question at a time, with immediate feedback, an explanation for each answer, live score tracking and a final result message.
- **Activity Log.** Records actions the bot takes (tasks added/completed/deleted, quiz started/completed, chat commands) with timestamps. View the last 10 actions by typing `show activity log`.
- **Natural Language Processing (NLP) simulation.** Recognises commands phrased in different ways using keyword detection and regular expressions. For example, typing *"remind me to enable two-factor authentication in 3 days"* creates a task with a 3-day reminder, and *"start the quiz"* launches the quiz.

---

## Prerequisites

| Requirement | Version |
|---|---|
| Operating System | Windows (required for `System.Media` audio and WPF) |
| IDE | Visual Studio 2022 (or newer) |
| Framework | .NET 10.0 SDK |
| Database | MySQL Server 8.0 (MySQL Workbench 8.0 CE used for setup) |
| NuGet package | `MySql.Data` (MySQL Connector/NET) |

---

## Setup

### 1. Clone the repository
```bash
git clone https://github.com/Keorapetse37/prog6221_part1.git
```

### 2. Create the database
The task assistant needs a MySQL database. Open MySQL Workbench, connect to your local instance, open a query tab, and run:

```sql
CREATE DATABASE IF NOT EXISTS CyberBotDB;
USE CyberBotDB;

CREATE TABLE IF NOT EXISTS tasks (
    Id            INT AUTO_INCREMENT PRIMARY KEY,
    Title         VARCHAR(255) NOT NULL,
    Description   TEXT,
    ReminderDate  DATETIME NULL,
    IsCompleted   BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt     TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
```

### 3. Set your database password
Open `CyberBot/CyberBot_GUI/TaskRepository.cs` and edit the connection string with **your own** MySQL root password:

```csharp
private readonly string _connectionString =
    "server=localhost;port=3306;database=CyberBotDB;user=root;password=YOUR_PASSWORD;";
```

> **Security note:** the password is stored in plain text here for assignment simplicity. In a real application this would be moved to a configuration file or environment variable and kept out of source control. Replace `YOUR_PASSWORD` with your password locally; do not commit your real password.

### 4. Restore the NuGet package
Open the solution in Visual Studio. The `MySql.Data` package should restore automatically on build. If it doesn't, right-click the `CyberBot_GUI` project → **Manage NuGet Packages** → **Browse** → search `MySql.Data` → **Install**.

---

## How to Run

1. Open `CyberBot/CyberBot.slnx` in Visual Studio 2022.
2. Set **CyberBot_GUI** as the startup project (right-click it → Set as Startup Project).
3. Press **F5** (or the green Run button).
4. The window opens with the voice greeting and ASCII logo. Enter your name on the **Chat** tab to begin.

### Things to try
- **Chat tab:** `What can I ask you about?`, `Tell me about passwords`, `I'm worried about scams`, `show activity log`.
- **Chat tab (NLP):** `remind me to update my password in 3 days`, `add a task to review privacy settings`, `start the quiz`.
- **Tasks tab:** add, complete and delete tasks; reminders show their due date.
- **Quiz tab:** press **Start Quiz** and answer the questions.

---

## Project Structure

```
CyberBot/
├── CyberBot/              # Part 1 console application
└── CyberBot_GUI/          # Part 3 WPF application (the graded project)
    ├── MainWindow.xaml        # UI: Chat, Tasks and Quiz tabs
    ├── MainWindow.xaml.cs      # UI logic and event handlers
    ├── ChatbotLogic.cs         # Chat brain: keywords, sentiment, NLP routing
    ├── TaskItem.cs             # Task data model
    ├── TaskRepository.cs        # MySQL CRUD operations
    ├── QuizQuestion.cs         # Quiz question model
    ├── QuizManager.cs          # Quiz state and scoring
    ├── LogEntry.cs             # Activity log entry model
    ├── ActivityLogger.cs       # Activity log storage and summary
    └── greeting.wav            # Voice greeting audio
```

---

## Continuous Integration

This repository uses GitHub Actions to build the project on every push. The workflow is defined in `.github/workflows/dotnet.yml`.

<!-- TODO: replace the line below with a screenshot of a passing Actions run (green check mark). -->
![CI status](docs/ci-passing.png)

---

## Technologies & Concepts Demonstrated

- WPF with XAML, `TabControl`, `ListView`/`GridView` data binding
- Generic collections: `Dictionary`, `List`
- Delegates and callbacks (`BotResponseHandler`, `Action`) for decoupling UI from logic
- MySQL integration with parameterised queries (SQL-injection safe)
- Regular expressions for extracting intent and reminder timeframes from free text
- Object-oriented design with separate model, repository, manager and logic classes
