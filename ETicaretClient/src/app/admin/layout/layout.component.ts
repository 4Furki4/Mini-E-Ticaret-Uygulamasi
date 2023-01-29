import { Component } from '@angular/core';
import { AlertifyService, AlertType } from 'src/app/services/admin/alertify/alertify.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {

  constructor(private alertify: AlertifyService) {
    alertify.message("Hoş geldiniz sayın yönetici!", AlertType.Notify)
  }
}