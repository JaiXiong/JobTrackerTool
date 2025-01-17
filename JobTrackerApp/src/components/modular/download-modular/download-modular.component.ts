import { Component, Input, Output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { Subject, takeUntil, map, tap, switchMap } from 'rxjs';
import { DialogModularComponent } from '../dialog-modular/dialog-modular.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-download-modular',
  standalone: true,
  imports: 
  [
    MatIcon,
    MatDialogModule
  ],
  templateUrl: './download-modular.component.html',
  styleUrl: './download-modular.component.scss'
})
export class DownloadModularComponent {
  @Input() _jobProfileId: string | null = '';
  private destroy$: Subject<void> = new Subject<void>();
  _loading = false;
  sendAll: boolean = false;

  constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) { }

  onDownloadEmployerProfile(): void {
    const dialogRef = this.dialog.open(DialogModularComponent, {
      width: '400px',
      height: '200px',
      disableClose: true,
    });

    // dialogRef.afterClosed().subscribe(result => {
    //   this.sendAll = result;
    // });
    dialogRef.afterClosed().pipe(
      switchMap(result => this.sendAll = result),
      takeUntil(this.destroy$)
    ).subscribe();

    if (this._loading) {
      return; // Prevent multiple clicks
    }

    this._loading = true;
    this.jobTrackerService.DownloadCsvEmployerProfile(this._jobProfileId, this.sendAll)
      .pipe(
        takeUntil(this.destroy$),
        map(response => new Blob([response], { type: 'text/csv' })),
      )
      .subscribe((csv: Blob) => {
        const url = window.URL.createObjectURL(csv);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'download.csv'; // Set the file name here
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
        this._loading = false;
      },
      () => {
        this._loading = false;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
