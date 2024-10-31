import { Component, ViewEncapsulation } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import {NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTab, MatTabGroup, MatTabsModule } from '@angular/material/tabs'; // Import MatTabsModule
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: 
  [
    RouterModule,
    RouterOutlet, 
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatTabGroup,
    MatTab,
    NgIf,
    MatIconModule,
  ],
  providers: 
  [
    provideAnimations(),
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'JobTrackerApp';
}
