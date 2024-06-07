import { Component, OnInit } from '@angular/core';
import { FavoriteService } from './profile.service';
import { MovieService } from '../../../shared/movie/movie.service';
import { AuthService } from '../../auth/auth.service';



@Component({
  selector: 'app-favorites',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class FavoritesComponent implements OnInit {
  favorites: any[] = [];
  movies: any[] = [];
  favoriteMovies: any[] = [];
  name: string='';
  email: string='';

  constructor(
    private favoriteService: FavoriteService,
    private movieService: MovieService,
    private authService: AuthService
  ) { }

ngOnInit() {
  this.favoriteService.getUserById().subscribe(
    user => {
      this.name = user.name;
      this.email = user.email;
      console.log(user);
    },
    error => {
      console.error('Errore durante il recupero dei dettagli dell\'utente:', error);
    }
  );

  const accessData = this.authService.getAccessData();
  const userId = accessData?.user.id;

this.movieService.getPopularMovies().subscribe(
  movies => {
    this.movies = movies;

    this.favoriteService.getFavorites().subscribe(
      favorites => {
        this.favorites = favorites.filter(favorite => favorite.userId === userId);

        const uniqueMovies: { [key: number]: boolean } = {};
        this.favoriteMovies = this.favorites.map(favorite => {
          const movie = this.movies.find(movie => movie.id === favorite.movieId);
          if (!uniqueMovies[movie.id]) {
            uniqueMovies[movie.id] = true;
            return { ...favorite, movie };
          }
        }).filter(movie => movie);
      },
      error => {
        console.error('Errore durante il recupero dei preferiti:', error);
      }
    );
  },
  error => {
    console.error('Errore durante il recupero dei film:', error);
  }
);
}
}
