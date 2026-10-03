using Xunit;

namespace Examination_System___OOP.Tests
{
    // =====================================================================
    //  Exam (tested through FinalExam / PracticalExam)
    // =====================================================================
    public class ExamTests
    {
        // ----- constructor validation -----

        [Theory]
        [InlineData(0, 2)]
        [InlineData(-5, 2)]
        [InlineData(30, 0)]
        [InlineData(30, -1)]
        public void Constructor_InvalidTimeOrCount_Throws(int time, int count)
        {
            Assert.Throws<ArgumentException>(() => new FinalExam(time, count));
            Assert.Throws<ArgumentException>(() => new PracticalExam(time, count));
        }

        [Fact]
        public void Constructor_FillsTimeCountAndQuestions()
        {
            FinalExam exam = TestData.MakeFinal();
            Assert.Equal(30, exam.Time);
            Assert.Equal(2, exam.NumberOfQuestions);
            Assert.Equal(2, exam.Questions.Length);
            Assert.All(exam.Questions, q => Assert.NotNull(q));
        }

        // ----- properties -----

        [Fact]
        public void Time_ZeroOrNegative_Throws()
        {
            FinalExam exam = TestData.MakeFinal();
            Assert.Throws<ArgumentException>(() => exam.Time = 0);
            Assert.Throws<ArgumentException>(() => exam.Time = -10);
            Assert.Equal(30, exam.Time);   // unchanged
        }

        [Fact]
        public void NumberOfQuestions_ZeroOrNegative_Throws()
        {
            FinalExam exam = TestData.MakeFinal();
            Assert.Throws<ArgumentException>(() => exam.NumberOfQuestions = 0);
            Assert.Throws<ArgumentException>(() => exam.NumberOfQuestions = -1);
            Assert.Equal(2, exam.NumberOfQuestions);   // unchanged
        }

        [Fact]
        public void Time_ValidValue_IsAccepted()
        {
            FinalExam exam = TestData.MakeFinal();
            exam.Time = 90;
            Assert.Equal(90, exam.Time);
        }

        [Fact]
        public void Subject_CanBeSetAndRead()
        {
            FinalExam exam = TestData.MakeFinal();
            var other = new Subject("Chemistry", 9);
            exam.Subject = other;
            Assert.Same(other, exam.Subject);
        }

        // ----- IComparable -----

        [Fact]
        public void CompareTo_FewerQuestions_IsNegative()
        {
            Exam two = TestData.MakeFinal();            // 2 questions
            Exam three = TestData.MakePractical(3);     // 3 questions
            Assert.True(two.CompareTo(three) < 0);
        }

        [Fact]
        public void CompareTo_MoreQuestions_IsPositive()
        {
            Exam two = TestData.MakeFinal();
            Exam three = TestData.MakePractical(3);
            Assert.True(three.CompareTo(two) > 0);
        }

        [Fact]
        public void CompareTo_SameNumber_IsZero_EvenForDifferentExamTypes()
        {
            Exam final = TestData.MakeFinal();           // 2
            Exam practical = TestData.MakePractical(2);  // 2
            Assert.Equal(0, final.CompareTo(practical));
        }

        [Fact]
        public void CompareTo_Null_IsOne()
        {
            Assert.Equal(1, TestData.MakeFinal().CompareTo(null));
        }

        [Fact]
        public void CompareTo_NotAnExam_Throws()
        {
            Assert.Throws<ArgumentException>(() => TestData.MakeFinal().CompareTo("not an exam"));
        }

        [Fact]
        public void ArraySort_SortsByNumberOfQuestions()
        {
            Exam[] exams = { TestData.MakePractical(3), TestData.MakeFinal(), TestData.MakePractical(1) };
            Array.Sort(exams);
            Assert.Equal(new[] { 1, 2, 3 }, exams.Select(e => e.NumberOfQuestions).ToArray());
        }

        // ----- ToString -----

        [Fact]
        public void ToString_HasTypeTimeCountAndSubjectName()
        {
            string text = TestData.MakeFinal().ToString();
            Assert.Contains("FinalExam", text);
            Assert.Contains("Time: 30 minutes", text);
            Assert.Contains("Number of Questions: 2", text);
            Assert.Contains("Subject: Math", text);
        }

        [Fact]
        public void ToString_WithoutSubject_DoesNotCrash()
        {
            // build an exam but never link it to a subject
            using var c = new ConsoleSession(Inputs.Mcq("P", "B", "1", 1, "A", "B"));
            var exam = new PracticalExam(10, 1);
            Assert.Contains("(none)", exam.ToString());
        }

        // ----- ShowModelAnswer -----

        [Fact]
        public void ShowModelAnswer_PrintsEveryQuestionWithItsRightAnswer_WithoutAskingAnything()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession();   // no input at all: it must not ask for anything
            exam.ShowModelAnswer();

