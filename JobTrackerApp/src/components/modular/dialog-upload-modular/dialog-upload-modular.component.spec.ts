import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogUploadModularComponent } from './dialog-upload-modular.component';

describe('DialogUploadModularComponent', () => {
  let component: DialogUploadModularComponent;
  let fixture: ComponentFixture<DialogUploadModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogUploadModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogUploadModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
