using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

public partial class ApprovalStateDTO
{
    public int ApprovalStateId { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    //[InverseProperty("ApprovalState")]
    //public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
