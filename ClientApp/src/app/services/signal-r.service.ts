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
                            .withUrl('http://localhost:5000/Hub')
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
    this.hubConnection.on('manystats', (data: Stats[]) => {
      data.forEach((itm: Stats) => itm.timestamp = new Date(itm.timestamp));
      this.AlleSub.next(data);
    });
  }
  public GetStats(): Observable<Stats>{
    return this.neusteSub;
  }
  public GetAllStats(): Observable<Stats[]>{
    return this.AlleSub;
  }
  weather = new BehaviorSubject<any>({"message":"","cod":"200","city_id":2643743,"calctime":0.0875,"cnt":3,"list":[{"main":{"temp":279.946,"temp_min":279.946,"temp_max":279.946,"pressure":1016.76,"sea_level":1024.45,"grnd_level":1016.76,"humidity":100},"wind":{"speed":4.59,"deg":163.001},"clouds":{"all":92},"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10n"}],"rain":{"3h":2.69},"dt":1485717216},{"main":{"temp":282.597,"temp_min":282.597,"temp_max":282.597,"pressure":1012.12,"sea_level":1019.71,"grnd_level":1012.12,"humidity":98},"wind":{"speed":4.04,"deg":226},"clouds":{"all":92},"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10n"}],"rain":{"3h":0.405},"dt":1485745061},{"main":{"temp":279.38,"pressure":1011,"humidity":93,"temp_min":278.15,"temp_max":280.15},"wind":{"speed":2.6,"deg":30},"clouds":{"all":90},"weather":[{"id":701,"main":"Mist","description":"mist","icon":"50d"},{"id":741,"main":"Fog","description":"fog","icon":"50d"}],"dt":1485768552}]});
  dailyForecast(): Observable<any> {
    return this.weather;
  }
}
