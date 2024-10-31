import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobprofileComponent } from './jobprofile.component';

describe('JobprofileComponent', () => {
  let component: JobprofileComponent;
  let fixture: ComponentFixture<JobprofileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobprofileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobprofileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
