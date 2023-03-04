import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { CreateUserResponse } from 'src/app/Contracts/create-user-response';
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
      controller: "Auth",
    }, user);
    return await firstValueFrom(observable) as CreateUserResponse;
  }
}
