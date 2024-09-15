using BreezeBuy.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BreezeBuy.Services
{
    public class InventoryService
    {
        private readonly IMongoCollection<Inventory> _inventoryCollection;
        // private readonly OrderService _orderService;

        public InventoryService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var settings = mongoDbSettings.Value;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _inventoryCollection = database.GetCollection<Inventory>(settings.InventoryCollectionName);
            //  _orderService = orderService;
        }

        //get all inventory items
        public async Task<List<Inventory>> GetAsync() =>
            await _inventoryCollection.Find(inventory => true).ToListAsync();

        //get inventory item by id
        public async Task<Inventory> GetIByIdAsync(string id) =>
            await _inventoryCollection.Find(inventory => inventory.Id == id).FirstOrDefaultAsync();

        //create a new inventory item
        public async Task CreateAsync(Inventory newInventory) =>
            await _inventoryCollection.InsertOneAsync(newInventory);

        //update an inventory item
        public async Task UpdateAsync(string id, Inventory updatedInventory)
        {
            updatedInventory.Id = id; // Ensure the Id is set correctly
            await _inventoryCollection.ReplaceOneAsync(x => x.Id == id, updatedInventory);
        }

        //delete an inventory item
        public async Task RemoveAsync(string id) =>
            await _inventoryCollection.DeleteOneAsync(inventory => inventory.Id == id);


         // Check for low stock and return a list of items that need to be reordered
        public async Task <List<Inventory>> GetLowStockItemsAsync()
        {
            var lowStockItems =  await _inventoryCollection.Find(inventory => inventory.QuantityAvailable < inventory.ReoderLevel).ToListAsync();
            return lowStockItems;
        }

    //     public async Task RemoveAsync(string id)
    // {
    //     // Check if the product has any pending orders
    //     var hasPendingOrders = await _orderService.HasPendingOrdersForProduct(id);
    //     if (hasPendingOrders)
    //     {
    //         throw new InvalidOperationException("Cannot remove product with pending orders.");
    //     }

    //     // If no pending orders, proceed with removal
    //     await _inventoryCollection.DeleteOneAsync(inventory => inventory.Id == id);
    // }

    }
}