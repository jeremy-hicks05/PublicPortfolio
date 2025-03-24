# KACE_API.py
import datetime
import json
from os import getlogin
from pickle import NONE
import string
import logging
import requests

from flask import jsonify
from flask_cors import CORS, cross_origin
from fastapi.middleware.cors import CORSMiddleware

from fastapi import FastAPI, status

from typing import List, Optional
from datetime import datetime

# Configure logging
logging.basicConfig(filename='C:\\Users\\jhicks\\Desktop\\Python\\kace_api.log',
                    level=logging.INFO,
                    format='%(asctime)s - %(levelname)s - %(message)s')

app = FastAPI()

# CORS(app)
# cors = CORS(app, resource={
#     r"/*":{
#         "origins": "*"}})

origins = [
    "http://localhost:4200",
    "https://mtadev.mta-flint.net"]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"]
    )


from typing import List, Optional
from datetime import datetime
from pydantic import BaseModel

def login():
    username = 'un'
    password = 'pw'
    org_name = 'Default'
    #org_name = 'ORG1'

    session = requests.session()
    session.params = {'password': password,
                        'userName': username,
                        'organizationName': org_name
                        }
    session.headers = {'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'x-dell-api-version': '13'}

    login_path = 'http://192.168.122.33/ams/shared/api/security/login'

    login = session.post(
        login_path, 
        headers=session.headers, 
        data=json.dumps(session.params))
    
    return login.cookies

def setHeaders():
    return {'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'x-dell-api-version': '13'}

# API endpoint with 0 custom fields and no due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 0 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate4(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    'priority': priorityId,
                    'machine': machineId,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())  

# API endpoint with 1 custom field and no due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}")
def postDynamicTicketWithoutDueDate1(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)
        
    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 1 custom field and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    
    # try to turn year month day hour into Date object?
    # upon failure, call other function
    # upon else, call API normally
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate5(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)
        
        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 2 custom fields and no due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}")
def postDynamicTicketWithoutDueDate2(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str): 
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())    
    
# API endpoint with 2 custom fields and a due date
# termination test1    
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
        # Termination Test
    except:
        postDynamicTicketWithoutDueDate6(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())     
    
# API endpoint with 3 custom fields and no due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}")
def postDynamicTicketWithoutDueDate3(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 3 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate7(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None),
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 4 custom fields and no due date
#@app.get("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}")
def postDynamicTicketWithoutDueDate4(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int, 
                      custom_1: str, 
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 4 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int, 
                      custom_1: str, 
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str, 
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str):
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate8(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None),
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())

