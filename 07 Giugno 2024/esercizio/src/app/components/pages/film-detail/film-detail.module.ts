import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilmDetailRoutingModule } from './film-detail-routing.module';
import { FilmDetailComponent } from './film-detail.component';


@NgModule({
  declarations: [
    FilmDetailComponent
  ],
  imports: [
    CommonModule,
    FilmDetailRoutingModule
  ]
})
export class FilmDetailModule { }
