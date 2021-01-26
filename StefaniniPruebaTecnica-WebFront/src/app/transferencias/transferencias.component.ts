import { FormControl, FormGroup } from '@angular/forms';
import ArrayStore from 'devextreme/data/array_store';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../core/app.service';

@Component({
  selector: 'app-transferencias',
  templateUrl: './transferencias.component.html',
  styleUrls: ['./transferencias.component.css']
})
export class TransferenciasComponent implements OnInit {
  formTransferencias: FormGroup;
  cuentasBancarias: any;
  dataSource: any;

  constructor(private appService: AppService) {
    this.obtenerCuentasBancarias();
    this.obtenerTransferencias();
  }

  ngOnInit() {
    this.crearFormulario();
  }

  crearFormulario() {
    this.formTransferencias = new FormGroup({
      cuentaOrigenId: new FormControl(null),
      cuentaDestinoId: new FormControl(null),
      montoTransferir: new FormControl(null),
      cuentaRetiroId: new FormControl(null),
      montoRetirar: new FormControl(null)
    })
  }

  obtenerCuentasBancarias() {
    const model = {
      controlador: 'CuentaBancaria',
      accion: 'ObtenerCuentasBancarias'
    } as any;

    this.appService.get(model)
      .subscribe(resp => {
        this.cuentasBancarias = new ArrayStore({
          data: resp.CuentasBancarias,
          key: "Id"
        });
      });
  }

  obtenerTransferencias() {
    const model = {
      controlador: 'Transaccion',
      accion: 'ObtenerTransferencias'
    } as any;

    this.dataSource = this.appService.getDataSource(model);
  }


  transferir() {
    const model = {
      controlador: 'Transaccion',
      accion: 'Transferir',
      parametros: {
        CuentaBancariaId: this.formTransferencias.value.cuentaOrigenId,
        CuentaBancariaDestinoId: this.formTransferencias.value.cuentaDestinoId,
        Monto: this.formTransferencias.value.montoTransferir
      }
    } as any;

    this.appService.post(model)
      .subscribe(resp => {
        console.log(resp);
        this.obtenerTransferencias();
      });
  }

  retirar() {
    const model = {
      controlador: 'Transaccion',
      accion: 'Retirar',
      parametros: {
        CuentaBancariaId: this.formTransferencias.value.cuentaRetiroId,
        Monto: this.formTransferencias.value.montoRetirar
      }
    } as any;

    this.appService.post(model)
      .subscribe(resp => {
        console.log(resp);
        this.obtenerTransferencias();
      });

  }

}
