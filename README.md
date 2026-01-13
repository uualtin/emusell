# Emusell - İkinci El E-Ticaret Platformu

## 📋 Proje Açıklaması

Emusell, kullanıcıların ikinci el ürünlerini güvenli ve kolay bir şekilde alıp satabileceği modern bir e-ticaret platformudur. 

### Proje Senaryosu
İkinci el ürünlerin alım satımını kolaylaştıran bu platform, üç ana kullanıcı rolü ile çalışır:
- **Admin**: Sistem yönetimi, kullanıcı yönetimi, kategori ve sipariş yönetimi
- **Satıcı (Seller)**: Ürün listeleme, ürün yönetimi, sipariş takibi
- **Alıcı (Buyer)**: Ürün arama, sepet yönetimi, sipariş verme, değerlendirme yapma

### Neden Bu Konu?
- İkinci el pazarlarının artan popülerliği
- Sürdürülebilir tüketim bilincinin artması
- Rol tabanlı yetkilendirme öğrenme deneyimi
- Gerçek dünya e-ticaret deneyimi

### Müşteri Gereksinimleri
- ✅ Güvenli giriş ve rol tabanlı erişim
- ✅ Satıcılar için ürün listeleme ve yönetimi
- ✅ Alıcılar için sepet ve ödeme işlemleri
- ✅ Sipariş ve ödeme takibi
- ✅ Admin yönetim paneli
- ✅ Ürün değerlendirme sistemi

---

## 🛠️ Teknoloji Stack

| Teknoloji | Versiyon |
|-----------|----------|
| .NET | 9.0 |
| Blazor Server | - |
| MongoDB Atlas | Cloud |
| MongoDB.Driver | 2.25.0 |
| Bootstrap | 5.x |
| Bootstrap Icons | 1.11.3 |
| BCrypt.Net | 4.0.3 |

---

## 📁 Proje Yapısı

```
emusell/
├── Models/
│   ├── User.cs           # Kullanıcı modeli (Admin, Seller, Buyer)
│   ├── Product.cs        # Ürün modeli
│   ├── Category.cs       # Kategori modeli
│   ├── Order.cs          # Sipariş modeli
│   ├── Cart.cs           # Sepet modeli
│   └── Review.cs         # Değerlendirme modeli
├── Services/
│   ├── MongoDbService.cs # MongoDB Atlas bağlantı servisi
│   ├── UserService.cs    # Kullanıcı işlemleri
│   ├── ProductService.cs # Ürün işlemleri
│   ├── CategoryService.cs# Kategori işlemleri
│   ├── OrderService.cs   # Sipariş işlemleri
│   ├── CartService.cs    # Sepet işlemleri
│   ├── ReviewService.cs  # Değerlendirme işlemleri
│   └── AuthService.cs    # Kimlik doğrulama
├── Pages/
│   ├── Auth/             # Giriş ve kayıt sayfaları
│   ├── Admin/            # Admin paneli sayfaları
│   ├── Seller/           # Satıcı paneli sayfaları
│   ├── Buyer/            # Alıcı sayfaları
│   ├── Products/         # Ürün sayfaları
│   └── Categories/       # Kategori sayfaları
├── Layout/
│   ├── MainLayout.razor
│   ├── NavMenu.razor
│   └── Footer.razor
└── wwwroot/
    └── css/app.css       # Stil dosyası
```

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler
- .NET 9.0 SDK
- MongoDB Atlas hesabı (ücretsiz)

### 1. Projeyi Klonlayın

```bash
git clone <repo-url>
cd emusell_repo/emusell
```

### 2. MongoDB Atlas Kurulumu

1. **MongoDB Atlas'a giriş yapın**: https://cloud.mongodb.com

