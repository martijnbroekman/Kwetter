import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { BadInput } from "../errors/bad-input";
import { NotFoundError } from "../errors/not-found-error";
import { AppError } from "../errors/app-error";
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';


@Injectable()
export class BaseService {
  constructor(private url: string, protected http: HttpClient) { }

  getAll() {
    return this.http.get(this.url)
      .catch(this.handleError);
  }

  getDefaultHeaders(): HttpHeaders {
    let headers = new HttpHeaders();

    headers = headers.append("Content-Type", "application/json");
    return headers;
  }

  getDefaultHeadersWithAccessToken(): HttpHeaders {
    let headers = this.getDefaultHeaders();

    headers = headers.append("Authorization", "Bearer " + this.getAccessToken())
    return headers;
  }

  private getAccessToken() {
    return localStorage.getItem('access_token');
  }

  handleError(error: Response) {
    if (error.status === 400)
      return Observable.throw(new BadInput(error));
  
    if (error.status === 404)
      return Observable.throw(new NotFoundError());
    
    return Observable.throw(new AppError(error));
  }
}