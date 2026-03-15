# ITI Shop — ASP.NET Core 8 MVC E-Commerce Project

A full-stack e-commerce web application built with **ASP.NET Core 8 MVC**, featuring a customer-facing storefront and a secure admin dashboard.

---

## Features

### Customer
- Browse a paginated, filterable, and searchable product catalog
- Filter by category, search by name, sort by price or name
- Persistent shopping cart (database-backed per user)
- Cart total includes **14% tax** calculated automatically
- Checkout with shipping address, phone number, and real-time stock validation
- Full order history with detailed per-order view
- User registration with **email confirmation** (account inactive until email verified)
- Login/logout with cookie-based authentication (2-hour session)
- Forgot password / reset password via email link

### Admin
- Manage **Categories** — create, edit, delete
- Manage **Products** — create, edit, delete (with image upload to `wwwroot/images`)
- Manage **Orders** — view all orders, update status (`Pending → Processing → Shipped → Delivered / Cancelled`)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 (SQL Server) |
| Identity | ASP.NET Core Identity (cookie auth, email confirmation required) |
| Email | MailKit v4.8 over Gmail SMTP |
| Frontend | Bootstrap 5, jQuery, jQuery Validation |
| Architecture | Generic Repository + Unit of Work pattern |
| File Storage | Local filesystem (`wwwroot/images`) |

---

## Project Structure

```
ITI_MVC_Project/
├── Areas/
│   └── Admin/
│       ├── Controllers/        # CategoriesController, ProductsController, OrdersController
│       └── Views/              # Products/, Categories/, Orders/, Shared/
├── Authentication Contract/
│   └── IJwtProvider.cs         # JWT interface (reserved for future API use)
├── Consts/
│   ├── DefaultRoles.cs         # Admin & Customer role GUIDs
│   ├── DefaultUsers.cs         # Seeded admin user credentials & GUIDs
│   └── RegexPatterns.cs        # Password validation regex
├── Controllers/                # HomeController, AccountController, CatalogController,
│                               # CartController, CheckoutController, OrdersController
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── DbSeeder.cs             # Seeds roles and default admin user
│   └── EntitiesConfigurations/ # Fluent API configs for all entities
├── Helpers/
│   └── EmailBodyBuilder.cs     # Loads HTML templates + {{placeholder}} replacement
├── Migrations/                 # EF Core migrations (InitialCreate, InsertAdminData)
├── Models/
│   ├── Entities/               # ApplicationUser, Product, Category, Order, OrderItem,
│   │                           # CartItem, OrderStatus (enum)
│   └── ViewModels/             # Auth VMs, Catalog VMs, Cart VMs, Order VMs,
│                               # Admin VMs, PaginationVM, ErrorViewModel
├── Repositories/               # IGenericRepository<T>, IUnitOfWork + implementations
├── Services/
│   ├── AccountService.cs       # Registration, email confirmation, password reset
│   ├── CatalogService.cs       # Product listing with filtering, search, sort, pagination
│   ├── CartService.cs          # Cart CRUD (add, update, remove)
│   ├── CheckoutService.cs      # Stock check, transaction-safe order placement
│   ├── OrderService.cs         # User order history & details
│   ├── EmailService.cs         # MailKit SMTP sender
│   ├── FileService.cs          # Product image save/delete in wwwroot/images
│   └── Admin/                  # AdminCategoryService, AdminProductService, AdminOrderService
├── Settings/
│   └── MailSettings.cs         # SMTP configuration model
├── Templates/                  # HTML email templates
│   ├── ConfirmEmail.html
│   ├── ResetPasswordEmail.html
│   └── WelcomeEmail.html
├── Views/
│   ├── Account/                # Login, Register, RegisterConfirmation, ForgotPassword,
│   │                           # ResetPassword, AccessDenied
│   ├── Catalog/                # Index (grid + filters), Details
│   ├── Cart/                   # Index
│   ├── Checkout/               # Index
│   ├── Orders/                 # Index, Details
│   ├── Home/                   # Index, Privacy
│   └── Shared/                 # _Layout, Error, _ValidationScriptsPartial
├── wwwroot/                    # Static assets (Bootstrap 5, jQuery, CSS, JS, product images)
├── DependencyInjection.cs      # All service/repository registrations
├── Program.cs
└── appsettings.json
```

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or full instance)
- A Gmail account with an [App Password](https://support.google.com/accounts/answer/185833) enabled

### 1. Clone the repository
```bash
git clone https://github.com/ZiadSalah2003/ITI_MVC_Project.git
cd ITI_MVC_Project/ITI_MVC_Project
```

### 2. Configure settings

Open `appsettings.json` and fill in the `MailSettings` and `ConnectionStrings` sections:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ITI_MVC_Project;Trusted_Connection=True;Encrypt=False"
},
"MailSettings": {
  "Mail": "your-email@gmail.com",
  "DisplayName": "ITI Shop",
  "Password": "your-gmail-app-password",
  "Host": "smtp.gmail.com",
  "Port": 587
}
```

### 3. Apply migrations & seed data
```bash
dotnet ef database update
```

This runs all migrations and seeds:
- `Admin` and `Customer` roles
- Default admin user: `admin@iti.com` / `Admin@123`

### 4. Run the application
```bash
dotnet run
```

Browse to `https://localhost:{port}`.

