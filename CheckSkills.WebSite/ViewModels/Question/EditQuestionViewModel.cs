using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    //methode contenant tous les éléments pour la creation de questions
    public class EditQuestionViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Le contenu est obligatoire")]
        [Display(Name = "Contenu")]
        public string Content { get; set; }

        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        [Display(Name = "Difficulté")]
        public int DifficultyId { get; set; }

        [Display(Name = "Type")]
        public int TypeId { get; set; }

        [Display(Name = "Nom du type")]
        public string TypeName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Difficulties { get; set; }
        public IEnumerable<SelectListItem> QuestionTypes { get; set; }
        public IEnumerable<SelectListItem> Answers { get; set; }

        public bool IsNew { get; set; }
    }
}
