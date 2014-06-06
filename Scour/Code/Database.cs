using System;
using System.Collections;
using System.Configuration;
using System.DirectoryServices;
using System.Management;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;

namespace Scour.Code {
    class Database {

        string hostKey = "mongodb.host";
        string nameKey = "mongodb.database.name";
        string collectionKey = "mongodb.database.collection.computers";

        public void Write() {
            var connectionString = ConfigurationManager.AppSettings[hostKey];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings[nameKey]);
            var collection = database.GetCollection(ConfigurationManager.AppSettings[collectionKey]);
        }        
    }
}
