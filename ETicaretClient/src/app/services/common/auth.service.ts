import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { debug } from 'console';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private JwtHelper: JwtHelperService) {

  }

  IdentityCheck() {
    let token: string | null = localStorage.getItem('token');
    let isExpired: boolean;
    try {
      isExpired = this.JwtHelper.isTokenExpired(token as string);
    } catch {
      isExpired = true;
    }
    isAuthenticated = token != null && !isExpired;
  }


  get IsAuthenticated() {
    return isAuthenticated;
  }
}



export var isAuthenticated !: boolean;
