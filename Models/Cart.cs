using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Emusell.Models;

public class Cart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;

    public List<CartItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [BsonIgnore]
    public decimal TotalAmount => Items.Sum(i => i.Price * i.Quantity);

    [BsonIgnore]
    public int TotalItems => Items.Sum(i => i.Quantity);
}

public class CartItem
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
