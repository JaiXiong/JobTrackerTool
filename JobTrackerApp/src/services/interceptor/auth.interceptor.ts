import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { EMPTY } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);

  const jwtToken = getJwtToken();

  if (!jwtToken && !req.url.includes('login')) {
    alert('Something happened! You are not logged in. Please log in first.');
    router.navigate(['/login']);
    return EMPTY;
  }

  if (jwtToken) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${jwtToken}`,
      },
    });
    return next(cloned);
  }

  if (req.url.includes('login') || req.url.includes('register')) {
    return next(req);
  }

  router.navigate(['/login']);
  return next(req);
};

function getJwtToken(): string | null {
  const token = localStorage.getItem('JWT_TOKEN');
  return token;
}
