import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject,map,tap,Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { iUser } from '../../models/i-user';
import { iAuthResponse } from '../../models/i-auth-response';
import { iLoginData } from '../../models/i-login-data';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  JwtHelper:JwtHelperService = new JwtHelperService();
  authSubject = new BehaviorSubject<null|iUser>(null);
  isLogged:boolean = false;
  user$=this.authSubject.asObservable();
  isLoggedIn$=this.user$.pipe(
    map(user=>!!user),
    tap(user=>this.isLogged=user));
  constructor(private httpc:HttpClient,private router:Router) {
    this.restoreUser()
  }
  loginUrl:string="http://localhost:3000/login";
  registerUrl:string="http://localhost:3000/register";

register(newUser:Partial<iUser>):Observable<iAuthResponse>{
  return this.httpc.post<iAuthResponse>(this.registerUrl,newUser)

}
login(loginData:iLoginData):Observable<iAuthResponse>{
  return this.httpc.post<iAuthResponse>(this.loginUrl,loginData)
  .pipe(tap(data=>{
    this.authSubject.next(data.user)
    localStorage.setItem('datiAccesso',JSON.stringify(data))
  }))
  this.autoLogout()
}

logout():void{
  this.authSubject.next(null)
  localStorage.removeItem('datiAccesso')
  this.router.navigate(['/auth'])
}
getAccessData():iAuthResponse|null{
  const JSONDatiAccesso=localStorage.getItem('datiAccesso')
  if(!JSONDatiAccesso)return null

  const datiAccesso: iAuthResponse=JSON.parse(JSONDatiAccesso)
  return datiAccesso;


}
getUsername(): string | null {
  const accessData = this.getAccessData();
  if (!accessData) return null;

  return accessData.user.name;
}
autoLogout():void{
const datiAccesso=this.getAccessData()
if(!datiAccesso)return
  const expDate=this.JwtHelper.getTokenExpirationDate(datiAccesso.accessToken) as Date
  const expMs=expDate.getTime()-new Date().getTime()
  setTimeout(this.logout,expMs)


}

restoreUser():void{
  const datiAccesso=this.getAccessData()
  if(!datiAccesso)return
  if(this.JwtHelper.isTokenExpired(datiAccesso.accessToken)) return

  this.authSubject.next(datiAccesso.user)
  this.autoLogout()


}

}
