import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  console.log('Interceptor running');
  // Skip auth for login/refresh endpoints if needed, for now we don't
  // if (req.url.includes('refreshtoken')) {
  //   return next(req);
  // }

  const jwtToken = getJwtToken();
  console.log('Token from storage:', jwtToken);

  if (jwtToken) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${jwtToken}`,
      },
    });
    
    console.log('Request headers:', cloned.headers.get('Authorization'));
    return next(cloned);
  }
  console.log('No token found, proceeding without auth');
  return next(req);
};

function getJwtToken(): string | null {
  const token = localStorage.getItem('JWT_TOKEN');
  if (!token) {
    return null;
  }
  
  console.log('Retrieved token:', token);
  return token;
}
