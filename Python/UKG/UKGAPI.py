# # UKG API

import json
from multiprocessing import Value
import requests

from flask import jsonify
from flask_cors import CORS, cross_origin
from fastapi.middleware.cors import CORSMiddleware

from fastapi import Body, FastAPI

app = FastAPI()

# CORS(app)
# cors = CORS(app, resource={
#     r"/*":{
#         "origins": "*"}})

origins = ["*"]

app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"]
    )

def getAccessToken():
    username = 'pw'
    password = 'pw'
    client_id = 'id'
    client_secret = 'secret'
    grant_type = 'password'
    auth_chain = 'OAuthLdapService'

    session = requests.session()
    data = {
                        'username': username,
                        'password': password,
                        'client_id': client_id,
                        'client_secret': client_secret,
                        'grant_type': grant_type,
                        'auth_chain': auth_chain}
    
    session.headers = {'Content-Type': 'application/x-www-form-urlencoded'}
    
    login_path = 'path...'

    login = session.post(
        login_path, 
        headers=session.headers, 
        data=data)

    cookie = login
    
    # Step 1: Decode the byte string to a regular string
    content_str = cookie.content.decode('utf-8')

    # Step 2: Parse the JSON string to a Python dictionary
    content_dict = json.loads(content_str)

    # Step 3: Access the 'access_token' value
    access_token = content_dict['access_token']
    
    return access_token

@app.get("/listUsers/{status}")
def listUsers(status: str):
    

    get_info = 'https://masstransportation.prd.mykronos.com/api/v1/commons/persons/apply_read'

    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}
    
    body = {'index': 0,
            'count': 1000,
            'where': {
                'employmentStatus': status}
            }    

    info = requests.post(
        get_info,
        headers=headers,
        json=body)
    print(info.content)
    return info.content


@app.get("/showApprovals/{employeeId}")
def showApprovals(employeeId: str):
    

    get_info = 'https://masstransportation.prd.mykronos.com/api/v1/timekeeping/timecard_approvals'
    get_info += '?person_num=' + employeeId
    get_info += '&start_date=2024-07-01'
    get_info += '&end_date=2024-07-14'
    
    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}
    
    # body = {
    #     'employee': { 
    #         'id': employeeId
    #     },
    #     'dateRange': {
    #         'startDate': '2024-07-01',
    #          'endDate': '2024-07-15'
    #     }
    # }

    info = requests.get(
        get_info,
        headers=headers,
        #json=body
        )
    print(info.content)
    return info.content

@app.get("/showApprovals/{employeeId}")
def showApprovals(employeeId: str):
    

    get_info = 'https://masstransportation.prd.mykronos.com/api/v1/timekeeping/timecard_approvals'
    get_info += '?person_num=' + employeeId
    get_info += '&start_date=2024-07-01'
    get_info += '&end_date=2024-07-14'
    
    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}
    
    # body = {
    #     'employee': { 
    #         'id': employeeId
    #     },
    #     'dateRange': {
    #         'startDate': '2024-07-01',
    #          'endDate': '2024-07-15'
    #     }
    # }

    info = requests.get(
        get_info,
        headers=headers,
        #json=body
        )
    print(info.content)
    return info.content

@app.get("/approveCurrentSchedule/{employeeId}")
def approveCurrentSchedule(employeeId: str):
    
    get_info = 'https://masstransportation.prd.mykronos.com/api/v1/timekeeping/timecard_approvals'
    # get_info += '?person_num=' + employeeId
    # get_info += '&start_date=2024-07-01'
    # get_info += '&end_date=2024-07-14'
    
    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}
    
    body = {
        'employee': { 
            'id': employeeId
        },
        'dateRange': 
        { 
            "symbolicPeriod": 
            { 
                "id": 1
            } 
        }
    }

    info = requests.post(
        get_info,
        headers=headers,
        json=body
        )
    print(info.content)
    return info.content

@app.get("/getLeaveCaseStatuses")
def getLeaveCaseStatuses():
    
    get_info = 'https://masstransportation.prd.mykronos.com/api/v2/leave/case_statuses'
    # get_info += '?'
    # get_info += 'employee_id=' + '3405'
    
    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}

    info = requests.get(
        get_info,
        headers=headers
        # json=body
        )
    print(info.content)
    #[{"id":1,"qualifier":"Open","name":"Open"},
    #{"id":2,"qualifier":"Closed","name":"Closed"},
    #{"id":0,"qualifier":"Submitted","name":"Submitted"}]
    return info.content

@app.get("/getLeaveEvents")
def getLeaveEvents():
    
    get_info = 'https://masstransportation.prd.mykronos.com/api/v1/leave/events'
    # get_info += '?'
    # get_info += 'employee_id=' + '8964'
    # get_info += '&leave_case_id=' + '24'
    # get_info += '&end_date=2024-07-14'
    
    headers = {'Content-Type': 'application/json',
                'Authorization': getAccessToken(),
                'Accept': 'application/json'}
    
    body = {
         'eventTypes':
             ['leave_time_all_cases'],
        'dateRange': 
        {
            'startDate': '2022-07-01',
            'endDate': '2024-07-23'
        }
    }

    info = requests.post(
        get_info,
        headers=headers,
        json=body
        )
    print(info.content)
    return info.content

#approveCurrentSchedule('3501')
#getLeaveCaseStatuses();
#getLeaveEvents();
#listUsers();
#listUsers('L');