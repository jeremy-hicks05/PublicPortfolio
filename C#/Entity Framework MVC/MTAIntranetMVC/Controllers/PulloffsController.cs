using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTAIntranet.MVC.Utility;
using MTAIntranet.Shared;
using MTAIntranetMVC.Models;
using System.Diagnostics;

namespace MTAIntranetMVC.Controllers
{
    [Authorize(Roles = AccessRoles.ITS + "," +
        AccessRoles.Administration + "," +
        AccessRoles.PulloffsAdmin)]
    public class PulloffsController : Controller
    {
        private readonly ILogger<PulloffsController> _logger;
        private readonly MTAIntranetContext db;

        public PulloffsController(ILogger<PulloffsController> logger,
            MTAIntranetContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        [Route("{controller}/{year}/{id?}")]
        //https://localhost:7198/Pulloffs/2022/1
        public async Task<IActionResult> PulloffsByMonth(int year, int? id)
        {
            PulloffsByMonthModel model = new(db, year, id);

            if (!id.HasValue)
            {
                if (db.Pulloffs != null)
                {
                    model.Pulloffs = await db.Pulloffs
                        .Where(p => p.PulloffTime!.Value.Year == year)
                        .OrderBy(p => p.Route_Name)
                        .ThenBy(p => p.PulloffTime).ToListAsync();

                    return View(model);
                }
            }
            else if (id > 0 && id < 13)
            {
                if (db.Pulloffs != null)
                {
                    model.Pulloffs = await db.Pulloffs
                        .Where(p => p.PulloffTime!.Value.Year == year)
                        .OrderBy(p => p.PulloffTime)
                        .Where(
                            p => p.PulloffTime != null &&
                            p.PulloffTime.Value.Month == id).ToListAsync();

                    return View(model);
                }
            }
            else
            {
                return NotFound($"Month must be between 1 and 12. {id} is not accepted.");
            }

            return View(model);
        }

        [Route("{controller}/{action}/{id}")]
        public IActionResult DeletePulloff(int year, int id)
        {
            PulloffsByMonthModel model = new(db, year, id);

            if (db.Pulloffs != null)
            {
                model.Pulloffs = db.Pulloffs
                    .Where(
                        p => p.PulloffID == id).ToList();
            }

            if (model.Pulloffs != null && model.Pulloffs.Count == 0)
            {
                return NotFound($"Pulloff with ID {id} not found.");
            }

            return View(model);
        }

        [Route("{controller}/{action}/{id?}")]
        public IActionResult Delete(int year, int? id)
        {
            Pulloff? pulloff = db.Pulloffs!.FirstOrDefault(p => p.PulloffID == id);

            if (id is not null)
            {
                if (pulloff is not null)
                {
                    db.Remove(pulloff);
                    db.SaveChanges();
                }
            }
            return RedirectToAction(year.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}