import { Routes } from '@angular/router';
import { JobprofileComponent } from '../components/manage-jobprofiles/jobprofile/jobprofile.component';
import { EmployerprofileComponent } from '../components/manage-employerprofiles/employerprofile/employerprofile.component';
import { JobactionComponent } from '../components/manage-jobactions/jobaction/jobaction.component';
import { LoginComponent } from '../components/login/login.component';

export const routes: Routes = 
[
    {path: '', redirectTo: '/login', pathMatch: 'full'},
    {path: 'jobprofile', component: JobprofileComponent},
    {path: 'employerprofile', component: EmployerprofileComponent},
    {path: 'jobaction', component: JobactionComponent},
    {path: 'login', component: LoginComponent}
    
];
