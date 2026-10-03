using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// Base class of every exam: it has a time, a number of questions, the questions and the subject it belongs to.
    /// Implements <see cref="IComparable"/>: exams are compared by their number of questions.
    /// </summary>
    public abstract class Exam : IComparable
    {
        private int time;
        private int numberOfQuestions;
        private Question[] questions = Array.Empty<Question>();
        private Subject? subject;

        /// <summary>The exam time in minutes.</summary>
        /// <exception cref="ArgumentException">The value is zero or negative.</exception>
        public int Time
        {
            get { return time; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Time cannot be negative or zero.");
                time = value;
            }
        }

        /// <summary>How many questions the exam has.</summary>
        /// <exception cref="ArgumentException">The value is zero or negative.</exception>
        public int NumberOfQuestions
        {
            get { return numberOfQuestions; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Number of questions cannot be negative or zero.");
                numberOfQuestions = value;
            }
        }

        /// <summary>The subject this exam belongs to (set by <see cref="Subject.CreateExam"/>).</summary>
        public Subject Subject
        {
            get { return subject!; }
            set { subject = value; }
        }

        /// <summary>The questions of the exam.</summary>
        public Question[] Questions
        {
            get { return questions; }
            set { questions = value; }
        }

        /// <summary>
        /// Creates an exam with an empty array of questions.
        /// The time and the number of questions must already be valid (more than 0):
        /// the derived exams read them from the user first.
        /// </summary>
        /// <param name="time">The exam time in minutes.</param>
        /// <param name="numberOfQuestions">How many questions the exam has.</param>
        /// <exception cref="ArgumentException">The time or the number of questions is zero or negative.</exception>
        public Exam(int time, int numberOfQuestions)
        {
            Time = time;
            NumberOfQuestions = numberOfQuestions;
            Questions = new Question[numberOfQuestions];
        }

        /// <summary>Compares this exam with another one by the number of questions.</summary>
        /// <param name="obj">The other exam (or <c>null</c>).</param>
        /// <returns>
        /// A negative number if this exam has fewer questions, zero if they have the same number,
        /// a positive number if it has more (and 1 if <paramref name="obj"/> is <c>null</c>).
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an <see cref="Exam"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;

            Exam? other = obj as Exam;
            if (other == null)
                throw new ArgumentException("Object is not an Exam");

            return NumberOfQuestions.CompareTo(other.NumberOfQuestions);
        }

        /// <summary>
        /// Returns the exam as text. It prints the subject NAME only: printing the whole
        /// <see cref="Subject"/> would call <c>Subject.ToString</c> again, which prints the exam again
        /// (an infinite recursion).
        /// </summary>
        public override string ToString()
        {
            string subjectName = subject == null ? "(none)" : subject.subjectName;
            return $"{GetType().Name} - Time: {Time} minutes, Number of Questions: {NumberOfQuestions}, Subject: {subjectName}";
        }

        /// <summary>Prints every question with its right answer, without asking the student anything.</summary>
        public void ShowModelAnswer()
        {
            Console.WriteLine();
            Console.WriteLine($"===== MODEL ANSWER: {GetType().Name} ({Time} min) =====");
            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"--- Question {i + 1} ---");
                Questions[i].DisplayQuestion();
                Console.WriteLine($"Right Answer: {Questions[i].GetCorrectAnswerText()}");
            }
        }

        /// <summary>Lets the student take the exam (each exam type does it in its own way).</summary>
        public abstract void ShowExam();

    }
}