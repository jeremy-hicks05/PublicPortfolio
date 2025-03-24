namespace MTAIntranet.MVC.Models
{
    public class SubmitTicketModel
    {
        // contains all the ticket types
        public List<string> TicketTypes { get; set; } = new List<string>()
        {
            "Desktop",
            "Hardware",
            "Software",
            "NewUser",
            "FuelmasterKey"
        };
    }
}
