using CheckSkills.Domain.Entities;
using System.Collections.Generic;

namespace CheckSkills.Domain
{
    public interface ISurvey_QuestionDao
    {
      IEnumerable<Survey> GetAllSurvey();
      void DeleteQuestionSurvey(int questionId,int surveyId);
      IEnumerable<Question> GetSurvey_Questions(int surveyId);
        void DeleteQuestionsSurvey(int questionId);
    }
}
    