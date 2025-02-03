import { Component, Input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DialogUploadModularComponent } from '../dialog-upload-modular/dialog-upload-modular.component';
import { UploadService } from '../../../services/upload/upload.service';

@Component({
  selector: 'app-upload-modular',
  standalone: true,
  imports: [MatIcon, MatDialogModule],
  templateUrl: './upload-modular.component.html',
  styleUrl: './upload-modular.component.scss',
})
export class UploadModularComponent {
  @Input() _jobProfileId!: string | null;
  selectedFile: File | null = null;

  constructor(
    private _uploadService: UploadService,
    private dialog: MatDialog
  ) {}

  onUploadEmployerProfile(): void {
    const dialogRef = this.dialog.open(DialogUploadModularComponent, {
      width: 'auto',
      height: 'auto',
      disableClose: true,
    });

    dialogRef.componentInstance.fileSelected.subscribe((file: File) => {
      this.selectedFile = file;
      //this.uploadFile(file);
      this.uploadFile();
      dialogRef.close();
    });
  }

  public uploadFile() {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);

      this._uploadService.Upload(formData, this._jobProfileId).subscribe({
        next: (result) => {
          if (result) {
            alert('File uploaded successfully');
          }
        },
        error: (error) => {
          console.log('File upload failed', error);
        },
      });
    }
  }
}
