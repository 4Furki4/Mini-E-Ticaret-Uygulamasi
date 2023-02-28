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

  async login(usernameOrpassword: string, password: string): Promise<void> {
    const observable: Observable<any | Token> = this.httpClient.post({ action: 'login', controller: 'users' }, { usernameOrpassword, password });

    await firstValueFrom(observable).then((val: TokenResponse) => {
      localStorage.setItem('token', val.token.accessToken);
    });
  }
}
