import { Component } from '@angular/core';
import { JobTrackerService } from '../../../services/jobtracker.service';
import { LoginService } from '../../../services/login.service';

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

  constructor(private loginService: LoginService) { }

  public onSubmitRegister(email: string, pw: string): void {
    this.loginService.RegisterUser(email, pw).subscribe(
      response => {
        console.log('Register button clicked!');
      },
      error => {
        console.error('Register failed', error);
      });
  }

  public submit(): void {
    console.log('Submit button clicked!');
  }
  
}
