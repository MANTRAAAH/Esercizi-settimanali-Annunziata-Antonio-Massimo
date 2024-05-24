import { iCars } from './../../interfaces/cars';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  cars: iCars[] = [];
  randomCars: iCars[] = [];
  uniqueBrands: string[] = [];
  getBrandLogo(brand: string): string {
    const car = this.cars.find(car => car.brand === brand);
    return car ? car.brandLogo : '';
  }

  constructor(private router: Router) {}

  async ngOnInit() {
    this.cars = await this.getCars();
    this.randomCars = this.getRandomCars();
    this.uniqueBrands = [...new Set(this.cars.map(car => car.brand))];
  }

  async getCars(): Promise<iCars[]> {
    const response = await fetch('../../../assets/db.json');
    return await response.json();
  }

  getRandomCars(): iCars[] {
    let randomIndex1, randomIndex2;

    do {
      randomIndex1 = Math.floor(Math.random() * this.cars.length);
      randomIndex2 = Math.floor(Math.random() * this.cars.length);
    } while (randomIndex1 === randomIndex2);

    return [this.cars[randomIndex1], this.cars[randomIndex2]];
  }

  navigateToBrand(brand: string) {
    this.router.navigate([`/${brand.toLowerCase()}-page`]);
  }
}
