import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
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
    this.httpService.get({
      controller: 'products',
    }).subscribe({
      next: (data) => {
        console.log(data);
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log("Request completed.");
      }
    });

    this.httpService.post({
      controller: 'products'
    }, {
      name: 'Test',
      stock: 10,
      price: 100
    }).subscribe({
      next: (data) => {
        console.log(data);
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log("Request completed.");
      }
    })
  }
}
