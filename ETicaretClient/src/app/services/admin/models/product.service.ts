import { Injectable } from '@angular/core';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { HttpClientService } from '../../common/http-client.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }

  postProduct(product: CreateProduct, successCallBack?: any) {
    this.httpClientService.post<CreateProduct>({
      controller: 'products'
    }, product).subscribe(result => {
      successCallBack();
    })
  }

}
