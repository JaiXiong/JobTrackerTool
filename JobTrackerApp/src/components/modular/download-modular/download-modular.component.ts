import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

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
  onDownloadEmployerProfile(): void {
    console.log('Download Employer Profile');
  }
}
