import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { Stats } from '../Models/stats';
import { environment } from 'src/environments/environment';
import { Anrufer } from '../Models/anrufer';
import { CallEvents } from '../Models/call-events';
import { AnrufExport } from '../Models/anruf-export';

@Injectable({
  providedIn: 'root'
})
export class SignalRService{
  private hubConnection: signalR.HubConnection;
  anruferHeute: BehaviorSubject<Anrufer[]>;
  neusteSub = new BehaviorSubject<Stats>(Stats.GetEmpty());
  AlleSub = new BehaviorSubject<Stats[]>([]);
  constructor() {
    this.startConnection();
    this.AddListener();
  }
  public startConnection = () => {
    if (environment.production){
      this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/Hub')
      .build();
    }else{
      this.hubConnection = new signalR.HubConnectionBuilder()
                              .withUrl('http://localhost:5000/Hub')
                              .build();
    }

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
      this.AlleSub.next([...this.AlleSub.value, data]);
      if(this.anruferHeute){
        this.HoleAnruferHeute();
      }
    });
    this.hubConnection.on('manystats', (data: Stats[]) => {
      data.forEach((itm: Stats) => itm.timestamp = new Date(itm.timestamp));
      this.AlleSub.next(data);
    });
  }
  private HoleAnruferHeute(){
    //Warten falls noch nicht Verbunden
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected){
      setTimeout(() => this.HoleAnruferHeute(), 10);
    } else {
      this.hubConnection.invoke('AnrufFromToday').then((data: Anrufer[]) => {
        data.forEach((anrufer: Anrufer) => {
          anrufer.anrufe.forEach((anruf: AnrufExport) => {
            anruf.events.forEach((event: CallEvents) => {
              event.timestamp = new Date(event.timestamp);
            });
          });
          this.anruferHeute.next(data);
        });
      });
    }
  }
  public GetStats(): Observable<Stats>{
    return this.neusteSub;
  }
  public GetAllStats(): Observable<Stats[]>{
    return this.AlleSub;
  }
  public GetAnruferHeute(): Observable<Anrufer[]> {
    if (!this.anruferHeute){
      this.anruferHeute = new BehaviorSubject<Anrufer[]>([]);
      this.HoleAnruferHeute();
    }
    return this.anruferHeute;
  }
}
