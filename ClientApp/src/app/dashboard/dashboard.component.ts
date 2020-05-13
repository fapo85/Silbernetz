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
  AktuellWarteCl: string;
  AktuellWarteNR: string;
  AktuellAngemeldetNr: number;
  Allesubscription: Subscription;
  Nowsubscription: Subscription;
  naechsteAktualisierungZP = 0;
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
      this.AktuellWarteNR = '';
      if(status.waittime > 60){
        this.AktuellWarteNR += status.waittime / 60 + 'Min ';
      }
      this.AktuellWarteNR += status.waittime % 60 + 'Sek';
      if (status.waittime < 45) {
        this.AktuellWarteCl = 'green';
      } else if (status.waittime < 55 ){
        this.AktuellWarteCl = 'olive';
      } else if (status.waittime < 65) {
        this.AktuellWarteCl = 'yellow';
      }else if (status.waittime < 80) {
        this.AktuellWarteCl = 'orange';
      }else{
        this.AktuellWarteCl = 'red';
      }
      this.AktuellAngemeldetNr = status.angemeldet;
    });
    this.Allesubscription = this.signal.AlleSub.subscribe((res: WaitTimeProp[]) => {
      const now = new Date();
      if (this.naechsteAktualisierungZP > now.getTime() || this.naechsteAktualisierungZP === 0){
      this.naechsteAktualisierungZP = (now.getTime() + 600000);
      const format = new Intl.DateTimeFormat('de', {weekday: 'long', hour: '2-digit', minute: '2-digit'});
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
    }
  });
  }
}
