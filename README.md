# 🛒 Emusell - Blazor E-ticaret Platformu

Modern, hızlı ve kullanıcı dostu e-ticaret platformu. Blazor WebAssembly ile geliştirilmiştir.

## 🚀 Özellikler

- 🛍️ Ürün kataloğu ve arama
- 🛒 Alışveriş sepeti
- 👤 Kullanıcı yönetimi
- 💳 Güvenli ödeme sistemi
- 📱 Responsive tasarım
- 🔍 Gelişmiş arama ve filtreleme

## 🛠️ Teknolojiler

- **Frontend:** Blazor WebAssembly
- **Backend:** ASP.NET Core Web API
- **Database:** SQL Server / SQLite
- **Authentication:** ASP.NET Core Identity
- **UI Framework:** Bootstrap 5
- **Icons:** Font Awesome

## 📋 Gereksinimler

- .NET 9.0 SDK
- Visual Studio 2022 veya VS Code
- SQL Server (opsiyonel, SQLite da kullanılabilir)

## 🚀 Kurulum

### 1. Repository'yi klonlayın
```bash
git clone https://github.com/yourusername/emusell-blazor.git
cd emusell-blazor
```

### 2. Bağımlılıkları yükleyin
```bash
dotnet restore
```

### 3. Veritabanını yapılandırın
```bash
# SQLite için (varsayılan)
dotnet ef database update

# SQL Server için
# appsettings.json'da connection string'i güncelleyin
dotnet ef database update
```

### 4. Projeyi çalıştırın
```bash
dotnet run
```

Tarayıcınızda `https://localhost:5001` adresine gidin.

## 🏗️ Proje Yapısı

```
Emusell/
├── Components/          # Blazor bileşenleri
│   ├── Layout/         # Layout bileşenleri
│   ├── Pages/          # Sayfa bileşenleri
│   └── Shared/         # Paylaşılan bileşenler
├── Services/           # Business logic servisleri
├── Models/             # Veri modelleri
├── Data/               # Veritabanı context
├── wwwroot/            # Statik dosyalar
└── Program.cs          # Uygulama giriş noktası
```

## 🤝 Katkıda Bulunma

Bu projeye katkıda bulunmak için [CONTRIBUTING.md](CONTRIBUTING.md) dosyasını okuyun.

### Geliştirme Süreci

1. Issue oluşturun veya mevcut issue'yu alın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

### Commit Mesajları

Conventional Commits formatını kullanın:

```
feat: yeni özellik eklendi
fix: hata düzeltildi
docs: dokümantasyon güncellendi
style: kod formatı düzeltildi
refactor: kod yeniden düzenlendi
test: test eklendi
```

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

## 👥 Ekip

- **Senior Developer:** [Your Name]
- **Contributors:** [Team Members]

## 📞 İletişim

- **GitHub Issues:** [Issues](https://github.com/yourusername/emusell-blazor/issues)
- **Discussions:** [Discussions](https://github.com/yourusername/emusell-blazor/discussions)

## 🎯 Roadmap

- [ ] Kullanıcı yönetimi
- [ ] Ürün kataloğu
- [ ] Alışveriş sepeti
- [ ] Ödeme sistemi
- [ ] Admin paneli
- [ ] Mobil uygulama

## 📊 Proje Durumu

![Build Status](https://github.com/yourusername/emusell-blazor/workflows/CI/badge.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-purple.svg)
