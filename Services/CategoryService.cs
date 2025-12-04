using Emusell.Models;

namespace Emusell.Services;

public class CategoryService
{
    private readonly List<Category> _categories = new()
    {
        new Category
        {
            Id = 1,
            Name = "Elektronik",
            Slug = "elektronik",
            Icon = "bi-phone",
            Description = "Telefon, bilgisayar, tablet ve daha fazlası",
            ProductCount = 145
        },
        new Category
        {
            Id = 2,
            Name = "Giyim & Moda",
            Slug = "giyim",
            Icon = "bi-bag",
            Description = "Kıyafet, ayakkabı, çanta ve aksesuarlar",
            ProductCount = 328
        },
        new Category
        {
            Id = 3,
            Name = "Ev & Yaşam",
            Slug = "ev-yasam",
            Icon = "bi-house",
            Description = "Mobilya, dekorasyon, ev eşyaları",
            ProductCount = 234
        },
        new Category
        {
            Id = 4,
            Name = "Kitap & Hobi",
            Slug = "kitap-hobi",
            Icon = "bi-book",
            Description = "Kitaplar, müzik aletleri, oyuncaklar",
            ProductCount = 189
        },
        new Category
        {
            Id = 5,
            Name = "Taşıt",
            Slug = "tasit",
            Icon = "bi-bicycle",
            Description = "Araba, bisiklet, motosiklet",
            ProductCount = 67
        },
        new Category
        {
            Id = 6,
            Name = "Spor & Outdoor",
            Slug = "spor",
            Icon = "bi-trophy",
            Description = "Spor ekipmanları, kamp malzemeleri",
            ProductCount = 98
        }
    };

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        await Task.Delay(200);
        return _categories;
    }

    public async Task<Category?> GetCategoryBySlugAsync(string slug)
    {
        await Task.Delay(200);
        return _categories.FirstOrDefault(c => c.Slug == slug);
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        await Task.Delay(200);
        return _categories.FirstOrDefault(c => c.Id == id);
    }
}
