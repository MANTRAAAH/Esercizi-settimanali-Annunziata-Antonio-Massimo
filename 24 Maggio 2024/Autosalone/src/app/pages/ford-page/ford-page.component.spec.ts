import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FordPageComponent } from './ford-page.component';

describe('FordPageComponent', () => {
  let component: FordPageComponent;
  let fixture: ComponentFixture<FordPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FordPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FordPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
