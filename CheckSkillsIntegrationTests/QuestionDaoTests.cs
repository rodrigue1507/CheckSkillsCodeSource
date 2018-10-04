using CheckSkills.DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CheckSkills.IntegrationTests
{
    [TestClass]
    public class QuestionDaoTests
    {
        [TestMethod]
        public void Can_GeAll_Questions()
        {
            // Arrange

            var questionDao = new QuestionDao();

            //Action
            var result = questionDao.GetAll();

            // AAsserts
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() > 0);
        }
    }
}
