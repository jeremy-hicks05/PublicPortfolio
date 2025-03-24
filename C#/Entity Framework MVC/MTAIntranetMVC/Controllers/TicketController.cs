using Microsoft.AspNetCore.Mvc;
using MTAIntranet.MVC.Utility;
using MTAIntranetMVC.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using MTAIntranet.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MTAIntranet.Shared;

namespace MTAIntranetMVC.Controllers
{
    [Authorize(Roles = AccessRoles.Everyone)]
    public class TicketController : Controller
    {
        private readonly FuelmasterContext _fuelmasterContext;
        public TicketController(FuelmasterContext fuelmasterContext)
        {
            _fuelmasterContext = fuelmasterContext;
        }

        [Route("SubmitTicket")]
        [HttpGet]
        public IActionResult SubmitTicket()
        {
            SubmitTicketModel model = new();

            return View(model);
        }

        [Route("DesktopTicket")]
        [HttpGet]
        public IActionResult DesktopTicket()
        {
            DesktopTicketModel model = new();

            return View(model);
        }

        [Route("HardwareTicket")]
        [HttpGet]
        public IActionResult HardwareTicket()
        {
            HardwareTicketModel model = new();

            return View(model);
        }

        [HttpPost]
        [Route("TicketController/TestFunction/{EmployeeNumber}")]
        public string TestFunction(string EmployeeNumber)
        {
            if (_fuelmasterContext.User != null &&
                _fuelmasterContext.User.First(u => u.USERID == EmployeeNumber) != null)
            {
                string? employeeInfo = _fuelmasterContext.User
                    .First(u => u.USERID == EmployeeNumber).FNAME + "," +
                    _fuelmasterContext.User
                    .First(u => u.USERID == EmployeeNumber).LNAME;

                if (_fuelmasterContext.MainTrans != null &&
                    _fuelmasterContext.MainTrans
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber) != null)
                {
                    // last transaction
                    // TC [2]
                    string? TC = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .TC;
                    // SITE ID [3]
                    string? SITEID = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .SITEID;
                    // VEHICLE ID [4]
                    string? VEHICLEID = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .VEHICLEID;
                    // ODOMETER [5]
                    int? ODOMETER = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .ODOMETER;
                    // TRANTIME [6]
                    DateTime? TRANTIME = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .TRANTIME;
                    // PRODUCT [7]
                    int? PRODUCT = _fuelmasterContext.MainTrans
                        .OrderByDescending(mt => mt.TRANTIME)
                        .FirstOrDefault(mt => mt.USERID == EmployeeNumber)!
                        .PRODUCT;

                    string? SITEIDTEXT = _fuelmasterContext.Site!.First(site => site.SITEID == SITEID).SITENAME;
                    string? PRODUCTTEXT = _fuelmasterContext.ProductConfig!.First(pc => pc.PRODUCT == PRODUCT).DESCRIPT;

                    string? lastTransaction =
                        TC + "," +
                        SITEIDTEXT + "," +
                        VEHICLEID + "," + 
                        ODOMETER + "," + 
                        TRANTIME + "," +
                        PRODUCTTEXT;
                    return employeeInfo + "," + lastTransaction;
                }
                return employeeInfo;
            }
            return "Employee " + EmployeeNumber + ",not found!";
        }

        [Route("FuelmasterKeyTicket")]
        [HttpGet]
        public IActionResult FuelmasterKeyTicket()
        {
            // need to fill dropdowns with data from database for future
            // expansion into content management system

            FuelmasterKeyTicketModel model = new(_fuelmasterContext);

            //model.FuelTypes = new List<SelectListItem>()
            //{
            //    new SelectListItem(text: "Unleaded", value:"Unleaded"),
            //    new SelectListItem(text: "Propane", value:"Propane"),
            //    new SelectListItem(text: "CNG", value:"CNG")
            //};

            //model.Locations = new List<SelectListItem>()
            //{
            //    new SelectListItem(text: "Dort Ops", value:"Dort Ops"),
            //    new SelectListItem(text: "RTW", value:"RTW"),
            //    new SelectListItem(text: "GB", value:"GB"),
            //    new SelectListItem(text: "Flushing", value:"Flushing")
            //};

            return View(model);
        }

        [Route("SoftwareTicket")]
        [HttpGet]
        public IActionResult SoftwareTicket()
        {
            SoftwareTicketModel model = new();

            return View(model);
        }

        [Route("NewUserTicket")]
        [HttpGet]
        public IActionResult NewUserTicket()
        {
            NewUserTicketModel model = new();

            return View(model);
        }

