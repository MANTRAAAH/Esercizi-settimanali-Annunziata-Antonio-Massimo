import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { iLoginData } from '../../../models/i-login-data';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
authData:iLoginData={email:'',password:''}
constructor(private auth:AuthService,private router:Router) { }
login(){
  this.auth.login(this.authData).subscribe({
    next:()=>this.router.navigate(['/'])
  })
}
}
