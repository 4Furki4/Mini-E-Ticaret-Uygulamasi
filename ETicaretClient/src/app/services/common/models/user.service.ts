import { SocialUser } from '@abacritt/angularx-social-login/public-api';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { CreateUserResponse } from 'src/app/Contracts/create-user-response';
import { Token } from 'src/app/Contracts/token/token';
import { TokenResponse } from 'src/app/Contracts/token/token-response';
import { User } from 'src/app/entities/user';
import { HttpClientService } from '../http-client.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClient: HttpClientService) { }

  async create(user: User): Promise<CreateUserResponse> {
    const observable: Observable<User | CreateUserResponse> = this.httpClient.post<User | CreateUserResponse>({
      action: "AppUser",
      controller: "Users",
    }, user);
    return await firstValueFrom(observable) as CreateUserResponse;
  }

  async login(usernameOrEmail: string, password: string): Promise<void> {
    const observable: Observable<any | Token> = this.httpClient.post({ action: 'login', controller: 'users' }, { usernameOrEmail, password });

    await firstValueFrom(observable).then((val: TokenResponse) => {
      localStorage.setItem('token', val.token.accessToken);
    });
  }

  async googleLogin(user: SocialUser): Promise<void> {
    let observable: Observable<SocialUser | TokenResponse> = this.httpClient.post<SocialUser | TokenResponse>({
      action: "google-login",
      controller: "users"
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
      controller: "users"
    }, user)

    await firstValueFrom(observable).then((value) => {
      value = value as TokenResponse;
      localStorage.setItem('token', value.token.accessToken);
    })
  }
}
