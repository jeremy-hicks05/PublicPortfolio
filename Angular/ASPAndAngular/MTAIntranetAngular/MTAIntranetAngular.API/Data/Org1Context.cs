using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Data.Models;

public partial class Org1Context : DbContext
{
    public Org1Context()
    {
    }

    public Org1Context(DbContextOptions<Org1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<HdCategory> HdCategories { get; set; }

    public virtual DbSet<HdCustomField> HdCustomFields { get; set; }

    public virtual DbSet<HdField> HdFields { get; set; }

    public virtual DbSet<HdImpact> HdImpacts { get; set; }

    public virtual DbSet<HdPriority> HdPriorities { get; set; }

    public virtual DbSet<HdQueue> HdQueues { get; set; }

    public virtual DbSet<HdQueueApproverLabelJt> HdQueueApproverLabelJts { get; set; }

    public virtual DbSet<HdQueueSubmitterLabelJt> HdQueueSubmitterLabelJts { get; set; }

    public virtual DbSet<HdStatus> HdStatuses { get; set; }

    public virtual DbSet<HdTicket> HdTickets { get; set; }

    public virtual DbSet<HdTicketChange> HdTicketChanges { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLabelJt> UserLabelJts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Data Source=192.168.122.33;port=3306;database=ORG1;uid=R1;password=pw...;Convert Zero Datetime=True;TreatTinyAsBoolean=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HdCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_CATEGORY");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.HasIndex(e => new { e.HdQueueId, e.ParentCategoryId }, "HD_QUEUE_PARENT_IDX");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.CcList)
                .HasDefaultValueSql("''''''")
                .HasColumnType("text")
                .HasColumnName("CC_LIST");
            entity.Property(e => e.DefaultOwnerId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEFAULT_OWNER_ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.ParentCategoryId)
                .HasColumnType("bigint(20)")
                .HasColumnName("PARENT_CATEGORY_ID");
            entity.Property(e => e.UserSettable)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("USER_SETTABLE");
        });

