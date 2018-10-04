using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels

{
    public class SurveyViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime CreationDate { get; set; }
        public string surveyEvaluation { get; set; }
    }
}
