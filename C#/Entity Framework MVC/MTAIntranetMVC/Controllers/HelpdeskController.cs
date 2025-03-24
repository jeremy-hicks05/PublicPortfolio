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
    //[AllowAnonymous]
    public class HelpdeskController : Controller
    {
        public HelpdeskController()
        {

        }

        //[AllowAnonymous]
        [Route("Helpdesk")]
        public IActionResult Helpdesk()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}