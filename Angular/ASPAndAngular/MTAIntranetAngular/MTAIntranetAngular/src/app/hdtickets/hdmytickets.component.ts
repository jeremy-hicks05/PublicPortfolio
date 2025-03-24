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
import { Router } from '@angular/router';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-hdmytickets',
  templateUrl: './hdmytickets.component.html',
  styleUrls: ['./hdmytickets.component.scss']
})
export class HdMyTicketsComponent implements OnInit {
  windowsCurrentUser!: string;
  userName!: string;
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
      'hdPriorityId',
      'approverId'
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

  constructor(private hdTicketService: HdTicketService,
    private router: Router,
    private http: HttpClient,
    userService: UserService) {
      userService.get().subscribe(username => {
        this.windowsCurrentUser = username;
        this.userName = username.split("\\")[1];
        this.loadData();
        //alert(this.windowsCurrentUser);
      });
    //this.loadData();
  }

  ngOnInit() {
    /*this.loadData();*/
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

    //this.hdTicketService.getData(
    //  event.pageIndex,
    //  event.pageSize,
    //  sortColumn,
    //  sortOrder,
    //  filterColumn,
    //  filterQuery)

    //  .subscribe(result => {
    //    console.log(result);
    //    this.paginator.length = result.totalCount;
    //    this.paginator.pageIndex = result.pageIndex;
    //    this.paginator.pageSize = result.pageSize;
    //    this.hdTickets = new MatTableDataSource<HdTicketDTO>(result.data);
    //  }, error => console.error(error));

    this.hdTicketService.getMyData(
      event.pageIndex,
      event.pageSize,
      sortColumn,
      sortOrder,
      filterColumn,
      filterQuery,
      this.windowsCurrentUser)

      .subscribe({
        next: (result) => {
          console.log(result);
          this.paginator.length = result.totalCount;
          this.paginator.pageIndex = result.pageIndex;
          this.paginator.pageSize = result.pageSize;
          this.hdTickets = new MatTableDataSource<HdTicketDTO>(result.data);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Subscription complete')
      });

  }

  approveTicket(ticketId: number, queueId: number) {

    this.http
      .get<number>(`${environment.baseUrl}api/HDStatus/OpenStatusFromQueue/${queueId}`)
      .pipe(
        switchMap((openStatusId) => {
          const setTicketToOpenUrl = `https://mtadev.mta-flint.net:8000/setTicketToOpen/${ticketId}/${openStatusId}`;
          return this.http.post<HdTicket>(setTicketToOpenUrl, null);
        })
      )
      .subscribe({
        next: () => {
          this.router.navigate([`/hdTicket/${ticketId}`]);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Request chain complete'),
      });


    var approveTicketUrl = 'https://mtadev.mta-flint.net:8000/approveTicket/' + ticketId;
    
    this.http
      .post<HdTicket>(approveTicketUrl, null)
      .subscribe({
        next: (result) => {
          // Uncomment this line if navigation is required
          // this.router.navigate(['/myTickets/']);
          console.log(result);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Approval process completed'),
      });
  }
}
