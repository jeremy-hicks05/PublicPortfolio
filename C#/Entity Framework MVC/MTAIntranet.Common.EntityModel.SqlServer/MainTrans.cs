using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTAIntranet.Shared
{
    [Index(nameof(AUTONUM), Name = "AUTONUM")]
    public partial class MainTrans
    {
        //[Key]
        [Column(TypeName = "int")]
        public int AUTONUM { get; set; }

        [Column(TypeName = "varchar (2)")]
        [StringLength(2)]
        public string? TC { get; set; }

        [Column(TypeName = "nvarchar (4)")]
        [StringLength(4)]
        public string? SITEID { get; set; }

        [Column(TypeName = "nvarchar (8)")]
        [StringLength(8)]
        public string? VEHICLEID { get; set; }

        [Column(TypeName = "smallint")]
        public int? PRODUCT { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TRANTIME { get; set; }

        [Column(TypeName = "float")]
        public float? QUANTITY { get; set; }

        [Column(TypeName = "int")]
        public int? ODOMETER { get; set; }

        [Column(TypeName = "varchar(9)")]
        [StringLength(9)]
        public string? USERID { get; set; }

        [NotMapped]
        public User? User { get; set; }
    }
}
