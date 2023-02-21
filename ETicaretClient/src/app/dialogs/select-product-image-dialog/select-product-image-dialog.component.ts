import { Component, Inject, OnInit, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ListProductImage } from 'src/app/Contracts/list-product-image';
import { ProductService } from 'src/app/services/admin/models/product.service';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';
import { BaseDialog } from '../base/base-dialog';

@Component({
  selector: 'app-select-product-image-dialog',
  templateUrl: './select-product-image-dialog.component.html',
  styleUrls: ['./select-product-image-dialog.component.scss']
})
export class SelectProductImageDialogComponent extends BaseDialog<SelectProductImageDialogComponent> implements OnInit {
  constructor(dialogRef: MatDialogRef<SelectProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageDialogResponse | string,
    private productService: ProductService) {
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
}

export enum SelectProductImageDialogResponse {
  Close
}
