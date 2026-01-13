using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class ReviewService
{
    private readonly MongoDbService _mongoDb;

    public ReviewService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<List<Review>> GetReviewsByProductAsync(string productId)
    {
        return await _mongoDb.Reviews
            .Find(r => r.ProductId == productId && r.IsApproved)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Review>> GetReviewsByUserAsync(string userId)
    {
        return await _mongoDb.Reviews
            .Find(r => r.UserId == userId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // Satıcının ürünlerine gelen değerlendirmeler
    public async Task<List<Review>> GetReviewsBySellerAsync(string sellerId)
    {
        return await _mongoDb.Reviews
            .Find(r => r.SellerId == sellerId && r.IsApproved)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    // Satıcının yanıtlamadığı değerlendirmeler
    public async Task<List<Review>> GetUnansweredReviewsBySellerAsync(string sellerId)
    {
        return await _mongoDb.Reviews
            .Find(r => r.SellerId == sellerId && r.IsApproved && r.SellerReply == null)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(string id)
    {
        return await _mongoDb.Reviews.Find(r => r.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Review?> GetUserReviewForProductAsync(string userId, string productId)
    {
        return await _mongoDb.Reviews
            .Find(r => r.UserId == userId && r.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    public async Task<Review> CreateReviewAsync(Review review)
    {
        review.CreatedAt = DateTime.UtcNow;
        await _mongoDb.Reviews.InsertOneAsync(review);
        return review;
    }

    public async Task<bool> UpdateReviewAsync(Review review)
    {
        review.UpdatedAt = DateTime.UtcNow;
        var result = await _mongoDb.Reviews.ReplaceOneAsync(r => r.Id == review.Id, review);
        return result.ModifiedCount > 0;
    }

    // Satıcı yanıtı ekle
    public async Task<bool> AddSellerReplyAsync(string reviewId, string reply)
    {
        var update = Builders<Review>.Update
            .Set(r => r.SellerReply, reply)
            .Set(r => r.SellerReplyDate, DateTime.UtcNow);
        var result = await _mongoDb.Reviews.UpdateOneAsync(r => r.Id == reviewId, update);
        return result.ModifiedCount > 0;
    }

    // Satıcı yanıtını güncelle
    public async Task<bool> UpdateSellerReplyAsync(string reviewId, string reply)
    {
        var update = Builders<Review>.Update
            .Set(r => r.SellerReply, reply)
            .Set(r => r.SellerReplyDate, DateTime.UtcNow);
        var result = await _mongoDb.Reviews.UpdateOneAsync(r => r.Id == reviewId, update);
        return result.ModifiedCount > 0;
    }

    // Satıcı yanıtını sil
    public async Task<bool> DeleteSellerReplyAsync(string reviewId)
    {
        var update = Builders<Review>.Update
            .Set(r => r.SellerReply, (string?)null)
            .Set(r => r.SellerReplyDate, (DateTime?)null);
        var result = await _mongoDb.Reviews.UpdateOneAsync(r => r.Id == reviewId, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteReviewAsync(string id)
    {
        var result = await _mongoDb.Reviews.DeleteOneAsync(r => r.Id == id);
        return result.DeletedCount > 0;
    }

    // Ürün ortalama puanı
    public async Task<double> GetAverageRatingAsync(string productId)
    {
        var reviews = await GetReviewsByProductAsync(productId);
        if (!reviews.Any()) return 0;
        return reviews.Average(r => r.Rating);
    }

    // Satıcı ortalama puanı
    public async Task<double> GetSellerAverageRatingAsync(string sellerId)
    {
        var reviews = await GetReviewsBySellerAsync(sellerId);
        if (!reviews.Any()) return 0;
        return Math.Round(reviews.Average(r => r.Rating), 1);
    }

    // Satıcı toplam değerlendirme sayısı
    public async Task<int> GetSellerReviewCountAsync(string sellerId)
    {
        return (int)await _mongoDb.Reviews.CountDocumentsAsync(r => r.SellerId == sellerId && r.IsApproved);
    }

    // Satıcı puan dağılımı (1-5 yıldız için kaç değerlendirme var)
    public async Task<Dictionary<int, int>> GetSellerRatingDistributionAsync(string sellerId)
    {
        var reviews = await GetReviewsBySellerAsync(sellerId);
        var distribution = new Dictionary<int, int>
        {
            { 5, 0 }, { 4, 0 }, { 3, 0 }, { 2, 0 }, { 1, 0 }
        };
        
        foreach (var review in reviews)
        {
            if (distribution.ContainsKey(review.Rating))
            {
                distribution[review.Rating]++;
            }
        }
        
        return distribution;
    }

    public async Task<int> GetReviewCountAsync(string productId)
    {
        return (int)await _mongoDb.Reviews.CountDocumentsAsync(r => r.ProductId == productId && r.IsApproved);
    }

    public async Task<bool> ApproveReviewAsync(string id)
    {
        var update = Builders<Review>.Update.Set(r => r.IsApproved, true);
        var result = await _mongoDb.Reviews.UpdateOneAsync(r => r.Id == id, update);
        return result.ModifiedCount > 0;
    }
}
