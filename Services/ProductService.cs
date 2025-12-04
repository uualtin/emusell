using Emusell.Models;

namespace Emusell.Services;

public class ProductService
{
    private static List<Product> _products = new();

    public ProductService()
    {
        // Seed data - İlk yüklemede mock data
        if (!_products.Any())
        {
            _products = GenerateMockProducts();
        }
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        await Task.Delay(500); // Simulate API delay
        return _products.Where(p => !p.IsSold).OrderByDescending(p => p.CreatedAt).ToList();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        await Task.Delay(300);
        var product = _products.FirstOrDefault(p => p.Id == id);
        
        if (product != null)
        {
            product.ViewCount++;
        }
        
        return product;
    }

    public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        await Task.Delay(400);
        return _products
            .Where(p => p.CategoryId == categoryId && !p.IsSold)
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm)
    {
        await Task.Delay(300);
        
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllProductsAsync();

        var term = searchTerm.ToLower();
        return _products
            .Where(p => !p.IsSold && 
                   (p.Title.ToLower().Contains(term) || 
                    p.Description.ToLower().Contains(term) ||
                    p.CategoryName.ToLower().Contains(term)))
            .OrderByDescending(p => p.CreatedAt)
            .ToList();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await Task.Delay(300);
        
        product.Id = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
        product.CreatedAt = DateTime.Now;
        product.ViewCount = 0;
        
        _products.Add(product);
        return product;
    }

    private List<Product> GenerateMockProducts()
    {
        return new List<Product>
        {
            new Product
            {
                Id = 1,
                Title = "iPhone 13 Pro - 128GB",
                Description = "Çok az kullanılmış, hiç çizik yok. Tüm aksesuarları ve kutusu mevcut. Garantisi devam ediyor.",
                Price = 28000,
                ImageUrl = "https://images.unsplash.com/photo-1632661674596-df8be070a5c5?w=600&h=400&fit=crop",
                CategoryId = 1,
                CategoryName = "Elektronik",
                Condition = ProductCondition.SıfırGibi,
                CreatedAt = DateTime.Now.AddDays(-2),
                SellerId = "seller1",
                SellerName = "Ahmet Yılmaz",
                IsSold = false,
                Location = "İstanbul, Kadıköy",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1632661674596-df8be070a5c5?w=600&h=400&fit=crop",
                    "https://images.unsplash.com/photo-1632633173522-c9b5a07b7346?w=600&h=400&fit=crop"
                },
                ViewCount = 45
            },
            new Product
            {
                Id = 2,
                Title = "MacBook Pro 14\" M1 Pro",
                Description = "2021 model, hiç kullanılmamış gibi. 16GB RAM, 512GB SSD. Orijinal şarj aleti ve kutusu mevcut.",
                Price = 45000,
                ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=600&h=400&fit=crop",
                CategoryId = 1,
                CategoryName = "Elektronik",
                Condition = ProductCondition.SıfırGibi,
                CreatedAt = DateTime.Now.AddDays(-5),
                SellerId = "seller2",
                SellerName = "Zeynep Kaya",
                IsSold = false,
                Location = "Ankara, Çankaya",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=600&h=400&fit=crop"
                },
                ViewCount = 78
            },
            new Product
            {
                Id = 3,
                Title = "PlayStation 5",
                Description = "2 kol, 3 oyun ile birlikte. Sorunsuz çalışıyor, garantisi var.",
                Price = 12000,
                ImageUrl = "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=600&h=400&fit=crop",
                CategoryId = 1,
                CategoryName = "Elektronik",
                Condition = ProductCondition.Iyi,
                CreatedAt = DateTime.Now.AddDays(-7),
                SellerId = "seller3",
                SellerName = "Mehmet Demir",
                IsSold = false,
                Location = "İzmir, Bornova",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=600&h=400&fit=crop"
                },
                ViewCount = 123
            },
            new Product
            {
                Id = 4,
                Title = "Zara Kadın Mont - S Beden",
                Description = "Bir sezon kullanıldı, temiz ve bakımlı. Kışlık, kalın kumaş.",
                Price = 350,
                ImageUrl = "https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=600&h=400&fit=crop",
                CategoryId = 2,
                CategoryName = "Giyim & Moda",
                Condition = ProductCondition.Iyi,
                CreatedAt = DateTime.Now.AddDays(-3),
                SellerId = "seller4",
                SellerName = "Ayşe Şahin",
                IsSold = false,
                Location = "İstanbul, Beşiktaş",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1539533018447-63fcce2678e3?w=600&h=400&fit=crop"
                },
                ViewCount = 34
            },
            new Product
            {
                Id = 5,
                Title = "Nike Air Max 90 - 42 Numara",
                Description = "Orijinal, az kullanılmış. Kutusuz ama temiz.",
                Price = 800,
                ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&h=400&fit=crop",
                CategoryId = 2,
                CategoryName = "Giyim & Moda",
                Condition = ProductCondition.Iyi,
                CreatedAt = DateTime.Now.AddDays(-4),
                SellerId = "seller5",
                SellerName = "Can Öztürk",
                IsSold = false,
                Location = "Bursa, Nilüfer",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&h=400&fit=crop"
                },
                ViewCount = 56
            },
            new Product
            {
                Id = 6,
                Title = "IKEA Koltuk Takımı - 3+2+1",
                Description = "2 yıllık, hafif kullanılmış. Temiz ve sağlam. Demontaj yapılabilir.",
                Price = 8500,
                ImageUrl = "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=600&h=400&fit=crop",
                CategoryId = 3,
                CategoryName = "Ev & Yaşam",
                Condition = ProductCondition.Iyi,
                CreatedAt = DateTime.Now.AddDays(-6),
                SellerId = "seller6",
                SellerName = "Elif Yıldız",
                IsSold = false,
                Location = "Ankara, Keçiören",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?w=600&h=400&fit=crop"
                },
                ViewCount = 67
            },
            new Product
            {
                Id = 7,
                Title = "Harry Potter Serisi - 7 Kitap",
                Description = "Tam set, temiz durumda. Ciltli özel baskı.",
                Price = 600,
                ImageUrl = "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=600&h=400&fit=crop",
                CategoryId = 4,
                CategoryName = "Kitap & Hobi",
                Condition = ProductCondition.SıfırGibi,
                CreatedAt = DateTime.Now.AddDays(-1),
                SellerId = "seller7",
                SellerName = "Emre Aksoy",
                IsSold = false,
                Location = "İstanbul, Şişli",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1544947950-fa07a98d237f?w=600&h=400&fit=crop"
                },
                ViewCount = 23
            },
            new Product
            {
                Id = 8,
                Title = "Yamaha Gitar - Akustik",
                Description = "Profesyonel model, çok az kullanılmış. Kılıf dahil.",
                Price = 3500,
                ImageUrl = "https://images.unsplash.com/photo-1510915361894-db8b60106cb1?w=600&h=400&fit=crop",
                CategoryId = 4,
                CategoryName = "Kitap & Hobi",
                Condition = ProductCondition.SıfırGibi,
                CreatedAt = DateTime.Now.AddDays(-8),
                SellerId = "seller8",
                SellerName = "Burak Çelik",
                IsSold = false,
                Location = "İzmir, Karşıyaka",
                Images = new List<string>
                {
                    "https://images.unsplash.com/photo-1510915361894-db8b60106cb1?w=600&h=400&fit=crop"
                },
                ViewCount = 89
            }
        };
    }
}