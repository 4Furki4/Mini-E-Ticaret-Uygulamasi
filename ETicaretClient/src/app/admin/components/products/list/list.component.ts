import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerTypes } from 'src/app/base/base.component';
import { ListProduct } from 'src/app/Contracts/list-product';
import { AlertifyService, AlertType, Position } from 'src/app/services/admin/alertify/alertify.service';
import { ProductService } from 'src/app/services/admin/models/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {
  displayedColumns: string[] = ['name', 'price', 'stock', 'createdDate', 'updatedDate'];
  dataSource!: MatTableDataSource<ListProduct>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private productService: ProductService, private alertify: AlertifyService, spinner: NgxSpinnerService) {
    super(spinner);
  }

  async ngOnInit(): Promise<void> {
    this.showSpinner(SpinnerTypes.Ball8Bits);
    let products: ListProduct[] | undefined = await this.productService.read(
      () => this.hideSpinner(SpinnerTypes.Ball8Bits),
      (error) => {
        this.hideSpinner(SpinnerTypes.Ball8Bits);
        this.alertify.message(error, { alertType: AlertType.Error, delay: 5000, position: Position.TopRight });
      });
    debugger
    this.dataSource = new MatTableDataSource(products);
    this.dataSource.paginator = this.paginator;
  }

}
