import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService } from '../services/signal-r.service';
import { Subscription } from 'rxjs';
import { Anrufer } from '../Models/anrufer';

@Component({
  selector: 'sn-call-today',
  templateUrl: './call-today.component.html',
  styleUrls: ['./call-today.component.scss']
})
export class CallTodayComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  Data: Anrufer[] = [];
  EintragSperrenOpen: boolean = false;
  EintragDetailsOpen: boolean = false;
  EintragSperrenTel: string;
  EintragDetailsItm: Anrufer;
  constructor(private signal: SignalRService) { }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  ngOnInit(): void {
    this.subscription = this.signal.GetAnruferHeute().subscribe((data: Anrufer[]) =>{
      this.Data = data;
    });
  }
  CalcGesamtDauer(sekunden: number): string{
    let ret: string = '';
    if(sekunden > 60){
      ret += (sekunden / 60).toString().split('.')[0];
      ret += ' Minuten ';
    }
    ret += sekunden % 60;
    ret += ' Sekunden';
    return ret;
  }
  addBlackList(anrufer: Anrufer){
    this.EintragSperrenTel = anrufer.telnummer.toString();
    this.EintragSperrenOpen = true;
    this.EintragDetailsItm = anrufer;
    this.EintragDetailsOpen = true;
  }
  addBlackListClose(evt: any){
    this.EintragSperrenOpen = false;
  }
  ShowDetails(anrufer: Anrufer){
    this.EintragDetailsItm = anrufer;
    this.EintragDetailsOpen = true;
    this.EintragSperrenOpen = false;
    this.EintragSperrenTel = null;
  }
}
