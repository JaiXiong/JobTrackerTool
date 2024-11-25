import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomEmployersnackbarComponent } from './custom-employersnackbar.component';

describe('CustomEmployersnackbarComponent', () => {
  let component: CustomEmployersnackbarComponent;
  let fixture: ComponentFixture<CustomEmployersnackbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomEmployersnackbarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomEmployersnackbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
