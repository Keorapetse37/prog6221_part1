using System;
using System.Collections.Generic;

namespace CyberBot_GUI
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;

        public int Score => _score;
        public int TotalQuestions => _questions.Count;
        public int CurrentQuestionNumber => _currentIndex + 1;
        public bool IsFinished => _currentIndex >= _questions.Count;

        public QuizManager()
        {
            _questions = BuildQuestions();
        }

        // Reset to the beginning for a fresh run
        public void Start()
        {
            _currentIndex = 0;
            _score = 0;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            return _questions[_currentIndex];
        }

        // Check the chosen answer; report correctness and the explanation
        public bool SubmitAnswer(int chosenIndex, out string explanation)
        {
            QuizQuestion q = _questions[_currentIndex];
            explanation = q.Explanation;
            bool correct = chosenIndex == q.CorrectIndex;
            if (correct)
            {
                _score++;
            }
            return correct;
        }

        public void MoveNext()
        {
            _currentIndex++;
        }

        // End-of-quiz message based on how they did
        public string GetFinalFeedback()
        {
            double percent = (double)_score / _questions.Count * 100;
            string verdict;
            if (percent >= 80)
                verdict = "Great job! You're a cybersecurity pro!";
            else if (percent >= 50)
                verdict = "Good effort! A little more practice and you'll be solid.";
            else
                verdict = "Keep learning to stay safe online!";

            return $"Quiz complete! You scored {_score} out of {_questions.Count}. {verdict}";
        }

        private List<QuizQuestion> BuildQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new List<string> { "Reply with your password", "Delete and ignore it", "Report it as phishing", "Forward it to friends" },
                    2,
                    "Reporting phishing emails helps prevent scams and warns your provider."),

                new QuizQuestion(
                    "True or False: Using the same strong password everywhere is safe.",
                    new List<string> { "True", "False" },
                    1,
                    "False. If one site is breached, every account sharing that password is exposed."),

                new QuizQuestion(
                    "Which is the strongest password?",
                    new List<string> { "Password123", "Your birthday", "A long random passphrase", "Your pet's name" },
                    2,
                    "Length and randomness beat everything. A long passphrase is hard to crack and easier to remember."),

                new QuizQuestion(
                    "True or False: Public Wi-Fi is always safe for online banking.",
                    new List<string> { "True", "False" },
                    1,
                    "False. Public networks can be intercepted. Use mobile data or a VPN for sensitive tasks."),

                new QuizQuestion(
                    "What is phishing?",
                    new List<string> { "A type of firewall", "Tricking you into revealing info by posing as someone trusted", "A secure backup method", "A password manager" },
                    1,
                    "Phishing impersonates trusted entities to steal credentials or money."),

                new QuizQuestion(
                    "A friend sends an unexpected link. What's safest?",
                    new List<string> { "Click it immediately", "Verify with them through another channel first", "Share it with others", "Enter your login to see it" },
                    1,
                    "Accounts get hijacked. Confirm out-of-band before clicking unexpected links."),

                new QuizQuestion(
                    "True or False: Two-factor authentication adds an extra layer of security.",
                    new List<string> { "True", "False" },
                    0,
                    "True. Even if your password leaks, 2FA blocks login without the second factor."),

                new QuizQuestion(
                    "What does HTTPS in a web address indicate?",
                    new List<string> { "The site is free", "The connection is encrypted", "The site is government-run", "The page loads faster" },
                    1,
                    "HTTPS means traffic is encrypted, so data in transit can't be easily read."),

                new QuizQuestion(
                    "True or False: Software updates only add features and don't affect security.",
                    new List<string> { "True", "False" },
                    1,
                    "False. Updates frequently patch security holes. Installing them promptly matters."),

                new QuizQuestion(
                    "Social engineering attacks mainly target what?",
                    new List<string> { "Server hardware", "Human trust and psychology", "Network cables", "Printer drivers" },
                    1,
                    "Social engineering manipulates people rather than breaking systems directly."),

                new QuizQuestion(
                    "What's the best response to a ransomware demand?",
                    new List<string> { "Pay immediately", "Disconnect, report it, and restore from backup", "Ignore it and keep working", "Email the attacker" },
                    1,
                    "Paying funds crime and rarely guarantees recovery. Isolate, report, and restore from backups."),

                new QuizQuestion(
                    "True or False: A password manager helps you use unique strong passwords.",
                    new List<string> { "True", "False" },
                    0,
                    "True. It generates and stores strong, unique passwords so you don't have to remember them.")
            };
        }
    }
}