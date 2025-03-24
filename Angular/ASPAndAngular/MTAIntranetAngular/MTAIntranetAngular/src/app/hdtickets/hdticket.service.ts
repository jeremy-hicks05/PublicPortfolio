import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../services/base.service';
import { Observable, map } from 'rxjs';

import { HdTicket } from './hdticket';
import { Apollo, gql } from 'apollo-angular';
import { HdTicketDTO } from './hdticketDTO';
/*import { Country } from './../countries/country';*/

@Injectable({
  providedIn: 'root',
})
export class HdTicketService
  extends BaseService<HdTicketDTO> {
  constructor(
    http: HttpClient,
    private apollo: Apollo) {
    super(http);
  }

  getData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<HdTicketDTO>> {
    return this.apollo
      .query({
        query: gql`
          query GetHdTicketsApiResult(
            $pageIndex: Int!,
            $pageSize: Int!,
            $sortColumn: String,
            $sortOrder: String,
            $filterColumn: String,
            $filterQuery: String) {
            hdTicketsApiResult(
              pageIndex: $pageIndex
              pageSize: $pageSize
              sortColumn: $sortColumn
              sortOrder: $sortOrder
              filterColumn: $filterColumn
              filterQuery: $filterQuery
              ) {
            data {
              id
              title
              created
              timeOpened
              timeStalled
              summary
              hdPriorityId
              hdImpactId
              ownerId
              submitterId
              hdStatusId
              hdQueueId
              hdCategoryId
              approverId
              approveState
              approval
              approvalNote
              priorityName
              impactName
              ownerName
              submitterName
              statusName
              queueName
              categoryName
            },
            pageIndex
            pageSize
            totalCount
            totalPages
            sortColumn
            sortOrder
            filterColumn
            filterQuery
          }
        }
        `,
        variables: {
          pageIndex,
          pageSize,
          sortColumn,
          sortOrder,
          filterColumn,
          filterQuery
        }
      })
      .pipe(map((result: any) =>
        result.data.hdTicketsApiResult));
  }

  getMyData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null,
    windowsCurrentUser: string | null
  ): Observable<ApiResult<HdTicketDTO>> {
    return this.apollo
      .query({
        query: gql`
          query GetHdMyTicketsApiResult(
            $pageIndex: Int!,
            $pageSize: Int!,
            $sortColumn: String,
            $sortOrder: String,
            $filterColumn: String,
            $filterQuery: String,
            $windowsCurrentUser: String) {
            hdMyTicketsApiResult(
              pageIndex: $pageIndex
              pageSize: $pageSize
              sortColumn: $sortColumn
              sortOrder: $sortOrder
              filterColumn: $filterColumn
              filterQuery: $filterQuery
              windowsCurrentUser: $windowsCurrentUser
              ) {
            data {
              id
              title
              created
              timeOpened
              timeStalled
              summary
              hdPriorityId
              hdImpactId
              ownerId
              submitterId
              hdStatusId
              hdQueueId
              hdCategoryId
              approverId
              approveState
              approval
              approvalNote
              approverName
              priorityName
              impactName
              ownerName
              submitterName
              statusName
              queueName
              categoryName
            },
            pageIndex
            pageSize
            totalCount
            totalPages
            sortColumn
            sortOrder
            filterColumn
            filterQuery
            windowsCurrentUser
          }
        }
        `,
        variables: {
          pageIndex,
          pageSize,
          sortColumn,
          sortOrder,
          filterColumn,
          filterQuery,
          windowsCurrentUser
        }
      })
      .pipe(map((result: any) =>
        result.data.hdMyTicketsApiResult));
  }

  getMyApprovalNeededData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null,
    windowsCurrentUser: string | null
  ): Observable<ApiResult<HdTicketDTO>> {
    return this.apollo
      .query({
        query: gql`
          query GetHdTicketsThatNeedMyApprovalApiResult(
            $pageIndex: Int!,
            $pageSize: Int!,
            $sortColumn: String,
            $sortOrder: String,
            $filterColumn: String,
            $filterQuery: String,
            $windowsCurrentUser: String) {
            hdTicketsThatNeedMyApprovalApiResult(
              pageIndex: $pageIndex
              pageSize: $pageSize
              sortColumn: $sortColumn
              sortOrder: $sortOrder
              filterColumn: $filterColumn
              filterQuery: $filterQuery
              windowsCurrentUser: $windowsCurrentUser
              ) {
            data {
              id
              title
              created
              timeOpened
              timeStalled
              summary
              hdPriorityId
              hdImpactId
              ownerId
              submitterId
              hdStatusId
              hdQueueId
              hdCategoryId
              approverId
              approveState
              approval
              approvalNote
              approverName
              priorityName
              impactName
              ownerName
              submitterName
              statusName
              queueName
              categoryName
            },
            pageIndex
            pageSize
            totalCount
            totalPages
            sortColumn
            sortOrder
            filterColumn
            filterQuery
            windowsCurrentUser
          }
        }
        `,
        variables: {
          pageIndex,
          pageSize,
          sortColumn,
          sortOrder,
          filterColumn,
          filterQuery,
          windowsCurrentUser
        }
      })
      .pipe(map((result: any) =>
        result.data.hdTicketsThatNeedMyApprovalApiResult));
  }

  //

  //get(id: number): Observable<Ticket> {
  //  var url = this.getUrl("api/Tickets/" + id);
  //  return this.http.get<Ticket>(url);
  //}

  get(id: number): Observable<HdTicketDTO> {
    return this.apollo
      .query({
        query: gql`
          query GetHdTicketById($id: Int!) {
          tickets(where: { ticketId: { eq: $id } }) {
          nodes {
              ticketId
              categoryId
              subTypeId
              impactId
              summary
              reasonForRejection
              approvalStateId
              approvedBy
              dateEntered
              dateLastUpdated
              enteredByUser
              categoryName
              subTypeName
              impactName
              approvalStateName
            }
          }
        }
        `,
        variables: {
          id
        }
      })
      .pipe(map((result: any) =>
        result.data.hdTickets.nodes[0]));
  }

  //put(item: Ticket): Observable<Ticket> {
  //  var url = this.getUrl("api/Tickets/" + item.ticketId);
  //  return this.http.put<Ticket>(url, item);
  //}

  // TODO: Add category, subType, impact, and approvalState names??
  put(input: HdTicketDTO): Observable<HdTicketDTO> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation UpdateTicket($ticket: TicketDTOInput!) {
          updateTicket(ticketDTO: $ticket) {
              ticketId
              categoryId
              subTypeId
              impactId
              summary
              reasonForRejection
              approvalStateId
              approvedBy
              dateEntered
              dateLastUpdated
              enteredByUser
              }
            }
          }
        `,
        variables: {
          ticket: input
        }
      }).pipe(map((result: any) =>
        result.data.updateTicket));
  }

  //post(item: Ticket): Observable<Ticket> {
  //  var url = this.getUrl("api/Tickets");
  //  return this.http.post<Ticket>(url, item);
  //}

  post(item: HdTicketDTO): Observable<HdTicketDTO> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation AddTicket($ticket: TicketDTOInput!) {
          addTicket(ticketDTO: $ticket) {
            ticketId
            categoryId
              subTypeId
              impactId
              summary
              reasonForRejection
              approvalStateId
              approvedBy
              dateEntered
              dateLastUpdated
              enteredByUser
          }
        }
        `,
        variables: {
          ticket: item
        }
      }).pipe(map((result: any) =>
        result.data.addTicket));
  }

  getCategories(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<HdTicket>> {
    var url = this.getUrl("api/HdTickets");
    var params = new HttpParams()
      .set("pageIndex", pageIndex.toString())
      .set("pageSize", pageSize.toString())
      .set("sortColumn", sortColumn)
      .set("sortOrder", sortOrder);

    if (filterColumn && filterQuery) {
      params = params
        .set("filterColumn", filterColumn)
        .set("filterQuery", filterQuery);
    }

    return this.http.get<ApiResult<HdTicket>>(url, { params });
  }

  isDupeTicket(item: HdTicket): Observable<boolean> {
    var url = this.getUrl("api/HdTickets/IsDupeHdTicket");
    return this.http.post<boolean>(url, item);
  }
}
