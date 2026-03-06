using Microsoft.AspNetCore.Mvc;
using ITI_MVC_Project.Repositories;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ITI_MVC_Project.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CatalogController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IActionResult> Index(int? categoryId, string? search,
            string sortBy = "name", int page = 1)
        {
            const int pageSize = 8;
            var query = _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.Id),
                _ => query.OrderBy(p => p.Name)
            };

            var totalItems = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var vm = new ProductListVM
            {
                Products = products.Select(p => new ProductSummaryVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category?.Name ?? ""
                }),
                Categories = (await _unitOfWork.Categories.GetAllAsync()).ToList(),
                SelectedCategoryId = categoryId,
                SearchTerm = search,
                SortBy = sortBy,
                Pagination = new PaginationVM
                {
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                    TotalItems = totalItems
                }
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return NotFound();

            var vm = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.Name ?? ""
            };
            return View(vm);
        }
    }
}