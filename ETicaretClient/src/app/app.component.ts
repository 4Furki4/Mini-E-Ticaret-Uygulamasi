import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { CustomToasterService, ToasterPosition, ToasterType } from './services/ui/toaster/custom-toaster.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ETicaretClient';
  constructor(private toaster: CustomToasterService) { }

  toast() {
    this.toaster.message("Merhaba!", "Başlık", {
      messageType: ToasterType.Success,
      position: ToasterPosition.BottomFullWidth
    })
  }
}

