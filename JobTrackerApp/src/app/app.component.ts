import { Component, EventEmitter, OnInit, Output, ViewEncapsulation } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import {CommonModule, NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTab, MatTabGroup, MatTabsModule } from '@angular/material/tabs'; // Import MatTabsModule
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LogoutbuttonModularComponent } from '../components/modular/logoutbutton-modular/logoutbutton-modular/logoutbutton-modular.component';
import { AuthService } from '../services/auth/auth.service';

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
export class AppComponent implements OnInit {
  title = 'JobTrackerApp';
  _isLoggedin: boolean = false;
  
  constructor(private router: Router, private authService: AuthService) {}

   ngOnInit() {
    // Check if user is already logged in (from token in localStorage)
    //this._isLoggedin = this.authService.isLoggedIn();
    this.authService.isLoggedIn$.subscribe(isLoggedIn => {
      this._isLoggedin = isLoggedIn;
    });
  }

  public mainpage() {
    this.router.navigate(['/jobprofile']);
  }

  public logout(): void {
    this.router.navigate(['/login']);
  }

  // this was used to be able to render the login/logout button in the header dynamically when it isn't being used as a parent component
  
  // public onLoginFromLogin(isLoggedIn: boolean): void {
  //   this._isLoggedin = isLoggedIn;
  // }

  // // Add this to your AppComponent class
  // public onActivate(componentRef: any): void {
  // // Check if the activated component is the login component
  //   if (componentRef.isLoggedIn) {
  //     // Subscribe to the isLoggedIn event from the login component
  //     componentRef.isLoggedIn.subscribe((loggedIn: boolean) => {
  //       this._isLoggedin = loggedIn;
  //     });
  //   }
  // }
}
