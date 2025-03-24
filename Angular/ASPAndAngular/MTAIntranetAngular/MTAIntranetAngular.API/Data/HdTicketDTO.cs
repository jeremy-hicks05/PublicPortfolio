using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTAIntranetAngular.API.Data.Models;

public partial class HdTicketDTO
{
    public long Id { get; set; }
    public string? Title {  get; set; }
    public string? Created {  get; set; }
    public DateTime TimeOpened {  get; set; }
    public DateTime TimeStalled {  get; set; }
    public string Summary { get; set; } = null!;
    public long HdPriorityId { get; set; }
    public long HdImpactId { get; set; }
    public long OwnerId { get; set; }
    public long SubmitterId {  get; set; }
    public long HdStatusId {  get; set; }
    public long HdQueueId {  get; set; }

    public long ApproverId { get; set; }
    public string? ApproveState { get; set; }
    public string? Approval { get; set; }
    public string? ApprovalNote { get; set; }
    public string? ApproverName {  get; set; }

    public long HdCategoryId { get; set; }
    public string PriorityName { get; set; } = null!;
    public string ImpactName { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public string SubmitterName { get; set; } = null!;
    public string StatusName { get; set; } = null!;
    public string QueueName { get; set; } = null!;
    public string CategoryName { get; set; } = null!;

    //public string? CustomFieldValue0 { get; set; }

    //public string? CustomFieldValue1 { get; set; }

    //public string? CustomFieldValue2 { get; set; }

    //public string? CustomFieldValue3 { get; set; }

    //public string? CustomFieldValue4 { get; set; }

    //public string? CustomFieldValue5 { get; set; }

    //public string? CustomFieldValue6 { get; set; }

    //public string? CustomFieldValue7 { get; set; }

    //public string? CustomFieldValue8 { get; set; }

    //public string? CustomFieldValue9 { get; set; }

    //public string? CustomFieldValue10 { get; set; }

    //public string? CustomFieldValue11 { get; set; }

    //public string? CustomFieldValue12 { get; set; }

    //public string? CustomFieldValue13 { get; set; }

    //public string? CustomFieldValue14 { get; set; }

}
