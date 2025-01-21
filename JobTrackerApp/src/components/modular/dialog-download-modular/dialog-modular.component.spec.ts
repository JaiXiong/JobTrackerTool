import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDownloadModularComponent } from './dialog-modular.component';

describe('DialogModularComponent', () => {
  let component: DialogDownloadModularComponent;
  let fixture: ComponentFixture<DialogDownloadModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogDownloadModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogDownloadModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
