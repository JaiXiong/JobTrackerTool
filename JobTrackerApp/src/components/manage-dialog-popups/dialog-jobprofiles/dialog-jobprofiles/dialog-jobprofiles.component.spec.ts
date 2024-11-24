import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateJobprofilesComponent } from './dialog-jobprofiles.component';

describe('CreateJobprofilesComponent', () => {
  let component: CreateJobprofilesComponent;
  let fixture: ComponentFixture<CreateJobprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateJobprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateJobprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
