import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NumValueComponent } from './num-value.component';

describe('NumValueComponent', () => {
  let component: NumValueComponent;
  let fixture: ComponentFixture<NumValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NumValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NumValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
