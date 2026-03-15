using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services.Admin
{
    public class AdminProductService : IAdminProductService
    {
        private const string ImageSubfolder = "products";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public AdminProductService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<IList<AdminProductVM>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetQueryable()
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
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
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

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
                Categories = await _unitOfWork.Categories.GetQueryable()
                    .Where(c => !c.IsDeleted)
                    .ToListAsync()
            };
        }

        public async Task<AdminProductVM> GetCreateFormAsync()
        {
            return new AdminProductVM
            {
                Categories = await _unitOfWork.Categories.GetQueryable()
                    .Where(c => !c.IsDeleted)
                    .ToListAsync()
            };
        }

        public async Task CreateAsync(AdminProductVM model)
        {
            string? imageUrl = null;
            if (model.ImageFile is not null)
                imageUrl = await _fileService.SaveFileAsync(model.ImageFile, ImageSubfolder);

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = imageUrl,
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

            if (model.ImageFile is not null)
            {
                if (!string.IsNullOrWhiteSpace(product.ImageUrl))
                    _fileService.DeleteFile(product.ImageUrl, ImageSubfolder);

                product.ImageUrl = await _fileService.SaveFileAsync(model.ImageFile, ImageSubfolder);
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Stock = model.Stock;
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

            product.IsDeleted = true;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _unitOfWork.Products.AnyAsync(
                p => p.Name == name && !p.IsDeleted && (excludeId == null || p.Id != excludeId));
        }
    }
}
