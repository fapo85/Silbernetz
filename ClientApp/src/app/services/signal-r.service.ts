import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { Stats } from '../Models/stats';

@Injectable({
  providedIn: 'root'
})
export class SignalRService{
  private hubConnection: signalR.HubConnection;
  neusteSub = new BehaviorSubject<Stats>(Stats.GetEmpty());
  AlleSub = new BehaviorSubject<Stats[]>([]);
  constructor() {
    this.startConnection();
    this.AddListener();
  }
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl('https://localhost:5001/hub')
                            .build();

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
    });
  }
  public GetStats(): Observable<Stats>{
    return this.neusteSub;
  }
  public GetAllStats(): Observable<Stats[]>{
    return this.AlleSub;
  }
}
