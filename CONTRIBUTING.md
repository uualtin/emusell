# 🤝 Katkıda Bulunma Rehberi

Bu projeye katkıda bulunmak için lütfen bu rehberi takip edin.

## 📋 Genel Kurallar

### 🎯 Commit Mesajları
Conventional Commits formatını kullanın:

```
<type>(<scope>): <description>

[optional body]
[optional footer(s)]
```

**Örnekler:**
```
feat(auth): kullanıcı girişi eklendi
fix(cart): sepet tutarı hesaplama hatası düzeltildi
docs(readme): kurulum talimatları güncellendi
style(ui): buton renkleri değiştirildi
refactor(api): user controller yeniden düzenlendi
test(auth): login testleri eklendi
```

### 🌿 Branch Stratejisi

**Ana Branch'ler:**
- `main` - Production-ready kod
- `develop` - Integration branch

**Feature Branch'ler:**
```bash
feature/user-authentication
feature/product-catalog
feature/shopping-cart
feature/payment-system
```

**Bug Fix Branch'ler:**
```bash
bugfix/cart-calculation-error
bugfix/login-validation-issue
```

**Hotfix Branch'ler:**
```bash
hotfix/critical-security-patch
hotfix/urgent-payment-bug
```

## 🔄 Pull Request Süreci

### 1. Branch Oluşturma
```bash
# Ana branch'ten yeni branch oluştur
git checkout develop
git pull origin develop
git checkout -b feature/your-feature-name
```

### 2. Geliştirme
- Kod yazarken sık sık commit yapın
- Her commit anlamlı olmalı
- Test yazmayı unutmayın

### 3. Pull Request Oluşturma
1. GitHub'da "Compare & pull request" butonuna tıklayın
2. PR template'ini doldurun
3. Senior'ı assign edin
4. Uygun label'ları ekleyin

### 4. Review Süreci
- Senior review bekleyin
- Gerekli düzeltmeleri yapın
- Onay sonrası merge edin

## 📝 Code Review Kuralları

### ✅ Yapılması Gerekenler
- [ ] Kod anlaşılır ve temiz
- [ ] Değişken/fonksiyon isimleri açıklayıcı
- [ ] Gereksiz kod yok
- [ ] Comment'ler uygun
- [ ] SOLID prensiplere uygun
- [ ] Test coverage yeterli
- [ ] Security açığı yok

### ❌ Yapılmaması Gerekenler
- [ ] Hard-coded değerler
- [ ] Gereksiz complexity
- [ ] Comment'lenmemiş kod
- [ ] Test yazılmamış kod
- [ ] Security açığı olan kod

## 🧪 Test Kuralları

### Unit Testler
- Her public method için test yazın
- Edge case'leri test edin
- Mock'ları doğru kullanın

### Integration Testler
- API endpoint'leri test edin
- Database işlemlerini test edin
- Authentication/Authorization test edin

## 📚 Dokümantasyon

### Kod Dokümantasyonu
- Public method'ları XML comment ile dokümante edin
- Complex algoritmaları açıklayın
- API endpoint'leri Swagger ile dokümante edin

### README Güncellemeleri
- Yeni özellikler için README güncelleyin
- Kurulum talimatlarını güncelleyin
- API dokümantasyonunu güncelleyin

## 🚀 Deployment

### Pre-deployment Checklist
- [ ] Tüm testler geçti
- [ ] Code review tamamlandı
- [ ] Security scan temiz
- [ ] Performance test edildi
- [ ] Database migration'lar hazır

## 🆘 Yardım

### Sorun Çözme
1. GitHub Issues'da arama yapın
2. Dokümantasyonu kontrol edin
3. Senior'a danışın

### İletişim
- **GitHub Issues:** Bug tracking ve feature requests
- **GitHub Discussions:** Genel tartışmalar
- **Pull Request Comments:** Kod review süreci

## 📋 Senior Review Checklist

### 🔍 Code Review Checklist

#### 📖 Kod Okunabilirliği
- [ ] Kod anlaşılır ve temiz
- [ ] Değişken/fonksiyon isimleri açıklayıcı
- [ ] Gereksiz kod yok
- [ ] Comment'ler uygun

#### 🏗️ Mimari
- [ ] SOLID prensiplere uygun
- [ ] Design pattern'ler doğru kullanılmış
- [ ] Separation of concerns sağlanmış

#### 🔒 Güvenlik
- [ ] SQL injection riski yok
- [ ] XSS koruması var
- [ ] Authentication/Authorization doğru
- [ ] Sensitive data korunmuş

#### ⚡ Performance
- [ ] N+1 query problemi yok
- [ ] Memory leak riski yok
- [ ] Async/await doğru kullanılmış

#### 🧪 Test
- [ ] Unit testler yazılmış
- [ ] Edge case'ler test edilmiş
- [ ] Test coverage yeterli

## 🎯 Label'lar

### Issue Labels
- `bug` - Hata
- `enhancement` - Yeni özellik
- `documentation` - Dokümantasyon
- `good first issue` - Yeni başlayanlar için
- `help wanted` - Yardım gerekli

### Priority Labels
- `critical` - Kritik
- `high` - Yüksek
- `medium` - Orta
- `low` - Düşük

### Type Labels
- `frontend` - Frontend
- `backend` - Backend
- `database` - Veritabanı
- `ui/ux` - Kullanıcı arayüzü
