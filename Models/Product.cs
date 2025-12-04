namespace Emusell.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ProductCondition Condition { get; set; }
    public DateTime CreatedAt { get; set; }
    public string SellerId { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public bool IsSold { get; set; }
    public string Location { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
    public int ViewCount { get; set; }
}

public enum ProductCondition
{
    Yeni,
    SıfırGibi,
    Iyi,
    Orta,
    Kullanilmis
}