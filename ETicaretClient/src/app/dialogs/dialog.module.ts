import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeleteDialogComponent } from './delete-dialog/delete-dialog.component';
import { FileUploadDialogComponent } from './file-upload-dialog/file-upload-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogService } from '../services/common/dialog.service';



@NgModule({
  declarations: [
    DeleteDialogComponent,
    FileUploadDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule
  ],
  providers: [
    { provide: DialogService }
  ]
})
export class DialogModule { }
