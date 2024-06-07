import { MovieService } from './../../../shared/movie/movie.service';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/auth.service';
import { iMovie } from '../../../models/i-movie';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoggedIn: boolean | undefined;
  movies: iMovie[] = [];
  username: string | null = null;

  constructor(private http: HttpClient, private authService: AuthService,private MovieService:MovieService) { }

ngOnInit() {

  this.authService.isLoggedIn$.subscribe((loggedIn) => {
    this.isLoggedIn = loggedIn;
  });
  this.MovieService.getPopularMovies().subscribe((data: iMovie[]) => {
    this.movies = data;
  });

  this.username = this.authService.getUsername();
}
}

