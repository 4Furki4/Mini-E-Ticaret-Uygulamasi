import { Component, Inject, OnInit, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerTypes } from 'src/app/base/base.component';
import { ListProductImage } from 'src/app/Contracts/list-product-image';
import { ProductService } from 'src/app/services/admin/models/product.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';
import { BaseDialog } from '../base/base-dialog';
import { DeleteDialogComponent, DeleteDialogResponse } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-select-product-image-dialog',
  templateUrl: './select-product-image-dialog.component.html',
  styleUrls: ['./select-product-image-dialog.component.scss']
})
export class SelectProductImageDialogComponent extends BaseDialog<SelectProductImageDialogComponent> implements OnInit {
  constructor(dialogRef: MatDialogRef<SelectProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageDialogResponse | string,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private diaglogService: DialogService) {
    super(dialogRef);
  }
  imageList: ListProductImage[] = [];
  async ngOnInit(): Promise<void> {
    this.imageList = await this.productService.readImagesAsync(this.data as string);
  }
  @Output() options: Partial<FileUploadOptions> = {
    accept: '.png, .jpg, .jpeg, .gif',
    controller: 'Products',
    action: 'Upload',
    explanation: 'Lütfen yüklemek istediğiniz görselleri seçiniz.',
    isAdminSide: true,
    queryParams: `id=${this.data}`
  }
  deleteImage(imageId: string) {
    this.diaglogService.openDialog({
      componentType: DeleteDialogComponent,
      data: DeleteDialogResponse.Yes,
      afterClosed: () => {
        this.spinner.show(SpinnerTypes.Ball8Bits);
        this.productService.deleteImage(this.data as string, imageId, () => {
          this.spinner.hide(SpinnerTypes.Ball8Bits);
          this.imageList = this.imageList.filter(x => x.id !== imageId);
        }, () => {
          this.spinner.hide(SpinnerTypes.Ball8Bits);
        }
        );
      }
    })
  }
}

export enum SelectProductImageDialogResponse {
  Close = 0
}
