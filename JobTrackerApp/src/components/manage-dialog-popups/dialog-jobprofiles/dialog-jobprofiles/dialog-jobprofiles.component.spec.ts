import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogJobprofilesComponent } from './dialog-jobprofiles.component';

describe('CreateJobprofilesComponent', () => {
  let component: DialogJobprofilesComponent;
  let fixture: ComponentFixture<DialogJobprofilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogJobprofilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogJobprofilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
