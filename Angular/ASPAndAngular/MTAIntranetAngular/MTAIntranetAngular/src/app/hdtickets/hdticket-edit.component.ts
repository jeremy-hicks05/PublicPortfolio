import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { environment } from './../../environments/environment';

/*import { Ticket } from './ticket';*/
import { HdImpact } from '../hdimpacts/hdimpact';
import { MatDatepicker } from '@angular/material/datepicker';
import { DatePipe } from '@angular/common';
import { MatFormField } from '@angular/material/form-field';
import { HdCategory } from '../hdcategories/hdcategory';
import { HdQueue } from '../hdqueues/hdqueue';
import { HdStatus } from '../hdstatuses/hdstatus';
import { HdPriority } from '../hdpriorities/hdpriority';
import { UserService } from '../services/user.service';
import { HdTicket } from './hdticket';
import { HdTicketChangeDTO } from '../hdticketchange/hdticketchangeDTO';
import { HdField } from '../fields/hdfield';
import { HdCustomField } from '../fields/hdcustomfield';

@Component({
  selector: 'app-hdticket-edit',
  templateUrl: './hdticket-edit.component.html',
  styleUrls: ['./hdticket-edit.component.scss']
})
export class HdTicketEditComponent implements OnInit {
  // the view title
  viewTitle?: string;

  // the form model
  form!: FormGroup;

  // the ticket object to edit
  hdTicket?: HdTicket;

  // the ticket object id, as fetched from the active route:
  // it's NULL when we're adding a new ticket
  // and not NULL when we're editing an existing one
  id?: number;

  //@ViewChild(MatDatepicker)
  //datePicker!: MatDatepicker<any>;

  //today = new Date();

  // the arrays for the select for the foreign keys
  /*owners?: Owner[];*/
  hdQueues?: HdQueue[];
  selectedQueue?: number;
  //approverId?: number | undefined;
  hdCategories?: HdCategory[];
  hdStatuses?: HdStatus[];
  hdImpacts?: HdImpact[];
  hdPriorities?: HdPriority[];
  hdFields?: HdField[];
  hdCustomTextFields?: HdField[];
  hdCustomCheckboxFields?: HdField[];
  hdCustomDateFields?: HdField[];
  hdAllFields?: HdField[];
  hdReadOnlyCustomFields?: HdCustomField[];
  hdFieldsCustom?: HdField[];
  hdCustomFields?: HdCustomField[];
  //hdCustomTextFields?: HdCustomField[];
  //hdCustomDateFields?: HdCustomField[];
  hasDueDate?: boolean;
  dueDateLabel?: string;

  hdTicketChanges?: HdTicketChangeDTO[];

  createButtonClicked?: boolean;

