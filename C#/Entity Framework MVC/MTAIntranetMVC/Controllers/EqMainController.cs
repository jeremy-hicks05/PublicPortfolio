using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranet.MVC.Models;
using MTAIntranet.MVC.Utility;
using MTAIntranet.Shared;
using MTAIntranetMVC.Models;
using System.Data;
using System.Diagnostics;

namespace MTAIntranetMVC.Controllers
{
    [Authorize(Roles = AccessRoles.ITS + "," +
        AccessRoles.Administration + "," +
        AccessRoles.Maintenance + "," +
        AccessRoles.EAM)]
    public class EqMainController : Controller
    {
        private readonly EAMProdContext db;

        public EqMainController(EAMProdContext injectedContext)
        {
            db = injectedContext;
        }

        [Route("EqMain")]
        public IActionResult EqMain()
        {
            EqMainModel model = new EqMainModel(db);
            return View(model);
        }

        [Route("EqMain/{location?}")]
        public IActionResult EqMain(string? location)
        {
            EqMainModel model = new EqMainModel(db, location);
            return View(model);
        }

        [AllowAnonymous]
        [Route("EqMain/Anonymous/")]
        public IActionResult EqMainAnon()
        {
            EqMainModel model = new EqMainModel(db);
            return View(model);
        }

        [AllowAnonymous]
        [Route("EqMain/Anonymous/{location?}")]
        public IActionResult EqMainAnon(string? location)
        {
            EqMainModel model = new EqMainModel(db, location);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}