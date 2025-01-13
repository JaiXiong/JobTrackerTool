import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SnackbarModularComponent } from './snackbar-modular.component';

describe('SnackbarModularComponent', () => {
  let component: SnackbarModularComponent;
  let fixture: ComponentFixture<SnackbarModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SnackbarModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SnackbarModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
