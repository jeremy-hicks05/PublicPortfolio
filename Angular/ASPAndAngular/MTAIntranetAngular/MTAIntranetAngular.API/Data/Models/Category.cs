using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<TicketSubType> TicketSubTypes { get; set; } = new List<TicketSubType>();

    [InverseProperty("Category")]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
