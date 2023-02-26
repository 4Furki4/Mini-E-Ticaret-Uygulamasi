import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private formBuilder: FormBuilder) {

  }

  form !: FormGroup;
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      userName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      password: ['', Validators.required]
    })
  }

  get userName() {
    return this.form.get('userName')
  }
  get password() {
    return this.form.get('password')
  }

  onSubmit(formValues: any, ngForm: FormGroupDirective) {
    console.log(formValues);
    console.log(ngForm);
  }

}
