import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { iFavoriteMovie } from '../../../models/i-favorite-movie';
import { AuthService } from '../../auth/auth.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class profileService {
  private favoritesSource = new BehaviorSubject<iFavoriteMovie[]>([]);
  favorites$ = this.favoritesSource.asObservable();

  constructor(private http: HttpClient, private authService: AuthService) { }

  addFavorite(favorite: any): Observable<any> {
    return this.http.post<any>('http://localhost:3000/favorites', favorite);
  }

  getFavorites(): Observable<iFavoriteMovie[]> {
    const url = 'http://localhost:3000/favorites';
    return this.http.get<iFavoriteMovie[]>(url);
  }

  pushFavoritesToServer(favorite: iFavoriteMovie) {
    const url = 'http://localhost:3000/favorites';
    return this.http.post(url, favorite);
  }

  getFavoritesByUserId(): Observable<iFavoriteMovie[]> {
    const userId = this.authService.getAccessData()?.user.id;
    const url = `http://localhost:3000/favorites/${userId}`;
    return this.http.get<iFavoriteMovie[]>(url);
  }
getUserById(): Observable<any> {
  const userId = this.authService.getAccessData()?.user.id;
  const url = `http://localhost:3000/users/${userId}`;


  return this.http.get<any>(url);
}
getAllUsersExceptLogged(): Observable<any[]> {
  const userId = this.authService.getAccessData()?.user.id;
  const url = `http://localhost:3000/users`;

  return this.http.get<any[]>(url).pipe(
    map(users => users.filter(user => user.id !== userId))
  );
}
}
