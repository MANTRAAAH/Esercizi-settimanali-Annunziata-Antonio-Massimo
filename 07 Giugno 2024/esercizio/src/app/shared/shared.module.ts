import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MovieComponent } from './movie/movie.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    MovieComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    MovieComponent
  ]
})
export class SharedModule { }
