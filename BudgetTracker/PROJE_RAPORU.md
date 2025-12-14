# Budget Tracker Projesi - Teknik Rapor

## 📋 İçindekiler
1. [Proje Genel Bakış](#proje-genel-bakış)
2. [Teknoloji Stack](#teknoloji-stack)
3. [Mimari Yapı](#mimari-yapı)
4. [Veritabanı Tasarımı](#veritabanı-tasarımı)
5. [Katmanlar ve Sorumluluklar](#katmanlar-ve-sorumluluklar)
6. [Güvenlik](#güvenlik)
7. [Özellikler](#özellikler)
8. [Kullanıcı Akışı](#kullanıcı-akışı)
9. [SOLID Prensipleri](#solid-prensipleri)
10. [Bağımlılıklar](#bağımlılıklar)

---

## 🎯 Proje Genel Bakış

**Budget Tracker**, kullanıcıların kişisel bütçelerini yönetmelerine olanak sağlayan bir ASP.NET Core MVC web uygulamasıdır. Kullanıcılar harcamalarını kategorilere göre sınıflandırabilir, geçmiş harcamalarını görüntüleyebilir ve istatistiksel analizler yapabilir.

### Temel Özellikler
- ✅ Kullanıcı kaydı ve giriş sistemi
- ✅ Kategori yönetimi (oluşturma, düzenleme, silme)
- ✅ Harcama takibi (ekleme, düzenleme, silme)
- ✅ Dashboard ile istatistiksel görünümler
- ✅ Tarih aralığına göre filtreleme
- ✅ Kategori bazında harcama analizi

---

## 🛠 Teknoloji Stack

### Backend
- **.NET 9.0** - Framework
- **ASP.NET Core MVC** - Web framework
- **Entity Framework Core 9.0** - ORM
- **SQLite** - Veritabanı

### Frontend
- **Bootstrap 5** - CSS framework
- **jQuery** - JavaScript kütüphanesi
- **jQuery Validation** - Form doğrulama

### Güvenlik
- **Cookie Authentication** - Kimlik doğrulama
- **SHA-256** - Şifre hashleme
- **Anti-Forgery Token** - CSRF koruması

---

## 🏗 Mimari Yapı

Proje **N-Tier (Çok Katmanlı) Mimari** desenini kullanmaktadır:

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│  (Controllers + Views)              │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         Business Layer              │
│  (Services - IUserService,          │
│   ICategoryService, IExpenseService)│
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         Data Access Layer           │
│  (Repository Pattern +              │
│   ApplicationDbContext)             │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         Database Layer              │
│  (SQLite Database)                  │
└─────────────────────────────────────┘
```

### Tasarım Desenleri

1. **Repository Pattern**
   - Generic `IRepository<T>` interface'i
   - Tüm entity'ler için ortak CRUD operasyonları
   - Veri erişim mantığının merkezileştirilmesi

2. **Service Layer Pattern**
   - Business logic'in controller'lardan ayrılması
   - Her domain için özel service interface'leri
   - Dependency Injection ile gevşek bağlılık

3. **Dependency Injection**
   - Constructor injection kullanımı
   - Interface'lere bağımlılık (Dependency Inversion)
   - `Program.cs` içinde servis kayıtları

---

## 💾 Veritabanı Tasarımı

### Entity İlişkileri

```
User (1) ────< (N) Expense
  │
  │ (1)
  │
  └───< (N) Category (1) ────< (N) Expense
```

### Tablolar

#### 1. Users
| Sütun | Tip | Açıklama |
|-------|-----|----------|
| Id | INT (PK) | Primary key |
| Username | VARCHAR(100) | Unique kullanıcı adı |
| PasswordHash | VARCHAR(255) | SHA-256 hash'lenmiş şifre |
| CreatedAt | DATETIME | Kayıt tarihi |

**İlişkiler:**
- `Users` → `Expenses` (1:N, Cascade Delete)
- `Users` → `Categories` (1:N, Cascade Delete)

#### 2. Categories
| Sütun | Tip | Açıklama |
|-------|-----|----------|
| Id | INT (PK) | Primary key |
| Name | VARCHAR(100) | Kategori adı |
| Description | VARCHAR(500) | Açıklama (nullable) |
| Color | VARCHAR(7) | Hex renk kodu (default: #007bff) |
| UserId | INT (FK) | Kullanıcı referansı |
| CreatedAt | DATETIME | Oluşturulma tarihi |

**İlişkiler:**
- `Categories` → `Users` (N:1)
- `Categories` → `Expenses` (1:N, Restrict Delete)

#### 3. Expenses
| Sütun | Tip | Açıklama |
|-------|-----|----------|
| Id | INT (PK) | Primary key |
| Amount | DECIMAL(18,2) | Harcama tutarı |
| Description | VARCHAR(500) | Açıklama |
| ExpenseDate | DATETIME | Harcama tarihi |
| UserId | INT (FK) | Kullanıcı referansı |
| CategoryId | INT (FK) | Kategori referansı |
| CreatedAt | DATETIME | Kayıt tarihi |

**İlişkiler:**
- `Expenses` → `Users` (N:1)
- `Expenses` → `Categories` (N:1)

**Indexler:**
- `ExpenseDate` (tekli index)
- `(UserId, ExpenseDate)` (bileşik index - performans için)

### Veritabanı Konfigürasyonu

- **Veritabanı:** SQLite
- **Dosya Konumu:** `BudgetTracker.db` (proje kök dizini)
- **Migration Stratejisi:** `EnsureCreated()` - Development için
- **Connection String:** `appsettings.json` içinde tanımlı

---

## 📂 Katmanlar ve Sorumluluklar

### 1. Presentation Layer (Controllers)

#### AccountController
- **Sorumluluk:** Kullanıcı kimlik doğrulama işlemleri
- **Metodlar:**
  - `Login()` - Giriş sayfası ve işlemi
  - `Register()` - Kayıt sayfası ve işlemi
  - `Logout()` - Çıkış işlemi
  - `AccessDenied()` - Yetkisiz erişim sayfası

#### CategoryController
- **Sorumluluk:** Kategori CRUD işlemleri
- **Özellikler:**
  - `[Authorize]` attribute ile korumalı
  - Kullanıcı bazlı veri izolasyonu
- **Metodlar:**
  - `Index()` - Kategori listesi
  - `Create()` - Yeni kategori oluşturma
  - `Edit()` - Kategori düzenleme
  - `Delete()` - Kategori silme

#### ExpenseController
- **Sorumluluk:** Harcama CRUD işlemleri
- **Özellikler:**
  - `[Authorize]` attribute ile korumalı
  - Kategori doğrulaması
- **Metodlar:**
  - `Index()` - Harcama listesi (tarihe göre sıralı)
  - `Create()` - Yeni harcama ekleme
  - `Edit()` - Harcama düzenleme
  - `Delete()` - Harcama silme

#### DashboardController
- **Sorumluluk:** İstatistiksel görünümler ve raporlama
- **Özellikler:**
  - Haftalık, aylık, 6 aylık toplamlar
  - Kategori bazında analiz
  - JSON API endpoint'i (`GetChartData`)
- **Metodlar:**
  - `Index()` - Dashboard ana sayfa
  - `GetChartData()` - Grafik verisi API

#### HomeController
- **Sorumluluk:** Ana sayfa yönlendirmeleri
- **Metodlar:**
  - `Index()` - Giriş yapmış kullanıcıları Dashboard'a yönlendirir

### 2. Business Layer (Services)

#### UserService
- **Sorumluluk:** Kullanıcı iş mantığı
- **Metodlar:**
  - `RegisterAsync()` - Yeni kullanıcı kaydı
  - `LoginAsync()` - Kullanıcı girişi ve şifre doğrulama
  - `GetUserByIdAsync()` - ID'ye göre kullanıcı getirme
  - `GetUserByUsernameAsync()` - Kullanıcı adına göre getirme
  - `UsernameExistsAsync()` - Kullanıcı adı kontrolü
- **Güvenlik:**
  - SHA-256 ile şifre hashleme
  - Şifre doğrulama

#### CategoryService
- **Sorumluluk:** Kategori iş mantığı
- **Özellikler:**
  - Kullanıcı bazlı veri izolasyonu
  - Kategori sahiplik kontrolü
- **Metodlar:**
  - `GetUserCategoriesAsync()` - Kullanıcının kategorileri
  - `GetCategoryByIdAsync()` - ID ve kullanıcı kontrolü ile getirme
  - `CreateCategoryAsync()` - Yeni kategori oluşturma
  - `UpdateCategoryAsync()` - Kategori güncelleme
  - `DeleteCategoryAsync()` - Kategori silme
  - `CategoryBelongsToUserAsync()` - Sahiplik kontrolü

#### ExpenseService
- **Sorumluluk:** Harcama iş mantığı
- **Özellikler:**
  - Kullanıcı bazlı veri izolasyonu
  - Kategori sahiplik kontrolü
  - Tarih aralığı filtreleme
  - İstatistiksel hesaplamalar
- **Metodlar:**
  - `GetUserExpensesAsync()` - Kullanıcının tüm harcamaları
  - `GetUserExpensesByDateRangeAsync()` - Tarih aralığına göre harcamalar
  - `GetExpenseByIdAsync()` - ID ve kullanıcı kontrolü ile getirme
  - `CreateExpenseAsync()` - Yeni harcama ekleme
  - `UpdateExpenseAsync()` - Harcama güncelleme
  - `DeleteExpenseAsync()` - Harcama silme
  - `GetTotalExpensesByDateRangeAsync()` - Toplam harcama hesaplama
  - `GetExpensesByCategoryAsync()` - Kategori bazında gruplama

### 3. Data Access Layer

#### Repository Pattern
- **IRepository<T>:** Generic repository interface
- **Repository<T>:** Generic repository implementasyonu
- **Özellikler:**
  - Async/await desteği
  - LINQ expression desteği
  - CRUD operasyonları
  - Count ve Exists metodları

#### ApplicationDbContext
- **Sorumluluk:** Entity Framework Core DbContext
- **Özellikler:**
  - Fluent API ile entity konfigürasyonları
  - İlişki tanımlamaları
  - Index tanımlamaları
  - Constraint'ler (unique, required, max length)

### 4. Models (Domain Layer)

#### User
- **Encapsulation:** Private set'ler ile korumalı property'ler
- **Validation:** Constructor içinde validasyon
- **Metodlar:**
  - `UpdatePassword()` - Şifre güncelleme

#### Category
- **Encapsulation:** Private set'ler ile korumalı property'ler
- **Validation:** Constructor içinde validasyon
- **Metodlar:**
  - `Update()` - Kategori bilgilerini güncelleme

#### Expense
- **Encapsulation:** Private set'ler ile korumalı property'ler
- **Validation:** Constructor içinde validasyon (amount > 0, vb.)
- **Metodlar:**
  - `Update()` - Harcama bilgilerini güncelleme

---

## 🔒 Güvenlik

### Kimlik Doğrulama (Authentication)
- **Yöntem:** Cookie-based Authentication
- **Süre:** 7 gün (sliding expiration)
- **Claims:** UserId ve Username

### Yetkilendirme (Authorization)
- **Attribute:** `[Authorize]` - Controller ve action seviyesinde
- **Korumalı Controller'lar:**
  - CategoryController
  - ExpenseController
  - DashboardController

### Şifre Güvenliği
- **Hash Algoritması:** SHA-256
- **Saklama:** Base64 encoded hash
- **Doğrulama:** Hash karşılaştırması

### Veri İzolasyonu
- Tüm veri işlemleri kullanıcı bazlı filtrelenir
- Kullanıcılar sadece kendi verilerine erişebilir
- Service katmanında sahiplik kontrolü

### CSRF Koruması
- `[ValidateAntiForgeryToken]` attribute'u
- POST işlemlerinde token kontrolü

### Veri Doğrulama
- Model seviyesinde validasyon
- Service katmanında business rule kontrolü
- Controller seviyesinde input validasyonu

---

## ✨ Özellikler

### 1. Kullanıcı Yönetimi
- ✅ Kullanıcı kaydı (username uniqueness kontrolü)
- ✅ Güvenli giriş sistemi
- ✅ Otomatik giriş (kayıt sonrası)
- ✅ Çıkış işlemi

### 2. Kategori Yönetimi
- ✅ Kategori oluşturma (isim, açıklama, renk)
- ✅ Kategori düzenleme
- ✅ Kategori silme
- ✅ Kullanıcı bazlı kategori listesi

### 3. Harcama Yönetimi
- ✅ Harcama ekleme (tutar, açıklama, tarih, kategori)
- ✅ Harcama düzenleme
- ✅ Harcama silme
- ✅ Tarih bazlı sıralama
- ✅ Geçmiş ve gelecek tarihli harcamalar

### 4. Dashboard ve Raporlama
- ✅ Haftalık toplam harcama
- ✅ Aylık toplam harcama
- ✅ 6 aylık toplam harcama
- ✅ Kategori bazında harcama dağılımı
- ✅ JSON API endpoint (grafik verisi için)

---

## 🔄 Kullanıcı Akışı

### 1. İlk Kullanım
```
Ana Sayfa → Login Sayfası → Register → Otomatik Giriş → Dashboard
```

### 2. Giriş Yapmış Kullanıcı
```
Dashboard → Kategoriler → Harcamalar → Dashboard (İstatistikler)
```

### 3. Kategori İşlemleri
```
Kategori Listesi → Yeni Kategori → Form Doldur → Kaydet → Liste
Kategori Listesi → Düzenle → Form Güncelle → Kaydet → Liste
Kategori Listesi → Sil → Onay → Liste
```

### 4. Harcama İşlemleri
```
Harcama Listesi → Yeni Harcama → Form Doldur → Kaydet → Liste
Harcama Listesi → Düzenle → Form Güncelle → Kaydet → Liste
Harcama Listesi → Sil → Onay → Liste
```

---

## 🎯 SOLID Prensipleri

### 1. Single Responsibility Principle (SRP)
- ✅ **Controller'lar:** Sadece HTTP isteklerini yönetir
- ✅ **Service'ler:** Sadece business logic içerir
- ✅ **Repository:** Sadece veri erişim işlemlerinden sorumlu
- ✅ **Models:** Sadece veri yapısı ve domain logic

### 2. Open/Closed Principle (OCP)
- ✅ Generic `IRepository<T>` ile yeni entity'ler eklenebilir
- ✅ Service interface'leri ile genişletilebilir yapı

### 3. Liskov Substitution Principle (LSP)
- ✅ Interface implementasyonları birbirinin yerine kullanılabilir
- ✅ Repository pattern ile farklı implementasyonlar mümkün

### 4. Interface Segregation Principle (ISP)
- ✅ Her service için ayrı interface (`IUserService`, `ICategoryService`, `IExpenseService`)
- ✅ Generic repository interface'i sadece gerekli metodları içerir

### 5. Dependency Inversion Principle (DIP)
- ✅ Controller'lar service interface'lerine bağımlı
- ✅ Service'ler repository interface'lerine bağımlı
- ✅ Concrete implementasyonlar `Program.cs`'de kayıt edilir

---

## 📦 Bağımlılıklar

### NuGet Paketleri
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
```

### Frontend Kütüphaneleri
- Bootstrap 5 (lib/bootstrap)
- jQuery (lib/jquery)
- jQuery Validation (lib/jquery-validation)
- jQuery Validation Unobtrusive (lib/jquery-validation-unobtrusive)

---

## 🚀 Çalıştırma

### Gereksinimler
- .NET 9.0 SDK
- Visual Studio 2022 veya VS Code

### Çalıştırma Adımları
1. Proje dizinine gidin: `cd BudgetTracker`
2. Uygulamayı çalıştırın: `dotnet run`
3. Tarayıcıda açın: `http://localhost:5000`

### Veritabanı
- İlk çalıştırmada otomatik oluşturulur (`EnsureCreated()`)
- Dosya: `BudgetTracker.db` (proje kök dizini)

---

## 📊 Proje İstatistikleri

- **Controller Sayısı:** 5
- **Service Sayısı:** 3
- **Model Sayısı:** 3
- **View Sayfası:** ~15
- **Veritabanı Tablosu:** 3
- **Toplam Kod Satırı:** ~2000+

---

## 🔮 Gelecek Geliştirmeler

### Önerilen İyileştirmeler
1. **Gelir Yönetimi:** Gelir ekleme ve takip özelliği
2. **Bütçe Planlama:** Aylık bütçe belirleme ve takip
3. **Raporlar:** PDF/Excel export özelliği
4. **Grafikler:** Daha detaylı görselleştirmeler (Chart.js entegrasyonu)
5. **Filtreleme:** Gelişmiş filtreleme ve arama özellikleri
6. **Migration:** Code-First migrations kullanımı
7. **Unit Tests:** Test coverage artırılması
8. **API:** RESTful API endpoint'leri
9. **Email:** Şifre sıfırlama özelliği
10. **Multi-language:** Çoklu dil desteği

---

## 📝 Sonuç

Budget Tracker projesi, modern yazılım geliştirme prensiplerine uygun, ölçeklenebilir ve bakımı kolay bir web uygulamasıdır. SOLID prensipleri, Repository Pattern ve Service Layer Pattern kullanılarak temiz bir mimari oluşturulmuştur. Güvenlik önlemleri alınmış, kullanıcı verileri izole edilmiştir.

Proje, eğitim amaçlı veya küçük ölçekli kullanım için uygundur. Production ortamı için ek güvenlik önlemleri (HTTPS zorunluluğu, rate limiting, vb.) ve performans optimizasyonları önerilir.

---

**Rapor Tarihi:** 2024  
**Proje Versiyonu:** 1.0  
**Framework:** .NET 9.0
