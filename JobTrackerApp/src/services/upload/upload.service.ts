import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, Subject, tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  private url = environment.jobTrackerUrl;
  private uploadCompleteSubject = new Subject<void>();
  uploadComplete$ = this.uploadCompleteSubject.asObservable();
  
  constructor(private http: HttpClient) { }

  public Upload(formData: FormData, jobProfileId: any): Observable<any> {
    return this.http.post<any>(`${this.url}/api/JobTracker/upload/${jobProfileId}`, formData, {}).pipe(
      tap(() => {
        this.uploadCompleteSubject.next();
      }),
    catchError(this.handleError<any>('Upload')));
  }

  private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
        console.error(`${operation} failed: ${error.message}`);
        return of(result as T);
      };
    }
}
