import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateEmployerprofilesComponent } from './create-employerprofiles.component';

describe('CreateEmployerprofilesComponent', () => {
  let component: CreateEmployerprofilesComponent;
  let fixture: ComponentFixture<CreateEmployerprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateEmployerprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateEmployerprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
