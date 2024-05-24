import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
dropdownOpen = false;
isNavbarCollapsed = true;

toggleDropdown() {
  this.dropdownOpen = !this.dropdownOpen;
}
}
