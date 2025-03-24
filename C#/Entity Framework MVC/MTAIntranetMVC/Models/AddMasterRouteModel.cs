namespace MTAIntranetMVC.Models
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using MTAIntranet.MVC.Models;
    using MTAIntranet.Shared;

    public class AddMasterRouteModel
    {
        public string? Error { get; set; }

        public AddMasterRouteModel(
            string? error)
        {
            Error = error;
        }

        [Key]
        public int? PK_Route_ID { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? mode { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? dow { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? route { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? run { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? suffix { get; set; }

        [Column(TypeName = "nvarchar (255")]
        [StringLength(255)]
        public string? route_name { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? pull_out_time { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? pull_in_time { get; set; }

        [Column(TypeName = "float")]
        public float? beg_dh_miles { get; set; }

        [Column(TypeName = "float")]
        public float? end_dh_miles { get; set; }
    }
}