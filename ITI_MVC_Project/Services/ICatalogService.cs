using ITI_MVC_Project.Models.ViewModels;

namespace ITI_MVC_Project.Services
{
    public interface ICatalogService
    {
        Task<ProductListVM> GetProductsAsync(int? categoryId, string? search, string sortBy, int page, int pageSize = 8);
        Task<ProductDetailsVM?> GetProductDetailsAsync(int id);
    }
}