using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// A practical exam: all the questions are MCQ, it has no grade, and the right answers are shown at the end.
    /// </summary>
    public class PracticalExam : Exam
    {
        /// <summary>Creates the exam and asks the teacher for the data of each MCQ question.</summary>
        /// <param name="time">The exam time in minutes (more than 0).</param>
        /// <param name="numberOfQuestions">How many questions to read (more than 0).</param>
        /// <exception cref="ArgumentException">The time or the number of questions is zero or negative.</exception>
        public PracticalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions)
        {
            for (int i = 0; i < numberOfQuestions; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"--- Question {i + 1} (MCQ) ---");
                Questions[i] = new MCQQuestion();
            }
        }


        /// <summary>Asks every question (the answers are not graded) and then shows the right answers.</summary>
        public override void ShowExam()
        {
            Console.WriteLine();
            Console.WriteLine($"===== PRACTICAL EXAM: {Subject.subjectName} ({Time} min) =====");

            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"--- Question {i + 1} of {Questions.Length} ---");
                Questions[i].DisplayQuestion();
                Questions[i].CheckAnswer();   // practical exam has no grade, the right answers are shown at the end
            }

            ShowRightAnswers();
        }


        /// <summary>Prints the right answer of each question.</summary>
        public void ShowRightAnswers()
        {
            Console.WriteLine();
            Console.WriteLine("===== RIGHT ANSWERS =====");
            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine($"Question {i + 1}: {Questions[i].GetCorrectAnswerText()}");
            }
        }

    }
}