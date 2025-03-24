namespace MTAIntranetMVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;

    public class NewUserTicketModel : TicketModel
    {
        //[Required]
        //[Range(DateTime.Now.AddHours(1), DateTime.MaxValue)]
        public DateTime? EarliestAppointment { get; set; }

        public string? Item { get; set; }

        public IEnumerable<SelectListItem>? ItemList { get; set; }

        public IEnumerable<string>? TicketInfo { get; set; }

        [Display(Name ="New UserName")]
        public string? UserName { get; set; }
        public string? Department { get; set; }
        public string? MatchingUserRights { get; set; }

        public NewUserTicketModel()
        {
            //ItemList = PopulateItems();
        }

        public NewUserTicketModel(string? subject, string? body, string? item)
        {
            Item = item;
            Subject = subject;
            Body = body;
        }
    }
}