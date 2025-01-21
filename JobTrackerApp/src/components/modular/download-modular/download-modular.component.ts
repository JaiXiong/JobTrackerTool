import { Component, Input, Output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { Subject, takeUntil, map, tap, switchMap, take, EMPTY } from 'rxjs';
import { DialogDownloadModularComponent } from '../dialog-download-modular/dialog-modular.component';
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
  sendPdf: boolean = false;
  sendCsv: boolean = false;

  constructor(private jobTrackerService: JobTrackerService, private dialog: MatDialog) { }

  onDownloadEmployerProfile(): void {
    const dialogRef = this.dialog.open(DialogDownloadModularComponent, {
      width: '400px',
      height: '200px',
      disableClose: true,
    });

    dialogRef
      .afterClosed()
      .pipe(
        tap((result) => {
          if (result) {
            console.log(result);
            console.log(result.all);
            console.log(result.pdf);
            console.log(result.csv);
            
            this.sendAll = result.all;
            this.sendPdf = result.pdf;
            this.sendCsv = result.csv;
          }
        }),
        switchMap((result) => {
          if (result) {
            return this.jobTrackerService.DownloadEmployerProfile(
              this._jobProfileId,
              this.sendAll,
              this.sendPdf,
              this.sendCsv
            );
          } else {
            return EMPTY;
          }
        }),
        takeUntil(this.destroy$)
      )
      .subscribe({
        next: (response) => {
          // if (this._loading) {
          //   return; // Prevent multiple clicks
          // }
          const blob = new Blob([response], { type: 'application/zip' });
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'employer_profile.zip';
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
        },
        error: (error) => {
          console.error('Download failed', error);
        },
      });

    

    // this._loading = true;
    // this.jobTrackerService.DownloadEmployerProfile(this._jobProfileId, this.sendAll, this.sendPdf, this.sendCsv)
    //   .pipe(
    //     takeUntil(this.destroy$),
    //     map(response => new Blob([response], { type: 'text/csv' })),
    //   )
    //   .subscribe((csv: Blob) => {
    //     const url = window.URL.createObjectURL(csv);
    //     const a = document.createElement('a');
    //     a.href = url;
    //     a.download = 'download.csv'; // Set the file name here
    //     document.body.appendChild(a);
    //     a.click();
    //     document.body.removeChild(a);
    //     window.URL.revokeObjectURL(url);
    //     this._loading = false;
    //   },
    //   () => {
    //     this._loading = false;
    //   });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
