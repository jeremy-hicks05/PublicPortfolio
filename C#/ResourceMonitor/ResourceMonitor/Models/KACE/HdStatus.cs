using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models.KACE;

public partial class HdStatus
{
    public long Id { get; set; }

    public long HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public long Ordinal { get; set; }

    public string State { get; set; } = null!;
}
