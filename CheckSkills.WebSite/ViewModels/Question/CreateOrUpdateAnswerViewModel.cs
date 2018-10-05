using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    //methode contenant tous les éléments pour la creation de formulaire
    public class CreateOrUpdateAnswerViewModel
    {
        public int? Id { get; set; }
        
        public int QuestionId { get; set; }

        [Display(Name = "Question")]
        public string QuestionContent { get; set; }
        [Display(Name="Réponse")]
        public string AnswerContent { get; set; }
      


    }
}
