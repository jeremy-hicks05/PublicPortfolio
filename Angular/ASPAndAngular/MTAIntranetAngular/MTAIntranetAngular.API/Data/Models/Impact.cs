using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

[Table("Impact")]
public partial class Impact
{
    [Key]
    [Column("ImpactID")]
    public int ImpactId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [InverseProperty("Impact")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
