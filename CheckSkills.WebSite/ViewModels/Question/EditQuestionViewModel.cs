using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    //methode contenant tous les éléments pour la creation de questions
    public class EditQuestionViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Vous avez oublié de remplir le champ contenu")]
        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Required(ErrorMessage = " La catégorie de question n'a pas été précisé")]
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Le niveau de difficulté n'a pas été précisé")]
        [Display(Name = "Difficulté")]
        public int DifficultyId { get; set; }

        [Required(ErrorMessage = "Le type de question n'a pas été précisé")]
        [Display(Name = "Type")]
        public int TypeId { get; set; }

       

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Difficulties { get; set; }
        public IEnumerable<SelectListItem> QuestionTypes { get; set; }
        public IEnumerable<SelectListItem> Answers { get; set; }

        public bool IsNew { get; set; }
    }
}
