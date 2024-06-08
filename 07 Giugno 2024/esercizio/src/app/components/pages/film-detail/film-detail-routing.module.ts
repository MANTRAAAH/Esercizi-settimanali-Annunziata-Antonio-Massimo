import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FilmDetailComponent } from './film-detail.component';
import { AuthGuard } from '../../auth/auth.guard';

const routes: Routes = [{ path: '', component: FilmDetailComponent}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FilmDetailRoutingModule { }
