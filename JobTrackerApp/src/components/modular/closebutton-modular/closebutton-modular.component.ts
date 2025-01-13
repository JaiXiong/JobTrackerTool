import { Component } from '@angular/core';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-closebutton-modular',
  standalone: true,
  imports: 
  [
    MatSnackBarModule,
    MatIconModule,

  ],
  templateUrl: './closebutton-modular.component.html',
  styleUrl: './closebutton-modular.component.scss'
})
export class ClosebuttonModularComponent {

  constructor(private snackBar: MatSnackBar) { }

  public onClose(): void {
    const snackBarRef = this.snackBar.open('Changes will be lost. Close without saving?', 'Cancel', {
      duration: 5000,
      horizontalPosition: 'right', 
      verticalPosition: 'top', 
    });
  }

  public onCloseRegister(): void {
    console.log('Close button clicked!');
  }
}
