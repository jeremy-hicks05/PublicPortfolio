using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTAIntranet.MVC.Utility;
using MTAIntranetMVC.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace MTAIntranetMVC.Controllers
{
    [Authorize(Roles = AccessRoles.Everyone)]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            // changed the "Home Page" for EAM as a demonstration
            if (User.Identity is not null && 
                User.Identity.Name is not null && 
                User.Identity.Name.Split("\\").Last() == "eam")
            {
                return RedirectToAction("EqMain", "EqMain");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}