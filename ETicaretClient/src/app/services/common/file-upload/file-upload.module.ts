import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileUploadComponent } from './file-upload.component';
import { NgxFileDropModule } from 'ngx-file-drop';
import { DialogModule } from 'src/app/dialogs/dialog.module';
import { DialogService } from '../dialog.service';



@NgModule({
  declarations: [
    FileUploadComponent
  ],
  imports: [
    CommonModule,
    NgxFileDropModule,
    DialogModule
  ],
  exports: [
    FileUploadComponent
  ],
  // providers: [
  //   { provide: DialogService }
  // ]
})
export class FileUploadModule { }
