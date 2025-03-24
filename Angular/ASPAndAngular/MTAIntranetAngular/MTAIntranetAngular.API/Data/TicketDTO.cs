using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTAIntranetAngular.API.Data.Models;

public partial class TicketDTO
{
    public int TicketId { get; set; }

    public int CategoryId { get; set; }

    public int SubTypeId { get; set; }

    public int ImpactId { get; set; }

    public int DepartmentId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string Summary { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? ReasonForRejection { get; set; }

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

    public string CategoryName { get; set; } = null!;

    public string SubTypeName { get; set; } = null!;

    public string ImpactName { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public string ApprovalStateName { get; set; } = null!;
}
