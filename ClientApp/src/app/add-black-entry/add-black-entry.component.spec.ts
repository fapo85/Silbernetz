import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddBlackEntryComponent } from './add-black-entry.component';

describe('AddBlackEntryComponent', () => {
  let component: AddBlackEntryComponent;
  let fixture: ComponentFixture<AddBlackEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddBlackEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddBlackEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
