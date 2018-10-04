using CheckSkills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CheckSkills.WebSite.Controllers.SurveyController;

namespace CheckSkills.WebSite.ViewModels
{
    public class SaveSurveyViewModel
    {
        public int surveyId { get;  }
        public string surveyName { get; set; }
        public IEnumerable<SurveySelectedQuestionViewModel> SurveySelectedQuestions { get; set; }
    }
}
