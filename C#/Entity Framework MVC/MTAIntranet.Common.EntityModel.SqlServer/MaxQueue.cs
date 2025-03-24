using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAIntranet.Shared
{
    public partial class MaxQueue
    {
        [Key]
        public int row_id { get; set; }

        public DateTime X_datetime_insert { get; set; }

        [Column(TypeName = "varchar (50)")]
        [MaxLength(50)]
        public string? X_userid_insert { get; set; }

        [Column(TypeName = "varchar (100)")]
        [MaxLength(100)]
        public string? ConnectorName { get; set; }

        [Column(TypeName = "varchar (2000)")]
        [MaxLength(2000)]
        public string? ErrorMessage { get; set; }

        [Column(TypeName = "text")]
        public string? ColumnList { get; set; }

    }
}
