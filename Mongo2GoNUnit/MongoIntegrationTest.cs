using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mongo2Go;
using MongoDB.Driver;

namespace Mongo2GoNUnit
{
    public class MongoIntegrationTest
    {
        internal static MongoDbRunner Runner;
        internal static string DatabaseName = "IntegrationTest";
        internal static string CollectionName = "TestCollection";

        internal static MongoCollection<T> GetCollection<T>()
        {
            Runner = MongoDbRunner.Start();
            return new MongoClient(Runner.ConnectionString).GetServer().GetDatabase(DatabaseName).GetCollection<T>(CollectionName);
        }
    }
}
