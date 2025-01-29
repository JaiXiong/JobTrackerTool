import { Component, Input, Output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { Subject, takeUntil, map, tap, switchMap, take, EMPTY } from 'rxjs';
import { DialogDownloadModularComponent } from '../dialog-download-modular/dialog-modular.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-download-modular',
  standalone: true,
  imports: [MatIcon, MatDialogModule],
  templateUrl: './download-modular.component.html',
  styleUrl: './download-modular.component.scss',
})
export class DownloadModularComponent {
  @Input() _jobProfileId: string | null = '';
  private destroy$: Subject<void> = new Subject<void>();
  _loading = false;
  sendAll: boolean = false;
  sendPdf: boolean = false;
  sendCsv: boolean = false;

  constructor(
    private jobTrackerService: JobTrackerService,
    private dialog: MatDialog
  ) {}

  onDownloadEmployerProfileAll(): void {
    const dialogRef = this.dialog.open(DialogDownloadModularComponent, {
      width: 'auto',
      height: 'auto',
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
            return this.jobTrackerService.DownloadEmployerProfileAll(
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

      //this was used for zip folder but it was invalid, can't unzip but now I think about it
      //maybe we should do actions 1 by 1 instead of everything
      //makes it more managable in the future

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

  public onDownloadEmployerProfileCsv() {
    const dialogRef = this.dialog.open(DialogDownloadModularComponent, {
      width: 'auto',
      height: 'auto',
      disableClose: true,
    });

    dialogRef
      .afterClosed()
      .pipe(
        tap((result) => {
          if (result) {
            this.sendAll = result.all;
            this.sendPdf = result.pdf;
            this.sendCsv = result.csv;
          }
        }),
        switchMap((result) => {
          if (result) {
            return this.jobTrackerService.DownloadEmployerProfileCsv(
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
        next: (csv: Blob) => {
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
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {
          this._loading = false;
        }
      });
  }

  public onDownloadEmployerProfilePdf() {
    const dialogRef = this.dialog.open(DialogDownloadModularComponent, {
      width: 'auto',
      height: 'auto',
      disableClose: true,
    });

    dialogRef
      .afterClosed()
      .pipe(
        tap((result) => {
          if (result) {
            this.sendAll = result.all;
            this.sendPdf = result.pdf;
            this.sendCsv = result.csv;
          }
        }),
        switchMap((result) => {
          if (result) {
            return this.jobTrackerService.DownloadEmployerProfilePdf(
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
        next: (pdf: Blob) => {
          const url = window.URL.createObjectURL(pdf);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'download.pdf'; // Set the file name here
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
          this._loading = false;
        },
        error: (error: any) => {
          console.log(error);
        },
        complete: () => {
          this._loading = false;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
