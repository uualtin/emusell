using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Emusell.Models;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    
    public ProductCondition Condition { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    
    public bool IsSold { get; set; }
    public bool IsActive { get; set; } = true;
    public string Location { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
    public int ViewCount { get; set; }
}

public enum ProductCondition
{
    Yeni,
    SifirGibi,
    Iyi,
    Orta,
    Kullanilmis
}
