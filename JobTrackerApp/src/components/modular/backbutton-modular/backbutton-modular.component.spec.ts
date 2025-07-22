import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackbuttonModularComponent } from './backbutton-modular.component';

describe('BackbuttonModularComponent', () => {
  let component: BackbuttonModularComponent;
  let fixture: ComponentFixture<BackbuttonModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackbuttonModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BackbuttonModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
