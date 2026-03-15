using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services.Admin
{
    public interface IAdminProductService
    {
        Task<IList<AdminProductVM>> GetAllAsync();
        Task<AdminProductVM?> GetByIdAsync(int id);
        Task<AdminProductVM> GetCreateFormAsync();
        Task CreateAsync(AdminProductVM model);
        Task<bool> UpdateAsync(AdminProductVM model);
        Task<bool> DeleteAsync(int id);
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}
