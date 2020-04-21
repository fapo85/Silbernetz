import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { MsalModule, MsalService } from '@azure/msal-angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CallTodayComponent } from './call-today/call-today.component';
import { NumValueComponent } from './num-value/num-value.component';
import { environment } from 'src/environments/environment';
import { BlacklistComponent } from './blacklist/blacklist.component';

export function GetRedirectUrl(): string{
  return environment.production ? 'https://silbernetz.gemeinschaft.dev' : 'http://localhost:4200/';
}

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    CallTodayComponent,
    NumValueComponent,
    BlacklistComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MsalModule.forRoot({
      auth: {
          clientId: '57932fae-4538-44a5-a4df-6ef590e306dd',
          authority: 'https://login.microsoftonline.com/50cffe9e-fac5-4685-8b73-d01ea3282f20/',
          validateAuthority: true,
          redirectUri: GetRedirectUrl(),
          postLogoutRedirectUri: GetRedirectUrl(),
          navigateToLoginRequestUrl: true,
      },
      cache: {
        cacheLocation: 'localStorage',
        storeAuthStateInCookie: (window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1)
    }
  }, {
    consentScopes: [
      'user.read',
      'openid',
      'profile',
    ]})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(msalService: MsalService) {
    msalService.handleRedirectCallback(_ => { });
  }
  static GetMsalConfig(){

  }
 }
