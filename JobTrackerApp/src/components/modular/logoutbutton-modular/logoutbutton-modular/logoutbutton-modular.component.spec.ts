import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogoutbuttonModularComponent } from './logoutbutton-modular.component';

describe('LogoutbuttonModularComponent', () => {
  let component: LogoutbuttonModularComponent;
  let fixture: ComponentFixture<LogoutbuttonModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LogoutbuttonModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LogoutbuttonModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
