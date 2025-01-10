import { TestBed } from '@angular/core/testing';

import { JobtrackerService } from './jobtracker.service';

describe('JobtrackerService', () => {
  let service: JobtrackerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobtrackerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