  /*userService!: UserService;*/
  windowsCurrentUser!: string;

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    userService: UserService
    //,
    //public datePipe: DatePipe
  ) {
    this.createButtonClicked = false;
    userService.get().subscribe(username => {
      this.windowsCurrentUser = username;
      //alert(this.windowsCurrentUser);
      this.loadData();
    });
    //this.loadData();
  }

  ngOnInit() {
    this.form = new FormGroup({
      id: new FormControl(''),
      title: new FormControl({ value: '', disabled: true }, Validators.required),
      owner_id: new FormControl({ value: '', disabled: true }, Validators.required),
      hdQueueId: new FormControl('', Validators.required),
      submitter_id: new FormControl({ value: '', disabled: true }, Validators.required),
      summary: new FormControl({ value: '', disabled: true }, Validators.required),
      category_id: new FormControl({ value: null, disabled: true }, Validators.required),
      status_id: new FormControl({ value: '', disabled: true }, Validators.required),
      impact_id: new FormControl({ value: '', disabled: true }, Validators.required),
      priority_id: new FormControl({ value: '', disabled: true }, Validators.required),
      machine_id: new FormControl({ value: '', disabled: true }, Validators.required),
      //approverId: new FormControl({ value: '', disabled: true }),
      due_date: new FormControl({ value: '', disabled: false }), // not required
      //firstName: new FormControl({ value: '', disabled: true }), // not required
      //lastName: new FormControl({ value: '', disabled: true }), // not required
      //userToMirror: new FormControl({ value: '', disabled: true }), // not required
      assetd_id: new FormControl({ value: '', disabled: true }), // not required
      //custom_0: new FormControl({ value: '', disabled: false }), // not required
      custom_1: new FormControl({ value: '', disabled: false }), // not required
      custom_2: new FormControl({ value: '', disabled: false }), // not required
      custom_3: new FormControl({ value: '', disabled: false }), // not required
      custom_4: new FormControl({ value: '', disabled: false }), // not required
      custom_5: new FormControl({ value: '', disabled: false }), // not required
      custom_6: new FormControl({ value: '', disabled: false }), // not required
      custom_7: new FormControl({ value: '', disabled: false }), // not required
      custom_8: new FormControl({ value: '', disabled: false }), // not required
      custom_9: new FormControl({ value: '', disabled: false }), // not required
      custom_10: new FormControl({ value: '', disabled: false }), // not required
      custom_11: new FormControl({ value: '', disabled: false }), // not required
      custom_12: new FormControl({ value: '', disabled: false }), // not required
      custom_13: new FormControl({ value: '', disabled: false }), // not required
      custom_14: new FormControl({ value: '', disabled: false }), // not required
      custom_15: new FormControl({ value: '', disabled: false }), // not required
      comment: new FormControl({ value: '', disabled: false }) // not required
    });
    //this.loadData();
    //this.setSubmitterId();
    //this.setOwnerId();
  }

  findIdFromWindowsAccount(windowsAccountName: string) {
    // look up user in KACE and return ID
    var unWithoutDomain = "";
    if (windowsAccountName != null) {
      var unWithoutDomain = windowsAccountName.toString().split("\\")[1];

      var url = environment.baseUrl + 'api/Users/FromName/' + unWithoutDomain;

      // set submitter id
      this.http.get<number>(url).subscribe({
        next: (result) => {
          this.setSubmitterId(result);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Request completed successfully'),
      });


      // set device id using PRIMARY_DEVICE_ID column
      url = environment.baseUrl + 'api/Users/Device/FromName/' + unWithoutDomain;

      // set device id
      this.http.get<number>(url).subscribe({
        next: (result) => {
          this.setDeviceId(result);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Device ID set successfully'),
      });

    }
    else {
      console.log("username not loaded...");
    }
  }

  loadData() {
    //this.loadQueues();
    this.loadQueuesWithUsername(this.windowsCurrentUser);

    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');

    this.id = idParam ? +idParam : 0;

    if (this.id) {
      // EDIT MODE

      // get list of hdTicketChange(s) / comments
      var url = environment.baseUrl + 'api/HDTicketChanges/ForTicket/' + this.id;
      this.http.get<HdTicketChangeDTO[]>(url).subscribe({
        next: (result) => {
          this.hdTicketChanges = result;
          // alert(result[0].timestamp);
        },
        error: (error) => console.error(error),
        complete: () => console.log('Ticket changes retrieved successfully'),
      });


      // fetch the ticket from the server
      var url = environment.baseUrl + 'api/HDTickets/' + this.id;
      this.http.get<HdTicket>(url).subscribe({
        next: (result) => {
          this.hdTicket = result;
          this.viewTitle = "Edit Ticket # - " + this.hdTicket.id;

          // Update the form with the ticket values
          this.queueSelected(this.hdTicket.hdQueueId);
          // this.form.patchValue(this.hdTicket); // Uncomment if patchValue is preferred
          this.form.controls['category_id'].setValue(this.hdTicket.hdCategoryId);
          this.categorySelected(this.hdTicket.hdQueueId, this.hdTicket.hdCategoryId);

          this.form.controls['impact_id'].setValue(this.hdTicket.hdImpactId);
          this.form.controls['priority_id'].setValue(this.hdTicket.hdPriorityId);
          this.form.controls['title'].setValue(this.hdTicket.title);
          this.form.controls['summary'].setValue(this.hdTicket.summary);
          this.form.controls['owner_id'].setValue(this.hdTicket.ownerId);
          this.form.controls['due_date'].setValue(this.hdTicket.dueDate);
          this.form.controls['custom_1'].setValue(this.hdTicket.customFieldValue0);
          this.form.controls['custom_2'].setValue(this.hdTicket.customFieldValue1);
          this.form.controls['custom_3'].setValue(this.hdTicket.customFieldValue2);
          this.form.controls['custom_4'].setValue(this.hdTicket.customFieldValue3);
          this.form.controls['custom_5'].setValue(this.hdTicket.customFieldValue4);
          this.form.controls['custom_6'].setValue(this.hdTicket.customFieldValue5);
          this.form.controls['custom_7'].setValue(this.hdTicket.customFieldValue6);
          this.form.controls['custom_8'].setValue(this.hdTicket.customFieldValue7);
          this.form.controls['custom_9'].setValue(this.hdTicket.customFieldValue8);
          this.form.controls['custom_10'].setValue(this.hdTicket.customFieldValue9);

          this.loadFields(this.hdTicket.hdQueueId);
          this.disableQueue();
          this.disableForm();
        },
        error: (error) => console.error(error),
        complete: () => console.log('Ticket loaded and form updated successfully'),
      });
    }
    else {
      // ADD NEW MODE

      this.viewTitle = "Create a new Ticket";
    }
  }

  setSubmitterId(submitterId: number) {
    // set submitter id to User ID that matches logged in user's email/username
    this.form.controls['submitter_id'].setValue(submitterId);
  }

  setDeviceId(deviceId: number) {
    // set submitter id to User ID that matches logged in user's email/username
    // get Device ID for submitter ID from API
    this.form.controls['machine_id'].setValue(deviceId);
  }

  setOwnerId(ownerId: number) {
    // set owner id to the ID of the default owner of the queue and category, set in KACE
    this.form.controls['owner_id'].setValue(ownerId);
  }

  setPriorityId(priorityId: number) {
    // set owner id to the ID of the default owner of the queue and category, set in KACE
    this.form.controls['priority_id'].setValue(priorityId);
  }

  enableTitle() {
    this.form.controls['title'].enable();
  }

  queueSelected(queueId: number) {
    // TODO: Hide all inputs, as though the form was resetting
    this.selectedQueue = queueId;

    //this.ngOnInit();
    //this.disableForm();
    //this.clearForm();
    this.form.controls['hdQueueId'].setValue(queueId);

    this.loadCategories(queueId);

    this.loadFields(queueId);
    //alert('Category value is :' + this.form.controls['hdCategoryId'].value);
  }

  disableForm() {
    this.disableCategory();
    this.disableTitle();
    //this.disableFirstName();
    //this.disableLastName();
    //this.disableUserToMirror();
    this.disableDueDate();
    this.disableImpact();
    this.disablePriority();
    this.disableSummary();
    this.disableCustomFields();
  }

  clearForm() {
    this.clearCategory();
    //this.clearTitle();
    //this.clearDueDate();
    //this.clearSummary();
  }

  disableCustomFields() {
    this.form.controls['custom_1'].disable();
    this.form.controls['custom_2'].disable();
    this.form.controls['custom_3'].disable();
    this.form.controls['custom_4'].disable();
    this.form.controls['custom_5'].disable();
    this.form.controls['custom_6'].disable();
    this.form.controls['custom_7'].disable();
    this.form.controls['custom_8'].disable();
    this.form.controls['custom_9'].disable();
    this.form.controls['custom_10'].disable();
    this.form.controls['custom_11'].disable();
    this.form.controls['custom_12'].disable();
    this.form.controls['custom_13'].disable();
    this.form.controls['custom_14'].disable();
    this.form.controls['custom_15'].disable();
  }

  disableCheckBoxes() {
    // TODO: foreach loop over checkbox elemets, disable each one

  }

  disableQueue() {
    this.form.controls['hdQueueId'].disable();
  }

  enableQueue() {
    this.form.controls['hdQueueId'].enable();
  }

  categorySelected(queueId: number | undefined, categoryId: number) {
    this.setOwner(categoryId);
    this.loadImpacts(queueId);
    this.loadPriorities(queueId);
    this.setStatusToDefault(queueId);
    this.enableTitle();
    this.enableSummary();
    this.form.controls['submitter_id'].setValue(this.findIdFromWindowsAccount(this.windowsCurrentUser));
    //this.form.controls['summary'].setValue(
    //  (this.getDefaultInfo(categoryId) == "" ?
    //    this.form.controls['summary'].value :
    //    this.getDefaultInfo(categoryId)));
  }

  impactSelected(queueId: number | undefined, impactId: number) {
    //alert('Loading priority based on impact for impact ' + impactId + ' and queueId ' + queueId);
    //this.setPriorityId(
      this.getPriority(queueId, impactId);
    //);
  }

  textAreaAdjust(element: any) {
    element.style.height = "1px";
    element.style.height = (25 + element.scrollHeight) + "px";
  }

  //impactSelected(impactId: number, queueId: number | undefined) {
  //  this.setPriority(impactId, queueId);
  //}

  enableCategoryy() {
    this.form.controls['category_id'].enable();
  }

  disableCategory() {
    this.form.controls['category_id'].disable();
  }

  clearCategory() {
    this.form.controls['category_id'].reset();
  }

  disableTitle() {
    this.form.controls['title'].disable();
  }

  clearTitle() {
    this.form.controls['title'].reset();
  }

  enableSummary() {
    this.form.controls['summary'].enable();
  }

  disableSummary() {
    this.form.controls['summary'].disable();
  }

  clearSummary() {
    this.form.controls['summary'].reset();
  }

  disableImpact() {
    this.form.controls['impact_id'].disable();
  }

  disablePriority() {
    this.form.controls['priority_id'].disable();
  }

  //enableFirstName() {
  //  this.form.controls['firstName'].enable();
  //}

  //enableLastName() {
  //  this.form.controls['lastName'].enable();
  //}

  //enableUserToMirror() {
  //  this.form.controls['userToMirror'].enable();
  //}

  //disableFirstName() {
  //  this.form.controls['firstName'].disable();
  //}

  //disableLastName() {
  //  this.form.controls['lastName'].disable();
  //}

  //disableUserToMirror() {
  //  this.form.controls['userToMirror'].disable();
  //}

  //enableDueDate() {
  //  this.form.controls['due_date'].enable();
  //}

  disableDueDate() {
    this.form.controls['due_date'].disable();
    //this.form.controls[this.dueDateLabel!.toString()].disable();
  }

  //clearDueDate() {
  //  this.form.controls['due_date'].setValue(null);
  //}

  setOwner(categoryId: number) {
    // set default Owner ID from category based on category ID
    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<number>(environment.baseUrl + 'api/HDCategories/' + categoryId + '/GetDefaultOwner').subscribe(
      result => {
        //alert(result);
        this.setOwnerId(result);
      }, error => console.error(error));
  }

  getPriority(queueId: number|undefined, impactId: number) {
    this.http.get<number>(environment.baseUrl + 'api/HDPriorities/GetPriority/' + queueId + '/' + impactId).subscribe(
      result => {
        //alert(result);
        this.setPriorityId(result);
      }, error => console.error(error));
  }

  //setPriority(impactId: number) {

  //  // get "name" of impact (many users cannot work)

  //  this.http.get<number>(environment.baseUrl + 'api/HDPriorities/Name/' + impactId + '/Queue/' + queueId).subscribe(
  //    result => {
  //      // set priority based on "name" of impact
  //      this.setPriorityId(result);
  //    }, error => console.error(error));
  //}

  //setPriorityId(impactId: number) {
  //  switch (impactName) {
  //    case "Many people cannot work":
  //      break;
  //      //this.setPriorityToHigh(queueId)
  //    default:
  //      break;
  //  }
  //}

  setStatusToOpen(queueId: number | undefined) {
    // set default Owner ID from category based on category ID
    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<number>(environment.baseUrl + 'api/HDStatus/OpenStatusFromQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.setStatusId(result);
      }, error => console.error(error));
  }

  setStatusToDefault(queueId: number | undefined) {
    // set default Owner ID from category based on category ID
    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<number>(environment.baseUrl + 'api/HDStatus/DefaultStatusFromQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.setStatusId(result);
      }, error => console.error(error));
  }

  setStatusToNew(queueId: number) {
    // set default Owner ID from category based on category ID
    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<number>(environment.baseUrl + 'api/HDStatus/NewStatusFromQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.setStatusId(result);
      }, error => console.error(error));
  }

  loadCategories(queueId: number) {
    //var url = environment.baseUrl + 'api/Categories';
    this.form.controls['category_id'].reset();
    this.form.controls['category_id'].enable();

    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<HdCategory[]>(environment.baseUrl + 'api/HDCategories/FromQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.hdCategories = result;
      }, error => console.error(error));
  }

  loadPriorities(queueId: number | undefined) {
    //var url = environment.baseUrl + 'api/Categories';
    // TODO
    //this.form.controls['priority_id'].enable();
    var params = new HttpParams();

    //alert("Trying KACE API for categories with queue " + queueId);
    this.http.get<HdPriority[]>(environment.baseUrl + 'api/HDPriorities/FromQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.hdPriorities = result;
      }, error => console.error(error));
  }

  //loadStatuses(queueId: number) {
  //  //var url = environment.baseUrl + 'api/Categories';
  //  //this.form.controls['statusId'].enable();
  //  var params = new HttpParams();

  //  //alert("Trying KACE API for statuses with queue " + queueId);
  //  this.http.get<Status[]>(environment.baseUrl + 'api/HDStatus/FromQueue/' + queueId).subscribe(
  //    result => {
  //      //alert(result);
  //      this.statuses = result;
  //    }, error => console.error(error));
  //}

  loadQueues() {
    var params = new HttpParams();

    //alert("Trying KACE API for queues");
    this.http.get<HdQueue[]>(environment.baseUrl + 'api/HDQueues').subscribe(
      result => {
        //alert(result);
        this.hdQueues = result;
      }, error => console.error(error));
  }

  loadFields(queueId: number) {
    var params = new HttpParams();

    //alert("Trying KACE API for queues");
    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/ForQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.hdAllFields = result;
        //this.hasDueDate = this.hdAllFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<HdCustomField[]>(environment.baseUrl + 'api/HDCustomFields/ReadOnly/ForQueue/' + queueId).subscribe(
      result => {
        //alert(result);
        this.hdReadOnlyCustomFields = result;
        //this.hasDueDate = this.hdAllFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<boolean>(environment.baseUrl + 'api/HDFields/HasDueDate/' + queueId).subscribe(
      result => {
        this.hasDueDate = result;
      }, error => console.error(error));

    this.http.get(environment.baseUrl + 'api/HDFields/DueDateLabel/' + queueId, { responseType: 'text' }).subscribe(
      result => {
        this.dueDateLabel = result;
      }, error => console.error(error));

    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/ForQueue/' + queueId + '/UserCreate').subscribe(
      result => {
        //alert(result);
        this.hdFields = result;
        //this.hasDueDate = this.hdFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/Text/ForQueue/' + queueId + '/UserCreate').subscribe(
      result => {
        //alert(result);
        this.hdCustomTextFields = result;
        //this.hasDueDate = this.hdFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/Checkbox/ForQueue/' + queueId + '/UserCreate').subscribe(
      result => {
        //alert(result);
        this.hdCustomCheckboxFields = result;
        //this.hasDueDate = this.hdFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/Date/ForQueue/' + queueId + '/UserCreate').subscribe(
      result => {
        //alert(result);
        this.hdCustomDateFields = result;
        //this.hasDueDate = this.hdFields?.find(hdf => hdf.name.toLowerCase() == "due_date") != undefined;
      }, error => console.error(error));

    this.http.get<HdField[]>(environment.baseUrl + 'api/HDFields/Custom/ForQueue/' + queueId + '/UserCreate').subscribe( // Add /UserCreate?
      result => {
        //alert(result);
        this.hdFieldsCustom = result;
      }, error => console.error(error));

    //this.http.get<HdCustomField[]>(environment.baseUrl + 'api/HDCustomFields/ForQueue/' + queueId).subscribe( // Add /UserCreate?
    //  result => {
    //    //alert(result);
    //    this.hdCustomFields = result;
    //  }, error => console.error(error));

    //this.http.get<HdCustomField[]>(environment.baseUrl + 'api/HDCustomTextFields/ForQueue/' + queueId).subscribe( // Add /UserCreate?
    //  result => {
    //    //alert(result);
    //    //this.hdCustomTextFields = result;
    //  }, error => console.error(error));

    //this.http.get<HdCustomField[]>(environment.baseUrl + 'api/HDCustomDateFields/ForQueue/' + queueId).subscribe( // Add /UserCreate?
    //  result => {
    //    //alert(result);
    //    //this.hdCustomDateFields = result;
    //  }, error => console.error(error));
  }

  loadQueuesWithUsername(userName: string) {
    var params = new HttpParams();

    if (userName != null) {
      var unWithoutDomain = userName.toString().split("\\")[1];

      var url = environment.baseUrl + 'api/Users/FromName/' + unWithoutDomain;

      // set submitter id
      this.http.get<number>(url).subscribe(result => {
        this.http.get<HdQueue[]>(environment.baseUrl +
          'api/HDQueues/WhereUserIsSubmitter/' +
          result).subscribe(
            result => {
              this.hdQueues = result;
            }, error => console.error(error));
        result;
      }, error => console.error(error));
    }
    else {
      console.log("username not loaded...");
    }
  }

  loadImpacts(queueId: number | undefined) {
    // try KACE API
    //alert("Trying KACE API for impacts");
    this.form.controls['impact_id'].enable();

    var params = new HttpParams();

    this.http.get<any>(environment.baseUrl + 'api/HDImpacts/FromQueue/' + queueId, { params }).subscribe(result => {
      this.hdImpacts = result;
    }, error => console.error(error));
  }

  setImpact(impactId: number) {
    this.form.controls['impact_id'].setValue(impactId);
  }

  setStatusId(statusId: number) {
    this.form.controls['status_id'].setValue(statusId);
  }

  createButtonResponse() {
    this.createButtonClicked = true;
  }

  getMonth(monthStr: string) {
    if ((new Date(monthStr + '-1-01').getMonth() + 1) <= 9) {
      return '0' + (new Date(monthStr + '-1-01').getMonth() + 1)
    }
    else if ((new Date(monthStr + '-1-01').getMonth() + 1) > 9) {
      return new Date(monthStr + '-1-01').getMonth() + 1
    }
    return '';
  }

  isChecked(controlName: string): boolean {
    return this.form.controls[controlName].value === '1';
  }



  onSubmit() {
    this.createButtonResponse();
    var ticket = (this.id) ? this.hdTicket : <HdTicket>{};
    if (ticket) {
      ticket.title = this.form.controls['title'].value;
      ticket.ownerId = +this.form.controls['owner_id'].value;
      ticket.hdQueueId = +this.form.controls['hdQueueId'].value;
      ticket.submitterId = +this.form.controls['submitter_id'].value;
      ticket.summary = this.form.controls['summary'].value;
      ticket.hdCategoryId = +this.form.controls['category_id'].value;
      ticket.hdStatusId = +this.form.controls['status_id'].value;
      ticket.hdImpactId = +this.form.controls['impact_id'].value;
      ticket.hdPriorityId = +this.form.controls['priority_id'].value;
      ticket.deviceId = +this.form.controls['machine_id'].value;
      ticket.dueDate = this.form.controls['due_date'].value;
      ticket.dueDateYear = this.form.controls['due_date'].value == '' ? "" : this.form.controls['due_date'].value.toString().split(" ")[3];
      ticket.dueDateMonth = this.form.controls['due_date'].value == '' ? "" : this.getMonth(this.form.controls['due_date'].value.toString().split(" ")[1]).toString();
      ticket.dueDateDay = this.form.controls['due_date'].value == '' ? "" : this.form.controls['due_date'].value.toString().split(" ")[2];
      ticket.dueDateHour = this.form.controls['due_date'].value == '' ? "" : "08";
      ticket.dueDate = this.form.controls['due_date'].value == '' ? "" : (
        this.form.controls['due_date'].value.toString().split(" ")[3] + '-' +
        this.getMonth(this.form.controls['due_date'].value.toString().split(" ")[1]) + '-' + // TODO: get month to always be two numbers, like 05 or 11
        this.form.controls['due_date'].value.toString().split(" ")[2]
        + ' 08:00:00'
      );
      // TODO: Translate any/all of these to YYYY-MM-DD HH:mm:SS value, if needed - check for
      // regex that matches a dateTime?  or can we check the type of custom_x??
      ticket.customFieldValue1 = this.form.controls['custom_1'].value == '' ? '' : this.form.controls['custom_1'].value;
      ticket.customFieldValue2 = this.form.controls['custom_2'].value == '' ? '' : this.form.controls['custom_2'].value;
      ticket.customFieldValue3 = this.form.controls['custom_3'].value == '' ? '' : this.form.controls['custom_3'].value;
      //if (this.hdCustomDateFields != null && this.hdCustomDateFields.find(cdf => cdf.name.toLowerCase() == 'custom_3')) {
      //  ticket.customFieldValue3 = this.form.controls['custom_3'].value == '' ? "" : (
      //    this.form.controls['custom_3'].value.toString().split(" ")[3] + '-' +
      //    this.getMonth(this.form.controls['custom_3'].value.toString().split(" ")[1]) + '-' + // TODO: get month to always be two numbers, like 05 or 11
      //    this.form.controls['custom_3'].value.toString().split(" ")[2]
      //    + ' 08:00:00');
      //}
      ticket.customFieldValue4 = this.form.controls['custom_4'].value == '' ? '' : this.form.controls['custom_4'].value;
      ticket.customFieldValue5 = this.form.controls['custom_5'].value == '' ? '' : this.form.controls['custom_5'].value;
      ticket.customFieldValue6 = this.form.controls['custom_6'].value == '' ? '' : this.form.controls['custom_6'].value;
      ticket.customFieldValue7 = this.form.controls['custom_7'].value == '' ? '' : this.form.controls['custom_7'].value;
      ticket.customFieldValue8 = this.form.controls['custom_8'].value == '' ? '' : this.form.controls['custom_8'].value;
      ticket.customFieldValue9 = this.form.controls['custom_9'].value == '' ? '' : this.form.controls['custom_9'].value;
      ticket.customFieldValue10 = this.form.controls['custom_10'].value == '' ? '' : this.form.controls['custom_10'].value;
      ticket.customFieldValue11 = this.form.controls['custom_11'].value == '' ? '' : this.form.controls['custom_11'].value;
      ticket.customFieldValue12 = this.form.controls['custom_12'].value == '' ? '' : this.form.controls['custom_12'].value;
      ticket.customFieldValue13 = this.form.controls['custom_13'].value == '' ? '' : this.form.controls['custom_13'].value;
      ticket.customFieldValue14 = this.form.controls['custom_14'].value == '' ? '' : this.form.controls['custom_14'].value;
      ticket.customFieldValue15 = this.form.controls['custom_15'].value == '' ? '' : this.form.controls['custom_15'].value;
      //ticket.firstName = this.form.controls['firstName'].value == null ? "" : this.form.controls['firstName'].value;
      //ticket.lastName = this.form.controls['lastName'].value == null ? "" : this.form.controls['lastName'].value;
      //ticket.userToMirror = this.form.controls['userToMirror'].value == null ? "" : this.form.controls['userToMirror'].value;

      if (this.id) {
        // EDIT mode
        var newComment = this.form.controls['comment'].value;
        var commentUpdateUrl = 'https://mtadev.mta-flint.net:8000/addComment/' + this.id + '/' + newComment;

        this.http
          .post<HdTicket>(commentUpdateUrl, null) // put?
          .subscribe(result => {
            console.log("Added comment");
            this.router.navigate(['/myTickets/']);

          }, error => console.error(error));

        //location.reload();

        // update standard request
        //var url = 'https://mtadev.mta-flint.net:8000/ticket/putStandardRequestTicket/' + ticket.id
        //  + '/' + ticket.title + '/' + ticket.ownerId + '/' + ticket.submitterId
        //  + '/' + ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' + ticket.hdImpactId
        //  + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;

        //this.http
        //  .post<HdTicket>(url, ticket)
        //  .subscribe(result => {
        //    console.log("Ticket " + result?.id + " has been updated.");
        //    this.router.navigate(['/myTickets']);

        //  }, error => console.error(error));
      }
      else {
        // ADD NEW mode
        // TODO: Make this dynamic
        var url = 'https://mtadev.mta-flint.net:8000/ticket/'



        //  curl - X POST "http://127.0.0.1:8000/tickets/" - H "Content-Type: application/json" - d '{
        //  "title": "Server Down",
        //    "summary": "The main server is down and needs immediate attention.",
        //      "category": 1,
        //        "impact": 2,
        //          "status": 3,
        //            "owner": 4,
        //              "submitter": 5,
        //                "approval_info": "Awaiting approval",
        //                  "priority": 1,
        //                    "asset": 123,
        //                      "machine": 456,
        //                        "custom_1": "Custom field 1 value",
        //                          "custom_2": "Custom field 2 value",
        //                            "custom_3": "Custom field 3 value",
        //                              "custom_4": "Custom field 4 value",
        //                                "due_date": "2024-06-01T12:00:00",
        //                                  "cc_list": ["admin@example.com", "support@example.com"],
        //                                    "resolution": "Pending resolution"
        //} '


        //if (ticket.hdQueueId == 13) { // Project
        //  url += 'newProjectTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //}
        //else if (ticket.hdQueueId == 10) { // New Manager
        //  url += 'newManagerTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId + '/' +
        //    ticket.dueDateYear + '/' + ticket.dueDateMonth + '/' + ticket.dueDateDay + '/' +
        //    ticket.dueDateHour + '/' + ticket.firstName + '/' + ticket.lastName + '/' +
        //    ticket.userToMirror;
        //}
        //else if (ticket.hdQueueId == 9) { // Termination
        //  url += 'terminationTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId + '/' +
        //    ticket.dueDateYear + '/' + ticket.dueDateMonth + '/' + ticket.dueDateDay + '/' +
        //    ticket.dueDateHour + '/' + ticket.firstName + '/' + ticket.lastName;
        //}
        //else if (ticket.hdQueueId == 8) { // Security
        //  url += 'securityTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //}
        //else if (ticket.hdQueueId == 7) { // Outage
        //  url += 'outageTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //}
        //else if (ticket.hdQueueId == 6) { // Standard Request
        //  url += 'standardRequestTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //}
        //else if (ticket.hdQueueId == 1) { // Helpdesk
        //  url += 'helpdeskTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //} // TODO: Add FuelMaster post
        //else { // Null
        //  url += 'defaultTicket/'
        //    + ticket.title + '/' +
        //    ticket.ownerId + '/' + ticket.hdQueueId + '/' + ticket.submitterId + '/' +
        //    ticket.summary + '/' + ticket.hdCategoryId + '/' + ticket.hdStatusId + '/' +
        //    ticket.hdImpactId + '/' + ticket.hdPriorityId + '/' + ticket.deviceId;
        //}

        //createTicket(ticketId: number, ticket: HdTicket): Observable < any > {
        //  return this.http.post(`${this.apiUrl}/${ticketId}`, ticket);
        //}

        url += 'newDynamicTicket/' +
          ticket.hdQueueId + '/' +
          ticket.title + '/' +
          ticket.summary + '/' +
          ticket.hdCategoryId + '/' +
          ticket.hdImpactId + '/' +
          ticket.hdStatusId + '/' +
          ticket.ownerId + '/' +
          ticket.submitterId + '/' +
          ticket.hdPriorityId + '/' +
          ticket.deviceId + '/' +
          //(ticket.customFieldValue0 != '' ? ticket.customFieldValue0 + '/' : '') +
          (ticket.customFieldValue1 != '' ? ticket.customFieldValue1 + '/' : '') +
          (ticket.customFieldValue2 != '' ? ticket.customFieldValue2 + '/' : '') +
          (ticket.customFieldValue3 != '' ? ticket.customFieldValue3 + '/' : '') +
          (ticket.customFieldValue4 != '' ? ticket.customFieldValue4 + '/' : '') +
          (ticket.customFieldValue5 != '' ? ticket.customFieldValue5 + '/' : '') +
          (ticket.customFieldValue6 != '' ? ticket.customFieldValue6 + '/' : '') +
          (ticket.customFieldValue7 != '' ? ticket.customFieldValue7 + '/' : '') +
          (ticket.customFieldValue8 != '' ? ticket.customFieldValue8 + '/' : '') +
          (ticket.customFieldValue9 != '' ? ticket.customFieldValue9 + '/' : '') +
          (ticket.customFieldValue10 != '' ? ticket.customFieldValue10 + '/' : '') +
          (ticket.customFieldValue11 != '' ? ticket.customFieldValue11 + '/' : '') +
          (ticket.customFieldValue12 != '' ? ticket.customFieldValue12 + '/' : '') +
          (ticket.customFieldValue13 != '' ? ticket.customFieldValue13 + '/' : '') +
          (ticket.customFieldValue14 != '' ? ticket.customFieldValue14 + '/' : '') +
          (ticket.customFieldValue15 != '' ? ticket.customFieldValue15 + '/' : '') +
          (ticket.dueDateYear != '' ? ticket.dueDateYear + '/' : '') +
          (ticket.dueDateMonth != '' ? ticket.dueDateMonth + '/' : '') +
          (ticket.dueDateDay != '' ? ticket.dueDateDay + '/' : '') +
          (ticket.dueDateHour != '' ? ticket.dueDateHour : '');

        this.http
          .post<HdTicket>(url, null)
          .subscribe(result => {
            //console.log("Ticket " + result.ticketId + " has been created.");
            alert("Ticket has been created!");
            // TODO: Send to "My Tickets" View
            //this.router.navigate(['/api/Tickets/SendTicketInfo/' + result?.ticketId]);
            this.router.navigate(['/myTickets']);
          },
            error =>
              console.error(error));
      }

    }
  }

  //approveTicket(ticketId: number, queueId: number) {
  //  var approveTicketUrl = 'https://mtadev.mta-flint.net:8000/approveTicket/' + ticketId;

  //  this.http
  //    .post<HdTicket>(approveTicketUrl, null) // put?
  //    .subscribe(result => {
  //      this.router.navigate(['/myTickets/']);

  //    }, error => console.error(error));
  //}

  getDefaultInfo(categoryId: number): string {
    switch (categoryId) {
      // Terminations
      case 146: // Coordinator Termination
        return "Disable AD Account\nDeactivate Badge\nDisable Kronos\nRemove Phone Extension\nDisable Email\nDisable Trapeze Access\nDisable Trapeze Employee\n";
      case 145: // Driver Termination
        return "Deactivate Badge\nDisable Kronos\n";
      case 144: // Management Termination
        return "Disable AD Account\nDeactivate Badge\nDisable Kronos\nRemove Phone Extension\nRemove Fuelmaster Access\nDisable Email\nDisable Trapeze Access\nDisable Trapeze Employee\n";
      case 130: // Standard Termination
        return "Deactivate Badge\nDisable Kronos\n";
      // End Terminations
      default:
        return "";
    }
  }
}
