import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [{ path: 'auth', loadChildren: () => import('./components/auth/auth.module').then(m => m.AuthModule) }, { path: '', loadChildren: () => import('./components/pages/home/home.module').then(m => m.HomeModule) }, { path: 'favorites', loadChildren: () => import('./components/pages/favorites/favorites.module').then(m => m.FavoritesModule) }, { path: 'film-detail', loadChildren: () => import('./components/pages/film-detail/film-detail.module').then(m => m.FilmDetailModule) }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
