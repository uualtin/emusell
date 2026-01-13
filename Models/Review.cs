using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Emusell.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; } = string.Empty;
    public string ProductTitle { get; set; } = string.Empty;
    public string? ProductImageUrl { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? UserProfileImage { get; set; }

    public int Rating { get; set; } // 1-5 arası
    public string Comment { get; set; } = string.Empty;
    
    // Satıcı Yanıtı
    public string? SellerReply { get; set; }
    public DateTime? SellerReplyDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsApproved { get; set; } = true;
}
