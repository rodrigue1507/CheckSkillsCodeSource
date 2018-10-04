using CheckSkills.Domain.Entities;
using System.Collections.Generic;

namespace CheckSkills.Domain
{
    public interface IQuestionCategoryDao
    {
        IEnumerable<QuestionCategory> GetAllQuestionCategory();
    }
}