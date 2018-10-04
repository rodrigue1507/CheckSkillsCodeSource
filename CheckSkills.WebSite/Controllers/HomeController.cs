using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CheckSkills.WebSite.ViewModels;
using CheckSkills.DAL;
using CheckSkills.Domain;

namespace CheckSkills.WebSite.Controllers
{
    public class HomeController : Controller
    {  
        public HomeController()
         
            :this(new QuestionDao(new AnswerDao()))
        {

        }

        private HomeController(IQuestionDao questionDao)
        {

        }

        [HttpGet]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpGet]
        public IActionResult ListQuestions()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
