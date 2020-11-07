import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AbnormalDetailComponent } from './abnormal-detail.component';

describe('AbnormalDetailComponent', () => {
  let component: AbnormalDetailComponent;
  let fixture: ComponentFixture<AbnormalDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbnormalDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbnormalDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
