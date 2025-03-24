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
    [Authorize(Roles = AccessRoles.ITS)]
    public class MaxQueueController : Controller
    {
        private readonly EAMProdContext db;

        public MaxQueueController(EAMProdContext injectedContext)
        {
            db = injectedContext;
        }

        [Route("MaxQueue")]
        public IActionResult MaxQueue()
        {
            MaxQueueModel model = new MaxQueueModel(db);
            model.MaxQueueList = db!.MaxQueues!
                .Where(
                    mq => mq.ConnectorName == "1764357731^1467^1470")
                .OrderBy(mq => mq.X_datetime_insert).ToList();
            return View(model);
        }

        [Route("MaxQueue/PurgeRangeErrors")]
        public IActionResult PurgeRangeErrors()
        {
            MaxQueueModel model = new MaxQueueModel(db);
            model.MaxQueueList = 
                db!.MaxQueues!
                .Where(
                    mq => mq.ConnectorName == "1764357731^1467^1470" && 
                    mq.ErrorMessage!.Contains("Meter 1"))
                .ToList();

            db.RemoveRange(model.MaxQueueList);
            //db.
            db.SaveChanges();

            return RedirectToAction("MaxQueue");
        }

        //[Route("MaxQueue/PurgeMiscCostErrors")]
        //public IActionResult PurgeMiscCostErrors()
        //{
        //    MaxQueueModel model = new MaxQueueModel(db);
        //    model.MaxQueueList =
        //        db!.MaxQueues!
        //        .Where(
        //            mq => mq.ConnectorName == "1764357731^1467^1470" &&
        //            mq.ErrorMessage!.Contains("Value is required for Misc cost"))
        //        .ToList();

        //    db.RemoveRange(model.MaxQueueList);
        //    //db.
        //    db.SaveChanges();

        //    return RedirectToAction("MaxQueue");
        //}

        [Route("MaxQueue/ViewBadTransactions")]
        public IActionResult ViewBadTransactions()
        {
            MaxQueueModel model = new MaxQueueModel(db);
            model.MaxQueueList =
                db!.MaxQueues!
                .Where(
                    mq => mq.ConnectorName == "1764357731^1467^1470" &&
                    mq.ErrorMessage!.Contains("Meter 1"))
                .OrderBy(mq => mq.X_datetime_insert)
                .ToList();
            return View(model);
        }

        //[Route("MaxQueue/ViewMiscCostErrors")]
        //public IActionResult ViewMiscCostErrors()
        //{
        //    MaxQueueModel model = new MaxQueueModel(db);
        //    model.MaxQueueList =
        //        db!.MaxQueues!
        //        .Where(
        //            mq => mq.ConnectorName == "1764357731^1467^1470" &&
        //            mq.ErrorMessage!.Contains("Value is required for Misc cost"))
        //        .OrderBy(mq => mq.X_datetime_insert)
        //        .ToList();
        //    return View(model);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}