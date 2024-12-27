import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule, MatTabGroup, MatTab } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule } from '@angular/router';
import { ClosebuttonModularComponent } from "../../modular/closebutton-modular/closebutton-modular.component";
import { SubmitbuttonModularComponent } from '../../modular/submitbutton-modular/submitbutton-modular.component';

@Component({
  selector: 'app-registeruser',
  standalone: true,
  imports: [
    RouterModule,
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatIconModule,
    MatTooltipModule,
    ReactiveFormsModule,
    ClosebuttonModularComponent,
    SubmitbuttonModularComponent
],
  templateUrl: './registeruser.component.html',
  styleUrl: './registeruser.component.scss',
})
export class RegisteruserComponent {
  //userProfileForm!: FormGroup;
  _email: string = '';
  _password: string = '';

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    // this.userProfileForm = this.formBuilder.group({
    //   email: ['', Validators.required],
    //   password: ['', Validators.required],
    //   confirmPassword: ['', Validators.required],
    // });
  }

  _registerUser = input('');

  public onSubmitRegister(): void {
    console.log('Register button clicked!');
  }

  public onCloseRegister(): void {
    console.log('Close button clicked!');
  }
}
