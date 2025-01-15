import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-upload-modular',
  standalone: true,
  imports: 
  [
    MatIcon
  ],
  templateUrl: './upload-modular.component.html',
  styleUrl: './upload-modular.component.scss'
})
export class UploadModularComponent {

  onUploadEmployerProfile(): void {
    console.log('Upload Employer Profile');
  }
}
