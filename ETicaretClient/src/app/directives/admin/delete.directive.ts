import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { ProductService } from 'src/app/services/admin/models/product.service';
import { HttpClientService } from 'src/app/services/common/http-client.service';
declare var $: any;
@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(private elementRef: ElementRef,
    private renderer: Renderer2,
    private productService: ProductService) {

    const imgEl = this.renderer.createElement('img');
    imgEl.setAttribute('src', '../../../../../assets/trash.png');
    imgEl.setAttribute('alt', 'delete');
    imgEl.setAttribute('width', '20');
    imgEl.setAttribute('height', '20');
    imgEl.setAttribute('style', 'cursor: pointer');
    this.renderer.appendChild(this.elementRef.nativeElement, imgEl);
  }
  @Input() id: string = '';
  @Output() deleteCallBack: EventEmitter<any> = new EventEmitter();
  @HostListener('click')
  async onCLick() {
    const td: HTMLTableCellElement = this.elementRef.nativeElement as HTMLTableCellElement;
    await this.productService.delete(this.id);
    $(td.parentElement).fadeOut(500, () => {
      this.deleteCallBack.emit();
    })
  }
}
