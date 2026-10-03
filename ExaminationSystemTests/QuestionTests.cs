using Xunit;

namespace Examination_System___OOP.Tests
{
    // =====================================================================
    //  Answer
    // =====================================================================
    public class AnswerTests
    {
        [Fact]
        public void Constructor_SetsIdAndText()
        {
            var a = new Answer(1, "Paris");
            Assert.Equal(1, a.AnswerId);
            Assert.Equal("Paris", a.AnswerText);
        }

        [Fact]
        public void Setters_ChangeTheValues()
        {
            var a = new Answer(1, "Paris");
            a.AnswerId = 4;
            a.AnswerText = "Rome";
            Assert.Equal(4, a.AnswerId);
            Assert.Equal("Rome", a.AnswerText);
        }

        [Fact]
        public void Clone_IsAnIndependentCopy()
        {
            var original = new Answer(2, "Five");
            var copy = (Answer)original.Clone();

            Assert.NotSame(original, copy);
            Assert.Equal(original.AnswerId, copy.AnswerId);
            Assert.Equal(original.AnswerText, copy.AnswerText);

            copy.AnswerText = "Changed";
            Assert.Equal("Five", original.AnswerText);
        }

        [Fact]
        public void ToString_HasIdAndText()
        {
            Assert.Equal("Answer ID: 1, Answer Text: Paris", new Answer(1, "Paris").ToString());
        }
    }

    // =====================================================================
    //  Question (tested through TrueFalseQuestion) + TrueFalseQuestion
    // =====================================================================
    public class TrueFalseQuestionTests
    {
        private static TrueFalseQuestion Make(bool right = true)
            => new TrueFalseQuestion("Header", "Body", 3, right);

        // ----- creating it from the teacher's input (this used to crash) -----

        [Fact]
        public void ReadingConstructor_ReadsEverything_AndDoesNotCrash()
        {
            using var c = new ConsoleSession("H", "B", "3", "true");
            var q = new TrueFalseQuestion();

            Assert.Equal("H", q.Header);
            Assert.Equal("B", q.Body);
            Assert.Equal(3, q.Mark);
            Assert.True((bool)q.CorrectAnswer);
        }

        [Fact]
        public void ReadingConstructor_AcceptsADecimalMark()
        {
            using var c = new ConsoleSession("H", "B", "2.5", "f");
            var q = new TrueFalseQuestion();
            Assert.Equal(2.5, q.Mark);
            Assert.False((bool)q.CorrectAnswer);
        }

        [Fact]
        public void ReadingConstructor_ZeroOrNegativeMark_IsAskedAgain()
        {
            using var c = new ConsoleSession("H", "B", "0", "-1", "2", "true");
            var q = new TrueFalseQuestion();
            Assert.Equal(2, q.Mark);
        }

        [Fact]
        public void ReadingConstructor_EmptyHeaderAndBody_AreAskedAgain()
        {
            using var c = new ConsoleSession("", "H", "  ", "B", "1", "true");
            var q = new TrueFalseQuestion();
            Assert.Equal("H", q.Header);
            Assert.Equal("B", q.Body);
        }

