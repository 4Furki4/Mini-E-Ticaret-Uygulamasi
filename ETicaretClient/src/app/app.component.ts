import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from './services/common/auth.service';
import { CustomToasterService, ToasterPosition, ToasterType } from './services/ui/toaster/custom-toaster.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ETicaretClient';
  constructor(public authService: AuthService, private toastrService: CustomToasterService) {
    authService.IdentityCheck();
    console.log(authService.IsAuthenticated);
  }
  signOut() {
    localStorage.removeItem('token');
    this.authService.IdentityCheck();
    this.toastrService.message("Başarıyla çıkış yaptınız.", "Hoşça Kalın!", {
      messageType: ToasterType.Success,
      position: ToasterPosition.TopFullWidth
    })
  }
}

