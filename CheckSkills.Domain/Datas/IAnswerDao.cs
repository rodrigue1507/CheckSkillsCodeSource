using CheckSkills.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace CheckSkills.Domain
{
    public interface IAnswerDao
    {
        IEnumerable<Answer> GetAll();
        Answer GetById(int answerId);
        int CreateAnswer(Answer r);
        int UpdateAnswer(Answer r);
        void DeleteAnswer(int answerId);
        void DeleteAnswerQuestionId(int questionId);
    }
}
