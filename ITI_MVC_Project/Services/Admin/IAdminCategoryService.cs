using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services.Admin
{
    public interface IAdminCategoryService
    {
        Task<IList<AdminCategoryVM>> GetAllAsync();
        Task<AdminCategoryVM?> GetByIdAsync(int id);
        Task CreateAsync(AdminCategoryVM model);
        Task<bool> UpdateAsync(AdminCategoryVM model);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
