using HotChocolate.AspNetCore.Authorization;

// Remove?
using HotChocolate.Authorization;

using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data.Models;
using System.Diagnostics.Metrics;
//using MTAIntranetAngular.API.Data;

namespace MTAIntranetAngular.API.Data.GraphQL
{
    public class Mutation
    {
        /// <summary>
        /// Add a new City
        /// </summary>
        [Serial]
        // TODO: Authorize IT only??
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Ticket> AddTicket(
            [Service] MtaticketsContext context,
            TicketDTO ticketDTO)
        {
            var ticket = new Ticket()
            {
                // add ticket info
                Summary = ticketDTO.Summary,
                ReasonForRejection = ticketDTO.ReasonForRejection,
                ApprovedBy = ticketDTO.ApprovedBy,
                DateEntered = ticketDTO.DateEntered,
                DateLastUpdated = ticketDTO.DateLastUpdated,
                EnteredByUser = ticketDTO.EnteredByUser,
                ApprovalStateId = ticketDTO.ApprovalStateId,
                CategoryId = ticketDTO.CategoryId,
                ImpactId = ticketDTO.ImpactId,
                SubTypeId = ticketDTO.SubTypeId
            };
            context.Tickets.Add(ticket);
            await context.SaveChangesAsync();
            return ticket;
        }

        /// <summary>
        /// Update an existing Country
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Ticket> UpdateTicket(
        [Service] MtaticketsContext context, TicketDTO ticketDTO)
        {
            var ticket = await context.Tickets
            .Where(t => t.TicketId == ticketDTO.TicketId)
            .FirstOrDefaultAsync();
            if (ticket == null)
                // todo: handle errors
                throw new NotSupportedException();

            // TODO: Set values from dto to object
            ticket.Summary = ticketDTO.Summary;
            ticket.ReasonForRejection = ticketDTO.ReasonForRejection;
            ticket.ApprovedBy = ticketDTO.ApprovedBy;
            ticket.DateEntered = ticketDTO.DateEntered;
            ticket.DateLastUpdated = ticketDTO.DateLastUpdated;
            ticket.EnteredByUser = ticketDTO.EnteredByUser;
            ticket.ApprovalStateId = ticketDTO.ApprovalStateId;
            ticket.CategoryId = ticketDTO.CategoryId;
            ticket.ImpactId = ticketDTO.ImpactId;
            ticket.SubTypeId = ticketDTO.SubTypeId;

            context.Tickets.Update(ticket);
            await context.SaveChangesAsync();
            return ticket;
        }

