import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, switchMap } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { response } from 'express';

const PUBLIC_ENDPOINTS = [
  '/api/Login/loginauth',
  '/api/Login/registeruser',
  '/api/Login/confirm-email'
];

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const jwtToken = getJwtToken();
  const refreshToken = authService.refreshToken();
  
  //what to omit old way
  // if (req.url.includes('login') || req.url.includes('register') || req.url.includes('confirm-email')) {
  //   return next(req);
  // }

  // Check if the request URL matches any public endpoint
  const isPublicEndpoint = PUBLIC_ENDPOINTS.some(endpoint => 
    req.url.endsWith(endpoint) || req.url.includes(`${endpoint}?`)
  );

   if (isPublicEndpoint) {
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
