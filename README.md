# Real-Time Chat API

ASP.NET Core, SignalR, JWT Authentication, Entity Framework Core ve SQLite kullanılarak geliştirilmiş gerçek zamanlı chat API projesidir.

Proje; kullanıcı kimlik doğrulama, oda bazlı mesajlaşma, gerçek zamanlı mesaj gönderimi, online kullanıcı takibi ve temiz hata yönetimi konularını öğrenmek amacıyla geliştirilmiştir.

## Özellikler

- Kullanıcı kayıt ve giriş sistemi
- JWT tabanlı kimlik doğrulama
- Oda oluşturma, odaya katılma ve odadan ayrılma
- SignalR ile gerçek zamanlı mesajlaşma
- Mesaj geçmişi ve sayfalama
- Online kullanıcı takibi
- Oda bazlı online kullanıcı listesi
- Global exception middleware
- Custom exception yapısı
- FluentValidation ile request doğrulama
- Swagger JWT desteği
- Katmanlı mimari

## Teknolojiler

- ASP.NET Core Web API
- SignalR
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- BCrypt.Net
- FluentValidation
- Swagger / Swashbuckle
- C#

## Proje Yapısı

```text
RealtimeChatAPI/
├── API/
│   ├── Controllers/
│   ├── Hubs/
│   ├── Middleware/
│   └── Validators/
├── Application/
│   ├── DTOs/
│   ├── Exceptions/
│   └── Services/
├── Domain/
│   ├── Entities/
│   └── Interfaces/
├── Infrastructure/
│   ├── Data/
│   ├── Migrations/
│   └── Repositories/
└── Program.cs
```

## Kurulum

```bash
git clone https://github.com/berataltinsuyu/realtime-chat-api.git
cd realtime-chat-api
dotnet restore
dotnet ef database update
dotnet run
```

Uygulama varsayılan olarak terminalde görünen localhost adresinde çalışır.

```text
http://localhost:5258
```

## Swagger

```text
http://localhost:5258/swagger
```

JWT gerektiren endpointler için Swagger üzerindeki `Authorize` butonuna token şu formatta girilebilir:

```text
Bearer {token}
```

## API Endpointleri

### Auth

```http
POST /api/auth/register
POST /api/auth/login
```

### Rooms

```http
POST /api/rooms
GET /api/rooms
POST /api/rooms/{id}/join
POST /api/rooms/{id}/leave
GET /api/rooms/{id}/members
GET /api/rooms/{id}/online
```

### Messages

```http
GET /api/rooms/{roomId}/messages
POST /api/rooms/{roomId}/messages
```

## SignalR Hub

Hub endpoint:

```text
/chatHub
```

Client tarafından çağrılabilen methodlar:

```text
JoinRoom(roomId)
LeaveRoom(roomId)
SendMessage(roomId, content)
MarkAsRead(roomId)
```

Dinlenebilen eventler:

```text
Connected
UserOnline
UserOffline
UserJoinedRoom
UserLeftRoom
ReceiveMessage
MessageRead
```

## Test

Projede bulunan `test.html` dosyası ile SignalR bağlantısı lokal olarak test edilebilir.

Genel test akışı:

```text
1. API'yi çalıştır
2. Login endpointinden JWT token al
3. test.html dosyasını aç
4. Token alanına JWT token'ı yapıştır
5. Connect butonuna bas
6. Join Room ile odaya katıl
7. Send Message ile mesaj gönder
```



