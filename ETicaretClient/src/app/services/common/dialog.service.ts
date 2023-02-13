import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { DialogPosition, MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(public dialog: MatDialog) { }
  openDialog(dialogParameters: Partial<DialogParameters>): void {
    const dialogRef = this.dialog.open(dialogParameters.componentType!, {
      width: dialogParameters.options!.width,
      height: dialogParameters.options!.height,
      position: dialogParameters.options!.position
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == dialogParameters.data)
        dialogParameters.deleteApproveCallBack!()
    })
  }
}

export class DialogParameters {
  componentType !: ComponentType<any>;
  data !: any;
  deleteApproveCallBack !: () => void;
  options !: Partial<DialogOptions>;
}
export class DialogOptions {
  width?: string = '500px';
  height?: string = '250px';
  position?: DialogPosition
}
