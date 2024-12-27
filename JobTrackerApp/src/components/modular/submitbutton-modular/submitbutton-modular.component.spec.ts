import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitbuttonModularComponent } from './submitbutton-modular.component';

describe('SubmitbuttonModularComponent', () => {
  let component: SubmitbuttonModularComponent;
  let fixture: ComponentFixture<SubmitbuttonModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubmitbuttonModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubmitbuttonModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
