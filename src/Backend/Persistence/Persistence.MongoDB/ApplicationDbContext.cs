using Common.Domain.Entities;
using MongoDB.Driver;
using Persistence.MongoDB.Configuration;

namespace Persistence.MongoDB
{
    public class ApplicationDbContext
    {
        public IMongoCollection<Logger> Loggers;

        public ApplicationDbContext(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Loggers = database.GetCollection<Logger>(settings.LoggerCollectionName);
        }
    }
}