        /// <summary>
        /// Delete a City
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "Administrator" })]
        public async Task DeleteTicket(
            [Service] MtaticketsContext context,
            int id)
        {
            var ticket = await context.Tickets
            .Where(t => t.TicketId == id)
            .FirstOrDefaultAsync();
            if (ticket != null)
            {
                context.Tickets.Remove(ticket);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add a new Country
        /// </summary>
        [Serial]
        // [Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Category> AddCategory(
        [Service] MtaticketsContext context, CategoryDTO categoryDTO)
        {
            var category = new Category()
            {
                // TODO: Set values from dto to object
                Name = categoryDTO.Name
            };
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category;
        }

        /// <summary>
        /// Update an existing City
        /// </summary>
        [Serial]
        // TODO: Authorize IT only??
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Category> UpdateCategory(
            [Service] MtaticketsContext context,
            CategoryDTO categoryDTO)
        {
            var category = await context.Categories
            .Where(c => c.CategoryId == categoryDTO.CategoryId)
            .FirstOrDefaultAsync();
            if (category == null)
                // todo: handle errors
                throw new NotSupportedException();

            // TODO: Set values from dto to object
            category.Name = categoryDTO.Name;

            context.Categories.Update(category);
            await context.SaveChangesAsync();
            return category;
        }

        /// <summary>
        /// Delete a Country
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "Administrator" })]
        public async Task DeleteCategory(
        [Service] MtaticketsContext context, int id)
        {
            var category = await context.Categories
            .Where(c => c.CategoryId == id)
            .FirstOrDefaultAsync();
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
        }

        // HD Categories
        /// <summary>
        /// Add a new Country
        /// </summary>
        //[Serial]
        //// [Authorize(Roles = new[] { "RegisteredUser" })]
        //public async Task<HdCategory> AddHDCategory(
        //[Service] Org1Context context, HDCategoryDTO hdCategoryDTO)
        //{
        //    var hdCategory = new HdCategory()
        //    {
        //        // TODO: Set values from dto to object
        //        Name = hdCategoryDTO.Name
        //    };
        //    context.HdCategories.Add(hdCategory);
        //    await context.SaveChangesAsync();
        //    return hdCategory;
        //}

        /// <summary>
        /// Update an existing City
        /// </summary>
        //[Serial]
        //// TODO: Authorize IT only??
        ////[Authorize(Roles = new[] { "RegisteredUser" })]
        //public async Task<HdCategory> UpdateHDCategory(
        //    [Service] Org1Context context,
        //    HDCategoryDTO hdCategoryDTO)
        //{
        //    var hdCategory = await context.HdCategories
        //    .Where(c => c.Id == hdCategoryDTO.Id)
        //    .FirstOrDefaultAsync();
        //    if (hdCategory == null)
        //        // todo: handle errors
        //        throw new NotSupportedException();

        //    // TODO: Set values from dto to object
        //    hdCategory.Name = hdCategoryDTO.Name;

        //    context.HdCategories.Update(hdCategory);
        //    await context.SaveChangesAsync();
        //    return hdCategory;
        //}

        /// <summary>
        /// Delete a Country
        /// </summary>
        //[Serial]
        ////[Authorize(Roles = new[] { "Administrator" })]
        //public async Task DeleteHDCategory(
        //[Service] Org1Context context, ulong id)
        //{
        //    var hdCategory = await context.HdCategories
        //    .Where(c => c.Id == id)
        //    .FirstOrDefaultAsync();
        //    if (hdCategory != null)
        //    {
        //        context.HdCategories.Remove(hdCategory);
        //        await context.SaveChangesAsync();
        //    }
        //}
        // End HD Categories

        /// <summary>
        /// Add a new Country
        /// </summary>
        [Serial]
        // [Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<ApprovalState> AddApprovalState(
        [Service] MtaticketsContext context, ApprovalStateDTO approvalStateDTO)
        {
            var approvalState = new ApprovalState()
            {
                // TODO: Set values from dto to object
                Name = approvalStateDTO.Name
            };
            context.ApprovalStates.Add(approvalState);
            await context.SaveChangesAsync();
            return approvalState;
        }

        /// <summary>
        /// Update an existing City
        /// </summary>
        [Serial]
        // TODO: Authorize IT only??
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<ApprovalState> UpdateApprovalState(
            [Service] MtaticketsContext context,
            ApprovalStateDTO approvalStateDTO)
        {
            var approvalState = await context.ApprovalStates
            .Where(a => a.ApprovalStateId == approvalStateDTO.ApprovalStateId)
            .FirstOrDefaultAsync();
            if (approvalState == null)
                // todo: handle errors
                throw new NotSupportedException();

            // TODO: Set values from dto to object
            approvalState.Name = approvalStateDTO.Name;

            context.ApprovalStates.Update(approvalState);
            await context.SaveChangesAsync();
            return approvalState;
        }

        /// <summary>
        /// Delete a Country
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "Administrator" })]
        public async Task DeleteApprovalState(
        [Service] MtaticketsContext context, int id)
        {
            var approvalState = await context.ApprovalStates
            .Where(a => a.ApprovalStateId == id)
            .FirstOrDefaultAsync();
            if (approvalState != null)
            {
                context.ApprovalStates.Remove(approvalState);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add a new Country
        /// </summary>
        [Serial]
        // [Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Impact> AddImpact(
        [Service] MtaticketsContext context, ImpactDTO impactDTO)
        {
            var impact = new Impact()
            {
                // TODO: Set values from dto to object
                Description = impactDTO.Description
            };
            context.Impacts.Add(impact);
            await context.SaveChangesAsync();
            return impact;
        }

        /// <summary>
        /// Update an existing City
        /// </summary>
        [Serial]
        // TODO: Authorize IT only??
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<Impact> UpdateImpact(
            [Service] MtaticketsContext context,
            ImpactDTO impactDTO)
        {
            var impact = await context.Impacts
            .Where(i => i.ImpactId == impactDTO.ImpactId)
            .FirstOrDefaultAsync();
            if (impact == null)
                // todo: handle errors
                throw new NotSupportedException();

            // TODO: Set values from dto to object
            impact.Description = impactDTO.Description;

            context.Impacts.Update(impact);
            await context.SaveChangesAsync();
            return impact;
        }

        /// <summary>
        /// Delete a Country
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "Administrator" })]
        public async Task DeleteImpact(
        [Service] MtaticketsContext context, int id)
        {
            var impact = await context.Impacts
            .Where(i => i.ImpactId == id)
            .FirstOrDefaultAsync();
            if (impact != null)
            {
                context.Impacts.Remove(impact);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add a new Country
        /// </summary>
        [Serial]
        // [Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<TicketSubType> AddTicketSubType(
        [Service] MtaticketsContext context, TicketSubTypeDTO subTypeDTO)
        {
            var subType = new TicketSubType()
            {
                // TODO: Set values from dto to object
                CategoryId = subTypeDTO.CategoryId,
                Description = subTypeDTO.Description,
                Cclist = subTypeDTO.Cclist,
                NeedsApproval = subTypeDTO.NeedsApproval,
                Name = subTypeDTO.Name
            };
            context.TicketSubTypes.Add(subType);
            await context.SaveChangesAsync();
            return subType;
        }

        /// <summary>
        /// Update an existing City
        /// </summary>
        [Serial]
        // TODO: Authorize IT only??
        //[Authorize(Roles = new[] { "RegisteredUser" })]
        public async Task<TicketSubType> UpdateTicketSubType(
            [Service] MtaticketsContext context,
            TicketSubTypeDTO subTypeDTO)
        {
            var subType = await context.TicketSubTypes
            .Where(s => s.TicketSubTypeId == subTypeDTO.TicketSubTypeId)
            .FirstOrDefaultAsync();
            if (subType == null)
                // todo: handle errors
                throw new NotSupportedException();

            // TODO: Set values from dto to object
            subType.Description = subTypeDTO.Description;
            subType.Cclist = subTypeDTO.Cclist;
            subType.NeedsApproval = subTypeDTO.NeedsApproval;
            subType.Name = subTypeDTO.Name;

            context.TicketSubTypes.Update(subType);
            await context.SaveChangesAsync();
            return subType;
        }

        /// <summary>
        /// Delete a Country
        /// </summary>
        [Serial]
        //[Authorize(Roles = new[] { "Administrator" })]
        public async Task DeleteTicketSubType(
        [Service] MtaticketsContext context, int id)
        {
            var subType = await context.TicketSubTypes
            .Where(t => t.TicketSubTypeId == id)
            .FirstOrDefaultAsync();
            if (subType != null)
            {
                context.TicketSubTypes.Remove(subType);
                await context.SaveChangesAsync();
            }
        }
    }
}
