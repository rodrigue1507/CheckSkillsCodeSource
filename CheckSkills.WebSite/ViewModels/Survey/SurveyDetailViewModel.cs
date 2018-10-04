using CheckSkills.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    public class SurveyDetailViewModel
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public DateTime DateCreation { get; set;}
        public string SurveyEvaluation { get; set;}
        public IEnumerable<QuestionViewModel> SurveySelectedQuestions { get; set;}
    }
}
