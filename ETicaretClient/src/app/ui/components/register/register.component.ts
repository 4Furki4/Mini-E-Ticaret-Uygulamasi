import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { debug } from 'console';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) {

  }
  form !: FormGroup
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      fullName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.maxLength(40), Validators.pattern(/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z])(?=.*[\.\*!_@-])(?=.*[a-zA-Z]).{8,}$/)]],
      confirmPassword: ['', [Validators.required]]
    })
    this.form.value
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
  onSubmit(value: FormControl[], ngForm: FormGroupDirective) {
    this.submitted = true;
    if (!this.form.valid) {
      return;
    }
    else {
      this.form.reset();
      ngForm.resetForm();
    }
  }
}
