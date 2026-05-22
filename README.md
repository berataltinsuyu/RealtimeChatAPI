# Real-Time Chat API

ASP.NET Core, SignalR, JWT Authentication, Entity Framework Core ve SQLite kullanılarak geliştirilmiş gerçek zamanlı mesajlaşma backend projesidir.

Bu proje; kullanıcı girişi, oda bazlı mesajlaşma, mesaj geçmişi ve SignalR ile gerçek zamanlı iletişim mantığını öğrenmek amacıyla geliştirilmiştir.

## Özellikler

- Kullanıcı kayıt ve giriş sistemi
- JWT tabanlı kimlik doğrulama
- BCrypt ile şifre hashleme
- Oda oluşturma, odaya katılma ve odadan ayrılma
- SignalR ile gerçek zamanlı mesajlaşma
- Mesajları veritabanına kaydetme
- Sayfalama ile mesaj geçmişi getirme
- Temel okundu bilgisi desteği
- Katmanlı mimari

## Kullanılan Teknolojiler

- ASP.NET Core Web API
- SignalR
- Entity Framework Core
- SQLite
- JWT Bearer Authentication
- BCrypt.Net
- C#

## Proje Yapısı

```text
RealtimeChatAPI/
├── API/
│   ├── Controllers/
│   └── Hubs/
├── Application/
│   ├── DTOs/
│   └── Services/
├── Domain/
│   ├── Entities/
│   └── Interfaces/
├── Infrastructure/
│   ├── Data/
│   ├── Migrations/
│   └── Repositories/
├── Program.cs
└── appsettings.json
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
```

### Messages

```http
GET /api/rooms/{roomId}/messages?page=1&pageSize=20
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

Client tarafında dinlenebilen eventler:

```text
Connected
UserJoinedRoom
UserLeftRoom
ReceiveMessage
MessageRead
```

## Kurulum

```bash
git clone https://github.com/berataltinsuyu/realtime-chat-api.git
cd realtime-chat-api
dotnet restore
dotnet ef database update
dotnet run
```

Uygulama terminalde görünen localhost adresinde çalışır.

Örnek:

```text
http://localhost:5258
```

## Örnek Test

Kullanıcı girişi:

```bash
curl -X POST http://localhost:5258/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"berat","password":"123456"}'
```

Token ile oda oluşturma:

```bash
curl -X POST http://localhost:5258/api/rooms \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"name":"general"}'
```

Mesaj gönderme:

```bash
curl -X POST http://localhost:5258/api/rooms/1/messages \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"content":"merhaba realtime chat"}'
```

## Mevcut Durum

Tamamlananlar:

- Auth sistemi
- JWT token üretimi
- Room endpointleri
- Message endpointleri
- SignalR ChatHub
- Mesajların veritabanına kaydedilmesi
- Basit HTML SignalR test client

Planlananlar:

- Online kullanıcı takibi
- Global exception middleware
- FluentValidation
- Swagger JWT desteği
- Gelişmiş okundu bilgisi modeli

