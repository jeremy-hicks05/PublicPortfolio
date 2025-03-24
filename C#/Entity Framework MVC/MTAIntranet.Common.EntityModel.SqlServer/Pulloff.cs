using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTAIntranet.Shared
{
    [Index(nameof(Route_Name), Name = "Route_Name")]
    public partial class Pulloff
    {
        [Key]
        public int? PulloffID { get; set; }

        [Column(TypeName = "varchar (50")]
        [StringLength(50)]
        public string? Route_Name { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Mode { get; set; }

        [Column(TypeName = "varchar (2)")]
        [StringLength(2)]
        public string? DoW { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Route { get; set; }

        // Try TIME (in SQL) and TimeOnly in C#
        [Column(TypeName = "datetime")]
        public DateTime? PulloffTime { get; set; }

        // Try TIME (in SQL) and TimeOnly in C#
        [Column(TypeName = "datetime")]
        public DateTime? PulloffReturn { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Run { get; set; }

        [Column(TypeName = "varchar (4)")]
        [StringLength(4)]
        public string? Suffix { get; set; }

        public string GetSignature()
        {
            return (Route_Name + Mode + DoW + Route + Run + Suffix).Replace(" ", "");
        }

        public string GetUniqueSignature()
        {
            return (Route_Name + Mode + Route + Run + Suffix + PulloffTime?.Month + PulloffTime?.Day).Replace(" ", "");
        }
    }
}
