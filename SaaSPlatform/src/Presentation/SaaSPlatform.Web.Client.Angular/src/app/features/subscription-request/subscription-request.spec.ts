import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubscriptionRequestComponent } from './subscription-request';

describe('SubscriptionRequestComponent', () => {
  let component: SubscriptionRequestComponent;
  let fixture: ComponentFixture<SubscriptionRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubscriptionRequestComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubscriptionRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
