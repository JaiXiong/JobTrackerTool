import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, of, pipe } from 'rxjs';
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

    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`, jobProfile, { headers }).pipe(
      catchError(this.handleError<any>('CreateJobProfile')));
  }

  public CreateEmployerProfile(employerProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateEmployerProfile`, employerProfile, { headers }).pipe(
      catchError(this.handleError<any>('CreateEmployerProfile')));
  }

  public CreateUser(user: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateUser`, user, { headers }).pipe(
      catchError(this.handleError<any>('CreateUser')));
  }

  public CreateUserProfile(userProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateUserProfile`, userProfile, { headers }).pipe(
      catchError(this.handleError<any>('CreateUserProfile')));
  }
  
  public CreateEmployerDetails(employerDetails: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateEmployerDetails`, employerDetails, { headers }).pipe(
      catchError(this.handleError<any>('CreateEmployerDetails')));
  }

  public CreateEmployerAction(jobAction: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/CreateJobAction`, jobAction, { headers }).pipe(
      catchError(this.handleError<any>('CreateJobAction')));
  }


  public GetEmployerProfiles(jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerProfiles`+ '/' + jobProfileId, { headers }).pipe(
      catchError(this.handleError<any>('GetEmployerProfiles')));
  }

  public GetJobProfile(userNameId: any, jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`+ '/' + userNameId + '/' + jobProfileId, { headers }).pipe(
      catchError(this.handleError<any>('GetJobProfile')));
  }

  public GetJobProfiles(userNameId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfiles`+ '/' + userNameId, { headers }).pipe(
      catchError(this.handleError<any>('GetJobProfiles')));
  }

  public GetEmployerPagingData(jobProfileId: any, pageIndex: any, pageSize: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerPagingData`+ '/' + jobProfileId + '/' + pageIndex + '/' + pageSize, { headers }).pipe(
      catchError(this.handleError<any>('GetEmployerPagingData')));
  }

  public UpdateJobProfile(jobProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.put<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`, jobProfile, { headers }).pipe(
      catchError(this.handleError<any>('UpdateJobProfile')));
  }

  public UpdateEmployerProfile(employerProfile: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.put<any>(`${this.jobTrackerUrl}/api/JobTracker/employerprofile`, employerProfile, { headers }).pipe(
      catchError(this.handleError<any>('UpdateEmployerProfile')));
  }

  public GetJobAction(employerProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobAction`+ '/' + employerProfileId, { headers }).pipe(
      catchError(this.handleError<any>('GetJobAction')));
  }

  public GetDetail(employerProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/Detail`+ '/' + employerProfileId, { headers }).pipe(
      catchError(this.handleError<any>('GetDetail')));
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}

