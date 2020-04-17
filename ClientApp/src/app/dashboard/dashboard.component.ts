import { Component, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SignalRService } from '../services/signal-r.service';
import { Chart } from 'chart.js';
import { Stats } from '../Models/stats';
import { Subscription } from 'rxjs';

@Component({
  selector: 'sn-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit, OnDestroy {
  chart = [];
  Datums = [];
  Anrufer = [];
  Angemeldet = [];
  subscription: Subscription;
  constructor(private signal: SignalRService,
              private cdRef: ChangeDetectorRef) { }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngAfterViewInit(): void {
    // this.signal.dailyForecast()
    // .subscribe(res => {
    //   this.temp_max = res['list'].map(res => res.main.temp_max);
    //   this.temp_min = res['list'].map(res => res.main.temp_min);
    //   let alldates = res['list'].map(res => res.dt);


    //   alldates.forEach((res) => {
    //       let jsdate = new Date(res * 1000);
    //       this.weatherDates.push(jsdate.toLocaleTimeString('en', { year: 'numeric', month: 'short', day: 'numeric' }))
    //   });
    // });
    this.subscription = this.signal.AlleSub.subscribe((res: Stats[]) => {
      const format = new Intl.DateTimeFormat('de',{weekday: 'long', hour: '2-digit', minute: '2-digit'});
      this.Datums = [];
      this.Anrufer = [];
      this.Angemeldet = [];
      res.forEach((itm: Stats) => {
        this.Datums.push(format.format(itm.timestamp) + ' Uhr');
        this.Anrufer.push(itm.amtelefon);
        this.Angemeldet.push(itm.angemeldet);
      });
      this.cdRef.detectChanges();
      this.chart = new Chart('canvas', {
        type: 'line',
        data: {
          labels: this.Datums,
          datasets: [
            {
              data: this.Angemeldet,
              borderColor: '#3cba9f',
              fill: true
            },
            {
              data: this.Anrufer,
              borderColor: '#ffcc00',
              fill: true
            },
          ]
        },
        options: {
          legend: {
            display: false
          },
          scales: {
            xAxes: [{
              display: true
            }],
            yAxes: [{
              display: true
            }],
          }
        }
    });
      this.cdRef.detectChanges();
  });
  }
}
