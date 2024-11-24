import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogUserprofilesComponent } from './dialog-userprofiles.component';

describe('CreateUserprofilesComponent', () => {
  let component: DialogUserprofilesComponent;
  let fixture: ComponentFixture<DialogUserprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogUserprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogUserprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
