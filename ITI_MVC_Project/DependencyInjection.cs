using ITI_MVC_Project.Data;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Services;
using ITI_MVC_Project.Services.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// MVC
			services.AddControllersWithViews();

			// EF Core + SQL Server
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			// ASP.NET Core Identity
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 6;
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

			// Cookie settings
			services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/Account/Login";
				options.LogoutPath = "/Account/Logout";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.ExpireTimeSpan = TimeSpan.FromHours(2);
			});

			// Session (for cart fallback)
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			// Repositories
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Services
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICartService, CartService>();
			services.AddScoped<ICatalogService, CatalogService>();
			services.AddScoped<ICheckoutService, CheckoutService>();
			services.AddScoped<IOrderService, OrderService>();

			// Admin Services
			services.AddScoped<IAdminCategoryService, AdminCategoryService>();
			services.AddScoped<IAdminProductService, AdminProductService>();
			services.AddScoped<IAdminOrderService, AdminOrderService>();

			return services;
		}
	}
}
