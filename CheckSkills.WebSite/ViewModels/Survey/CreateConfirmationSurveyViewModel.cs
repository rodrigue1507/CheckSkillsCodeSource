using CheckSkills.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels

{
    public class CreateConfirmationSurveyViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; }

        public IEnumerable<QuestionViewModel> SurveySelectedQuestions { get; set; }

        public IEnumerable<int> OriginalSurveySelectedQuestions { get; set; }
    }
}
