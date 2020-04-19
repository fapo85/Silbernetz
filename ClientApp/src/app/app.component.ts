import { Component, OnInit, ViewChild } from '@angular/core';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { Logger, CryptoUtils } from 'msal';
import { SignalRService } from './services/signal-r.service';
import { Stats } from './Models/stats';

@Component({
  selector: 'sn-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  @ViewChild('sidebar') sidebar: any;
  title = 'Silbernetz';
  loggedIn = false;
  data: Stats = new Stats();
  constructor(private signal: SignalRService,
              private broadcastService: BroadcastService,
              private authService: MsalService){
  }
  ngOnInit(): void {
    this.signal.GetStats().subscribe(data => {
      this.data = data;
    });
    //Auth Service
    this.checkoutAccount();

    //new Token To Signal R Service
    this.broadcastService.subscribe('msal:loginSuccess', payload => {
      this.signal.SetAcessToken(payload.idToken.rawIdToken);
      this.checkoutAccount();
  });
    this.authService.handleRedirectCallback((authError, response) => {
      if (authError) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }

      console.log('Redirect Success: ', response);
    });

    this.authService.setLogger(new Logger((logLevel, message, piiEnabled) => {
      console.log('MSAL Logging: ', message);
    }, {
      correlationId: CryptoUtils.createNewGuid(),
      piiLoggingEnabled: false
    }));
    if (this.loggedIn){
      this.SetAcessToken();
    }
  }

  checkoutAccount() {
    this.loggedIn = !!this.authService.getAccount();
//    if (this.loggedIn){
//      console.log('logedin: ', this.authService.getAccount());
//   }else{
//      console.log('logedout');
//    }
  }

  login() {
    const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

    if (isIE) {
      this.authService.loginRedirect({
        extraScopesToConsent: ['"user.read', 'openid', 'profile']
      });
    } else {
      this.authService.loginPopup({
        extraScopesToConsent: ['"user.read', 'openid', 'profile']
      });
    }
  }

  logout() {
    this.authService.logout();
  }
  AuthClick(){
    console.log(this.loggedIn);
    if (!this.loggedIn){
      this.login();
    }else{
      this.logout();
    }
  }
  private SetAcessToken() {
    const timestamp = Math.floor((new Date()).getTime() / 1000)
    let token = null;
    for (const key of Object.keys(localStorage)) {
      if (key.includes('"authority":')) {
        const val: any = JSON.parse(localStorage.getItem(key)!);
        if (val.expiresIn) {
          // We have a (possibly expired) token
          if (val.expiresIn > timestamp && val.idToken === val.accessToken) {
            // Found the correct token
            token = val.idToken;
          }
          else {
            // Clear old data
            localStorage.removeItem(key);
          }
        }
      }
    }
    if (token){
      this.signal.SetAcessToken(token);
    }
    //throw new Error('No valid token found');
  }
  toggleSidebar(){
  }
}
