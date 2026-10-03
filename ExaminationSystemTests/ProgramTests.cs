using Xunit;

namespace Examination_System___OOP.Tests
{
    /// <summary>
    /// Runs the real menu: the tests type into the console and read what the program prints.
    /// Every script must end with "0" (Exit), otherwise Main keeps asking and the input closes.
    /// </summary>
    public class ProgramTests
    {
        private static string Run(params string[] lines)
        {
            using var c = new ConsoleSession(lines);
            Program.Main(Array.Empty<string>());
            return c.Output;
        }

        // Ready-made "create exam" scripts
        private static string[] FinalWithOneTf(string subject = "Math", int id = 1)
            => Inputs.MenuCreateFinal(subject, id, 30,
                   Inputs.Typed(1, Inputs.Tf("Q1", "Body1", "2", "true")));

        private static string[] FinalWithTwoTf(string subject, int id)
            => Inputs.MenuCreateFinal(subject, id, 30,
                   Inputs.Typed(1, Inputs.Tf("Q1", "Body1", "1", "true")),
                   Inputs.Typed(1, Inputs.Tf("Q2", "Body2", "1", "false")));

        private static string[] PracticalWithOneMcq()
            => Inputs.MenuCreatePractical("Physics", 2, 20,
                   Inputs.Mcq("Q1", "Body1", "1", 1, "A", "B"));

        // ---------- basic menu ----------

        [Fact]
        public void Exit_PrintsGoodbye()
        {
            string output = Run("0");
            Assert.Contains("EXAMINATION SYSTEM", output);
            Assert.Contains("Goodbye!", output);
        }

        [Fact]
        public void InvalidMenuChoice_IsAskedAgain()
        {
            string output = Run("9", "abc", "0");
            Assert.Contains("between 0 and 7", output);
            Assert.Contains("Goodbye!", output);
        }

        [Fact]
        public void ClosedInput_WithoutExit_ThrowsInvalidOperation()
        {
            using var c = new ConsoleSession();
            Assert.Throws<InvalidOperationException>(() => Program.Main(Array.Empty<string>()));
        }

        [Fact]
        public void Options_WithNoExams_ShowAFriendlyMessage()
        {
            foreach (string option in new[] { "2", "3", "4", "5", "7" })
            {
                string output = Run(option, "0");
                Assert.Contains("No exams yet", output);
            }
        }

