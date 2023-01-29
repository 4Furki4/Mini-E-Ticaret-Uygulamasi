import { Component } from '@angular/core';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {

}
declare var $: any;
declare var alertify: any;
$(document).ready(() => {
  alertify.success("You've done it !")
})