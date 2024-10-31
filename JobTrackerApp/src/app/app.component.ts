import { Component, ViewEncapsulation } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTab, MatTabGroup, MatTabsModule } from '@angular/material/tabs'; // Import MatTabsModule
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: 
  [
    RouterOutlet, 
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatTabGroup,
    MatTab,
    NgIf,
    MatIconModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'JobTrackerApp';

  download() {
    console.log('Download button clicked!');
  }
}
