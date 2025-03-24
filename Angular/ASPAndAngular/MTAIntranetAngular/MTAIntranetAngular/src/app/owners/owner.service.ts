import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BaseService, ApiResult } from '../services/base.service';
import { Observable, map } from 'rxjs';

import { Owner } from './owner';
import { Apollo, gql } from 'apollo-angular';
/*import { Country } from './../countries/country';*/

@Injectable({
  providedIn: 'root',
})
export class OwnerService
  extends BaseService<Owner> {
  constructor(
    http: HttpClient,
    private apollo: Apollo) {
    super(http);
  }

  //getData(
  //  pageIndex: number,
  //  pageSize: number,
  //  sortColumn: string,
  //  sortOrder: string,
  //  filterColumn: string | null,
  //  filterQuery: string | null
  //): Observable<ApiResult<Impact>> {
  //  var url = this.getUrl("api/Impacts");
  //  var params = new HttpParams()
  //    .set("pageIndex", pageIndex.toString())
  //    .set("pageSize", pageSize.toString())
  //    .set("sortColumn", sortColumn)
  //    .set("sortOrder", sortOrder);

  //  if (filterColumn && filterQuery) {
  //    params = params
  //      .set("filterColumn", filterColumn)
  //      .set("filterQuery", filterQuery);
  //  }

  //  return this.http.get<ApiResult<Impact>>(url, { params });
  //}

  getData(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<Owner>> {
    return this.apollo
      .query({
        query: gql`
          query GetOwnerssApiResult(
            $pageIndex: Int!,
            $pageSize: Int!,
            $sortColumn: String,
            $sortOrder: String,
            $filterColumn: String,
            $filterQuery: String) {
            impactsApiResult(
              pageIndex: $pageIndex
              pageSize: $pageSize
              sortColumn: $sortColumn
              sortOrder: $sortOrder
              filterColumn: $filterColumn
              filterQuery: $filterQuery
              ) {
            data {
              ownerId
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
        result.data.ownersApiResult));
  }

  //get(id: number): Observable<Impact> {
  //  var url = this.getUrl("api/Impacts/" + id);
  //  return this.http.get<Impact>(url);
  //}

  get(id: number): Observable<Owner> {
    return this.apollo
      .query({
        query: gql`
          query GetOwnerById($id: Int!) {
          impacts(where: { ownerId: { eq: $id } }) {
          nodes {
              ownerId
            }
          }
        }
        `,
        variables: {
          id
        }
      })
      .pipe(map((result: any) =>
        result.data.owners.nodes[0]));
  }

  //put(item: Impact): Observable<Impact> {
  //  var url = this.getUrl("api/Impacts/" + item.impactId);
  //  return this.http.put<Impact>(url, item);
  //}

  put(input: Owner): Observable<Owner> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation UpdateOwner($owner: OwnerDTOInput!) {
          updateOwner(ownerDTO: $owner) {
              ownerId
              }
            }
          }
        `,
        variables: {
          owner: input
        }
      }).pipe(map((result: any) =>
        result.data.updateOwner));
  }

  //post(item: Impact): Observable<Impact> {
  //  var url = this.getUrl("api/Impacts");
  //  return this.http.post<Impact>(url, item);
  //}

  post(item: Owner): Observable<Owner> {
    return this.apollo
      .mutate({
        mutation: gql`
          mutation AddOwner($owner: OwnerDTOInput!) {
          addOwner(ownerDTO: $owner) {
            ownerId
          }
        }
        `,
        variables: {
          owner: item
        }
      }).pipe(map((result: any) =>
        result.data.addOwner));
  }

  getCategories(
    pageIndex: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: string,
    filterColumn: string | null,
    filterQuery: string | null
  ): Observable<ApiResult<Owner>> {
    var url = this.getUrl("api/Owners");
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

    return this.http.get<ApiResult<Owner>>(url, { params });
  }

  isDupeOwner(item: Owner): Observable<boolean> {
    var url = this.getUrl("api/Impacts/IsDupeOwner");
    return this.http.post<boolean>(url, item);
  }
}
