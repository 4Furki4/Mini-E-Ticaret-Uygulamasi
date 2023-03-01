import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, FormGroupDirective, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/app/services/common/models/user.service';
import { CustomToasterService, ToasterPosition, ToasterType } from 'src/app/services/ui/toaster/custom-toaster.service';
import { trigger, state, style, animate, transition } from '@angular/animations'
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  animations: [
    trigger('openClosed', [
      state('open', style({
        opacity: '1'
      })),
      state('void', style({
        opacity: '0'
      })),
      transition('void => open', animate('0.75s ease-out')),
      transition('open => void', animate('1s ease-out'))
    ]),
    trigger('validatonBorder', [
      state('void', style({
        border: '2px solid red',
      })),
      state('valid', style({
        border: '2px solid green',
      })),
      transition('void => valid', animate('1s ease-in')),
      transition('valid => void', animate('1s ease-in'))
    ])
  ]
})
export class RegisterComponent extends BaseComponent implements OnInit, OnDestroy {

  constructor(private formBuilder: FormBuilder, private userService: UserService, private toastrService: CustomToasterService, spinner: NgxSpinnerService) {
    super(spinner)
  }
  ngOnDestroy(): void {
    this.isOpen = false;
  }
  isOpen: boolean = false;
  form !: FormGroup
  ngOnInit(): void {
    this.isOpen = true;
    this.form = this.formBuilder.group({
      fullName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50), nameValidation()]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.maxLength(40), Validators.pattern(/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[\.\*!_@-])(?=.*[a-zA-Z]).{8,}$/)]],
      confirmPassword: ['']
    })
  }
  get fullName() {
    return this.form.get('fullName');
  }
  get userName() {
    return this.form.get('userName');
  }
  get email() {
    return this.form.get('email');
  }
  get password() {
    return this.form.get('password');
  }
  get confirmPassword() {
    return this.form.get('confirmPassword');
  }
  submitted: boolean = false;
  async onSubmit(user: User, ngForm: FormGroupDirective) {
    this.showSpinner(SpinnerTypes.Ball8Bits)
    this.submitted = true;
    if (this.form.invalid) {
      this.hideSpinner(SpinnerTypes.Ball8Bits)
      return;
    }
    else {
      console.log(user);
      await this.userService.create(user).then((res) => {
        if (res.isSuccessfull) {
          this.hideSpinner(SpinnerTypes.Ball8Bits)
          this.toastrService.message(res.message, "Kayıt Başarılı", {
            messageType: ToasterType.Success,
            position: ToasterPosition.TopRight
          })
          ngForm.resetForm();
          this.form.reset();
        }
        else {
          this.hideSpinner(SpinnerTypes.Ball8Bits)
          this.toastrService.message(res.message, "Kayıt Başarısız", {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
        }

      })
        .catch((error: HttpErrorResponse) => {
          this.hideSpinner(SpinnerTypes.Ball8Bits)
          this.toastrService.message(error.message, "HATA", {
            messageType: ToasterType.Error,
            position: ToasterPosition.BottomRight
          })
        });

    }
  }
  onPasswordChange() {
    if (this.confirmPassword?.value == this.password?.value) {
      this.confirmPassword?.setErrors(null);
    } else {
      this.confirmPassword?.setErrors({ mismatch: true });
    }
  }
}

export function nameValidation(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let name: string = control?.value;
    let arr = name?.split(' ');
    if (arr)
      return arr[1]?.length ?? 0 > 2 ? null : { nameError: true }
    else
      return null;
  }
}