import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-dialog-upload-modular',
  standalone: true,
  imports: 
  [
    MatIcon,
    MatProgressBarModule,
    CommonModule,
    MatButtonModule
  ],
  templateUrl: './dialog-upload-modular.component.html',
  styleUrl: './dialog-upload-modular.component.scss',
})
export class DialogUploadModularComponent {
  @Output() uploadCancelled = new EventEmitter<void>();
  @Output() fileSelected = new EventEmitter<File>();
  fileName: string = '';

  selectedFile: File | null = null;
  uploadProgress: number = 0;
  error: string | null = null;

  public onFileSelected(event: any) {
    const file: File = event.target.files[0];
    this.error = null;

    if (file) {
      if (file.size > 10000000) { // 10MB limit
        this.error = 'File size exceeds 10MB limit';
        return;
      }

    if (file) {
      this.fileName = file.name;
      this.selectedFile = file;
      }
    }
  }

  public cancelUpload() {
    this.selectedFile = null;
    this.fileName = '';
    this.uploadProgress = 0;
    this.error = null;
    this.uploadCancelled.emit();
    }

  public onUpload()
  {
    if (this.selectedFile)
    {
      this.fileSelected.emit(this.selectedFile);
    }
    else
    {
      console.log("No file selected in upload dialog.");
    }
  }
}
