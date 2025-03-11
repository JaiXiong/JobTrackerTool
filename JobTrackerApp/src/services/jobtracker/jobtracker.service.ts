import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, Observable, of, pipe } from 'rxjs';
import { environment } from '../../environments/environment';

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
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerProfile`, employerProfile, { headers }).pipe(
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

  public GetJobProfile(jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`+ '/' + jobProfileId, { headers }).pipe(
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
  //this is incase we want to limit data for the table, we leave here for now maybe use later if needed
  //this will only get the columns we want but realistically, if they click on the row it needs all data.
  public GetEmployerPagingTableData(jobProfileId: any, pageIndex: any, pageSize: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerPagingTableData`+ '/' + jobProfileId + '/' + pageIndex + '/' + pageSize, { headers }).pipe(
      catchError(this.handleError<any>('GetEmployerPagingTableData')));
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

  public DeleteJobProfile(jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.delete<any>(`${this.jobTrackerUrl}/api/JobTracker/JobProfile`+ '/' + jobProfileId, { headers }).pipe(
      catchError(this.handleError<any>('DeleteJobProfile')));
  }

  public GetEmployerTotalCount(jobProfileId: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get<any>(`${this.jobTrackerUrl}/api/JobTracker/EmployerTotalCount`+ '/' + jobProfileId, { headers }).pipe(
      catchError(this.handleError<any>('GetEmployerTotalCount')));
  }

  public DownloadEmployerProfileAll(jobProfileId: any, sendAll: boolean, sendPdf:boolean, sendCsv:boolean): Observable<Blob> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const params = new HttpParams()
      .set('pdf', sendPdf.toString())
      .set('csv', sendCsv.toString())
      .set('include', sendAll.toString());

    return this.http.get(`http://localhost:5001/api/JobTracker/downloadall/${jobProfileId}`, { headers, params, responseType: 'blob' }).pipe(
      catchError(this.handleError<any>('DownloadEmployerProfile')));
  }

  public DownloadEmployerProfileCsv(jobProfileId: any, sendAll: boolean, sendPdf:boolean, sendCsv:boolean): Observable<Blob> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const params = new HttpParams()
      .set('pdf', sendPdf.toString())
      .set('csv', sendCsv.toString())
      .set('include', sendAll.toString());

    return this.http.get(`http://localhost:5001/api/JobTracker/downloadcsv/${jobProfileId}`, { headers, params, responseType: 'blob' }).pipe(
      catchError(this.handleError<any>('DownloadEmployerProfile')));
  }

  public DownloadEmployerProfilePdf(jobProfileId: any, sendAll: boolean, sendPdf:boolean, sendCsv:boolean): Observable<Blob> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const params = new HttpParams()
      .set('pdf', sendPdf.toString())
      .set('csv', sendCsv.toString())
      .set('include', sendAll.toString());

    return this.http.get(`http://localhost:5001/api/JobTracker/downloadpdf/${jobProfileId}`, { headers, params, responseType: 'blob' }).pipe(
      catchError(this.handleError<any>('DownloadEmployerProfile')));
  }

  public UpLoad(formData: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(`${this.jobTrackerUrl}/api/JobTracker/Upload`, formData, { headers }).pipe(
      catchError(this.handleError<any>('Upload')));
    }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
}

