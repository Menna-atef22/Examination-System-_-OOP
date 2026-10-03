using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// A final exam: each question can be True/False or MCQ, and the student gets a grade at the end.
    /// </summary>
    public class FinalExam : Exam
    {
        /// <summary>Creates the exam and asks the teacher for the type and the data of each question.</summary>
        /// <param name="time">The exam time in minutes (more than 0).</param>
        /// <param name="numberOfQuestions">How many questions to read (more than 0).</param>
        /// <exception cref="ArgumentException">The time or the number of questions is zero or negative.</exception>
        public FinalExam(int time, int numberOfQuestions) : base(time, numberOfQuestions)
        {
            for (int i = 0; i < numberOfQuestions; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"--- Question {i + 1} ---");

                int type = ConsoleInput.ReadInt("Type (1 = True/False, 2 = MCQ): ", 1, 2);

                if (type == 1)
                    Questions[i] = new TrueFalseQuestion();
                else
                    Questions[i] = new MCQQuestion();
            }
        }

        /// <summary>
        /// Asks every question, tells the student if he was right, and prints his grade
        /// (marks he got out of the total marks, and the percentage).
        /// </summary>
        public override void ShowExam()
        {
            Console.WriteLine();
            Console.WriteLine($"===== FINAL EXAM: {Subject.subjectName} ({Time} min) =====");

            double grade = CalculateGrade();

            double totalMarks = 0;
            for (int i = 0; i < Questions.Length; i++)
            {
                totalMarks += Questions[i].Mark;
            }

            Console.WriteLine();
            Console.WriteLine($"Grade: {grade} / {totalMarks}");
            if (totalMarks > 0)
                Console.WriteLine($"Percentage: {grade / totalMarks * 100:0.##}%");
        }

        /// <summary>
        /// Asks every question; adds the mark of each right answer and shows the right answer for a wrong one.
        /// </summary>
        /// <returns>The sum of the marks of the questions the student answered right.</returns>
        private double CalculateGrade()
        {
            double totalGrade = 0;
            for (int i = 0; i < Questions.Length; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"--- Question {i + 1} of {Questions.Length} ---");
                Questions[i].DisplayQuestion();
                if (Questions[i].CheckAnswer())
                {
                    totalGrade += Questions[i].Mark;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine($"Incorrect. Right answer: {Questions[i].GetCorrectAnswerText()}");
                }
            }
            return totalGrade;
        }

    }
}