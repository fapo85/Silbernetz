import { AnrufExport } from './anruf-export';

export class Anrufer {
  telnummer: number;
  gesamtdauer: number;
  gesamtdauerdays: number;
  anzahl: number;
  anzahldays: number;
  anrufe: AnrufExport[];
}
