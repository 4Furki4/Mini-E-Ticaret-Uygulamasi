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


  post<T>(requestParams: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    let url: string = `${this.url(requestParams)}`
    return this.httpClient.post<T>(url, body, {
      headers: requestParams.headers
    });
  }


  put<T>(requestParams: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    let url: string = `${this.url(requestParams)}`
    return this.httpClient.put<T>(url, body, {
      headers: requestParams.headers
    });
  }

  delete(requestParams: Partial<RequestParameters>, id: string): Observable<any> {
    let url: string = `${this.url(requestParams)}/${id}`
    return this.httpClient.delete(url, {
      headers: requestParams.headers
    });
  }
}

export class RequestParameters {
  baseUrl?: string;
  controller?: string;
  action?: string;
  fullEndpoint?: string;
  headers?: HttpHeaders;
}