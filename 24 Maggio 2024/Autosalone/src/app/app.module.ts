import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './main/navbar/navbar.component';
import { FooterComponent } from './main/footer/footer.component';
import { HomeComponent } from './pages/home/home.component';
import { FiatPageComponent } from './pages/fiat-page/fiat-page.component';
import { FordPageComponent } from './pages/ford-page/ford-page.component';
import { AudiPageComponent } from './pages/audi-page/audi-page.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    FiatPageComponent,
    FordPageComponent,
    AudiPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
