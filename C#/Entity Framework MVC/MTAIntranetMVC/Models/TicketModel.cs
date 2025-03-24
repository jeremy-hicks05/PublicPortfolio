namespace MTAIntranetMVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;

    public class TicketModel
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string? Subject { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string? Body { get; set; }
        public string? Sender { get; set; }

        public string? TicketType { get; set; }

        public IEnumerable<SelectListItem>? TicketTypeList { get; set; }

        public TicketModel()
        {
            //TicketTypeList = PopulateItems();
        }

        public TicketModel(string? subject, string? body, string? type)
        {
            TicketType = type;
            Subject = subject;
            Body = body;
        }

        //protected IEnumerable<SelectListItem> PopulateItems()
        //{
        //    List<SelectListItem> tempList = new List<SelectListItem>
        //    {
        //        new SelectListItem
        //        {
        //            Text = "Desktop",
        //            Value = "Desktop"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Hardware",
        //            Value = "Hardware"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Software",
        //            Value = "Software"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "EAM",
        //            Value = "EAM"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "UltiPro",
        //            Value = "UltiPro"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Solomon",
        //            Value = "Solomon"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "KVD",
        //            Value = "KVD"
        //        },
        //        new SelectListItem
        //        {
        //            Text = "Tablet",
        //            Value = "Tablet"
        //        }
        //    };
        //    return tempList;
        //}
    }
}