import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseDialog } from '../base/base-dialog';

@Component({
  selector: 'app-select-product-image-dialog',
  templateUrl: './select-product-image-dialog.component.html',
  styleUrls: ['./select-product-image-dialog.component.scss']
})
export class SelectProductImageDialogComponent extends BaseDialog<SelectProductImageDialogComponent> {
  constructor(dialogRef: MatDialogRef<SelectProductImageDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: SelectProductImageDialogResponse | string) {
    super(dialogRef);
  }
}

export enum SelectProductImageDialogResponse {
  Close
}
