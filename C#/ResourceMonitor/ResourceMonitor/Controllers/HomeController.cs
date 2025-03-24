using Microsoft.AspNetCore.Mvc;

namespace ResourceMonitor.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
