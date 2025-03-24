using System;
using System.Collections.Generic;

namespace MTAIntranetAngular.API.Data.Models;

/// <summary>
/// This table holds all of the custom fields for the Service Desk
/// </summary>
public partial class HdCustomField
{
    public int Id { get; set; }

    /// <summary>
    /// The queue_id this custom field belongs to.
    /// </summary>
    public int HdQueueId { get; set; }

    /// <summary>
    /// The name of the custom field.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The type of the custom field.
    /// </summary>
    public string Type { get; set; } = null!;

    /// <summary>
    /// The value of the custom field.
    /// </summary>
    public string? Values { get; set; }

    /// <summary>
    /// The default value of the custom field.
    /// </summary>
    public string? Default { get; set; }

    public bool OwnersOnly { get; set; }

    public bool OwnersOnlyRead { get; set; }
}
