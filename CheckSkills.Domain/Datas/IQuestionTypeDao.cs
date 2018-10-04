using CheckSkills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckSkills.Domain
{
    public interface IQuestionTypeDao
    {
        IEnumerable<QuestionType> GetAll();
    }
}
