import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { ListProduct } from 'src/app/Contracts/list-product';
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

  async read(successCallBack: () => void, errorCallBack: (errorResponse: string) => void): Promise<Array<ListProduct> | undefined> {
    const promiseVal: Promise<Array<ListProduct> | undefined> = this.httpClientService.get<Array<ListProduct>>({
      controller: 'products'
    }).toPromise();

    promiseVal.then(d => successCallBack())
      .catch((errorResponse: HttpErrorResponse) => errorCallBack(errorResponse.message));

    return await promiseVal;
  }

}
