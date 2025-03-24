using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

[Table("ApprovalState")]
public partial class ApprovalState
{
    [Key]
    [Column("ApprovalStateID")]
    public int ApprovalStateId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("ApprovalState")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
