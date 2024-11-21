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
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateJobProfile`, jobProfile, { headers });
  }
  
  public GetEmployerProfiles(jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerProfiles`+ '/' + jobProfileId, { headers });
  }

  public GetJobProfile(userNameId: any, jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`+ '/' + userNameId + '/' + jobProfileId, { headers });
  }

  public GetJobProfiles(userNameId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfiles`+ '/' + userNameId, { headers });
  }

  public GetEmployerPagingData(jobProfileId: any, pageIndex: any, pageSize: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerPagingData`+ '/' + jobProfileId + '/' + pageIndex + '/' + pageSize, { headers });
  }

  public UpdateEmployerProfile(employerProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.put<any>(`${this.jobTrackerUrl}/api/JobTracker/employerprofile`, employerProfile, { headers });
  }

  public GetJobAction(employerProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobAction`+ '/' + employerProfileId, { headers });
  }

  public GetDetail(employerProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/Detail`+ '/' + employerProfileId, { headers });
  }
}
