using Microsoft.AspNetCore.Mvc;

namespace SnapManager.Controllers.ViewsControllers
{
    public class ConfigurationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DbSettings()
        {
            return View();
        }

        public IActionResult Feedback()
        {
            return View();
        }
    }
}
