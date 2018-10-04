using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//renvoie la liste des questions selectionnée et utilisé dans le viewmodel SurveyFilterInfoViewModel
namespace CheckSkills.WebSite.ViewModels
{
    public class SurveySelectedQuestionViewModel
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
    }
}
