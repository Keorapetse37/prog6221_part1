using System;
using System.Collections.Generic;

namespace CyberBot_GUI
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; } = string.Empty;

        public QuizQuestion(string questionText, List<string> options, int correctIndex, string explanation)
        {
            QuestionText = questionText;
            Options = options;
            CorrectIndex = correctIndex;
            Explanation = explanation;
        }
    }
}