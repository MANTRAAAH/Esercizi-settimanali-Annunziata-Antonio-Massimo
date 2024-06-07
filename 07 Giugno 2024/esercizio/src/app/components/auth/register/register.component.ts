import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';
import { iUser } from '../../../models/i-user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  newUser: Partial<iUser> = {};
  registerForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private auth: AuthService) {
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
  this.newUser = this.registerForm.value;
  this.auth.register(this.newUser).subscribe({
    next: () => console.log('user registered')
  })
}
}
