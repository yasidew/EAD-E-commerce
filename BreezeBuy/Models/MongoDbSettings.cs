// Contains the MongoDbSettings class which is used to store the connection string, database name, and collection names for the MongoDB database.
namespace BreezeBuy.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string InventoryCollectionName { get; set; }
    }
}