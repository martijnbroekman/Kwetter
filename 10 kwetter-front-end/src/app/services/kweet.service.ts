import { BaseService } from './base.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { Kweet } from '../interfaces/Kweet';

@Injectable()
export class KweetsService extends BaseService {

    private kweetsUrl: string;

    constructor(http: HttpClient) {
        let kweetsUrl = environment.rootUrl + '/kweets';
        super(kweetsUrl, http);
        this.kweetsUrl = kweetsUrl;
    }

    getTimeLine(userId: number) : Observable<Kweet[]> {
        return this.http.get<Kweet[]>(this.kweetsUrl + '/timeline/' + userId, { headers: super.getDefaultHeaders() })
            .catch(this.handleError);
    }
}
