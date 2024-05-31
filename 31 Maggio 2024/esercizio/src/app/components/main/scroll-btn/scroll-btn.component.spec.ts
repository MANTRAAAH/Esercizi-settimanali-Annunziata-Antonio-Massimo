import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScrollBtnComponent } from './scroll-btn.component';

describe('ScrollBtnComponent', () => {
  let component: ScrollBtnComponent;
  let fixture: ComponentFixture<ScrollBtnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ScrollBtnComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ScrollBtnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
