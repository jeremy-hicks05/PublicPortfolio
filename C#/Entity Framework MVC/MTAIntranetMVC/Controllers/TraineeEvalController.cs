using Microsoft.AspNetCore.Mvc;
using MTAIntranet.MVC.Utility;
using MTAIntranetMVC.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using MTAIntranet.MVC.Models;
using MTAIntranet.Shared;

namespace MTAIntranetMVC.Controllers
{
    [AllowAnonymous]
    public class TraineeEvalController : Controller
    {

        private readonly MTAIntranetContext db;
        public TraineeEvalController(MTAIntranetContext _db)
        {
            db = _db;
        }

        [HttpGet]
        public IActionResult EnterEval()
        {
            TraineeEvalModel model = new TraineeEvalModel();

            return View(model);
        }

        [HttpGet]
        public IActionResult GetEval(int EmployeeNum)
        {
            ViewTraineeEvalsModel model = new()
            {
                TraineeEvals = db.TraineeEvals!.Where(ev => ev.EmployeeNumber == EmployeeNum).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitTraineeEval(TraineeEvalModel model)
        {
            TraineeEval eval = new()
            {
                EmployeeNumber = model.EmployeeNumber,
                EmployeeFirstName = model.EmployeeFirstName,
                EmployeeLastName = model.EmployeeLastName,
                Department = model.Department,
                Date = model.Date,
                VehicleType = model.VehicleType,
                DIName = model.DIName,
                PrePostTrips = model.PrePostTrips,
                RadioEtiquette = model.RadioEtiquette,
                CustomerService = model.CustomerService,
                DefensiveDriving = model.DefensiveDriving,
                RRCrossings = model.RRCrossings,
                WheelChairSecurements = model.WheelChairSecurements,
                Comments = model.Comments
            };
            db.Add(eval);
            db.SaveChanges();

            TempData["success"] = "Daily Trainee Evaluation Entered";

            return RedirectToAction("EnterEval");
            //return View(model);
        } // "2180 and 2283"

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}