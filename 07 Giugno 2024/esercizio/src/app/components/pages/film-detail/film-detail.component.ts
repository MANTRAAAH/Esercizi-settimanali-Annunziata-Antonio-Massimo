import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from '../../../shared/movie/movie.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-film-detail',
  templateUrl: './film-detail.component.html',
  styleUrls: ['./film-detail.component.scss']
})
export class FilmDetailComponent implements OnInit {
  movie: any;

  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService
  ) {}

  ngOnInit() {
    this.getMovie();
  }

getMovie(): void {
  const id = this.route.snapshot.paramMap.get('id');
  if (id !== null) {
    this.movieService.getMovie(+id)
      .subscribe(movie => {
        this.movie = movie;
        console.log(movie);
      });
  }
}
}
