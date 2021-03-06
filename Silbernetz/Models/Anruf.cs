﻿using Silbernetz.Models.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization;

namespace Silbernetz.Models
{
    public class Anruf
    {
        /// <summary>
        /// Eindeutige Call-ID
        /// </summary>
        [JsonPropertyName("id")]
        public ulong id { get; set; }
        private string tel;
        [JsonPropertyName("telnummer")]
        public string TelNummer {
            get {
                if (string.IsNullOrEmpty(tel))
                {
                    return "Unbekannt";
                }
                return tel;
            }
            set {
                tel = value;
            }
        }
        [JsonPropertyName("service")]
        public string Service { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime TimeStamp { get; set; }
        [JsonPropertyName("inbound")]
        public ulong InBound { get; set; }
        [JsonPropertyName("outbound")]
        public ulong OutBound { get; set; }
        [JsonPropertyName("wait")]
        public ulong Wait {
            get {
                if (InBound <= OutBound)
                {
                    return 0;
                }
                return InBound - OutBound;
            }
        }
        [JsonPropertyName("target")]
        public string Target { get; set; }
        [JsonPropertyName("lauftnoch")]
        public bool lauftnoch { get; set; }
        public void DataFromEvn(Datum evn)
        {
            id = evn.Id;
            if (string.IsNullOrEmpty(tel))
            {
                TelNummer = evn.Caller;
            }

            if (string.IsNullOrEmpty(Service)) {
                Service = evn.Service;
            }
            //Service = evn.Service;
            TimeStamp = DateTime.Parse(evn.Created);
            InBound = evn.Duration.Inbound;
            OutBound = evn.Duration.Outbound;
            if (!string.IsNullOrEmpty(Target))
            {
                Target = evn.Service;
            }
            lauftnoch = false;
        }

        internal void DataFromLC(CallDatum itm)
        {
            id = itm.Id;
            TelNummer = itm.Caller;
            OutBound = (uint)itm.Duration;
            Target = itm.Destination;
            Service = itm.Service;
            lauftnoch = true;
            TimeStamp = DateTime.Parse(itm.Created);
        }
    }
}
