# Examination System (OOP)

A console application written in C# that lets a teacher create subjects and exams (Final or Practical), and lets a student take them. It was built to practice object-oriented programming: abstraction, inheritance, polymorphism, encapsulation, and the `ICloneable` and `IComparable` interfaces.

## Features

- Create **subjects** (name + unique ID).
- Create an **exam** for a subject, either:
  - **Final exam**: each question is True/False or MCQ. The student gets a grade (marks and percentage), and the right answer is shown for every wrong answer.
  - **Practical exam**: MCQ questions only. There is no grade; the right answers are shown at the end.
- **Take an exam** from the menu.
- Show an exam with its **right answers** (model answer) without asking anything.
- Demonstrate **`ICloneable`**: deep-copy a question, edit the copy, and prove the original did not change.
- Demonstrate **`IComparable`**: compare two exams, or sort all exams, by number of questions.
- Input is validated everywhere (numbers in range, no empty text, `true`/`false`), and an MCQ question refuses **duplicate answers**.

## The menu

```
========== EXAMINATION SYSTEM ==========
1. Create a subject
2. Create an exam for a subject (Final / Practical)
3. List subjects and their exams
4. Take an exam
5. Show an exam with its right answers (model answer)
6. Clone a question (ICloneable demo)
7. Compare two exams (IComparable demo)
8. Sort all exams by number of questions (IComparable demo)
0. Exit
```

## Requirements

- [.NET SDK 10](https://dotnet.microsoft.com/download) (the project targets `net10.0`)

## Run

From the project folder (the one that contains `Examination System _ OOP.csproj`):

```bash
dotnet run
```

Or open the solution in Visual Studio and press **F5**.

## Classes

| Class | Kind | Role |
|---|---|---|
| `Question` | abstract, `ICloneable` | Header, body, mark and right answer. Each type reads, shows, checks and clones itself. |
| `TrueFalseQuestion` | derives from `Question` | The right answer is a `bool`. |
| `MCQQuestion` | derives from `Question` | Has an array of `Answer`; the right answer is one of them. `Clone()` is a deep copy. |
| `Answer` | `ICloneable` | One choice of an MCQ question: an ID and a text. |
| `Exam` | abstract, `IComparable` | Time, number of questions, questions, and subject. Exams are compared by number of questions. |
| `FinalExam` | derives from `Exam` | True/False + MCQ questions, with a grade. |
| `PracticalExam` | derives from `Exam` | MCQ questions only, shows the right answers at the end. |
| `Subject` | | Name, ID and one exam. `CreateExam()` builds the exam the user chooses. |
| `ConsoleInput` | static helper | Reads and validates numbers, text and booleans from the console. |
| `Program` | | The menu. |

### OOP concepts used

- **Abstraction:** `Question` and `Exam` are abstract and define what every type must do (`DisplayQuestion`, `CheckAnswer`, `ShowExam`, ...).
- **Inheritance:** `TrueFalseQuestion` and `MCQQuestion` extend `Question`; `FinalExam` and `PracticalExam` extend `Exam`.
- **Polymorphism:** an exam works with `Question` objects only. For example, `GetCorrectAnswerText()` lets any exam print a right answer without checking the question type.
- **Encapsulation:** private fields with properties; setters validate (`Time`, `NumberOfQuestions` and `Mark`).
- **`ICloneable`:** `Answer` and `Question` can be copied. `MCQQuestion.Clone()` also clones the answers.
- **`IComparable`:** `Exam.CompareTo` compares by number of questions, so `Array.Sort` works on exams.

## Project structure

```
Examination System _ OOP/
├── Examination System _ OOP.csproj
├── Program.cs
├── Subject.cs
├── Exam.cs
├── FinalExam.cs
├── PracticalExam.cs
├── Question.cs
├── TrueFalseQuestion.cs
├── MCQQuestion.cs
├── Answer.cs
├── ConsoleInput.cs
├── Examination_System___UML.drawio.png   (class diagram)
└── Tests/                                (xUnit test project)
```

## Tests

The `Tests` folder is an xUnit project that tests every class and the whole menu (the tests type into the console and read what the program prints).

```bash
cd Tests
dotnet test
```

or use **Test Explorer** in Visual Studio.

## Example session

```
Choose exam type (1 = Final, 2 = Practical): 1
Exam time (minutes): 30
Number of questions: 2

--- Question 1 ---
Type (1 = True/False, 2 = MCQ): 1
...

===== FINAL EXAM: Math (30 min) =====

--- Question 1 of 2 ---
True/False Question:
Question: Q1
Body: Body1
Mark: 2
Enter your answer (true/false): true
Correct!

Grade: 2 / 5
Percentage: 40%
```

## Class diagram

![UML class diagram](Examination_System___UML.drawio.png)
