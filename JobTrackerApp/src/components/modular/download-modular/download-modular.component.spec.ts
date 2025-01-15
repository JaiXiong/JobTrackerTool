import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadModularComponent } from './download-modular.component';

describe('DownloadModularComponent', () => {
  let component: DownloadModularComponent;
  let fixture: ComponentFixture<DownloadModularComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DownloadModularComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DownloadModularComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
