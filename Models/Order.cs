using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Emusell.Models;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string OrderNumber { get; set; } = string.Empty;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string BuyerId { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public string BuyerPhone { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public string? PaymentMethod { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? Notes { get; set; }
}

public class OrderItem
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; } = string.Empty;
    public string ProductTitle { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
}

public enum OrderStatus
{
    Pending,      // Beklemede
    Confirmed,    // Onaylandı
    Shipped,      // Kargoya Verildi
    Delivered,    // Teslim Edildi
    Cancelled,    // İptal Edildi
    Refunded      // İade Edildi
}

public enum PaymentStatus
{
    Pending,      // Beklemede
    Completed,    // Tamamlandı
    Failed,       // Başarısız
    Refunded      // İade Edildi
}
