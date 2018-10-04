using System;
using System.Collections.Generic;
using System.Text;

namespace CheckSkills.Domain.Entities
    {
        public class Question
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public QuestionCategory Category  { get; set; }
            public QuestionDifficulty Difficulty { get; set; }
            public QuestionType Type { get; set; }
            public IEnumerable<Answer> Answers { get; set; }
            public string QuestionEvaluation { get; set; }

        }
    }
