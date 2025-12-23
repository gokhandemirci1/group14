# Ücretsiz Deployment Kılavuzu

Bu dokümanda .NET Core 9.0 uygulamanızı ücretsiz olarak deploy edebileceğiniz platformlar ve adımları bulacaksınız.

## 🆓 Ücretsiz Platformlar

### 1. **Render** (Önerilen - En Kolay) ⭐

**Avantajları:**
- ✅ Tamamen ücretsiz plan
- ✅ Otomatik SSL sertifikası
- ✅ GitHub entegrasyonu
- ✅ .NET 9.0 desteği
- ✅ Kolay kurulum

**Sınırlamalar:**
- 15 dakika aktivite olmazsa uygulama durur (sleep)
- İlk istekte 30-60 saniye bekleme (cold start)
- 750 saat/ay ücretsiz kullanım

**Kurulum Adımları:**

1. **Hesap Oluşturun**
   - https://render.com adresine gidin
   - "Get Started for Free" butonuna tıklayın
   - GitHub hesabınızla giriş yapın

2. **Yeni Web Service Oluşturun**
   - Dashboard'da "New +" → "Web Service" seçin
   - GitHub repository'nizi bağlayın: `gokhandemirci1/group14`

3. **Ayarları Yapılandırın**
   - **Name**: `budgettracker` (veya istediğiniz isim)
   - **Region**: `Frankfurt (EU)` (Türkiye'ye yakın)
   - **Branch**: `master`
   - **Runtime**: `Docker` (otomatik algılanacak)
   - **Build Command**: Boş bırakın (Dockerfile kullanılacak)
   - **Start Command**: Boş bırakın (Dockerfile kullanılacak)

4. **Environment Variables (Opsiyonel)**
   - `ASPNETCORE_ENVIRONMENT`: `Production`
   - `PORT`: Render otomatik sağlar

5. **Deploy**
   - "Create Web Service" butonuna tıklayın
   - Build işlemi başlar (5-10 dakika sürebilir)
   - Deploy tamamlandığında URL alırsınız: `https://budgettracker-xxxx.onrender.com`

**Önemli Not:** İlk deploy'dan sonra 15 dakika inaktif kalırsa uygulama "sleep" moduna geçer. İlk istekte 30-60 saniye bekleme olabilir.

---

### 2. **Fly.io** ⭐

**Avantajları:**
- ✅ Ücretsiz plan (3 shared-cpu-1x VM)
- ✅ Hızlı deployment
- ✅ Global CDN
- ✅ Sleep yok (her zaman çalışır)
- ✅ Özel domain desteği

**Sınırlamalar:**
- Aylık 160 GB transfer limiti (çoğu proje için yeterli)
- 3 GB RAM limiti

**Kurulum Adımları:**

1. **Fly.io CLI Kurulumu**
   
   **Windows (PowerShell):**
   ```powershell
   # PowerShell'i Admin olarak açın
   iwr https://fly.io/install.ps1 -useb | iex
   ```

   **Alternatif (Scoop):**
   ```powershell
   scoop install flyctl
   ```

2. **Giriş Yapın**
   ```bash
   fly auth login
   ```
   - Tarayıcı açılacak, GitHub ile giriş yapın

3. **Deploy**
   ```bash
   fly launch
   ```
   - App name sorulacak: `budgettracker` (veya istediğiniz isim)
   - Region sorulacak: `ams` (Amsterdam) veya `fra` (Frankfurt) seçin
   - Dockerfile otomatik algılanacak
   - PostgreSQL sorulursa "No" deyin (SQLite kullanıyoruz)

4. **Deploy Tamamlandıktan Sonra**
   ```bash
   fly open
   ```
   - Uygulama tarayıcıda açılacak
   - URL: `https://budgettracker.fly.dev`

5. **Logları İzleme**
   ```bash
   fly logs
   ```

**Önemli Komutlar:**
```bash
# Uygulama durumunu kontrol et
fly status

# Logları görüntüle
fly logs

# Uygulamayı yeniden başlat
fly apps restart budgettracker

# Scale ayarları (ücretsiz plan için gerekli değil)
fly scale count 1
```

---

### 3. **Azure App Service (Free Tier)**

**Avantajları:**
- ✅ Microsoft'un resmi platformu
- ✅ .NET için mükemmel destek
- ✅ Production için ideal
- ✅ Güvenilir ve stabil

**Sınırlamalar:**
- Free tier'da sınırlı kaynaklar
- Kredi kartı gerekebilir (ücretsiz kullanım için)

**Kurulum Adımları:**

1. **Azure Hesabı Oluşturun**
   - https://azure.microsoft.com/free/ adresine gidin
   - Microsoft hesabıyla kayıt olun
   - $200 kredisi alırsınız (12 ay geçerli)

2. **Azure Portal'a Giriş**
   - https://portal.azure.com
   - "Create a resource" → "Web App"

3. **Web App Oluşturma**
   - **Name**: `budgettracker-xxxx` (benzersiz olmalı)
   - **Publish**: `Code`
   - **Runtime stack**: `.NET 9 (LTS)` seçin
   - **Operating System**: `Linux`
   - **Region**: `West Europe` veya `North Europe`
   - **App Service Plan**: `Free F1` seçin (veya yeni bir tane oluşturun)

4. **Deployment**
   - "Deployment Center" → "GitHub" seçin
   - Repository'nizi bağlayın: `gokhandemirci1/group14`
   - Branch: `master`
   - Deployment otomatik başlar

5. **Configuration**
   - "Configuration" → "Application settings"
   - `ASPNETCORE_ENVIRONMENT`: `Production`
   - `PORT`: Azure otomatik sağlar

**Önemli:** Azure Free tier'ı kullanırken kredi kartı gerekebilir ama ücret alınmaz (limitler içinde kaldığınız sürece).

---

### 4. **Railway** (Zaten Konfigüre Edilmiş)

Railway için zaten `railway.json` ve `Dockerfile` hazır. Sadece Railway dashboard'dan GitHub repository'nizi bağlamanız yeterli.

**Kurulum:**
1. https://railway.app → "Login with GitHub"
2. "New Project" → "Deploy from GitHub repo"
3. `gokhandemirci1/group14` seçin
4. Otomatik deploy başlar

---

## 🔄 Platform Karşılaştırması

| Platform | Ücretsiz Plan | Sleep Mode | Cold Start | Kurulum Kolaylığı | Önerilen |
|----------|--------------|------------|------------|-------------------|----------|
| **Render** | ✅ 750 saat/ay | ✅ Var (15 dk) | 30-60 sn | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Fly.io** | ✅ 3 VM | ❌ Yok | Yok | ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Railway** | ✅ $5 kredi/ay | ❌ Yok | Yok | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ |
| **Azure** | ✅ Free tier | ✅ Var | 30-60 sn | ⭐⭐⭐ | ⭐⭐⭐ |

---

## 📝 Ortak Notlar

### SQLite Veritabanı

Tüm platformlarda SQLite kullanıyorsunuz. Ancak:

⚠️ **Önemli:** SQLite dosyaları geçici depolamada saklanır. Uygulama yeniden başlatıldığında veriler kaybolabilir!

**Çözüm Seçenekleri:**

1. **Kalıcı Depolama Kullanın:**
   - Render: Disk ekleyebilirsiniz (ücretli)
   - Fly.io: Volumes kullanabilirsiniz
   - Azure: App Service için storage mount

2. **PostgreSQL'e Geçin (Önerilen):**
   - Render: PostgreSQL ekleyebilirsiniz (ücretsiz)
   - Fly.io: PostgreSQL ekleyebilirsiniz
   - Railway: PostgreSQL ekleyebilirsiniz
   - Azure: Azure Database for PostgreSQL (ücretsiz tier)

### Environment Variables

Tüm platformlarda şu environment variable'ları ayarlayabilirsiniz:
- `ASPNETCORE_ENVIRONMENT`: `Production`
- `ConnectionStrings__DefaultConnection`: PostgreSQL için (eğer kullanıyorsanız)

### Custom Domain

Tüm platformlar custom domain desteği sunar:
- **Render**: Settings → Custom Domains
- **Fly.io**: `fly certs add yourdomain.com`
- **Azure**: Custom domains blade
- **Railway**: Settings → Domains

---

## 🚀 Hızlı Başlangıç

En kolay başlangıç için **Render** önerilir:

1. https://render.com → GitHub ile giriş
2. "New Web Service" → Repository seç
3. Docker seç → Deploy

5-10 dakika içinde uygulamanız canlıda!

---

## ❓ Sorun Giderme

### Build Hatası
- Dockerfile'ın doğru konumda olduğundan emin olun
- `.dockerignore` dosyası varsa kontrol edin

### PORT Hatası
- `Program.cs`'de PORT environment variable desteği var
- Platform otomatik PORT sağlar

### Veritabanı Hatası
- SQLite dosyası geçici depolamada, veriler kaybolabilir
- Production için PostgreSQL kullanmayı düşünün

---

## 📚 Daha Fazla Bilgi

- [Render Docs](https://render.com/docs)
- [Fly.io Docs](https://fly.io/docs)
- [Azure App Service Docs](https://learn.microsoft.com/azure/app-service/)
- [Railway Docs](https://docs.railway.app)

