using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAIntranet.Shared
{
    public partial class ProductConfig
    {
        [Key]
        [Column(TypeName = "smallint")]
        public int PRODUCT { get; set; }

        [Column(TypeName = "varchar (50)")]
        public string? DESCRIPT { get; set; }
    }
}
