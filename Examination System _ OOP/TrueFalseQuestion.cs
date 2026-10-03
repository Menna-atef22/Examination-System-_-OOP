using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// A True/False question. Its right answer is stored as a <see cref="bool"/>.
    /// </summary>
    public class TrueFalseQuestion : Question
    {

        /// <summary>Builds a question from ready values. Used by <see cref="Clone"/>.</summary>
        /// <param name="header">The short title of the question.</param>
        /// <param name="body">The full text of the question.</param>
        /// <param name="mark">The marks of the question (more than 0).</param>
        /// <param name="correctAnswer">The right answer.</param>
        /// <exception cref="ArgumentException">The mark is zero or negative.</exception>
        public TrueFalseQuestion(string header, string body, double mark, bool correctAnswer) : base(header, body, mark)
        {
            CorrectAnswer = correctAnswer;
        }


        /// <summary>Creates a question by asking the teacher for its data and its right answer.</summary>
        public TrueFalseQuestion() : base()
        {
            ReadBasicData();
            ReadAnswers();
        }

        /// <summary>Prints the question (header, body and mark) for the student.</summary>
        public override void DisplayQuestion()
        {
            Console.WriteLine("True/False Question:");
            Console.WriteLine($"Question: {Header}");
            Console.WriteLine($"Body: {Body}");
            Console.WriteLine($"Mark: {Mark}");
        }

        /// <summary>Creates a new, independent copy of the question.</summary>
        /// <returns>The copy, as a <see cref="TrueFalseQuestion"/> boxed in <see cref="object"/>.</returns>
        public override object Clone()
        {
            return new TrueFalseQuestion(Header, Body, Mark, (bool)CorrectAnswer);
        }

        /// <summary>Asks the student for true or false.</summary>
        /// <returns><c>true</c> if his answer is the right one; otherwise <c>false</c>.</returns>
        public override bool CheckAnswer()
        {
            bool studentAnswer = ConsoleInput.ReadBool("Enter your answer (true/false): ");
            return studentAnswer == (bool)CorrectAnswer;
        }

        /// <summary>Gets the right answer as text: "True" or "False".</summary>
        public override string GetCorrectAnswerText()
        {
            return ((bool)CorrectAnswer).ToString();
        }

        /// <summary>Asks the teacher whether the right answer is true or false.</summary>
        protected override void ReadAnswers()
        {
            CorrectAnswer = ConsoleInput.ReadBool("Enter the correct answer for the question (true/false): ");
        }
    }

}