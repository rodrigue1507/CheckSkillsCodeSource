using System;
using System.Collections.Generic;
using System.Text;

namespace CheckSkills.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
    }
}
