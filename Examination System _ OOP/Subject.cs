using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// A school subject with a name, an id and (optionally) one exam.
    /// </summary>
    public class Subject
    {
        private string SubjectName;
        private int SubjectId;
        private Exam? Exam;

        /// <summary>The id of the subject.</summary>
        public int subjectId
        {
            get { return SubjectId; }
            set { SubjectId = value; }
        }

        /// <summary>The name of the subject.</summary>
        public string subjectName
        {
            get { return SubjectName; }
            set { SubjectName = value; }
        }

        /// <summary>The exam of the subject, or <c>null</c> if no exam was created yet.</summary>
        public Exam? exam
        {
            get { return Exam; }
            set { Exam = value; }
        }

        /// <summary>Creates a subject without an exam.</summary>
        /// <param name="subjectName">The name of the subject.</param>
        /// <param name="subjectId">The id of the subject.</param>
        public Subject(string subjectName, int subjectId)
        {
            this.SubjectName = subjectName;
            this.SubjectId = subjectId;
        }

        /// <summary>Returns the subject (and its exam, if it has one) as text.</summary>
        public override string ToString()
        {
            string examText = Exam == null ? "no exam yet" : Exam.ToString();
            return $"Subject Name: {subjectName}, Subject ID: {subjectId}, Exam: {examText}";
        }

        /// <summary>
        /// Reads the exam type, the time and the number of questions, then builds the exam
        /// (which reads its questions) and links it to this subject. A previous exam is replaced.
        /// </summary>
        /// <returns>The new <see cref="FinalExam"/> or <see cref="PracticalExam"/>.</returns>
        public Exam CreateExam()
        {
            int choice = ConsoleInput.ReadInt("Choose exam type (1 = Final, 2 = Practical): ", 1, 2);
            int time = ConsoleInput.ReadInt("Exam time (minutes): ", 1);
            int count = ConsoleInput.ReadInt("Number of questions: ", 1);

            Exam newExam;
            if (choice == 1)
                newExam = new FinalExam(time, count);
            else
                newExam = new PracticalExam(time, count);

            newExam.Subject = this;
            Exam = newExam;
            return newExam;
        }

    }
}