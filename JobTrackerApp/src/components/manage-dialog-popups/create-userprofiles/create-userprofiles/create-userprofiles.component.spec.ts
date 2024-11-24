import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUserprofilesComponent } from './create-userprofiles.component';

describe('CreateUserprofilesComponent', () => {
  let component: CreateUserprofilesComponent;
  let fixture: ComponentFixture<CreateUserprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateUserprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateUserprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
