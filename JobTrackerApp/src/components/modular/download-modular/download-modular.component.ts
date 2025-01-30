import { Component, Input, Output } from '@angular/core';
import JSZip from 'jszip';
import { MatIcon } from '@angular/material/icon';
import { JobTrackerService } from '../../../services/jobtracker/jobtracker.service';
import { Subject, takeUntil, map, tap, switchMap, take, EMPTY, forkJoin } from 'rxjs';
import { DialogDownloadModularComponent } from '../dialog-download-modular/dialog-download-modular.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-download-modular',
  standalone: true,
  imports: 
  [
    MatIcon, 
    MatDialogModule
  ],
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

  public todaysDate()
  {
    var currentDate = new Date().toString();
    const format = 'dd-MM-YY--HH-mm-ss'
    const locale = 'en-US'
    const formattedDate = formatDate(currentDate, format, locale);
    return formattedDate;
  }

  public onDownloadEmployerProfile(): void {
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
          if (result.pdf == true && result.csv == true) {
            return forkJoin({
              pdf: this.jobTrackerService.DownloadEmployerProfilePdf(
                this._jobProfileId,
                this.sendAll,
                this.sendCsv,
                this.sendPdf
              ),
              csv: this.jobTrackerService.DownloadEmployerProfileCsv(
                this._jobProfileId,
                this.sendAll,
                this.sendCsv,
                this.sendPdf
              ),
            });
          } else if (result.csv == true && result.pdf == false) {
            return this.jobTrackerService.DownloadEmployerProfileCsv(
              this._jobProfileId,
              this.sendAll,
              this.sendPdf,
              this.sendCsv
            );
          } else if (result.csv == false && result.pdf == true) {
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
        next: (files: any) => {
          if (files.pdf && files.csv) {
            const zip = new JSZip();
            const reader = new FileReader();
            reader.onload = (event: any) => {
              const csvContent = event.target.result;
              zip.file(`employer-profile--${this.todaysDate()}.pdf`, files.pdf, { binary: true });
              zip.file(`employer-profile--${this.todaysDate()}.csv`, csvContent);

              zip.generateAsync({ type: 'blob' }).then((content: any) => {
                const url = window.URL.createObjectURL(content);
                const a = document.createElement('a');
                a.href = url;
                a.download = `employer-profile--${this.todaysDate()}.zip`;
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                window.URL.revokeObjectURL(url);
              });
            };
            reader.readAsText(files.csv);
          } else if (files instanceof Blob) {
            const url = window.URL.createObjectURL(files);
            const a = document.createElement('a');
            a.href = url;
            a.download =
              files.type === 'application/pdf'
                ? `employer-profile--${this.todaysDate()}.pdf`
                : `employer-profile--${this.todaysDate()}.csv`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
          }
        },
        error: (error) => {
          console.error('Download failed', error);
        },
      });
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
          a.download = `employer-profile--${this.todaysDate()}.csv`;
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
          a.download = `employer-profile--${this.todaysDate()}.pdf`; // Set the file name here
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

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
