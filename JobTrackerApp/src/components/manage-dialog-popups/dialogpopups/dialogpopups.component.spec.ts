import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogpopupsComponent } from './dialogpopups.component';

describe('DialogpopupsComponent', () => {
  let component: DialogpopupsComponent;
  let fixture: ComponentFixture<DialogpopupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DialogpopupsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DialogpopupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
