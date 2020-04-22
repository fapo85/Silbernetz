import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { BLIDatum } from '../Models/blidatum';
import { SignalRService } from '../services/signal-r.service';

@Component({
  selector: 'sn-blacklist',
  templateUrl: './blacklist.component.html',
  styleUrls: ['./blacklist.component.scss']
})
export class BlacklistComponent implements OnInit {
  Data: BLIDatum[] = [];
  EintragSperrenOpen: boolean = false;
  EintragSperrenTel: string;
  constructor(private signal: SignalRService) { }

  ngOnInit(): void {
    this.update();
  }
  private update(){
    this.signal.GetBlacklist().subscribe((data: BLIDatum[]) => {
      this.Data = data;
    });
  }
  addBlackList(){
    this.EintragSperrenTel = '';
    this.EintragSperrenOpen = true;
  }
  addBlackListClose(evt: any){
    this.EintragSperrenOpen = false;
    this.update();
  }
}
