import { Component } from '@angular/core';

@Component({
  selector: 'app-submitbutton-modular',
  standalone: true,
  imports: [],
  templateUrl: './submitbutton-modular.component.html',
  styleUrl: './submitbutton-modular.component.scss'
})
export class SubmitbuttonModularComponent {

  //WE might not use this, but keep this here for now
  public onSubmitRegister(): void {
    console.log('Register button clicked!');
  }

  public submit(): void {
    console.log('Submit button clicked!');
  }
  
}
