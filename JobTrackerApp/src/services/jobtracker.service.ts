import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from './../environments/environment';
// Fetches from `http://my-prod-url` in production, `http://my-dev-url` in development.
//fetch(environment.jobTrackerUrl);

@Injectable({
  providedIn: 'root'
})
export class JobTrackerService {
  private jobTrackerUrl = environment.jobTrackerUrl; 

  constructor(private http: HttpClient) { }

  public CreateJobProfile(jobProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/JobTracker/CreateJobProfile`, jobProfile, { headers });
  }
  
  public GetEmployerProfiles(jobProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/JobTracker/GetEmployerProfiles`+ '/' + jobProfile.id, { headers });
  }

  public GetJobProfile(userNameId: any, jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/JobTracker/GetJobProfile`,  { headers });
  }

  public GetJobProfiles(): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/JobTracker/GetJobProfiles`, { headers });
  }
}
