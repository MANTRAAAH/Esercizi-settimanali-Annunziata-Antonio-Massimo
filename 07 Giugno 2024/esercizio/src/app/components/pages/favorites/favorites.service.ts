import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { iFavoriteMovie } from '../../../models/i-favorite-movie';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FavoriteService {
  private apiUrl = 'http://localhost:3000/favorites';

  constructor(private http: HttpClient) { }

  addToFavorites(favorite: iFavoriteMovie): Observable<iFavoriteMovie> {
    return this.http.post<iFavoriteMovie>(this.apiUrl, favorite);
  }
}
