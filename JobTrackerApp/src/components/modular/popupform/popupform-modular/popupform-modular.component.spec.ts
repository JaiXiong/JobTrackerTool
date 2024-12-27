import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopupformModularComponent } from './popupform-modular.component';

describe('PopupformModularComponent', () => {
  let component: PopupformModularComponent;
  let fixture: ComponentFixture<PopupformModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PopupformModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopupformModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
