import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { delay, finalize, Observable } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  constructor(private spinnerService: NgxSpinnerService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    this.showSpinner();
    return next.handle(request).pipe(
      finalize(() => {
        this.hideSpinnder();
      })
    );
  }

  showSpinner() {
    this.spinnerService.show(undefined, {
      type: 'square-jelly-box',
      bdColor: 'rgba(0,0,0,0.5)',
      color: '#ffffff',
      fullScreen: true,
    });
  }

  hideSpinnder() {
    this.spinnerService.hide();
  }
}
