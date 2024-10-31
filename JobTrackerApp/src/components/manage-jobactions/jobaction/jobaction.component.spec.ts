import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobactionComponent } from './jobaction.component';

describe('JobactionComponent', () => {
  let component: JobactionComponent;
  let fixture: ComponentFixture<JobactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobactionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(JobactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
