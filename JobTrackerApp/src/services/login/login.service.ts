import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { catchError, Observable, of } from "rxjs";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: 'root'
  })
export class LoginService {
    private loginUrl = environment.loginUrl;
    constructor(private http: HttpClient) { }
    
    public Login(username: string, password: string): Observable<any> { 
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        var email = username;
        const body = { email, password };

        return this.http.post<any>(`${this.loginUrl}/api/Login/loginauth`, body, { headers }).pipe(
            catchError(this.handleError<any>('Login')));
    }

    public RegisterUser(email: string, password: string): Observable<any> {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        const body = { email, password };
        //const params = new HttpParams().set('email', email).set('password', password);
      
        return this.http.post<any>(`${this.loginUrl}/api/Login/registeruser`, body, { headers }).pipe(
            catchError(this.handleError<any>('RegisterUser')));
    }

    public ConfirmEmail(token: string): Observable<any> {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        //const body = { token };
        const params = new HttpParams().set('token', token);

        return this.http.get<any>(`${this.loginUrl}/api/Login/confirm-email`, { headers, params }).pipe(
            catchError(this.handleError<any>('confirmEmail')));
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
          console.error(`${operation} failed: ${error.message}`);
          return of(result as T);
        };
      }
}
