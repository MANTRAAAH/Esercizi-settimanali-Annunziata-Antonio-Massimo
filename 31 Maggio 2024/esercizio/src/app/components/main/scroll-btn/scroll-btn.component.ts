import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-scroll-btn',
  templateUrl: './scroll-btn.component.html',
  styleUrl: './scroll-btn.component.scss'
})
export class ScrollBtnComponent {
  showScrollTopButton = false;

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.showScrollTopButton = true;
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}
