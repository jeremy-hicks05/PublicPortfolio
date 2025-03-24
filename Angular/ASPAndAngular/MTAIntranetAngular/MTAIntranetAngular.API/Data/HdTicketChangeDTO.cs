using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTAIntranetAngular.API.Data.Models;

public partial class HdTicketChangeDTO
{
    public long Id { get; set; }

    public long? HdTicketId { get; set; }

    public string? Timestamp { get; set; }

    public long UserId { get; set; }

    public string? Comment { get; set; }

    public string? CommentLoc { get; set; }

    public string? Description { get; set; }

    public string? OwnersOnlyDescription { get; set; }

    public string? LocalizedDescription { get; set; }

    public string? LocalizedOwnersOnlyDescription { get; set; }

    //public sbyte Mailed { get; set; }

    //public DateTime? MailedTimestamp { get; set; }

    public int MailerSession { get; set; }

    public string? NotifyUsers { get; set; }

    public string ViaEmail { get; set; } = null!;

    public bool OwnersOnly { get; set; }

    public bool ResolutionChanged { get; set; }

    //public sbyte SystemComment { get; set; }

    //public sbyte TicketDataChange { get; set; }

    //public sbyte ViaScheduledProcess { get; set; }

    //public sbyte ViaImport { get; set; }

    //public sbyte ViaBulkUpdate { get; set; }

    public string? UserName {  get; set; }
}
