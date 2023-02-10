import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { Product } from 'src/app/Contracts/product';
import { HttpClientService } from 'src/app/services/common/http-client.service';
import { ListComponent } from './list/list.component';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService, private httpService: HttpClientService) {
    super(spinner);
  }

  @ViewChild(ListComponent) listComponent!: ListComponent;
  ProductCreated(product: CreateProduct) {
    console.log(product)
    this.listComponent.getProducts();
  }
  ngOnInit() {
  }
}
