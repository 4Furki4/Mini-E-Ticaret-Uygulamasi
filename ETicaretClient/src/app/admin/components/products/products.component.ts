import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { Product } from 'src/app/Contracts/product';
import { HttpClientService } from 'src/app/services/common/http-client.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService, private httpService: HttpClientService) {
    super(spinner);
  }

  ngOnInit() {
    this.showSpinner()
    this.httpService.get<Product[]>({
      controller: 'products',
    }).subscribe({
      next: (data) => {
        console.log(data.forEach(product => console.log(`Ürün adı: ${product.name}`)));
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log("Request completed.");
      }
    });

    // this.httpService.post({
    //   controller: 'products'
    // }, {
    //   name: 'Test',
    //   stock: 10,
    //   price: 100
    // }).subscribe({
    //   next: (data) => {
    //     console.log(data);
    //   },
    //   error: (error) => {
    //     console.log(error);
    //   },
    //   complete: () => {
    //     console.log("Request completed.");
    //   }
    // })

    // this.httpService.put({
    //   controller: 'products'
    // }, {
    //   id: '9f1c52d7-d388-49ef-fb2f-08db049be384',
    //   name: 'Updated Test',
    //   stock: 31,
    //   price: 3131
    // }).subscribe({
    //   next: (data) => {
    //     console.log(data);
    //   },
    //   error: (error) => {
    //     console.log(error);
    //   },
    //   complete: () => {
    //     console.log("Request completed.");
    //   }
    // })

    // this.httpService.delete({
    //   controller: 'products'
    // }, "9f1c52d7-d388-49ef-fb2f-08db049be384").subscribe({
    //   next: (data) => {
    //     console.log(data);
    //   },
    //   error: (error) => {
    //     console.log(error);
    //   },
    //   complete: () => {
    //     console.log("Request completed.");
    //   }
    // })
  }
}
