
using CheckSkills.Domain.Constants;

namespace CheckSkills.WebSite.ViewModels
{
    //methode contenant tous les éléments pour la creation de questions
    public class CreateOrUpdateQuestionViewModel
    {
        public QuestionTypeEnum QuestionTypeEnum { get; set; }
        public EditQuestionViewModel EditQuestionViewModel { get; set; }
        public EditAnswerViewModel EditAnswerViewModel { get; set; }
        
    }
}
