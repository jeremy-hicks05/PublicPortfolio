using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTAIntranet.MVC.Models
{
    public class PulloffModel : IValidatableObject
    {
        //[Key]
        //public int? PulloffID { get; set; }

        [Required]
        [Column(TypeName = "varchar (50")]
        [StringLength(50)]
        public string? Route_Name { get; set; }

        [Required]
        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Mode { get; set; }

        [Required]
        [Column(TypeName = "varchar (2)")]
        [StringLength(2)]
        public string? DoW { get; set; }

        [Required]
        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Route { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? PulloffTime { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime? PulloffReturn { get; set; }

        [Required]
        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Run { get; set; }

        [Required]
        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Suffix { get; set; }

        [Required]
        public DateTime? RouteStart { get; set; }

        [Required]
        public DateTime? RouteEnd { get; set; }

        [Required]
        public int Year { get; set; }

        public string GetSignature()
        {
            return (Route_Name + Mode + DoW + Route + Run + Suffix).Replace(" ", "");
        }

        public string GetUniqueSignature()
        {
            return (Route_Name + Mode + Route + Run + Suffix + PulloffTime?.Month + PulloffTime?.Day).Replace(" ", "");
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // if route ends on the same day it begins
            if (RouteStart.HasValue && RouteEnd.HasValue &&
                RouteStart.Value.Day == RouteEnd.Value.Day)
            {
                // Pulloff time must be after the start of the route
                if (RouteStart is not null &&
                PulloffTime.HasValue &&
                PulloffTime.Value.Hour < RouteStart.Value.Hour)
                {
                    yield return new ValidationResult("Pulloff time must be after the start of the route", new[] { nameof(PulloffTime) });
                }
                if (RouteStart is not null && PulloffTime.HasValue &&
                    PulloffTime.Value.Hour == RouteStart.Value.Hour &&
                        PulloffTime.Value.Minute < RouteStart.Value.Minute)
                {
                    yield return new ValidationResult("Pulloff time must be after the start of the route", new[] { nameof(PulloffTime) });
                }

                // Pulloff time must be before the end of the route
                if (RouteEnd is not null &&
                    PulloffTime.HasValue &&
                    PulloffTime.Value.Hour > RouteEnd.Value.Hour)
                {
                    yield return new ValidationResult("Pulloff time must be before the end of the route", new[] { nameof(PulloffTime) });
                }
                if (RouteEnd is not null && PulloffTime.HasValue &&
                    PulloffTime.Value.Hour == RouteEnd.Value.Hour &&
                        PulloffTime.Value.Minute > RouteEnd.Value.Minute)
                {
                    yield return new ValidationResult("Pulloff time must be before the end of the route", new[] { nameof(PulloffTime) });
                }

                // pulloff return must be later than pulloff time
                if (PulloffReturn < PulloffTime)
                {
                    yield return new ValidationResult("Pulloff return must be after pulloff time", new[] { nameof(PulloffTime), nameof(PulloffReturn) });
                }

                if (PulloffTime.HasValue && DoW is not null &&
                    (!Utility.Utility.MatchDoW(PulloffTime.Value, DoW)))
                {
                    yield return new ValidationResult($"Pulloff day must match route day, which is on a {Utility.Utility.DoWToName(DoW)}", new[] { nameof(PulloffTime), nameof(PulloffReturn) });
                }
            }
            else
            {
                // TODO:
                // need to implement code for dealing with route that spill over
                // into next day
                yield return new ValidationResult($"Unable to offer pulloff entry for route that spans across days", new[] { nameof(PulloffTime) });
            }
        }
    }
}
