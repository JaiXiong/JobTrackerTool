import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogModularComponent } from './dialog-modular.component';

describe('DialogModularComponent', () => {
  let component: DialogModularComponent;
  let fixture: ComponentFixture<DialogModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
