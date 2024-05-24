import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import{HomeComponent} from './pages/home/home.component';
import { AudiPageComponent } from './pages/audi-page/audi-page.component';
import { FiatPageComponent } from './pages/fiat-page/fiat-page.component';
import { FordPageComponent } from './pages/ford-page/ford-page.component';


const routes: Routes = [
  {path:'', redirectTo:'home', pathMatch:'full'},
  {path:'home', component:HomeComponent},
  {path:'audi-page', component:AudiPageComponent},
  {path:'fiat-page', component:FiatPageComponent},
  {path:'ford-page', component:FordPageComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
