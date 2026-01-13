using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class OrderService
{
    private readonly MongoDbService _mongoDb;

    public OrderService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _mongoDb.Orders
            .Find(_ => true)
            .SortByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        return await _mongoDb.Orders.Find(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Order?> GetOrderByOrderNumberAsync(string orderNumber)
    {
        return await _mongoDb.Orders.Find(o => o.OrderNumber == orderNumber).FirstOrDefaultAsync();
    }

    public async Task<List<Order>> GetOrdersByBuyerAsync(string buyerId)
    {
        return await _mongoDb.Orders
            .Find(o => o.BuyerId == buyerId)
            .SortByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Order>> GetOrdersBySellerAsync(string sellerId)
    {
        var filter = Builders<Order>.Filter.ElemMatch(o => o.Items, i => i.SellerId == sellerId);
        return await _mongoDb.Orders
            .Find(filter)
            .SortByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        order.OrderNumber = GenerateOrderNumber();
        order.CreatedAt = DateTime.UtcNow;
        await _mongoDb.Orders.InsertOneAsync(order);
        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status)
    {
        var update = Builders<Order>.Update
            .Set(o => o.Status, status)
            .Set(o => o.UpdatedAt, DateTime.UtcNow);
        var result = await _mongoDb.Orders.UpdateOneAsync(o => o.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> UpdatePaymentStatusAsync(string id, PaymentStatus status)
    {
        var update = Builders<Order>.Update
            .Set(o => o.PaymentStatus, status)
            .Set(o => o.UpdatedAt, DateTime.UtcNow);
        var result = await _mongoDb.Orders.UpdateOneAsync(o => o.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> CancelOrderAsync(string id)
    {
        var update = Builders<Order>.Update
            .Set(o => o.Status, OrderStatus.Cancelled)
            .Set(o => o.UpdatedAt, DateTime.UtcNow);
        var result = await _mongoDb.Orders.UpdateOneAsync(o => o.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> RefundOrderAsync(string id)
    {
        var update = Builders<Order>.Update
            .Set(o => o.Status, OrderStatus.Refunded)
            .Set(o => o.PaymentStatus, PaymentStatus.Refunded)
            .Set(o => o.UpdatedAt, DateTime.UtcNow);
        var result = await _mongoDb.Orders.UpdateOneAsync(o => o.Id == id, update);
        return result.ModifiedCount > 0;
    }

    public async Task<int> GetTotalOrderCountAsync()
    {
        return (int)await _mongoDb.Orders.CountDocumentsAsync(_ => true);
    }

    public async Task<int> GetOrderCountByStatusAsync(OrderStatus status)
    {
        return (int)await _mongoDb.Orders.CountDocumentsAsync(o => o.Status == status);
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        var orders = await _mongoDb.Orders
            .Find(o => o.PaymentStatus == PaymentStatus.Completed)
            .ToListAsync();
        return orders.Sum(o => o.TotalAmount);
    }

    public async Task<decimal> GetSellerRevenueAsync(string sellerId)
    {
        var orders = await GetOrdersBySellerAsync(sellerId);
        var completedOrders = orders.Where(o => o.PaymentStatus == PaymentStatus.Completed);
        return completedOrders.Sum(o => o.Items.Where(i => i.SellerId == sellerId).Sum(i => i.Price * i.Quantity));
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
