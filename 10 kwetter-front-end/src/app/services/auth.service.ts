import { Http, RequestOptions, Headers } from '@angular/http';
import { Injectable } from '@angular/core';
import { tokenNotExpired, JwtHelper } from 'angular2-jwt'; 
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/add/operator/map'; 

@Injectable()
export class AuthService {
  private _currentUser: any;

  get currentUser() {
    if(this._currentUser === undefined){
      let jwt = new JwtHelper();
      this._currentUser = jwt.decodeToken(localStorage.getItem('id_token'));
    }
    return this._currentUser
  }

  constructor(private http: HttpClient) {
    let token = localStorage.getItem('id_token');
    if (token) {
      let jwt = new JwtHelper();
      this._currentUser = jwt.decodeToken(token);
    }
  }

  login(credentials) {
    let body = new URLSearchParams();
    body.set('username', credentials.email);
    body.set('password', credentials.password);
    body.set('grant_type', environment.grantType);
    body.set('scope', environment.scope);

    let headers = this.getDefaultHeaders();

    return this.http.post<any>(environment.tokenEndPoint, body.toString(), { headers })
      .map(result => {
        
        if (result && result.access_token) {
          localStorage.setItem('access_token', result.access_token);
          localStorage.setItem('refresh_token', result.refresh_token);
          localStorage.setItem('id_token', result.id_token);

          let jwt = new JwtHelper();
          this._currentUser = jwt.decodeToken(localStorage.getItem('id_token'));
          console.log(this._currentUser);

          return true; 
        }
        else return false;
      });
  }

  logout() { 
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('id_token');
    this._currentUser = null;
  }

  isLoggedIn() : boolean { 
    return tokenNotExpired('access_token');
  }

  private getDefaultHeaders(): HttpHeaders {
    let headers = new HttpHeaders();

    headers = headers.append("Content-Type", "application/x-www-form-urlencoded");
    return headers;
  }
}

