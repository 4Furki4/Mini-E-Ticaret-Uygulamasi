import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, of } from 'rxjs';
import { CustomToasterService, ToasterPosition, ToasterType } from '../ui/toaster/custom-toaster.service';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerHttpInterceptorService implements HttpInterceptor {

  constructor(private customToastr: CustomToasterService, private router: Router) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(err => {
      switch (err.status) {
        case HttpStatusCode.Unauthorized:
          this.router.navigate(['login'], {
            queryParams: {
              returnUrl: this.router.url
            }
          })
          this.customToastr.message('Lütfen tekrar giriş yapmayı deneyiniz.', 'GİRİŞ YAPILMADI!', {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
        case HttpStatusCode.Forbidden:
          this.customToastr.message('Yetkisiz işlem yapmaya çalıştınız, lütfen site yetkilisiyle iletişime geçiniz!'
            , 'YETKİSİZ İŞLEM!', {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
        case HttpStatusCode.NotFound:
          this.customToastr.message("Yapmak istediğiniz işlem sunucuda bulunamadı", 'İŞLEM BULUNAMADI!', {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
        case HttpStatusCode.BadRequest:
          this.customToastr.message("Yaptığınız işlemi lütfen gözden geçirip tekrar deneyiniz.", "İŞLEM GERÇEKLEŞTİRİLEMEDİ!", {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
        case HttpStatusCode.InternalServerError:
          this.customToastr.message("İşlem, sunucu tarafında gerçekleşen bir hatadan ötürü tamamlanamadı."
            , "SUNUCU HATASI!", {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
        default:
          this.customToastr.message("İşlem, bilinmeyen bir hatadan ötürü tamamlanamadı."
            , "BİLİNMEYEN HATA!", {
            messageType: ToasterType.Error,
            position: ToasterPosition.TopRight
          })
          break;
      }
      return of(err);
    }))
  }
}
