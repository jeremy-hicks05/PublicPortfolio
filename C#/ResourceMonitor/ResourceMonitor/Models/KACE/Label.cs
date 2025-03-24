using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models.KACE;

public partial class Label
{
    public long Id { get; set; }

    public DateTime Modified { get; set; }

    public DateTime Created { get; set; }

    public string? Notes { get; set; }

    public string Name { get; set; } = null!;

    public string? KaceAltLocation { get; set; }

    public string? KaceAltLocationUser { get; set; }

    public byte[]? KaceAltLocationPasswordEnc { get; set; }

    public string? Type { get; set; }

    public sbyte UsageNode { get; set; }

    public sbyte UsageAll { get; set; }

    public sbyte UsageDell { get; set; }

    public sbyte UsageLabel { get; set; }

    public sbyte UsageUser { get; set; }

    public sbyte UsageProcess { get; set; }

    public sbyte UsagePatch { get; set; }

    public sbyte UsageSoftware { get; set; }

    public sbyte UsageMachine { get; set; }

    public sbyte MeterEnabled { get; set; }

    public sbyte UsageCatalog { get; set; }

    public sbyte AppCtrlEnabled { get; set; }

    public long ScopeUserRoleId { get; set; }
}
