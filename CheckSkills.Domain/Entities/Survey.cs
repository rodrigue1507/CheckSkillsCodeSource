using System;
using System.Collections.Generic;
using System.Text;

namespace CheckSkills.Domain
{
    public class Survey
    {
        public Survey()
        {
            CreationDate = DateTime.Now ;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string SurveyEvaluation  { get; set; }
    }
}
