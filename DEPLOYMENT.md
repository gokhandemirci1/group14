# Deployment Kılavuzu

Bu proje için deployment konfigürasyon dosyaları hazırlanmıştır.

## GitHub Repository
- **Repository URL**: https://github.com/gokhandemirci1/group14

## Deployment Platformları

### ⚠️ Vercel Hakkında Önemli Bilgi

**Vercel .NET Core uygulamalarını doğrudan desteklemez.**

Vercel aşağıdaki runtime'ları destekler:
- Node.js
- Python
- Go
- Static Sites

`.NET Core` için Vercel'de resmi bir builder yoktur.

### ✅ Önerilen Platformlar

#### 1. Railway (En Kolay)
Railway .NET Core uygulamalarını tam destekler.

**Kurulum:**
1. https://railway.app adresinden hesap oluşturun
2. "New Project" → "Deploy from GitHub repo"
3. Repository'yi seçin: `gokhandemirci1/group14`
4. Railway otomatik olarak `railway.json` dosyasını kullanacaktır
5. Deploy başlar ve URL alırsınız

**Avantajları:**
- Kolay kurulum
- Otomatik deployment
- Ücretsiz plan mevcut
- SQLite desteği

#### 2. Render
Render .NET Core uygulamalarını destekler.

**Kurulum:**
1. https://render.com adresinden hesap oluşturun
2. "New" → "Web Service"
3. GitHub repository'nizi bağlayın: `gokhandemirci1/group14`
4. Render `render.yaml` dosyasını otomatik olarak algılar
5. "Create Web Service" butonuna tıklayın

**Avantajları:**
- Ücretsiz plan mevcut
- Otomatik SSL
- Kolay kurulum

#### 3. Azure App Service
Microsoft'un kendi platformu, .NET için mükemmel.

**Kurulum:**
1. Azure Portal → App Services → Create
2. GitHub ile continuous deployment ayarlayın
3. .NET 9.0 runtime seçin

**Avantajları:**
- Microsoft desteği
- Enterprise özellikler
- Production için ideal

### Git Push Komutları

Repository'ye push etmek için:

```bash
# Mevcut değişiklikleri kontrol et
git status

# Tüm değişiklikleri ekle
git add .

# Commit oluştur
git commit -m "Add deployment configuration files"

# GitHub'a push et
git push origin main
```

Eğer branch adı farklıysa (örneğin `master`):
```bash
git push origin master
```

## Konfigürasyon Dosyaları

### vercel.json
Vercel için konfigürasyon (⚠️ .NET desteklemediği için çalışmayabilir)

### railway.json
Railway için konfigürasyon (✅ Çalışır)

### render.yaml
Render için konfigürasyon (✅ Çalışır)

## Production İçin Öneriler

1. **Veritabanı**: SQLite yerine PostgreSQL veya SQL Server kullanmayı düşünün
2. **Environment Variables**: Hassas bilgileri environment variable olarak saklayın
3. **HTTPS**: Tüm platformlar otomatik SSL sağlar
4. **Logging**: Production logging için uygun seviyeleri ayarlayın
5. **Error Handling**: Production mode'da detaylı hata mesajlarını kullanıcıya göstermeyin