            Assert.Contains("MODEL ANSWER", c.Output);
            Assert.Contains("--- Question 1 ---", c.Output);
            Assert.Contains("--- Question 2 ---", c.Output);
            Assert.Contains("Right Answer: True", c.Output);
            Assert.Contains("Right Answer: 2. Five", c.Output);
        }
    }

    // =====================================================================
    //  FinalExam
    // =====================================================================
    public class FinalExamTests
    {
        [Fact]
        public void Constructor_CreatesTheRightQuestionTypes()
        {
            FinalExam exam = TestData.MakeFinal();
            Assert.IsType<TrueFalseQuestion>(exam.Questions[0]);
            Assert.IsType<MCQQuestion>(exam.Questions[1]);
        }

        [Fact]
        public void Constructor_InvalidQuestionType_IsAskedAgain()
        {
            string[] lines = Inputs.Join(new[] { "3", "0" },
                                         Inputs.Typed(1, Inputs.Tf("T", "B", "1", "true")));
            using var c = new ConsoleSession(lines);
            var exam = new FinalExam(10, 1);

            Assert.IsType<TrueFalseQuestion>(exam.Questions[0]);
            Assert.Contains("between 1 and 2", c.Output);
        }

        [Fact]
        public void ShowExam_AllRight_FullGrade()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession("true", "2");
            exam.ShowExam();

            Assert.Contains("FINAL EXAM: Math (30 min)", c.Output);
            Assert.Contains("Grade: 5 / 5", c.Output);
            Assert.Contains("Percentage: 100%", c.Output);
            Assert.DoesNotContain("Incorrect", c.Output);
        }

        [Fact]
        public void ShowExam_AllWrong_ZeroGrade_AndShowsTheRightAnswers()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession("false", "1");
            exam.ShowExam();

            Assert.Contains("Grade: 0 / 5", c.Output);
            Assert.Contains("Percentage: 0%", c.Output);
            Assert.Contains("Incorrect. Right answer: True", c.Output);
            Assert.Contains("Incorrect. Right answer: 2. Five", c.Output);
        }

        [Fact]
        public void ShowExam_OneRightOneWrong_PartialGrade()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession("true", "1");   // TF right (2 marks), MCQ wrong
            exam.ShowExam();

            Assert.Contains("Grade: 2 / 5", c.Output);
            Assert.Contains("Percentage: 40%", c.Output);
            Assert.Contains("Correct!", c.Output);
            Assert.Contains("Incorrect. Right answer: 2. Five", c.Output);
        }

        [Fact]
        public void ShowExam_InvalidStudentInput_IsAskedAgain()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession("maybe", "true", "9", "2");
            exam.ShowExam();
            Assert.Contains("Grade: 5 / 5", c.Output);
        }

        [Fact]
        public void ShowExam_PrintsEveryQuestion()
        {
            FinalExam exam = TestData.MakeFinal();
            using var c = new ConsoleSession("true", "2");
            exam.ShowExam();

            Assert.Contains("--- Question 1 of 2 ---", c.Output);
            Assert.Contains("--- Question 2 of 2 ---", c.Output);
            Assert.Contains("True/False Question:", c.Output);
            Assert.Contains("MCQ Question:", c.Output);
        }
    }

    // =====================================================================
    //  PracticalExam
    // =====================================================================
    public class PracticalExamTests
    {
        [Fact]
        public void Constructor_AllQuestionsAreMCQ()
        {
            PracticalExam exam = TestData.MakePractical(3);
            Assert.Equal(3, exam.Questions.Length);
            Assert.All(exam.Questions, q => Assert.IsType<MCQQuestion>(q));
        }

        [Fact]
        public void ShowExam_HasNoGrade_AndShowsRightAnswersAtTheEnd()
        {
            PracticalExam exam = TestData.MakePractical(2);
            using var c = new ConsoleSession("2", "2");   // both wrong on purpose
            exam.ShowExam();

            Assert.Contains("PRACTICAL EXAM: Physics (20 min)", c.Output);
            Assert.DoesNotContain("Grade", c.Output);
            Assert.DoesNotContain("Percentage", c.Output);
            Assert.DoesNotContain("Correct!", c.Output);
            Assert.DoesNotContain("Incorrect", c.Output);

            Assert.Contains("===== RIGHT ANSWERS =====", c.Output);
            Assert.Contains("Question 1: 1. Red", c.Output);
            Assert.Contains("Question 2: 1. Red", c.Output);
        }

        [Fact]
        public void ShowExam_InvalidStudentInput_IsAskedAgain()
        {
            PracticalExam exam = TestData.MakePractical(1);
            using var c = new ConsoleSession("7", "abc", "1");
            exam.ShowExam();
            Assert.Contains("RIGHT ANSWERS", c.Output);
        }

        [Fact]
        public void ShowRightAnswers_PrintsOneLinePerQuestion()
        {
            PracticalExam exam = TestData.MakePractical(3);
            using var c = new ConsoleSession();
            exam.ShowRightAnswers();

            Assert.Contains("Question 1:", c.Output);
            Assert.Contains("Question 2:", c.Output);
            Assert.Contains("Question 3:", c.Output);
        }
    }
}
