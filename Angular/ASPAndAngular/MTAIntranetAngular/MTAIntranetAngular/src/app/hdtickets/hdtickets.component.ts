import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';

import { environment } from '../../environments/environment';

import { MatTableDataSource } from '@angular/material/table'
import { MatPaginator, PageEvent } from '@angular/material/paginator'
import { MatSort } from '@angular/material/sort';

import { HdTicket } from './hdticket';
import { HdTicketDTO } from './hdticketDTO';
import { HdTicketService } from './hdticket.service';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-hdtickets',
  templateUrl: './hdtickets.component.html',
  styleUrls: ['./hdtickets.component.scss']
})
export class HdTicketsComponent implements OnInit {
  public displayedColumns: string[] =
    ['id',
      'title',
      'created',
      'ownerId',
      'hdQueueId',
      'submitterId',
      'summary',
      'hdCategoryId',
      'hdStatusId',
      'hdImpactId',
      'hdPriorityId'
    ];
  public hdTickets!: MatTableDataSource<HdTicketDTO>;

  defaultPageIndex: number = 0;
  defaultPageSize: number = 10;
  public defaultSortColumn: string = "id";
  public defaultSortOrder: "asc" | "desc" = "asc";

  defaultFilterColumn: string = "id";
  filterQuery?: string;

  @ViewChild(MatPaginator)
  paginator!: MatPaginator;

  @ViewChild(MatSort)
  sort!: MatSort;

  constructor(private hdTicketService: HdTicketService) {
  }

  ngOnInit() {
    this.loadData();
  }

  loadData(query?: string) {
    var pageEvent = new PageEvent();
    pageEvent.pageIndex = this.defaultPageIndex;
    pageEvent.pageSize = this.defaultPageSize;
    this.filterQuery = query;
    this.getData(pageEvent);
  }

  getData(event: PageEvent) {
    var sortColumn = (this.sort)
      ? this.sort.active
      : this.defaultSortColumn;

    var sortOrder = (this.sort)
      ? this.sort.direction
      : this.defaultSortOrder;

    var filterColumn = (this.filterQuery)
      ? this.defaultFilterColumn
      : null;

    var filterQuery = (this.filterQuery)
      ? this.filterQuery
      : null;

    this.hdTicketService.getData(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery)

      .subscribe(result => {
        console.log(result);
        this.paginator.length = result.totalCount;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.hdTickets = new MatTableDataSource<HdTicketDTO>(result.data);
      }, error => console.error(error));

    //this.hdTicketService.getMyData(
    //  event.pageIndex,
    //  event.pageSize,
    //  sortColumn,
    //  sortOrder,
    //  filterColumn,
    //  filterQuery,
    //  this.windowsCurrentUser)

    //  .subscribe(result => {
    //    console.log(result);
    //    this.paginator.length = result.totalCount;
    //    this.paginator.pageIndex = result.pageIndex;
    //    this.paginator.pageSize = result.pageSize;
    //    this.hdTickets = new MatTableDataSource<HdTicketDTO>(result.data);
    //  }, error => console.error(error));
  }
}
