namespace MTAIntranetMVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;

    public class SoftwareTicketModel : TicketModel
    {
        //[Required]
        //[Range(DateTime.Now.AddHours(1), DateTime.MaxValue)]
        public DateTime? EarliestAppointment { get; set; }

        public string? Item { get; set; }

        public IEnumerable<SelectListItem>? ItemList { get; set; }

        public IEnumerable<string>? TicketInfo { get; set; }

        public SoftwareTicketModel()
        {
            //ItemList = PopulateItems();
        }

        public SoftwareTicketModel(string? subject, string? body, string? item)
        {
            Item = item;
            Subject = subject;
            Body = body;
        }
    }
}