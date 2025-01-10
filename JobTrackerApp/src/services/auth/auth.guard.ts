import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  var authService = inject(AuthService);
  var routerService = inject(Router);
  
  if (!authService.isLoggedIn()) {
    routerService.navigate(['/login']);
    return false;
  }
  return true;
};
