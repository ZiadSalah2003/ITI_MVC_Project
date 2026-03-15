using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface ICatalogService
    {
        Task<ProductListVM> GetProductListAsync(int? categoryId, string? search, string sortBy, int page);
        Task<ProductDetailsVM?> GetProductDetailsAsync(int id);
    }
}
