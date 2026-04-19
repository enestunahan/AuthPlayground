# AuthPlayground Authentication & Authorization Rehberi

Bu doküman, `AuthPlayground` projesinde kurulan **Identity + JWT + Refresh Token + Role-based Authorization** yapısını ve test akışını anlatır.

## 1) Kurulan Mimari

Proje katmanları `BookNetwork` mantığıyla korunmuştur:

- `src/Core/AuthPlayground.Domain`
- `src/Core/AuthPlayground.Application`
- `src/Infrastructure/AuthPlayground.Persistence`
- `src/Infrastructure/AuthPlayground.Infrastructure`
- `src/Presentation/AuthPlayground.API`

### 1.1 Domain

- `AppUser : IdentityUser<string>`
- `AppRole : IdentityRole<string>` (şimdilik boş)
- `Book` entity (önceki akış korunuyor)

`AppUser`:

```csharp
public class AppUser : IdentityUser<string>
{
    public string NameSurname { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}
```

### 1.2 Persistence

- `AuthPlaygroundDbContext`, `IdentityDbContext<AppUser, AppRole, string>` üzerinden çalışır.
- `Book` tablosu + Identity tabloları (`AspNetUsers`, `AspNetRoles`, ...) aynı DB içinde oluşturulur.
- `AddIdentityCore + AddRoles + AddEntityFrameworkStores` yapılandırması eklidir.
- Uygulama açılışında:
  - `EnsureCreated()` ile tablo oluşturma
  - `IdentitySeeder.SeedAsync()` ile rol/kullanıcı seed

### 1.3 Infrastructure

- `JwtTokenService`:
  - Access token üretimi
  - Refresh token üretimi
  - User role claim'lerini token'a ekleme
- `CurrentUserService`:
  - Login olmuş kullanıcının `UserId`, `UserName`, `Role` bilgilerine erişim

### 1.4 Application (CQRS + MediatR)

Auth komutları:

- `RegisterCommand`
- `LoginCommand`
- `RefreshTokenLoginCommand`
- `LogoutCommand`

Book CQRS akışı korunmuştur (`Home` ve `AdminBooks` endpointleri için query/command handler'lar).

### 1.5 API

- JWT authentication aktif
- Authorization aktif
- `AuthController` eklendi
- `AdminBooksController` sadece `Admin`
- `HomeController` içinde farklı role kombinasyonları için örnek endpointler eklendi
- Permission policy (dynamic policy/provider) tarafına **bilerek girilmedi**

## 2) Seed Edilen Roller ve Kullanıcılar

Seed sırasında oluşturulan roller:

- `Admin`
- `Editor`
- `User`
- `Viewer`

Seed kullanıcıları (ilk açılışta):

- `admin` -> `Admin`
- `enes.editor` -> `Editor`
- `enes.user` -> `User`
- `enes.viewer` -> `Viewer`

Tüm seed kullanıcı şifresi:

- `Password123!`

## 3) Config

`src/Presentation/AuthPlayground.API/appsettings.json` içinde:

- ConnectionString: `AuthPlaygroundIdentityDb`
- TokenOptions: `Issuer`, `Audience`, `SecurityKey`, expiration ayarları

## 4) Endpoint Listesi

## 4.1 Auth

Base route: `/api/auth`

- `POST /register`
- `POST /login`
- `POST /refresh-token-login`
- `POST /logout` (Authorize)

## 4.2 Home

Base route: `/api/home`

- `GET /public` -> herkes erişebilir
- `GET /books` -> `Admin,Editor,User,Viewer`
- `GET /me` -> authenticated user
- `GET /admin-or-editor` -> `Admin,Editor`
- `GET /user-and-above` -> `Admin,Editor,User`
- `GET /admin-only` -> `Admin`

## 4.3 Admin Books

Base route: `/api/admin/books`

- `GET /` -> Admin
- `POST /` -> Admin
- `PUT /{id}` -> Admin
- `DELETE /{id}` -> Admin

## 5) Test Senaryoları (Adım Adım)

Aşağıdaki testlerde API'nin `http://localhost:5105` portunda çalıştığı varsayılmıştır.

### 5.1 Uygulamayı başlat

```bash
dotnet run --project src/Presentation/AuthPlayground.API/AuthPlayground.API.csproj
```

### 5.2 Admin login

```bash
curl -X POST http://localhost:5105/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "userNameOrEmail": "admin",
    "password": "Password123!"
  }'
```

Response içinde şunlar gelir:

- `accessToken`
- `accessTokenExpiration`
- `refreshToken`
- `refreshTokenExpiration`

### 5.3 Editor login

```bash
curl -X POST http://localhost:5105/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "userNameOrEmail": "enes.editor",
    "password": "Password123!"
  }'
```

### 5.4 Admin token ile admin endpointine erişim (200 beklenir)

```bash
curl http://localhost:5105/api/admin/books \
  -H "Authorization: Bearer <ADMIN_ACCESS_TOKEN>"
```

### 5.5 Editor token ile admin endpointine erişim (403 beklenir)

```bash
curl -i http://localhost:5105/api/admin/books \
  -H "Authorization: Bearer <EDITOR_ACCESS_TOKEN>"
```

### 5.6 Role örnek endpoint testleri

Editor için `admin-or-editor` endpointi (200):

```bash
curl http://localhost:5105/api/home/admin-or-editor \
  -H "Authorization: Bearer <EDITOR_ACCESS_TOKEN>"
```

Viewer için `user-and-above` endpointi (403):

```bash
curl -i http://localhost:5105/api/home/user-and-above \
  -H "Authorization: Bearer <VIEWER_ACCESS_TOKEN>"
```

Viewer için `books` endpointi (200):

```bash
curl http://localhost:5105/api/home/books \
  -H "Authorization: Bearer <VIEWER_ACCESS_TOKEN>"
```

### 5.7 Refresh token login

```bash
curl -X POST http://localhost:5105/api/auth/refresh-token-login \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "<REFRESH_TOKEN>"
  }'
```

Yeni access/refresh token çifti döner.

### 5.8 Logout

```bash
curl -X POST http://localhost:5105/api/auth/logout \
  -H "Authorization: Bearer <ACCESS_TOKEN>"
```

Bu işlem user'ın refresh token bilgisini temizler.

### 5.9 Register (otomatik User rolü)

```bash
curl -X POST http://localhost:5105/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "nameSurname": "Test Kullanici",
    "userName": "test.user",
    "email": "test.user@authplayground.local",
    "password": "Password123!",
    "passwordConfirm": "Password123!"
  }'
```

Register sonrası kullanıcıya `User` rolü atanır.

## 6) Notlar

- Bu sürümde authorization tarafı **role-based** (Authorize(Roles=...)) olarak bırakıldı.
- Senin isteğine göre permission policy / dynamic endpoint permission yapısına girilmedi.
- İstersen bir sonraki adımda:
  - claim bazlı kontrol,
  - policy tabanlı erişim,
  - db-driven dynamic permission
  adımlarına geçebiliriz.
