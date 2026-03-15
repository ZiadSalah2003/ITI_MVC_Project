using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services.Admin
{
    public class AdminProductService : IAdminProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminProductService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IList<AdminProductVM>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return products.Select(p => new AdminProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                Categories = null
            }).ToList();
        }

        public async Task<AdminProductVM?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new AdminProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                Categories = await _unitOfWork.Categories.GetAllAsync()
            };
        }

        public async Task<AdminProductVM> GetCreateFormAsync()
        {
            return new AdminProductVM
            {
                Categories = await _unitOfWork.Categories.GetAllAsync()
            };
        }

        public async Task CreateAsync(AdminProductVM model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                CategoryId = model.CategoryId
            };
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AdminProductVM model)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(model.Id);
            if (product == null) return false;

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.ImageUrl = model.ImageUrl;
            product.IsActive = model.IsActive;
            product.CategoryId = model.CategoryId;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
