using System;
using System.Collections.Generic;

namespace MTAIntranetAngular.API.Data.Models;

public partial class HdImpact
{
    public long Id { get; set; }

    public long HdQueueId { get; set; }

    public long Ordinal { get; set; }

    public string Name { get; set; } = null!;
}
