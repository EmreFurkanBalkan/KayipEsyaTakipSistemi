# LostItems API - Postman Test Rehberi

## API Endpoint'leri

Base URL: `http://localhost:5203/api/LostItemsApi`

### 1. Tüm Kayıp Eşyaları Getir (GET)

**Endpoint:** `GET /api/LostItemsApi`

**Postman Ayarları:**
- Method: `GET`
- URL: `http://localhost:5203/api/LostItemsApi`
- Headers: `Content-Type: application/json`

**Beklenen Response:**
```json
[
  {
    "id": 1,
    "itemName": "Cüzdan",
    "description": "Siyah deri cüzdan",
    "foundDate": "2024-01-15T10:30:00",
    "status": "TeslimEdilmedi",
    "lineCodeId": 1,
    "lineCodeName": "M1A",
    "location": "Araç Kapı No: 1234",
    "foundBy": "Ahmet Yılmaz"
  }
]
```

### 2. Belirli Bir Kayıp Eşyayı Getir (GET)

**Endpoint:** `GET /api/LostItemsApi/{id}`

**Postman Ayarları:**
- Method: `GET`
- URL: `http://localhost:5203/api/LostItemsApi/1`
- Headers: `Content-Type: application/json`

**Beklenen Response:**
```json
{
  "id": 1,
  "itemName": "Cüzdan",
  "description": "Siyah deri cüzdan",
  "foundDate": "2024-01-15T10:30:00",
  "status": "TeslimEdilmedi",
  "lineCodeId": 1,
  "lineCodeName": "M1A",
  "location": "Araç Kapı No: 1234",
  "foundBy": "Ahmet Yılmaz"
}
```

### 3. Yeni Kayıp Eşya Oluştur (POST)

**Endpoint:** `POST /api/LostItemsApi`

**Postman Ayarları:**
- Method: `POST`
- URL: `http://localhost:5203/api/LostItemsApi`
- Headers: `Content-Type: application/json`
- Body (raw JSON):

```json
{
  "itemName": "Telefon",
  "description": "iPhone 14 Pro, mavi renk",
  "foundDate": "2024-01-20T14:30:00",
  "status": "TeslimEdilmedi",
  "lineCodeId": 2,
  "location": "Araç Kapı No: 5678",
  "foundBy": "Mehmet Demir"
}
```

**Zorunlu Alanlar:**
- `itemName` (string): Eşya adı
- `foundDate` (datetime): Bulunma tarihi
- `status` (string): "TeslimEdilmedi" veya "TeslimEdildi"
- `location` (string): Araç kapı numarası

**Opsiyonel Alanlar:**
- `description` (string): Açıklama
- `lineCodeId` (int): Hat kodu ID'si
- `foundBy` (string): Bulan kişi

**Beklenen Response (201 Created):**
```json
{
  "id": 2,
  "itemName": "Telefon",
  "description": "iPhone 14 Pro, mavi renk",
  "foundDate": "2024-01-20T14:30:00",
  "status": "TeslimEdilmedi",
  "lineCodeId": 2,
  "lineCodeName": "M2",
  "location": "Araç Kapı No: 5678",
  "foundBy": "Mehmet Demir"
}
```

### 4. Kayıp Eşyayı Güncelle (PUT)

**Endpoint:** `PUT /api/LostItemsApi/{id}`

**Postman Ayarları:**
- Method: `PUT`
- URL: `http://localhost:5203/api/LostItemsApi/1`
- Headers: `Content-Type: application/json`
- Body (raw JSON):

```json
{
  "itemName": "Cüzdan (Güncellendi)",
  "description": "Siyah deri cüzdan - içinde kimlik var",
  "foundDate": "2024-01-15T10:30:00",
  "status": "TeslimEdildi",
  "lineCodeId": 1,
  "location": "Araç Kapı No: 1234",
  "foundBy": "Ahmet Yılmaz"
}
```

**Beklenen Response (204 No Content):**
Boş response body

### 5. Kayıp Eşyayı Sil (DELETE)

**Endpoint:** `DELETE /api/LostItemsApi/{id}`

**Postman Ayarları:**
- Method: `DELETE`
- URL: `http://localhost:5203/api/LostItemsApi/1`
- Headers: `Content-Type: application/json`

**Beklenen Response (204 No Content):**
Boş response body

### 6. Hat Kodlarını Getir (GET)

**Endpoint:** `GET /api/LostItemsApi/linecodes`

**Postman Ayarları:**
- Method: `GET`
- URL: `http://localhost:5203/api/LostItemsApi/linecodes`
- Headers: `Content-Type: application/json`

**Beklenen Response:**
```json
[
  {
    "id": 1,
    "line": "M1A"
  },
  {
    "id": 2,
    "line": "M2"
  }
]
```

## Hata Durumları

