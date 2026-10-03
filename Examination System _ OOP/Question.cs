using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// Base class of every question: it has a header, a body, a mark and a right answer.
    /// Each question type (True/False, MCQ) decides how it reads, shows and checks its answer.
    /// </summary>
    public abstract class Question : ICloneable
    {
        private string header = "";
        private string body = "";
        private double mark;
        private object? correctAnswer;


        /// <summary>The short title of the question.</summary>
        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>The marks the student gets for a right answer.</summary>
        /// <exception cref="ArgumentException">The value is zero or negative.</exception>
        public double Mark
        {
            get { return mark; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Mark cannot be negative or zero.");
                mark = value;
            }
        }

        /// <summary>The full text of the question.</summary>
        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        /// <summary>
        /// The right answer: an <see cref="Answer"/> for an MCQ question, a <see cref="bool"/> for a True/False question.
        /// </summary>
        public object CorrectAnswer
        {
            get { return correctAnswer!; }
            set { correctAnswer = value; }
        }


        /// <summary>
        /// Creates an empty question. Used by the derived classes that read their data from the teacher
        /// right after (<see cref="ReadBasicData"/>), so there is no valid mark yet and nothing is validated here.
        /// </summary>
        protected Question()
        {
        }

        /// <summary>Creates a question from ready values (used by <c>Clone</c>). The answer is set by the derived class.</summary>
        /// <param name="header">The short title of the question.</param>
        /// <param name="body">The full text of the question.</param>
        /// <param name="mark">The marks of the question (more than 0).</param>
        /// <exception cref="ArgumentException">The mark is zero or negative.</exception>
        public Question(string header, string body, double mark)
        {
            Header = header;
            Body = body;
            Mark = mark;
        }


        /// <summary>Returns the question as text: "header - body (Mark: ...)".</summary>
        public override string ToString()
        {
            return Header + " - " + Body + " (Mark: " + Mark + ")";
        }

        /// <summary>Asks the teacher for the header, the body and the mark of the question.</summary>
        protected void ReadBasicData()
        {
            Header = ConsoleInput.ReadText("Enter the header of the question: ");
            Body = ConsoleInput.ReadText("Enter the body of the question: ");
            Mark = ConsoleInput.ReadPositiveDouble("Enter the mark for the question: ");
        }

        /// <summary>Gets the right answer.</summary>
        /// <returns>An <see cref="Answer"/> (MCQ) or a <see cref="bool"/> (True/False), boxed in <see cref="object"/>.</returns>
        public object GetCorrectAnswer()
        {
            return CorrectAnswer;
        }

        /// <summary>
        /// Gets the right answer as text, so any exam can print it without checking the question type.
        /// </summary>
        public abstract string GetCorrectAnswerText();


        /// <summary>Asks the teacher for the answer(s) of this type of question and the right one.</summary>
        protected abstract void ReadAnswers();

        /// <summary>Prints the question (and its choices, if it has any) for the student.</summary>
        public abstract void DisplayQuestion();

        /// <summary>Asks the student for his answer.</summary>
        /// <returns><c>true</c> if the student's answer is the right one; otherwise <c>false</c>.</returns>
        public abstract bool CheckAnswer();

        /// <summary>Creates a new, independent copy of the question.</summary>
        /// <returns>The copy, boxed in <see cref="object"/>.</returns>
        public abstract object Clone();

    }
}