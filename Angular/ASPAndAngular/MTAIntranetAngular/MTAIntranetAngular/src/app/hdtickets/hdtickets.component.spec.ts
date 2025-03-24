import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HdTicketsComponent } from './hdtickets.component';

describe('TicketsComponent', () => {
  let component: HdTicketsComponent;
  let fixture: ComponentFixture<HdTicketsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HdTicketsComponent]
    });
    fixture = TestBed.createComponent(HdTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
