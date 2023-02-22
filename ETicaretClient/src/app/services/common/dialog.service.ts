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
      width: dialogParameters.options?.width ?? '500px',
      height: dialogParameters.options?.height ?? '250px',
      data: dialogParameters.data
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result)
        dialogParameters.afterClosed();
    })
  }
}

export class DialogParameters {
  componentType !: ComponentType<any>;
  data !: any;
  afterClosed: any;
  options?: Partial<DialogOptions>;
}
export class DialogOptions {
  width?: string;
  height?: string;
}
