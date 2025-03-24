using System;
using System.Collections.Generic;

namespace ResourceMonitor.ModelsTest;

public partial class HdField
{
    public long Id { get; set; }

    public long HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public string? HdTicketFieldName { get; set; }

    public long Ordinal { get; set; }

    public string RequiredState { get; set; } = null!;

    public string FieldLabel { get; set; } = null!;

    public string Visible { get; set; } = null!;
}
