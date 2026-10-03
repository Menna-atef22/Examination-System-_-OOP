using Xunit;

namespace Examination_System___OOP.Tests
{
    public class SubjectTests
    {
        [Fact]
        public void Constructor_SetsNameAndId_AndHasNoExam()
        {
            var s = new Subject("Math", 1);
            Assert.Equal("Math", s.subjectName);
            Assert.Equal(1, s.subjectId);
            Assert.Null(s.exam);
        }

        [Fact]
        public void Properties_CanBeChanged()
        {
            var s = new Subject("Math", 1);
            s.subjectName = "Physics";
            s.subjectId = 5;
            Assert.Equal("Physics", s.subjectName);
            Assert.Equal(5, s.subjectId);
        }

        [Fact]
        public void ToString_WithoutExam_SaysNoExamYet()
        {
            string text = new Subject("Math", 1).ToString();
            Assert.Contains("Subject Name: Math", text);
            Assert.Contains("Subject ID: 1", text);
            Assert.Contains("no exam yet", text);
        }

        // ----- CreateExam -----

        [Fact]
        public void CreateExam_Final_BuildsAFinalExamLinkedToTheSubject()
        {
            var s = new Subject("Math", 1);
            string[] lines = Inputs.Join(new[] { "1", "30", "1" },
                                         Inputs.Typed(1, Inputs.Tf("Q1", "Body1", "2", "true")));
            using var c = new ConsoleSession(lines);

            Exam exam = s.CreateExam();

            Assert.IsType<FinalExam>(exam);
            Assert.Equal(30, exam.Time);
            Assert.Equal(1, exam.NumberOfQuestions);
            Assert.Same(s, exam.Subject);
            Assert.Same(exam, s.exam);
        }

        [Fact]
        public void CreateExam_Practical_BuildsAPracticalExam()
        {
            var s = new Subject("Physics", 2);
            string[] lines = Inputs.Join(new[] { "2", "45", "1" },
                                         Inputs.Mcq("Q1", "Body1", "1", 1, "A", "B"));
            using var c = new ConsoleSession(lines);

            Exam exam = s.CreateExam();

            Assert.IsType<PracticalExam>(exam);
            Assert.Equal(45, exam.Time);
            Assert.Same(s, exam.Subject);
        }

        [Fact]
        public void CreateExam_InvalidTypeTimeAndCount_AreAskedAgain()
        {
            var s = new Subject("Math", 1);
            // type 3 refused, time 0 refused, count -2 refused, then valid values
            string[] lines = Inputs.Join(new[] { "3", "1", "0", "20", "-2", "1" },
                                         Inputs.Typed(1, Inputs.Tf("Q1", "B", "1", "false")));
            using var c = new ConsoleSession(lines);

            Exam exam = s.CreateExam();

            Assert.IsType<FinalExam>(exam);
            Assert.Equal(20, exam.Time);
            Assert.Equal(1, exam.NumberOfQuestions);
        }

        [Fact]
        public void CreateExam_Twice_ReplacesThePreviousExam()
        {
            var s = new Subject("Math", 1);

            using (new ConsoleSession(Inputs.Join(new[] { "1", "10", "1" },
                                                  Inputs.Typed(1, Inputs.Tf("A", "B", "1", "true")))))
            {
                s.CreateExam();
            }
            Exam first = s.exam!;

            using (new ConsoleSession(Inputs.Join(new[] { "2", "20", "1" },
                                                  Inputs.Mcq("A", "B", "1", 1, "X", "Y"))))
            {
                s.CreateExam();
            }

            Assert.NotSame(first, s.exam);
            Assert.IsType<PracticalExam>(s.exam);
        }

        [Fact]
        public void ToString_WithExam_ShowsTheExam_AndDoesNotRecurseForever()
        {
            var s = new Subject("Math", 1);
            string[] lines = Inputs.Join(new[] { "1", "30", "1" },
                                         Inputs.Typed(1, Inputs.Tf("Q1", "B", "1", "true")));
            using var c = new ConsoleSession(lines);
            s.CreateExam();

            string text = s.ToString();   // would overflow the stack if Exam.ToString printed the whole Subject

            Assert.Contains("Subject Name: Math", text);
            Assert.Contains("FinalExam", text);
            Assert.DoesNotContain("no exam yet", text);
        }
    }
}