        modelBuilder.Entity<HdCustomField>(entity =>
        {
            entity.HasKey(e => new { e.HdQueueId, e.Id }).HasName("PRIMARY");

            entity.ToTable("HD_CUSTOM_FIELDS", tb => tb.HasComment("This table holds all of the custom fields for the Service Desk"));

            entity.HasIndex(e => new { e.HdQueueId, e.Name }, "NAME_UNIQUE").IsUnique();

            entity.Property(e => e.HdQueueId)
                .HasComment("The queue_id this custom field belongs to.")
                .HasColumnType("int(11)")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Default)
                .HasDefaultValueSql("''''''")
                .HasComment("The default value of the custom field.")
                .HasColumnType("text")
                .HasColumnName("DEFAULT");
            entity.Property(e => e.Name)
                .HasComment("The name of the custom field.")
                .HasColumnName("NAME");
            entity.Property(e => e.OwnersOnly).HasColumnName("OWNERS_ONLY");
            entity.Property(e => e.OwnersOnlyRead).HasColumnName("OWNERS_ONLY_READ");
            entity.Property(e => e.Type)
                .HasComment("The type of the custom field.")
                .HasColumnType("text")
                .HasColumnName("TYPE");
            entity.Property(e => e.Values)
                .HasDefaultValueSql("''''''")
                .HasComment("The value of the custom field.")
                .HasColumnType("text")
                .HasColumnName("VALUES");
        });

        modelBuilder.Entity<HdField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_FIELD");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.FieldLabel)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("FIELD_LABEL");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.HdTicketFieldName)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("HD_TICKET_FIELD_NAME");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.RequiredState)
                .HasDefaultValueSql("'''none'''")
                .HasColumnType("enum('none','all','opened','closed','stalled')")
                .HasColumnName("REQUIRED_STATE");
            entity.Property(e => e.Visible)
                .HasMaxLength(20)
                .HasDefaultValueSql("'''hidden'''")
                .HasColumnName("VISIBLE");
        });

        modelBuilder.Entity<HdImpact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_IMPACT");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.HasIndex(e => new { e.Name, e.HdQueueId }, "NAME").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
        });

        modelBuilder.Entity<HdPriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_PRIORITY");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("COLOR");
            entity.Property(e => e.EscalationMinutes)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ESCALATION_MINUTES");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.IsSlaEnabled)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_SLA_ENABLED");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.ResolutionDueDateMinutes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("RESOLUTION_DUE_DATE_MINUTES");
            entity.Property(e => e.SlaNotificationRecurrenceMinutes)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SLA_NOTIFICATION_RECURRENCE_MINUTES");
            entity.Property(e => e.UseBusinessHoursForEscalation)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USE_BUSINESS_HOURS_FOR_ESCALATION");
            entity.Property(e => e.UseBusinessHoursForSla)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USE_BUSINESS_HOURS_FOR_SLA");
        });

        modelBuilder.Entity<HdQueue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_QUEUE");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.AllowAggressiveHtmlSanitization)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_AGGRESSIVE_HTML_SANITIZATION");
            entity.Property(e => e.AllowAllApprovers)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_ALL_APPROVERS");
            entity.Property(e => e.AllowAllUsers)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_ALL_USERS");
            entity.Property(e => e.AllowDelete)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_DELETE");
            entity.Property(e => e.AllowKbSuggestions)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_KB_SUGGESTIONS");
            entity.Property(e => e.AllowLastChildCloseParent)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_LAST_CHILD_CLOSE_PARENT");
            entity.Property(e => e.AllowManagerCommentViaUserui)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_MANAGER_COMMENT_VIA_USERUI");
            entity.Property(e => e.AllowNoExtFileAttachment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_NO_EXT_FILE_ATTACHMENT");
            entity.Property(e => e.AllowOwnersEditAllComment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_OWNERS_EDIT_ALL_COMMENT");
            entity.Property(e => e.AllowOwnersViaAdminui)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_OWNERS_VIA_ADMINUI");
            entity.Property(e => e.AllowParentClose)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_PARENT_CLOSE");
            entity.Property(e => e.AllowUsersEditOwnComment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("ALLOW_USERS_EDIT_OWN_COMMENT");
            entity.Property(e => e.AltEmailAddr)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("ALT_EMAIL_ADDR");
            entity.Property(e => e.ArchiveInterval)
                .HasMaxLength(50)
                .HasDefaultValueSql("'''never'''")
                .HasColumnName("ARCHIVE_INTERVAL");
            entity.Property(e => e.AutoAddCclistOnComment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("AUTO_ADD_CCLIST_ON_COMMENT");
            entity.Property(e => e.ConflictWarningEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("CONFLICT_WARNING_ENABLED");
            entity.Property(e => e.CreateUsersOnEmail)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("CREATE_USERS_ON_EMAIL");
            entity.Property(e => e.DefaultCategoryId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEFAULT_CATEGORY_ID");
            entity.Property(e => e.DefaultImpactId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEFAULT_IMPACT_ID");
            entity.Property(e => e.DefaultPriorityId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEFAULT_PRIORITY_ID");
            entity.Property(e => e.DefaultStatusId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEFAULT_STATUS_ID");
            entity.Property(e => e.EmailUser)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("EMAIL_USER");
            entity.Property(e => e.GmailCredentialId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("GMAIL_CREDENTIAL_ID");
            entity.Property(e => e.GmailEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("GMAIL_ENABLED");
            entity.Property(e => e.ImapEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("IMAP_ENABLED");
            entity.Property(e => e.ImapPasswordEnc)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("tinyblob")
                .HasColumnName("IMAP_PASSWORD_ENC");
            entity.Property(e => e.ImapServer)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("IMAP_SERVER");
            entity.Property(e => e.ImapSsl)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("IMAP_SSL");
            entity.Property(e => e.ImapUsername)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("IMAP_USERNAME");
            entity.Property(e => e.LimitedAttachmentList)
                .HasDefaultValueSql("''''''")
                .HasColumnType("text")
                .HasColumnName("LIMITED_ATTACHMENT_LIST");
            entity.Property(e => e.MaxFilesAttachmentSize)
                .HasDefaultValueSql("'10'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MAX_FILES_ATTACHMENT_SIZE");
            entity.Property(e => e.MaxImagesAttachmentSize)
                .HasDefaultValueSql("'10'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MAX_IMAGES_ATTACHMENT_SIZE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("NAME");
            entity.Property(e => e.Office365CredentialId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("OFFICE365_CREDENTIAL_ID");
            entity.Property(e => e.Office365Enabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("OFFICE365_ENABLED");
            entity.Property(e => e.Office365GraphServiceId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(20) unsigned")
                .HasColumnName("OFFICE365_GRAPH_SERVICE_ID");
            entity.Property(e => e.OwnersOnlyComments)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("OWNERS_ONLY_COMMENTS");
            entity.Property(e => e.PopEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("POP_ENABLED");
            entity.Property(e => e.PopPasswordEnc)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("tinyblob")
                .HasColumnName("POP_PASSWORD_ENC");
            entity.Property(e => e.PopServer)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("POP_SERVER");
            entity.Property(e => e.PopSsl)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("POP_SSL");
            entity.Property(e => e.PopUsername)
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("POP_USERNAME");
            entity.Property(e => e.PurgeInterval)
                .HasMaxLength(50)
                .HasDefaultValueSql("'''never'''")
                .HasColumnName("PURGE_INTERVAL");
            entity.Property(e => e.SendFilesAsAttachment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SEND_FILES_AS_ATTACHMENT");
            entity.Property(e => e.SendImagesAsAttachment)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SEND_IMAGES_AS_ATTACHMENT");
            entity.Property(e => e.ShowNewTicketAttachments)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SHOW_NEW_TICKET_ATTACHMENTS");
            entity.Property(e => e.ShowNewTicketComments)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SHOW_NEW_TICKET_COMMENTS");
            entity.Property(e => e.SmtpInboundEnabled)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SMTP_INBOUND_ENABLED");
            entity.Property(e => e.SmtpPasswordEnc)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("tinyblob")
                .HasColumnName("SMTP_PASSWORD_ENC");
            entity.Property(e => e.SmtpPort)
                .HasColumnType("bigint(20)")
                .HasColumnName("SMTP_PORT");
            entity.Property(e => e.SmtpServer)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("SMTP_SERVER");
            entity.Property(e => e.SmtpUsername)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("SMTP_USERNAME");
            entity.Property(e => e.TicketAttachmentRestriction)
                .HasDefaultValueSql("'''none'''")
                .HasColumnType("enum('none','image','custom','all')")
                .HasColumnName("TICKET_ATTACHMENT_RESTRICTION");
            entity.Property(e => e.TicketPrefix)
                .HasMaxLength(10)
                .HasDefaultValueSql("'''TICK:'''")
                .HasColumnName("TICKET_PREFIX");
            entity.Property(e => e.UnsetDefaultCategory)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("UNSET_DEFAULT_CATEGORY");
            entity.Property(e => e.UnsetDefaultImpact)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("UNSET_DEFAULT_IMPACT");
            entity.Property(e => e.UnsetDefaultPriority)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("UNSET_DEFAULT_PRIORITY");
            entity.Property(e => e.UnsetDefaultStatus)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("UNSET_DEFAULT_STATUS");
        });

        modelBuilder.Entity<HdQueueApproverLabelJt>(entity =>
        {
            entity.HasKey(e => new { e.HdQueueId, e.LabelId }).HasName("PRIMARY");

            entity.ToTable("HD_QUEUE_APPROVER_LABEL_JT");

            entity.HasIndex(e => e.LabelId, "LABEL_IDX");

            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20)")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.LabelId)
                .HasColumnType("bigint(20)")
                .HasColumnName("LABEL_ID");
        });

        modelBuilder.Entity<HdQueueSubmitterLabelJt>(entity =>
        {
            entity.HasKey(e => new { e.HdQueueId, e.LabelId }).HasName("PRIMARY");

            entity.ToTable("HD_QUEUE_SUBMITTER_LABEL_JT");

            entity.HasIndex(e => e.LabelId, "LABEL_IDX");

            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20)")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.LabelId)
                .HasColumnType("bigint(20)")
                .HasColumnName("LABEL_ID");
        });

        modelBuilder.Entity<HdStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_STATUS");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Ordinal)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ORDINAL");
            entity.Property(e => e.State)
                .HasDefaultValueSql("'''opened'''")
                .HasColumnType("enum('opened','closed','stalled')")
                .HasColumnName("STATE");
        });

        modelBuilder.Entity<HdTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_TICKET");

            entity.HasIndex(e => e.HdCategoryId, "HD_CATEGORY_IDX");

            entity.HasIndex(e => e.HdImpactId, "HD_IMPACT_IDX");

            entity.HasIndex(e => e.HdPriorityId, "HD_PRIORITY_IDX");

            entity.HasIndex(e => e.HdQueueId, "HD_QUEUE_IDX");

            entity.HasIndex(e => e.HdStatusId, "HD_STATUS_IDX");

            entity.HasIndex(e => e.MachineId, "MACHINE_IDX");

            entity.HasIndex(e => new { e.OwnerId, e.HdStatusId }, "OWNER_STATUS");

            entity.HasIndex(e => e.ParentId, "PARENT");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Approval)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("APPROVAL");
            entity.Property(e => e.ApprovalNote)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("APPROVAL_NOTE");
            entity.Property(e => e.ApproveState)
                .HasMaxLength(20)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("APPROVE_STATE");
            entity.Property(e => e.ApproverId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("APPROVER_ID");
            entity.Property(e => e.AssetId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ASSET_ID");
            entity.Property(e => e.CcList)
                .HasColumnType("text")
                .HasColumnName("CC_LIST");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("CREATED");
            entity.Property(e => e.CustomFieldValue0)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE0");
            entity.Property(e => e.CustomFieldValue1)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE1");
            entity.Property(e => e.CustomFieldValue10)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE10");
            entity.Property(e => e.CustomFieldValue11)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE11");
            entity.Property(e => e.CustomFieldValue12)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE12");
            entity.Property(e => e.CustomFieldValue13)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE13");
            entity.Property(e => e.CustomFieldValue14)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE14");
            entity.Property(e => e.CustomFieldValue2)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE2");
            entity.Property(e => e.CustomFieldValue3)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE3");
            entity.Property(e => e.CustomFieldValue4)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE4");
            entity.Property(e => e.CustomFieldValue5)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE5");
            entity.Property(e => e.CustomFieldValue6)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE6");
            entity.Property(e => e.CustomFieldValue7)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE7");
            entity.Property(e => e.CustomFieldValue8)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE8");
            entity.Property(e => e.CustomFieldValue9)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("CUSTOM_FIELD_VALUE9");
            entity.Property(e => e.DueDate)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("DUE_DATE");
            entity.Property(e => e.EmailMessageId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("EMAIL_MESSAGE_ID");
            entity.Property(e => e.Escalated)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("ESCALATED");
            entity.Property(e => e.HdCategoryId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_CATEGORY_ID");
            entity.Property(e => e.HdImpactId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_IMPACT_ID");
            entity.Property(e => e.HdPriorityId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_PRIORITY_ID");
            entity.Property(e => e.HdQueueId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_QUEUE_ID");
            entity.Property(e => e.HdServiceStatusId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_SERVICE_STATUS_ID");
            entity.Property(e => e.HdStatusId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_STATUS_ID");
            entity.Property(e => e.HdUseProcessStatus)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("HD_USE_PROCESS_STATUS");
            entity.Property(e => e.HtmlSummary)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("HTML_SUMMARY");
            entity.Property(e => e.IsManualDueDate).HasColumnName("IS_MANUAL_DUE_DATE");
            entity.Property(e => e.IsParent)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IS_PARENT");
            entity.Property(e => e.MachineId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MACHINE_ID");
            entity.Property(e => e.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.OwnerId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("OWNER_ID");
            entity.Property(e => e.ParentId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PARENT_ID");
            entity.Property(e => e.Resolution)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("RESOLUTION");
            entity.Property(e => e.SatisfactionComment)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("SATISFACTION_COMMENT");
            entity.Property(e => e.SatisfactionRating)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("SATISFACTION_RATING");
            entity.Property(e => e.ServiceTicketId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SERVICE_TICKET_ID");
            entity.Property(e => e.SlaNotified)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("SLA_NOTIFIED");
            entity.Property(e => e.SubmitterId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SUBMITTER_ID");
            entity.Property(e => e.Summary)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("SUMMARY");
            entity.Property(e => e.TicketTemplateId)
                .HasColumnType("bigint(20)")
                .HasColumnName("TICKET_TEMPLATE_ID");
            entity.Property(e => e.TimeClosed)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("TIME_CLOSED");
            entity.Property(e => e.TimeOpened)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("TIME_OPENED");
            entity.Property(e => e.TimeStalled)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("TIME_STALLED");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<HdTicketChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("HD_TICKET_CHANGE");

            entity.HasIndex(e => e.HdTicketId, "HD_TICKET_IDX");

            entity.HasIndex(e => e.Mailed, "MAILED");

            entity.HasIndex(e => new { e.Mailed, e.MailerSession }, "MAILED_MAILER_SESSION_IDX");

            entity.HasIndex(e => e.MailerSession, "MAILER_SESSION");

            entity.HasIndex(e => e.UserId, "USER_IDX");

            entity.HasIndex(e => new { e.ViaEmail, e.Timestamp }, "VIA_EMAIL_TIMESTAMP_IDX");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Comment)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("COMMENT");
            entity.Property(e => e.CommentLoc)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("COMMENT_LOC");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.HdTicketId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_TICKET_ID");
            entity.Property(e => e.LocalizedDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("LOCALIZED_DESCRIPTION");
            entity.Property(e => e.LocalizedOwnersOnlyDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("LOCALIZED_OWNERS_ONLY_DESCRIPTION");
            entity.Property(e => e.Mailed)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("MAILED");
            entity.Property(e => e.MailedTimestamp)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime")
                .HasColumnName("MAILED_TIMESTAMP");
            entity.Property(e => e.MailerSession)
                .HasColumnType("int(11) unsigned")
                .HasColumnName("MAILER_SESSION");
            entity.Property(e => e.NotifyUsers)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("NOTIFY_USERS");
            entity.Property(e => e.OwnersOnly).HasColumnName("OWNERS_ONLY");
            entity.Property(e => e.OwnersOnlyDescription)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("mediumtext")
                .HasColumnName("OWNERS_ONLY_DESCRIPTION");
            entity.Property(e => e.ResolutionChanged).HasColumnName("RESOLUTION_CHANGED");
            entity.Property(e => e.SystemComment)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("SYSTEM_COMMENT");
            entity.Property(e => e.TicketDataChange)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("TICKET_DATA_CHANGE");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("datetime")
                .HasColumnName("TIMESTAMP");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("USER_ID");
            entity.Property(e => e.ViaBulkUpdate)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("VIA_BULK_UPDATE");
            entity.Property(e => e.ViaEmail)
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("VIA_EMAIL");
            entity.Property(e => e.ViaImport)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("VIA_IMPORT");
            entity.Property(e => e.ViaScheduledProcess)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("VIA_SCHEDULED_PROCESS");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("LABEL");

            entity.HasIndex(e => e.MeterEnabled, "IDX_METER");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.AppCtrlEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("APP_CTRL_ENABLED");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("CREATED");
            entity.Property(e => e.KaceAltLocation)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("KACE_ALT_LOCATION");
            entity.Property(e => e.KaceAltLocationPasswordEnc)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("tinyblob")
                .HasColumnName("KACE_ALT_LOCATION_PASSWORD_ENC");
            entity.Property(e => e.KaceAltLocationUser)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("KACE_ALT_LOCATION_USER");
            entity.Property(e => e.MeterEnabled)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("METER_ENABLED");
            entity.Property(e => e.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NAME");
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("NOTES");
            entity.Property(e => e.ScopeUserRoleId)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SCOPE_USER_ROLE_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("TYPE");
            entity.Property(e => e.UsageAll)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_ALL");
            entity.Property(e => e.UsageCatalog)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_CATALOG");
            entity.Property(e => e.UsageDell)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_DELL");
            entity.Property(e => e.UsageLabel)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_LABEL");
            entity.Property(e => e.UsageMachine)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_MACHINE");
            entity.Property(e => e.UsageNode)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_NODE");
            entity.Property(e => e.UsagePatch)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_PATCH");
            entity.Property(e => e.UsageProcess)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_PROCESS");
            entity.Property(e => e.UsageSoftware)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_SOFTWARE");
            entity.Property(e => e.UsageUser)
                .HasColumnType("tinyint(1) unsigned")
                .HasColumnName("USAGE_USER");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("USER");

            entity.HasIndex(e => e.LdapUid, "IDX_LDAP_UID");

            entity.HasIndex(e => e.UserName, "IDX_NAME");

            entity.HasIndex(e => e.ManagerId, "IDX_PARENT");

            entity.HasIndex(e => e.Path, "IDX_PATH");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.ApiEnabled)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("API_ENABLED");
            entity.Property(e => e.ArchivedBy)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ARCHIVED_BY");
            entity.Property(e => e.ArchivedDate)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("ARCHIVED_DATE");
            entity.Property(e => e.BudgetCode)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("BUDGET_CODE");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("CREATED");
            entity.Property(e => e.DeviceCount)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("DEVICE_COUNT");
            entity.Property(e => e.Domain)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("DOMAIN");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasDefaultValueSql("''''''")
                .HasColumnName("EMAIL");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasDefaultValueSql("''''''")
                .HasColumnName("FULL_NAME");
            entity.Property(e => e.HdDefaultQueueId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("HD_DEFAULT_QUEUE_ID");
            entity.Property(e => e.HdDefaultView)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("HD_DEFAULT_VIEW");
            entity.Property(e => e.HomePhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("HOME_PHONE");
            entity.Property(e => e.IsArchived).HasColumnName("IS_ARCHIVED");
            entity.Property(e => e.LdapImported)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("LDAP_IMPORTED");
            entity.Property(e => e.LdapUid)
                .HasDefaultValueSql("''''''")
                .HasColumnName("LDAP_UID");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("int(11)")
                .HasColumnName("LEVEL");
            entity.Property(e => e.LinkedApplianceId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LINKED_APPLIANCE_ID");
            entity.Property(e => e.LocaleBrowserId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LOCALE_BROWSER_ID");
            entity.Property(e => e.LocationId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("LOCATION_ID");
            entity.Property(e => e.ManagerId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("MANAGER_ID");
            entity.Property(e => e.MobilePhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("MOBILE_PHONE");
            entity.Property(e => e.Modified)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("'current_timestamp()'")
                .HasColumnType("timestamp")
                .HasColumnName("MODIFIED");
            entity.Property(e => e.PagerPhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("PAGER_PHONE");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Path)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("PATH");
            entity.Property(e => e.Permissions)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PERMISSIONS");
            entity.Property(e => e.PrimaryDeviceId)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("PRIMARY_DEVICE_ID");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("'0'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.SalesNotifications)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SALES_NOTIFICATIONS");
            entity.Property(e => e.SambaAccess).HasColumnName("SAMBA_ACCESS");
            entity.Property(e => e.SamlImported)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("SAML_IMPORTED");
            entity.Property(e => e.SecurityNotifications)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("SECURITY_NOTIFICATIONS");
            entity.Property(e => e.Theme)
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("THEME");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasDefaultValueSql("''''''")
                .HasColumnName("USER_NAME");
            entity.Property(e => e.WorkPhone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''''''")
                .HasColumnName("WORK_PHONE");
            entity.Property(e => e._2faConfigured)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(2)")
                .HasColumnName("2FA_CONFIGURED");
            entity.Property(e => e._2faCutoffDate)
                .HasDefaultValueSql("'''0000-00-00 00:00:00'''")
                .HasColumnType("timestamp")
                .HasColumnName("2FA_CUTOFF_DATE");
            entity.Property(e => e._2faRequired)
                .HasDefaultValueSql("'0'")
                .HasColumnType("tinyint(2)")
                .HasColumnName("2FA_REQUIRED");
            entity.Property(e => e._2faSecret)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("tinyblob")
                .HasColumnName("2FA_SECRET");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("USER_MANAGER_ID");
        });

        modelBuilder.Entity<UserLabelJt>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LabelId }).HasName("PRIMARY");

            entity.ToTable("USER_LABEL_JT");

            entity.HasIndex(e => e.LabelId, "LABEL_IDX");

            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("USER_ID");
            entity.Property(e => e.LabelId)
                .HasColumnType("bigint(20)")
                .HasColumnName("LABEL_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
