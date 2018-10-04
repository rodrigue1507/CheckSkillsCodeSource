using CheckSkills.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckSkills.WebSite.ViewModels
{
    public class DeleteQuestionAnswerViewModel
    {

        public int qestionId { get; set; }
        public int answerId { get; set; }

    }
}