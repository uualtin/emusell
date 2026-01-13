using MongoDB.Driver;
using Emusell.Models;

namespace Emusell.Services;

public class CategoryService
{
    private readonly MongoDbService _mongoDb;

    public CategoryService(MongoDbService mongoDb)
    {
        _mongoDb = mongoDb;
    }

    // Tüm aktif kategorileri getir
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _mongoDb.Categories
            .Find(c => c.IsActive)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Admin için tüm kategorileri getir (aktif/pasif)
    public async Task<List<Category>> GetAllCategoriesIncludingInactiveAsync()
    {
        return await _mongoDb.Categories
            .Find(_ => true)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Sadece ana kategorileri getir (ParentCategoryId null olanlar)
    public async Task<List<Category>> GetMainCategoriesAsync()
    {
        return await _mongoDb.Categories
            .Find(c => c.ParentCategoryId == null && c.IsActive)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Admin için tüm ana kategorileri getir
    public async Task<List<Category>> GetMainCategoriesIncludingInactiveAsync()
    {
        return await _mongoDb.Categories
            .Find(c => c.ParentCategoryId == null)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Bir ana kategorinin alt kategorilerini getir
    public async Task<List<Category>> GetSubcategoriesAsync(string parentId)
    {
        return await _mongoDb.Categories
            .Find(c => c.ParentCategoryId == parentId && c.IsActive)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Admin için bir ana kategorinin tüm alt kategorilerini getir
    public async Task<List<Category>> GetSubcategoriesIncludingInactiveAsync(string parentId)
    {
        return await _mongoDb.Categories
            .Find(c => c.ParentCategoryId == parentId)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Sadece alt kategorileri getir (ParentCategoryId dolu olanlar)
    public async Task<List<Category>> GetAllSubcategoriesAsync()
    {
        return await _mongoDb.Categories
            .Find(c => c.ParentCategoryId != null && c.IsActive)
            .SortBy(c => c.Name)
            .ToListAsync();
    }

    // Eski uyumluluk için
    public async Task<List<Category>> GetParentCategoriesAsync()
    {
        return await GetMainCategoriesAsync();
    }

    public async Task<Category?> GetCategoryByIdAsync(string id)
    {
        return await _mongoDb.Categories.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug)
    {
        return await _mongoDb.Categories.Find(c => c.Slug == slug && c.IsActive).FirstOrDefaultAsync();
    }

    // Ana kategori ve alt kategorileri slug ile getir
    public async Task<(Category? MainCategory, List<Category> SubCategories)> GetCategoryWithSubcategoriesBySlugAsync(string slug)
    {
        var category = await GetCategoryBySlugAsync(slug);
        if (category == null) return (null, new List<Category>());
        
        var subCategories = await GetSubcategoriesAsync(category.Id);
        return (category, subCategories);
    }

    // Kategori ID'sine göre ana kategoriyi getir (eğer alt kategoriyse ana kategorisini döndür)
    public async Task<Category?> GetMainCategoryAsync(string categoryId)
    {
        var category = await GetCategoryByIdAsync(categoryId);
        if (category == null) return null;
        
        if (category.ParentCategoryId == null)
            return category;
        
        return await GetCategoryByIdAsync(category.ParentCategoryId);
    }

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        category.Slug = GenerateSlug(category.Name);
        await _mongoDb.Categories.InsertOneAsync(category);
        return category;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        category.Slug = GenerateSlug(category.Name);
        var result = await _mongoDb.Categories.ReplaceOneAsync(c => c.Id == category.Id, category);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategoryAsync(string id)
    {
        // Önce alt kategorileri kontrol et
        var subCategories = await GetSubcategoriesIncludingInactiveAsync(id);
        if (subCategories.Any())
        {
            // Alt kategorileri de sil
            foreach (var sub in subCategories)
            {
                await _mongoDb.Categories.DeleteOneAsync(c => c.Id == sub.Id);
            }
        }
        
        var result = await _mongoDb.Categories.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<bool> SetActiveStatusAsync(string id, bool isActive)
    {
        var update = Builders<Category>.Update.Set(c => c.IsActive, isActive);
        var result = await _mongoDb.Categories.UpdateOneAsync(c => c.Id == id, update);
        return result.ModifiedCount > 0;
    }

    // Alt kategori sayısını getir
    public async Task<int> GetSubcategoryCountAsync(string parentId)
    {
        return (int)await _mongoDb.Categories.CountDocumentsAsync(c => c.ParentCategoryId == parentId);
    }

    private string GenerateSlug(string name)
    {
        return name.ToLower()
            .Replace("ı", "i")
            .Replace("ğ", "g")
            .Replace("ü", "u")
            .Replace("ş", "s")
            .Replace("ö", "o")
            .Replace("ç", "c")
            .Replace(" ", "-")
            .Replace("&", "")
            .Replace(".", "");
    }

    public async Task SeedDefaultCategoriesAsync()
    {
        var count = await _mongoDb.Categories.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        // Ana Kategoriler
        var elektronik = new Category { Name = "Elektronik", Slug = "elektronik", Icon = "bi-phone", Description = "Telefon, bilgisayar, tablet ve daha fazlası" };
        var giyim = new Category { Name = "Giyim & Moda", Slug = "giyim", Icon = "bi-bag", Description = "Kıyafet, ayakkabı, çanta ve aksesuarlar" };
        var evYasam = new Category { Name = "Ev & Yaşam", Slug = "ev-yasam", Icon = "bi-house", Description = "Mobilya, dekorasyon, ev eşyaları" };
        var kitapHobi = new Category { Name = "Kitap & Hobi", Slug = "kitap-hobi", Icon = "bi-book", Description = "Kitaplar, müzik aletleri, oyuncaklar" };
        var tasit = new Category { Name = "Taşıt", Slug = "tasit", Icon = "bi-bicycle", Description = "Araba, bisiklet, motosiklet" };
        var spor = new Category { Name = "Spor & Outdoor", Slug = "spor", Icon = "bi-trophy", Description = "Spor ekipmanları, kamp malzemeleri" };

        var mainCategories = new List<Category> { elektronik, giyim, evYasam, kitapHobi, tasit, spor };
        await _mongoDb.Categories.InsertManyAsync(mainCategories);

        // Alt Kategoriler
        var subCategories = new List<Category>
        {
            // Elektronik alt kategorileri
            new Category { Name = "Cep Telefonu", Slug = "cep-telefonu", Icon = "bi-phone", ParentCategoryId = elektronik.Id, Description = "Akıllı telefonlar" },
            new Category { Name = "Bilgisayar", Slug = "bilgisayar", Icon = "bi-laptop", ParentCategoryId = elektronik.Id, Description = "Laptop ve masaüstü bilgisayarlar" },
            new Category { Name = "Tablet", Slug = "tablet", Icon = "bi-tablet", ParentCategoryId = elektronik.Id, Description = "Tabletler" },
            new Category { Name = "Kulaklık & Hoparlör", Slug = "kulaklik-hoparlor", Icon = "bi-headphones", ParentCategoryId = elektronik.Id, Description = "Ses cihazları" },
            new Category { Name = "Oyun Konsolu", Slug = "oyun-konsolu", Icon = "bi-controller", ParentCategoryId = elektronik.Id, Description = "PlayStation, Xbox, Nintendo" },
            
            // Giyim alt kategorileri
            new Category { Name = "Kadın Giyim", Slug = "kadin-giyim", Icon = "bi-gender-female", ParentCategoryId = giyim.Id, Description = "Kadın kıyafetleri" },
            new Category { Name = "Erkek Giyim", Slug = "erkek-giyim", Icon = "bi-gender-male", ParentCategoryId = giyim.Id, Description = "Erkek kıyafetleri" },
            new Category { Name = "Ayakkabı", Slug = "ayakkabi", Icon = "bi-boot", ParentCategoryId = giyim.Id, Description = "Her türlü ayakkabı" },
            new Category { Name = "Çanta", Slug = "canta", Icon = "bi-handbag", ParentCategoryId = giyim.Id, Description = "Çanta ve cüzdan" },
            
            // Ev & Yaşam alt kategorileri
            new Category { Name = "Mobilya", Slug = "mobilya", Icon = "bi-lamp", ParentCategoryId = evYasam.Id, Description = "Ev mobilyaları" },
            new Category { Name = "Dekorasyon", Slug = "dekorasyon", Icon = "bi-palette", ParentCategoryId = evYasam.Id, Description = "Ev dekorasyonu" },
            new Category { Name = "Mutfak Eşyaları", Slug = "mutfak-esyalari", Icon = "bi-cup-hot", ParentCategoryId = evYasam.Id, Description = "Mutfak gereçleri" },
            
            // Kitap & Hobi alt kategorileri
            new Category { Name = "Kitap", Slug = "kitap", Icon = "bi-book", ParentCategoryId = kitapHobi.Id, Description = "Her türlü kitap" },
            new Category { Name = "Müzik Aleti", Slug = "muzik-aleti", Icon = "bi-music-note", ParentCategoryId = kitapHobi.Id, Description = "Müzik aletleri" },
            new Category { Name = "Oyuncak", Slug = "oyuncak", Icon = "bi-puzzle", ParentCategoryId = kitapHobi.Id, Description = "Çocuk oyuncakları" },
            
            // Taşıt alt kategorileri
            new Category { Name = "Otomobil", Slug = "otomobil", Icon = "bi-car-front", ParentCategoryId = tasit.Id, Description = "İkinci el arabalar" },
            new Category { Name = "Motosiklet", Slug = "motosiklet", Icon = "bi-bicycle", ParentCategoryId = tasit.Id, Description = "Motosikletler" },
            new Category { Name = "Bisiklet", Slug = "bisiklet", Icon = "bi-bicycle", ParentCategoryId = tasit.Id, Description = "Bisikletler" },
            
            // Spor alt kategorileri
            new Category { Name = "Fitness", Slug = "fitness", Icon = "bi-heart-pulse", ParentCategoryId = spor.Id, Description = "Fitness ekipmanları" },
            new Category { Name = "Kamp & Outdoor", Slug = "kamp-outdoor", Icon = "bi-tree", ParentCategoryId = spor.Id, Description = "Kamp malzemeleri" },
            new Category { Name = "Spor Giyim", Slug = "spor-giyim", Icon = "bi-person-walking", ParentCategoryId = spor.Id, Description = "Spor kıyafetleri" }
        };

        await _mongoDb.Categories.InsertManyAsync(subCategories);
    }
}
