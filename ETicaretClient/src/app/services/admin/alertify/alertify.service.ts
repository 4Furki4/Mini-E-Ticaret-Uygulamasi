import { Injectable } from '@angular/core';
declare var alertify: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  message(message: string, alertType: AlertType) {
    alertify[alertType](message);
  }
}

export enum AlertType {
  Error = "error",
  Notify = "notify",
  Message = "message",
  Success = "success",
  Warning = "warning"
}
