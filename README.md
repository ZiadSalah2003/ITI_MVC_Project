# ITI Shop — ASP.NET Core 8 MVC E-Commerce Project

A full-stack e-commerce web application built with **ASP.NET Core 8 MVC**, featuring a customer-facing storefront and a secure admin dashboard.

---

## Features

### Customer
- Browse a paginated, filterable, and searchable product catalog
- Filter by category, search by name, sort by price or name
- Persistent shopping cart (database-backed per user)
- Checkout with shipping address and order placement
- Full order history with detailed order view
- User registration with **email confirmation** (account inactive until email verified)
- Login/logout with cookie-based authentication
- Forgot password / reset password via email link

### Admin
- Manage **Categories** — create, edit, delete
- Manage **Products** — create, edit, delete (with image upload)
- Manage **Orders** — view all orders, update status (`Pending → Processing → Shipped → Delivered / Cancelled`)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 MVC |
| ORM | Entity Framework Core 8 (SQL Server) |
| Identity | ASP.NET Core Identity (cookie auth) |
| Email | MailKit + MimeKit over Gmail SMTP |
| Frontend | Bootstrap 5, jQuery, jQuery Validation |
| Architecture | Generic Repository + Unit of Work pattern |

---

## Project Structure

```
ITI_MVC_Project/
├── Areas/
│   └── Admin/
│       ├── Controllers/        # CategoriesController, ProductsController, OrdersController
│       └── Views/
├── Controllers/                # AccountController, CatalogController, CartController,
│                               # CheckoutController, OrdersController, HomeController
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── DbSeeder.cs
│   └── EntitiesConfigurations/
├── Helpers/
│   └── EmailBodyBuilder.cs     # Loads HTML templates + token replacement
├── Models/
│   ├── Entities/               # ApplicationUser, Product, Category, Order, OrderItem,
│   │                           # CartItem, OrderStatus
│   └── ViewModels/             # RegisterVM, LoginVM, ForgotPasswordVM, ResetPasswordVM,
│                               # ProductListVM, CartVM, CheckoutVM, OrderDetailsVM, ...
├── Repositories/               # IGenericRepository, IUnitOfWork + implementations
├── Services/
│   ├── AccountService.cs       # Registration, email confirmation, password reset
│   ├── CatalogService.cs
│   ├── CartService.cs
│   ├── CheckoutService.cs
│   ├── OrderService.cs
│   ├── EmailService.cs         # MailKit SMTP sender
│   ├── FileService.cs          # Product image upload
│   └── Admin/                  # AdminCategoryService, AdminProductService, AdminOrderService
├── Settings/
│   └── MailSettings.cs
├── Templates/                  # HTML email templates
│   ├── ConfirmEmail.html
│   ├── ResetPasswordEmail.html
│   └── WelcomeEmail.html
├── Views/
│   ├── Account/                # Login, Register, RegisterConfirmation, ForgotPassword,
│   │                           # ResetPassword, AccessDenied
│   ├── Catalog/                # Index, Details
│   ├── Cart/                   # Index
│   ├── Checkout/               # Index
│   ├── Orders/                 # Index, Details
│   ├── Home/                   # Index, Privacy
│   └── Shared/                 # _Layout, Error
├── wwwroot/                    # Static assets (Bootstrap 5, jQuery, CSS, JS, images)
├── DependencyInjection.cs      # All service registrations in one place
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

Open `appsettings.json` and fill in the `MailSettings` section:

```json
"MailSettings": {
  "Mail": "your-email@gmail.com",
  "DisplayName": "ITI Shop",
  "Password": "your-gmail-app-password",
  "Host": "smtp.gmail.com",
  "Port": 587
}
```

Update the connection string if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ITI_MVC_Project;Trusted_Connection=True;Encrypt=False"
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

---

## Roles & Authorization

| Role | Access |
|---|---|
| `Customer` | Catalog, Cart, Checkout, Orders, Account |
| `Admin` | Everything above + `/Admin` area (Categories, Products, Orders management) |

The `Admin` role is seeded automatically on first run.

---

## Email Templates

Located in `Templates/`:

| Template | Trigger |
|---|---|
| `ConfirmEmail.html` | After registration — confirms the account |
| `ResetPasswordEmail.html` | After "Forgot Password" request |
| `WelcomeEmail.html` | Available for welcome messaging |

Templates use `{{name}}` and `{{action_url}}` as placeholders, replaced at send time by `EmailBodyBuilder`.

---

## Database Schema

```
ApplicationUser  ──< Order       ──< OrderItem >── Product
                 ──< CartItem    >── Product
Product          >── Category
```

---

## Screenshots

> _Add screenshots of the storefront, cart, checkout, admin panel, and email confirmation pages here._

---

## License

This project was built as part of an ITI (Information Technology Institute) training program.
