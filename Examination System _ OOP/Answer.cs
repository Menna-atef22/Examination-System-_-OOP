using System;
using System.Collections.Generic;
using System.Text;

namespace Examination_System___OOP
{
    /// <summary>
    /// One possible answer of a multiple-choice question: its number (position in the list) and its text.
    /// Implements <see cref="ICloneable"/> so an answer can be copied.
    /// </summary>
    public class Answer : ICloneable
    {
        private int answerId;
        private string answerText = "";

        /// <summary>The number of the answer inside its question (the first answer is 1).</summary>
        public int AnswerId
        {
            get { return answerId; }
            set { answerId = value; }
        }

        /// <summary>The text shown to the student for this answer.</summary>
        public string AnswerText
        {
            get { return answerText; }
            set { answerText = value; }
        }

        /// <summary>Creates an answer.</summary>
        /// <param name="answerId">The number of the answer inside its question.</param>
        /// <param name="answerText">The text of the answer.</param>
        public Answer(int answerId, string answerText)
        {
            this.AnswerId = answerId;
            this.AnswerText = answerText;
        }

        /// <summary>Creates a new, independent answer with the same id and text.</summary>
        /// <returns>The copy, as an <see cref="Answer"/> boxed in <see cref="object"/>.</returns>
        public object Clone()
        {
            return new Answer(this.AnswerId, this.AnswerText);
        }

        /// <summary>Returns the answer as text: "Answer ID: ..., Answer Text: ...".</summary>
        public override string ToString()
        {
            return $"Answer ID: {AnswerId}, Answer Text: {AnswerText}";
        }


    }
}