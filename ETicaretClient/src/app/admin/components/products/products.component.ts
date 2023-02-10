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

    this.transitionSpinner(SpinnerTypes.Ball8Bits)
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
  }
}
