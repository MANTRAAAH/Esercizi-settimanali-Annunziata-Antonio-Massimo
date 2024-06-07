import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { iFavoriteMovie } from '../../models/i-favorite-movie';
import { iMovie } from '../../models/i-movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {


  private apiUrl = 'http://localhost:3000/movies-popular';
  private favoriteUrl = 'http://localhost:3000/favorites';

  constructor(private http: HttpClient) { }
  getMovie(id: number): Observable<any> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<any>(url);
  }
  getPopularMovies(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
  addFavorite(favorite: iFavoriteMovie): Observable<any> {
    return this.http.post(`${this.favoriteUrl}`, favorite);
  }

}
