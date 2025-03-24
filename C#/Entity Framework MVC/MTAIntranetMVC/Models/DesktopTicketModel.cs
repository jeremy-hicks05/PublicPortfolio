namespace MTAIntranetMVC.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using MTAIntranet.Shared;
    using System.ComponentModel.DataAnnotations;

    public class DesktopTicketModel : TicketModel, IValidatableObject
    {
        //[Required]
        //[Range(DateTime.Now.AddHours(1), DateTime.MaxValue)]
        public DateTime? EarliestAppointment { get; set; }

        public string? Item { get; set; }

        public IEnumerable<SelectListItem>? ItemList { get; set; }

        public IEnumerable<string>? TicketInfo { get; set; }

        public DesktopTicketModel()
        {
            //EarliestAppointment = DateTime.Now;
            //ItemList = PopulateItems();
        }

        public DesktopTicketModel(string? subject, string? body, string? item)
        {
            Item = item;
            Subject = subject;
            Body = body;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if(EarliestAppointment.HasValue && EarliestAppointment.Value < DateTime.Now.AddHours(1))
            {
                yield return new ValidationResult("Appointment must be at least one hour in the future", new[] { nameof(EarliestAppointment)});
            }
            //return ValidationResult.Success;
        }
    }
}