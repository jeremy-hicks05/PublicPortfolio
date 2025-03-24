import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  url = environment.baseUrl;
  constructor(private http: HttpClient) { }

  // get user windows name from API
  get(): Observable<string> {
    var userName = this.http.get(this.url + "api/User", { responseType: 'text' })
    return userName;
  }

  hasRole(role: string): Observable<string> {
   var userHasRole = this.http.get(this.url + "api/User/HasRole/" + role, { responseType: 'text' })
    return userHasRole;
  }
}
