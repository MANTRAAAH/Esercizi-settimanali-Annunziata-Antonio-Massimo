import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { iFavoriteMovie } from '../../models/i-favorite-movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private apiUrl = 'http://localhost:3000/movies-popular';
  private favoriteUrl = 'http://localhost:3000/favorites';

  constructor(private http: HttpClient) { }

  getPopularMovies(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
  addFavorite(favorite: iFavoriteMovie): Observable<any> {
    return this.http.post(`${this.favoriteUrl}`, favorite);
  }
}
