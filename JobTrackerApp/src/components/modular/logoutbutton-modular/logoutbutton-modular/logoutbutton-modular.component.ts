import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-logoutbutton-modular',
  standalone: true,
  imports: 
  [
    RouterModule,
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule
  ],
  templateUrl: './logoutbutton-modular.component.html',
  styleUrl: './logoutbutton-modular.component.scss'
})
export class LogoutbuttonModularComponent {
  @Output() isLoggedIn = new EventEmitter<boolean>();
  
  constructor(private router: Router, private authService: AuthService) {}

  public logout(): void {
    this.authService.logout();
    this.isLoggedIn.emit(false);
    this.router.navigate(['/login']);
  }
}
