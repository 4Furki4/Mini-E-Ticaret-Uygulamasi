import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class HttpClientService {

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  private url(requestParams: Partial<RequestParameters>): string {

    if (requestParams.fullEndpoint)
      return requestParams.fullEndpoint;
    else {
      return `${requestParams.baseUrl ? requestParams.baseUrl : this.baseUrl}/${requestParams.controller}${requestParams.action ? `/${requestParams.action}` : ``}`
    }
  }
  get<T>(requestParams: Partial<RequestParameters>, id?: string): Observable<T> {
    let url = `${this.url(requestParams)}${id ? `/${id}` : ``}`;
    return this.httpClient.get<T>(url, { headers: requestParams.headers });
  }


  post() {

  }


  put() {

  }

  delete() {

  }
}

export class RequestParameters {
  baseUrl?: string;
  controller?: string;
  action?: string;
  fullEndpoint?: string;
  headers?: HttpHeaders;
}