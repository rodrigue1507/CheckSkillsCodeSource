using System.Collections.Generic;

namespace CheckSkills.Domain
{
    public interface ISurveyDao
    {
        //IEnumerable<Survey> GetByPreferencies(string name);
        IEnumerable<Survey> GetAllSurvey();

        void CreateSurvey(string name, List<int> questionIds);

        int UpdateSurvey(Survey q);
    
        void DeleteSurvey(int surveyId);

        Survey SelectSurveyInfo(int surveyId);
    }
}
