using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheckSkills.WebSite.ViewModels.Answer
{
    public class QuestionAnswerViewModel
    {
        public int? AnswerId { get; set; }

        public int QuestionId { get; set; }

        [Display(Name="Question")]
        public string QuestionContent { get; set; }

        [Display(Name = "Réponse")]
        public string AnswerContent { get; set; }
    }
}
