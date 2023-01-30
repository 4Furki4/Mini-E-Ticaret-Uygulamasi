import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CustomToasterService {

  constructor(private toaster: ToastrService) { }

  message(message: string, title: string, toasterOptions: Partial<ToasterOptions> = { messageType: ToasterType.Info, position: ToasterPosition.BottomLeft }) {
    this.toaster[toasterOptions.messageType ?? ToasterType.Info](message, title, { progressBar: true });
  }
}

export class ToasterOptions {
  position !: ToasterPosition;
  messageType !: ToasterType;
}



export enum ToasterType {
  Error = "error",
  Info = "info",
  Success = "success",
  Warning = "warning"
}

export enum ToasterPosition {
  TopRight = "toast-top-right",
  TopFullWidth = "toast-top-full-width",
  TopLeft = "toast-top-left",
  TopCenter = "toast-top-center",
  BottomRight = "toast-bottom-right",
  BottomLeft = "toast-bottom-left",
  BottomCenter = "toast-bottom-center",
  BottomFullWidth = "toast-bottom-full-width",
}