        [Route("DesktopTicket")]
        [HttpPost]
        public IActionResult DesktopTicket(DesktopTicketModel model)
        {
            if (ModelState.IsValid)
            {
                SendTicket(model);

                TempData["success"] = "Ticket Created";

                return RedirectToAction("SubmitTicket");
            }
            return View(model);
        }

        [Route("HardwareTicket")]
        [HttpPost]
        public IActionResult HardwareTicket(HardwareTicketModel model)
        {
            if (ModelState.IsValid)
            {
                SendTicket(model);

                TempData["success"] = "Ticket Created";

                return RedirectToAction("SubmitTicket");
            }
            return View(model);
        }

        

        [Route("FuelmasterKeyTicket")]
        [HttpPost]
        public IActionResult FuelmasterTicket(FuelmasterKeyTicketModel model)
        {
            if (ModelState.IsValid)
            {
                SendTicket(model);

                TempData["success"] = "Ticket Created";

                return RedirectToAction("SubmitTicket");
            }
            return View(model);
        }

        [Route("SoftwareTicket")]
        [HttpPost]
        public IActionResult SoftwareTicket(SoftwareTicketModel model)
        {
            if (ModelState.IsValid)
            {
                SendTicket(model);

                TempData["success"] = "Ticket Created";

                return RedirectToAction("SubmitTicket");
            }
            return View(model);
        }

        

        [Route("NewUserTicket")]
        [HttpPost]
        public IActionResult NewUserTicket(NewUserTicketModel model)
        {
            if (ModelState.IsValid)
            {
                SendTicket(model);

                TempData["success"] = "Ticket Created";

                return RedirectToAction("SubmitTicket");
            }
            return View(model);
        }

        private static SmtpClient SetCreds()
        {
            return new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                        "email...",
                        "pw..."),
                EnableSsl = true
            };
        }

        private void SendTicket(FuelmasterKeyTicketModel model)
        {
            string userName = User.Identity!.Name!.Split("\\").Last();
            var smtpClient = SetCreds();

            smtpClient.Send(
                // account
                "email...",
                "email...",

                // subject
                "Subject: Fuelmaster Key Ticket" +
                ", Sender: " + userName +
                ", Category: Fuelmaster",

                // body
                // name of person to receive new key
                "Name of person to receive new key: " + model.EmployeeFirstName +
                ", " + model.EmployeeLastName +

                // dept of person to receive new key
                ", Department of person to receive new key: " + model.Department +
                // vehicle id
                ", Vehicle ID: " + model.VehicleID +
                // current odometer
                ", Current Odometer : " + model.CurrentOdometer +
                // error message on pump
                ", Error Message on Pump: " + model.PumpErrorMessage +
                // location of fuel pump
                ", Location of Fuel Pump: " + model.FuelPumpLocation +
                // type of fuel
                ", Type of Fuel: " + model.FuelType //+
                                                    //model.Body
                );
        }

        private void SendTicket(DesktopTicketModel model)
        {
            string userName = User.Identity!.Name!.Split("\\").Last();
            var smtpClient = SetCreds();

            smtpClient.Send(
                // account
                "email...",
                "email...",

                // subject
                "Sender: " + userName +
                ", Subject: " + model.Subject +
                ", Ticket Type: " + model.GetType(),

                    // body
                    model.Body);
        }

        private void SendTicket(HardwareTicketModel model)
        {
            string userName = User.Identity!.Name!.Split("\\").Last();
            var smtpClient = SetCreds();

            smtpClient.Send(
                // account
                "email...",
                "email...",

                // subject
                "Sender: " + userName +
                ", Subject: " + model.Subject +
                ", Ticket Type: " + model.GetType(),

                    // body
                    model.Body);
        }

        private void SendTicket(SoftwareTicketModel model)
        {
            string userName = User.Identity!.Name!.Split("\\").Last();
            var smtpClient = SetCreds();

            smtpClient.Send(
                // account
                "email...",
                "email...",

                // subject
                "Sender: " + userName +
                ", Subject: " + model.Subject +
                ", Ticket Type: " + model.GetType(),

                    // body
                    model.Body);
        }

        private void SendTicket(NewUserTicketModel model)
        {
            string userName = User.Identity!.Name!.Split("\\").Last();
            var smtpClient = SetCreds();

            smtpClient.Send(
                // account
                "email...",
                "email...",

                // subject
                "Sender: " + userName +
                ", Subject: " + model.Subject +
                ", Ticket Type: " + model.GetType(),

                // body
                "New UserName: " + model.UserName + ", " +
                "Department: " + model.Department + ", " +
                model.Body);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}