import { RouterModule, Routes } from '@angular/router';
import { JobprofileComponent } from '../components/manage-jobprofiles/jobprofile/jobprofile.component';
import { EmployerprofileComponent } from '../components/manage-employerprofiles/employerprofile/employerprofile.component';
import { JobactionComponent } from '../components/manage-jobactions/jobaction/jobaction.component';
import { LoginComponent } from '../components/login/login.component';
import { authGuard } from '../services/auth/auth.guard';

export const routes: Routes = 
[
    {path: '', redirectTo: 'login', pathMatch: 'full', canActivate: [authGuard]},
    {path: 'login', component: LoginComponent, title: 'Login Page'},
    {path: 'jobprofile', component: JobprofileComponent, title: 'Home Page'},
    {path: 'employerprofile', component: EmployerprofileComponent, title: 'Employer Page'},
    {path: 'jobaction', component: JobactionComponent, title: 'JobAction Page'},
];

export const AppRoutingModule = RouterModule.forRoot(routes, {
    onSameUrlNavigation: 'reload'
});
// export const routes: Routes = [
//     { path: '', redirectTo: '/login', pathMatch: 'full' },
//     { path: 'jobprofile', loadComponent: () => import('../components/manage-jobprofiles/jobprofile/jobprofile.component').then(m => m.JobprofileComponent) },
//     { path: 'employerprofile', loadComponent: () => import('../components/manage-employerprofiles/employerprofile/employerprofile.component').then(m => m.EmployerprofileComponent) },
//     { path: 'jobaction', loadComponent: () => import('../components/manage-jobactions/jobaction/jobaction.component').then(m => m.JobactionComponent) },
//     { path: 'login', loadComponent: () => import('../components/login/login.component').then(m => m.LoginComponent) }
//   ];
