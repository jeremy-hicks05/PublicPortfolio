using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Data.GraphQL
{
    public class Query
    {
        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ApprovalState> GetApprovalStates(
        [Service] MtaticketsContext context)
        => context.ApprovalStates;


        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Category> GetCategories(
        [Service] MtaticketsContext context)
        => context.Categories;

        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<HdCategory> GetHDCategories(
        [Service] Org1Context context)
        => context.HdCategories;

        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<HdImpact> GetImpacts(
        [Service] Org1Context context)
        => context.HdImpacts;

        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<HdTicket> GetHdTickets(
        [Service] Org1Context context)
        => context.HdTickets;

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<HdTicketDTO>> GetHdTicketsApiResult(
        [Service] Org1Context context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult<HdTicketDTO>.CreateAsync(
            context.HdTickets.AsNoTracking()
            .Select(c => new HdTicketDTO()
            {
                Id = c.Id,
                Title = c.Title,
                Created = c.Created.ToString("yyyy-MM-dd"),
                TimeOpened = c.TimeOpened,
                TimeStalled = c.TimeStalled,
                Summary = c.Summary!,
                HdPriorityId = c.HdPriorityId,
                HdImpactId = c.HdImpactId,
                OwnerId = c.OwnerId,
                SubmitterId = c.SubmitterId,
                HdStatusId = c.HdStatusId,
                HdQueueId = c.HdQueueId,
                HdCategoryId = c.HdCategoryId,
                ApproverId = c.ApproverId,
                ApproveState = c.ApproveState,
                Approval = c.Approval,
                ApprovalNote = c.ApprovalNote,
                PriorityName = context.HdPriorities.First(hdp => hdp.Id == c.HdPriorityId).Name,
                ImpactName = context.HdImpacts.First(hdi => hdi.Id == c.HdImpactId).Name,
                OwnerName = context.Users.FirstOrDefault(u => u.Id == c.OwnerId) == null ? "Unassigned" : context.Users.First(u => u.Id == c.OwnerId).UserName,
                SubmitterName = context.Users.First(u => u.Id == c.SubmitterId).UserName,
                StatusName = context.HdStatuses.First(s => s.Id == c.HdStatusId).Name,
                QueueName = context.HdQueues.First(q => q.Id == c.HdQueueId).Name!,
                CategoryName = context.HdCategories.First(hdc => hdc.Id == c.HdCategoryId).Name
                //CustomFieldValue0 = c.CustomFieldValue0,
                //CustomFieldValue1 = c.CustomFieldValue1,
                //CustomFieldValue2 = c.CustomFieldValue2,
                //CustomFieldValue3 = c.CustomFieldValue3,
                //CustomFieldValue4 = c.CustomFieldValue4,
                //CustomFieldValue5 = c.CustomFieldValue5,
                //CustomFieldValue6 = c.CustomFieldValue6,
                //CustomFieldValue7 = c.CustomFieldValue7,
                //CustomFieldValue8 = c.CustomFieldValue8,
                //CustomFieldValue9 = c.CustomFieldValue9,
                //CustomFieldValue10 = c.CustomFieldValue10,
                //CustomFieldValue11 = c.CustomFieldValue11,
                //CustomFieldValue12 = c.CustomFieldValue12,
                //CustomFieldValue13 = c.CustomFieldValue13,
                //CustomFieldValue14 = c.CustomFieldValue14
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<HdTicketDTO>> GetHdMyTicketsApiResult(
        [Service] Org1Context context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null,
        string? windowsCurrentUser = null)
        {
            string userWithoutDomain = windowsCurrentUser == null ? "" : windowsCurrentUser!.Split("\\")[1];

            return await ApiResult<HdTicketDTO>.CreateAsync(
            context.HdTickets
            .Where(hdt =>
            (
                (context.Users.First(u =>
                        u.UserName == userWithoutDomain).Id == hdt.OwnerId ||
                    hdt.SubmitterId == context.Users.First(u =>
                        u.UserName == userWithoutDomain).Id)
                &&
                // Not Closed
                context.HdStatuses.First(st =>
                    st.Id == hdt.HdStatusId).Name != "Closed"
            )
            // OR
            // most recent comment is within last 14 days
            ||
            (
                (context.Users.First(u =>
                    u.UserName == userWithoutDomain).Id == hdt.OwnerId ||
                hdt.SubmitterId == context.Users.First(u =>
                    u.UserName == userWithoutDomain).Id)
                &&
                hdt.Modified >= DateTime.Now.AddDays(-14))
            )
            .AsNoTracking()
            .Select(c => new HdTicketDTO()
            {
                Id = c.Id,
                Title = c.Title,
                Created = c.Created.ToString("yyyy-MM-dd"),
                TimeOpened = c.TimeOpened,
                TimeStalled = c.TimeStalled,
                Summary = c.Summary!,
                HdPriorityId = c.HdPriorityId,
                HdImpactId = c.HdImpactId,
                OwnerId = c.OwnerId,
                SubmitterId = c.SubmitterId,
                HdStatusId = c.HdStatusId,
                HdQueueId = c.HdQueueId,
                HdCategoryId = c.HdCategoryId,
                ApproverId = c.ApproverId,
                ApproveState = c.ApproveState,
                Approval = c.Approval,
                ApprovalNote = c.ApprovalNote,
                ApproverName = context.Users.FirstOrDefault(u => u.Id == c.ApproverId) == null ? "" : context.Users.First(u => u.Id == c.ApproverId).UserName,
                PriorityName = context.HdPriorities.First(hdp => hdp.Id == c.HdPriorityId).Name,
                ImpactName = context.HdImpacts.First(hdi => hdi.Id == c.HdImpactId).Name,
                OwnerName = context.Users.FirstOrDefault(u => u.Id == c.OwnerId) == null ? "Unassigned" : context.Users.First(u => u.Id == c.OwnerId).UserName,
                SubmitterName = context.Users.First(u => u.Id == c.SubmitterId).UserName,
                StatusName = context.HdStatuses.First(s => s.Id == c.HdStatusId).Name,
                QueueName = context.HdQueues.First(q => q.Id == c.HdQueueId).Name!,
                CategoryName = context.HdCategories.First(hdc => hdc.Id == c.HdCategoryId).Name
                //CustomFieldValue0 = c.CustomFieldValue0,
                //CustomFieldValue1 = c.CustomFieldValue1,
                //CustomFieldValue2 = c.CustomFieldValue2,
                //CustomFieldValue3 = c.CustomFieldValue3,
                //CustomFieldValue4 = c.CustomFieldValue4,
                //CustomFieldValue5 = c.CustomFieldValue5,
                //CustomFieldValue6 = c.CustomFieldValue6,
                //CustomFieldValue7 = c.CustomFieldValue7,
                //CustomFieldValue8 = c.CustomFieldValue8,
                //CustomFieldValue9 = c.CustomFieldValue9,
                //CustomFieldValue10 = c.CustomFieldValue10,
                //CustomFieldValue11 = c.CustomFieldValue11,
                //CustomFieldValue12 = c.CustomFieldValue12,
                //CustomFieldValue13 = c.CustomFieldValue13,
                //CustomFieldValue14 = c.CustomFieldValue14
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<HdTicketDTO>> GetHdTicketsThatNeedMyApprovalApiResult(
        [Service] Org1Context context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null,
        string? windowsCurrentUser = null)
        {
            string userWithoutDomain = windowsCurrentUser == null ? "" : windowsCurrentUser!.Split("\\")[1];
            long userId = context.Users.First(u => u.UserName == userWithoutDomain).Id;

            var userLabels = context.UserLabelJts
            .Where(ujt => ujt.UserId == userId)
            .Select(ujt => ujt.LabelId)
            .ToList();

            var approverForQueues = context.HdQueueApproverLabelJts
                .Where(qa => userLabels.Contains(qa.LabelId))
                .Select(qa => qa.HdQueueId)
                .ToList();

            // reutrn list of tickets in queue that I am an approver on
            return await ApiResult<HdTicketDTO>.CreateAsync(
            context.HdTickets
            .Where(hdt =>
            (
                approverForQueues.Contains(hdt.HdQueueId) &&
                //(hdt.ApproveState == "opened" || hdt.ApproverId == 0 || hdt.Approval == "" || hdt.Approval == "none")
                !(hdt.Approval == "approved" || hdt.Approval == "rejected")
            //(userId == hdt.ApproverId) &&
            ))
            .AsNoTracking()
            .Select(c => new HdTicketDTO()
            {
                Id = c.Id,
                Title = c.Title,
                Created = c.Created.ToString("yyyy-MM-dd"),
                TimeOpened = c.TimeOpened,
                TimeStalled = c.TimeStalled,
                Summary = c.Summary!,
                HdPriorityId = c.HdPriorityId,
                HdImpactId = c.HdImpactId,
                OwnerId = c.OwnerId,
                SubmitterId = c.SubmitterId,
                HdStatusId = c.HdStatusId,
                HdQueueId = c.HdQueueId,
                HdCategoryId = c.HdCategoryId,
                ApproverId = c.ApproverId,
                ApproveState = c.ApproveState,
                Approval = c.Approval,
                ApprovalNote = c.ApprovalNote,
                ApproverName = context.Users.FirstOrDefault(u => u.Id == c.ApproverId) == null ? "" : context.Users.First(u => u.Id == c.ApproverId).UserName,
                PriorityName = context.HdPriorities.First(hdp => hdp.Id == c.HdPriorityId).Name,
                ImpactName = context.HdImpacts.First(hdi => hdi.Id == c.HdImpactId).Name,
                OwnerName = context.Users.FirstOrDefault(u => u.Id == c.OwnerId) == null ? "Unassigned" : context.Users.First(u => u.Id == c.OwnerId).UserName,
                SubmitterName = context.Users.First(u => u.Id == c.SubmitterId).UserName,
                StatusName = context.HdStatuses.First(s => s.Id == c.HdStatusId).Name,
                QueueName = context.HdQueues.First(q => q.Id == c.HdQueueId).Name!,
                CategoryName = context.HdCategories.First(hdc => hdc.Id == c.HdCategoryId).Name
                //CustomFieldValue0 = c.CustomFieldValue0,
                //CustomFieldValue1 = c.CustomFieldValue1,
                //CustomFieldValue2 = c.CustomFieldValue2,
                //CustomFieldValue3 = c.CustomFieldValue3,
                //CustomFieldValue4 = c.CustomFieldValue4,
                //CustomFieldValue5 = c.CustomFieldValue5,
                //CustomFieldValue6 = c.CustomFieldValue6,
                //CustomFieldValue7 = c.CustomFieldValue7,
                //CustomFieldValue8 = c.CustomFieldValue8,
                //CustomFieldValue9 = c.CustomFieldValue9,
                //CustomFieldValue10 = c.CustomFieldValue10,
                //CustomFieldValue11 = c.CustomFieldValue11,
                //CustomFieldValue12 = c.CustomFieldValue12,
                //CustomFieldValue13 = c.CustomFieldValue13,
                //CustomFieldValue14 = c.CustomFieldValue14
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<HdTicketChangeDTO>> GetHdMyTicketChangesApiResult(
        [Service] Org1Context context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null,
        int ticketNumber = 0)
        {
            return await ApiResult<HdTicketChangeDTO>.CreateAsync(
            context.HdTicketChanges
            .Where(hdtc => hdtc.HdTicketId == ticketNumber)
            .AsNoTracking()
            .Select(c => new HdTicketChangeDTO()
            {
                Id = c.Id,
                HdTicketId = c.HdTicketId,
                UserId = c.UserId,
                UserName = context.Users.FirstOrDefault(u => u.Id == c.UserId) == null ? "Unassigned" :
                    context.Users.First(u => u.Id == c.UserId).UserName,
                OwnersOnly = c.OwnersOnly,
                Timestamp = c.Timestamp.ToString("yyyy-MM-dd HH:mm"),
                Comment = c.Comment,
                Description = c.Description
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<HdTicketDTO>> GetTicketsThatNeedMyAttentionApiResult(
        [Service] Org1Context context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null,
        string? windowsCurrentUser = null)
        {
            string userWithoutDomain = windowsCurrentUser == null ? "" : windowsCurrentUser!.Split("\\")[1];

            return await ApiResult<HdTicketDTO>.CreateAsync(
            context.HdTickets
            .Where(hdt =>
            context.Users.First(u =>
                        u.UserName == userWithoutDomain).Id == hdt.SubmitterId
                &&
                (
                context.HdStatuses.First(st =>
                    st.Id == hdt.HdStatusId).Name != "Waiting on Submitter Input"
                ))
            .AsNoTracking()
            .Select(c => new HdTicketDTO()
            {
                Id = c.Id,
                Title = c.Title,
                Summary = c.Summary!,
                HdPriorityId = c.HdPriorityId,
                HdImpactId = c.HdImpactId,
                OwnerId = c.OwnerId,
                SubmitterId = c.SubmitterId,
                HdStatusId = c.HdStatusId,
                HdQueueId = c.HdQueueId,
                HdCategoryId = c.HdCategoryId,
                PriorityName = context.HdPriorities.First(hdp => hdp.Id == c.HdPriorityId).Name,
                ImpactName = context.HdImpacts.First(hdi => hdi.Id == c.HdImpactId).Name,
                OwnerName = context.Users.FirstOrDefault(u => u.Id == c.OwnerId) == null ? "Unassigned" : context.Users.First(u => u.Id == c.OwnerId).UserName,
                SubmitterName = context.Users.First(u => u.Id == c.SubmitterId).UserName,
                StatusName = context.HdStatuses.First(s => s.Id == c.HdStatusId).Name,
                QueueName = context.HdQueues.First(q => q.Id == c.HdQueueId).Name!,
                CategoryName = context.HdCategories.First(hdc => hdc.Id == c.HdCategoryId).Name
                //CustomFieldValue0 = c.CustomFieldValue0,
                //CustomFieldValue1 = c.CustomFieldValue1,
                //CustomFieldValue2 = c.CustomFieldValue2,
                //CustomFieldValue3 = c.CustomFieldValue3,
                //CustomFieldValue4 = c.CustomFieldValue4,
                //CustomFieldValue5 = c.CustomFieldValue5,
                //CustomFieldValue6 = c.CustomFieldValue6,
                //CustomFieldValue7 = c.CustomFieldValue7,
                //CustomFieldValue8 = c.CustomFieldValue8,
                //CustomFieldValue9 = c.CustomFieldValue9,
                //CustomFieldValue10 = c.CustomFieldValue10,
                //CustomFieldValue11 = c.CustomFieldValue11,
                //CustomFieldValue12 = c.CustomFieldValue12,
                //CustomFieldValue13 = c.CustomFieldValue13,
                //CustomFieldValue14 = c.CustomFieldValue14
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities.
        /// </summary>
        [Serial]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TicketSubType> GetTicketSubTypes(
        [Service] MtaticketsContext context)
        => context.TicketSubTypes;


        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<CategoryDTO>> GetCategoriesApiResult(
        [Service] MtaticketsContext context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult<CategoryDTO>.CreateAsync(
            context.Categories.AsNoTracking()
            .Select(c => new CategoryDTO()
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        //[Serial]
        //public async Task<ApiResult<HDCategoryDTO>> GetHDCategoriesApiResult(
        //[Service] Org1Context context,
        //int pageIndex = 0,
        //int pageSize = 10,
        //string? sortColumn = null,
        //string? sortOrder = null,
        //string? filterColumn = null,
        //string? filterQuery = null)
        //{
        //    return await ApiResult<HDCategoryDTO>.CreateAsync(
        //    context.HdCategories.AsNoTracking()
        //    .Select(c => new HDCategoryDTO()
        //    {
        //        Id = c.Id,
        //        Name = c.Name
        //    }),
        //    pageIndex,
        //    pageSize,
        //    sortColumn,
        //    sortOrder,
        //    filterColumn,
        //    filterQuery);
        //}

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<ApprovalStateDTO>> GetApprovalStatesApiResult(
        [Service] MtaticketsContext context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult<ApprovalStateDTO>.CreateAsync(
            context.ApprovalStates.AsNoTracking()
            .Select(c => new ApprovalStateDTO()
            {
                ApprovalStateId = c.ApprovalStateId,
                Name = c.Name
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        //[Serial]
        //public async Task<ApiResult<HdImpact>> GetImpactsApiResult(
        //[Service] Org1Context context,
        //int pageIndex = 0,
        //int pageSize = 10,
        //string? sortColumn = null,
        //string? sortOrder = null,
        //string? filterColumn = null,
        //string? filterQuery = null)
        //{
        //    return await ApiResult<HdImpact>.CreateAsync(
        //    context.HdImpacts.AsNoTracking()
        //    .Select(c => new HdImpact()
        //    {
        //        Id = c.Id,
        //        Name = c.Name
        //    }),
        //    pageIndex,
        //    pageSize,
        //    sortColumn,
        //    sortOrder,
        //    filterColumn,
        //    filterQuery);
        //}

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<TicketDTO>> GetTicketsApiResult(
        [Service] MtaticketsContext context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult<TicketDTO>.CreateAsync(
            context.Tickets.AsNoTracking()
            .Select(c => new TicketDTO()
            {
                TicketId = c.TicketId,
                ApprovalStateId = c.ApprovalStateId,
                ImpactId = c.ImpactId,
                CategoryId = c.CategoryId,
                SubTypeId = c.SubTypeId,
                ApprovedBy = c.ApprovedBy,
                EnteredByUser = c.EnteredByUser,
                DateEntered = c.DateEntered,
                DateLastUpdated = c.DateLastUpdated,
                ReasonForRejection = c.ReasonForRejection,
                Summary = c.Summary,
                CategoryName = (c.Category != null ? c.Category.Name : "Null Category"),
                SubTypeName = (c.SubType != null ? c.SubType.Name : "Null Category"),
                ImpactName = (c.Impact != null ? c.Impact.Description : "Null Impact"),
                ApprovalStateName = (c.ApprovalState != null ? c.ApprovalState.Name : "Null Approval State")
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }

        /// <summary>
        /// Gets all Cities (with ApiResult and DTO support).
        /// </summary>
        [Serial]
        public async Task<ApiResult<TicketSubTypeDTO>> GetTicketSubTypesApiResult(
        [Service] MtaticketsContext context,
        int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = null,
        string? sortOrder = null,
        string? filterColumn = null,
        string? filterQuery = null)
        {
            return await ApiResult<TicketSubTypeDTO>.CreateAsync(
            context.TicketSubTypes.AsNoTracking()
            .Select(c => new TicketSubTypeDTO()
            {
                TicketSubTypeId = c.TicketSubTypeId,
                CategoryId = c.CategoryId,
                NeedsApproval = c.NeedsApproval,
                Cclist = c.Cclist,
                Name = c.Name,
                Description = c.Description,
                CategoryName = c.Category!.Name
            }),
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterColumn,
            filterQuery);
        }
    }
}
