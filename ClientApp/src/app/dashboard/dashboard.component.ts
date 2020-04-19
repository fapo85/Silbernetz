import { Component, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SignalRService } from '../services/signal-r.service';
import { Chart } from 'chart.js';
import { Stats } from '../Models/stats';
import { Subscription } from 'rxjs';
import { WaitTimeProp } from '../Models/wait-time-prop';

@Component({
  selector: 'sn-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements AfterViewInit, OnDestroy {
  chart = [];
  Datums = [];
  // Anrufer = [];
  WarteZeit = [];
  AktuellAmTelefonCl: string;
  AktuellAmTelefonNR: number;
  Allesubscription: Subscription;
  Nowsubscription: Subscription;
  constructor(private signal: SignalRService,
              private cdRef: ChangeDetectorRef) { }
  ngOnDestroy(): void {
    this.Allesubscription.unsubscribe();
    this.Nowsubscription.unsubscribe();
  }

  ngAfterViewInit(): void {
    this.Nowsubscription = this.signal.neusteSub.subscribe((status: Stats) => {
      // Aktuell Am telefon:
      this.AktuellAmTelefonNR = status.amtelefon;
      if (status.amtelefon < (status.angemeldet / 5)) {
        this.AktuellAmTelefonCl = 'green';
      } else if (status.amtelefon < (status.angemeldet / 4)) {
        this.AktuellAmTelefonCl = 'olive';
      } else if (status.amtelefon < (status.angemeldet / 3)) {
        this.AktuellAmTelefonCl = 'yellow';
      }else if (status.amtelefon < (status.angemeldet / 2)) {
        this.AktuellAmTelefonCl = 'orange';
      }else{
        this.AktuellAmTelefonCl = 'red';
      }
    });
    this.Allesubscription = this.signal.AlleSub.subscribe((res: WaitTimeProp[]) => {
      const format = new Intl.DateTimeFormat('de',{weekday: 'long', hour: '2-digit', minute: '2-digit'});
      this.Datums = [];
      this.WarteZeit = [];
      // this.Angemeldet = [];
      res.forEach((itm: Stats) => {
        this.Datums.push(format.format(itm.timestamp) + ' Uhr');
        // this.Anrufer.push(itm.amtelefon);
        this.WarteZeit.push(itm.waittime);
      });
      this.cdRef.detectChanges();
      this.chart = new Chart('canvas', {
        type: 'line',
        data: {
          labels: this.Datums,
          datasets: [
            {
              data: this.WarteZeit,
              borderColor: 'black', /*'#3cba9f'*/
              fill: true
            },
            /*{
              data: this.Anrufer,
              borderColor: '#ffcc00',
              fill: true
            },*/
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
