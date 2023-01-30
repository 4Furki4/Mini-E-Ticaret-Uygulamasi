import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends BaseComponent implements OnInit {
  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService) {
    super(spinner);
  }
  ngOnInit(): void {
    this.showSpinner(SpinnerTypes.Cog);
  }

  trigger() {
    this.alertify.message("MALFUNCTION !!!!", { alertType: AlertType.Error, delay: 31, dismissOthers: true, position: Position.TopLeft });
  }
  dismiss() {
    this.alertify.dismissAll()
  }
}

