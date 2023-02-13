import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { firstValueFrom, Observable } from 'rxjs';
import { DeleteDialogComponent, DeleteDialogResponse } from 'src/app/dialogs/delete-dialog/delete-dialog.component';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
import { ProductService } from 'src/app/services/admin/models/product.service';
import { HttpClientService } from 'src/app/services/common/http-client.service';
declare var $: any;
@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(private elementRef: ElementRef,
    private renderer: Renderer2,
    private httpClient: HttpClientService,
    public dialog: MatDialog,
    private alertifyServie: AlertifyService) {

    const imgEl = this.renderer.createElement('img');
    imgEl.setAttribute('src', '../../../../../assets/trash.png');
    imgEl.setAttribute('alt', 'delete');
    imgEl.setAttribute('width', '20');
    imgEl.setAttribute('height', '20');
    imgEl.setAttribute('style', 'cursor: pointer');
    this.renderer.appendChild(this.elementRef.nativeElement, imgEl);
  }
  @Input() id: string = '';
  @Input() controller: string = '';
  @Output() deleteCallBack: EventEmitter<any> = new EventEmitter();
  @HostListener('click')
  async onCLick() {
    this.openDialog(async () => {
      const td: HTMLTableCellElement = this.elementRef.nativeElement as HTMLTableCellElement;
      const request: Observable<any> = this.httpClient.delete({
        controller: this.controller,
      }, this.id)
      await firstValueFrom(request).then(
        $(td.parentElement).fadeOut(500, () => {
          this.deleteCallBack.emit();
          this.alertifyServie.message('Deleted Successfully', {
            position: Position.TopRight,
            alertType: AlertType.Success,
            delay: 3000,
            dismissOthers: true
          })
        })
      )
    })
  }

  openDialog(deleteApproveCallBack: () => void): void {
    const dialogRef = this.dialog.open(DeleteDialogComponent, {
      data: DeleteDialogResponse.Yes,
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result == DeleteDialogResponse.Yes)
        deleteApproveCallBack();
    })
  }
}

