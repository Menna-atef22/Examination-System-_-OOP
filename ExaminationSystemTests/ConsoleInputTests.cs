using Xunit;

namespace Examination_System___OOP.Tests
{
    public class ConsoleInputTests
    {
        // ---------- ReadInt ----------

        [Fact]
        public void ReadInt_ValidNumber_ReturnsIt()
        {
            using var c = new ConsoleSession("5");
            Assert.Equal(5, ConsoleInput.ReadInt("n: ", 1));
        }

        [Fact]
        public void ReadInt_InvalidThenValid_KeepsAsking()
        {
            using var c = new ConsoleSession("abc", "0", "7");
            Assert.Equal(7, ConsoleInput.ReadInt("n: ", 1));
            Assert.Contains("Invalid input, enter a number >= 1", c.Output);
        }

        [Fact]
        public void ReadInt_AboveMax_IsRefused()
        {
            using var c = new ConsoleSession("11", "3");
            Assert.Equal(3, ConsoleInput.ReadInt("n: ", 1, 10));
            Assert.Contains("between 1 and 10", c.Output);
        }

        [Fact]
        public void ReadInt_AcceptsMinAndMaxThemselves()
        {
            using (new ConsoleSession("1")) Assert.Equal(1, ConsoleInput.ReadInt("n: ", 1, 10));
            using (new ConsoleSession("10")) Assert.Equal(10, ConsoleInput.ReadInt("n: ", 1, 10));
        }

        [Fact]
        public void ReadInt_DecimalNumber_IsRefused()
        {
            using var c = new ConsoleSession("2.5", "2");
            Assert.Equal(2, ConsoleInput.ReadInt("n: ", 1));
        }

        [Fact]
        public void ReadInt_ClosedStream_Throws()
        {
            using var c = new ConsoleSession();
            Assert.Throws<InvalidOperationException>(() => ConsoleInput.ReadInt("n: ", 1));
        }

        [Fact]
        public void ReadInt_OnlyInvalidInputThenClosed_ThrowsInsteadOfLoopingForever()
        {
            using var c = new ConsoleSession("abc");
            Assert.Throws<InvalidOperationException>(() => ConsoleInput.ReadInt("n: ", 1));
        }

        // ---------- ReadDouble ----------

        [Fact]
        public void ReadDouble_ValidNumber_ReturnsIt()
        {
            using var c = new ConsoleSession("2.5");
            Assert.Equal(2.5, ConsoleInput.ReadDouble("n: ", 0));
        }

        [Fact]
        public void ReadDouble_AcceptsTheMinimumItself()
        {
            using var c = new ConsoleSession("0");
            Assert.Equal(0, ConsoleInput.ReadDouble("n: ", 0));
        }

        [Fact]
        public void ReadDouble_NegativeAndTextAreRefused()
        {
            using var c = new ConsoleSession("-1", "abc", "3");
            Assert.Equal(3, ConsoleInput.ReadDouble("n: ", 0));
        }

        [Fact]
        public void ReadDouble_NaNAndInfinityAreRefused()
        {
            using var c = new ConsoleSession("NaN", "Infinity", "-Infinity", "4");
            Assert.Equal(4, ConsoleInput.ReadDouble("n: ", 0));
        }

        [Fact]
        public void ReadDouble_CommaIsNotADecimalSeparator()
        {
            using var c = new ConsoleSession("2,5", "2.5");
            Assert.Equal(2.5, ConsoleInput.ReadDouble("n: ", 0));
            Assert.Contains("Invalid input", c.Output);
        }

        // ---------- ReadPositiveDouble ----------

        [Fact]
        public void ReadPositiveDouble_ValidNumber_ReturnsIt()
        {
            using var c = new ConsoleSession("1.5");
            Assert.Equal(1.5, ConsoleInput.ReadPositiveDouble("n: "));
        }

        [Fact]
        public void ReadPositiveDouble_ZeroNegativeNaNAndTextAreRefused()
        {
            using var c = new ConsoleSession("0", "-2", "NaN", "Infinity", "abc", "1.5");
            Assert.Equal(1.5, ConsoleInput.ReadPositiveDouble("n: "));
            Assert.Contains("greater than 0", c.Output);
        }

        [Fact]
        public void ReadPositiveDouble_VerySmallPositiveNumber_IsAccepted()
        {
            using var c = new ConsoleSession("0.001");
            Assert.Equal(0.001, ConsoleInput.ReadPositiveDouble("n: "));
        }

        // ---------- ReadText ----------

        [Fact]
        public void ReadText_TrimsTheSpaces()
        {
            using var c = new ConsoleSession("  hello world  ");
            Assert.Equal("hello world", ConsoleInput.ReadText("t: "));
        }

        [Fact]
        public void ReadText_EmptyAndSpacesOnly_AreRefused()
        {
            using var c = new ConsoleSession("", "   ", "hi");
            Assert.Equal("hi", ConsoleInput.ReadText("t: "));
            Assert.Contains("Cannot be empty", c.Output);
        }

        // ---------- ReadBool ----------

        [Theory]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("t", true)]
        [InlineData("T", true)]
        [InlineData("  True  ", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("f", false)]
        [InlineData("F", false)]
        public void ReadBool_AcceptedValues(string input, bool expected)
        {
            using var c = new ConsoleSession(input);
            Assert.Equal(expected, ConsoleInput.ReadBool("b: "));
        }

        [Fact]
        public void ReadBool_InvalidThenValid_KeepsAsking()
        {
            using var c = new ConsoleSession("yes", "1", "", "t");
            Assert.True(ConsoleInput.ReadBool("b: "));
            Assert.Contains("Enter true or false", c.Output);
        }

        [Fact]
        public void ReadBool_ClosedStream_Throws()
        {
            using var c = new ConsoleSession();
            Assert.Throws<InvalidOperationException>(() => ConsoleInput.ReadBool("b: "));
        }

        // ---------- the prompt ----------

        [Fact]
        public void Prompt_IsPrintedBeforeReading()
        {
            using var c = new ConsoleSession("5");
            ConsoleInput.ReadInt("Your number: ", 1);
            Assert.StartsWith("Your number: ", c.Output);
        }
    }
}
