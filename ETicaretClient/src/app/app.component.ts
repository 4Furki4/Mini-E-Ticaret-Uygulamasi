import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { LogoutDialogComponent, LogoutDialogResponse } from './dialogs/logout-dialog/logout-dialog.component';
import { AuthService } from './services/common/auth.service';
import { DialogService } from './services/common/dialog.service';
import { CustomToasterService, ToasterPosition, ToasterType } from './services/ui/toaster/custom-toaster.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ETicaretClient';
  constructor(public authService: AuthService, private toastrService: CustomToasterService, private dialogService: DialogService) {
    authService.IdentityCheck();
    console.log(authService.IsAuthenticated);
  }
  signOut() {
    this.dialogService.openDialog({
      componentType: LogoutDialogComponent,
      data: LogoutDialogResponse.Yes,
      options: {
        height: "200px"
      },
      afterClosed: () => {
        localStorage.removeItem('token');
        this.authService.IdentityCheck();
        this.toastrService.message("Başarıyla çıkış yaptınız.", "Hoşça Kalın!", {
          messageType: ToasterType.Success,
          position: ToasterPosition.TopFullWidth
        })
      }
    })
  }
}

