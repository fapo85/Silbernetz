import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CallTodayComponent } from './call-today.component';

describe('CallTodayComponent', () => {
  let component: CallTodayComponent;
  let fixture: ComponentFixture<CallTodayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CallTodayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CallTodayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