2. **Cluster oluşturun** (veya mevcut cluster'ınızı kullanın)

3. **Database Access** bölümünden kullanıcı oluşturun:
   - Username ve Password belirleyin
   - Role: "Read and write to any database"

4. **Network Access** bölümünden IP adresinizi ekleyin:
   - "Add IP Address" → "Allow Access from Anywhere" (geliştirme için)
   - Veya kendi IP adresinizi ekleyin

5. **Connection String'i alın**:
   - Cluster'ınıza tıklayın → "Connect" → "Connect your application"
   - Connection string'i kopyalayın

### 3. Connection String'i Yapılandırın

`appsettings.json` dosyasını açın ve connection string'i güncelleyin:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://<username>:<password>@<cluster>.mongodb.net/?retryWrites=true&w=majority&appName=<appName>",
    "DatabaseName": "EmusellDb"
  }
}
```

**Örnek Connection String:**
```
mongodb+srv://emusell_user:MySecurePassword123@cluster0.abc123.mongodb.net/?retryWrites=true&w=majority&appName=Emusell
```

> ⚠️ **ÖNEMLİ**: Connection string'deki `<password>` kısmındaki özel karakterleri URL encode etmeyi unutmayın!

### 4. Bağımlılıkları Yükleyin

```bash
dotnet restore
```

### 5. Uygulamayı Çalıştırın

```bash
dotnet run
```

### 6. Tarayıcıda Açın

```
https://localhost:5001
```

veya

```
http://localhost:5000
```

---

## 🗄️ MongoDB Atlas Koleksiyonları

Uygulama ilk çalıştığında aşağıdaki koleksiyonlar otomatik oluşturulur:

| Koleksiyon | Açıklama |
|------------|----------|
| `users` | Kullanıcı bilgileri |
| `products` | Ürün bilgileri |
| `categories` | Kategori bilgileri |
| `orders` | Sipariş bilgileri |
| `carts` | Sepet bilgileri |
| `reviews` | Değerlendirmeler |

---

## 👤 Kullanıcı Ekranları

### 1. Giriş & Kayıt Ekranı
- Kullanıcı girişi (e-posta + şifre)
- Yeni kullanıcı kaydı (Alıcı / Satıcı seçimi)
- Şifre sıfırlama (profil sayfasından)

### 2. Ürün Yönetimi (Satıcı)
- ✅ Ürün Ekleme
- ✅ Ürün Düzenleme
- ✅ Ürün Silme
- ✅ Satıldı / Mevcut İşaretleme
- ✅ Aktif / Pasif Durumu

### 3. Kullanıcı Yönetimi (Admin)
- ✅ Kullanıcı Ekleme
- ✅ Kullanıcı Düzenleme
- ✅ Kullanıcı Silme
- ✅ Rol Atama (Admin, Satıcı, Alıcı)
- ✅ Hesap Askıya Alma / Aktifleştirme

### 4. Kategori Yönetimi
- ✅ Kategori Ekleme
- ✅ Kategori Düzenleme
- ✅ Kategori Silme
- ✅ Alt Kategori Desteği

### 5. Sipariş Yönetimi
- ✅ Siparişleri Görüntüleme
- ✅ Sipariş Durumu Güncelleme
- ✅ Sipariş Detayları
- ✅ İptal / İade İşlemleri

### 6. Sepet & Ödeme (Alıcı)
- ✅ Sepete Ekleme / Çıkarma
- ✅ Miktar Güncelleme
- ✅ Ödeme Süreci
- ✅ Sipariş Onayı

### 7. Dashboard Ekranları
- ✅ Admin Dashboard
- ✅ Satıcı Dashboard
- ✅ Alıcı Dashboard

### 8. Değerlendirme & Puanlama
- ✅ Değerlendirme Ekleme
- ✅ Değerlendirme Düzenleme / Silme
- ✅ Ürün Değerlendirmelerini Görüntüleme

---

## 📊 Veritabanı Şeması

### User Collection
```json
{
  "_id": "ObjectId",
  "email": "string",
  "passwordHash": "string",
  "fullName": "string",
  "phone": "string",
  "address": "string",
  "role": "Admin | Seller | Buyer",
  "isActive": "boolean",
  "createdAt": "DateTime"
}
```

### Product Collection
```json
{
  "_id": "ObjectId",
  "title": "string",
  "description": "string",
  "price": "decimal",
  "categoryId": "ObjectId",
  "sellerId": "ObjectId",
  "condition": "Yeni | SifirGibi | Iyi | Orta | Kullanilmis",
  "isSold": "boolean",
  "isActive": "boolean",
  "viewCount": "int"
}
```

### Order Collection
```json
{
  "_id": "ObjectId",
  "orderNumber": "string",
  "buyerId": "ObjectId",
  "items": "[OrderItem]",
  "totalAmount": "decimal",
  "status": "Pending | Confirmed | Shipped | Delivered | Cancelled | Refunded",
  "paymentStatus": "Pending | Completed | Failed | Refunded"
}
```

---

## 🔐 Güvenlik Özellikleri

- BCrypt ile şifre hashleme
- Rol tabanlı erişim kontrolü
- MongoDB Atlas SSL/TLS şifreleme
- Network Access whitelist

---

## 🐛 Sorun Giderme

### Bağlantı Hatası
```
MongoAuthenticationException: Unable to authenticate
```
**Çözüm**: 
- Username ve password'u kontrol edin
- Password'daki özel karakterleri URL encode edin
- Database Access'te kullanıcı rollerini kontrol edin

### Network Hatası
```
MongoTimeoutException: A timeout occurred
```
**Çözüm**:
- Network Access'te IP adresinizi ekleyin
- Firewall ayarlarınızı kontrol edin

### Koleksiyon Oluşturulmuyor
**Çözüm**:
- Connection string'deki database adını kontrol edin
- Atlas'ta cluster'ın aktif olduğundan emin olun

---

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.

**ITEC420 & CMPR208 Dönem Projesi**

---

## 🤝 Katkıda Bulunma

Katkıda bulunmak için `CONTRIBUTING.md` dosyasına bakın.
