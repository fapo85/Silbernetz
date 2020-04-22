import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SignalRService } from '../services/signal-r.service';

@Component({
  selector: 'sn-add-black-entry',
  templateUrl: './add-black-entry.component.html',
  styleUrls: ['./add-black-entry.component.scss']
})
export class AddBlackEntryComponent implements OnInit {
  kommentar: string = '';
  @Input() telnr: string = '';
  @Output() formclose = new EventEmitter<boolean>();
  constructor(private signal: SignalRService) { }

  ngOnInit(): void {
  }
  isValid(){
    return this.kommentar.length > 10  && this.telnr.length > 4;
  }
  onSubmit(){
    this.signal.AddBlacklistEntry(this.telnr, this.kommentar).subscribe(() => {
      this.formclose.emit(true);
    });
  }
  onCancel(){
    this.formclose.emit(false);
  }
}