        // ----- encapsulation -----

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-0.5)]
        public void Mark_ZeroOrNegative_Throws(double bad)
        {
            var q = Make();
            Assert.Throws<ArgumentException>(() => q.Mark = bad);
        }

        [Fact]
        public void Mark_PositiveValue_IsAccepted()
        {
            var q = Make();
            q.Mark = 7.5;
            Assert.Equal(7.5, q.Mark);
        }

        [Fact]
        public void ValuesConstructor_ZeroMark_Throws()
        {
            Assert.Throws<ArgumentException>(() => new TrueFalseQuestion("H", "B", 0, true));
        }

        [Fact]
        public void HeaderAndBody_CanBeChanged()
        {
            var q = Make();
            q.Header = "New header";
            q.Body = "New body";
            Assert.Equal("New header", q.Header);
            Assert.Equal("New body", q.Body);
        }

        // ----- behavior -----

        [Fact]
        public void GetCorrectAnswerText_IsTrueOrFalse()
        {
            Assert.Equal("True", Make(true).GetCorrectAnswerText());
            Assert.Equal("False", Make(false).GetCorrectAnswerText());
        }

        [Fact]
        public void GetCorrectAnswer_ReturnsTheBoxedBool()
        {
            Assert.Equal(true, Make(true).GetCorrectAnswer());
        }

        [Theory]
        [InlineData(true, "true", true)]
        [InlineData(true, "false", false)]
        [InlineData(false, "f", true)]
        [InlineData(false, "T", false)]
        public void CheckAnswer_ComparesWithTheRightAnswer(bool right, string typed, bool expected)
        {
            var q = Make(right);
            using var c = new ConsoleSession(typed);
            Assert.Equal(expected, q.CheckAnswer());
        }

        [Fact]
        public void CheckAnswer_InvalidInput_IsAskedAgain()
        {
            var q = Make(true);
            using var c = new ConsoleSession("maybe", "true");
            Assert.True(q.CheckAnswer());
        }

        [Fact]
        public void DisplayQuestion_PrintsHeaderBodyAndMark()
        {
            var q = Make();
            using var c = new ConsoleSession();
            q.DisplayQuestion();

            Assert.Contains("True/False Question:", c.Output);
            Assert.Contains("Question: Header", c.Output);
            Assert.Contains("Body: Body", c.Output);
            Assert.Contains("Mark: 3", c.Output);
        }

        [Fact]
        public void ToString_HasHeaderBodyAndMark()
        {
            Assert.Equal("Header - Body (Mark: 3)", Make().ToString());
        }

        // ----- ICloneable -----

        [Fact]
        public void Clone_IsADifferentObjectWithTheSameData()
        {
            var original = Make(true);
            var copy = Assert.IsType<TrueFalseQuestion>(original.Clone());

            Assert.NotSame(original, copy);
            Assert.Equal(original.Header, copy.Header);
            Assert.Equal(original.Body, copy.Body);
            Assert.Equal(original.Mark, copy.Mark);
            Assert.Equal(original.GetCorrectAnswerText(), copy.GetCorrectAnswerText());
        }

        [Fact]
        public void Clone_ChangingTheCopyDoesNotChangeTheOriginal()
        {
            var original = Make(true);
            var copy = (TrueFalseQuestion)original.Clone();

            copy.Header = "Changed";
            copy.Mark = 9;
            copy.CorrectAnswer = false;

            Assert.Equal("Header", original.Header);
            Assert.Equal(3, original.Mark);
            Assert.True((bool)original.CorrectAnswer);
        }
    }

    // =====================================================================
    //  MCQQuestion
    // =====================================================================
    public class MCQQuestionTests
    {
        /// <summary>Reads an MCQ question (mark 2) from simulated teacher input.</summary>
        private static MCQQuestion Make(int right, params string[] answers)
        {
            using var c = new ConsoleSession(Inputs.Mcq("Header", "Body", "2", right, answers));
            return new MCQQuestion();
        }

        // ----- creating it from the teacher's input (this used to crash) -----

        [Fact]
        public void ReadingConstructor_ReadsEverything_AndDoesNotCrash()
        {
            var q = Make(1, "Paris", "Rome", "Berlin");

            Assert.Equal("Header", q.Header);
            Assert.Equal("Body", q.Body);
            Assert.Equal(2, q.Mark);
            Assert.Equal(3, q.Answers.Length);
        }

        [Fact]
        public void Answers_AreNumberedFromOne()
        {
            var q = Make(1, "A", "B", "C");
            Assert.Equal(new[] { 1, 2, 3 }, q.Answers.Select(a => a.AnswerId).ToArray());
            Assert.Equal(new[] { "A", "B", "C" }, q.Answers.Select(a => a.AnswerText).ToArray());
        }

        [Fact]
        public void CorrectAnswer_IsTheChosenOne()
        {
            var q = Make(2, "Four", "Five", "Six");
            Assert.Same(q.Answers[1], q.CorrectAnswer);
        }

        [Fact]
        public void ReadingConstructor_ZeroMark_IsAskedAgain()
        {
            using var c = new ConsoleSession("H", "B", "0", "4", "2", "A", "B", "1");
            var q = new MCQQuestion();
            Assert.Equal(4, q.Mark);
        }

        [Fact]
        public void ReadingConstructor_LessThanTwoAnswers_IsAskedAgain()
        {
            // count "1" is refused, then "2" is accepted
            using var c = new ConsoleSession("H", "B", "1", "1", "2", "A", "B", "2");
            var q = new MCQQuestion();
            Assert.Equal(2, q.Answers.Length);
        }

        [Fact]
        public void ReadingConstructor_DuplicateAnswer_IsRefused_IgnoringCaseAndSpaces()
        {
            using var c = new ConsoleSession("H", "B", "1", "2", "Yes", "  YES ", "No", "1");
            var q = new MCQQuestion();

            Assert.Equal("Yes", q.Answers[0].AnswerText);
            Assert.Equal("No", q.Answers[1].AnswerText);
            Assert.Contains("already entered", c.Output);
        }

        [Fact]
        public void ReadingConstructor_RightNumberOutOfRange_IsAskedAgain()
        {
            using var c = new ConsoleSession("H", "B", "1", "2", "A", "B", "5", "2");
            var q = new MCQQuestion();
            Assert.Same(q.Answers[1], q.CorrectAnswer);
        }

        // ----- behavior -----

        [Fact]
        public void GetCorrectAnswerText_IsNumberDotText()
        {
            Assert.Equal("2. Five", Make(2, "Four", "Five", "Six").GetCorrectAnswerText());
        }

        [Theory]
        [InlineData("2", true)]
        [InlineData("1", false)]
        [InlineData("3", false)]
        public void CheckAnswer_ComparesTheChoiceNumber(string typed, bool expected)
        {
            var q = Make(2, "Four", "Five", "Six");
            using var c = new ConsoleSession(typed);
            Assert.Equal(expected, q.CheckAnswer());
        }

        [Fact]
        public void CheckAnswer_OutOfRangeOrText_IsAskedAgain()
        {
            var q = Make(2, "Four", "Five", "Six");
            using var c = new ConsoleSession("9", "0", "abc", "2");
            Assert.True(q.CheckAnswer());
        }

        [Fact]
        public void DisplayQuestion_PrintsTheNumberedChoices()
        {
            var q = Make(1, "Four", "Five");
            using var c = new ConsoleSession();
            q.DisplayQuestion();

            Assert.Contains("MCQ Question:", c.Output);
            Assert.Contains("Question: Header", c.Output);
            Assert.Contains("1. Four", c.Output);
            Assert.Contains("2. Five", c.Output);
        }

        [Fact]
        public void GetCorrectAnswer_ReturnsAnAnswerObject()
        {
            var q = Make(1, "A", "B");
            Assert.IsType<Answer>(q.GetCorrectAnswer());
        }

        // ----- ICloneable (deep copy) -----

        [Fact]
        public void Clone_HasTheSameData()
        {
            var original = Make(2, "Four", "Five", "Six");
            var copy = Assert.IsType<MCQQuestion>(original.Clone());

            Assert.NotSame(original, copy);
            Assert.Equal(original.Header, copy.Header);
            Assert.Equal(original.Body, copy.Body);
            Assert.Equal(original.Mark, copy.Mark);
            Assert.Equal(original.Answers.Length, copy.Answers.Length);
            Assert.Equal(original.GetCorrectAnswerText(), copy.GetCorrectAnswerText());
        }

        [Fact]
        public void Clone_IsDeep_TheAnswersAreCopiedToo()
        {
            var original = Make(2, "Four", "Five", "Six");
            var copy = (MCQQuestion)original.Clone();

            for (int i = 0; i < original.Answers.Length; i++)
                Assert.NotSame(original.Answers[i], copy.Answers[i]);

            // the right answer of the copy points INSIDE the copy's own answers
            Assert.Same(copy.Answers[1], copy.CorrectAnswer);
            Assert.NotSame(original.CorrectAnswer, copy.CorrectAnswer);
        }

        [Fact]
        public void Clone_ChangingTheCopyDoesNotChangeTheOriginal()
        {
            var original = Make(2, "Four", "Five", "Six");
            var copy = (MCQQuestion)original.Clone();

            copy.Header = "Changed";
            copy.Answers[1].AnswerText = "Changed answer";

            Assert.Equal("Header", original.Header);
            Assert.Equal("Five", original.Answers[1].AnswerText);
            Assert.Equal("2. Five", original.GetCorrectAnswerText());
        }
    }
}
