import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { jwtDecode } from "jwt-decode";
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loginUrl = environment.loginUrl;
  private readonly JWT_TOKEN = 'JWT_TOKEN';
  private readonly REFRESH_TOKEN = 'REFRESH_TOKEN';
  private loggedUser?: string;
  private isAuthicatedSubject = new BehaviorSubject<boolean>(false);

  //private cookieService = inject(CookieService);

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  public doLoginUser(email: string, acesstoken: any, refreshtoken: any) {

    this.loggedUser = email;
    this.storeJwtTokens({
      access_token: acesstoken,
      refresh_token: refreshtoken,
    });
    this.isAuthicatedSubject.next(true);
  }

  public storeJwtTokens(tokens: any) {
    localStorage.setItem(this.JWT_TOKEN, tokens.access_token);
    this.cookieService.set(this.REFRESH_TOKEN, tokens.refresh_token, {
      secure: true,
      sameSite: 'Strict',
      expires: 7, //7 days
    });
  }

  public getCurrentUser(): Observable<any> {
    return this.http.get('${this.loginUrl}/api/Login/currentuser');
  }

  public isLoggedIn() {
    return !!localStorage.getItem(this.JWT_TOKEN);
  }

  public isTokenExpired() {
    const token = localStorage.getItem(this.JWT_TOKEN);
    if (!token) {
      return true;
    }
    //console.log('logging: '+JSON.parse(tokens).access_token);
    //const token = JSON.parse(tokens).access_token;
    const decoded = jwtDecode(token);
    if (!decoded.exp) {
      return true;
    }
    const expirationDate = decoded.exp * 1000;
    const now = Date.now();

    //console.log('Token expiration:', new Date(expirationDate));
    //console.log('Current time:', new Date(now));

    return expirationDate < now;
  }
  catch(error: any) {
    console.error('Error decoding token:', error);
    return true;
  }

  public refreshToken(): Observable<any> {
    var refreshToken = this.cookieService.get(this.REFRESH_TOKEN);
    if (!refreshToken) {
      //debugging purposes
      //console.error('No refresh token found'); 
    }

    const headers = new HttpHeaders().set(
      'Authorization',
      `Bearer ${refreshToken}`
    );

    return this.http
      .post('${this.loginUrl}/api/login/refreshtoken', {}, { headers })
      .pipe(
        tap({
          next: (response: any) => {
            //console.log('Token refreshed:', response);
            this.storeJwtTokens({
              access_token: response.access_token,
              refresh_token: this.cookieService.get(this.REFRESH_TOKEN),
            });
          },
          complete: () => {
            //console.log('Request completed');
          },
          error: (error) => {
            console.log('Error refreshing token:', error);
          },
        })
      );
  }

  public logout() {
    localStorage.removeItem(this.JWT_TOKEN);
    this.cookieService.delete(this.REFRESH_TOKEN);
    localStorage.removeItem('jobProfileId');
    this.isAuthicatedSubject.next(false);
  }
}

