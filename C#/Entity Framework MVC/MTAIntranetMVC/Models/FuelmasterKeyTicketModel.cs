using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.FileIO;
using MTAIntranet.Shared;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace MTAIntranetMVC.Models
{
    public class FuelmasterKeyTicketModel : TicketModel
    {
        private readonly FuelmasterContext _fuelmasterContext;
        public string? EmployeeNum { get; set; }
        public IEnumerable<SelectListItem>? EmployeeNums { get; set; }

        public MainTrans? LastTransaction { get; set; }
        public IEnumerable<MainTrans>? Transactions { get; set; }
        public string? EmployeeFirstName { get; set; }
        public string? EmployeeLastName { get; set; }
        public string? Department { get; set; }
        public int VehicleID { get; set; }
        public IEnumerable<SelectListItem>? VehicleIDs { get; set; }
        public int CurrentOdometer { get; set; }
        public string? PumpErrorMessage { get; set; }
        public IEnumerable<SelectListItem>? PumpErrorMessages { get; set; }
        public string? FuelPumpLocation { get; set; }
        public string? FuelType { get; set; } // unleaded, propane, cng
        public IEnumerable<SelectListItem>? FuelTypes { get; set; }

        public IEnumerable<SelectListItem>? Locations { get; set; }

        public FuelmasterKeyTicketModel(FuelmasterContext fuelmasterContext)
        {
            _fuelmasterContext = fuelmasterContext;
            Body = "Test body";
            Subject = "Test subject";
            if (_fuelmasterContext.User is not null)
            {
                EmployeeNums = _fuelmasterContext.User
                    .Where(u => 
                     u.USERID != null)
                    .Select(u => new SelectListItem
                {
                    Text = u.USERID!.ToString().TrimStart('0').PadLeft(4, '0'),
                    Value = u.USERID.ToString()
                });
            }

            //Transactions = _fuelmasterContext.MainTrans.Where(
            //    mt => mt.USERID == 
            //    );
        }
    }
}