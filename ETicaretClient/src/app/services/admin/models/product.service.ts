import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { List } from 'src/app/Contracts/List';
import { ListProduct } from 'src/app/Contracts/list-product';
import { ListProductImage } from 'src/app/Contracts/list-product-image';
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

  async read(page: number = 0, size: number = 5, successCallBack: () => void, errorCallBack: (errorResponse: string) => void): Promise<List | undefined> {
    const promiseVal: Promise<List | undefined> = this.httpClientService.get<List>({
      controller: 'products',
      queryString: `page=${page}&size=${size}`
    }).toPromise();

    promiseVal.then(d => successCallBack())
      .catch((errorResponse: HttpErrorResponse) => errorCallBack(errorResponse.message));

    return await promiseVal;
  }

  async delete(id: string) {
    const deletedObservable: Observable<any> = this.httpClientService.delete({
      controller: 'products',
    }, id);
    await firstValueFrom(deletedObservable)
  }

  async readImagesAsync(id: string): Promise<ListProductImage[]> {
    const getObservable: Observable<ListProductImage[]> = this.httpClientService.get<ListProductImage[]>({
      controller: 'products',
      action: 'images'
    }, id);
    return await firstValueFrom(getObservable);
  }

  async deleteImage(id: string, imageId: string, successCallBack?: () => void, errorCallBack?: () => void) {
    const deletedObservable = this.httpClientService.delete({
      controller: 'products',
      action: 'images',
      queryString: `imageId=${imageId}`,
    }, id)
    await firstValueFrom(deletedObservable).then(() => {
      if (successCallBack) {
        successCallBack();
      }
    }).catch(() => {
      if (errorCallBack) {
        errorCallBack();
      }
    });
  }

}
