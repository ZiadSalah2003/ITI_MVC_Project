using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CatalogService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<ProductListVM> GetProductListAsync(int? categoryId, string? search, string sortBy, int page)
        {
            const int pageSize = 8;
            var query = _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search) ||
                    (p.Description != null && p.Description.Contains(search)));

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "newest" => query.OrderByDescending(p => p.Id),
                _ => query.OrderBy(p => p.Name)
            };

            var totalItems = await query.CountAsync();
            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new ProductListVM
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
        }

        public async Task<ProductDetailsVM?> GetProductDetailsAsync(int id)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return null;

            return new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryName = product.Category?.Name ?? ""
            };
        }
    }
}
