using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private const string ImageSubfolder = "products";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public AdminCategoryService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<IList<AdminCategoryVM>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetQueryable()
                .Include(c => c.Products)
                .Where(c => !c.IsDeleted)
                .ToListAsync();

            return categories.Select(c => new AdminCategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCount = c.Products?.Count ?? 0
            }).ToList();
        }

        public async Task<AdminCategoryVM?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetQueryable()
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (category == null) return null;

            return new AdminCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task CreateAsync(AdminCategoryVM model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AdminCategoryVM model)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.Id);
            if (category == null) return false;

            category.Name = model.Name;
            category.Description = model.Description;
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetQueryable()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null) return false;

            // Block deletion if category still contains active products
            if (category.Products.Any(p => !p.IsDeleted))
                return false;

            category.IsDeleted = true;
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _unitOfWork.Categories.AnyAsync(
                c => c.Name == name && !c.IsDeleted && (excludeId == null || c.Id != excludeId));
        }
    }
}
