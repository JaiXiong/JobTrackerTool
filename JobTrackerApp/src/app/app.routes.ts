import { RouterModule, Routes } from '@angular/router';
import { JobprofileComponent } from '../components/manage-jobprofiles/jobprofile/jobprofile.component';
import { EmployerprofileComponent } from '../components/manage-employerprofiles/employerprofile/employerprofile.component';
import { JobactionComponent } from '../components/manage-jobactions/jobaction/jobaction.component';
import { LoginComponent } from '../components/login/login.component';
import { authGuard } from '../services/auth/auth.guard';

export const routes: Routes = 
[
    {path: '', redirectTo: 'login', pathMatch: 'full'},
    {path: 'login', component: LoginComponent, title: 'Login Page'},
    {path: 'jobprofile', component: JobprofileComponent, title: 'Home Page', canActivate: [authGuard]},
    {path: 'employerprofile', component: EmployerprofileComponent, title: 'Employer Page', canActivate: [authGuard]},
    {path: 'jobaction', component: JobactionComponent, title: 'JobAction Page', canActivate: [authGuard]},
];

export const AppRoutingModule = RouterModule.forRoot(routes, {
    onSameUrlNavigation: 'reload'
});
