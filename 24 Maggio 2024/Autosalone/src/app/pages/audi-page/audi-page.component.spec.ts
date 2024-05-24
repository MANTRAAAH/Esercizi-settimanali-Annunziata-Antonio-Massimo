import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AudiPageComponent } from './audi-page.component';

describe('AudiPageComponent', () => {
  let component: AudiPageComponent;
  let fixture: ComponentFixture<AudiPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AudiPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AudiPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
