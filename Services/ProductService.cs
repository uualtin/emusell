using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class ProductService
{
    private readonly MongoDbService _mongoDb;

    public ProductService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _mongoDb.Products
            .Find(p => !p.IsSold && p.IsActive)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
        var product = await _mongoDb.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        if (product != null)
        {
            var update = Builders<Product>.Update.Inc(p => p.ViewCount, 1);
            await _mongoDb.Products.UpdateOneAsync(p => p.Id == id, update);
        }
        return product;
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(string categoryId)
    {
        return await _mongoDb.Products
            .Find(p => p.CategoryId == categoryId && !p.IsSold && p.IsActive)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsBySellerAsync(string sellerId)
    {
        return await _mongoDb.Products
            .Find(p => p.SellerId == sellerId)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllProductsAsync();

        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq(p => p.IsSold, false),
            Builders<Product>.Filter.Eq(p => p.IsActive, true),
            Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Regex(p => p.Title, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Product>.Filter.Regex(p => p.Description, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            )
        );

        return await _mongoDb.Products.Find(filter).SortByDescending(p => p.CreatedAt).ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.ViewCount = 0;
        await _mongoDb.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var result = await _mongoDb.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
        var result = await _mongoDb.Products.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> MarkAsSoldAsync(string id)
    {
        var update = Builders<Product>.Update.Set(p => p.IsSold, true);
        var result = await _mongoDb.Products.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> MarkAsAvailableAsync(string id)
    {
        var update = Builders<Product>.Update.Set(p => p.IsSold, false);
        var result = await _mongoDb.Products.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> SetActiveStatusAsync(string id, bool isActive)
    {
        var update = Builders<Product>.Update.Set(p => p.IsActive, isActive);
        var result = await _mongoDb.Products.UpdateOneAsync(p => p.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<int> GetProductCountBySellerAsync(string sellerId)
    {
        return (int)await _mongoDb.Products.CountDocumentsAsync(p => p.SellerId == sellerId);
    }

    public async Task<int> GetSoldProductCountBySellerAsync(string sellerId)
    {
        return (int)await _mongoDb.Products.CountDocumentsAsync(p => p.SellerId == sellerId && p.IsSold);
    }

    public async Task<int> GetTotalProductCountAsync()
    {
        return (int)await _mongoDb.Products.CountDocumentsAsync(_ => true);
    }

    public async Task<List<Product>> GetFeaturedProductsAsync(int count = 8)
    {
        return await _mongoDb.Products
            .Find(p => !p.IsSold && p.IsActive)
            .SortByDescending(p => p.ViewCount)
            .Limit(count)
            .ToListAsync();
    }
}
