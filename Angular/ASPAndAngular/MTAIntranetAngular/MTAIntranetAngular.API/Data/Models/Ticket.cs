using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

[Table("Ticket")]
public partial class Ticket
{
    [Key]
    [Column("TicketID")]
    public int TicketId { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [Column("SubTypeID")]
    public int SubTypeId { get; set; }

    [Column("ImpactID")]
    public int ImpactId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Summary { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? ReasonForRejection { get; set; }

    [Column("ApprovalStateID")]
    public int ApprovalStateId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ApprovedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateEntered { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateLastUpdated { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string EnteredByUser { get; set; } = null!;

    [ForeignKey("ApprovalStateId")]
    [InverseProperty("Tickets")]
    public virtual ApprovalState? ApprovalState { get; set; } = null!;

    [ForeignKey("CategoryId")]
    [InverseProperty("Tickets")]
    public virtual Category? Category { get; set; } = null!;

    [ForeignKey("ImpactId")]
    [InverseProperty("Tickets")]
    public virtual Impact? Impact { get; set; } = null!;

    [ForeignKey("SubTypeId")]
    [InverseProperty("Tickets")]
    public virtual TicketSubType? SubType { get; set; } = null!;
}
