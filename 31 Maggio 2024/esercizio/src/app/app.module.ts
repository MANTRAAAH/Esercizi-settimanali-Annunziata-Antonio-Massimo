import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavbarComponent } from './components/main/navbar/navbar.component';
import { FooterComponent } from './components/main/footer/footer.component';
import { HomeComponent } from './components/pages/home/home.component';
import { CompletedComponent } from './components/pages/completed/completed.component';
import { UsersComponent } from './components/pages/users/users.component';
import { FormsModule } from '@angular/forms';
import { ScrollBtnComponent } from './components/main/scroll-btn/scroll-btn.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    CompletedComponent,
    UsersComponent,
    ScrollBtnComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
