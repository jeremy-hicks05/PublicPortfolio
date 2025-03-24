using System;
using System.Collections.Generic;

namespace MTAIntranetAngular.API.Data.Models;

public partial class HdPriority
{
    public long Id { get; set; }

    public long HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public long Ordinal { get; set; }

    public string Color { get; set; } = null!;

    public long? EscalationMinutes { get; set; }

    public sbyte? UseBusinessHoursForEscalation { get; set; }

    public bool? IsSlaEnabled { get; set; }

    public long? ResolutionDueDateMinutes { get; set; }

    public sbyte? UseBusinessHoursForSla { get; set; }

    public long? SlaNotificationRecurrenceMinutes { get; set; }
}
