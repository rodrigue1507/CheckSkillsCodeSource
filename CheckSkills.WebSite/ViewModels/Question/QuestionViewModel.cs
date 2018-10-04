using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }

        [Display(Name = "Categorie")]
        public string CategoryName { get; set; }

        [Display(Name = "Type")]
        public string QuestionTypeName { get; set; }

        [Display(Name = "Type")]
        public string TypeName { get; set; }

        public string DifficultyLevel { get; set; }

        public bool IsChecked { get; set; }

        public List<CreateOrUpdateQuestionAnswerViewModel> QuestionAnswerList { get; set; }      
    }
}