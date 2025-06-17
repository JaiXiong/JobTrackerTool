import { Component, EventEmitter, Input, Output } from '@angular/core';
import { JobTrackerService } from '../../../services/jobtracker.service-old';
import { LoginService } from '../../../services/login.service-old';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-submitbutton-modular',
  standalone: true,
  imports: 
  [

  ],
  templateUrl: './submitbutton-modular.component.html',
  styleUrl: './submitbutton-modular.component.scss'
})
export class SubmitbuttonModularComponent {
  @Input() _registerUser: any;
  @Output() onSubmit = new EventEmitter<any>();
  @Output() closeRegister = new EventEmitter<void>();
  
  constructor(private loginService: LoginService, private snackBar: MatSnackBar) { }

  public onSubmitRegister(email: string, pw: string): void {
    this.loginService.RegisterUser(email, pw).subscribe({
      next: (response) => {
        //console.log('Response received:', response);
      },
      complete: () => {
        console.log('Request completed');
      },
      error: (error) => {
        console.log('Error received:', error);
      }
    });
  }

  public submit(): void {
    //console.log('Submit button clicked!');
  }
  
  submitForm() {
    //console.log(this._registerUser);
    this.loginService.RegisterUser(this._registerUser.email, this._registerUser.password).subscribe({
      next: response => {
        this.snackBar.open('Registered successfully', 'Close', {
          duration: 5000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
        this.closeRegister.emit();
      },
      error: error => {
        console.error('Register failed', error);
      }
    });
    
    this.onSubmit.emit();
  }
  
}
