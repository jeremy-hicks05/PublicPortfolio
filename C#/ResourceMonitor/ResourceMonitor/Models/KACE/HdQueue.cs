using System;
using System.Collections.Generic;

namespace ResourceMonitor.Models.KACE;

public partial class HdQueue
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? EmailUser { get; set; }

    public string? PopUsername { get; set; }

    public byte[]? PopPasswordEnc { get; set; }

    public sbyte PopSsl { get; set; }

    public long? DefaultPriorityId { get; set; }

    public long? DefaultStatusId { get; set; }

    public long? DefaultImpactId { get; set; }

    public long? DefaultCategoryId { get; set; }

    public sbyte CreateUsersOnEmail { get; set; }

    public string? AltEmailAddr { get; set; }

    public sbyte AllowAllUsers { get; set; }

    public sbyte AllowDelete { get; set; }

    public sbyte AllowParentClose { get; set; }

    public sbyte AllowAllApprovers { get; set; }

    public sbyte AllowOwnersViaAdminui { get; set; }

    public string? ArchiveInterval { get; set; }

    public string? PurgeInterval { get; set; }

    public string PopServer { get; set; } = null!;

    public string SmtpServer { get; set; } = null!;

    public string SmtpUsername { get; set; } = null!;

    public byte[]? SmtpPasswordEnc { get; set; }

    public long SmtpPort { get; set; }

    public sbyte OwnersOnlyComments { get; set; }

    public sbyte ConflictWarningEnabled { get; set; }

    public sbyte ShowNewTicketComments { get; set; }

    public sbyte ShowNewTicketAttachments { get; set; }

    public sbyte AllowManagerCommentViaUserui { get; set; }

    public sbyte AutoAddCclistOnComment { get; set; }

    public sbyte AllowUsersEditOwnComment { get; set; }

    public sbyte AllowOwnersEditAllComment { get; set; }

    public sbyte AllowAggressiveHtmlSanitization { get; set; }

    public string TicketAttachmentRestriction { get; set; } = null!;

    public string LimitedAttachmentList { get; set; } = null!;

    public sbyte AllowNoExtFileAttachment { get; set; }

    public sbyte SendImagesAsAttachment { get; set; }

    public long MaxImagesAttachmentSize { get; set; }

    public sbyte PopEnabled { get; set; }

    public sbyte ImapEnabled { get; set; }

    public string? ImapUsername { get; set; }

    public byte[]? ImapPasswordEnc { get; set; }

    public string? ImapServer { get; set; }

    public sbyte ImapSsl { get; set; }

    public sbyte GmailEnabled { get; set; }

    public sbyte Office365Enabled { get; set; }

    public sbyte SmtpInboundEnabled { get; set; }

    public long GmailCredentialId { get; set; }

    public long Office365CredentialId { get; set; }

    public string TicketPrefix { get; set; } = null!;

    public sbyte Office365GraphServiceId { get; set; }

    public sbyte AllowKbSuggestions { get; set; }

    public sbyte AllowLastChildCloseParent { get; set; }

    public sbyte SendFilesAsAttachment { get; set; }

    public long MaxFilesAttachmentSize { get; set; }

    public sbyte UnsetDefaultPriority { get; set; }

    public sbyte UnsetDefaultStatus { get; set; }

    public sbyte UnsetDefaultImpact { get; set; }

    public sbyte UnsetDefaultCategory { get; set; }
}
