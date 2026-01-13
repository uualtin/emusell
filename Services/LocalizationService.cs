using Microsoft.JSInterop;
using System.Text.Json;

namespace Emusell.Services;

public class LocalizationService
{
    private readonly IWebHostEnvironment _env;
    private IJSRuntime? _jsRuntime;
    private Dictionary<string, string> _translations = new();
    private string _currentLanguage = "tr";

    public string CurrentLanguage => _currentLanguage;
    public event Action? OnLanguageChanged;

    public LocalizationService(IWebHostEnvironment env)
    {
        _env = env;
        LoadTranslations("tr");
    }

    public async Task InitializeAsync(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        try
        {
            var savedLang = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "language");
            if (!string.IsNullOrEmpty(savedLang) && (savedLang == "tr" || savedLang == "en"))
            {
                _currentLanguage = savedLang;
                LoadTranslations(_currentLanguage);
            }
        }
        catch
        {
            // Default to Turkish
        }
    }

    public string this[string key]
    {
        get
        {
            if (_translations.TryGetValue(key, out var value))
                return value;
            return key; // Return key if translation not found
        }
    }

    public async Task SetLanguageAsync(string language)
    {
        if (language != "tr" && language != "en")
            return;

        _currentLanguage = language;
        LoadTranslations(language);

        if (_jsRuntime != null)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "language", language);
        }

        OnLanguageChanged?.Invoke();
    }

    private void LoadTranslations(string language)
    {
        try
        {
            var path = Path.Combine(_env.WebRootPath, "locales", $"{language}.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
            }
            else
            {
                // Load default translations if file doesn't exist
                _translations = GetDefaultTranslations(language);
            }
        }
        catch
        {
            _translations = GetDefaultTranslations(language);
        }
    }

    private Dictionary<string, string> GetDefaultTranslations(string language)
    {
        if (language == "en")
        {
            return new Dictionary<string, string>
            {
                // Navigation
                ["Nav.Home"] = "Home",
                ["Nav.Products"] = "Products",
                ["Nav.Categories"] = "Categories",
                ["Nav.About"] = "About",
                ["Nav.Cart"] = "Cart",
                ["Nav.Login"] = "Login",
                ["Nav.Register"] = "Register",
                ["Nav.Logout"] = "Logout",
                ["Nav.Profile"] = "Profile",
                ["Nav.MyAccount"] = "My Account",
                ["Nav.MyOrders"] = "My Orders",
                ["Nav.MyReviews"] = "My Reviews",
                ["Nav.AdminPanel"] = "Admin Panel",
                ["Nav.UserManagement"] = "User Management",
                ["Nav.CategoryManagement"] = "Category Management",
                ["Nav.OrderManagement"] = "Order Management",
                ["Nav.SellerPanel"] = "Seller Panel",
                ["Nav.MyProducts"] = "My Products",
                ["Nav.AddProduct"] = "Add Product",
                ["Nav.Language"] = "Language",
                ["Nav.ToggleTheme"] = "Toggle Theme",

                // Auth
                ["Auth.Email"] = "Email",
                ["Auth.Password"] = "Password",
                ["Auth.ConfirmPassword"] = "Confirm Password",
                ["Auth.FullName"] = "Full Name",
                ["Auth.Phone"] = "Phone",
                ["Auth.LoginTitle"] = "Login",
                ["Auth.RegisterTitle"] = "Register",
                ["Auth.LoginButton"] = "Login",
                ["Auth.RegisterButton"] = "Register",
                ["Auth.NoAccount"] = "Don't have an account?",
                ["Auth.HasAccount"] = "Already have an account?",
                ["Auth.RegisterAsSeller"] = "Register as Seller",
                ["Auth.RegisterAsBuyer"] = "Register as Buyer",

                // Home
                ["Home.HeroTitle"] = "Buy and Sell Second-Hand Products Easily",
                ["Home.HeroSubtitle"] = "A reliable and secure platform for your second-hand shopping needs",
                ["Home.BrowseProducts"] = "Browse Products",
                ["Home.StartSelling"] = "Start Selling",
                ["Home.FeaturedProducts"] = "Featured Products",
                ["Home.PopularCategories"] = "Popular Categories",
                ["Home.HowItWorks"] = "How It Works",
                ["Home.ViewAll"] = "View All",

                // Products
                ["Products.Title"] = "Products",
                ["Products.Search"] = "Search products...",
                ["Products.Filter"] = "Filter",
                ["Products.Sort"] = "Sort",
                ["Products.AddToCart"] = "Add to Cart",
                ["Products.ViewDetails"] = "View Details",
                ["Products.Price"] = "Price",
                ["Products.Condition"] = "Condition",
                ["Products.Location"] = "Location",
                ["Products.Seller"] = "Seller",

                // Cart
                ["Cart.Title"] = "Shopping Cart",
                ["Cart.Empty"] = "Your cart is empty",
                ["Cart.Total"] = "Total",
                ["Cart.Checkout"] = "Checkout",
                ["Cart.Remove"] = "Remove",
                ["Cart.Quantity"] = "Quantity",

                // Common
                ["Common.Save"] = "Save",
                ["Common.Cancel"] = "Cancel",
                ["Common.Delete"] = "Delete",
                ["Common.Edit"] = "Edit",
                ["Common.Add"] = "Add",
                ["Common.Search"] = "Search",
                ["Common.Loading"] = "Loading...",
                ["Common.NoResults"] = "No results found",
                ["Common.Success"] = "Success",
                ["Common.Error"] = "Error",
                ["Common.Confirm"] = "Confirm",

                // Footer
                ["Footer.QuickLinks"] = "Quick Links",
                ["Footer.CustomerService"] = "Customer Service",
                ["Footer.FollowUs"] = "Follow Us",
                ["Footer.Rights"] = "All rights reserved.",
                ["Footer.FAQ"] = "FAQ",
                ["Footer.Contact"] = "Contact",
                ["Footer.Terms"] = "Terms of Service",
                ["Footer.Privacy"] = "Privacy Policy"
            };
        }

        // Turkish (default)
        return new Dictionary<string, string>
        {
            // Navigation
            ["Nav.Home"] = "Ana Sayfa",
            ["Nav.Products"] = "Ürünler",
            ["Nav.Categories"] = "Kategoriler",
            ["Nav.About"] = "Hakkımızda",
            ["Nav.Cart"] = "Sepet",
            ["Nav.Login"] = "Giriş",
            ["Nav.Register"] = "Kayıt Ol",
            ["Nav.Logout"] = "Çıkış Yap",
            ["Nav.Profile"] = "Profil",
            ["Nav.MyAccount"] = "Hesabım",
            ["Nav.MyOrders"] = "Siparişlerim",
            ["Nav.MyReviews"] = "Değerlendirmelerim",
            ["Nav.AdminPanel"] = "Admin Panel",
            ["Nav.UserManagement"] = "Kullanıcı Yönetimi",
            ["Nav.CategoryManagement"] = "Kategori Yönetimi",
            ["Nav.OrderManagement"] = "Sipariş Yönetimi",
            ["Nav.SellerPanel"] = "Satıcı Paneli",
            ["Nav.MyProducts"] = "Ürünlerim",
            ["Nav.AddProduct"] = "Ürün Ekle",
            ["Nav.Language"] = "Dil",
            ["Nav.ToggleTheme"] = "Tema Değiştir",

            // Auth
            ["Auth.Email"] = "E-posta",
            ["Auth.Password"] = "Şifre",
            ["Auth.ConfirmPassword"] = "Şifre Tekrar",
            ["Auth.FullName"] = "Ad Soyad",
            ["Auth.Phone"] = "Telefon",
            ["Auth.LoginTitle"] = "Giriş Yap",
            ["Auth.RegisterTitle"] = "Kayıt Ol",
            ["Auth.LoginButton"] = "Giriş Yap",
            ["Auth.RegisterButton"] = "Kayıt Ol",
            ["Auth.NoAccount"] = "Hesabınız yok mu?",
            ["Auth.HasAccount"] = "Zaten hesabınız var mı?",
            ["Auth.RegisterAsSeller"] = "Satıcı Olarak Kayıt Ol",
            ["Auth.RegisterAsBuyer"] = "Alıcı Olarak Kayıt Ol",

            // Home
            ["Home.HeroTitle"] = "İkinci El Ürünleri Kolayca Al ve Sat",
            ["Home.HeroSubtitle"] = "İkinci el alışverişiniz için güvenilir ve güvenli bir platform",
            ["Home.BrowseProducts"] = "Ürünleri İncele",
            ["Home.StartSelling"] = "Satışa Başla",
            ["Home.FeaturedProducts"] = "Öne Çıkan Ürünler",
            ["Home.PopularCategories"] = "Popüler Kategoriler",
            ["Home.HowItWorks"] = "Nasıl Çalışır",
            ["Home.ViewAll"] = "Tümünü Gör",

            // Products
            ["Products.Title"] = "Ürünler",
            ["Products.Search"] = "Ürün ara...",
            ["Products.Filter"] = "Filtrele",
            ["Products.Sort"] = "Sırala",
            ["Products.AddToCart"] = "Sepete Ekle",
            ["Products.ViewDetails"] = "Detayları Gör",
            ["Products.Price"] = "Fiyat",
            ["Products.Condition"] = "Durum",
            ["Products.Location"] = "Konum",
            ["Products.Seller"] = "Satıcı",

            // Cart
            ["Cart.Title"] = "Alışveriş Sepeti",
            ["Cart.Empty"] = "Sepetiniz boş",
            ["Cart.Total"] = "Toplam",
            ["Cart.Checkout"] = "Ödemeye Geç",
            ["Cart.Remove"] = "Kaldır",
            ["Cart.Quantity"] = "Adet",

            // Common
            ["Common.Save"] = "Kaydet",
            ["Common.Cancel"] = "İptal",
            ["Common.Delete"] = "Sil",
            ["Common.Edit"] = "Düzenle",
            ["Common.Add"] = "Ekle",
            ["Common.Search"] = "Ara",
            ["Common.Loading"] = "Yükleniyor...",
            ["Common.NoResults"] = "Sonuç bulunamadı",
            ["Common.Success"] = "Başarılı",
            ["Common.Error"] = "Hata",
            ["Common.Confirm"] = "Onayla",

            // Footer
            ["Footer.QuickLinks"] = "Hızlı Linkler",
            ["Footer.CustomerService"] = "Müşteri Hizmetleri",
            ["Footer.FollowUs"] = "Bizi Takip Edin",
            ["Footer.Rights"] = "Tüm hakları saklıdır.",
            ["Footer.FAQ"] = "SSS",
            ["Footer.Contact"] = "İletişim",
            ["Footer.Terms"] = "Kullanım Koşulları",
            ["Footer.Privacy"] = "Gizlilik Politikası"
        };
    }
}
