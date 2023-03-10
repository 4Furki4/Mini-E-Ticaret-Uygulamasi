import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { AuthService } from 'src/app/services/common/auth.service';
import { CustomToasterService, ToasterPosition, ToasterType } from 'src/app/services/ui/toaster/custom-toaster.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard extends BaseComponent implements CanActivate {

  constructor(private jwtHelper: JwtHelperService, private router: Router, spinner: NgxSpinnerService, private toastr: CustomToasterService, private authService: AuthService) {
    super(spinner)
  }

  canActivate(
    route: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : Observable<boolean | UrlTree> |
    Promise<boolean | UrlTree> | boolean | UrlTree {
    this.showSpinner(SpinnerTypes.Ball8Bits);
    this.authService.IdentityCheck();
    debugger;
    if (!this.authService.IsAuthenticated) {

      this.router.navigate(["login"], {
        queryParams: {
          returnUrl: state.url
        }
      })
      this.toastr.message("Giriş yapmanız gerekiyor!", "Yetkisiz Giriş!", {
        messageType: ToasterType.Error,
        position: ToasterPosition.TopFullWidth
      })
      this.hideSpinner(SpinnerTypes.Ball8Bits)
    }
    this.hideSpinner(SpinnerTypes.Ball8Bits)
    return true;
  }

}
