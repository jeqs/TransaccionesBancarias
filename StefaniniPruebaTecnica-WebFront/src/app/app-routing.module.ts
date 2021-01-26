import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TransferenciasComponent } from './transferencias/transferencias.component';

const routes: Routes = [
  { path: '', redirectTo: '/transferencias', pathMatch: 'full' },
  { path: 'transferencias', component: TransferenciasComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
