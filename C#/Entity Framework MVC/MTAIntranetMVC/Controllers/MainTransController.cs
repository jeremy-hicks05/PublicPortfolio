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
    //[Authorize(Roles = AccessRoles.Maintenance)]
    //[Authorize(Roles = AccessRoles.Administration)]
    [Authorize(Roles = AccessRoles.ITS + "," + 
        AccessRoles.Maintenance + "," +
        AccessRoles.Administration)]
    public class MainTransController : Controller
    {
        private readonly FuelmasterContext db;

        public MainTransController(FuelmasterContext injectedContext)
        {
            db = injectedContext;
        }

        [Route("Fuelmaster")]
        public IActionResult MainTrans()
        {
            MainTransModel model = new MainTransModel(db);
            return View(model);
        }

        [Route("Fuelmaster/Search")]
        public IActionResult MainTransOdometer(string vehicleId, int odometer)
        {
            MainTransModel model;
            if (odometer == 0)
            {
                model = new MainTransModel(db, vehicleId);
            }
            else
            {
                model = new MainTransModel(db, vehicleId, odometer);
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}