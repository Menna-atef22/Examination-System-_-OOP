namespace Examination_System___OOP
{
    /// <summary>
    /// The console menu of the examination system: create exams, take them,
    /// and try <see cref="ICloneable"/> and <see cref="IComparable"/>.
    /// </summary>
    public class Program
    {
        /// <summary>All the exams created in this run.</summary>
        static List<Exam> exams = new List<Exam>();

        /// <summary>Shows the menu again and again until the user chooses 0 (exit).</summary>
        public static void Main(string[] args)
        {
            exams.Clear();   // start clean

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("========== EXAMINATION SYSTEM ==========");
                Console.WriteLine("1. Create an exam (Final / Practical)");
                Console.WriteLine("2. List the exams");
                Console.WriteLine("3. Take an exam");
                Console.WriteLine("4. Show an exam with its right answers");
                Console.WriteLine("5. Clone a question (ICloneable)");
                Console.WriteLine("6. Compare two exams (IComparable)");
                Console.WriteLine("7. Sort the exams by number of questions (IComparable)");
                Console.WriteLine("0. Exit");

                int choice = ConsoleInput.ReadInt("Your choice: ", 0, 7);
                Console.WriteLine();

                switch (choice)
                {
                    case 1: CreateExam(); break;
                    case 2: ListExams(); break;
                    case 3: TakeExam(); break;
                    case 4: ShowRightAnswers(); break;
                    case 5: CloneQuestion(); break;
                    case 6: CompareExams(); break;
                    case 7: SortExams(); break;
                    case 0:
                        Console.WriteLine("Goodbye!");
                        return;
                }
            }
        }

        /// <summary>Prints the numbered exams.</summary>
        static void ListExams()
        {
            if (exams.Count == 0)
            {
                Console.WriteLine("No exams yet. Create one first (option 1).");
                return;
            }

            for (int i = 0; i < exams.Count; i++)
                Console.WriteLine($"{i + 1}. {exams[i]}");
        }

        /// <summary>Lists the exams and asks the user to choose one. Returns <c>null</c> if there are no exams.</summary>
        static Exam? PickExam(string prompt)
        {
            if (exams.Count == 0)
            {
                Console.WriteLine("No exams yet. Create one first (option 1).");
                return null;
            }

            ListExams();
            int number = ConsoleInput.ReadInt(prompt, 1, exams.Count);
            return exams[number - 1];
        }

        /// <summary>Option 1: reads a subject, then creates its exam (the exam reads its own questions).</summary>
        static void CreateExam()
        {
            string name = ConsoleInput.ReadText("Subject name: ");
            int id = ConsoleInput.ReadInt("Subject ID: ", 1);

            Subject subject = new Subject(name, id);
            Exam exam = subject.CreateExam();
            exams.Add(exam);

            Console.WriteLine();
            Console.WriteLine("Exam created: " + exam);
        }

        /// <summary>Option 3: lets the student take a chosen exam.</summary>
        static void TakeExam()
        {
            Exam? exam = PickExam("Choose the exam to take: ");
            if (exam != null)
                exam.ShowExam();
        }

        /// <summary>Option 4: prints a chosen exam with its right answers.</summary>
        static void ShowRightAnswers()
        {
            Exam? exam = PickExam("Choose an exam: ");
            if (exam != null)
                exam.ShowModelAnswer();
        }

        /// <summary>Option 5 (ICloneable): clones a question, edits the copy, and shows the original did not change.</summary>
        static void CloneQuestion()
        {
            Exam? exam = PickExam("Choose the exam that has the question: ");
            if (exam == null) return;

            int number = ConsoleInput.ReadInt($"Question number (1-{exam.Questions.Length}): ", 1, exam.Questions.Length);
            Question original = exam.Questions[number - 1];
            Question copy = (Question)original.Clone();

            Console.WriteLine();
            Console.WriteLine("Original : " + original);
            Console.WriteLine("Cloned   : " + copy);
            Console.WriteLine("Clone is a different object: " + !ReferenceEquals(copy, original));

            copy.Header = copy.Header + " (COPY)";
            Console.WriteLine("After editing the copy:");
            Console.WriteLine("  Original header : " + original.Header);
            Console.WriteLine("  Copy header     : " + copy.Header);
        }

        /// <summary>Option 6 (IComparable): compares two exams by their number of questions.</summary>
        static void CompareExams()
        {
            if (exams.Count < 2)
            {
                Console.WriteLine("You need at least 2 exams (create more with option 1).");
                return;
            }

            Exam? first = PickExam("Choose the first exam: ");
            Exam? second = PickExam("Choose the second exam: ");
            if (first == null || second == null) return;

            Console.WriteLine();
            Console.WriteLine($"First exam : {first.NumberOfQuestions} questions");
            Console.WriteLine($"Second exam: {second.NumberOfQuestions} questions");

            int result = first.CompareTo(second);
            if (result < 0)
                Console.WriteLine("The first exam has FEWER questions.");
            else if (result > 0)
                Console.WriteLine("The first exam has MORE questions.");
            else
                Console.WriteLine("Both exams have the same number of questions.");
        }

        /// <summary>Option 7 (IComparable): sorts the exams by number of questions with <c>Array.Sort</c>.</summary>
        static void SortExams()
        {
            if (exams.Count == 0)
            {
                Console.WriteLine("No exams yet. Create one first (option 1).");
                return;
            }

            Exam[] sorted = exams.ToArray();
            Array.Sort(sorted);   // works because Exam implements IComparable

            Console.WriteLine("Exams sorted by number of questions (ascending):");
            foreach (Exam exam in sorted)
                Console.WriteLine("  " + exam);
        }
    }
}