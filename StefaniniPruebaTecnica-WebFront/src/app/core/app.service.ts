import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import * as AspNetData from "devextreme-aspnet-data-nojquery";

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable()
export class AppService {
  dataSource: any;

  constructor(private http: HttpClient) { }

  getDataSource(model: any): any {
    return AspNetData.createStore({
      loadUrl: environment.apiUrl + '/' + model.controlador + '/' + model.accion
    });
  }

  get(model: any): Observable<any> {
    if (model.parametros != undefined) {
      const body = JSON.stringify(model.parametros);
      return this.http.get<any>(environment.apiUrl + model.controlador)
        .pipe(
          retry(1),
          catchError(this.handleError)
        )
    }
    else {
      return this.http.get<any>(environment.apiUrl + '/' + model.controlador + '/' + model.accion)
        .pipe(
          retry(1),
          catchError(this.handleError)
        )
    }
  }

  post(model: any): Observable<any> {
    const body = JSON.stringify(model.parametros);
    return this.http.post<any>(environment.apiUrl + '/' + model.controlador + '/' + model.accion, body, httpOptions)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  handleError(error) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Obtener error de client-side
      errorMessage = error.error.message;
    } else {
      // Obtener error de server-side
      errorMessage = 'Error Code: ${error.status}\n Message: ${error.message}';
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }

}