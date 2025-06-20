import { Component, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import {CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTab, MatTabGroup, MatTabsModule } from '@angular/material/tabs'; // Import MatTabsModule
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LogoutbuttonModularComponent } from '../components/modular/logoutbutton-modular/logoutbutton-modular/logoutbutton-modular.component';

// Used this to check the environment, during prod only
// import { environment } from '../environments/environment';
// console.log('Current environment:', environment);

@Component({
  selector: 'app-root',
  standalone: true,
  imports: 
  [
    RouterModule,
    RouterOutlet, 
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
    LogoutbuttonModularComponent
  ],
  providers: 
  [
    //provideAnimations(), for some reason this was causing my routing to append on top of each other to I commented it out
    //provideAnimations(),
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'JobTrackerApp';
  constructor(private router: Router) {}

  public mainpage() {
    this.router.navigate(['/jobprofile']);
  }

  public logout(): void {
    this.router.navigate(['/login']);
  }
}
