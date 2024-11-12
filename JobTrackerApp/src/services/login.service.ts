import { HttpClient, HttpHeaders } from "@angular/common/http";
import e from "express"
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
        
        return this.http.post<any>(`${this.loginUrl}/login`, body, { headers });
    }


}
