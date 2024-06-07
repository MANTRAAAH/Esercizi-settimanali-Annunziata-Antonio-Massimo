import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './components/auth/auth.guard';

const routes: Routes = [{ path: 'auth', loadChildren: () => import('./components/auth/auth.module').then(m => m.AuthModule)},
  { path: '', loadChildren: () => import('./components/pages/home/home.module').then(m => m.HomeModule) },
  { path: 'profile', loadChildren: () => import('./components/pages/Profilo/profile.module').then(m => m.FavoritesModule),canActivate:[AuthGuard],canActivateChild:[AuthGuard] },
  { path: 'film-detail', loadChildren: () => import('./components/pages/film-detail/film-detail.module').then(m => m.FilmDetailModule) ,canActivate:[AuthGuard],canActivateChild:[AuthGuard]}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
