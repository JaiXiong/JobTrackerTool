import { HttpClient } from "@angular/common/http";
import e from "express"
import { Observable } from "rxjs";

export class LoginService {
    constructor(private http: HttpClient) { }
    
    public Login(username: string, password: string): Observable<any> { 
        return this.http.post('http://localhost:3000/login', { username, password });
    }


}
