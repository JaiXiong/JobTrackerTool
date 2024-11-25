import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DialogEmployerprofilesComponent } from './dialog-employerprofiles.component';



describe('CreateEmployerprofilesComponent', () => {
  let component: DialogEmployerprofilesComponent;
  let fixture: ComponentFixture<DialogEmployerprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogEmployerprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogEmployerprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
