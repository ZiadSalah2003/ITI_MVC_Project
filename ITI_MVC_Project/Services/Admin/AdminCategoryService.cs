using Microsoft.EntityFrameworkCore;
using ITI_MVC_Project.Models.Entities;
using ITI_MVC_Project.Models.ViewModels;
using ITI_MVC_Project.Repositories;

namespace ITI_MVC_Project.Services.Admin
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminCategoryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IList<AdminCategoryVM>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetQueryable()
                .Include(c => c.Products)
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
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
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
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
