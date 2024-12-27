import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClosebuttonModularComponent } from './closebutton-modular.component';

describe('ClosebuttonModularComponent', () => {
  let component: ClosebuttonModularComponent;
  let fixture: ComponentFixture<ClosebuttonModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClosebuttonModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClosebuttonModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
