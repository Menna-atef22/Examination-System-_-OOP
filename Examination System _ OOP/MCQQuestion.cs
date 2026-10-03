using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// A multiple-choice question: the student chooses one answer by its number.
    /// The answers must be different from each other.
    /// </summary>
    public class MCQQuestion : Question
    {
        private Answer[] answers = Array.Empty<Answer>();


        /// <summary>The choices of the question, numbered from 1.</summary>
        public Answer[] Answers
        {
            get { return answers; }
            set { answers = value; }
        }

        /// <summary>Builds a question from ready values. Used by <see cref="Clone"/>.</summary>
        /// <param name="header">The short title of the question.</param>
        /// <param name="body">The full text of the question.</param>
        /// <param name="mark">The marks of the question (more than 0).</param>
        /// <param name="answers">The choices (they are used as they are, not copied).</param>
        /// <param name="rightNumber">The number (starting at 1) of the right choice.</param>
        private MCQQuestion(string header, string body, double mark, Answer[] answers, int rightNumber)
            : base(header, body, mark)
        {
            Answers = answers;
            CorrectAnswer = answers[rightNumber - 1];
        }

        /// <summary>Creates a question by asking the teacher for its data, its answers and the right answer.</summary>
        public MCQQuestion() : base()
        {
            ReadBasicData();
            ReadAnswers();
        }

        /// <summary>
        /// Asks for the number of answers (2 or more), the text of each one (a repeated text is refused)
        /// and the number of the right answer.
        /// </summary>
        protected override void ReadAnswers()
        {
            int count = ConsoleInput.ReadInt("Number of answers (2 or more): ", 2);

            Answers = new Answer[count];
            for (int i = 0; i < count; i++)
            {
                string text = ConsoleInput.ReadText($"Answer {i + 1}: ");
                while (IsDuplicate(text, i))
                {
                    Console.WriteLine("This answer was already entered for this question, enter a different one.");
                    text = ConsoleInput.ReadText($"Answer {i + 1}: ");
                }
                Answers[i] = new Answer(i + 1, text);
            }

            int right = ConsoleInput.ReadInt($"Right answer number (1-{count}): ", 1, count);
            CorrectAnswer = Answers[right - 1];
        }

        /// <summary>Checks if a text is the same as one of the answers already entered (ignores upper/lower case and spaces).</summary>
        /// <param name="text">The new answer text.</param>
        /// <param name="count">How many answers (from the first one) were already entered.</param>
        /// <returns><c>true</c> if the text repeats one of them; otherwise <c>false</c>.</returns>
        private bool IsDuplicate(string text, int count)
        {
            for (int j = 0; j < count; j++)
            {
                if (string.Equals(Answers[j].AnswerText.Trim(), text.Trim(), StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Prints the question and its numbered choices.</summary>
        public override void DisplayQuestion()
        {
            Console.WriteLine("MCQ Question:");
            Console.WriteLine($"Question: {Header}");
            Console.WriteLine($"Body: {Body}");
            Console.WriteLine($"Mark: {Mark}");
            for (int i = 0; i < Answers.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Answers[i].AnswerText}");
            }
        }

        /// <summary>Asks the student for the number of his choice.</summary>
        /// <returns><c>true</c> if he chose the right answer; otherwise <c>false</c>.</returns>
        public override bool CheckAnswer()
        {
            int choice = ConsoleInput.ReadInt("Enter the number of your answer: ", 1, Answers.Length);
            return ((Answer)CorrectAnswer).AnswerId == choice;
        }

        /// <summary>Gets the right answer as "number. text", for example "2. Five".</summary>
        public override string GetCorrectAnswerText()
        {
            Answer right = (Answer)CorrectAnswer;
            return $"{right.AnswerId}. {right.AnswerText}";
        }

        /// <summary>
        /// Creates a deep copy: the answers are cloned too, so changing the copy never changes the original.
        /// </summary>
        /// <returns>The copy, as an <see cref="MCQQuestion"/> boxed in <see cref="object"/>.</returns>
        public override object Clone()
        {
            Answer[] newAnswers = new Answer[Answers.Length];
            for (int i = 0; i < Answers.Length; i++)
                newAnswers[i] = (Answer)Answers[i].Clone();

            return new MCQQuestion(Header, Body, Mark, newAnswers, ((Answer)CorrectAnswer).AnswerId);
        }

    }
}