import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { FileUploadDialogComponent } from './file-upload-dialog/file-upload-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogService } from '../services/common/dialog.service';
import { SelectProductImageDialogComponent } from './select-product-image-dialog/select-product-image-dialog.component';
import { FileUploadModule } from '../services/common/file-upload/file-upload.module';
import { MatCardModule } from '@angular/material/card';
import { LogoutDialogComponent } from './logout-dialog/logout-dialog.component';



@NgModule({
  declarations: [
    DeleteDialogComponent,
    FileUploadDialogComponent,
    SelectProductImageDialogComponent,
    LogoutDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule, MatDialogModule, MatCardModule,
    FileUploadModule
  ],
  exports: [
    SelectProductImageDialogComponent
  ],
  providers: [
    { provide: DialogService }
  ]
})
export class DialogModule { }
