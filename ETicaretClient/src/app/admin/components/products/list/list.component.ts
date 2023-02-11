import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { List } from 'src/app/Contracts/List';
import { ListProduct } from 'src/app/Contracts/list-product';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
import { ProductService } from 'src/app/services/admin/models/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {
  displayedColumns: string[] = ['name', 'price', 'stock', 'createdDate', 'updatedDate', 'delete', 'edit'];
  dataSource!: MatTableDataSource<ListProduct>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private productService: ProductService, private alertify: AlertifyService, spinner: NgxSpinnerService) {
    super(spinner);
  }
  async getProducts() {
    this.showSpinner(SpinnerTypes.Ball8Bits);
    let data: List | undefined = await this.productService.read(
      this.paginator ? this.paginator.pageIndex : 0,
      this.paginator ? this.paginator.pageSize : 5,
      () => this.hideSpinner(SpinnerTypes.Ball8Bits),
      (error) => {
        this.hideSpinner(SpinnerTypes.Ball8Bits);
        this.alertify.message(error, { alertType: AlertType.Error, delay: 5000, position: Position.TopRight });
      });
    this.dataSource = new MatTableDataSource<ListProduct>(data?.products);
    this.paginator.length = data?.totalSize || 0;
  }
  async pageChanged() {
    await this.getProducts();
  }
  async ngOnInit(): Promise<void> {
    await this.getProducts();
  }

}
