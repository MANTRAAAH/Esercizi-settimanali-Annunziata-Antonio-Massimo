import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';
import { iUser } from '../../../models/i-user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  newUser: iUser = {
    id: 0,
    name: '',
    email: '',
    password: ''
  };
  registerForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private auth: AuthService,private router:Router) {
  this.registerForm = this.formBuilder.group({});
}
ngOnInit(): void {
  this.registerForm = this.formBuilder.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });
}

register() {
  if (this.registerForm.valid) {
    this.newUser = this.registerForm.value;
    this.auth.register(this.newUser).subscribe({
      next: () => {
        console.log('user registered');
        this.router.navigate(['/']); // reindirizza alla home
      },
      error: (err) => console.error('Registration failed', err)
    });
  } else {
    console.log('Form is not valid');
  }
}
}
