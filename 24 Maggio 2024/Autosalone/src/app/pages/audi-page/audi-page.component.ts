import { Component, OnInit } from '@angular/core';
import { iCars } from '../../interfaces/cars';

@Component({
  selector: 'app-audi-page',
  templateUrl: './audi-page.component.html',
  styleUrl: './audi-page.component.scss'
})
export class AudiPageComponent implements OnInit{
  cars: iCars[] = [];

async ngOnInit() {
  this.cars = await this.getCars();
  this.cars = this.cars.filter(car => car.brand.toLowerCase() === 'audi');
  console.log(this.cars);
}

  async getCars(): Promise<iCars[]> {
    const response = await fetch('../../../assets/db.json');
    return await response.json();
  }
  getBrandLogo(brand: string): string {
    const car = this.cars.find(car => car.brand.toLowerCase() === brand.toLowerCase());
    return car ? car.brandLogo : '';
  }
}
