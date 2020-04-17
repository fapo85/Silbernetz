import { ParsedEventType } from '@angular/compiler';

export class CallEvents {
  timestamp: Date;
  event: EventType;
}
export enum EventType{
  ringing, ringing_outbound, connect, connect_outbound, hangup, heartbeat, start_rp, start_skill
}
