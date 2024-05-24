import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FiatPageComponent } from './fiat-page.component';

describe('FiatPageComponent', () => {
  let component: FiatPageComponent;
  let fixture: ComponentFixture<FiatPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FiatPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FiatPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
