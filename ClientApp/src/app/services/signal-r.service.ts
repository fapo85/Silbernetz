import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { BroadcastService } from '@azure/msal-angular';
import { Stats } from '../Models/stats';
import { environment } from 'src/environments/environment';
import { Anrufer } from '../Models/anrufer';
import { AnrufExport } from '../Models/anruf-export';
import { WaitTimeProp } from '../Models/wait-time-prop';
import { BLIDatum } from '../Models/blidatum';

@Injectable({
  providedIn: 'root'
})
export class SignalRService{
  private hubConnection: signalR.HubConnection;
  anruferHeute: BehaviorSubject<Anrufer[]>;
  neusteSub = new BehaviorSubject<Stats>(Stats.GetEmpty());
  AlleSub = new BehaviorSubject<WaitTimeProp[]>([]);
  private accessToken = '';
  constructor() {
    this.startConnection();
  }
  public SetAcessToken(token: string){
    this.accessToken = token;
    this.hubConnection.stop();
    setTimeout(() => this.startConnection(), 1);
  }
  public startConnection = () => {
    if (environment.production){
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/Hub', { accessTokenFactory: () => this.accessToken })
      .build();
    }else{
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/Hub', { accessTokenFactory: () => this.accessToken })
      .build();
    }

    this.AddListener();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public AddListener = () => {
    this.hubConnection.on('stats', (data: Stats) => {
      data.timestamp = new Date(data.timestamp);
      if (data.timestamp.getTime() > this.neusteSub.value.timestamp.getTime()){
        this.neusteSub.next(data);
      }
      this.AlleSub.next([...this.AlleSub.value.filter(itm => (itm as Stats).angemeldet === undefined), data]);
      if(this.anruferHeute){
        this.HoleAnruferHeute();
      }
    });
    this.hubConnection.on('manystats', (data: WaitTimeProp[]) => {
      data.forEach((itm: WaitTimeProp) => itm.timestamp = new Date(itm.timestamp));
      this.AlleSub.next(data);
    });
  }
  private HoleAnruferHeute(){
    //Warten falls noch nicht Verbunden
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected){
      setTimeout(() => this.HoleAnruferHeute(), 20);
    } else {
      this.hubConnection.invoke('AnrufFromToday').then((data: Anrufer[]) => {
        data.forEach((anrufer: Anrufer) => {
          anrufer.anrufe.forEach((anruf: AnrufExport) => {
            anruf.timestamp = new Date(anruf.timestamp);
          });
          this.anruferHeute.next(data);
        });
      });
    }
  }
  public GetStats(): Observable<Stats>{
    return this.neusteSub;
  }
  public GetAllStats(): Observable<WaitTimeProp[]>{
    return this.AlleSub;
  }
  public GetAnruferHeute(): Observable<Anrufer[]> {
    if (!this.anruferHeute){
      this.anruferHeute = new BehaviorSubject<Anrufer[]>([]);
    }
    this.HoleAnruferHeute();
    return this.anruferHeute;
  }
  private HoleBlacklist(sub: Subject<BLIDatum[]>){
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected){
      setTimeout(() => this.HoleBlacklist(sub), 20);
    }else{
      this.hubConnection.invoke('GetBlackList').then((data: BLIDatum[]) => {
        data.forEach(itm => itm.created = new Date(itm.created));
        sub.next(data);
      });
    }
  }
  public GetBlacklist(): Observable<BLIDatum[]> {
    const sub = new Subject<BLIDatum[]>();
    this.HoleBlacklist(sub);
    return sub;
  }
  public AddBlacklistEntry(telnr: string, kommentar: string): Subject<boolean> {
    const sub = new Subject<boolean>();
    this.hubConnection.invoke('AddToBlackList', telnr, kommentar).then(() =>
      sub.next(true)
    );
    return sub;
  }
}
