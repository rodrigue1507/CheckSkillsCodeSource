using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    public class CreateSurveyViewModel
    {
        public int? Id { get; set; }

        [Display(Name = "Donner un nom au Formulaire")]
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Name { get; set; }
        public string SurveyEvaluation { get; set; }

        [Display(Name="Catégorie")]
        public int? CategoryId { get; set; }

        [Display(Name = "Difficulté")]
        public int? DifficultyId { get; set; }

        [Display(Name = "Type")]
        public int? TypeId { get; set; }


        public IList<QuestionViewModel> SurveySelectedQuestions { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Difficulties { get; set; }
        public IEnumerable<SelectListItem> QuestionTypes { get; set; }
    }
}
