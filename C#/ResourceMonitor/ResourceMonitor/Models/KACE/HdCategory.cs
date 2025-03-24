using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models.KACE;

public partial class HdCategory
{
    public long Id { get; set; }

    public long HdQueueId { get; set; }

    public string Name { get; set; } = null!;

    public long Ordinal { get; set; }

    public long? DefaultOwnerId { get; set; }

    public string CcList { get; set; } = null!;

    public bool? UserSettable { get; set; }

    public long ParentCategoryId { get; set; }
}