# API endpoint with 5 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}")
def postDynamicTicketWithoutDueDate5(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 5 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate9(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())    
    
# API endpoint with 6 custom fields and no due date
# termination test2        
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}")
def postDynamicTicketWithoutDueDate6(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 6 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate10(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 7 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}")
def postDynamicTicketWithoutDueDate7(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 7 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate11(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 8 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}")
def postDynamicTicketWithoutDueDate8(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 8 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate12(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 9 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}")
def postDynamicTicketWithoutDueDate9(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str, 
                      custom_3: str, 
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 9 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate13(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 10 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}")
def postDynamicTicketWithoutDueDate10(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())    
# API endpoint with 10 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate14(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 11 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}")
def postDynamicTicketWithoutDueDate11(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 11 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate15(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, custom_11, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'custom_11': custom_11,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 12 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}")
def postDynamicTicketWithoutDueDate12(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 12 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate16(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, custom_11, custom_12, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'custom_11': custom_11,
                    'custom_12': custom_12,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 13 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}")
def postDynamicTicketWithoutDueDate13(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 13 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate17(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, custom_11, custom_12, custom_13, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'custom_11': custom_11,
                    'custom_12': custom_12,
                    'custom_13': custom_13,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 14 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}")
def postDynamicTicketWithoutDueDate14(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 14 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate18(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, custom_11, custom_12, custom_13, custom_14, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'custom_11': custom_11,
                    'custom_12': custom_12,
                    'custom_13': custom_13,
                    'custom_14': custom_14,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())
    
# API endpoint with 15 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}")
def postDynamicTicketWithoutDueDate15(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14,
                'custom_15': custom_15
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# API endpoint with 15 custom fields and a due date
@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
def postDynamicTicket(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str,
                      dueDateYear: str, 
                      dueDateMonth: str, 
                      dueDateDay: str, 
                      dueDateHour: str): 
    try:
        year = int(dueDateYear);
        #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
    except:
        postDynamicTicketWithoutDueDate19(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, custom_3, custom_4, custom_5, custom_6, custom_7, custom_8, custom_9, custom_10, custom_11, custom_12, custom_13, custom_14, custom_15, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
    else:
        post_info = 'http://192.168.122.33/api/service_desk/tickets/'

        cookie = login()

        session3 = requests.session()
        session3.headers = setHeaders()

        session3.params = {
            'Tickets':
            [
                {
                    'hd_queue_id': queueId,
                    'title': title,
                    'summary': summary,
                    'category': categoryId,
                    'impact': impactId,
                    'status': statusId,
                    'owner': {'id': ownerId},
                    'submitter': {'id': submitterId},
                    # 'approval_info': {
                    #     'approver': { 'id' : 10 },
                    #     'approve_state' : 'opened'
                    #     },
                    'priority': priorityId,
                    #'asset': assetId,
                    'machine': machineId,
                    'custom_1': custom_1,
                    'custom_2': custom_2,
                    'custom_3': custom_3,
                    'custom_4': custom_4,
                    'custom_5': custom_5,
                    'custom_6': custom_6,
                    'custom_7': custom_7,
                    'custom_8': custom_8,
                    'custom_9': custom_9,
                    'custom_10': custom_10,
                    'custom_11': custom_11,
                    'custom_12': custom_12,
                    'custom_13': custom_13,
                    'custom_14': custom_14,
                    'custom_15': custom_15,
                    'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
                    'is_manual_due_date': int(dueDateHour != None)
                    #'cc_list' : newTicket.cc_list,
                    #'resolution': newTicket.resolution
                }
            ]
        }

        ticket = session3.post(
            post_info, 
            headers=session3.headers, 
            data=json.dumps(session3.params),
            cookies=cookie)

        logging.info(ticket)
        logging.info(ticket.json())


#16
# API endpoint with 16 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}/{custom_16}")
def postDynamicTicketWithoutDueDate16(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str,
                      custom_16: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14,
                'custom_15': custom_15,
                'custom_16': custom_16
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())      
#17
# API endpoint with 17 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}/{custom_16}/{custom_17}")
def postDynamicTicketWithoutDueDate17(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str,
                      custom_16: str,
                      custom_17: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14,
                'custom_15': custom_15,
                'custom_16': custom_16,
                'custom_17': custom_17
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
#18
# API endpoint with 18 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}/{custom_16}/{custom_17}/{custom_18}")
def postDynamicTicketWithoutDueDate18(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str,
                      custom_16: str,
                      custom_17: str,
                      custom_18: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14,
                'custom_15': custom_15,
                'custom_16': custom_16,
                'custom_17': custom_17,
                'custom_18': custom_18
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
#19
# API endpoint with 19 custom fields and no due date
#@app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{custom_3}/{custom_4}/{custom_5}/{custom_6}/{custom_7}/{custom_8}/{custom_9}/{custom_10}/{custom_11}/{custom_12}/{custom_13}/{custom_14}/{custom_15}/{custom_16}/{custom_17}/{custom_18}/{custom_19}")
def postDynamicTicketWithoutDueDate19(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
                      custom_1: str,
                      custom_2: str,
                      custom_3: str,
                      custom_4: str,
                      custom_5: str,
                      custom_6: str,
                      custom_7: str,
                      custom_8: str,
                      custom_9: str,
                      custom_10: str,
                      custom_11: str,
                      custom_12: str,
                      custom_13: str,
                      custom_14: str,
                      custom_15: str,
                      custom_16: str,
                      custom_17: str,
                      custom_18: str,
                      custom_19: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'hd_queue_id': queueId,
                'title': title,
                'summary': summary,
                'category': categoryId,
                'impact': impactId,
                'status': statusId,
                'owner': {'id': ownerId},
                'submitter': {'id': submitterId},
                # 'approval_info': {
                #     'approver': { 'id' : 10 },
                #     'approve_state' : 'opened'
                #     },
                'priority': priorityId,
                #'asset': assetId,
                'machine': machineId,
                'custom_1': custom_1,
                'custom_2': custom_2,
                'custom_3': custom_3,
                'custom_4': custom_4,
                'custom_5': custom_5,
                'custom_6': custom_6,
                'custom_7': custom_7,
                'custom_8': custom_8,
                'custom_9': custom_9,
                'custom_10': custom_10,
                'custom_11': custom_11,
                'custom_12': custom_12,
                'custom_13': custom_13,
                'custom_14': custom_14,
                'custom_15': custom_15,
                'custom_16': custom_16,
                'custom_17': custom_17,
                'custom_18': custom_18,
                'custom_19': custom_19
                #'cc_list' : newTicket.cc_list,
                #'resolution': newTicket.resolution
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())    
   
# approval Test
@app.post("/approveTicket/{ticketId}")
def approveTicket(ticketId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId) + '/approve'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'approvalData':
        [
            {
                'approvalNote': 'test approve ticket'
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())

# clear approval Test
@app.post("/clearApproval/{ticketId}")
def clearApprovalTicket(ticketId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId) + '/clearApproval'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'approvalData':
        [
            {
                'approvalNote': 'test clear approval'
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())  

# reject ticket Test
@app.post("/rejectTicket/{ticketId}")
def rejectTicket(ticketId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId) + '/reject'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'approvalData':
        [
            {
                'approvalNote': 'test rejection'
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# set ticket to open test
@app.post("/setTicketToOpen/{ticketId}/{statusId}")
def setTicketToOpen(ticketId: int, statusId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId)

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'status': statusId
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
    
# set ticket to admin approver test
@app.post("/setTicketApproverToAdmin/{ticketId}")
def setTicketApproverToAdmin(ticketId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId)

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'approver': { 'id' : 10 }
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())

# set ticket to admin approver test
@app.post("/setTicketApproverToId/{ticketId}/{approverId}")
def setTicketApproverToId(ticketId: int, approverId: int): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId)

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':
        [
            {
                'approver': { 'id' : approverId }
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())
 
# add comment POST Test
@app.post("/addComment/{ticketId}/{comment}")
def addComment(ticketId: int, comment: str): 
    
    post_info = 'http://192.168.122.33/api/service_desk/tickets/' + str(ticketId)
    # post_info = 'http://192.168.122.33/api/service_desk/tickets/13939'

    cookie = login()

    session3 = requests.session()
    session3.headers = setHeaders()

    session3.params = {
        'Tickets':[
            {
                'change': {'comment' : comment}
            }
        ]
    }

    ticket = session3.post(
        post_info, 
        headers=session3.headers, 
        data=json.dumps(session3.params),
        cookies=cookie)

    logging.info(ticket)
    logging.info(ticket.json())


    # TEST AREA #

# test API endpoint with 2 custom fields and a due date
# @app.post("/ticket/newDynamicTicket/{queueId}/{title}/{summary}/{categoryId}/{impactId}/{statusId}/{ownerId}/{submitterId}/{priorityId}/{machineId}/{custom_1}/{custom_2}/{dueDateYear}/{dueDateMonth}/{dueDateDay}/{dueDateHour}")
# def postDynamicTicketTest(queueId: int, title: str, summary: str, categoryId: int, impactId: int, statusId: int, ownerId: int, submitterId: int, priorityId: int, machineId: int,
#                       custom_1: str,
#                       custom_2: str,
#                       dueDateYear: str, 
#                       dueDateMonth: str, 
#                       dueDateDay: str, 
#                       dueDateHour: str): 
#     try:
#         year = int(dueDateYear);
#         #date_obj = datetime.date(year, int(dueDateMonth), int(dueDateDay), int(dueDateHour));
#         # Termination Test
#     except:
#         postDynamicTicketWithoutDueDate6(queueId, title, summary, categoryId, impactId, statusId, ownerId, submitterId, priorityId, machineId, custom_1, custom_2, dueDateYear, dueDateMonth, dueDateDay, dueDateHour)
#     else:
#         post_info = 'http://192.168.122.33/api/service_desk/tickets/'

#         cookie = login()

#         session3 = requests.session()
#         session3.headers = setHeaders()

#         session3.params = {
#             'Tickets':
#             [
#                 {
#                     'hd_queue_id': queueId,
#                     'title': title,
#                     'summary': summary,
#                     'category': categoryId,
#                     'impact': impactId,
#                     'status': statusId,
#                     'owner': {'id': ownerId},
#                     'submitter': {'id': submitterId},
#                     # 'approval_info': {
#                     #     'approver': { 'id' : 10 },
#                     #     'approve_state' : 'opened'
#                     #     },
#                     'priority': priorityId,
#                     #'asset': assetId,
#                     'machine': machineId,
#                     'custom_1': custom_1,
#                     'custom_2': custom_2,
#                     'due_date': dueDateYear + '-' + dueDateMonth + '-' + dueDateDay + ' ' + dueDateHour + ':' + '00:00',
#                     'is_manual_due_date': int(dueDateHour != None)
#                     #'cc_list' : newTicket.cc_list,
#                     #'resolution': newTicket.resolution
#                 }
#             ]
#         }

#         ticket = session3.post(
#             post_info, 
#             headers=session3.headers, 
#             data=json.dumps(session3.params),
#             cookies=cookie)

#         logging.info(ticket)
#         logging.info(ticket.json())  


# postDynamicTicketTest(
# 9, 
# 'test', 
# 'test', 
# 130, 
# 35, 
# 156, 
# 0, 
# 434, 
# 26, 
# 619,
# 'test',
# 'test',
# '2024', 
# '7', 
# '12', 
# '8');

# def postDynamicTicketWithoutDueDate19(
# queueId: int, 
# title: str, 
# summary: str, 
# categoryId: int, 
# impactId: int, 
# statusId: int, 
# ownerId: int, 
# submitterId: int, 
# priorityId: int, 
# machineId: int,
#                       custom_1: str,
#                       custom_2: str,
#                       custom_3: str,
#                       custom_4: str,
#                       custom_5: str,
#                       custom_6: str,
#                       custom_7: str,
#                       custom_8: str,
#                       custom_9: str,
#                       custom_10: str,
#                       custom_11: str,
#                       custom_12: str,
#                       custom_13: str,
#                       custom_14: str,
#                       custom_15: str,
#                       custom_16: str,
#                       custom_17: str,
#                       custom_18: str,
#                       custom_19: str): 