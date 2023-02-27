import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormGroupDirective, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { CreateUserResponse } from 'src/app/Contracts/create-user-response';
import { User } from 'src/app/entities/user';
import { UserService } from 'src/app/services/common/models/user.service';
import { CustomToasterService, ToasterPosition, ToasterType } from 'src/app/services/ui/toaster/custom-toaster.service';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private userService: UserService, private toastrService: CustomToasterService) {

  }

  form !: FormGroup
  ngOnInit(): void {
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
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    else {
      console.log(user);
      await this.userService.create(user).then((res) => {
        if (res.isSuccessfull) {
          this.toastrService.message(res.message, "Kayıt Başarılı", {
            messageType: ToasterType.Success,
            position: ToasterPosition.TopRight
          })
          ngForm.resetForm();
          this.form.reset();
        }
        else {
          this.toastrService.message(res.message, "Kayıt Başarısız", {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
        }

      })
        .catch((error: HttpErrorResponse) => {
          this.toastrService.message(error.message, "HATA", {
            messageType: ToasterType.Error,
            position: ToasterPosition.BottomRight
          })
        });

    }
  }
  onPasswordChange() {
    this.confirmPassword?.valueChanges.subscribe((value) => {

      if (this.confirmPassword?.value == this.password?.value) {
        this.confirmPassword?.setErrors(null);
      } else {
        this.confirmPassword?.setErrors({ mismatch: true });
      }
    })
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