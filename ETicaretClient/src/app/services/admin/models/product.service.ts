import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { HttpClientService } from '../../common/http-client.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }

  postProduct(product: CreateProduct, successCallBack: () => void, errorCallBack: (val: string) => void) {
    this.httpClientService.post<CreateProduct>({
      controller: 'products'
    }, product).subscribe({
      next: (response) => {
        successCallBack();
      },
      error: (errorResponse: HttpErrorResponse) => {
        const errors: Array<{ key: string, value: Array<string> }> = errorResponse.error;
        let message = '';
        errors.forEach(error => {
          error.value.forEach(value => {
            message += `${value}<br>`
          })
        })
        errorCallBack(message);
      }
    })
  }

}
