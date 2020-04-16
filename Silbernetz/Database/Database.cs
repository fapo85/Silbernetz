using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Silbernetz.Controllers.SignalHub;
using Silbernetz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Silbernetz.Database
{
    public class Database
    {
        private readonly IMongoClient mongoClient;
        private readonly IMongoDatabase mongoDatabase;
        public Database(IOptions<SecretConf> options, IHubContext<SignalRHub> hubContext)
        {
            var mongoDbConf = options.Value;
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(mongoDbConf.DBConnection));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            mongoClient = new MongoClient(settings);
            mongoDatabase = mongoClient.GetDatabase(mongoDbConf.DbName);

        }
    }
}
