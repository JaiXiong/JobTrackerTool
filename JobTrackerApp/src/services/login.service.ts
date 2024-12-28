import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from './../environments/environment';
import { Injectable } from "@angular/core";
//fetch(environment.loginUrl);

@Injectable({
    providedIn: 'root'
  })
export class LoginService {
    private loginUrl = environment.loginUrl;
    constructor(private http: HttpClient) { }
    
    public Login(username: string, password: string): Observable<any> { 
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        const body = { username, password };
        const params = new HttpParams().set('username', username).set('password', password);
        
        //return this.http.post<any>(`${this.loginUrl}/api/Login/loginauth`, body, { headers });
        return this.http.post<any>(`${this.loginUrl}/api/Login/loginauth`, {}, { headers, params });
    }

    public RegisterUser(email: string, password: string): Observable<any> {
        const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        const body = { email, password };
        const params = new HttpParams().set('email', email).set('password', password);
        
        //return this.http.post<any>(`${this.loginUrl}/api/Login/registeruser`, body, { headers });
        return this.http.post<any>(`${this.loginUrl}/api/Login/registeruser`, {}, { headers, params });
    }
}
