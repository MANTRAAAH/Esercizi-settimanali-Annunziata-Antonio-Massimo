import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/pages/home/home.component';
import { CompletedComponent } from './components/pages/completed/completed.component';
import { UsersComponent } from './components/pages/users/users.component';

const routes: Routes = [
 {
    path: "",
    redirectTo: "home",
    pathMatch: 'full'
},
{
    path: "home",
    component: HomeComponent
},
{
    path: "completed",
    component: CompletedComponent
},
{
    path: "users",
    component: UsersComponent
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
