using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    private readonly IMongoClient _client;

    public MongoDbService(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString") ?? "mongodb://localhost:27017";
        var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName") ?? "EmusellDb";

        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(databaseName);

        // Create indexes
        CreateIndexes();
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
    public IMongoCollection<Product> Products => _database.GetCollection<Product>("products");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
    public IMongoCollection<Order> Orders => _database.GetCollection<Order>("orders");
    public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("carts");
    public IMongoCollection<Review> Reviews => _database.GetCollection<Review>("reviews");

    private void CreateIndexes()
    {
        // User indexes
        var userEmailIndex = Builders<User>.IndexKeys.Ascending(u => u.Email);
        Users.Indexes.CreateOne(new CreateIndexModel<User>(userEmailIndex, new CreateIndexOptions { Unique = true }));

        // Product indexes
        var productSellerIndex = Builders<Product>.IndexKeys.Ascending(p => p.SellerId);
        Products.Indexes.CreateOne(new CreateIndexModel<Product>(productSellerIndex));

        var productCategoryIndex = Builders<Product>.IndexKeys.Ascending(p => p.CategoryId);
        Products.Indexes.CreateOne(new CreateIndexModel<Product>(productCategoryIndex));

        // Order indexes
        var orderBuyerIndex = Builders<Order>.IndexKeys.Ascending(o => o.BuyerId);
        Orders.Indexes.CreateOne(new CreateIndexModel<Order>(orderBuyerIndex));

        // Cart indexes
        var cartUserIndex = Builders<Cart>.IndexKeys.Ascending(c => c.UserId);
        Carts.Indexes.CreateOne(new CreateIndexModel<Cart>(cartUserIndex, new CreateIndexOptions { Unique = true }));

        // Review indexes
        var reviewProductIndex = Builders<Review>.IndexKeys.Ascending(r => r.ProductId);
        Reviews.Indexes.CreateOne(new CreateIndexModel<Review>(reviewProductIndex));
    }
}
