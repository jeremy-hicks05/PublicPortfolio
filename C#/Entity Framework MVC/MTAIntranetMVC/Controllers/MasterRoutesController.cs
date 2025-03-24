using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MTAIntranet.MVC.Models;
using MTAIntranet.MVC.Utility;
using MTAIntranet.Shared;
using MTAIntranetMVC.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;
//using MasterRoute = MTAIntranet.MVC.Models.MasterRoute;
//using Pulloff = MTAIntranet.MVC.Models.Pulloff;

namespace MTAIntranetMVC.Controllers
{
    /// <summary>
    /// Handles the routing of 
    /// MasterRouteList, 
    /// MasterRoutePulloffs, 
    /// AddPulloff (Get and Post), 
    /// AddMasterRoute (Get and Post),
    /// DelMasterRoute (Get and Post)
    /// </summary>

    //[Authorize(Roles = AccessRoles.WebAdmin)]
    [Authorize(Roles = AccessRoles.ITS + "," +
        AccessRoles.Administration + "," +
        AccessRoles.MasterRouteAdmin)]
    public class MasterRoutesController : Controller
    {
        //private readonly ILogger<MasterRoutesController> _logger;
        private readonly MTAIntranetContext db;

        public MasterRoutesController(MTAIntranetContext injectedContext
            //, ILogger<MasterRoutesController> logger
            )
        {
            //_logger = logger;
            db = injectedContext;
        }

        [Route("{controller}/{year?}")]
        public IActionResult MasterRouteList(int? year)
        {
            MasterRouteListModel model = new(db, DateTime.Now.Year);
            if (year == null)
            {
                model = new(db, DateTime.Now.Year);
            }
            else
            {
                model = new(db, year);
            }
            return View(model);
        }

        [Route("{controller}/Pulloffs/{year}/{id?}")]
        //https://localhost:7198/MasterRoutes/Pulloffs/2023/SHOPPER01H100030
        public IActionResult MasterRoutePulloffs(string? id, int? year)
        {
            if (id == null)
            {
                MasterRouteListModel? model = new(db, new MrSignature(""), year);

                return View(model);
            }
            else if (id is not null)
            {
                MasterRouteListModel? model = new(db, new MrSignature(id), year);

                return View(model);
            }
            else
            {
                return NotFound($"Route not found.");
            }
        }

        [Route("{controller}/{year}/AddPulloff/{id}")]
        [HttpGet]
        public IActionResult AddPulloff(string id, int year)
        {
            AddPulloffToMasterRouteModel mrModel = new(db,
                new MrSignature(id),
                year);

            return View(mrModel);
        }

        [Route("{controller}/{year}/AddPulloff/{id?}")]
        [HttpPost]
        public IActionResult AddPulloff(PulloffModel pulloff, int year)
        { // change to PulloffModel for validation outside of sql db settings
            //ViewBag.Year = year;
            if (ModelState.IsValid)
            {
                // translate to Pulloff -> Database class
                var pulloffToSave = new Pulloff
                {
                    Route_Name = pulloff.Route_Name,
                    Suffix = pulloff.Suffix,
                    Mode = pulloff.Mode,
                    DoW = pulloff.DoW,
                    Route = pulloff.Route,
                    Run = pulloff.Run,
                    PulloffTime = pulloff.PulloffTime,
                    PulloffReturn = pulloff.PulloffReturn
                };

                db.Add(pulloffToSave);
                db.SaveChanges();

                TempData["success"] = "Pulloff added successfully";

                pulloff.PulloffTime = null;
                pulloff.PulloffReturn = null;

                return View(new AddPulloffToMasterRouteModel(
                    db,
                    pulloff,
                    year));
            }
            return View(
                new AddPulloffToMasterRouteModel(
                    db,
                    pulloff,
                    year)
                );

            //return View();

            //return View(new AddPulloffToMasterRouteModel(
            //        db,
            //        new MrSignature(pulloff.GetSignature()),
            //        year));
        }

        [Route("{controller}/{action}")]
        [HttpGet]
        public IActionResult AddMasterRoute()
        {
            MasterRouteModel model = new();

            return View(model);
        }

        [Route("{controller}/{action}")]
        [HttpPost]
        public IActionResult AddMasterRoute(MasterRouteModel model)
        {
            if (ModelState.IsValid)
            {
                // build MasterRoute
                MasterRoute mrToAdd = new()
                {
                    route_name = model.route_name,
                    mode = model.mode,
                    dow = model.dow,
                    route = model.route,
                    run = model.run,
                    suffix = model.suffix,
                    pull_out_time = model.pull_out_time,
                    pull_in_time = model.pull_in_time,
                    beg_dh_miles = model.beg_dh_miles,
                    end_dh_miles = model.end_dh_miles
                };

                db.Add(mrToAdd);
                db.SaveChanges();
                TempData["success"] = "Master Route added successfully";
            }
            return View(model);
        }


        [Route("{controller}/DeleteMasterRoute/{id?}")]
        [HttpGet]
        public IActionResult DelMasterRoute(int id)
        {
            DelMasterRouteModel model = new(db, id);

            return View(model);
        }

        [Route("{controller}/DeleteMasterRoute/{id?}")]
        [HttpPost]
        public IActionResult DelMasterRoute(MasterRoute masterRoute)
        {
            db.Remove(masterRoute);
            db.SaveChanges();

            return RedirectToAction("MasterRouteList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}