import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';

import { DashboardComponent } from './dashboard/dashboard.component';
import { CallTodayComponent } from './call-today/call-today.component';
import { BlacklistComponent } from './blacklist/blacklist.component';


const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
 /* { path: 'calltoday', component: CallTodayComponent, canActivate: [MsalGuard] },
  { path: 'blacklist', component: BlacklistComponent, canActivate: [MsalGuard] },*/
  { path: 'calltoday', component: CallTodayComponent},
  { path: 'blacklist', component: BlacklistComponent},
  {path: 'hub', redirectTo: 'http://localhost:5000/hub'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
