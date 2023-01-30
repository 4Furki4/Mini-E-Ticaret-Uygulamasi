import { Injectable } from '@angular/core';
declare var alertify: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }
  message(message: string, options: Partial<AlertifyOptions>) {
    this.setPosition(options.position);
    this.setDelay(options.delay)
    const msg = alertify[options.alertType ?? AlertType.Message](message);
    if (options.dismissOthers)
      msg.dismissOthers();
  }
  setPosition(position: Position | undefined) {
    alertify.set("notifier", "position", position)
  }
  setDelay(delay: number | undefined) {
    alertify.set("notifier", "delay", delay)
  }
  dismissAll() {
    alertify.dismissAll();
  }
}
export class AlertifyOptions {
  alertType!: AlertType;
  position: Position = Position.BottomLeft;
  delay: number = 4;
  dismissOthers: boolean = false;
}
export enum AlertType {
  Error = "error",
  Notify = "notify",
  Message = "message",
  Success = "success",
  Warning = "warning"
}

export enum Position {
  BottomLeft = "bottom-left",
  BottomRight = "bottom-right",
  BottomCenter = "bottom-center",
  TopLeft = "top-left",
  TopRight = "top-right",
  TopCenter = "top-center"
}