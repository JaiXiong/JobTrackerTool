import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  //this is the subject that will be used to close the dialog
  private dialogCloseSubject = new Subject<void>();
  //this is the observable that will be used to close the dialog
  dialogClose$ = this.dialogCloseSubject.asObservable();

  constructor(private snackBar: MatSnackBar) {}

  public showNotification(message: string, duration: number = 5000): void {
    this.snackBar.open(message, 'Close', {
      duration,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  public ChangesWillBeLost(message: string, duration: number = 5000): void {
    const snackBarRef = this.snackBar.open(message, 'Close', {
      duration,
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });

    snackBarRef.onAction().subscribe(() => {
      console.log('Close action canceled');
      // Do nothing, just close the snackbar
    });

    snackBarRef.afterDismissed().subscribe((info) => {
      if (!info.dismissedByAction) {
        //this.dialogRef.close();
        this.dialogCloseSubject.next();
      }
    });
  }
}