---

## Authentication Flow

```
Register → email sent → click confirmation link → account activated → Login
                                                                         ↓
                                                               Forget Password?
                                                                         ↓
                                                          Enter email → reset link sent
                                                                         ↓
                                                          Click link → set new password
```

- Registration does **not** auto-sign in — the account is locked until the email is confirmed.
- `SignIn.RequireConfirmedEmail = true` is enforced at the Identity level.
- Unconfirmed users attempting to log in see a clear message directing them to check their inbox.
- Cookie auth session expires after **2 hours**; session state (cart fallback) expires after **30 minutes** of inactivity.

---

## Password Requirements

Passwords must be at least **6 characters** and contain:
- At least one **digit**
- At least one **lowercase** letter

---

## Roles & Authorization

| Role | Access |
|---|---|
| `Customer` | Catalog (public), Cart, Checkout, Orders, Account |
| `Admin` | Everything above + `/Admin` area (Categories, Products, Orders management) |

Both roles are seeded automatically on first run.

---

## Email Templates

Located in `Templates/`:

| Template | Trigger |
|---|---|
| `ConfirmEmail.html` | After registration — confirms the account |
| `ResetPasswordEmail.html` | After "Forgot Password" request |
| `WelcomeEmail.html` | Available for post-confirmation welcome messaging |

Templates use `{{name}}` and `{{action_url}}` as placeholders, replaced at send time by `EmailBodyBuilder`.

---

## Database Schema

```
ApplicationUser  ──< Order       ──< OrderItem >── Product
                 ──< CartItem    >── Product
Product          >── Category
```

### Entity Highlights
- **ApplicationUser** extends `IdentityUser` with `FirstName`, `LastName`, `Address`, `City`, `CreatedAt`
- **Order** tracks `ShippingAddress`, `City`, `PhoneNumber`, `TotalAmount`, and `OrderStatus`
- **CartItem** is persisted per user in the database (not session-only)
- **Product** supports soft-availability via `IsActive` flag and tracks `Stock` quantity

---

## NuGet Packages

| Package | Version | Purpose |
|---|---|---|
| `MailKit` | 4.8.0 | Email sending over SMTP |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | 8.0.* | Identity + EF Core integration |
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.* | SQL Server provider |
| `Microsoft.EntityFrameworkCore.Design` | 8.0.* | EF Core tooling support |
| `Microsoft.EntityFrameworkCore.Tools` | 8.0.* | CLI migration tools |

---

## Screenshots

> _Add screenshots of the storefront, cart, checkout, admin panel, and email confirmation pages here._

---

## License

This project was built as part of an ITI (Information Technology Institute) training program.
