import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { LoginService } from '../../../services/login/login.service';
import { MatIcon } from '@angular/material/icon';


@Component({
  selector: 'app-emailconfirmation',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatButtonModule,
    MatIcon
  ],
  templateUrl: './emailconfirmation.component.html',
  styleUrl: './emailconfirmation.component.scss'
})
export class EmailconfirmationComponent implements OnInit {
  isLoading = true;
  isConfirmed = false;
  errorMessage: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private loginService: LoginService
  ) {}
  
  ngOnInit(): void {
      this.route.queryParams.subscribe(params => {
          const token = params['token'];
          if (token) {
            this.confirmEmail(token);
          }
      });
  }

  private confirmEmail(token: string): void {
    this.loginService.ConfirmEmail(token).subscribe({
      next: () => {
        this.isConfirmed = true;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Email confirmation failed. Please try again.';
        console.error('Email confirmation error:', error);
        this.isLoading = false;
      }
    });
  }

  public goToLogin(): void {
    if (this.isConfirmed) {
      this.router.navigate(['/login']);
    }
  }
}
