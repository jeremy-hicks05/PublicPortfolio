using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MTAIntranetAngular.API.Data.Models;

public partial class TicketSubTypeDTO
{
    public int TicketSubTypeId { get; set; }

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

    [StringLength(255)]
    [Unicode(false)]
    public string Cclist { get; set; } = null!;

    public string CategoryName { get; set; } = null!;
}
