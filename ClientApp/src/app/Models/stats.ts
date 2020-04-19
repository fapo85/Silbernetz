import { WaitTimeProp } from './wait-time-prop';

export class Stats extends WaitTimeProp {
  angemeldet: number;
  amtelefon: number;
  benutzer: number;
  public static GetEmpty(): Stats {
    const stats = new Stats();
    stats.angemeldet = 0;
    stats.amtelefon = 0;
    stats.benutzer = 0;
    stats.timestamp = new Date(0);
    return stats;
  }
}
