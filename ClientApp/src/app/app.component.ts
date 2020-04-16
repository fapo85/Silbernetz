import { Component, OnInit } from '@angular/core';
import { SignalRService } from './services/signal-r.service';
import { Stats } from './Models/stats';

@Component({
  selector: 'sn-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'Silbernetz';
  data: Stats = new Stats();
  constructor(private signal: SignalRService){
  }
  ngOnInit(): void {
    this.signal.GetStats().subscribe(data => {
      this.data = data;
    });
  }
}
