import { SocialUser } from '@abacritt/angularx-social-login';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import { Observable, firstValueFrom } from 'rxjs';
import { TokenResponse } from 'src/app/Contracts/token/token-response';
import { HttpClientService } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserAuthService {

  constructor(private httpClient: HttpClientService) { }
  async login(usernameOrEmail: string, password: string): Promise<void> {
    const observable: Observable<any | Token> = this.httpClient.post({ action: 'login', controller: 'users' }, { usernameOrEmail, password });

    await firstValueFrom(observable).then((val: TokenResponse) => {
      localStorage.setItem('token', val.token.accessToken);
    });
  }

  async googleLogin(user: SocialUser): Promise<void> {
    let observable: Observable<SocialUser | TokenResponse> = this.httpClient.post<SocialUser | TokenResponse>({
      action: "google-login",
      controller: "Auth"
    }, user);
    await firstValueFrom(observable).then((value) => {
      value = value as TokenResponse;
      localStorage.setItem('token', value.token.accessToken);
    });
  }

  async facebookLogin(user: SocialUser): Promise<void> {
    console.log(user);

    let observable: Observable<SocialUser | TokenResponse> = this.httpClient.post<SocialUser | TokenResponse>({
      action: "facebook-login",
      controller: "Auth"
    }, user)

    await firstValueFrom(observable).then((value) => {
      value = value as TokenResponse;
      localStorage.setItem('token', value.token.accessToken);
    })
  }
}
