using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class CartService
{
    private readonly MongoDbService _mongoDb;

    public CartService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        var cart = await _mongoDb.Carts.Find(c => c.UserId == userId).FirstOrDefaultAsync();
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _mongoDb.Carts.InsertOneAsync(cart);
        }
        return cart;
    }

    public async Task<bool> AddToCartAsync(string userId, CartItem item)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart.Items.Add(item);
        }

        cart.UpdatedAt = DateTime.UtcNow;
        var result = await _mongoDb.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> RemoveFromCartAsync(string userId, string productId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        cart.Items.RemoveAll(i => i.ProductId == productId);
        cart.UpdatedAt = DateTime.UtcNow;
        var result = await _mongoDb.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdateQuantityAsync(string userId, string productId, int quantity)
    {
        var cart = await GetCartByUserIdAsync(userId);
        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            cart.UpdatedAt = DateTime.UtcNow;
            var result = await _mongoDb.Carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return result.ModifiedCount > 0;
        }

        return false;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        var update = Builders<Cart>.Update
            .Set(c => c.Items, new List<CartItem>())
            .Set(c => c.UpdatedAt, DateTime.UtcNow);
        var result = await _mongoDb.Carts.UpdateOneAsync(c => c.UserId == userId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<int> GetCartItemCountAsync(string userId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        return cart.TotalItems;
    }
}
