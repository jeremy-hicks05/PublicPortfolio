using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models.KACE;

public partial class HdTicketViewModel
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public HdPriority? HdPriority { get; set; }

    public HdImpact? HdImpact { get; set; }

    public DateTime Modified { get; set; }

    public DateTime Created { get; set; }

    public User? Owner { get; set; }

    public User? Submitter { get; set; }

    public HdStatus? HdStatus { get; set; }

    public HdQueue? HdQueue { get; set; }

    public HdCategory? HdCategory { get; set; }

    public string CcList { get; set; } = null!;

    public DateTime Escalated { get; set; }

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

    public DateTime? DueDate { get; set; }

    public bool IsManualDueDate { get; set; }

    public DateTime SlaNotified { get; set; }

    public DateTime TimeOpened { get; set; }

    public DateTime TimeClosed { get; set; }

    //public DateTime TimeStalled { get; set; }

    //public long? MachineId { get; set; }

    //public int? SatisfactionRating { get; set; }

    //public string? SatisfactionComment { get; set; }

    //public string? Resolution { get; set; }

    //public long AssetId { get; set; }

    //public long ParentId { get; set; }

    //public bool? IsParent { get; set; }

    //public long ApproverId { get; set; }

    //public string? ApproveState { get; set; }

    //public string? Approval { get; set; }

    //public string? ApprovalNote { get; set; }

    //public long ServiceTicketId { get; set; }

    //public long? HdServiceStatusId { get; set; }

    //public sbyte? HdUseProcessStatus { get; set; }

    //public string? HtmlSummary { get; set; }

    //public long TicketTemplateId { get; set; }

    //public string? EmailMessageId { get; set; }
    //public User? LastCommenter { get; set; }
}
