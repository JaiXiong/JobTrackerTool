import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogEditJobprofilesComponent } from './dialog-edit-jobprofiles.component';

describe('DialogEditJobprofilesComponent', () => {
  let component: DialogEditJobprofilesComponent;
  let fixture: ComponentFixture<DialogEditJobprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogEditJobprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogEditJobprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
