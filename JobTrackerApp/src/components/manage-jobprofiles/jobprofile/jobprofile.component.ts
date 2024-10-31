import { Component } from '@angular/core';

@Component({
  selector: 'app-jobprofile',
  standalone: true,
  imports: [],
  templateUrl: './jobprofile.component.html',
  styleUrl: './jobprofile.component.scss'
})
export class JobprofileComponent {
  download() {
    console.log('Download button clicked!');
  }
}
