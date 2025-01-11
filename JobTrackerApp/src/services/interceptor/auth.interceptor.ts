import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, switchMap } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { response } from 'express';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const jwtToken = getJwtToken();
  const refreshToken = authService.refreshToken();
  
  if (req.url.includes('login') || req.url.includes('register')) {
    return next(req);
  }
  
  if (authService.isTokenExpired() && refreshToken) {
    return authService.refreshToken().pipe(
      switchMap((response: any) => {
        const token = response.token || response;
        const cloned = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          }
        });
        return next(cloned);
      }),
      catchError((error) => {
        console.error('Failed to refresh token', error);
        router.navigate(['/login']);
        return EMPTY;
      })
    );
  }

  if (!jwtToken && !refreshToken) {
    alert('Something happened! You are not logged in. Please log in first.');
    router.navigate(['/login']);
    return EMPTY;
  }

  const cloned = req.clone({
    setHeaders: {
      Authorization: `Bearer ${jwtToken}`,
    },
  });
  return next(cloned);
};

function getJwtToken(): string | null {
  const token = localStorage.getItem('JWT_TOKEN');
  return token;
}
