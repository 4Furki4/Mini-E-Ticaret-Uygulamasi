import { HttpHeaders } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { error } from 'console';
import { NgxFileDropEntry } from 'ngx-file-drop';
import { FileUploadDialogComponent, FileUploadDialogResponse } from 'src/app/dialogs/file-upload-dialog/file-upload-dialog.component';
import { AlertifyService, AlertType, Position } from '../../admin/alertify/alertify.service';
import { CustomToasterService, ToasterOptions, ToasterPosition, ToasterType } from '../../ui/toaster/custom-toaster.service';
import { DialogService } from '../dialog.service';
import { HttpClientService } from '../http-client.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent {

  @Input() options !: Partial<FileUploadOptions>;

  constructor(private httpClientService: HttpClientService,
    private alertifyService: AlertifyService, private toastrService: CustomToasterService, public dialog: MatDialog,
    private dialogService: DialogService) {

  }
  public files: NgxFileDropEntry[] = [];
  public dropped(files: NgxFileDropEntry[]) {
    const formFile: FormData = new FormData();
    for (const droppedFile of files) {
      (droppedFile.fileEntry as FileSystemFileEntry).file((file: File) => {
        formFile.append(file.name, file, droppedFile.relativePath);
      });
    }
    this.dialogService.openDialog({
      componentType: FileUploadDialogComponent,
      data: FileUploadDialogResponse.Yes,
      deleteApproveCallBack: () => {
        this.httpClientService.post({
          controller: this.options.controller,
          action: this.options.action,
          queryString: this.options.queryParams,
          headers: new HttpHeaders({
            "responseType": "blob"
          })
        }, formFile).subscribe({
          next: (response) => {
            this.files = files;
            debugger;
            if (this.options.isAdminSide) {
              this.alertifyService.message("Seçilen dosya(lar) başarıyla yüklendi.", {
                alertType: AlertType.Success,
                delay: 5000,
                dismissOthers: true,
                position: Position.TopLeft
              })
            }
            else {
              this.toastrService.message("Seçilen dosya(lar) başarıyla yüklendi.", 'Yükleme Başarılı !', {
                messageType: ToasterType.Success,
                position: ToasterPosition.TopLeft
              });
            }
          },
          error: (error) => {
            if (this.options.isAdminSide) {
              this.alertifyService.message("Seçilen dosya(lar) yüklenirken hata oluştu.", {
                alertType: AlertType.Error,
                delay: 5000,
                dismissOthers: true,
                position: Position.TopLeft
              })
            }
            else {
              this.toastrService.message("Seçilen dosya(lar) yüklenirken hata oluştu.", 'Yükleme Başarısız !', {
                messageType: ToasterType.Error,
                position: ToasterPosition.TopLeft
              });
            }
          }
        })
      },
      options: {
        width: '500px',
        height: '250px',
      }
    })
  }
}

export class FileUploadOptions {
  controller?: string;
  action?: string;
  queryParams?: string;
  explanation?: string;
  accept?: string;
  isAdminSide?: boolean = false;
}
