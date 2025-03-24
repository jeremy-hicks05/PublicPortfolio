using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

[Table("TicketSubType")]
public partial class TicketSubType
{
    [Key]
    [Column("TicketSubTypeID")]
    public int TicketSubTypeId { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [StringLength(3)]
    [Unicode(false)]
    public string NeedsApproval { get; set; } = null!;

    [Column("CCList")]
    [StringLength(255)]
    [Unicode(false)]
    public string Cclist { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("TicketSubTypes")]
    public virtual Category? Category { get; set; } = null!;

    [InverseProperty("SubType")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
