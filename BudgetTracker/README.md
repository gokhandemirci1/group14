# Kişisel Bütçe Takip Uygulaması

## Projeyi Çalıştırma

### 1. Projeyi Çalıştırın
```bash
cd BudgetTracker
dotnet run
```

### 2. Tarayıcıda Açın
Proje çalıştıktan sonra şu adreslerden birini kullanın:

- **HTTP**: http://localhost:5081
- **HTTPS**: https://localhost:7179

### 3. İlk Kullanım
1. "Kayıt Ol" butonuna tıklayın
2. Kullanıcı adı ve şifre oluşturun
3. Sisteme giriş yapın
4. Kategori oluşturun
5. Harcama ekleyin

## Sorun Giderme

### Siteye Ulaşılamıyor Hatası

**Çözüm 1: Port Kontrolü**
- Başka bir uygulama aynı portu kullanıyor olabilir
- `launchSettings.json` dosyasında farklı portlar deneyin

**Çözüm 2: Firewall Kontrolü**
- Windows Firewall'un portları engellemediğinden emin olun
- Geliştirme ortamında genellikle sorun olmaz

**Çözüm 3: Projeyi Yeniden Başlatın**
```bash
# Terminal'de Ctrl+C ile durdurun
# Sonra tekrar başlatın:
dotnet run
```

**Çözüm 4: Port Değiştirme**
`Properties/launchSettings.json` dosyasını düzenleyin:
```json
"applicationUrl": "http://localhost:5000;https://localhost:5001"
```

**Çözüm 5: Build ve Restore**
```bash
dotnet clean
dotnet restore
dotnet build
dotnet run
```

## Teknolojiler
- .NET 9.0
- ASP.NET Core MVC
- Entity Framework Core (SQLite)
- Bootstrap 5
- Chart.js

## Veritabanı
SQLite veritabanı (`BudgetTracker.db`) proje çalıştırıldığında otomatik oluşturulur.





