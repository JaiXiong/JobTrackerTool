import { CommonModule } from '@angular/common';
import { Component, Output } from '@angular/core';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-dialog-modular',
  standalone: true,
  imports: [
    MatIconModule,
    MatDialogModule,
    CommonModule,
    MatCheckboxModule,
    FormsModule,
    MatTooltipModule
  ],
  templateUrl: './dialog-modular.component.html',
  styleUrl: './dialog-modular.component.scss',
})
export class DialogDownloadModularComponent {

  selections: any = {
    all: false,
    pdf: false,
    csv: false
  }
  // selectAll: boolean = false;
  // selectPdf: boolean = false;
  // selectCsv: boolean = false;

  constructor() {}

  onOk(): void {
    // this.dialogRef.close(this.selections);
  }
}