### 400 Bad Request
```json
{
  "message": "Geçersiz durum değeri. Kullanılabilir değerler: TeslimEdilmedi, TeslimEdildi"
}
```

### 404 Not Found
```json
{
  "message": "Kayıp eşya bulunamadı."
}
```

### 400 Bad Request (Validation Error)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "...",
  "errors": {
    "ItemName": [
      "Eşya adı gereklidir"
    ]
  }
}
```

## Postman Collection Oluşturma Adımları

### 1. Yeni Collection Oluştur
1. Postman'i açın
2. Sol panelde "Collections" sekmesine tıklayın
3. "+ Create Collection" butonuna tıklayın
4. Collection adını "LostItems API" olarak ayarlayın

### 2. Environment Oluştur
1. Sol üst köşedeki "Environments" sekmesine tıklayın
2. "+ Create Environment" butonuna tıklayın
3. Environment adını "Local Development" olarak ayarlayın
4. Variable ekleyin:
   - Variable: `baseUrl`
   - Initial Value: `http://localhost:5203`
   - Current Value: `http://localhost:5203`

### 3. Request'leri Ekle

Her endpoint için ayrı request oluşturun:

#### GET All Items
- Name: "Get All Lost Items"
- Method: GET
- URL: `{{baseUrl}}/api/LostItemsApi`

#### GET Single Item
- Name: "Get Lost Item by ID"
- Method: GET
- URL: `{{baseUrl}}/api/LostItemsApi/1`

#### POST Create Item
- Name: "Create Lost Item"
- Method: POST
- URL: `{{baseUrl}}/api/LostItemsApi`
- Headers: `Content-Type: application/json`
- Body: Yukarıdaki JSON örneğini kullanın

#### PUT Update Item
- Name: "Update Lost Item"
- Method: PUT
- URL: `{{baseUrl}}/api/LostItemsApi/1`
- Headers: `Content-Type: application/json`
- Body: Yukarıdaki JSON örneğini kullanın

#### DELETE Item
- Name: "Delete Lost Item"
- Method: DELETE
- URL: `{{baseUrl}}/api/LostItemsApi/1`

#### GET Line Codes
- Name: "Get Line Codes"
- Method: GET
- URL: `{{baseUrl}}/api/LostItemsApi/linecodes`

## Test Senaryoları

### Senaryo 1: Tam CRUD İşlemleri
1. **GET /linecodes** - Mevcut hat kodlarını görün
2. **POST /** - Yeni kayıp eşya oluşturun
3. **GET /** - Tüm kayıp eşyaları listeleyin
4. **GET /{id}** - Oluşturduğunuz eşyayı ID ile getirin
5. **PUT /{id}** - Eşyayı güncelleyin (status'u "TeslimEdildi" yapın)
6. **DELETE /{id}** - Eşyayı silin

### Senaryo 2: Hata Durumları
1. **GET /999** - Olmayan ID ile 404 hatası alın
2. **POST /** - Eksik alanlarla 400 hatası alın
3. **PUT /999** - Olmayan ID ile güncelleme yapın
4. **DELETE /999** - Olmayan ID ile silme yapın

### Senaryo 3: Validation Testleri
1. **POST /** - ItemName olmadan gönder
2. **POST /** - Geçersiz Status ile gönder ("InvalidStatus")
3. **POST /** - Geçersiz LineCodeId ile gönder
4. **POST /** - Geçersiz tarih formatı ile gönder

## Önemli Notlar

1. **Uygulama Çalıştırma:** API testlerini yapmadan önce uygulamanızı `dotnet run` komutu ile çalıştırmayı unutmayın.

2. **Port Kontrolü:** Uygulamanızın hangi portta çalıştığını kontrol edin. Genellikle `https://localhost:7203` veya `http://localhost:5203` olur.

3. **HTTPS vs HTTP:** Eğer HTTPS kullanıyorsanız, Postman'de SSL certificate verification'ı kapatmanız gerekebilir.

4. **Database:** SQLite veritabanınızda test verileri olduğundan emin olun.

5. **Logging:** API çağrıları sistem loglarına kaydedilir, bu logları MVC uygulamasından kontrol edebilirsiniz.

6. **CORS:** Eğer farklı bir domain'den test yapıyorsanız, CORS ayarlarını kontrol edin.

## Troubleshooting

### Problem: 404 Not Found
- URL'nin doğru olduğundan emin olun
- Uygulamanın çalıştığını kontrol edin
- Route'ların doğru tanımlandığını kontrol edin

### Problem: 500 Internal Server Error
- Uygulama loglarını kontrol edin
- Database bağlantısını kontrol edin
- Model validation'ları kontrol edin

### Problem: CORS Hatası
- Program.cs'de CORS ayarlarını kontrol edin
- Browser'dan test yapmak yerine Postman kullanın

Bu rehber ile LostItems API'nizi Postman üzerinden kapsamlı bir şekilde test edebilirsiniz.