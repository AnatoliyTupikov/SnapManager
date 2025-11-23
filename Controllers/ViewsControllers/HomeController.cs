using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnapManager.Data;

namespace SnapManager.Controllers.Test
{
    public class HomeController : Controller
    {
        private DbService dbContextBuilder;
        public HomeController(DbService dbcbuilder)
        {
            dbContextBuilder = dbcbuilder;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Эндпойнт, который будет обрабатывать запросы на /Home/Greet
        public IActionResult Greet()
        {
            return Content("Привет от контроллера!");
        }

        public IActionResult CredentialsWizard()
        {
            return View();
        }
    }
}
