using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTAIntranet.Shared
{
    [Index(nameof(USERID), Name = "USERID")]
    public partial class User
    {
        [Column(TypeName = "varchar (9)")]
        [StringLength(9)]
        public string? USERID { get; set; }

        [Column(TypeName = "varchar (20)")]
        [StringLength(20)]
        public string? LNAME { get; set; }

        [Column(TypeName = "varchar (20)")]
        [StringLength(20)]
        public string? FNAME { get; set; }

        //[NotMapped]
        //public MainTrans? MainTrans { get; set; }
    }
}
