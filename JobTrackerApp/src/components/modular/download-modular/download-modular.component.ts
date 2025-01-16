import { Component, Input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';

@Component({
  selector: 'app-download-modular',
  standalone: true,
  imports: 
  [
    MatIcon
  ],
  templateUrl: './download-modular.component.html',
  styleUrl: './download-modular.component.scss'
})
export class DownloadModularComponent {
  @Input() _jobProfileId: string | null = '';

  constructor(private _jobTrackerService: JobTrackerService) { }

  onDownloadEmployerProfile(): void {
    this._jobTrackerService.DownloadEmployerProfile(this._jobProfileId);
  }
}
