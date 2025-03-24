using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAIntranet.Shared
{
    public partial class Site
    {
        [Key]
        [Column(TypeName = "varchar (4)")]
        public string? SITEID { get; set; }

        [Column(TypeName = "varchar (40)")]
        public string? SITENAME { get; set; }
    }
}
