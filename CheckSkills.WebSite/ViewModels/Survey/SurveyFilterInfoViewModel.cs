using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CheckSkills.WebSite.Controllers.SurveyController;

namespace CheckSkills.WebSite.ViewModels
{
    public class SurveyFilterInfoViewModel
    {
        public int CategoryId { get; set; }
        public int DifficultyId { get; set; }
        public int TypeId { get; set; }
        public IEnumerable<SurveySelectedQuestionViewModel> SurveySelectedQuestions { get; set; }
    }
}
