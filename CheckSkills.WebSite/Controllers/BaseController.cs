using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CheckSkills.WebSite.Controllers
{
    public abstract class BaseController : Controller
    {
        public string Success {
            set
            {
                TempData["Success"] = ViewData["Success"] = value;
            }}
    }
}
