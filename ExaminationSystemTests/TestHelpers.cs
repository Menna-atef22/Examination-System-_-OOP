using Xunit;

// The tests replace Console.In / Console.Out, which are global, so they must not run in parallel.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Examination_System___OOP.Tests
{
    /// <summary>
    /// Types the given lines into the console and captures everything the program prints.
    /// Use it with <c>using var c = new ConsoleSession("5", "abc");</c> then read <c>c.Output</c>.
    /// When the lines run out, ReadLine returns null (like a closed input stream).
    /// </summary>
    public sealed class ConsoleSession : IDisposable
    {
        private readonly TextReader oldIn;
        private readonly TextWriter oldOut;
        private readonly StringWriter writer = new StringWriter();

        public ConsoleSession(params string[] inputLines)
        {
            oldIn = Console.In;
            oldOut = Console.Out;
            Console.SetIn(new StringReader(string.Join("\n", inputLines)));
            Console.SetOut(writer);
        }

        /// <summary>Everything printed so far.</summary>
        public string Output => writer.ToString();

        public void Dispose()
        {
            Console.SetIn(oldIn);
            Console.SetOut(oldOut);
        }
    }

    /// <summary>Builds the lines a user would type, so tests stay readable.</summary>
    public static class Inputs
    {
        /// <summary>Glues several groups of lines together.</summary>
        public static string[] Join(params string[][] parts) => parts.SelectMany(p => p).ToArray();

        /// <summary>The lines of a True/False question: header, body, mark, right answer.</summary>
        public static string[] Tf(string header, string body, string mark, string answer)
            => new[] { header, body, mark, answer };

        /// <summary>The lines of an MCQ question: header, body, mark, count, the answers, right number.</summary>
        public static string[] Mcq(string header, string body, string mark, int right, params string[] answers)
            => Join(new[] { header, body, mark, answers.Length.ToString() }, answers, new[] { right.ToString() });

        /// <summary>FinalExam asks for the question type before each question: 1 = True/False, 2 = MCQ.</summary>
        public static string[] Typed(int type, string[] data)
            => Join(new[] { type.ToString() }, data);

        /// <summary>Menu option 1 for a Final exam (the questions must already include their type line).</summary>
        public static string[] MenuCreateFinal(string subject, int id, int time, params string[][] typedQuestions)
            => Join(new[] { "1", subject, id.ToString(), "1", time.ToString(), typedQuestions.Length.ToString() },
                    Join(typedQuestions));

        /// <summary>Menu option 1 for a Practical exam (MCQ questions only, no type line).</summary>
        public static string[] MenuCreatePractical(string subject, int id, int time, params string[][] mcqQuestions)
            => Join(new[] { "1", subject, id.ToString(), "2", time.ToString(), mcqQuestions.Length.ToString() },
                    Join(mcqQuestions));
    }

    /// <summary>Ready-made exams used by many tests.</summary>
    public static class TestData
    {
        /// <summary>
        /// Final exam "Math", 30 minutes, 2 questions:
        /// Q1 True/False (mark 2, right = true), Q2 MCQ (mark 3, answers Four/Five/Six, right = 2). Total = 5.
        /// </summary>
        public static FinalExam MakeFinal()
        {
            string[] lines = Inputs.Join(
                Inputs.Typed(1, Inputs.Tf("TF1", "Sky is blue", "2", "true")),
                Inputs.Typed(2, Inputs.Mcq("MCQ1", "2+3?", "3", 2, "Four", "Five", "Six")));

            using var c = new ConsoleSession(lines);
            var exam = new FinalExam(30, 2);
            exam.Subject = new Subject("Math", 1);
            return exam;
        }

        /// <summary>
        /// Practical exam "Physics", 20 minutes, with <paramref name="questionCount"/> MCQ questions.
        /// Each question has the answers Red/Blue and the right answer is 1 (Red).
        /// </summary>
        public static PracticalExam MakePractical(int questionCount = 2)
        {
            var parts = new List<string[]>();
            for (int i = 1; i <= questionCount; i++)
                parts.Add(Inputs.Mcq("P" + i, "Body" + i, "1", 1, "Red", "Blue"));

            using var c = new ConsoleSession(Inputs.Join(parts.ToArray()));
            var exam = new PracticalExam(20, questionCount);
            exam.Subject = new Subject("Physics", 2);
            return exam;
        }
    }
}
