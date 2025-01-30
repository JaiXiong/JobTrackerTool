import { Component, Input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { MatDialog } from '@angular/material/dialog';
import { DialogUploadModularComponent } from '../dialog-upload-modular/dialog-upload-modular.component';
import { switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-upload-modular',
  standalone: true,
  imports: 
  [
    MatIcon
  ],
  templateUrl: './upload-modular.component.html',
  styleUrl: './upload-modular.component.scss'
})
export class UploadModularComponent {
@Input() _jobProfileId!: string | null;

constructor(private _jobTrackerService: JobTrackerService, private dialog: MatDialog) {}

  onUploadEmployerProfile(): void {
    const dialogRef = this.dialog.open(DialogUploadModularComponent, 
    {
      width: 'auto',
      height: 'auto',
      disableClose: true
    });

    dialogRef.afterClosed()
    .pipe(
      tap(result => {
        if (result)
        {
          //to some logic
        }
      })
    ),
    switchMap((result: any) => {
      return result;
    });
  }
}
