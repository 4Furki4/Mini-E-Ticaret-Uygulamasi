import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { AuthService } from 'src/app/services/common/auth.service';
import { UserService } from 'src/app/services/common/models/user.service';
import { CustomToasterService, ToasterPosition, ToasterType } from 'src/app/services/ui/toaster/custom-toaster.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends BaseComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private userService: UserService, ngxSpinner: NgxSpinnerService, private toastr: CustomToasterService,
    private authService: AuthService, private activatedRoute: ActivatedRoute, private router: Router) {
    super(ngxSpinner);
  }

  form !: FormGroup;
  ngOnInit(): void {
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
    await this.userService.login(this.userName?.value, this.password?.value).then((val) => {
      this.authService.IdentityCheck();
      this.hideSpinner(SpinnerTypes.Ball8Bits);
      this.toastr.message("Giriş yapılıyor...", "BAŞARILI!", {
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
      this.toastr.message(error.statusText, "BAŞARISIZ!", {
        messageType: ToasterType.Error,
        position: ToasterPosition.TopFullWidth
      });
    })
  }

}
