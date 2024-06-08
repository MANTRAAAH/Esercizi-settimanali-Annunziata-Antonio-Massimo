import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { AuthGuard } from '../../auth/auth.guard';

const routes: Routes = [{ path: '', component: HomeComponent },
  { path: 'film-detail/:id', loadChildren: () => import('../film-detail/film-detail.module').then(m => m.FilmDetailModule),canActivate:[AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
