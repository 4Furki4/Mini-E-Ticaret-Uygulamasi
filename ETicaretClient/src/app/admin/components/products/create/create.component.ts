import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { CreateProduct } from 'src/app/Contracts/create-product';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
import { ProductService } from 'src/app/services/admin/models/product.service';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent extends BaseComponent implements OnInit {

  @Output() CreatedProduct: EventEmitter<CreateProduct> = new EventEmitter();
  constructor(spinner: NgxSpinnerService, private createService: ProductService, private alertify: AlertifyService) {
    super(spinner);
  }
  ngOnInit(): void {
  }
  options: Partial<FileUploadOptions> = {
    controller: "Products",
    action: "Upload",
    explanation: "Sürükleyerek veya seçerek ürün resmi ekleyiniz.",
    isAdminSide: true,
    accept: ".jpg,.jpeg,.png",

  }

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {

    this.showSpinner(SpinnerTypes.Ball8Bits);
    let product: CreateProduct = new CreateProduct();

    product.name = name.value;
    product.stock = parseInt(stock.value);
    product.price = parseFloat(price.value);

    this.createService.postProduct(product, () => {
      this.hideSpinner(SpinnerTypes.Ball8Bits);
      this.alertify.message("Ürün başarıyla oluşturuldu.", {
        alertType: AlertType.Success,
        position: Position.TopRight,
        dismissOthers: true
      });
      this.CreatedProduct.emit(product);
    },
      (message) => {
        this.hideSpinner(SpinnerTypes.Ball8Bits);
        this.alertify.message(message, {
          alertType: AlertType.Error,
          position: Position.TopRight,
          dismissOthers: true
        });
      }
    )
  }
}
