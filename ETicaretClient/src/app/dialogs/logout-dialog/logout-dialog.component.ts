import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseDialog } from '../base/base-dialog';

@Component({
  selector: 'app-logout-dialog',
  templateUrl: './logout-dialog.component.html',
  styleUrls: ['./logout-dialog.component.scss']
})
export class LogoutDialogComponent extends BaseDialog<LogoutDialogComponent> {
  constructor(DialogRef: MatDialogRef<LogoutDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
    super(DialogRef)
  }
}

export enum LogoutDialogResponse {
  No,
  Yes
}
