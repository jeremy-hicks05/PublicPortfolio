import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { HdTicketsComponent } from './hdtickets/hdtickets.component';
import { HdMyTicketsComponent } from './hdtickets/hdmytickets.component';
import { HdTicketEditComponent } from './hdtickets/hdticket-edit.component';
/*import { HealthCheckComponent } from './health-check/health-check.component';*/
import { HdMyTicketsApprovalComponent } from './hdtickets/hdmyticketsapproval.component';
//import { ImpactsComponent } from './impacts/impacts.component';
//import { ImpactEditComponent } from './impacts/impact-edit.component';


const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'tickets', component: HdTicketsComponent },
  { path: 'myTickets', component: HdMyTicketsComponent },
  { path: 'ticketsThatNeedMyApproval', component: HdMyTicketsApprovalComponent },
  { path: 'hdTicket/:id', component: HdTicketEditComponent },
  { path: 'mtaIntranet/ticket/:id', component: HdTicketEditComponent },
  { path: 'ticket', component: HdTicketEditComponent },
  //{ path: 'impacts', component: ImpactsComponent },
  //{ path: 'impact/:id', component: ImpactEditComponent },
  //{ path: 'impact', component: ImpactEditComponent },
/*  { path: 'health-check', component: HealthCheckComponent },*/
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
