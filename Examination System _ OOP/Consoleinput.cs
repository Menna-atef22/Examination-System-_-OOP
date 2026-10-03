using System;
using System.Globalization;

namespace Examination_System___OOP
{
    /// <summary>
    /// Small helper so every class reads and validates console input the same way.
    /// Every method keeps asking until the user types a valid value.
    /// </summary>
    public static class ConsoleInput
    {
        /// <summary>Reads one line from the console.</summary>
        /// <exception cref="InvalidOperationException">The input stream is closed (no more lines).</exception>
        private static string ReadLineOrFail()
        {
            string? line = Console.ReadLine();
            if (line == null)
                throw new InvalidOperationException("Input stream closed.");
            return line;
        }

        /// <summary>Asks for a whole number inside a range.</summary>
        /// <param name="prompt">The text printed before reading.</param>
        /// <param name="min">The smallest accepted value.</param>
        /// <param name="max">The largest accepted value (no upper limit by default).</param>
        /// <returns>The first valid number the user typed.</returns>
        /// <exception cref="InvalidOperationException">The input stream is closed.</exception>
        public static int ReadInt(string prompt, int min, int max = int.MaxValue)
        {
            Console.Write(prompt);
            int value;
            while (!int.TryParse(ReadLineOrFail(), out value) || value < min || value > max)
            {
                if (max == int.MaxValue)
                    Console.Write($"Invalid input, enter a number >= {min}: ");
                else
                    Console.Write($"Invalid input, enter a number between {min} and {max}: ");
            }
            return value;
        }

        /// <summary>
        /// Tries to read a finite decimal number. The dot is always the decimal separator ("2.5"),
        /// whatever the language settings of the computer are. NaN and Infinity are refused.
        /// </summary>
        private static bool TryParseFinite(string? input, out double value)
        {
            return double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                   && !double.IsNaN(value)
                   && !double.IsInfinity(value);
        }

        /// <summary>Asks for a decimal number that is not smaller than a minimum.</summary>
        /// <param name="prompt">The text printed before reading.</param>
        /// <param name="min">The smallest accepted value.</param>
        /// <returns>The first valid number the user typed.</returns>
        /// <exception cref="InvalidOperationException">The input stream is closed.</exception>
        public static double ReadDouble(string prompt, double min)
        {
            Console.Write(prompt);
            double value;
            while (!TryParseFinite(ReadLineOrFail(), out value) || value < min)
                Console.Write($"Invalid input, enter a number >= {min}: ");
            return value;
        }

        /// <summary>Asks for a decimal number that is strictly greater than 0 (for example a mark).</summary>
        /// <param name="prompt">The text printed before reading.</param>
        /// <returns>The first valid number the user typed.</returns>
        /// <exception cref="InvalidOperationException">The input stream is closed.</exception>
        public static double ReadPositiveDouble(string prompt)
        {
            Console.Write(prompt);
            double value;
            while (!TryParseFinite(ReadLineOrFail(), out value) || value <= 0)
                Console.Write("Invalid input, enter a number greater than 0: ");
            return value;
        }

        /// <summary>Asks for a text that is not empty.</summary>
        /// <param name="prompt">The text printed before reading.</param>
        /// <returns>The text the user typed, without the spaces at its start and end.</returns>
        /// <exception cref="InvalidOperationException">The input stream is closed.</exception>
        public static string ReadText(string prompt)
        {
            Console.Write(prompt);
            string text = ReadLineOrFail().Trim();
            while (text.Length == 0)
            {
                Console.Write("Cannot be empty, try again: ");
                text = ReadLineOrFail().Trim();
            }
            return text;
        }

        /// <summary>Asks for true or false. Accepts "true", "t", "false" and "f" in any letter case.</summary>
        /// <param name="prompt">The text printed before reading.</param>
        /// <returns>The boolean the user chose.</returns>
        /// <exception cref="InvalidOperationException">The input stream is closed.</exception>
        public static bool ReadBool(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = ReadLineOrFail().Trim().ToLowerInvariant();
                if (input == "true" || input == "t") return true;
                if (input == "false" || input == "f") return false;
                Console.Write("Invalid input. Enter true or false: ");
            }
        }
    }
}