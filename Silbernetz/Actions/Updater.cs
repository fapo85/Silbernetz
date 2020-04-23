using Silbernetz.Database;
using Silbernetz.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Silbernetz.Actions
{
    public class Updater
    {
        private readonly InoplaClient inoplaClient;
        private readonly AnrufSafe database;
        private readonly BlackListSave blsave;
        private readonly Task runnner;
        private DateTime letzteFehlerfreieAktualisierung = DateTime.MinValue;
        private DateTime letztesAufräumen = DateTime.MinValue;
        private DateTime letztesBlacklistUpdate = DateTime.MinValue;
        private static bool OnlyOneInstanceCounter = false;
        //Es Wird immer Mindestens so Lange gewartet bis ein Arbeitsschritt abgeschlossen ist.
        private static readonly TimeSpan SchlafensZeit = TimeSpan.FromSeconds(15);
        public Updater(InoplaClient inoplaClient, AnrufSafe database, BlackListSave blsave)
        {
            if (OnlyOneInstanceCounter)
            {
                throw new Exception("es darf nur einen Updater geben, du sollst keine Anderen Updater neben mir haben");
            }
            OnlyOneInstanceCounter = true;
            this.inoplaClient = inoplaClient;
            this.database = database;
            this.blsave = blsave;
            runnner = new Task(()=> Schleife());
            runnner.Start();
        }

        private void Schleife()
        {
            Task.Delay(SchlafensZeit).Wait();
            while (true)
            {
                Task SchlafensTask = Task.Delay(SchlafensZeit);
                Task ArbeitsTask = new Task(() => Run());
                ArbeitsTask.Start();
                ArbeitsTask.Wait();
                SchlafensTask.Wait();
            }
        }

        private void Run()
        {
            LiveData liveData = null;
            LiveCalls liveCalls = null;
            Evn evn = null;
            bool fehler = false;
            try
            {
                database.UpdateStatistik();
            }
            catch (Exception e)
            {
                fehler = true;
                Console.WriteLine("Updater: Fehler Beim Update Der Statistik: " + e.Message);
            }
            try
            {
                liveData = inoplaClient.GetLiveDataAsync().Result;
            }
            catch (Exception e)
            {
                fehler = true;
                Console.WriteLine("Updater: Fehler Beim Holen der Live Daten: " + e.Message);
            }
            try
            {
                liveCalls = inoplaClient.GetLiveCallsAsync().Result;
            }
            catch (Exception e)
            {
                fehler = true;
                Console.WriteLine("Updater: Fehler Beim Holen der Live Calls: " + e.Message);
            }
            try
            {
                evn = inoplaClient.GetEVNDataAsync().Result;
            }
            catch (Exception e)
            {
                fehler = true;
                Console.WriteLine("Updater: Fehler Beim Holen der EVN Daten: " + e.Message);
            }
            if (letztesBlacklistUpdate.AddMinutes(3) < DateTime.Now)
            {
                fehler = UpdateBlacklist();
            }
            try
            {
                if (!fehler)
                {
                    letzteFehlerfreieAktualisierung = DateTime.Now;
                }
                database.NewDataToAdd(liveData, evn, liveCalls, letzteFehlerfreieAktualisierung);
            }
            catch (Exception e)
            {
                Console.WriteLine("Updater: Fehler Beim Auswerten der Daten: " + e.Message);
            }
            //Cleanup
            try
            {
                //Wird Einmal am Tag ausgeführt.
                if(letztesAufräumen.AddDays(1) < DateTime.Today.AddHours(5))
                {
                    database.AnrufCleanUp();
                    letztesAufräumen = DateTime.Today;
                    Console.WriteLine($"Es Wurde aufgeräumt: " + letztesAufräumen);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Updater: Fehler Beim Aufräumen der Daten: " + e.Message);
            }
        }
        public bool UpdateBlacklist()
        {
            try
            {
                blsave.RenewBlackList(inoplaClient.GetBlackAsync().Result);
                letztesBlacklistUpdate = DateTime.Now;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Updater: Fehler Beim Holen der Blacklist Daten: " + e.Message);
                return false;
            }
        }
    }
}
