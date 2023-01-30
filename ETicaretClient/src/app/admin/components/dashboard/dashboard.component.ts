import { Component } from '@angular/core';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  constructor(private alertify: AlertifyService) { }
  trigger() {
    this.alertify.message("MALFUNCTION !!!!", { alertType: AlertType.Error, delay: 31, dismissOthers: true, position: Position.TopLeft });
  }
  dismiss() {
    this.alertify.dismissAll()
  }
}

