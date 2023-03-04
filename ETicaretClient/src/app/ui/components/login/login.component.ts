import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { AuthService } from 'src/app/services/common/auth.service';
import { CustomToasterService, ToasterPosition, ToasterType } from 'src/app/services/ui/toaster/custom-toaster.service';
import { trigger, state, style, animate, transition } from '@angular/animations'
import { SocialAuthService, FacebookLoginProvider } from '@abacritt/angularx-social-login';
import { SocialUser } from '@abacritt/angularx-social-login/public-api';
import { ToastrService } from 'ngx-toastr';
import { UserAuthService } from 'src/app/services/common/models/user-auth.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  animations: [
    trigger('openClosed', [
      state('open', style({
        opacity: '1',
        marginLeft: '*'
      })),
      state('void', style({
        opacity: '0',
        marginLeft: '0'
      })),
      transition('void => open', animate('1s ease-out')),
      transition('open => void', animate('1s ease-out'))
    ]),
    trigger('validatonBorder', [
      state('valid', style({
        border: '2px solid #198754',
      })),
      state('invalid', style({
        border: '2px solid #F44336',
      })),
      state('void', style({
        border: '2px solid gray'
      })),
      transition('void<=>invalid', animate('0.75s ease-in-out')),
      transition('valid<=>invalid', animate('0.5s ease-in-out'))
    ]),
  ]
})
export class LoginComponent extends BaseComponent implements OnInit, OnDestroy {

  constructor(private formBuilder: FormBuilder, private userAuthService: UserAuthService, ngxSpinner: NgxSpinnerService, private customToastr: CustomToasterService,
    private authService: AuthService, private activatedRoute: ActivatedRoute, private router: Router, private socialAuthService: SocialAuthService, private toastr: ToastrService
  ) {
    super(ngxSpinner);
    this.socialAuthService.authState.subscribe(async (user: SocialUser) => {
      this.showSpinner(SpinnerTypes.Ball8Bits);
      switch (user?.provider) {
        case 'GOOGLE':
          await userAuthService.googleLogin(user).then(() => {

            this.hideSpinner(SpinnerTypes.Ball8Bits);
            this.customToastr.message("Google Hesabınız ile girişiniz başarılı", "GİRİŞ BAŞARILI!", {
              messageType: ToasterType.Success,
              position: ToasterPosition.TopFullWidth
            })
          }).catch((err: HttpErrorResponse) => {
            this.hideSpinner(SpinnerTypes.Ball8Bits);
            this.customToastr.message("Google Girişi başarısız!", "BEKLENMEDİK HATA!", {
              messageType: ToasterType.Error,
              position: ToasterPosition.BottomCenter
            })
          })
          break;
        case 'FACEBOOK':
          await this.userAuthService.facebookLogin(user).then(() => {
            this.hideSpinner(SpinnerTypes.Ball8Bits);
            this.customToastr.message("Facebook Hesabınız ile girişiniz başarılı", "GİRİŞ BAŞARILI!", {
              messageType: ToasterType.Success,
              position: ToasterPosition.TopFullWidth
            })
          }).catch((err: HttpErrorResponse) => {
            this.hideSpinner(SpinnerTypes.Ball8Bits);
            this.customToastr.message("Facebook Girişi başarısız!", "BEKLENMEDİK HATA!", {
              messageType: ToasterType.Error,
              position: ToasterPosition.BottomCenter
            })
          })
          break;
        default:
          this.hideSpinner(SpinnerTypes.Ball8Bits);
          break;
      }
      authService.IdentityCheck()
    })
  }
  ngOnDestroy(): void {
    this.isOpen = false;
    this.toastr.clear();
    debugger;
    this.socialAuthService.signOut();
  }
  isOpen = false
  form !: FormGroup;
  ngOnInit(): void {
    this.isOpen = true;
    this.form = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(40)]],
      password: ['', Validators.required]
    })
  }

  get userName() {
    return this.form.get('userName')
  }
  get password() {
    return this.form.get('password')
  }

  async onSubmit(formValues: AbstractControl, ngForm: FormGroupDirective) {
    this.showSpinner(SpinnerTypes.Ball8Bits);
    await this.userAuthService.login(this.userName?.value, this.password?.value).then((val) => {
      this.authService.IdentityCheck();
      this.hideSpinner(SpinnerTypes.Ball8Bits);
      this.customToastr.message("Giriş yapılıyor...", "BAŞARILI!", {
        messageType: ToasterType.Success,
        position: ToasterPosition.TopFullWidth
      });
      this.activatedRoute.queryParams.subscribe(params => {
        const returnUrl: string = params["returnUrl"]
        this.router.navigate([returnUrl]);
      })
    }).catch((error: HttpErrorResponse) => {
      console.log(error);
      this.hideSpinner(SpinnerTypes.Ball8Bits);
      this.customToastr.message('Şifre, e-posta veya kullanıcı adı yanlış. Lütfen tekrar deneyiniz', "BAŞARISIZ!", {
        messageType: ToasterType.Error,
        position: ToasterPosition.TopFullWidth
      });
    })
  }

  facebookLogin() {
    debugger;
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

}
