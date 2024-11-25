import { Component, EventEmitter, Output } from '@angular/core';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatSnackBarRef } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-custom-employersnackbar',
  standalone: true,
  imports: 
  [
    MatIconModule,
    MatTooltipModule
  ],
  templateUrl: './custom-employersnackbar.component.html',
  styleUrl: './custom-employersnackbar.component.scss'
})
export class CustomEmployersnackbarComponent {
  @Output() saveClicked = new EventEmitter<void>();
  @Output() cancelClicked = new EventEmitter<void>();

  constructor(private snackBarRef: MatSnackBarRef<CustomEmployersnackbarComponent>) {}

  onSave() {
    // Handle save action, this emats an event to the parent component
    this.saveClicked.emit();
    this.snackBarRef.dismissWithAction();
  }

  onCancel() {
    // Handle cancel action
    this.snackBarRef.dismiss();
    
  }
}