        [Fact]
        public void Compare_WithLessThanTwoExams_ShowsAMessage()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "6", "0" }));
            Assert.Contains("You need at least 2 exams", output);
        }

        // ---------- option 1 / 2: create and list ----------

        [Fact]
        public void CreateFinalExam_ThenList()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "2", "0" }));

            Assert.Contains("Exam created: FinalExam", output);
            Assert.Contains("Subject: Math", output);
            Assert.Contains("1. FinalExam", output);
        }

        [Fact]
        public void CreatePracticalExam_ThenList()
        {
            string output = Run(Inputs.Join(PracticalWithOneMcq(), new[] { "2", "0" }));

            Assert.Contains("Exam created: PracticalExam", output);
            Assert.Contains("1. PracticalExam", output);
        }

        [Fact]
        public void CreateExam_WithZeroMark_DoesNotCrash_AndAsksAgain()
        {
            // the mark "0" is refused, "2" is accepted (this used to crash the whole program)
            string[] lines = Inputs.Join(
                new[] { "1", "Math", "1", "1", "30", "1", "1", "Q1", "Body1", "0", "2", "true" },
                new[] { "2", "0" });

            string output = Run(lines);

            Assert.Contains("greater than 0", output);
            Assert.Contains("1. FinalExam", output);
        }

        [Fact]
        public void CreateMcqQuestionInsideFinalExam_DoesNotCrash()
        {
            string[] lines = Inputs.Join(
                Inputs.MenuCreateFinal("Math", 1, 30,
                    Inputs.Typed(2, Inputs.Mcq("Q1", "2+3?", "3", 2, "Four", "Five", "Six"))),
                new[] { "0" });

            string output = Run(lines);
            Assert.Contains("Exam created: FinalExam", output);
        }

        [Fact]
        public void CreateSeveralExams_ListsThemAllNumbered()
        {
            string output = Run(Inputs.Join(
                FinalWithOneTf("A", 1),
                PracticalWithOneMcq(),
                new[] { "2", "0" }));

            Assert.Contains("1. FinalExam", output);
            Assert.Contains("2. PracticalExam", output);
        }

        // ---------- option 3: take an exam ----------

        [Fact]
        public void TakeFinalExam_ShowsTheGrade()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "3", "1", "true", "0" }));

            Assert.Contains("FINAL EXAM: Math (30 min)", output);
            Assert.Contains("Correct!", output);
            Assert.Contains("Grade: 2 / 2", output);
            Assert.Contains("Percentage: 100%", output);
        }

        [Fact]
        public void TakeFinalExam_WrongAnswer_ShowsTheRightOne()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "3", "1", "false", "0" }));

            Assert.Contains("Incorrect. Right answer: True", output);
            Assert.Contains("Grade: 0 / 2", output);
        }

        [Fact]
        public void TakePracticalExam_ShowsRightAnswersAndNoGrade()
        {
            string output = Run(Inputs.Join(PracticalWithOneMcq(), new[] { "3", "1", "2", "0" }));

            Assert.Contains("PRACTICAL EXAM: Physics (20 min)", output);
            Assert.Contains("Question 1: 1. A", output);
            Assert.DoesNotContain("Grade", output);
        }

        [Fact]
        public void PickingAnExam_OutOfRange_IsAskedAgain()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "3", "5", "1", "true", "0" }));

            Assert.Contains("between 1 and 1", output);
            Assert.Contains("Grade: 2 / 2", output);
        }

        // ---------- option 4: model answer ----------

        [Fact]
        public void ShowRightAnswers_PrintsTheModelAnswer()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "4", "1", "0" }));

            Assert.Contains("MODEL ANSWER", output);
            Assert.Contains("Right Answer: True", output);
        }

        // ---------- option 5: ICloneable ----------

        [Fact]
        public void CloneQuestion_CopyIsIndependentOfTheOriginal()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "5", "1", "1", "0" }));

            Assert.Contains("Clone is a different object: True", output);
            Assert.Contains("Original header : Q1", output);
            Assert.Contains("Copy header     : Q1 (COPY)", output);
        }

        [Fact]
        public void CloneQuestion_NumberOutOfRange_IsAskedAgain()
        {
            string output = Run(Inputs.Join(FinalWithOneTf(), new[] { "5", "1", "9", "1", "0" }));

            Assert.Contains("between 1 and 1", output);
            Assert.Contains("Clone is a different object: True", output);
        }

        [Fact]
        public void CloneQuestion_Mcq_WorksToo()
        {
            string output = Run(Inputs.Join(PracticalWithOneMcq(), new[] { "5", "1", "1", "0" }));
            Assert.Contains("Clone is a different object: True", output);
        }

        // ---------- option 6: IComparable (compare) ----------

        [Fact]
        public void CompareExams_FirstHasFewerQuestions()
        {
            string output = Run(Inputs.Join(
                FinalWithOneTf("A", 1),        // 1 question
                FinalWithTwoTf("B", 2),        // 2 questions
                new[] { "6", "1", "2", "0" }));

            Assert.Contains("First exam : 1 questions", output);
            Assert.Contains("Second exam: 2 questions", output);
            Assert.Contains("The first exam has FEWER questions.", output);
        }

        [Fact]
        public void CompareExams_FirstHasMoreQuestions()
        {
            string output = Run(Inputs.Join(
                FinalWithOneTf("A", 1),
                FinalWithTwoTf("B", 2),
                new[] { "6", "2", "1", "0" }));

            Assert.Contains("The first exam has MORE questions.", output);
        }

        [Fact]
        public void CompareExams_SameNumberOfQuestions()
        {
            string output = Run(Inputs.Join(
                FinalWithOneTf("A", 1),
                FinalWithOneTf("B", 2),
                new[] { "6", "1", "2", "0" }));

            Assert.Contains("Both exams have the same number of questions.", output);
        }

        // ---------- option 7: IComparable (sort) ----------

        [Fact]
        public void SortExams_OrdersByNumberOfQuestions_Ascending()
        {
            string output = Run(Inputs.Join(
                FinalWithTwoTf("Big", 1),      // 2 questions, created first
                FinalWithOneTf("Small", 2),    // 1 question, created second
                new[] { "7", "0" }));

            int start = output.IndexOf("Exams sorted by number of questions", StringComparison.Ordinal);
            Assert.True(start >= 0);

            string sortedPart = output.Substring(start);
            int small = sortedPart.IndexOf("Subject: Small", StringComparison.Ordinal);
            int big = sortedPart.IndexOf("Subject: Big", StringComparison.Ordinal);

            Assert.True(small >= 0 && big >= 0);
            Assert.True(small < big, "The exam with fewer questions must come first.");
        }

        [Fact]
        public void SortExams_DoesNotChangeTheOrderOfTheMainList()
        {
            string output = Run(Inputs.Join(
                FinalWithTwoTf("Big", 1),
                FinalWithOneTf("Small", 2),
                new[] { "7", "2", "0" }));

            // after sorting, the normal list still shows the creation order: line 1 is "Big"
            string firstLine = output.Split('\n').Last(l => l.StartsWith("1. FinalExam"));
            Assert.Contains("Subject: Big", firstLine);
        }

        // ---------- the menu keeps working after many actions ----------

        [Fact]
        public void FullSession_CreateTakeCloneCompareSort_Exit()
        {
            string output = Run(Inputs.Join(
                FinalWithOneTf("A", 1),
                PracticalWithOneMcq(),
                new[]
                {
                    "2",
                    "3", "1", "true",
                    "4", "2",
                    "5", "1", "1",
                    "6", "1", "2",
                    "7",
                    "0"
                }));

            Assert.Contains("Grade: 2 / 2", output);
            Assert.Contains("MODEL ANSWER", output);
            Assert.Contains("Clone is a different object: True", output);
            Assert.Contains("Exams sorted by number of questions", output);
            Assert.Contains("Goodbye!", output);
        }
    }
}
