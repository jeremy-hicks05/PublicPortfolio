import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HdMyTicketsComponent } from './hdmytickets.component';

describe('TicketsComponent', () => {
  let component: HdMyTicketsComponent;
  let fixture: ComponentFixture<HdMyTicketsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HdMyTicketsComponent]
    });
    fixture = TestBed.createComponent(HdMyTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
