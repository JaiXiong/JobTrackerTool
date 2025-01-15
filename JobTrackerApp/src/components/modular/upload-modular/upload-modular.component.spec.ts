import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadModularComponent } from './upload-modular.component';

describe('UploadModularComponent', () => {
  let component: UploadModularComponent;
  let fixture: ComponentFixture<UploadModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
