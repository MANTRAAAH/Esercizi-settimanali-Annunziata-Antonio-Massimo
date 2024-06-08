import { profileService } from './../../components/pages/Profilo/profile.service';
import { Component, OnInit } from '@angular/core';
import { MovieService } from './movie.service';
import { AuthService } from '../../components/auth/auth.service';
import { iMovie } from '../../models/i-movie';
import { iFavoriteMovie } from '../../models/i-favorite-movie';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.scss']
})
export class MovieComponent implements OnInit {
  movies: iMovie[] = [];
  isLoggedIn: boolean | undefined;
  isLiked = false


  constructor(private movieService: MovieService, private authService: AuthService,private profileService:profileService) { }

  ngOnInit() {
    this.authService.isLoggedIn$.subscribe((loggedIn) => {
      this.isLoggedIn = loggedIn;
    });
    this.movieService.getPopularMovies().subscribe((data: iMovie[]) => {
      this.movies = data;
    });
  }



  likeMovie(movieId: number) {
    if (this.isLoggedIn) {
      const accessData = this.authService.getAccessData();
      if (accessData) {
        const favorite: iFavoriteMovie = {
          userId: accessData.user.id,
          movieId: Number(movieId)
        };

        this.profileService.addFavorite(favorite).subscribe(response => {
          console.log(response);
          Swal.fire({
            icon: 'success',
            title: 'Aggiunto ai preferiti',
            text: 'Il film è stato aggiunto ai tuoi preferiti.',
            confirmButtonText: 'OK'
          });
        });
      }
    }
  }
}
