using System.Collections.Generic;

namespace CheckSkills.WebSite.ViewModels
{
    public class EditAnswerViewModel
    {
        public int QuestionId { get; set; }
        public List<CreateOrUpdateQuestionAnswerViewModel> QuestionAnswerList { get; set; }
    }
}
