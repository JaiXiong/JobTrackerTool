import { Component, EventEmitter, Output, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import e from 'express';

@Component({
  selector: 'app-backbutton-modular',
  standalone: true,
  imports: [
    RouterModule,
    MatButtonModule
  ],
  templateUrl: './backbutton-modular.component.html',
  styleUrl: './backbutton-modular.component.scss'
})
export class BackbuttonModularComponent {
  @Output() backClicked = new EventEmitter<void>();
  
  constructor(private router: Router) {}

  public onBack(): void {
    //window.history.back();
    //this.router.navigate(['/login']);
    if (this.backClicked.observed) {
      this.backClicked.emit();
    }
    else {
      this.router.navigate(['/login']);
    }
  }
}
